using System;
using System.Threading;
using Vortice.Direct3D12;
using InfinityEngine.Core.UObject;

namespace InfinityEngine.Graphics.RHI
{
    public class RHIFence : UObject
    {
        private ulong FenceValue;
        private ulong LastFenceValue;
        private ID3D12Fence NativeFence;
        protected ID3D12CommandQueue SrcNativeCmdQueue;
        protected ID3D12CommandQueue DescNativeCmdQueue;

        public RHIFence(ID3D12Device6 D3D12Device, RHICommandContext SrcCmdBuffer, RHICommandContext DescCmdBuffer) : base()
        {
            NativeFence = D3D12Device.CreateFence(0, FenceFlags.None);
            SrcNativeCmdQueue = SrcCmdBuffer.NativeCmdQueue;
            DescNativeCmdQueue = DescCmdBuffer.NativeCmdQueue;
        }

        public void Signal()
        {
            ++FenceValue;
            SrcNativeCmdQueue.Signal(NativeFence, FenceValue);
        }

        public bool Completed()
        {
            if (FenceValue > LastFenceValue)
            {
                LastFenceValue = Math.Max(LastFenceValue, NativeFence.CompletedValue);
            }
            return FenceValue <= LastFenceValue;
        }

        public void WaitOnCPU(ManualResetEvent FenceEvent)
        {
            if (!this.Completed())
            {
                //ManualResetEvent FenceEvent = new ManualResetEvent(false);
                NativeFence.SetEventOnCompletion(FenceValue, FenceEvent);
                FenceEvent.WaitOne();
            }
        }

        public void WaitOnGPU()
        {
            DescNativeCmdQueue.Wait(NativeFence, FenceValue);
        }

        protected override void DisposeManaged()
        {

        }

        protected override void DisposeUnManaged()
        {
            NativeFence.Release();
            NativeFence.Dispose();
        }
    }
}
