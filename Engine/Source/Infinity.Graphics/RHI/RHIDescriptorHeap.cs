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
        protected int DescriptorSize;

        protected ID3D12Device6 NativeDevice;
        protected ID3D12DescriptorHeap CPUDescriptorHeap;
        protected ID3D12DescriptorHeap GPUDescriptorHeap;


        internal FRHIDescriptorHeapFactory(ID3D12Device6 InNativeDevice, in DescriptorHeapType InType, in int DescriptorCount) : base()
        {
            NativeDevice = InNativeDevice;

            DescriptorSize = InNativeDevice.GetDescriptorHandleIncrementSize(DescriptorHeapType.DepthStencilView);

            DescriptorHeapDescription CPUDescriptorHeapDescription = new DescriptorHeapDescription(InType, DescriptorCount, DescriptorHeapFlags.ShaderVisible);
            CPUDescriptorHeap = InNativeDevice.CreateDescriptorHeap<ID3D12DescriptorHeap>(CPUDescriptorHeapDescription);

            DescriptorHeapDescription GPUDescriptorHeapDescription = new DescriptorHeapDescription(InType, DescriptorCount, DescriptorHeapFlags.None);
            GPUDescriptorHeap = InNativeDevice.CreateDescriptorHeap<ID3D12DescriptorHeap>(GPUDescriptorHeapDescription);
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
            return DescriptorSize;
        }

        internal CpuDescriptorHandle GetCPUHandleStart()
        {
            return CPUDescriptorHeap.GetCPUDescriptorHandleForHeapStart();
        }

        internal GpuDescriptorHandle GetGPUHandleStart()
        {
            return GPUDescriptorHeap.GetGPUDescriptorHandleForHeapStart();
        }

        protected override void Disposed()
        {
            CPUDescriptorHeap?.Dispose();
            GPUDescriptorHeap?.Dispose();
        }
    }
}
