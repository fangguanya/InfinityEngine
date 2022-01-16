using System.Threading;
using InfinityEngine.Core.Object;

namespace InfinityEngine.Graphics.RHI
{
    internal abstract class FRHICommandContext : FDisposal
    {
        EContextType contextType;
        protected AutoResetEvent m_FenceEvent;

        internal FRHICommandContext(FRHIDevice device, EContextType contextType, string name) 
        { 
            this.contextType = contextType;
        }

        public abstract void SignalQueue(FRHIFence fence);
        public abstract void WaitQueue(FRHIFence fence);
        public abstract void ExecuteQueue(FRHICommandBuffer cmdBuffer);
        public abstract void Flush();
    }
}
