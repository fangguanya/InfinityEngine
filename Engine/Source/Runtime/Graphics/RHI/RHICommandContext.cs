using System.Threading;
using InfinityEngine.Core.Object;

namespace InfinityEngine.Graphics.RHI
{
    internal class FRHICommandContext : FDisposal
    {
        protected AutoResetEvent m_FenceEvent;

        internal FRHICommandContext(FRHIDevice device, EContextType contextType) : base() { }

        public virtual void SignalQueue(FRHIFence fence) { }
        public virtual void WaitQueue(FRHIFence fence) { }
        public virtual void ExecuteQueue(FRHICommandBuffer cmdBuffer) { }
        public virtual void Flush() { }
    }
}
