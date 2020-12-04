﻿using System;
using Vortice.DXGI;
using Vortice.Direct3D12;
using Infinity.Runtime.Graphics.Core;
using System.Runtime.CompilerServices;

namespace Infinity.Runtime.Graphics.RHI
{
    // Resource
    public enum EUseFlag
    {
        GPURW,
        CPURW,
        CPURead,
        CPUWrite,
    };

    public enum EResourceType
    {
        Buffer,
        Texture,
    };

    public enum EDescriptorType
    {
        DSV = 0,
        RTV = 1,
        CbvSrvUav = 2,
        Sample = 3
    };

    public enum EBufferType
    {
        Append,
        Consume,
        Counter,
        Constant,
        Structured,
        ByteAddressBuffer,
        IndirectArguments,
    };

    public enum ETextureType
    {
        Tex2D,
        Tex2DArray,
        TexCube,
        TexCubeArray,
        Tex3D,
    };

    public struct RHIIndexBufferView
    {

    }

    public struct RHIVertexBufferView
    {

    }

    public struct RHIDeptnStencilView
    {

    }

    public struct RHIRenderTargetView
    {

    }

    public struct RHIConstantBufferView
    {
        internal int DescriptorSize;
        internal int DescriptorIndex;
        internal CpuDescriptorHandle CPUDescriptorHandle;


        public RHIConstantBufferView(int InDescriptorSize, int InDescriptorIndex, CpuDescriptorHandle InCPUDescriptorHandle)
        {
            DescriptorSize = InDescriptorSize;
            DescriptorIndex = InDescriptorIndex;
            CPUDescriptorHandle = InCPUDescriptorHandle;
        }

        public CpuDescriptorHandle GetDescriptorHandle()
        {
            return CPUDescriptorHandle + DescriptorSize * DescriptorIndex;
        }

        public void Release()
        {
            DescriptorSize = 0;
            DescriptorIndex = 0;
        }
    }

    public struct RHIShaderResourceView
    {
        internal int DescriptorSize;
        internal int DescriptorIndex;
        internal CpuDescriptorHandle CPUDescriptorHandle;


        public RHIShaderResourceView(int InDescriptorSize, int InDescriptorIndex, CpuDescriptorHandle InCPUDescriptorHandle)
        {
            DescriptorSize = InDescriptorSize;
            DescriptorIndex = InDescriptorIndex;
            CPUDescriptorHandle = InCPUDescriptorHandle;
        }

        public CpuDescriptorHandle GetDescriptorHandle()
        {
            return CPUDescriptorHandle + DescriptorSize * DescriptorIndex;
        }

        public void Release()
        {
            DescriptorSize = 0;
            DescriptorIndex = 0;
        }
    }

    public struct RHIUnorderedAccessView
    {
        internal int DescriptorSize;
        internal int DescriptorIndex;
        internal CpuDescriptorHandle CPUDescriptorHandle;


        public RHIUnorderedAccessView(int InDescriptorSize, int InDescriptorIndex, CpuDescriptorHandle InCPUDescriptorHandle)
        {
            DescriptorSize = InDescriptorSize;
            DescriptorIndex = InDescriptorIndex;
            CPUDescriptorHandle = InCPUDescriptorHandle;
        }

        public CpuDescriptorHandle GetDescriptorHandle()
        {
            return CPUDescriptorHandle + DescriptorSize * DescriptorIndex;
        }

        public void Release()
        {
            DescriptorSize = 0;
            DescriptorIndex = 0;
        }
    }


    public class RHIResource : TObject
    {
        public string Name;

        protected EUseFlag UseFlag;
        protected EResourceType ResourceType;
        protected Format GraphicsFormat;

        internal ID3D12Resource DefaultResource;
        internal ID3D12Resource UploadResource;
        internal ID3D12Resource ReadbackResource;

        internal RHIShaderResourceView SRV;
        internal RHIUnorderedAccessView UAV;

        protected ID3D12Device6 NativeDevice;
        protected ID3D12GraphicsCommandList6 NativeCopyList;


        public RHIResource(ID3D12Device6 InNativeDevice, ID3D12GraphicsCommandList6 InNativeCopyList, EUseFlag InUseFlag) : base()
        {
            UseFlag = InUseFlag;
            NativeDevice = InNativeDevice;
            NativeCopyList = InNativeCopyList;
        }

        protected override void DisposeManaged()
        {
            UAV.Release();
        }

        protected override void DisposeUnManaged()
        {
            NativeDevice = null;
            NativeCopyList = null;

            DefaultResource.Release();
            DefaultResource.Dispose();

            if (UploadResource != null)
            {
                UploadResource.Release();
                UploadResource.Dispose();
            }

            if (ReadbackResource != null)
            {
                ReadbackResource.Release();
                ReadbackResource.Dispose();
            }
        }
    }

    public class RHIBuffer : RHIResource
    {
        internal ulong Count;
        internal ulong Stride;
        internal EBufferType BufferType;

        internal RHIIndexBufferView IBV;
        internal RHIVertexBufferView VBV;
        internal RHIConstantBufferView CBV;

        internal RHIBuffer(ID3D12Device6 InNativeDevice, ID3D12GraphicsCommandList6 InNativeCopyList, EUseFlag InUseFlag, EBufferType InBufferType, ulong InCount, ulong InStride) : base(InNativeDevice, InNativeCopyList, InUseFlag)
        {
            Count = InCount;
            Stride = InStride;
            BufferType = InBufferType;
            ResourceType = EResourceType.Buffer;

            // GPUMemory
            HeapProperties DefaultHeapProperty;
            {
                DefaultHeapProperty.Type = HeapType.Default;
                DefaultHeapProperty.CPUPageProperty = CpuPageProperty.Unknown;
                DefaultHeapProperty.MemoryPoolPreference = MemoryPool.Unknown;
                DefaultHeapProperty.CreationNodeMask = 1;
                DefaultHeapProperty.VisibleNodeMask = 1;
            }
            ResourceDescription DefaultResourceDesc;
            {
                DefaultResourceDesc.Dimension = ResourceDimension.Buffer;
                DefaultResourceDesc.Alignment = 0;
                DefaultResourceDesc.Width = Stride;
                DefaultResourceDesc.Height = 1;
                DefaultResourceDesc.DepthOrArraySize = 1;
                DefaultResourceDesc.MipLevels = 1;
                DefaultResourceDesc.Format = Format.Unknown;
                DefaultResourceDesc.SampleDescription.Count = 1;
                DefaultResourceDesc.SampleDescription.Quality = 0;
                DefaultResourceDesc.Layout = TextureLayout.RowMajor;
                DefaultResourceDesc.Flags = ResourceFlags.None;
            }
            DefaultResource = NativeDevice.CreateCommittedResource(DefaultHeapProperty, HeapFlags.None, DefaultResourceDesc, ResourceStates.Common, null);

            // CPUMemory
            if (UseFlag == EUseFlag.CPURW || UseFlag == EUseFlag.CPUWrite)
            {
                HeapProperties UploadHeapProperty;
                {
                    UploadHeapProperty.Type = HeapType.Upload;
                    UploadHeapProperty.CPUPageProperty = CpuPageProperty.Unknown;
                    UploadHeapProperty.MemoryPoolPreference = MemoryPool.Unknown;
                    UploadHeapProperty.CreationNodeMask = 1;
                    UploadHeapProperty.VisibleNodeMask = 1;
                }
                ResourceDescription UploadResourceDesc;
                {
                    UploadResourceDesc.Dimension = ResourceDimension.Buffer;
                    UploadResourceDesc.Alignment = 0;
                    UploadResourceDesc.Width = Stride;
                    UploadResourceDesc.Height = 1;
                    UploadResourceDesc.DepthOrArraySize = 1;
                    UploadResourceDesc.MipLevels = 1;
                    UploadResourceDesc.Format = Format.Unknown;
                    UploadResourceDesc.SampleDescription.Count = 1;
                    UploadResourceDesc.SampleDescription.Quality = 0;
                    UploadResourceDesc.Layout = TextureLayout.RowMajor;
                    UploadResourceDesc.Flags = ResourceFlags.None;
                }
                UploadResource = NativeDevice.CreateCommittedResource(UploadHeapProperty, HeapFlags.None, UploadResourceDesc, ResourceStates.GenericRead, null);
            }

            // Readback
            if (UseFlag == EUseFlag.CPURW || UseFlag == EUseFlag.CPURead)
            {
                HeapProperties ReadbackHeapProperties;
                {
                    ReadbackHeapProperties.Type = HeapType.Readback;
                    ReadbackHeapProperties.CPUPageProperty = CpuPageProperty.Unknown;
                    ReadbackHeapProperties.MemoryPoolPreference = MemoryPool.Unknown;
                    ReadbackHeapProperties.CreationNodeMask = 1;
                    ReadbackHeapProperties.VisibleNodeMask = 1;
                }
                ResourceDescription ReadbackResourceDesc;
                {
                    ReadbackResourceDesc.Dimension = ResourceDimension.Buffer;
                    ReadbackResourceDesc.Alignment = 0;
                    ReadbackResourceDesc.Width = Stride;
                    ReadbackResourceDesc.Height = 1;
                    ReadbackResourceDesc.DepthOrArraySize = 1;
                    ReadbackResourceDesc.MipLevels = 1;
                    ReadbackResourceDesc.Format = Format.Unknown;
                    ReadbackResourceDesc.SampleDescription.Count = 1;
                    ReadbackResourceDesc.SampleDescription.Quality = 0;
                    ReadbackResourceDesc.Layout = TextureLayout.RowMajor;
                    ReadbackResourceDesc.Flags = ResourceFlags.None;
                }
                ReadbackResource = NativeDevice.CreateCommittedResource(ReadbackHeapProperties, HeapFlags.None, ReadbackResourceDesc, ResourceStates.CopyDestination, null);
            }
        }

        public unsafe void GetData<T>(T[] Data) where T : unmanaged
        {
            if (UseFlag == EUseFlag.CPURW || UseFlag == EUseFlag.CPURead)
            {
                NativeCopyList.ResourceBarrierTransition(DefaultResource, ResourceStates.Common, ResourceStates.CopySource);
                NativeCopyList.CopyBufferRegion(ReadbackResource, 0, DefaultResource, 0, (ulong)Data.Length * (ulong)Unsafe.SizeOf<T>());
                NativeCopyList.ResourceBarrierTransition(DefaultResource, ResourceStates.CopySource, ResourceStates.Common);

                IntPtr ReadbackResourcePtr = ReadbackResource.Map(0);
                ReadbackResourcePtr.CopyTo(Data.AsSpan());
                ReadbackResource.Unmap(0);
            }
        }

        public unsafe void SetData<T>(T[] Data) where T : unmanaged
        {
            if (UseFlag == EUseFlag.CPURW || UseFlag == EUseFlag.CPUWrite)
            {
                IntPtr UploadResourcePtr = UploadResource.Map(0);
                Data.AsSpan().CopyTo(UploadResourcePtr);
                UploadResource.Unmap(0);

                NativeCopyList.ResourceBarrierTransition(DefaultResource, ResourceStates.Common, ResourceStates.CopyDestination);
                NativeCopyList.CopyBufferRegion(DefaultResource, 0, UploadResource, 0, (ulong)Data.Length * (ulong)Unsafe.SizeOf<T>());
                NativeCopyList.ResourceBarrierTransition(DefaultResource, ResourceStates.CopyDestination, ResourceStates.GenericRead);
            }
        }

        protected override void DisposeManaged()
        {
            base.DisposeManaged();
        }

        protected override void DisposeUnManaged()
        {
            base.DisposeUnManaged();
        }
    }

    public class RHITexture : RHIResource
    {
        internal ETextureType TextureType;

        internal RHIDeptnStencilView DSV;
        internal RHIRenderTargetView RTV;

        public RHITexture(ID3D12Device6 InNativeDevice, ID3D12GraphicsCommandList6 InCopyCmdList, EUseFlag InUseFlag, ETextureType InTextureType) : base(InNativeDevice, InCopyCmdList, InUseFlag)
        {
            TextureType = InTextureType;
            ResourceType = EResourceType.Texture;
        }

        protected override void DisposeManaged()
        {
            base.DisposeManaged();
        }

        protected override void DisposeUnManaged()
        {
            base.DisposeUnManaged();
        }
    }

    public class RHIResourceViewRange : TObject
    {
        public string name;

        protected int RangeSize;
        protected int DescriptorIndex;

        protected ID3D12Device6 NativeDevice;
        protected CpuDescriptorHandle DescriptorHandle;


        internal RHIResourceViewRange(ID3D12Device6 InNativeDevice, RHIDescriptorHeapFactory DescriptorHeapFactory, int DescriptorLength) : base()
        {
            RangeSize = DescriptorLength;
            NativeDevice = InNativeDevice;
            DescriptorIndex = DescriptorHeapFactory.Allocator(DescriptorLength);
            DescriptorHandle = DescriptorHeapFactory.GetCPUHandleStart();
        }

        protected CpuDescriptorHandle GetDescriptorHandle(int Offset)
        {
            return DescriptorHandle + NativeDevice.GetDescriptorHandleIncrementSize(DescriptorHeapType.ConstantBufferViewShaderResourceViewUnorderedAccessView) * (DescriptorIndex + Offset);
        }

        public void SetConstantBufferView(int Index, RHIConstantBufferView ConstantBufferView)
        {
            NativeDevice.CopyDescriptorsSimple(1, GetDescriptorHandle(Index), ConstantBufferView.GetDescriptorHandle(), DescriptorHeapType.ConstantBufferViewShaderResourceViewUnorderedAccessView);
        }

        public void SetShaderResourceView(int Index, RHIShaderResourceView ShaderResourceView)
        {
            NativeDevice.CopyDescriptorsSimple(1, GetDescriptorHandle(Index), ShaderResourceView.GetDescriptorHandle(), DescriptorHeapType.ConstantBufferViewShaderResourceViewUnorderedAccessView);
        }

        public void SetUnorderedAccessView(int Index, RHIUnorderedAccessView UnorderedAccessView)
        {
            NativeDevice.CopyDescriptorsSimple(1, GetDescriptorHandle(Index), UnorderedAccessView.GetDescriptorHandle(), DescriptorHeapType.ConstantBufferViewShaderResourceViewUnorderedAccessView);
        }

        protected override void DisposeManaged()
        {

        }

        protected override void DisposeUnManaged()
        {
            NativeDevice = null;
        }
    }

    internal sealed class RHIMemoryHeapFactory : TObject
    {
        internal string name;


        internal RHIMemoryHeapFactory(ID3D12Device6 InNativeDevice, int HeapCount) : base()
        {

        }

        protected override void DisposeManaged()
        {

        }

        protected override void DisposeUnManaged()
        {

        }
    }

    internal class RHIDescriptorHeapFactory : TObject
    {
        internal string name;

        protected int DescriptorSize;

        protected ID3D12Device6 NativeDevice;
        protected ID3D12DescriptorHeap CPUDescriptorHeap;
        protected ID3D12DescriptorHeap GPUDescriptorHeap;


        internal RHIDescriptorHeapFactory(ID3D12Device6 InNativeDevice, DescriptorHeapType InType, int DescriptorCount) : base()
        {
            NativeDevice = InNativeDevice;

            DescriptorSize = InNativeDevice.GetDescriptorHandleIncrementSize(DescriptorHeapType.DepthStencilView);

            DescriptorHeapDescription CPUDescriptorHeapDescription = new DescriptorHeapDescription(InType, DescriptorCount, DescriptorHeapFlags.ShaderVisible);
            CPUDescriptorHeap = InNativeDevice.CreateDescriptorHeap(CPUDescriptorHeapDescription);

            DescriptorHeapDescription GPUDescriptorHeapDescription = new DescriptorHeapDescription(InType, DescriptorCount, DescriptorHeapFlags.None);
            GPUDescriptorHeap = InNativeDevice.CreateDescriptorHeap(GPUDescriptorHeapDescription);
        }

        protected static DescriptorHeapType GetDescriptorType(EDescriptorType DescriptorType)
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

        internal int Allocator(int Count)
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

        protected override void DisposeManaged()
        {

        }

        protected override void DisposeUnManaged()
        {
            CPUDescriptorHeap.Release();
            CPUDescriptorHeap.Dispose();

            GPUDescriptorHeap.Release();
            GPUDescriptorHeap.Dispose();
        }
    }
}
