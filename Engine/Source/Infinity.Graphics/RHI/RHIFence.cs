using System;
using System.Threading;
using Vortice.Direct3D12;
using InfinityEngine.Core.Object;

namespace InfinityEngine.Graphics.RHI
{
    public class FRHIFence : UObject
    {
        private ulong fenceValue;
        private ID3D12Fence d3D12Fence;

        public FRHIFence(ID3D12Device6 d3D12Device) : base()
        {
            d3D12Fence = d3D12Device.CreateFence<ID3D12Fence>(0, FenceFlags.None);
        }

        public void Signal(ID3D12CommandQueue d3D12CmdQueue)
        {
            fenceValue++;
            d3D12CmdQueue.Signal(d3D12Fence, fenceValue);
        }

        public bool Completed()
        {
            if (d3D12Fence.CompletedValue < fenceValue)
            {
                return false;
            }
            return true;
        }

        public void WaitOnCPU(ManualResetEvent FenceEvent)
        {
            if (!Completed())
            {
                //ManualResetEvent FenceEvent = new ManualResetEvent(false);
                d3D12Fence.SetEventOnCompletion(fenceValue, FenceEvent);
                FenceEvent.WaitOne();
            }
        }

        public void WaitOnGPU(ID3D12CommandQueue d3D12CmdQueue)
        {
            d3D12CmdQueue.Wait(d3D12Fence, fenceValue);
        }

        protected override void Disposed()
        {
            //NativeFence.Release();
            d3D12Fence?.Dispose();
        }
    }
}
