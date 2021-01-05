using System;
using System.Threading;
using Vortice.Direct3D12;
using InfinityEngine.Core.Object;

namespace InfinityEngine.Graphics.RHI
{
    public class FRHIFence : UObject
    {
        private ulong FenceValue;
        private ulong LastFenceValue;
        private ID3D12Fence NativeFence;

        public FRHIFence(ID3D12Device6 D3D12Device) : base()
        {
            NativeFence = D3D12Device.CreateFence(0, FenceFlags.None);
        }

        public void Signal(ID3D12CommandQueue NativeCmdQueue)
        {
            ++FenceValue;
            NativeCmdQueue.Signal(NativeFence, FenceValue);
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

        public void WaitOnGPU(ID3D12CommandQueue NativeCmdQueue)
        {
            NativeCmdQueue.Wait(NativeFence, FenceValue);
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
