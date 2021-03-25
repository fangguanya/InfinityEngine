using System.Threading;
using Vortice.Direct3D12;
using InfinityEngine.Core.Object;

namespace InfinityEngine.Graphics.RHI
{
    public class FRHICommandContext : UObject
    {
        public FRHIFence FrameFence;
        public ManualResetEvent FenceEvent;

        public ID3D12CommandQueue NativeCmdQueue;


        public FRHICommandContext(ID3D12Device6 NativeDevice, CommandListType CommandBufferType) : base()
        {
            FenceEvent = new ManualResetEvent(false);
            FrameFence = new FRHIFence(NativeDevice);

            CommandQueueDescription CmdQueueDescription = new CommandQueueDescription();
            CmdQueueDescription.Type = CommandBufferType;
            CmdQueueDescription.Flags = CommandQueueFlags.None;
            NativeCmdQueue = NativeDevice.CreateCommandQueue<ID3D12CommandQueue>(CmdQueueDescription);
        }

        public static implicit operator ID3D12CommandQueue(FRHICommandContext RHICmdContext) { return RHICmdContext.NativeCmdQueue; }

        public void SignalQueue(FRHIFence GPUFence)
        {
            GPUFence.Signal(NativeCmdQueue);
        }

        public void WaitQueue(FRHIFence GPUFence)
        {
            GPUFence.WaitOnGPU(NativeCmdQueue);
        }

        public void ExecuteQueue(FRHICommandList rhiCmdList)
        {
            rhiCmdList.Close();
            NativeCmdQueue.ExecuteCommandList(rhiCmdList);
        }

        public void Flush()
        {
            FrameFence.Signal(NativeCmdQueue);
            FrameFence.WaitOnCPU(FenceEvent);
        }

        protected override void Disposed()
        {
            FrameFence?.Dispose();
            FenceEvent?.Dispose();
            NativeCmdQueue?.Dispose();
        }
    }
}
