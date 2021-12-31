using Vortice.Direct3D12;
using InfinityEngine.Core.Object;

namespace InfinityEngine.Graphics.RHI
{
    public enum EDescriptorType
    {
        DSV = 0,
        RTV = 1,
        CbvSrvUav = 2,
        Sample = 3
    };

    public class FRHIResourceView
    {
        internal ulong virtualAddressGPU;
    }

    public class FRHIIndexBufferView : FRHIResourceView
    {

    }

    public class FRHIVertexBufferView : FRHIResourceView
    {

    }

    public class FRHIDeptnStencilView : FRHIResourceView
    {

    }

    public class FRHIRenderTargetView : FRHIResourceView
    {

    }

    public class FRHIConstantBufferView : FRHIResourceView
    {
        internal int descriptorSize;
        internal int descriptorIndex;
        internal CpuDescriptorHandle descriptorHandle
        {
            get
            {
                return m_DescriptorHandle + descriptorSize * descriptorIndex;
            }
        }

        private CpuDescriptorHandle m_DescriptorHandle;

        public FRHIConstantBufferView(int descriptorSize, int descriptorIndex, CpuDescriptorHandle descriptorHandle)
        {
            this.descriptorSize = descriptorSize;
            this.descriptorIndex = descriptorIndex;
            this.m_DescriptorHandle = descriptorHandle;
        }
    }

    public class FRHIShaderResourceView : FRHIResourceView
    {
        internal int descriptorSize;
        internal int descriptorIndex;
        internal CpuDescriptorHandle descriptorHandle
        {
            get
            {
                return m_DescriptorHandle + descriptorSize * descriptorIndex;
            }
        }

        private CpuDescriptorHandle m_DescriptorHandle;

        public FRHIShaderResourceView(int descriptorSize, int descriptorIndex, CpuDescriptorHandle descriptorHandle)
        {
            this.descriptorSize = descriptorSize;
            this.descriptorIndex = descriptorIndex;
            this.m_DescriptorHandle = descriptorHandle;
        }
    }

    public class FRHIUnorderedAccessView : FRHIResourceView
    {
        internal int descriptorSize;
        internal int descriptorIndex;
        internal CpuDescriptorHandle descriptorHandle
        {
            get
            {
                return m_DescriptorHandle + descriptorSize * descriptorIndex;
            }
        }

        private CpuDescriptorHandle m_DescriptorHandle;

        public FRHIUnorderedAccessView(int descriptorSize, int descriptorIndex, CpuDescriptorHandle descriptorHandle)
        {
            this.descriptorSize = descriptorSize;
            this.descriptorIndex = descriptorIndex;
            this.m_DescriptorHandle = descriptorHandle;
        }
    }

    public sealed class FRHIResourceSet : FDisposal
    {
        /*internal int length;
        internal int descriptorIndex;
        internal ID3D12Device6 nativeDevice;
        internal CpuDescriptorHandle descriptorHandle;*/

        internal FRHIResourceSet(FRHIDevice device, FRHIDescriptorHeapFactory descriptorHeapFactory, in int length) : base()
        {
            /*this.length = length;
            this.nativeDevice = device.nativeDevice;
            this.descriptorIndex = descriptorHeapFactory.Allocator(length);
            this.descriptorHandle = descriptorHeapFactory.GetCPUHandleStart();*/
        }

        private CpuDescriptorHandle GetDescriptorHandle(in int offset)
        {
            return default;
            //return descriptorHandle + nativeDevice.GetDescriptorHandleIncrementSize(DescriptorHeapType.ConstantBufferViewShaderResourceViewUnorderedAccessView) * (descriptorIndex + offset);
        }

        public void SetShaderResourceView(in int slot, FRHIShaderResourceView shaderResourceView)
        {
            //nativeDevice.CopyDescriptorsSimple(1, GetDescriptorHandle(slot), shaderResourceView.descriptorHandle, DescriptorHeapType.ConstantBufferViewShaderResourceViewUnorderedAccessView);
        }

        public void SetUnorderedAccessView(in int slot, FRHIUnorderedAccessView unorderedAccessView)
        {
            //nativeDevice.CopyDescriptorsSimple(1, GetDescriptorHandle(slot), unorderedAccessView.descriptorHandle, DescriptorHeapType.ConstantBufferViewShaderResourceViewUnorderedAccessView);
        }

        protected override void Release()
        {

        }
    }
}
