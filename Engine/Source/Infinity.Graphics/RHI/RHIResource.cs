using System;
using Vortice.DXGI;
using Vortice.Direct3D12;
using InfinityEngine.Core.Object;
using System.Runtime.CompilerServices;
using InfinityEngine.Core.Native.Utility;

namespace InfinityEngine.Graphics.RHI
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


    public class FRHIResource : UObject
    {
        protected EUseFlag UseFlag;
        protected Format GraphicsFormat;
        protected EResourceType ResourceType;

        internal ID3D12Resource UploadResource;
        internal ID3D12Resource DefaultResource;
        internal ID3D12Resource ReadbackResource;


        public FRHIResource(EUseFlag InUseFlag) : base()
        {
            UseFlag = InUseFlag;
        }

        protected override void Disposed()
        {
            UploadResource?.Dispose();
            DefaultResource?.Dispose();
            ReadbackResource?.Dispose();
        }
    }

    public class FRHIBuffer : FRHIResource
    {
        internal ulong Count;
        internal ulong Stride;
        internal EBufferType BufferType;

        internal FRHIBuffer(ID3D12Device6 NativeDevice, EUseFlag InUseFlag, EBufferType InBufferType, ulong InCount, ulong InStride) : base(InUseFlag)
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
                DefaultResourceDesc.Width = Stride * Count;
                DefaultResourceDesc.Height = 1;
                DefaultResourceDesc.DepthOrArraySize = 1;
                DefaultResourceDesc.MipLevels = 1;
                DefaultResourceDesc.Format = Format.Unknown;
                DefaultResourceDesc.SampleDescription.Count = 1;
                DefaultResourceDesc.SampleDescription.Quality = 0;
                DefaultResourceDesc.Layout = TextureLayout.RowMajor;
                DefaultResourceDesc.Flags = ResourceFlags.None;
            }
            DefaultResource = NativeDevice.CreateCommittedResource<ID3D12Resource>(DefaultHeapProperty, HeapFlags.None, DefaultResourceDesc, ResourceStates.Common, null);

            // CPUMemory
            if (UseFlag == EUseFlag.CPUWrite || UseFlag == EUseFlag.CPURW)
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
                    UploadResourceDesc.Width = Stride * Count;
                    UploadResourceDesc.Height = 1;
                    UploadResourceDesc.DepthOrArraySize = 1;
                    UploadResourceDesc.MipLevels = 1;
                    UploadResourceDesc.Format = Format.Unknown;
                    UploadResourceDesc.SampleDescription.Count = 1;
                    UploadResourceDesc.SampleDescription.Quality = 0;
                    UploadResourceDesc.Layout = TextureLayout.RowMajor;
                    UploadResourceDesc.Flags = ResourceFlags.None;
                }
                UploadResource = NativeDevice.CreateCommittedResource<ID3D12Resource>(UploadHeapProperty, HeapFlags.None, UploadResourceDesc, ResourceStates.GenericRead, null);
            }

            // Readback
            if (UseFlag == EUseFlag.CPURead || UseFlag == EUseFlag.CPURW)
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
                    ReadbackResourceDesc.Width = Stride * Count;
                    ReadbackResourceDesc.Height = 1;
                    ReadbackResourceDesc.DepthOrArraySize = 1;
                    ReadbackResourceDesc.MipLevels = 1;
                    ReadbackResourceDesc.Format = Format.Unknown;
                    ReadbackResourceDesc.SampleDescription.Count = 1;
                    ReadbackResourceDesc.SampleDescription.Quality = 0;
                    ReadbackResourceDesc.Layout = TextureLayout.RowMajor;
                    ReadbackResourceDesc.Flags = ResourceFlags.None;
                }
                ReadbackResource = NativeDevice.CreateCommittedResource<ID3D12Resource>(ReadbackHeapProperties, HeapFlags.None, ReadbackResourceDesc, ResourceStates.CopyDestination, null);
            }
        }

        public void GetData<T>(ID3D12GraphicsCommandList5 NativeCopyList, T[] Data) where T : struct
        {
            if (UseFlag == EUseFlag.CPURead || UseFlag == EUseFlag.CPURW)
            {
                NativeCopyList.ResourceBarrierTransition(DefaultResource, ResourceStates.Common, ResourceStates.CopySource);
                NativeCopyList.CopyBufferRegion(ReadbackResource, 0, DefaultResource, 0, (ulong)Data.Length * (ulong)Unsafe.SizeOf<T>());
                NativeCopyList.ResourceBarrierTransition(DefaultResource, ResourceStates.CopySource, ResourceStates.Common);

                IntPtr ReadbackResourcePtr = ReadbackResource.Map(0);
                ReadbackResourcePtr.CopyTo(Data.AsSpan());
                ReadbackResource.Unmap(0);
            }
        }

        public void SetData<T>(ID3D12GraphicsCommandList5 NativeCopyList, params T[] Data) where T : struct
        {
            if (UseFlag == EUseFlag.CPUWrite || UseFlag == EUseFlag.CPURW) 
            {
                IntPtr UploadResourcePtr = UploadResource.Map(0);
                Data.AsSpan().CopyTo(UploadResourcePtr);
                UploadResource.Unmap(0);

                NativeCopyList.ResourceBarrierTransition(DefaultResource, ResourceStates.Common, ResourceStates.CopyDestination);
                NativeCopyList.CopyBufferRegion(DefaultResource, 0, UploadResource, 0, (ulong)Data.Length * (ulong)Unsafe.SizeOf<T>());
                NativeCopyList.ResourceBarrierTransition(DefaultResource, ResourceStates.CopyDestination, ResourceStates.Common);
            }
        }

        protected override void Disposed()
        {
            base.Disposed();
        }
    }

    public class FRHITexture : FRHIResource
    {
        internal ETextureType TextureType;

        public FRHITexture(ID3D12Device6 NativeDevice, EUseFlag InUseFlag, ETextureType InTextureType) : base(InUseFlag)
        {
            TextureType = InTextureType;
            ResourceType = EResourceType.Texture;
        }

        protected override void Disposed()
        {
            base.Disposed();
        }
    }

    public class FRHIResourceViewRange : UObject
    {
        protected int RangeSize;
        protected int DescriptorIndex;

        protected ID3D12Device6 NativeDevice;
        protected CpuDescriptorHandle DescriptorHandle;


        internal FRHIResourceViewRange(ID3D12Device6 InNativeDevice, FRHIDescriptorHeapFactory DescriptorHeapFactory, int DescriptorLength) : base()
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

        public void SetConstantBufferView(int Index, FRHIConstantBufferView ConstantBufferView)
        {
            NativeDevice.CopyDescriptorsSimple(1, GetDescriptorHandle(Index), ConstantBufferView.GetDescriptorHandle(), DescriptorHeapType.ConstantBufferViewShaderResourceViewUnorderedAccessView);
        }

        public void SetShaderResourceView(int Index, FRHIShaderResourceView ShaderResourceView)
        {
            NativeDevice.CopyDescriptorsSimple(1, GetDescriptorHandle(Index), ShaderResourceView.GetDescriptorHandle(), DescriptorHeapType.ConstantBufferViewShaderResourceViewUnorderedAccessView);
        }

        public void SetUnorderedAccessView(int Index, FRHIUnorderedAccessView UnorderedAccessView)
        {
            NativeDevice.CopyDescriptorsSimple(1, GetDescriptorHandle(Index), UnorderedAccessView.GetDescriptorHandle(), DescriptorHeapType.ConstantBufferViewShaderResourceViewUnorderedAccessView);
        }

        protected override void Disposed()
        {

        }
    }

    internal sealed class FRHIMemoryHeapFactory : UObject
    {
        internal FRHIMemoryHeapFactory(ID3D12Device6 InNativeDevice, int HeapCount) : base()
        {

        }

        protected override void Disposed()
        {

        }
    }

    internal class FRHIDescriptorHeapFactory : UObject
    {
        protected int DescriptorSize;

        protected ID3D12Device6 NativeDevice;
        protected ID3D12DescriptorHeap CPUDescriptorHeap;
        protected ID3D12DescriptorHeap GPUDescriptorHeap;


        internal FRHIDescriptorHeapFactory(ID3D12Device6 InNativeDevice, DescriptorHeapType InType, int DescriptorCount) : base()
        {
            NativeDevice = InNativeDevice;

            DescriptorSize = InNativeDevice.GetDescriptorHandleIncrementSize(DescriptorHeapType.DepthStencilView);

            DescriptorHeapDescription CPUDescriptorHeapDescription = new DescriptorHeapDescription(InType, DescriptorCount, DescriptorHeapFlags.ShaderVisible);
            CPUDescriptorHeap = InNativeDevice.CreateDescriptorHeap<ID3D12DescriptorHeap>(CPUDescriptorHeapDescription);

            DescriptorHeapDescription GPUDescriptorHeapDescription = new DescriptorHeapDescription(InType, DescriptorCount, DescriptorHeapFlags.None);
            GPUDescriptorHeap = InNativeDevice.CreateDescriptorHeap<ID3D12DescriptorHeap>(GPUDescriptorHeapDescription);
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

        protected override void Disposed()
        {
            CPUDescriptorHeap?.Dispose();
            GPUDescriptorHeap?.Dispose();
        }
    }
}
