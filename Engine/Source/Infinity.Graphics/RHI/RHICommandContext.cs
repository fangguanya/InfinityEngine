using System.Threading;
using Vortice.Direct3D12;
using InfinityEngine.Core.Object;

namespace InfinityEngine.Graphics.RHI
{
    public class FRHICommandContext : FDisposer
    {
        public FRHIFence fence;
        public AutoResetEvent fenceEvent;
        public ID3D12CommandQueue d3D12CmdQueue;

        public FRHICommandContext(ID3D12Device6 d3D12Device, CommandListType cmdListType) : base()
        {
            fence = new FRHIFence(d3D12Device);
            fenceEvent = new AutoResetEvent(false);

            CommandQueueDescription CmdQueueDescription = new CommandQueueDescription();
            CmdQueueDescription.Type = cmdListType;
            CmdQueueDescription.Flags = CommandQueueFlags.None;
            d3D12CmdQueue = d3D12Device.CreateCommandQueue<ID3D12CommandQueue>(CmdQueueDescription);
        }

        public static implicit operator ID3D12CommandQueue(FRHICommandContext RHICmdContext) { return RHICmdContext.d3D12CmdQueue; }

        public void SignalQueue(FRHIFence fence)
        {
            fence.Signal(d3D12CmdQueue);
        }

        public void WaitQueue(FRHIFence fence)
        {
            fence.WaitOnGPU(d3D12CmdQueue);
        }

        public void ExecuteQueue(FRHICommandList cmdList)
        {
            cmdList.Close();
            d3D12CmdQueue.ExecuteCommandList(cmdList);
        }

        public void Flush()
        {
            fence.Signal(d3D12CmdQueue);
            fence.WaitOnCPU(fenceEvent);
        }

        protected override void Disposed()
        {
            fence?.Dispose();
            fenceEvent?.Dispose();
            d3D12CmdQueue?.Dispose();
        }
    }
}
