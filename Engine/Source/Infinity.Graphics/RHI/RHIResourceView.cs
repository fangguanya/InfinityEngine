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
        internal int DescriptorSize;
        internal int DescriptorIndex;
        internal CpuDescriptorHandle CPUDescriptorHandle;


        public FRHIConstantBufferView(int InDescriptorSize, int InDescriptorIndex, CpuDescriptorHandle InCPUDescriptorHandle)
        {
            DescriptorSize = InDescriptorSize;
            DescriptorIndex = InDescriptorIndex;
            CPUDescriptorHandle = InCPUDescriptorHandle;
        }

        public CpuDescriptorHandle GetDescriptorHandle()
        {
            return CPUDescriptorHandle + DescriptorSize * DescriptorIndex;
        }

        public void Disposed()
        {
            DescriptorSize = 0;
            DescriptorIndex = 0;
        }
    }

    public struct FRHIShaderResourceView
    {
        internal int DescriptorSize;
        internal int DescriptorIndex;
        internal CpuDescriptorHandle CPUDescriptorHandle;


        public FRHIShaderResourceView(int InDescriptorSize, int InDescriptorIndex, CpuDescriptorHandle InCPUDescriptorHandle)
        {
            DescriptorSize = InDescriptorSize;
            DescriptorIndex = InDescriptorIndex;
            CPUDescriptorHandle = InCPUDescriptorHandle;
        }

        public CpuDescriptorHandle GetDescriptorHandle()
        {
            return CPUDescriptorHandle + DescriptorSize * DescriptorIndex;
        }

        public void Disposed()
        {
            DescriptorSize = 0;
            DescriptorIndex = 0;
        }
    }

    public struct FRHIUnorderedAccessView
    {
        internal int DescriptorSize;
        internal int DescriptorIndex;
        internal CpuDescriptorHandle CPUDescriptorHandle;


        public FRHIUnorderedAccessView(int InDescriptorSize, int InDescriptorIndex, CpuDescriptorHandle InCPUDescriptorHandle)
        {
            DescriptorSize = InDescriptorSize;
            DescriptorIndex = InDescriptorIndex;
            CPUDescriptorHandle = InCPUDescriptorHandle;
        }

        public CpuDescriptorHandle GetDescriptorHandle()
        {
            return CPUDescriptorHandle + DescriptorSize * DescriptorIndex;
        }

        public void Disposed()
        {
            DescriptorSize = 0;
            DescriptorIndex = 0;
        }
    }
}
