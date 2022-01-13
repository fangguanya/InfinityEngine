using System;
using TerraFX.Interop.Windows;
using TerraFX.Interop.DirectX;
using InfinityEngine.Core.Memory;
using System.Runtime.CompilerServices;

namespace InfinityEngine.Graphics.RHI.D3D
{
    internal static class FD3DTextureUtility
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static DXGI_FORMAT GetNativeFormat(this EGraphicsFormat format)
        {
            switch (format)
            {
                case EGraphicsFormat.R8_SRGB:
                    return DXGI_FORMAT.DXGI_FORMAT_R8_TYPELESS;

                case EGraphicsFormat.R8G8_SRGB:
                    return DXGI_FORMAT.DXGI_FORMAT_R8G8_TYPELESS;

                case EGraphicsFormat.R8G8B8A8_SRGB:
                    return DXGI_FORMAT.DXGI_FORMAT_R8G8B8A8_UNORM_SRGB;

                case EGraphicsFormat.R8_UNorm:
                    return DXGI_FORMAT.DXGI_FORMAT_R8_TYPELESS;

                case EGraphicsFormat.R8G8_UNorm:
                    return DXGI_FORMAT.DXGI_FORMAT_R8G8_TYPELESS;

                case EGraphicsFormat.R8G8B8A8_UNorm:
                    return DXGI_FORMAT.DXGI_FORMAT_R8G8B8A8_TYPELESS;

            }

            return DXGI_FORMAT.DXGI_FORMAT_UNKNOWN;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static DXGI_FORMAT GetNativeViewFormat(this EGraphicsFormat format)
        {
            switch (format)
            {
                case EGraphicsFormat.R8_SRGB:
                    return DXGI_FORMAT.DXGI_FORMAT_R8_UNORM;

                case EGraphicsFormat.R8G8_SRGB:
                    return DXGI_FORMAT.DXGI_FORMAT_R8G8B8A8_UNORM;

                case EGraphicsFormat.R8G8B8A8_SRGB:
                    return DXGI_FORMAT.DXGI_FORMAT_R8G8B8A8_UNORM_SRGB;

                case EGraphicsFormat.R8_UNorm:
                    return DXGI_FORMAT.DXGI_FORMAT_R8_UNORM;

                case EGraphicsFormat.R8G8_UNorm:
                    return DXGI_FORMAT.DXGI_FORMAT_R8G8_UNORM;

                case EGraphicsFormat.R8G8B8A8_UNorm:
                    return DXGI_FORMAT.DXGI_FORMAT_R8G8B8A8_UNORM;
            }

            return DXGI_FORMAT.DXGI_FORMAT_UNKNOWN;
        }

        /*[MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static EGraphicsFormat GetAbstractFormat(this DXGI_FORMAT format)
        {
            switch (format)
            {
                case DXGI_FORMAT.DXGI_FORMAT_B8G8R8A8_UNORM_SRGB:
                    return EGraphicsFormat.R8_SRGB;
                case DXGI_FORMAT.DXGI_FORMAT_R8G8_TYPELESS:
                    return EGraphicsFormat.R8G8_SRGB;
                case DXGI_FORMAT.DXGI_FORMAT_R8G8B8A8_TYPELESS:
                    return EGraphicsFormat.R8G8B8A8_SRGB;
                case DXGI_FORMAT.DXGI_FORMAT_R8_UNORM:
                    return EGraphicsFormat.R8_UNorm;
                case DXGI_FORMAT.DXGI_FORMAT_R8G8_TYPELESS:
                    return EGraphicsFormat.R8G8_UNorm;
                case DXGI_FORMAT.DXGI_FORMAT_R8G8B8A8_TYPELESS:
                    return EGraphicsFormat.R8G8B8A8_UNorm;
            }

            return DXGI_FORMAT.DXGI_FORMAT_UNKNOWN;
        }*/

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static D3D12_RTV_DIMENSION GetRTVDimension(this ETextureType type)
        {
            switch (type)
            {
                case ETextureType.Tex2D:
                    return D3D12_RTV_DIMENSION.D3D12_RTV_DIMENSION_TEXTURE2D;

                case ETextureType.Tex2DArray:
                    return D3D12_RTV_DIMENSION.D3D12_RTV_DIMENSION_TEXTURE2DARRAY;
            }

            return D3D12_RTV_DIMENSION.D3D12_RTV_DIMENSION_TEXTURE3D;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static D3D12_RESOURCE_DIMENSION GetNativeDimension(this ETextureType type)
        {
            switch (type)
            {
                case ETextureType.Tex2D:
                    return D3D12_RESOURCE_DIMENSION.D3D12_RESOURCE_DIMENSION_TEXTURE2D;

                case ETextureType.Tex2DArray:
                    return D3D12_RESOURCE_DIMENSION.D3D12_RESOURCE_DIMENSION_TEXTURE2D;

                case ETextureType.Tex3D:
                    return D3D12_RESOURCE_DIMENSION.D3D12_RESOURCE_DIMENSION_TEXTURE3D;

                case ETextureType.TexCube:
                    return D3D12_RESOURCE_DIMENSION.D3D12_RESOURCE_DIMENSION_TEXTURE2D;

                case ETextureType.TexCubeArray:
                    return D3D12_RESOURCE_DIMENSION.D3D12_RESOURCE_DIMENSION_TEXTURE2D;
            }

            return D3D12_RESOURCE_DIMENSION.D3D12_RESOURCE_DIMENSION_UNKNOWN;
        }
    }

    public unsafe class FD3DBuffer : FRHIBuffer
    {
        internal ID3D12Resource* uploadResource;
        internal ID3D12Resource* defaultResource;
        internal ID3D12Resource* readbackResource;

        internal FD3DBuffer() : base()
        {

        }

        internal FD3DBuffer(FRHIDevice device, in FBufferDescriptor descriptor) : base(device, descriptor)
        {
            this.descriptor = descriptor;
            FD3DDevice d3dDevice = (FD3DDevice)device;

            // GPU Memory
            if ((descriptor.flag & EUsageType.Default) == EUsageType.Default || (descriptor.flag & EUsageType.Dynamic) == EUsageType.Dynamic || (descriptor.flag & EUsageType.Static) == EUsageType.Static)
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

                ID3D12Resource* defaultPtr;
                d3dDevice.nativeDevice->CreateCommittedResource(&defaultHeapProperties, D3D12_HEAP_FLAGS.D3D12_HEAP_FLAG_NONE, &defaultResourceDesc, D3D12_RESOURCE_STATES.D3D12_RESOURCE_STATE_COMMON, null, Windows.__uuidof<ID3D12Resource>(), (void**)&defaultPtr);
                fixed (char* namePtr = descriptor.name + "_Default")
                {
                    defaultPtr->SetName((ushort*)namePtr);
                }
                defaultResource = defaultPtr;
            }

            // Upload Memory
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

                ID3D12Resource* uploadPtr;
                d3dDevice.nativeDevice->CreateCommittedResource(&uploadHeapProperties, D3D12_HEAP_FLAGS.D3D12_HEAP_FLAG_NONE, &uploadResourceDesc, D3D12_RESOURCE_STATES.D3D12_RESOURCE_STATE_GENERIC_READ, null, Windows.__uuidof<ID3D12Resource>(), (void**)&uploadPtr);
                fixed (char* namePtr = descriptor.name + "_Upload")
                {
                    uploadPtr->SetName((ushort*)namePtr);
                }
                uploadResource = uploadPtr;
            }

            // Readback Memory
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

                ID3D12Resource* readbackPtr;
                d3dDevice.nativeDevice->CreateCommittedResource(&readbackHeapProperties, D3D12_HEAP_FLAGS.D3D12_HEAP_FLAG_NONE, &readbackResourceDesc, D3D12_RESOURCE_STATES.D3D12_RESOURCE_STATE_COPY_DEST, null, Windows.__uuidof<ID3D12Resource>(), (void**)&readbackPtr);
                fixed (char* namePtr = descriptor.name + "_Readback")
                {
                    readbackPtr->SetName((ushort*)namePtr);
                }
                readbackResource = readbackPtr;
            }
        }

        public override void SetData<T>(params T[] data) where T : struct
        {
            if ((descriptor.flag & EUsageType.Static) == EUsageType.Static || (descriptor.flag & EUsageType.Dynamic) == EUsageType.Dynamic)
            {
                void* uploadPtr;
                D3D12_RANGE range = new D3D12_RANGE(0, 0);
                uploadResource->Map(0, &range, &uploadPtr);
                data.AsSpan().CopyTo(new IntPtr(uploadPtr));
                uploadResource->Unmap(0, null);
            }
        }

        public override void Upload<T>(FRHICommandBuffer cmdBuffer) where T : struct
        {
            if ((descriptor.flag & EUsageType.Static) == EUsageType.Static || (descriptor.flag & EUsageType.Dynamic) == EUsageType.Dynamic)
            {
                FD3DCommandBuffer d3dCmdBuffer = (FD3DCommandBuffer)cmdBuffer;
                D3D12_RESOURCE_BARRIER beforeBarrier = D3D12_RESOURCE_BARRIER.InitTransition(defaultResource, D3D12_RESOURCE_STATES.D3D12_RESOURCE_STATE_COMMON, D3D12_RESOURCE_STATES.D3D12_RESOURCE_STATE_COPY_DEST);
                D3D12_RESOURCE_BARRIER afterBarrier = D3D12_RESOURCE_BARRIER.InitTransition(defaultResource, D3D12_RESOURCE_STATES.D3D12_RESOURCE_STATE_COPY_DEST, D3D12_RESOURCE_STATES.D3D12_RESOURCE_STATE_COMMON);

                d3dCmdBuffer.nativeCmdList->ResourceBarrier(0, &beforeBarrier);
                d3dCmdBuffer.nativeCmdList->CopyBufferRegion(defaultResource, 0, uploadResource, 0, descriptor.count * (ulong)Unsafe.SizeOf<T>());
                d3dCmdBuffer.nativeCmdList->ResourceBarrier(0, &afterBarrier);
            }
        }

        public override void SetData<T>(FRHICommandBuffer cmdBuffer, params T[] data) where T : struct
        {
            if ((descriptor.flag & EUsageType.Static) == EUsageType.Static || (descriptor.flag & EUsageType.Dynamic) == EUsageType.Dynamic)
            {
                void* uploadPtr;
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

        public override void GetData<T>(T[] data) where T : struct
        {
            if ((descriptor.flag & EUsageType.Staging) == EUsageType.Staging)
            {
                void* readbackPtr;
                D3D12_RANGE range = new D3D12_RANGE(0, 0);
                readbackResource->Map(0, &range, &readbackPtr);
                new IntPtr(readbackPtr).CopyTo(data.AsSpan());
                readbackResource->Unmap(0, null);
            }
        }

        public override void Readback<T>(FRHICommandBuffer cmdBuffer) where T : struct
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

                void* readbackPtr;
                D3D12_RANGE range = new D3D12_RANGE(0, 0);
                readbackResource->Map(0, &range, &readbackPtr);
                new IntPtr(readbackPtr).CopyTo(data.AsSpan());
                readbackResource->Unmap(0, null);
            }
        }

        protected override void Release()
        {
            uploadResource->Release();
            defaultResource->Release();
            readbackResource->Release();
        }
    }

    public unsafe class FD3DTexture : FRHITexture
    {
        internal ID3D12Resource* uploadResource;
        internal ID3D12Resource* defaultResource;
        internal ID3D12Resource* readbackResource;

        internal FD3DTexture() : base()
        {

        }

        internal FD3DTexture(FRHIDevice device, in FTextureDescriptor descriptor) : base(device, descriptor)
        {
            this.descriptor = descriptor;
            FD3DDevice d3dDevice = (FD3DDevice)device;

            // GPU Memory
            if ((descriptor.flag & EUsageType.Default) == EUsageType.Default)
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
                    defaultResourceDesc.Dimension = descriptor.type.GetNativeDimension();
                    defaultResourceDesc.Alignment = 0;
                    defaultResourceDesc.Width = (ulong)descriptor.width;
                    defaultResourceDesc.Height = (uint)descriptor.height;
                    defaultResourceDesc.DepthOrArraySize = (ushort)descriptor.slices;
                    defaultResourceDesc.MipLevels = descriptor.mipLevel;
                    defaultResourceDesc.Format = descriptor.format.GetNativeFormat();
                    defaultResourceDesc.SampleDesc.Count = (uint)descriptor.sample;
                    defaultResourceDesc.SampleDesc.Quality = 0;
                    defaultResourceDesc.Flags = D3D12_RESOURCE_FLAGS.D3D12_RESOURCE_FLAG_NONE;
                    defaultResourceDesc.Layout = D3D12_TEXTURE_LAYOUT.D3D12_TEXTURE_LAYOUT_UNKNOWN;
                }

                ID3D12Resource* defaultPtr;
                d3dDevice.nativeDevice->CreateCommittedResource(&defaultHeapProperties, D3D12_HEAP_FLAGS.D3D12_HEAP_FLAG_NONE, &defaultResourceDesc, D3D12_RESOURCE_STATES.D3D12_RESOURCE_STATE_COMMON, null, Windows.__uuidof<ID3D12Resource>(), (void**)&defaultPtr);
                defaultResource = defaultPtr;
            }

            // Upload Memory
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
                    uploadResourceDesc.Dimension = descriptor.type.GetNativeDimension();
                    uploadResourceDesc.Alignment = 0;
                    uploadResourceDesc.Width = (ulong)descriptor.width;
                    uploadResourceDesc.Height = (uint)descriptor.height;
                    uploadResourceDesc.DepthOrArraySize = (ushort)descriptor.slices;
                    uploadResourceDesc.MipLevels = descriptor.mipLevel;
                    uploadResourceDesc.Format = descriptor.format.GetNativeFormat();
                    uploadResourceDesc.SampleDesc.Count = (uint)descriptor.sample;
                    uploadResourceDesc.SampleDesc.Quality = 0;
                    uploadResourceDesc.Flags = D3D12_RESOURCE_FLAGS.D3D12_RESOURCE_FLAG_NONE;
                    uploadResourceDesc.Layout = D3D12_TEXTURE_LAYOUT.D3D12_TEXTURE_LAYOUT_UNKNOWN;
                }

                ID3D12Resource* uploadPtr;
                d3dDevice.nativeDevice->CreateCommittedResource(&uploadHeapProperties, D3D12_HEAP_FLAGS.D3D12_HEAP_FLAG_NONE, &uploadResourceDesc, D3D12_RESOURCE_STATES.D3D12_RESOURCE_STATE_GENERIC_READ, null, Windows.__uuidof<ID3D12Resource>(), (void**)&uploadPtr);
                uploadResource = uploadPtr;
            }

            // Readback Memory
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
                    readbackResourceDesc.Dimension = descriptor.type.GetNativeDimension();
                    readbackResourceDesc.Alignment = 0;
                    readbackResourceDesc.Width = (ulong)descriptor.width;
                    readbackResourceDesc.Height = (uint)descriptor.height;
                    readbackResourceDesc.DepthOrArraySize = (ushort)descriptor.slices;
                    readbackResourceDesc.MipLevels = descriptor.mipLevel;
                    readbackResourceDesc.Format = descriptor.format.GetNativeFormat();
                    readbackResourceDesc.SampleDesc.Count = (uint)descriptor.sample;
                    readbackResourceDesc.SampleDesc.Quality = 0;
                    readbackResourceDesc.Flags = D3D12_RESOURCE_FLAGS.D3D12_RESOURCE_FLAG_NONE;
                    readbackResourceDesc.Layout = D3D12_TEXTURE_LAYOUT.D3D12_TEXTURE_LAYOUT_UNKNOWN;
                }

                ID3D12Resource* readbackPtr;
                d3dDevice.nativeDevice->CreateCommittedResource(&readbackHeapProperties, D3D12_HEAP_FLAGS.D3D12_HEAP_FLAG_NONE, &readbackResourceDesc, D3D12_RESOURCE_STATES.D3D12_RESOURCE_STATE_COPY_DEST, null, Windows.__uuidof<ID3D12Resource>(), (void**)&readbackPtr);
                readbackResource = readbackPtr;
            }
        }

        protected override void Release()
        {
            uploadResource->Release();
            defaultResource->Release();
            readbackResource->Release();
        }
    }
}
