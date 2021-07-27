﻿using System;
using Vortice.DXGI;
using Vortice.Direct3D12;
using InfinityEngine.Core.Object;
using System.Runtime.CompilerServices;
using InfinityEngine.Core.Native.Utility;

namespace InfinityEngine.Graphics.RHI
{
    internal class FRHIDescriptorHeapFactory : FDisposer
    {
        protected int descriptorSize;
        protected ID3D12Device6 d3dDevice;
        protected ID3D12DescriptorHeap d3d12CPUDescriptorHeap;
        protected ID3D12DescriptorHeap d3d12GPUDescriptorHeap;

        internal FRHIDescriptorHeapFactory(ID3D12Device6 d3dDevice, in DescriptorHeapType descriptorType, in int descriptorCount) : base()
        {
            this.d3dDevice = d3dDevice;

            this.descriptorSize = d3dDevice.GetDescriptorHandleIncrementSize(DescriptorHeapType.DepthStencilView);

            DescriptorHeapDescription descriptionCPU = new DescriptorHeapDescription(descriptorType, descriptorCount, DescriptorHeapFlags.ShaderVisible);
            this.d3d12CPUDescriptorHeap = d3dDevice.CreateDescriptorHeap<ID3D12DescriptorHeap>(descriptionCPU);

            DescriptorHeapDescription descriptionGPU = new DescriptorHeapDescription(descriptorType, descriptorCount, DescriptorHeapFlags.None);
            this.d3d12GPUDescriptorHeap = d3dDevice.CreateDescriptorHeap<ID3D12DescriptorHeap>(descriptionGPU);
        }

        protected static DescriptorHeapType GetDescriptorType(in EDescriptorType DescriptorType)
        {
            DescriptorHeapType OutType = DescriptorHeapType.Sampler;

            switch (DescriptorType)
            {
                case EDescriptorType.DSV:
                    OutType = DescriptorHeapType.DepthStencilView;
                    break;

                case EDescriptorType.RTV:
                    OutType = DescriptorHeapType.RenderTargetView;
                    break;

                case EDescriptorType.CbvSrvUav:
                    OutType = DescriptorHeapType.ConstantBufferViewShaderResourceViewUnorderedAccessView;
                    break;
            }

            return OutType;
        }

        internal int Allocator(in int count)
        {
            return 1;
        }

        internal int GetDescriptorSize()
        {
            return descriptorSize;
        }

        internal CpuDescriptorHandle GetCPUHandleStart()
        {
            return d3d12CPUDescriptorHeap.GetCPUDescriptorHandleForHeapStart();
        }

        internal GpuDescriptorHandle GetGPUHandleStart()
        {
            return d3d12GPUDescriptorHeap.GetGPUDescriptorHandleForHeapStart();
        }

        protected override void Disposed()
        {
            d3d12CPUDescriptorHeap?.Release();
            d3d12GPUDescriptorHeap?.Release();
        }
    }
}
