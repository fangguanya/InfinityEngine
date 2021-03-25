using System;
using Vortice.DXGI;
using Vortice.Direct3D12;
using InfinityEngine.Core.Object;
using System.Runtime.CompilerServices;
using InfinityEngine.Core.Native.Utility;

namespace InfinityEngine.Graphics.RHI
{
    internal class FRHIDescriptorHeapFactory : UObject
    {
        protected int descriptorSize;
        protected ID3D12Device6 d3D12Device;
        protected ID3D12DescriptorHeap d3D12CPUDescriptorHeap;
        protected ID3D12DescriptorHeap d3D12GPUDescriptorHeap;


        internal FRHIDescriptorHeapFactory(ID3D12Device6 d3D12Device, in DescriptorHeapType descriptorType, in int descriptorCount) : base()
        {
            this.d3D12Device = d3D12Device;

            descriptorSize = d3D12Device.GetDescriptorHandleIncrementSize(DescriptorHeapType.DepthStencilView);

            DescriptorHeapDescription CPUDescriptorHeapDescription = new DescriptorHeapDescription(descriptorType, descriptorCount, DescriptorHeapFlags.ShaderVisible);
            d3D12CPUDescriptorHeap = d3D12Device.CreateDescriptorHeap<ID3D12DescriptorHeap>(CPUDescriptorHeapDescription);

            DescriptorHeapDescription GPUDescriptorHeapDescription = new DescriptorHeapDescription(descriptorType, descriptorCount, DescriptorHeapFlags.None);
            d3D12GPUDescriptorHeap = d3D12Device.CreateDescriptorHeap<ID3D12DescriptorHeap>(GPUDescriptorHeapDescription);
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

        internal int Allocator(in int Count)
        {
            return 1;
        }

        internal int GetDescriptorSize()
        {
            return descriptorSize;
        }

        internal CpuDescriptorHandle GetCPUHandleStart()
        {
            return d3D12CPUDescriptorHeap.GetCPUDescriptorHandleForHeapStart();
        }

        internal GpuDescriptorHandle GetGPUHandleStart()
        {
            return d3D12GPUDescriptorHeap.GetGPUDescriptorHandleForHeapStart();
        }

        protected override void Disposed()
        {
            d3D12CPUDescriptorHeap?.Dispose();
            d3D12GPUDescriptorHeap?.Dispose();
        }
    }
}
