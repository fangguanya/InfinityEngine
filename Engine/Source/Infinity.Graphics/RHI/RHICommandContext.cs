using System.Threading;
using Vortice.Direct3D12;
using InfinityEngine.Core.Object;

namespace InfinityEngine.Graphics.RHI
{
    public class FRHICommandContext : UObject
    {
        public FRHIFence rhiFence;
        public ManualResetEvent fenceEvent;
        public ID3D12CommandQueue d3D12CmdQueue;


        public FRHICommandContext(ID3D12Device6 d3D12Device, CommandListType cmdListType) : base()
        {
            rhiFence = new FRHIFence(d3D12Device);
            fenceEvent = new ManualResetEvent(false);

            CommandQueueDescription CmdQueueDescription = new CommandQueueDescription();
            CmdQueueDescription.Type = cmdListType;
            CmdQueueDescription.Flags = CommandQueueFlags.None;
            d3D12CmdQueue = d3D12Device.CreateCommandQueue<ID3D12CommandQueue>(CmdQueueDescription);
        }

        public static implicit operator ID3D12CommandQueue(FRHICommandContext RHICmdContext) { return RHICmdContext.d3D12CmdQueue; }

        public void SignalQueue(FRHIFence rhiFence)
        {
            rhiFence.Signal(d3D12CmdQueue);
        }

        public void WaitQueue(FRHIFence rhiFence)
        {
            rhiFence.WaitOnGPU(d3D12CmdQueue);
        }

        public void ExecuteQueue(FRHICommandList rhiCmdList)
        {
            rhiCmdList.Close();
            d3D12CmdQueue.ExecuteCommandList(rhiCmdList);
        }

        public void Flush()
        {
            rhiFence.Signal(d3D12CmdQueue);
            rhiFence.WaitOnCPU(fenceEvent);
        }

        protected override void Disposed()
        {
            rhiFence?.Dispose();
            fenceEvent?.Dispose();
            d3D12CmdQueue?.Dispose();
        }
    }
}
