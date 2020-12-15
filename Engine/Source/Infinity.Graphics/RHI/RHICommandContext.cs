using System.Threading;
using Vortice.Direct3D;
using Vortice.Direct3D12;
using InfinityEngine.Core.Object;

namespace InfinityEngine.Graphics.RHI
{
    public class RHICommandContext : UObject
    {
        public RHIFence FrameFence;
        public ManualResetEvent FenceEvent;

        public ID3D12CommandQueue NativeCmdQueue;


        public RHICommandContext(ID3D12Device6 NativeDevice, CommandListType CommandBufferType) : base()
        {
            FenceEvent = new ManualResetEvent(false);
            FrameFence = new RHIFence(NativeDevice, this, this);
            NativeCmdQueue = NativeDevice.CreateCommandQueue(CommandBufferType);
        }

        public void WriteFence(RHIFence GPUFence)
        {
            GPUFence.Signal();
        }

        public void WaitFence(RHIFence GPUFence)
        {
            GPUFence.WaitOnGPU();
        }

        public void ExecuteCmdBuffer(RHICommandBuffer CmdBuffer)
        {
            CmdBuffer.Close();
            NativeCmdQueue.ExecuteCommandList(CmdBuffer.NativeCmdList);
        }

        public void Flush()
        {
            FrameFence.Signal();
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
