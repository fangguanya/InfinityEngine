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

        internal FRHICommandContext(ID3D12Device6 d3d12Device, CommandListType cmdListType) : base()
        {
            m_Fence = new FRHIFence(d3d12Device);
            m_FenceEvent = new AutoResetEvent(false);

            CommandQueueDescription queueDescription = new CommandQueueDescription();
            queueDescription.Type = cmdListType;
            queueDescription.Flags = CommandQueueFlags.None;
            d3dCmdQueue = d3d12Device.CreateCommandQueue<ID3D12CommandQueue>(queueDescription);
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
