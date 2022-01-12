using System;
using InfinityEngine.Core.Object;
using InfinityEngine.Graphics.RHI;
using InfinityEngine.Core.Container;

namespace InfinityEngine.Graphics.RDG
{
    public enum EDepthAccess
    {
        Read = 1 << 0,
        Write = 1 << 1,
        ReadWrite = Read | Write,
    }

    internal struct FRDGResourceRef
    {
        internal bool IsValid;
        public int index { get; private set; }
        public EResourceType type { get; private set; }
        public int iType { get { return (int)type; } }


        internal FRDGResourceRef(int value, EResourceType type)
        {
            this.type = type;
            this.index = value;
            this.IsValid = true;
        }

        public static implicit operator int(FRDGResourceRef handle) => handle.index;
    }

    public struct FRDGBufferRef
    {
        internal FRDGResourceRef handle;

        internal FRDGBufferRef(int handle) 
        { 
            this.handle = new FRDGResourceRef(handle, EResourceType.Buffer); 
        }

        public static implicit operator FRHIBuffer(FRDGBufferRef bufferRef) => bufferRef.handle.IsValid ? FRDGResourceFactory.current.GetBuffer(bufferRef) : null;
    }

    public struct FRDGTextureRef
    {
        internal FRDGResourceRef handle;

        internal FRDGTextureRef(int handle) 
        { 
            this.handle = new FRDGResourceRef(handle, EResourceType.Texture); 
        }

        public static implicit operator FRHITexture(FRDGTextureRef textureRef) => textureRef.handle.IsValid ? FRDGResourceFactory.current.GetTexture(textureRef) : null;
    }

    internal class IRDGResource
    {
        public bool imported;
        public bool wasReleased;
        public int cachedHash;
        public int shaderProperty;
        public int temporalPassIndex;

        public virtual void Reset()
        {
            imported = false;
            wasReleased = false;
            cachedHash = -1;
            shaderProperty = 0;
            temporalPassIndex = -1;
        }

        public virtual string GetName()
        {
            return "";
        }
    }

    internal class FRDGResource<DescType, ResType> : IRDGResource where DescType : struct where ResType : class
    {
        public DescType desc;
        public ResType resource;

        protected FRDGResource()
        {

        }

        public override void Reset()
        {
            base.Reset();
            resource = null;
        }
    }

    internal class FRDGBuffer : FRDGResource<FBufferDescriptor, FRHIBuffer>
    {
        public override string GetName()
        {
            return desc.name;
        }
    }

    internal class FRDGTexture : FRDGResource<FTextureDescriptor, FRHITexture>
    {
        public override string GetName()
        {
            return desc.name;
        }
    }

    internal class FRDGResourceFactory : FDisposal
    {
        static FRDGResourceFactory m_CurrentRegistry;
        internal static FRDGResourceFactory current
        {
            get
            {
                return m_CurrentRegistry;
            } set {
                m_CurrentRegistry = value;
            }
        }

        FRHIBufferCache m_BufferPool = new FRHIBufferCache();
        FRHITextureCache m_TexturePool = new FRHITextureCache();
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

        internal FRDGTextureRef ImportTexture(FRHITexture rhiTexture, in int shaderProperty = 0)
        {
            int newHandle = AddNewResource(m_Resources[(int)EResourceType.Texture], out FRDGTexture rdgTexture);
            rdgTexture.resource = rhiTexture;
            rdgTexture.imported = true;
            rdgTexture.shaderProperty = shaderProperty;

            return new FRDGTextureRef(newHandle);
        }

        internal FRDGTextureRef CreateTexture(in FTextureDescriptor textureDescriptor, in int shaderProperty = 0, in int temporalPassIndex = -1)
        {
            int newHandle = AddNewResource(m_Resources[(int)EResourceType.Texture], out FRDGTexture rdgTexture);
            rdgTexture.desc = textureDescriptor;
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

        internal FTextureDescriptor GetTextureDescriptor(in FRDGResourceRef resourceRef)
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

        internal FRDGBufferRef CreateBuffer(in FBufferDescriptor bufferDescriptor, in int temporalPassIndex = -1)
        {
            int newHandle = AddNewResource(m_Resources[(int)EResourceType.Buffer], out FRDGBuffer bufferResource);
            bufferResource.desc = bufferDescriptor;
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

        internal FBufferDescriptor GetBufferDescriptor(in FRDGResourceRef handle)
        {
            return (m_Resources[(int)EResourceType.Buffer][handle] as FRDGBuffer).desc;
        }

        internal void CreateRealBuffer(in int index)
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

        internal void ReleaseRealBuffer(in int index)
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

        internal void CreateRealTexture(in int index)
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

        internal void ReleaseRealTexture(in int index)
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

        internal void Release()
        {
            m_BufferPool.Dispose();
            m_TexturePool.Dispose();
        }
        #endregion
    }
}
