using System.Threading;
using Vortice.Direct3D12;
using InfinityEngine.Core.Object;

namespace InfinityEngine.Graphics.RHI
{
    public class FRHIFence : FDisposable
    {
        private ulong fenceValue;
        private ID3D12Fence d3dFence;

        internal FRHIFence(ID3D12Device6 d3dDevice) : base()
        {
            d3dFence = d3dDevice.CreateFence<ID3D12Fence>(0, FenceFlags.None);
        }

        public void Signal(ID3D12CommandQueue d3d12CmdQueue)
        {
            ++fenceValue;
            d3d12CmdQueue.Signal(d3dFence, fenceValue);
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
                d3dFence.SetEventOnCompletion(fenceValue, fenceEvent);
                fenceEvent.WaitOne();
            }
        }

        public void WaitOnGPU(ID3D12CommandQueue d3d12CmdQueue)
        {
            d3d12CmdQueue.Wait(d3dFence, fenceValue);
        }

        protected override void Disposed()
        {
            d3dFence?.Dispose();
        }
    }
}
