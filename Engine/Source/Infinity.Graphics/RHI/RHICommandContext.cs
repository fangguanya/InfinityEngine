using System.Threading;
using Vortice.Direct3D;
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
            NativeCmdQueue = NativeDevice.CreateCommandQueue(CommandBufferType);
        }

        public void SignalQueue(FRHIFence GPUFence)
        {
            GPUFence.Signal(NativeCmdQueue);
        }

        public void WaitQueue(FRHIFence GPUFence)
        {
            GPUFence.WaitOnGPU(NativeCmdQueue);
        }

        public void ExecuteQueue(FRHICommandBuffer CmdBuffer)
        {
            CmdBuffer.Close();
            NativeCmdQueue.ExecuteCommandList(CmdBuffer.NativeCmdList);
        }

        public void Flush()
        {
            FrameFence.Signal(NativeCmdQueue);
            FrameFence.WaitOnCPU(FenceEvent);
        }

        protected override void DisposeManaged()
        {

        }

        protected override void DisposeUnManaged()
        {

        }
    }
}
