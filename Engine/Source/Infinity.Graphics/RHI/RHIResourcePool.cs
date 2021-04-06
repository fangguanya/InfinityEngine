using System.Collections.Generic;

namespace InfinityEngine.Graphics.RHI
{
    public abstract class FRHIResourcePool<Type> where Type : class
    {
        protected Dictionary<int, List<Type>> m_ResourcePool = new Dictionary<int, List<Type>>(64);

        abstract protected void ReleaseInternalResource(Type res);
        abstract protected string GetResourceName(Type res);
        abstract protected string GetResourceTypeName();

        public bool Pull(int hashCode, out Type resource)
        {
            if (m_ResourcePool.TryGetValue(hashCode, out var list) && list.Count > 0)
            {
                resource = list[list.Count - 1];
                list.RemoveAt(list.Count - 1);
                return true;
            }

            resource = null;
            return false;
        }

        public void Push(int hash, Type resource)
        {
            if (!m_ResourcePool.TryGetValue(hash, out var list))
            {
                list = new List<Type>();
                m_ResourcePool.Add(hash, list);
            }

            list.Add(resource);
        }

        public void Disposed()
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

    public class FRHIBufferPool : FRHIResourcePool<FRHIBuffer>
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

    public class FRHITexturePool : FRHIResourcePool<FRHITexture>
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
}
