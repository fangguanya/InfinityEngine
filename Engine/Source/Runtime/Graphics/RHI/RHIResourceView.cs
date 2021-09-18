using System;
using Vortice.DXGI;
using Vortice.Direct3D12;
using InfinityEngine.Core.Object;
using InfinityEngine.Core.Memory;
using System.Runtime.CompilerServices;

namespace InfinityEngine.Graphics.RHI
{
    public enum EDescriptorType
    {
        DSV = 0,
        RTV = 1,
        CbvSrvUav = 2,
        Sample = 3
    };

    public struct FRHIIndexBufferView
    {
        internal ulong virtualAddressGPU;
        internal IndexBufferView nativeBufferView;
    }

    public struct FRHIVertexBufferView
    {
        internal ulong virtualAddressGPU;
        internal VertexBufferView nativeBufferView;
    }

    public struct FRHIDeptnStencilView
    {

    }

    public struct FRHIRenderTargetView
    {

    }

    public struct FRHIConstantBufferView
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

    public struct FRHIShaderResourceView
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

    public struct FRHIUnorderedAccessView
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

    public sealed class FRHIResourceSet : FDisposable
    {
        internal int length;
        internal int descriptorIndex;
        internal ID3D12Device6 nativeDevice;
        internal CpuDescriptorHandle descriptorHandle;

        internal FRHIResourceSet(FRHIDevice device, FRHIDescriptorHeapFactory descriptorHeapFactory, in int length) : base()
        {
            this.length = length;
            this.nativeDevice = device.nativeDevice;
            this.descriptorIndex = descriptorHeapFactory.Allocator(length);
            this.descriptorHandle = descriptorHeapFactory.GetCPUHandleStart();
        }

        private CpuDescriptorHandle GetDescriptorHandle(in int offset)
        {
            return descriptorHandle + nativeDevice.GetDescriptorHandleIncrementSize(DescriptorHeapType.ConstantBufferViewShaderResourceViewUnorderedAccessView) * (descriptorIndex + offset);
        }

        public void SetConstantBufferView(in int slot, FRHIConstantBufferView constantBufferView)
        {
            nativeDevice.CopyDescriptorsSimple(1, GetDescriptorHandle(slot), constantBufferView.descriptorHandle, DescriptorHeapType.ConstantBufferViewShaderResourceViewUnorderedAccessView);
        }

        public void SetShaderResourceView(in int slot, FRHIShaderResourceView shaderResourceView)
        {
            nativeDevice.CopyDescriptorsSimple(1, GetDescriptorHandle(slot), shaderResourceView.descriptorHandle, DescriptorHeapType.ConstantBufferViewShaderResourceViewUnorderedAccessView);
        }

        public void SetUnorderedAccessView(in int slot, FRHIUnorderedAccessView unorderedAccessView)
        {
            nativeDevice.CopyDescriptorsSimple(1, GetDescriptorHandle(slot), unorderedAccessView.descriptorHandle, DescriptorHeapType.ConstantBufferViewShaderResourceViewUnorderedAccessView);
        }

        protected override void Release()
        {

        }
    }
}
