using InfinityEngine.Core.Object;

namespace InfinityEngine.Graphics.RHI
{
    public class FD3DMemoryReadback : FDisposable
    {
        public bool IsReady { get { return m_Fence.IsCompleted; } }

        private FRHIFence m_Fence;

        internal FD3DMemoryReadback(FRHIDevice device) : base()
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
