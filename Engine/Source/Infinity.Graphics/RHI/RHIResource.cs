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

    public enum EResourceType
    {
        Buffer,
        Texture,
    };

    public class FRHIResource : UObject
    {
        protected EUseFlag useFlag;
        protected Format graphicsFormat;
        protected EResourceType resourceType;
        internal ID3D12Resource uploadResource;
        internal ID3D12Resource defaultResource;
        internal ID3D12Resource readbackResource;


        public FRHIResource(in EUseFlag useFlag) : base()
        {
            this.useFlag = useFlag;
        }

        protected override void Disposed()
        {
            uploadResource?.Dispose();
            defaultResource?.Dispose();
            readbackResource?.Dispose();
        }
    }

    public class FRHIBuffer : FRHIResource
    {
        internal ulong count;
        internal ulong stride;
        internal EBufferType bufferType;


        internal FRHIBuffer(ID3D12Device6 d3D12Device, in EUseFlag useFlag, in EBufferType bufferType, in ulong count, in ulong stride) : base(useFlag)
        {
            this.count = count;
            this.stride = stride;
            this.bufferType = bufferType;
            this.resourceType = EResourceType.Buffer;

            // GPUMemory
            HeapProperties defaultHeapProperty;
            {
                defaultHeapProperty.Type = HeapType.Default;
                defaultHeapProperty.CPUPageProperty = CpuPageProperty.Unknown;
                defaultHeapProperty.MemoryPoolPreference = MemoryPool.Unknown;
                defaultHeapProperty.CreationNodeMask = 1;
                defaultHeapProperty.VisibleNodeMask = 1;
            }
            ResourceDescription defaultResourceDesc;
            {
                defaultResourceDesc.Dimension = ResourceDimension.Buffer;
                defaultResourceDesc.Alignment = 0;
                defaultResourceDesc.Width = stride * count;
                defaultResourceDesc.Height = 1;
                defaultResourceDesc.DepthOrArraySize = 1;
                defaultResourceDesc.MipLevels = 1;
                defaultResourceDesc.Format = Format.Unknown;
                defaultResourceDesc.SampleDescription.Count = 1;
                defaultResourceDesc.SampleDescription.Quality = 0;
                defaultResourceDesc.Layout = TextureLayout.RowMajor;
                defaultResourceDesc.Flags = ResourceFlags.None;
            }
            defaultResource = d3D12Device.CreateCommittedResource<ID3D12Resource>(defaultHeapProperty, HeapFlags.None, defaultResourceDesc, ResourceStates.Common, null);

            // CPUMemory
            if (useFlag == EUseFlag.CPUWrite || useFlag == EUseFlag.CPURW)
            {
                HeapProperties uploadHeapProperty;
                {
                    uploadHeapProperty.Type = HeapType.Upload;
                    uploadHeapProperty.CPUPageProperty = CpuPageProperty.Unknown;
                    uploadHeapProperty.MemoryPoolPreference = MemoryPool.Unknown;
                    uploadHeapProperty.CreationNodeMask = 1;
                    uploadHeapProperty.VisibleNodeMask = 1;
                }
                ResourceDescription uploadResourceDesc;
                {
                    uploadResourceDesc.Dimension = ResourceDimension.Buffer;
                    uploadResourceDesc.Alignment = 0;
                    uploadResourceDesc.Width = stride * count;
                    uploadResourceDesc.Height = 1;
                    uploadResourceDesc.DepthOrArraySize = 1;
                    uploadResourceDesc.MipLevels = 1;
                    uploadResourceDesc.Format = Format.Unknown;
                    uploadResourceDesc.SampleDescription.Count = 1;
                    uploadResourceDesc.SampleDescription.Quality = 0;
                    uploadResourceDesc.Layout = TextureLayout.RowMajor;
                    uploadResourceDesc.Flags = ResourceFlags.None;
                }
                uploadResource = d3D12Device.CreateCommittedResource<ID3D12Resource>(uploadHeapProperty, HeapFlags.None, uploadResourceDesc, ResourceStates.GenericRead, null);
            }

            // Readback
            if (useFlag == EUseFlag.CPURead || useFlag == EUseFlag.CPURW)
            {
                HeapProperties readbackHeapProperties;
                {
                    readbackHeapProperties.Type = HeapType.Readback;
                    readbackHeapProperties.CPUPageProperty = CpuPageProperty.Unknown;
                    readbackHeapProperties.MemoryPoolPreference = MemoryPool.Unknown;
                    readbackHeapProperties.CreationNodeMask = 1;
                    readbackHeapProperties.VisibleNodeMask = 1;
                }
                ResourceDescription readbackResourceDesc;
                {
                    readbackResourceDesc.Dimension = ResourceDimension.Buffer;
                    readbackResourceDesc.Alignment = 0;
                    readbackResourceDesc.Width = stride * count;
                    readbackResourceDesc.Height = 1;
                    readbackResourceDesc.DepthOrArraySize = 1;
                    readbackResourceDesc.MipLevels = 1;
                    readbackResourceDesc.Format = Format.Unknown;
                    readbackResourceDesc.SampleDescription.Count = 1;
                    readbackResourceDesc.SampleDescription.Quality = 0;
                    readbackResourceDesc.Layout = TextureLayout.RowMajor;
                    readbackResourceDesc.Flags = ResourceFlags.None;
                }
                readbackResource = d3D12Device.CreateCommittedResource<ID3D12Resource>(readbackHeapProperties, HeapFlags.None, readbackResourceDesc, ResourceStates.CopyDestination, null);
            }
        }

        public void GetData<T>(ID3D12GraphicsCommandList5 NativeCopyList, T[] Data) where T : struct
        {
            if (useFlag == EUseFlag.CPURead || useFlag == EUseFlag.CPURW)
            {
                NativeCopyList.ResourceBarrierTransition(defaultResource, ResourceStates.Common, ResourceStates.CopySource);
                NativeCopyList.CopyBufferRegion(readbackResource, 0, defaultResource, 0, (ulong)Data.Length * (ulong)Unsafe.SizeOf<T>());
                NativeCopyList.ResourceBarrierTransition(defaultResource, ResourceStates.CopySource, ResourceStates.Common);

                //Because current frame read-back copy cmd is not execute on GPU, so this will get last frame data
                IntPtr readbackResourcePtr = readbackResource.Map(0);
                readbackResourcePtr.CopyTo(Data.AsSpan());
                readbackResource.Unmap(0);
            }
        }

        public void SetData<T>(ID3D12GraphicsCommandList5 NativeCopyList, params T[] Data) where T : struct
        {
            if (useFlag == EUseFlag.CPUWrite || useFlag == EUseFlag.CPURW) 
            {
                IntPtr uploadResourcePtr = uploadResource.Map(0);
                Data.AsSpan().CopyTo(uploadResourcePtr);
                uploadResource.Unmap(0);

                NativeCopyList.ResourceBarrierTransition(defaultResource, ResourceStates.Common, ResourceStates.CopyDestination);
                NativeCopyList.CopyBufferRegion(defaultResource, 0, uploadResource, 0, (ulong)Data.Length * (ulong)Unsafe.SizeOf<T>());
                NativeCopyList.ResourceBarrierTransition(defaultResource, ResourceStates.CopyDestination, ResourceStates.Common);
            }
        }

        protected override void Disposed()
        {
            base.Disposed();
        }
    }

    public class FRHITexture : FRHIResource
    {
        internal ETextureType textureType;

        public FRHITexture(ID3D12Device6 d3D12Device, in EUseFlag useFlag, in ETextureType textureType) : base(useFlag)
        {
            this.textureType = textureType;
            this.resourceType = EResourceType.Texture;
        }

        protected override void Disposed()
        {
            base.Disposed();
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
