using System.Threading;
using Vortice.Direct3D12;
using InfinityEngine.Core.Object;

namespace InfinityEngine.Graphics.RHI
{
    internal class FRHICommandContext : FDisposable
    {
        protected AutoResetEvent m_FenceEvent;

        internal FRHICommandContext(FRHIDevice device, EContextType contextType) : base()
        {

        }

        public virtual void SignalQueue(FRHIFence fence) { }

        public virtual void WaitQueue(FRHIFence fence) { }

        public virtual void ExecuteQueue(FRHICommandList cmdList) { }

        public virtual void Flush() { }
    }
}
