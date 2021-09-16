using Vortice.Direct3D12;
using InfinityEngine.Core.Object;

namespace InfinityEngine.Graphics.RHI
{
    public class FRHIMemoryReadback : FDisposable
    {
        public bool bReady { get { return m_Fence.Completed(); } }

        private FRHIFence m_Fence;

        internal FRHIMemoryReadback(FRHIDevice device) : base()
        {
            m_Fence = new FRHIFence(device);
        }

        protected virtual void RequestReadback(FRHIGraphicsContext graphicsContext, FRHICommandList cmdList, FRHIBuffer buffer) { }

        protected virtual void RequestReadback() { }

        protected override void Release()
        {
            m_Fence?.Dispose();
        }
    }
}
