using Vortice.Direct3D12;
using InfinityEngine.Core.Object;

namespace InfinityEngine.Graphics.RHI
{
    public class FRHIMemoryReadback : FDisposer
    {
        public bool bReady { get { return m_Fence.Completed(); } }

        private FRHIFence m_Fence;

        internal FRHIMemoryReadback(ID3D12Device6 d3dDevice) : base()
        {
            m_Fence = new FRHIFence(d3dDevice);
        }

        protected virtual void RequestReadback(FRHIGraphicsContext graphicsContext, FRHICommandList cmdList, FRHIBuffer buffer) { }

        protected virtual void RequestReadback() { }

        protected override void Disposed()
        {
            m_Fence?.Dispose();
        }
    }
}
