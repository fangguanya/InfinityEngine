using System.Threading;
using Vortice.Direct3D12;
using InfinityEngine.Core.Object;

namespace InfinityEngine.Graphics.RHI
{
    internal class FRHICommandContext : FDisposable
    {
        private FRHIFence m_Fence;
        private AutoResetEvent m_FenceEvent;
        internal ID3D12CommandQueue d3dCmdQueue;

        internal FRHICommandContext(FRHIDevice device, EContextType contextType) : base()
        {
            m_Fence = new FRHIFence(device);
            m_FenceEvent = new AutoResetEvent(false);

            CommandQueueDescription queueDescription = new CommandQueueDescription();
            queueDescription.Type = (CommandListType)contextType;
            queueDescription.Flags = CommandQueueFlags.None;
            d3dCmdQueue = device.d3dDevice.CreateCommandQueue<ID3D12CommandQueue>(queueDescription);
        }

        public static implicit operator ID3D12CommandQueue(FRHICommandContext cmdContext) { return cmdContext.d3dCmdQueue; }

        public void SignalQueue(FRHIFence fence)
        {
            fence.Signal(d3dCmdQueue);
        }

        public void WaitQueue(FRHIFence fence)
        {
            fence.WaitOnGPU(d3dCmdQueue);
        }

        public void ExecuteQueue(FRHICommandList cmdList)
        {
            cmdList.Close();
            d3dCmdQueue.ExecuteCommandList(cmdList);
        }

        public void Flush()
        {
            m_Fence.Signal(d3dCmdQueue);
            m_Fence.WaitOnCPU(m_FenceEvent);
        }

        protected override void Release()
        {
            m_Fence?.Dispose();
            d3dCmdQueue?.Dispose();
            m_FenceEvent?.Dispose();
        }
    }
}
