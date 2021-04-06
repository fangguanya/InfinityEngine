using System;
using Vortice.DXGI;
using Vortice.Direct3D12;
using InfinityEngine.Core.Object;
using System.Runtime.CompilerServices;
using InfinityEngine.Core.Native.Utility;

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

    }

    public struct FRHIVertexBufferView
    {

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
        internal CpuDescriptorHandle descriptorHandle;


        public FRHIConstantBufferView(int descriptorSize, int descriptorIndex, CpuDescriptorHandle descriptorHandle)
        {
            this.descriptorSize = descriptorSize;
            this.descriptorIndex = descriptorIndex;
            this.descriptorHandle = descriptorHandle;
        }

        public CpuDescriptorHandle GetDescriptorHandle()
        {
            return descriptorHandle + descriptorSize * descriptorIndex;
        }

        public void Disposed()
        {
            descriptorSize = 0;
            descriptorIndex = 0;
        }
    }

    public struct FRHIShaderResourceView
    {
        internal int descriptorSize;
        internal int descriptorIndex;
        internal CpuDescriptorHandle descriptorHandle;


        public FRHIShaderResourceView(int descriptorSize, int descriptorIndex, CpuDescriptorHandle descriptorHandle)
        {
            this.descriptorSize = descriptorSize;
            this.descriptorIndex = descriptorIndex;
            this.descriptorHandle = descriptorHandle;
        }

        public CpuDescriptorHandle GetDescriptorHandle()
        {
            return descriptorHandle + descriptorSize * descriptorIndex;
        }

        public void Disposed()
        {
            descriptorSize = 0;
            descriptorIndex = 0;
        }
    }

    public struct FRHIUnorderedAccessView
    {
        internal int descriptorSize;
        internal int descriptorIndex;
        internal CpuDescriptorHandle descriptorHandle;


        public FRHIUnorderedAccessView(int descriptorSize, int descriptorIndex, CpuDescriptorHandle descriptorHandle)
        {
            this.descriptorSize = descriptorSize;
            this.descriptorIndex = descriptorIndex;
            this.descriptorHandle = descriptorHandle;
        }

        public CpuDescriptorHandle GetDescriptorHandle()
        {
            return descriptorHandle + descriptorSize * descriptorIndex;
        }

        public void Disposed()
        {
            descriptorSize = 0;
            descriptorIndex = 0;
        }
    }

    public class FRHIResourceViewRange : UObject
    {
        protected int rangeSize;
        protected int descriptorIndex;

        protected ID3D12Device6 d3D12Device;
        protected CpuDescriptorHandle descriptorHandle;


        internal FRHIResourceViewRange(ID3D12Device6 d3D12Device, FRHIDescriptorHeapFactory descriptorHeapFactory, in int descriptorLength) : base()
        {
            this.rangeSize = descriptorLength;
            this.d3D12Device = d3D12Device;
            this.descriptorIndex = descriptorHeapFactory.Allocator(descriptorLength);
            this.descriptorHandle = descriptorHeapFactory.GetCPUHandleStart();
        }

        protected CpuDescriptorHandle GetDescriptorHandle(in int offset)
        {
            return descriptorHandle + d3D12Device.GetDescriptorHandleIncrementSize(DescriptorHeapType.ConstantBufferViewShaderResourceViewUnorderedAccessView) * (descriptorIndex + offset);
        }

        public void SetConstantBufferView(in int index, FRHIConstantBufferView constantBufferView)
        {
            d3D12Device.CopyDescriptorsSimple(1, GetDescriptorHandle(index), constantBufferView.GetDescriptorHandle(), DescriptorHeapType.ConstantBufferViewShaderResourceViewUnorderedAccessView);
        }

        public void SetShaderResourceView(in int index, FRHIShaderResourceView shaderResourceView)
        {
            d3D12Device.CopyDescriptorsSimple(1, GetDescriptorHandle(index), shaderResourceView.GetDescriptorHandle(), DescriptorHeapType.ConstantBufferViewShaderResourceViewUnorderedAccessView);
        }

        public void SetUnorderedAccessView(in int index, FRHIUnorderedAccessView unorderedAccessView)
        {
            d3D12Device.CopyDescriptorsSimple(1, GetDescriptorHandle(index), unorderedAccessView.GetDescriptorHandle(), DescriptorHeapType.ConstantBufferViewShaderResourceViewUnorderedAccessView);
        }

        protected override void Disposed()
        {

        }
    }
}
