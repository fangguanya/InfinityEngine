using System;
using TerraFX.Interop.DirectX;
using InfinityEngine.Core.Memory;
using System.Runtime.CompilerServices;
using TerraFX.Interop.Windows;

namespace InfinityEngine.Graphics.RHI.D3D
{
    public unsafe class FD3DBuffer : FRHIBuffer
    {
        internal ID3D12Resource* uploadResource;
        internal ID3D12Resource* defaultResource;
        internal ID3D12Resource* readbackResource;

        internal FD3DBuffer(FRHIDevice device, in FRHIBufferDescriptor descriptor) : base(device, descriptor)
        {
            this.descriptor = descriptor;
            FD3DDevice d3dDevice = (FD3DDevice)device;

            // GPUMemory
            if ((descriptor.flag & EUsageType.Static) == EUsageType.Static || (descriptor.flag & EUsageType.Dynamic) == EUsageType.Dynamic || (descriptor.flag & EUsageType.Default) == EUsageType.Default)
            {
                D3D12_HEAP_PROPERTIES defaultHeapProperties;
                {
                    defaultHeapProperties.Type = D3D12_HEAP_TYPE.D3D12_HEAP_TYPE_DEFAULT;
                    defaultHeapProperties.CPUPageProperty = D3D12_CPU_PAGE_PROPERTY.D3D12_CPU_PAGE_PROPERTY_UNKNOWN;
                    defaultHeapProperties.MemoryPoolPreference = D3D12_MEMORY_POOL.D3D12_MEMORY_POOL_UNKNOWN;
                    defaultHeapProperties.VisibleNodeMask = 1;
                    defaultHeapProperties.CreationNodeMask = 1;
                }
                D3D12_RESOURCE_DESC defaultResourceDesc;
                {
                    defaultResourceDesc.Dimension = D3D12_RESOURCE_DIMENSION.D3D12_RESOURCE_DIMENSION_BUFFER;
                    defaultResourceDesc.Alignment = 0;
                    defaultResourceDesc.Width = descriptor.stride * descriptor.count;
                    defaultResourceDesc.Height = 1;
                    defaultResourceDesc.DepthOrArraySize = 1;
                    defaultResourceDesc.MipLevels = 1;
                    defaultResourceDesc.Format = DXGI_FORMAT.DXGI_FORMAT_UNKNOWN;
                    defaultResourceDesc.SampleDesc.Count = 1;
                    defaultResourceDesc.SampleDesc.Quality = 0;
                    defaultResourceDesc.Flags = D3D12_RESOURCE_FLAGS.D3D12_RESOURCE_FLAG_NONE;
                    defaultResourceDesc.Layout = D3D12_TEXTURE_LAYOUT.D3D12_TEXTURE_LAYOUT_ROW_MAJOR;
                }

                ID3D12Resource* defaultPtr = null;
                d3dDevice.nativeDevice->CreateCommittedResource(&defaultHeapProperties, D3D12_HEAP_FLAGS.D3D12_HEAP_FLAG_NONE, &defaultResourceDesc, D3D12_RESOURCE_STATES.D3D12_RESOURCE_STATE_COMMON, null, Windows.__uuidof<ID3D12Resource>(), (void**)&defaultPtr);
                defaultResource = defaultPtr;
            }

            // UploadMemory
            if ((descriptor.flag & EUsageType.Static) == EUsageType.Static || (descriptor.flag & EUsageType.Dynamic) == EUsageType.Dynamic)
            {
                D3D12_HEAP_PROPERTIES uploadHeapProperties;
                {
                    uploadHeapProperties.Type = D3D12_HEAP_TYPE.D3D12_HEAP_TYPE_UPLOAD;
                    uploadHeapProperties.CPUPageProperty = D3D12_CPU_PAGE_PROPERTY.D3D12_CPU_PAGE_PROPERTY_UNKNOWN;
                    uploadHeapProperties.MemoryPoolPreference = D3D12_MEMORY_POOL.D3D12_MEMORY_POOL_UNKNOWN;
                    uploadHeapProperties.VisibleNodeMask = 1;
                    uploadHeapProperties.CreationNodeMask = 1;
                }
                D3D12_RESOURCE_DESC uploadResourceDesc;
                {
                    uploadResourceDesc.Dimension = D3D12_RESOURCE_DIMENSION.D3D12_RESOURCE_DIMENSION_BUFFER;
                    uploadResourceDesc.Alignment = 0;
                    uploadResourceDesc.Width = descriptor.stride * descriptor.count;
                    uploadResourceDesc.Height = 1;
                    uploadResourceDesc.DepthOrArraySize = 1;
                    uploadResourceDesc.MipLevels = 1;
                    uploadResourceDesc.Format = DXGI_FORMAT.DXGI_FORMAT_UNKNOWN;
                    uploadResourceDesc.SampleDesc.Count = 1;
                    uploadResourceDesc.SampleDesc.Quality = 0;
                    uploadResourceDesc.Flags = D3D12_RESOURCE_FLAGS.D3D12_RESOURCE_FLAG_NONE;
                    uploadResourceDesc.Layout = D3D12_TEXTURE_LAYOUT.D3D12_TEXTURE_LAYOUT_ROW_MAJOR;
                }

                ID3D12Resource* uploadPtr = null;
                d3dDevice.nativeDevice->CreateCommittedResource(&uploadHeapProperties, D3D12_HEAP_FLAGS.D3D12_HEAP_FLAG_NONE, &uploadResourceDesc, D3D12_RESOURCE_STATES.D3D12_RESOURCE_STATE_GENERIC_READ, null, Windows.__uuidof<ID3D12Resource>(), (void**)&uploadPtr);
                uploadResource = uploadPtr;
            }

            // ReadbackMemory
            if ((descriptor.flag & EUsageType.Staging) == EUsageType.Staging)
            {
                D3D12_HEAP_PROPERTIES readbackHeapProperties;
                {
                    readbackHeapProperties.Type = D3D12_HEAP_TYPE.D3D12_HEAP_TYPE_READBACK;
                    readbackHeapProperties.CPUPageProperty = D3D12_CPU_PAGE_PROPERTY.D3D12_CPU_PAGE_PROPERTY_UNKNOWN;
                    readbackHeapProperties.MemoryPoolPreference = D3D12_MEMORY_POOL.D3D12_MEMORY_POOL_UNKNOWN;
                    readbackHeapProperties.VisibleNodeMask = 1;
                    readbackHeapProperties.CreationNodeMask = 1;
                }
                D3D12_RESOURCE_DESC readbackResourceDesc;
                {
                    readbackResourceDesc.Dimension = D3D12_RESOURCE_DIMENSION.D3D12_RESOURCE_DIMENSION_BUFFER;
                    readbackResourceDesc.Alignment = 0;
                    readbackResourceDesc.Width = descriptor.stride * descriptor.count;
                    readbackResourceDesc.Height = 1;
                    readbackResourceDesc.DepthOrArraySize = 1;
                    readbackResourceDesc.MipLevels = 1;
                    readbackResourceDesc.Format = DXGI_FORMAT.DXGI_FORMAT_UNKNOWN;
                    readbackResourceDesc.SampleDesc.Count = 1;
                    readbackResourceDesc.SampleDesc.Quality = 0;
                    readbackResourceDesc.Flags = D3D12_RESOURCE_FLAGS.D3D12_RESOURCE_FLAG_NONE;
                    readbackResourceDesc.Layout = D3D12_TEXTURE_LAYOUT.D3D12_TEXTURE_LAYOUT_ROW_MAJOR;
                }

                ID3D12Resource* readbackPtr = null;
                d3dDevice.nativeDevice->CreateCommittedResource(&readbackHeapProperties, D3D12_HEAP_FLAGS.D3D12_HEAP_FLAG_NONE, &readbackResourceDesc, D3D12_RESOURCE_STATES.D3D12_RESOURCE_STATE_COPY_DEST, null, Windows.__uuidof<ID3D12Resource>(), (void**)&readbackPtr);
                readbackResource = readbackPtr;
            }
        }

        public override void SetData<T>(params T[] data) where T : struct
        {
            if ((descriptor.flag & EUsageType.Dynamic) == EUsageType.Dynamic)
            {
                void* uploadPtr = null;
                D3D12_RANGE range = new D3D12_RANGE(0, 0);
                uploadResource->Map(0, &range, &uploadPtr);
                data.AsSpan().CopyTo(new IntPtr(uploadPtr));
                uploadResource->Unmap(0, null);
            }
        }

        public override void SetData<T>(FRHICommandBuffer cmdBuffer, params T[] data) where T : struct
        {
            if ((descriptor.flag & EUsageType.Dynamic) == EUsageType.Dynamic)
            {
                void* uploadPtr = null;
                D3D12_RANGE range = new D3D12_RANGE(0, 0);
                uploadResource->Map(0, &range, &uploadPtr);
                data.AsSpan().CopyTo(new IntPtr(uploadPtr));
                uploadResource->Unmap(0, null);

                FD3DCommandBuffer d3dCmdBuffer = (FD3DCommandBuffer)cmdBuffer;
                D3D12_RESOURCE_BARRIER beforeBarrier = D3D12_RESOURCE_BARRIER.InitTransition(defaultResource, D3D12_RESOURCE_STATES.D3D12_RESOURCE_STATE_COMMON, D3D12_RESOURCE_STATES.D3D12_RESOURCE_STATE_COPY_DEST);
                D3D12_RESOURCE_BARRIER afterBarrier = D3D12_RESOURCE_BARRIER.InitTransition(defaultResource, D3D12_RESOURCE_STATES.D3D12_RESOURCE_STATE_COPY_DEST, D3D12_RESOURCE_STATES.D3D12_RESOURCE_STATE_COMMON);

                d3dCmdBuffer.nativeCmdList->ResourceBarrier(0, &beforeBarrier);
                d3dCmdBuffer.nativeCmdList->CopyBufferRegion(defaultResource, 0, uploadResource, 0, descriptor.count * (ulong)Unsafe.SizeOf<T>());
                d3dCmdBuffer.nativeCmdList->ResourceBarrier(0, &afterBarrier);
            }
        }

        public override void RequestUpload<T>(FRHICommandBuffer cmdBuffer) where T : struct
        {
            if ((descriptor.flag & EUsageType.Dynamic) == EUsageType.Dynamic)
            {
                FD3DCommandBuffer d3dCmdBuffer = (FD3DCommandBuffer)cmdBuffer;
                D3D12_RESOURCE_BARRIER beforeBarrier = D3D12_RESOURCE_BARRIER.InitTransition(defaultResource, D3D12_RESOURCE_STATES.D3D12_RESOURCE_STATE_COMMON, D3D12_RESOURCE_STATES.D3D12_RESOURCE_STATE_COPY_DEST);
                D3D12_RESOURCE_BARRIER afterBarrier = D3D12_RESOURCE_BARRIER.InitTransition(defaultResource, D3D12_RESOURCE_STATES.D3D12_RESOURCE_STATE_COPY_DEST, D3D12_RESOURCE_STATES.D3D12_RESOURCE_STATE_COMMON);

                d3dCmdBuffer.nativeCmdList->ResourceBarrier(0, &beforeBarrier);
                d3dCmdBuffer.nativeCmdList->CopyBufferRegion(defaultResource, 0, uploadResource, 0, descriptor.count * (ulong)Unsafe.SizeOf<T>());
                d3dCmdBuffer.nativeCmdList->ResourceBarrier(0, &afterBarrier);
            }
        }

        public override void GetData<T>(T[] data) where T : struct
        {
            if ((descriptor.flag & EUsageType.Staging) == EUsageType.Staging)
            {
                void* readbackPtr = null;
                D3D12_RANGE range = new D3D12_RANGE(0, 0);
                readbackResource->Map(0, &range, &readbackPtr);
                new IntPtr(readbackPtr).CopyTo(data.AsSpan());
                readbackResource->Unmap(0, null);
            }
        }

        public override void GetData<T>(FRHICommandBuffer cmdBuffer, T[] data) where T : struct
        {
            if ((descriptor.flag & EUsageType.Staging) == EUsageType.Staging)
            {
                FD3DCommandBuffer d3dCmdBuffer = (FD3DCommandBuffer)cmdBuffer;
                D3D12_RESOURCE_BARRIER beforeBarrier = D3D12_RESOURCE_BARRIER.InitTransition(defaultResource, D3D12_RESOURCE_STATES.D3D12_RESOURCE_STATE_COMMON, D3D12_RESOURCE_STATES.D3D12_RESOURCE_STATE_COPY_SOURCE);
                D3D12_RESOURCE_BARRIER afterBarrier = D3D12_RESOURCE_BARRIER.InitTransition(defaultResource, D3D12_RESOURCE_STATES.D3D12_RESOURCE_STATE_COPY_SOURCE, D3D12_RESOURCE_STATES.D3D12_RESOURCE_STATE_COMMON);

                d3dCmdBuffer.nativeCmdList->ResourceBarrier(0, &beforeBarrier);
                d3dCmdBuffer.nativeCmdList->CopyBufferRegion(readbackResource, 0, defaultResource, 0, descriptor.count * (ulong)Unsafe.SizeOf<T>());
                d3dCmdBuffer.nativeCmdList->ResourceBarrier(0, &afterBarrier);

                void* readbackPtr = null;
                D3D12_RANGE range = new D3D12_RANGE(0, 0);
                readbackResource->Map(0, &range, &readbackPtr);
                new IntPtr(readbackPtr).CopyTo(data.AsSpan());
                readbackResource->Unmap(0, null);
            }
        }

        public override void RequestReadback<T>(FRHICommandBuffer cmdBuffer) where T : struct
        {
            if ((descriptor.flag & EUsageType.Staging) == EUsageType.Staging)
            {
                FD3DCommandBuffer d3dCmdBuffer = (FD3DCommandBuffer)cmdBuffer;
                D3D12_RESOURCE_BARRIER beforeBarrier = D3D12_RESOURCE_BARRIER.InitTransition(defaultResource, D3D12_RESOURCE_STATES.D3D12_RESOURCE_STATE_COMMON, D3D12_RESOURCE_STATES.D3D12_RESOURCE_STATE_COPY_SOURCE);
                D3D12_RESOURCE_BARRIER afterBarrier = D3D12_RESOURCE_BARRIER.InitTransition(defaultResource, D3D12_RESOURCE_STATES.D3D12_RESOURCE_STATE_COPY_SOURCE, D3D12_RESOURCE_STATES.D3D12_RESOURCE_STATE_COMMON);

                d3dCmdBuffer.nativeCmdList->ResourceBarrier(0, &beforeBarrier);
                d3dCmdBuffer.nativeCmdList->CopyBufferRegion(readbackResource, 0, defaultResource, 0, descriptor.count * (ulong)Unsafe.SizeOf<T>());
                d3dCmdBuffer.nativeCmdList->ResourceBarrier(0, &afterBarrier);
            }
        }

        protected override void Release()
        {
            uploadResource->Release();
            defaultResource->Release();
            readbackResource->Release();
        }
    }

    /*internal static class FD3DTextureUtility
    {
        internal static Format GetNativeFormat(this EGraphicsFormat format)
        {
            switch (format)
            {
                case EGraphicsFormat.R8_SRGB:
                    return Format.R8_Typeless;
                case EGraphicsFormat.R8G8_SRGB:
                    return Format.R8G8_Typeless;
                case EGraphicsFormat.R8G8B8A8_SRGB:
                    return Format.R8G8B8A8_Typeless;
                case EGraphicsFormat.R8_UNorm:
                    return Format.R8_Typeless;
                case EGraphicsFormat.R8G8_UNorm:
                    return Format.R8G8_Typeless;
                case EGraphicsFormat.R8G8B8A8_UNorm:
                    return Format.R8G8B8A8_Typeless;
            }

            return Format.Unknown;
        }

        internal static ResourceDimension GetNativeDimension(this ETextureType type)
        {
            switch (type)
            {
                case ETextureType.Tex2DArray:
                    return ResourceDimension.Texture2D;
                case ETextureType.Tex3D:
                    return ResourceDimension.Texture3D;
                case ETextureType.TexCube:
                    return ResourceDimension.Texture2D;
                case ETextureType.TexCubeArray:
                    return ResourceDimension.Texture2D;
            }

            return ResourceDimension.Texture2D;
        }
    }*/

    public class FD3DTexture : FRHITexture
    {
        internal ID3D12Resource uploadResource;
        internal ID3D12Resource defaultResource;
        internal ID3D12Resource readbackResource;

        internal FD3DTexture(FRHIDevice device, in FRHITextureDescriptor descriptor) : base(device, descriptor)
        {
            /*this.descriptor = descriptor;
            FD3DDevice d3dDevice = (FD3DDevice)device;

            // GPUMemory
            if ((descriptor.flag & EUsageType.Static) == EUsageType.Static || (descriptor.flag & EUsageType.Dynamic) == EUsageType.Dynamic || (descriptor.flag & EUsageType.Default) == EUsageType.Default)
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
                    defaultResourceDesc.Dimension = descriptor.type.GetNativeDimension();
                    defaultResourceDesc.Alignment = 0;
                    defaultResourceDesc.Width = (ulong)descriptor.width;
                    defaultResourceDesc.Height = descriptor.height;
                    defaultResourceDesc.DepthOrArraySize = (ushort)descriptor.slices;
                    defaultResourceDesc.MipLevels = descriptor.mipLevel;
                    defaultResourceDesc.Format = descriptor.format.GetNativeFormat();
                    defaultResourceDesc.SampleDescription.Count = (int)descriptor.sample;
                    defaultResourceDesc.SampleDescription.Quality = 0;
                    defaultResourceDesc.Flags = ResourceFlags.None;
                    defaultResourceDesc.Layout = TextureLayout.Unknown;
                }
                defaultResource = d3dDevice.nativeDevice.CreateCommittedResource<ID3D12Resource>(defaultHeapProperties, HeapFlags.None, defaultResourceDesc, ResourceStates.Common, null);
            }

            // UploadMemory
            if ((descriptor.flag & EUsageType.Static) == EUsageType.Static || (descriptor.flag & EUsageType.Dynamic) == EUsageType.Dynamic)
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
                    uploadResourceDesc.Dimension = descriptor.type.GetNativeDimension();
                    uploadResourceDesc.Alignment = 0;
                    uploadResourceDesc.Width = (ulong)descriptor.width;
                    uploadResourceDesc.Height = descriptor.height;
                    uploadResourceDesc.DepthOrArraySize = (ushort)descriptor.slices;
                    uploadResourceDesc.MipLevels = descriptor.mipLevel;
                    uploadResourceDesc.Format = descriptor.format.GetNativeFormat();
                    uploadResourceDesc.SampleDescription.Count = (int)descriptor.sample;
                    uploadResourceDesc.SampleDescription.Quality = 0;
                    uploadResourceDesc.Flags = ResourceFlags.None;
                    uploadResourceDesc.Layout = TextureLayout.Unknown;
                }
                uploadResource = d3dDevice.nativeDevice.CreateCommittedResource<ID3D12Resource>(uploadHeapProperties, HeapFlags.None, uploadResourceDesc, ResourceStates.GenericRead, null);
            }

            // ReadbackMemory
            if ((descriptor.flag & EUsageType.Staging) == EUsageType.Staging)
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
                    readbackResourceDesc.Dimension = descriptor.type.GetNativeDimension();
                    readbackResourceDesc.Alignment = 0;
                    readbackResourceDesc.Width = (ulong)descriptor.width;
                    readbackResourceDesc.Height = descriptor.height;
                    readbackResourceDesc.DepthOrArraySize = (ushort)descriptor.slices;
                    readbackResourceDesc.MipLevels = descriptor.mipLevel;
                    readbackResourceDesc.Format = descriptor.format.GetNativeFormat();
                    readbackResourceDesc.SampleDescription.Count = (int)descriptor.sample;
                    readbackResourceDesc.SampleDescription.Quality = 0;
                    readbackResourceDesc.Flags = ResourceFlags.None;
                    readbackResourceDesc.Layout = TextureLayout.Unknown;
                }
                readbackResource = d3dDevice.nativeDevice.CreateCommittedResource<ID3D12Resource>(readbackHeapProperties, HeapFlags.None, readbackResourceDesc, ResourceStates.CopyDestination, null);
            }*/
        }

        protected override void Release()
        {
            /*uploadResource?.Dispose();
            defaultResource?.Dispose();
            readbackResource?.Dispose();*/
        }
    }
}
