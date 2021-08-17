using System.Collections.Generic;

namespace InfinityEngine.Graphics.RHI
{
    public abstract class FRHIResourceCache<Type> where Type : class
    {
        protected Dictionary<int, List<Type>> m_ResourcePool = new Dictionary<int, List<Type>>(64);
        abstract protected void ReleaseInternalResource(Type res);
        abstract protected string GetResourceName(Type res);
        abstract protected string GetResourceTypeName();

        public bool Pull(in int hashCode, out Type resource)
        {
            if (m_ResourcePool.TryGetValue(hashCode, out var list) && list.Count > 0)
            {
                /*resource = list[0];
                list.RemoveAt(0);*/
                resource = list[list.Count - 1];
                list.RemoveAt(list.Count - 1);
                return true;
            }

            resource = null;
            return false;
        }

        public void Push(in int hash, Type resource)
        {
            if (!m_ResourcePool.TryGetValue(hash, out var list))
            {
                list = new List<Type>();
                m_ResourcePool.Add(hash, list);
            }

            list.Add(resource);
        }

        public void Dispose()
        {
            foreach (var kvp in m_ResourcePool)
            {
                foreach (Type resource in kvp.Value)
                {
                    ReleaseInternalResource(resource);
                }
            }
        }
    }

    public class FRHIBufferCache : FRHIResourceCache<FRHIBuffer>
    {
        protected override void ReleaseInternalResource(FRHIBuffer rhiBuffer)
        {
            rhiBuffer.Dispose();
        }

        protected override string GetResourceName(FRHIBuffer rhiBuffer)
        {
            return rhiBuffer.name;
        }

        override protected string GetResourceTypeName()
        {
            return "Buffer";
        }
    }

    public class FRHITextureCache : FRHIResourceCache<FRHITexture>
    {
        protected override void ReleaseInternalResource(FRHITexture rhiTexture)
        {
            rhiTexture.Dispose();
        }

        protected override string GetResourceName(FRHITexture rhiTexture)
        {
            return rhiTexture.name;
        }

        override protected string GetResourceTypeName()
        {
            return "Texture";
        }
    }

    public class FRHIResourcePool
    {
        FRHIBufferCache m_BufferPool;
        FRHITextureCache m_TexturePool;

        public FRHIResourcePool()
        {
            m_BufferPool = new FRHIBufferCache();
            m_TexturePool = new FRHITextureCache();
        }

        public FRHIBufferRef AllocateBuffer(in FRHIBufferDescription description)
        {
            FRHIBuffer buffer;
            int handle = description.GetHashCode();

            if (!m_BufferPool.Pull(handle, out buffer))
            {
                //buffer = new ComputeBuffer(description.count, description.stride, description.type);
                buffer.name = description.name;
            }

            return new FRHIBufferRef(handle, buffer);
        }

        public void ReleaseBuffer(in FRHIBufferRef bufferRef)
        {
            m_BufferPool.Push(bufferRef.handle, bufferRef.buffer);
        }

        public FRHITextureRef AllocateTexture(in FRHITextureDescription description)
        {
            FRHITexture texture;
            int handle = description.GetHashCode();

            if (!m_TexturePool.Pull(handle, out texture))
            {
                //texture = RTHandles.Alloc(description.width, description.height, description.slices, (DepthBits)description.depthBufferBits, description.colorFormat, description.filterMode, description.wrapMode, description.dimension, description.enableRandomWrite,
                                          //description.useMipMap, description.autoGenerateMips, description.isShadowMap, description.anisoLevel, description.mipMapBias, (MSAASamples)description.msaaSamples, description.bindTextureMS, false, RenderTextureMemoryless.None, description.name);
            }

            return new FRHITextureRef(handle, texture);
        }

        public void ReleaseTexture(in FRHITextureRef textureRef)
        {
            m_TexturePool.Push(textureRef.handle, textureRef.texture);
        }

        public void Dispose()
        {
            m_BufferPool.Dispose();
            m_TexturePool.Dispose();
        }
    }
}
