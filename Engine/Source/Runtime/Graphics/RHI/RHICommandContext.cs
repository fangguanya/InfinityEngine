using System.Threading;
using Vortice.Direct3D12;
using InfinityEngine.Core.Object;

namespace InfinityEngine.Graphics.RHI
{
    internal class FRHICommandContext : FDisposable
    {
        private FRHIFence m_Fence;
        private AutoResetEvent m_FenceEvent;
        private ID3D12CommandQueue m_NativeCmdQueue;

        internal ID3D12CommandQueue nativeCmdQueue
        {
            get
            {
                return m_NativeCmdQueue;
            }
        }

        internal FRHICommandContext(FRHIDevice device, EContextType contextType) : base()
        {
            m_Fence = new FRHIFence(device);
            m_FenceEvent = new AutoResetEvent(false);

            CommandQueueDescription queueDescription = new CommandQueueDescription();
            queueDescription.Type = (CommandListType)contextType;
            queueDescription.Flags = CommandQueueFlags.None;
            m_NativeCmdQueue = device.nativeDevice.CreateCommandQueue<ID3D12CommandQueue>(queueDescription);
        }

        public static implicit operator ID3D12CommandQueue(FRHICommandContext cmdContext) { return cmdContext.m_NativeCmdQueue; }

        public void SignalQueue(FRHIFence fence)
        {
            fence.Signal(this);
        }

        public void WaitQueue(FRHIFence fence)
        {
            fence.WaitOnGPU(this);
        }

        public void ExecuteQueue(FRHICommandList cmdList)
        {
            cmdList.Close();
            m_NativeCmdQueue.ExecuteCommandList(cmdList);
        }

        public void Flush()
        {
            m_Fence.Signal(this);
            m_Fence.WaitOnCPU(m_FenceEvent);
        }

        protected override void Release()
        {
            m_Fence?.Dispose();
            m_FenceEvent?.Dispose();
            m_NativeCmdQueue?.Dispose();
        }
    }
}
