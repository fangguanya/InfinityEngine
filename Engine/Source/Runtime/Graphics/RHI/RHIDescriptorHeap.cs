using InfinityEngine.Core.Object;

namespace InfinityEngine.Graphics.RHI
{
    internal enum EDescriptorType
    {
        DSV = 0,
        RTV = 1,
        CbvSrvUav = 2,
        Sampler = 3
    };

    internal abstract class FRHIDescriptorHeapFactory : FDisposal
    {
        protected EDescriptorType m_Type;

        public FRHIDescriptorHeapFactory(FRHIDevice device, in EDescriptorType type, in uint count, string name)
        {
            m_Type = type;
        }

        public abstract int Allocate();
        public abstract int Allocate(in int count);
        public abstract void Free(in int index);
    }
}
