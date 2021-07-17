using System;
using System.Threading;
using Vortice.Direct3D12;
using InfinityEngine.Core.Object;

namespace InfinityEngine.Graphics.RHI
{
    public class FRHIFence : FDisposer
    {
        private ulong fenceValue;
        private ID3D12Fence d3dFence;

        public FRHIFence(ID3D12Device6 d3dDevice) : base()
        {
            d3dFence = d3dDevice.CreateFence<ID3D12Fence>(0, FenceFlags.None);
        }

        public void Signal(ID3D12CommandQueue d3D12CmdQueue)
        {
            ++fenceValue;
            d3D12CmdQueue.Signal(d3dFence, fenceValue);
        }

        public bool Completed()
        {
            if (d3dFence.CompletedValue < fenceValue)
            {
                return false;
            }
            return true;
        }

        public void WaitOnCPU(AutoResetEvent fenceEvent)
        {
            if (!Completed())
            {
                //ManualResetEvent FenceEvent = new ManualResetEvent(false);
                d3dFence.SetEventOnCompletion(fenceValue, fenceEvent);
                fenceEvent.WaitOne();
            }
        }

        public void WaitOnGPU(ID3D12CommandQueue d3D12CmdQueue)
        {
            d3D12CmdQueue.Wait(d3dFence, fenceValue);
        }

        protected override void Disposed()
        {
            //NativeFence.Release();
            d3dFence?.Dispose();
        }
    }
}
