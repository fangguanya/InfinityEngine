using System.Threading;
using Vortice.Direct3D12;

namespace InfinityEngine.Graphics.RHI
{
    public class FD3DFence : FRHIFence
    {
        public override bool IsCompleted
        {
            get { return m_NativeFence.CompletedValue >= m_FenceValue ? true : false; }
        }
        
        private ulong m_FenceValue;
        private ID3D12Fence m_NativeFence;

        internal FD3DFence(FRHIDevice device, string name = null) : base(device, name)
        {
            FD3DDevice d3dDevice = (FD3DDevice)device;

            this.name = name;
            this.m_NativeFence = d3dDevice.nativeDevice.CreateFence<ID3D12Fence>(0, FenceFlags.None);
        }

        internal override void Signal(FRHICommandContext cmdContext)
        {
            FD3DCommandContext d3dCmdContext = (FD3DCommandContext)cmdContext;

            ++m_FenceValue;
            d3dCmdContext.nativeCmdQueue.Signal(m_NativeFence, m_FenceValue);
        }

        internal override void WaitOnCPU(AutoResetEvent fenceEvent)
        {
            if (!IsCompleted)
            {
                m_NativeFence.SetEventOnCompletion(m_FenceValue, fenceEvent);
                fenceEvent.WaitOne();
            }
        }

        internal override void WaitOnGPU(FRHICommandContext cmdContext)
        {
            FD3DCommandContext d3dCmdContext = (FD3DCommandContext)cmdContext;
            d3dCmdContext.nativeCmdQueue.Wait(m_NativeFence, m_FenceValue);
        }

        protected override void Release()
        {
            m_NativeFence?.Dispose();
        }
    }
}
