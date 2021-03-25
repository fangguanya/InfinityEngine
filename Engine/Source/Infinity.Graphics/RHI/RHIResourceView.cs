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
}
