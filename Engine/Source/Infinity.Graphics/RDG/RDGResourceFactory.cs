using System;
using InfinityEngine.Graphics.RHI;
using InfinityEngine.Core.Container;

namespace InfinityEngine.Graphics.RDG
{
    internal class FRDGResourceFactory
    {
        static FRDGResourceFactory m_CurrentRegistry;
        internal static FRDGResourceFactory current
        {
            get
            {
                return m_CurrentRegistry;
            }
            set
            {
                m_CurrentRegistry = value;
            }
        }

        FRHIBufferPool m_BufferPool = new FRHIBufferPool();
        FRHITexturePool m_TexturePool = new FRHITexturePool();
        TArray<IRDGResource>[] m_Resources = new TArray<IRDGResource>[2];


        internal FRDGResourceFactory()
        {
            for (int i = 0; i < 2; ++i)
                m_Resources[i] = new TArray<IRDGResource>();
        }

        internal FRHIBuffer GetBuffer(in FRDGBufferRef bufferRef)
        {
            return GetBufferResource(bufferRef.handle).resource;
        }

        internal FRHITexture GetTexture(in FRDGTextureRef textureRef)
        {
            return GetTextureResource(textureRef.handle).resource;
        }

        #region Internal Interface
        ResType GetResource<DescType, ResType>(TArray<IRDGResource> resourceArray, int index) where DescType : struct where ResType : class
        {
            var rdgResource = resourceArray[index] as FRDGResource<DescType, ResType>;
            return rdgResource.resource;
        }

        internal void BeginRender()
        {
            current = this;
        }

        internal void EndRender()
        {
            current = null;
        }

        internal string GetResourceName(in FRDGResourceRef resourceRef)
        {
            return m_Resources[resourceRef.iType][resourceRef.index].GetName();
        }

        internal bool IsResourceImported(in FRDGResourceRef resourceRef)
        {
            return m_Resources[resourceRef.iType][resourceRef.index].imported;
        }

        internal int GetResourceTemporalIndex(in FRDGResourceRef resourceRef)
        {
            return m_Resources[resourceRef.iType][resourceRef.index].temporalPassIndex;
        }

        int AddNewResource<ResType>(TArray<IRDGResource> resourceArray, out ResType outRes) where ResType : IRDGResource, new()
        {
            int result = resourceArray.length;
            resourceArray.Resize(resourceArray.length + 1, true);
            if (resourceArray[result] == null)
                resourceArray[result] = new ResType();

            outRes = resourceArray[result] as ResType;
            outRes.Reset();
            return result;
        }

        internal FRDGTextureRef ImportTexture(FRHITexture rhiTexture, int shaderProperty = 0)
        {
            int newHandle = AddNewResource(m_Resources[(int)EResourceType.Texture], out FRDGTexture rdgTexture);
            rdgTexture.resource = rhiTexture;
            rdgTexture.imported = true;
            rdgTexture.shaderProperty = shaderProperty;

            return new FRDGTextureRef(newHandle);
        }

        internal FRDGTextureRef CreateTexture(in FRHITextureDescription textureDescription, int shaderProperty = 0, int temporalPassIndex = -1)
        {
            int newHandle = AddNewResource(m_Resources[(int)EResourceType.Texture], out FRDGTexture rdgTexture);
            rdgTexture.desc = textureDescription;
            rdgTexture.shaderProperty = shaderProperty;
            rdgTexture.temporalPassIndex = temporalPassIndex;
            return new FRDGTextureRef(newHandle);
        }

        internal int GetTextureResourceCount()
        {
            return m_Resources[(int)EResourceType.Texture].length;
        }

        FRDGTexture GetTextureResource(in FRDGResourceRef resourceRef)
        {
            return m_Resources[(int)EResourceType.Texture][resourceRef] as FRDGTexture;
        }

        internal FRHITextureDescription GetTextureResourceDesc(in FRDGResourceRef resourceRef)
        {
            return (m_Resources[(int)EResourceType.Texture][resourceRef] as FRDGTexture).desc;
        }

        internal FRDGBufferRef ImportBuffer(FRHIBuffer rhiBuffer)
        {
            int newHandle = AddNewResource(m_Resources[(int)EResourceType.Buffer], out FRDGBuffer rdgBuffer);
            rdgBuffer.resource = rhiBuffer;
            rdgBuffer.imported = true;

            return new FRDGBufferRef(newHandle);
        }

        internal FRDGBufferRef CreateBuffer(in FRHIBufferDescription bufferDescription, int temporalPassIndex = -1)
        {
            int newHandle = AddNewResource(m_Resources[(int)EResourceType.Buffer], out FRDGBuffer bufferResource);
            bufferResource.desc = bufferDescription;
            bufferResource.temporalPassIndex = temporalPassIndex;

            return new FRDGBufferRef(newHandle);
        }

        internal int GetBufferResourceCount()
        {
            return m_Resources[(int)EResourceType.Buffer].length;
        }

        FRDGBuffer GetBufferResource(in FRDGResourceRef resourceRef)
        {
            return m_Resources[(int)EResourceType.Buffer][resourceRef] as FRDGBuffer;
        }


        internal void CreateRealBuffer(int index)
        {
            var resource = m_Resources[(int)EResourceType.Buffer][index] as FRDGBuffer;
            if (!resource.imported)
            {
                var desc = resource.desc;
                int hashCode = desc.GetHashCode();

                if (resource.resource != null)
                    throw new InvalidOperationException(string.Format("Trying to create an already created Compute Buffer ({0}). Buffer was probably declared for writing more than once in the same pass.", resource.desc.name));

                resource.resource = null;
                if (!m_BufferPool.Pull(hashCode, out resource.resource))
                {
                    //resource.resource = new ComputeBuffer(resource.desc.count, resource.desc.stride, resource.desc.type);
                }
                resource.cachedHash = hashCode;
            }
        }

        internal void ReleaseRealBuffer(int index)
        {
            var resource = m_Resources[(int)EResourceType.Buffer][index] as FRDGBuffer;

            if (!resource.imported)
            {
                if (resource.resource == null)
                    throw new InvalidOperationException($"Tried to release a compute buffer ({resource.desc.name}) that was never created. Check that there is at least one pass writing to it first.");

                m_BufferPool.Push(resource.cachedHash, resource.resource);
                resource.cachedHash = -1;
                resource.resource = null;
                resource.wasReleased = true;
            }
        }

        internal void CreateRealTexture(int index)
        {
            var resource = m_Resources[(int)EResourceType.Texture][index] as FRDGTexture;

            if (!resource.imported)
            {
                var desc = resource.desc;
                int hashCode = desc.GetHashCode();

                if (resource.resource != null)
                    throw new InvalidOperationException(string.Format("Trying to create an already created texture ({0}). Texture was probably declared for writing more than once in the same pass.", resource.desc.name));

                resource.resource = null;

                if (!m_TexturePool.Pull(hashCode, out resource.resource))
                {
                    //resource.resource = new Texture();
                }

                resource.cachedHash = hashCode;
            }
        }

        internal void ReleaseRealTexture(int index)
        {
            var resource = m_Resources[(int)EResourceType.Texture][index] as FRDGTexture;

            if (!resource.imported)
            {
                if (resource.resource == null)
                    throw new InvalidOperationException($"Tried to release a texture ({resource.desc.name}) that was never created. Check that there is at least one pass writing to it first.");

                m_TexturePool.Push(resource.cachedHash, resource.resource);
                resource.cachedHash = -1;
                resource.resource = null;
                resource.wasReleased = true;
            }
        }

        internal void Clear()
        {
            for (int i = 0; i < 2; ++i)
            {
                m_Resources[i].Clear();
            }
        }

        internal void Cleanup()
        {
            m_BufferPool.Cleanup();
            m_TexturePool.Cleanup();
        }

        #endregion
    }
}
