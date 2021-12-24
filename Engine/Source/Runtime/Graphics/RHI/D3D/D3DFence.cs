using System;
using System.Threading;
using TerraFX.Interop.Windows;
using TerraFX.Interop.DirectX;

namespace InfinityEngine.Graphics.RHI.D3D
{
    public unsafe class FD3DFence : FRHIFence
    {
        public override bool IsCompleted
        {
            get { return m_NativeFence->GetCompletedValue() >= m_FenceValue ? true : false; }
        }
        
        private ulong m_FenceValue;
        private ID3D12Fence* m_NativeFence;

        internal FD3DFence(FRHIDevice device, string name = null) : base(device, name)
        {
            this.name = name;
            FD3DDevice d3dDevice = (FD3DDevice)device;

            ID3D12Fence* fence = null;
            d3dDevice.nativeDevice->CreateFence(0, D3D12_FENCE_FLAGS.D3D12_FENCE_FLAG_NONE, Windows.__uuidof<ID3D12Fence>() , (void**)&fence);
            m_NativeFence = fence;
        }

        internal override void Signal(FRHICommandContext cmdContext)
        {
            FD3DCommandContext d3dCmdContext = (FD3DCommandContext)cmdContext;

            ++m_FenceValue;
            d3dCmdContext.nativeCmdQueue->Signal(m_NativeFence, m_FenceValue);
        }

        internal override void WaitOnCPU(AutoResetEvent fenceEvent)
        {
            if (!IsCompleted)
            {
                IntPtr eventPtr = fenceEvent.SafeWaitHandle.DangerousGetHandle();
                HANDLE eventHandle = new HANDLE(eventPtr.ToPointer());
                m_NativeFence->SetEventOnCompletion(m_FenceValue, eventHandle);
                Windows.WaitForSingleObject(eventHandle, uint.MaxValue);
            }
        }

        internal override void WaitOnGPU(FRHICommandContext cmdContext)
        {
            FD3DCommandContext d3dCmdContext = (FD3DCommandContext)cmdContext;
            d3dCmdContext.nativeCmdQueue->Wait(m_NativeFence, m_FenceValue);
        }

        protected override void Release()
        {
            m_NativeFence->Release();
        }
    }
}
