using System;
using Vortice.DXGI;
using Vortice.Direct3D12;
using InfinityEngine.Core.Object;
using InfinityEngine.Core.Memory;
using InfinityEngine.Core.Mathmatics;
using System.Runtime.CompilerServices;

namespace InfinityEngine.Graphics.RHI
{
    public class FD3DBuffer : FRHIBuffer
    {
        internal ID3D12Resource uploadResource;
        internal ID3D12Resource defaultResource;
        internal ID3D12Resource readbackResource;

        internal FD3DBuffer(FRHIDevice device, in FRHIBufferDescription description) : base(device, description)
        {
            this.description = description;
            FD3DDevice d3dDevice = (FD3DDevice)device;

            // GPUMemory
            if ((description.flag & EUsageType.Static) == EUsageType.Static || (description.flag & EUsageType.Dynamic) == EUsageType.Dynamic || (description.flag & EUsageType.Default) == EUsageType.Default)
            {
                HeapProperties defaultHeapProperties;
                {
                    defaultHeapProperties.Type = HeapType.Default;
                    defaultHeapProperties.CPUPageProperty = CpuPageProperty.Unknown;
                    defaultHeapProperties.MemoryPoolPreference = MemoryPool.Unknown;
                    defaultHeapProperties.VisibleNodeMask = 1;
                    defaultHeapProperties.CreationNodeMask = 1;
                }
                ResourceDescription defaultResourceDesc;
                {
                    defaultResourceDesc.Dimension = ResourceDimension.Buffer;
                    defaultResourceDesc.Alignment = 0;
                    defaultResourceDesc.Width = description.stride * description.count;
                    defaultResourceDesc.Height = 1;
                    defaultResourceDesc.DepthOrArraySize = 1;
                    defaultResourceDesc.MipLevels = 1;
                    defaultResourceDesc.Format = Format.Unknown;
                    defaultResourceDesc.SampleDescription.Count = 1;
                    defaultResourceDesc.SampleDescription.Quality = 0;
                    defaultResourceDesc.Flags = ResourceFlags.None;
                    defaultResourceDesc.Layout = TextureLayout.RowMajor;
                }
                defaultResource = d3dDevice.nativeDevice.CreateCommittedResource<ID3D12Resource>(defaultHeapProperties, HeapFlags.None, defaultResourceDesc, ResourceStates.Common, null);
            }

            // UploadMemory
            if ((description.flag & EUsageType.Static) == EUsageType.Static || (description.flag & EUsageType.Dynamic) == EUsageType.Dynamic)
            {
                HeapProperties uploadHeapProperties;
                {
                    uploadHeapProperties.Type = HeapType.Upload;
                    uploadHeapProperties.CPUPageProperty = CpuPageProperty.Unknown;
                    uploadHeapProperties.MemoryPoolPreference = MemoryPool.Unknown;
                    uploadHeapProperties.VisibleNodeMask = 1;
                    uploadHeapProperties.CreationNodeMask = 1;
                }
                ResourceDescription uploadResourceDesc;
                {
                    uploadResourceDesc.Dimension = ResourceDimension.Buffer;
                    uploadResourceDesc.Alignment = 0;
                    uploadResourceDesc.Width = description.stride * description.count;
                    uploadResourceDesc.Height = 1;
                    uploadResourceDesc.DepthOrArraySize = 1;
                    uploadResourceDesc.MipLevels = 1;
                    uploadResourceDesc.Format = Format.Unknown;
                    uploadResourceDesc.SampleDescription.Count = 1;
                    uploadResourceDesc.SampleDescription.Quality = 0;
                    uploadResourceDesc.Flags = ResourceFlags.None;
                    uploadResourceDesc.Layout = TextureLayout.RowMajor;
                }
                uploadResource = d3dDevice.nativeDevice.CreateCommittedResource<ID3D12Resource>(uploadHeapProperties, HeapFlags.None, uploadResourceDesc, ResourceStates.GenericRead, null);
            }

            // ReadbackMemory
            if ((description.flag & EUsageType.Staging) == EUsageType.Staging)
            {
                HeapProperties readbackHeapProperties;
                {
                    readbackHeapProperties.Type = HeapType.Readback;
                    readbackHeapProperties.CPUPageProperty = CpuPageProperty.Unknown;
                    readbackHeapProperties.MemoryPoolPreference = MemoryPool.Unknown;
                    readbackHeapProperties.VisibleNodeMask = 1;
                    readbackHeapProperties.CreationNodeMask = 1;
                }
                ResourceDescription readbackResourceDesc;
                {
                    readbackResourceDesc.Dimension = ResourceDimension.Buffer;
                    readbackResourceDesc.Alignment = 0;
                    readbackResourceDesc.Width = description.stride * description.count;
                    readbackResourceDesc.Height = 1;
                    readbackResourceDesc.DepthOrArraySize = 1;
                    readbackResourceDesc.MipLevels = 1;
                    readbackResourceDesc.Format = Format.Unknown;
                    readbackResourceDesc.SampleDescription.Count = 1;
                    readbackResourceDesc.SampleDescription.Quality = 0;
                    readbackResourceDesc.Flags = ResourceFlags.None;
                    readbackResourceDesc.Layout = TextureLayout.RowMajor;
                }
                readbackResource = d3dDevice.nativeDevice.CreateCommittedResource<ID3D12Resource>(readbackHeapProperties, HeapFlags.None, readbackResourceDesc, ResourceStates.CopyDestination, null);
            }
        }

        public override void SetData<T>(params T[] data) where T : struct
        {
            if ((description.flag & EUsageType.Dynamic) == EUsageType.Dynamic)
            {
                IntPtr uploadResourcePtr = uploadResource.Map(0);
                data.AsSpan().CopyTo(uploadResourcePtr);
                uploadResource.Unmap(0);
            }
        }

        public override void SetData<T>(FRHICommandList cmdList, params T[] data) where T : struct
        {
            if ((description.flag & EUsageType.Dynamic) == EUsageType.Dynamic)
            {
                IntPtr uploadResourcePtr = uploadResource.Map(0);
                data.AsSpan().CopyTo(uploadResourcePtr);
                uploadResource.Unmap(0);

                FD3DCommandList d3dCmdList = (FD3DCommandList)cmdList;
                d3dCmdList.nativeCmdList.ResourceBarrierTransition(defaultResource, ResourceStates.Common, ResourceStates.CopyDestination);
                d3dCmdList.nativeCmdList.CopyBufferRegion(defaultResource, 0, uploadResource, 0, description.count * (ulong)Unsafe.SizeOf<T>());
                d3dCmdList.nativeCmdList.ResourceBarrierTransition(defaultResource, ResourceStates.CopyDestination, ResourceStates.Common);
            }
        }

        public override void RequestUpload<T>(FRHICommandList cmdList) where T : struct
        {
            if ((description.flag & EUsageType.Dynamic) == EUsageType.Dynamic)
            {
                FD3DCommandList d3dCmdList = (FD3DCommandList)cmdList;
                d3dCmdList.nativeCmdList.ResourceBarrierTransition(defaultResource, ResourceStates.Common, ResourceStates.CopyDestination);
                d3dCmdList.nativeCmdList.CopyBufferRegion(defaultResource, 0, uploadResource, 0, description.count * (ulong)Unsafe.SizeOf<T>());
                d3dCmdList.nativeCmdList.ResourceBarrierTransition(defaultResource, ResourceStates.CopyDestination, ResourceStates.Common);
            }
        }

        public override void GetData<T>(T[] data) where T : struct
        {
            if ((description.flag & EUsageType.Staging) == EUsageType.Staging)
            {
                //Because current frame read-back copy cmd is not execute on GPU, so this will get last frame data
                IntPtr readbackResourcePtr = readbackResource.Map(0);
                readbackResourcePtr.CopyTo(data.AsSpan());
                readbackResource.Unmap(0);
            }
        }

        public override void GetData<T>(FRHICommandList cmdList, T[] data) where T : struct
        {
            if ((description.flag & EUsageType.Staging) == EUsageType.Staging)
            {
                FD3DCommandList d3dCmdList = (FD3DCommandList)cmdList;
                d3dCmdList.nativeCmdList.ResourceBarrierTransition(defaultResource, ResourceStates.Common, ResourceStates.CopySource);
                d3dCmdList.nativeCmdList.CopyBufferRegion(readbackResource, 0, defaultResource, 0, description.count * (ulong)Unsafe.SizeOf<T>());
                d3dCmdList.nativeCmdList.ResourceBarrierTransition(defaultResource, ResourceStates.CopySource, ResourceStates.Common);

                //Because current frame read-back copy cmd is not execute on GPU, so this will get last frame data
                IntPtr readbackResourcePtr = readbackResource.Map(0);
                readbackResourcePtr.CopyTo(data.AsSpan());
                readbackResource.Unmap(0);
            }
        }

        public override void RequestReadback<T>(FRHICommandList cmdList) where T : struct
        {
            if ((description.flag & EUsageType.Staging) == EUsageType.Staging)
            {
                FD3DCommandList d3dCmdList = (FD3DCommandList)cmdList;
                d3dCmdList.nativeCmdList.ResourceBarrierTransition(defaultResource, ResourceStates.Common, ResourceStates.CopySource);
                d3dCmdList.nativeCmdList.CopyBufferRegion(readbackResource, 0, defaultResource, 0, description.count * (ulong)Unsafe.SizeOf<T>());
                //cmdList.nativeCmdList.ResourceBarrierTransition(defaultResource, ResourceStates.CopySource, ResourceStates.Common);
            }
        }

        protected override void Release()
        {
            uploadResource?.Dispose();
            defaultResource?.Dispose();
            readbackResource?.Dispose();
        }
    }

    public class FD3DTexture : FRHITexture
    {
        internal ID3D12Resource uploadResource;
        internal ID3D12Resource defaultResource;
        internal ID3D12Resource readbackResource;

        internal FD3DTexture(FRHIDevice device, in FRHITextureDescription description) : base(device, description)
        {
            this.description = description;
            FD3DDevice d3dDevice = (FD3DDevice)device;

            // GPUMemory
            if ((description.flag & EUsageType.Static) == EUsageType.Static || (description.flag & EUsageType.Dynamic) == EUsageType.Dynamic || (description.flag & EUsageType.Default) == EUsageType.Default)
            {
                HeapProperties defaultHeapProperties;
                {
                    defaultHeapProperties.Type = HeapType.Default;
                    defaultHeapProperties.CPUPageProperty = CpuPageProperty.Unknown;
                    defaultHeapProperties.MemoryPoolPreference = MemoryPool.Unknown;
                    defaultHeapProperties.VisibleNodeMask = 1;
                    defaultHeapProperties.CreationNodeMask = 1;
                }
                ResourceDescription defaultResourceDesc;
                {
                    defaultResourceDesc.Dimension = description.type.GetNativeDimension();
                    defaultResourceDesc.Alignment = 0;
                    defaultResourceDesc.Width = (ulong)description.width;
                    defaultResourceDesc.Height = description.height;
                    defaultResourceDesc.DepthOrArraySize = (ushort)description.slices;
                    defaultResourceDesc.MipLevels = description.mipLevel;
                    defaultResourceDesc.Format = description.format.GetNativeFormat();
                    defaultResourceDesc.SampleDescription.Count = (int)description.sample;
                    defaultResourceDesc.SampleDescription.Quality = 0;
                    defaultResourceDesc.Flags = ResourceFlags.None;
                    defaultResourceDesc.Layout = TextureLayout.Unknown;
                }
                defaultResource = d3dDevice.nativeDevice.CreateCommittedResource<ID3D12Resource>(defaultHeapProperties, HeapFlags.None, defaultResourceDesc, ResourceStates.Common, null);
            }

            // UploadMemory
            if ((description.flag & EUsageType.Static) == EUsageType.Static || (description.flag & EUsageType.Dynamic) == EUsageType.Dynamic)
            {
                HeapProperties uploadHeapProperties;
                {
                    uploadHeapProperties.Type = HeapType.Upload;
                    uploadHeapProperties.CPUPageProperty = CpuPageProperty.Unknown;
                    uploadHeapProperties.MemoryPoolPreference = MemoryPool.Unknown;
                    uploadHeapProperties.VisibleNodeMask = 1;
                    uploadHeapProperties.CreationNodeMask = 1;
                }
                ResourceDescription uploadResourceDesc;
                {
                    uploadResourceDesc.Dimension = description.type.GetNativeDimension();
                    uploadResourceDesc.Alignment = 0;
                    uploadResourceDesc.Width = (ulong)description.width;
                    uploadResourceDesc.Height = description.height;
                    uploadResourceDesc.DepthOrArraySize = (ushort)description.slices;
                    uploadResourceDesc.MipLevels = description.mipLevel;
                    uploadResourceDesc.Format = description.format.GetNativeFormat();
                    uploadResourceDesc.SampleDescription.Count = (int)description.sample;
                    uploadResourceDesc.SampleDescription.Quality = 0;
                    uploadResourceDesc.Flags = ResourceFlags.None;
                    uploadResourceDesc.Layout = TextureLayout.Unknown;
                }
                uploadResource = d3dDevice.nativeDevice.CreateCommittedResource<ID3D12Resource>(uploadHeapProperties, HeapFlags.None, uploadResourceDesc, ResourceStates.GenericRead, null);
            }

            // ReadbackMemory
            if ((description.flag & EUsageType.Staging) == EUsageType.Staging)
            {
                HeapProperties readbackHeapProperties;
                {
                    readbackHeapProperties.Type = HeapType.Readback;
                    readbackHeapProperties.CPUPageProperty = CpuPageProperty.Unknown;
                    readbackHeapProperties.MemoryPoolPreference = MemoryPool.Unknown;
                    readbackHeapProperties.VisibleNodeMask = 1;
                    readbackHeapProperties.CreationNodeMask = 1;
                }
                ResourceDescription readbackResourceDesc;
                {
                    readbackResourceDesc.Dimension = description.type.GetNativeDimension();
                    readbackResourceDesc.Alignment = 0;
                    readbackResourceDesc.Width = (ulong)description.width;
                    readbackResourceDesc.Height = description.height;
                    readbackResourceDesc.DepthOrArraySize = (ushort)description.slices;
                    readbackResourceDesc.MipLevels = description.mipLevel;
                    readbackResourceDesc.Format = description.format.GetNativeFormat();
                    readbackResourceDesc.SampleDescription.Count = (int)description.sample;
                    readbackResourceDesc.SampleDescription.Quality = 0;
                    readbackResourceDesc.Flags = ResourceFlags.None;
                    readbackResourceDesc.Layout = TextureLayout.Unknown;
                }
                readbackResource = d3dDevice.nativeDevice.CreateCommittedResource<ID3D12Resource>(readbackHeapProperties, HeapFlags.None, readbackResourceDesc, ResourceStates.CopyDestination, null);
            }
        }

        protected override void Release()
        {
            uploadResource?.Dispose();
            defaultResource?.Dispose();
            readbackResource?.Dispose();
        }
    }
}
