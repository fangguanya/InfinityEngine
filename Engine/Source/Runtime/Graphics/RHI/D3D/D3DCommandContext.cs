using System.Threading;
using Vortice.Direct3D12;

namespace InfinityEngine.Graphics.RHI.D3D
{
    internal class FD3DCommandContext : FRHICommandContext
    {
        private FD3DFence m_Fence;
        private ID3D12CommandQueue m_NativeCmdQueue;

        internal ID3D12CommandQueue nativeCmdQueue
        {
            get
            {
                return m_NativeCmdQueue;
            }
        }

        internal FD3DCommandContext(FRHIDevice device, EContextType contextType) : base(device, contextType)
        {
            m_Fence = new FD3DFence(device);
            m_FenceEvent = new AutoResetEvent(false);

            CommandQueueDescription queueDescription = new CommandQueueDescription();
            queueDescription.Type = (CommandListType)contextType;
            queueDescription.Flags = CommandQueueFlags.None;
            FD3DDevice d3dDevice = (FD3DDevice)device;

            m_NativeCmdQueue = d3dDevice.nativeDevice.CreateCommandQueue<ID3D12CommandQueue>(queueDescription);
        }

        public static implicit operator ID3D12CommandQueue(FD3DCommandContext cmdContext) { return cmdContext.m_NativeCmdQueue; }

        public override void SignalQueue(FRHIFence fence)
        {
            fence.Signal(this);
        }

        public override void WaitQueue(FRHIFence fence)
        {
            fence.WaitOnGPU(this);
        }

        public override void ExecuteQueue(FRHICommandBuffer cmdBuffer)
        {
            FD3DCommandBuffer d3dCmdBuffer = (FD3DCommandBuffer)cmdBuffer;

            d3dCmdBuffer.Close();
            m_NativeCmdQueue.ExecuteCommandList(d3dCmdBuffer);
        }

        public override void Flush()
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
