using Vortice.Direct3D12;
using System.Collections.Generic;
using InfinityEngine.Core.Object;

namespace InfinityEngine.Graphics.RHI
{
    internal abstract class FRHIResourceCache<Type> where Type : class
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

    internal class FRHIBufferCache : FRHIResourceCache<FRHIBuffer>
    {
        protected override void ReleaseInternalResource(FRHIBuffer buffer)
        {
            buffer.Dispose();
        }

        protected override string GetResourceName(FRHIBuffer buffer)
        {
            return buffer.description.name;
        }

        override protected string GetResourceTypeName()
        {
            return "Buffer";
        }
    }

    internal class FRHITextureCache : FRHIResourceCache<FRHITexture>
    {
        protected override void ReleaseInternalResource(FRHITexture texture)
        {
            texture.Dispose();
        }

        protected override string GetResourceName(FRHITexture texture)
        {
            return texture.description.name;
        }

        override protected string GetResourceTypeName()
        {
            return "Texture";
        }
    }

    public class FRHIResourcePool
    {
        FRHIDevice m_Device;
        FRHIBufferCache m_BufferPool;
        FRHITextureCache m_TexturePool;

        internal FRHIResourcePool(FRHIDevice device)
        {
            m_Device = device;
            m_BufferPool = new FRHIBufferCache();
            m_TexturePool = new FRHITextureCache();
        }

        public FRHIBufferRef GetBuffer(in FRHIBufferDescription description)
        {
            FRHIBuffer buffer;
            int handle = description.GetHashCode();

            if (!m_BufferPool.Pull(handle, out buffer))
            {
                buffer = new FRHIBuffer(m_Device, description);
            }

            return new FRHIBufferRef(handle, buffer);
        }

        public void ReleaseBuffer(in FRHIBufferRef bufferRef)
        {
            m_BufferPool.Push(bufferRef.handle, bufferRef.buffer);
        }

        public FRHITextureRef GetTexture(in FRHITextureDescription description)
        {
            FRHITexture texture;
            int handle = description.GetHashCode();

            if (!m_TexturePool.Pull(handle, out texture))
            {
                texture = new FRHITexture(m_Device, description);
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

    internal struct FRHICommandListPooledObject : IDisposable
    {
        readonly FRHICommandList m_ToReturn;
        readonly FRHICommandListPool m_Pool;

        internal FRHICommandListPooledObject(FRHICommandList value, FRHICommandListPool pool)
        {
            m_Pool = pool;
            m_ToReturn = value;
        }

        void IDisposable.Dispose() => m_Pool.ReleaseTemporary(m_ToReturn);
    }

    internal class FRHICommandListPool : FDisposable
    {
        private FRHIDevice m_Device;
        private EContextType m_ContextType;
        readonly bool m_CollectionCheck = true;
        readonly Stack<FRHICommandList> m_Stack = new Stack<FRHICommandList>();
        public int countAll { get; private set; }
        public int countActive { get { return countAll - countInactive; } }
        public int countInactive { get { return m_Stack.Count; } }

        internal FRHICommandListPool(FRHIDevice device, EContextType contextType, bool collectionCheck = true)
        {
            m_Device = device;
            m_ContextType = contextType;
            m_CollectionCheck = collectionCheck;
        }

        public FRHICommandList GetTemporary(string name)
        {
            FRHICommandList element;
            if (m_Stack.Count == 0)
            {
                element = new FRHICommandList(m_Device.d3dDevice, m_ContextType);
                countAll++;
            } else {
                element = m_Stack.Pop();
            }
            element.name = name;
            return element;
        }

        public void ReleaseTemporary(FRHICommandList element)
        {
#if WITH_EDITOR // keep heavy checks in editor
            if (m_CollectionCheck && m_Stack.Count > 0)
            {
                if (m_Stack.Contains(element))
                    Console.WriteLine("Internal error. Trying to destroy object that is already released to pool.");
            }
#endif
            m_Stack.Push(element);
        }

        protected override void Release()
        {
            m_Device = null;
            foreach (FRHICommandList cmdList in m_Stack)
            {
                cmdList.Dispose();
            }
        }
    }
}
