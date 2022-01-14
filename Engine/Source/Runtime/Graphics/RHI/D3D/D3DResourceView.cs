using System;
using TerraFX.Interop.Windows;
using TerraFX.Interop.DirectX;
using InfinityEngine.Core.Object;
using InfinityEngine.Core.Memory;
using System.Runtime.CompilerServices;

namespace InfinityEngine.Graphics.RHI.D3D
{
    public unsafe class FD3DDeptnStencilView : FRHIDeptnStencilView
    {
        internal D3D12_CPU_DESCRIPTOR_HANDLE descriptorHandle => m_DescriptorHandle;

        private D3D12_CPU_DESCRIPTOR_HANDLE m_DescriptorHandle;
        private FRHIDescriptorHeapFactory m_DescriptorHeapFactory;

        internal FD3DDeptnStencilView(FRHIDevice device, FRHIDescriptorHeapFactory descriptorHeapFactory, FRHITexture texture) 
        {
            m_DescriptorHeapFactory = descriptorHeapFactory;
            FD3DDescriptorHeapFactory d3dDescriptorHeapFactory = (FD3DDescriptorHeapFactory)descriptorHeapFactory;
            descriptorIndex = d3dDescriptorHeapFactory.Allocate();
            m_DescriptorHandle = d3dDescriptorHeapFactory.cpuStartHandle;
            m_DescriptorHandle.Offset(descriptorIndex, d3dDescriptorHeapFactory.descriptorSize);

            FD3DDevice d3dDevice = (FD3DDevice)device;
            FD3DTexture d3DTexture = (FD3DTexture)texture;
            D3D12_DEPTH_STENCIL_VIEW_DESC dsvDescriptor;
            dsvDescriptor.Format = DXGI_FORMAT.DXGI_FORMAT_D32_FLOAT_S8X24_UINT;
            dsvDescriptor.ViewDimension = D3D12_DSV_DIMENSION.D3D12_DSV_DIMENSION_TEXTURE2D;
            d3dDevice.nativeDevice->CreateDepthStencilView(d3DTexture.defaultResource, &dsvDescriptor, m_DescriptorHandle);
        }

        protected override void Release()
        {
            m_DescriptorHeapFactory.Free(descriptorIndex);
        }
    }

    public unsafe class FD3DRenderTargetView : FRHIRenderTargetView
    {
        internal D3D12_CPU_DESCRIPTOR_HANDLE descriptorHandle => m_DescriptorHandle;

        private D3D12_CPU_DESCRIPTOR_HANDLE m_DescriptorHandle;
        private FRHIDescriptorHeapFactory m_DescriptorHeapFactory;

        internal FD3DRenderTargetView(FRHIDevice device, FRHIDescriptorHeapFactory descriptorHeapFactory, FRHITexture texture)
        {
            m_DescriptorHeapFactory = descriptorHeapFactory;
            FD3DDescriptorHeapFactory d3dDescriptorHeapFactory = (FD3DDescriptorHeapFactory)descriptorHeapFactory;
            descriptorIndex = d3dDescriptorHeapFactory.Allocate();
            m_DescriptorHandle = d3dDescriptorHeapFactory.cpuStartHandle;
            m_DescriptorHandle.Offset(descriptorIndex, d3dDescriptorHeapFactory.descriptorSize);

            FD3DDevice d3dDevice = (FD3DDevice)device;
            FD3DTexture d3DTexture = (FD3DTexture)texture;
            D3D12_RENDER_TARGET_VIEW_DESC rtvDescriptor;
            rtvDescriptor.Format = FD3DTextureUtility.GetNativeViewFormat(d3DTexture.descriptor.format);
            rtvDescriptor.ViewDimension = FD3DTextureUtility.GetRTVDimension(d3DTexture.descriptor.type);
            d3dDevice.nativeDevice->CreateRenderTargetView(d3DTexture.defaultResource, &rtvDescriptor, m_DescriptorHandle);
        }

        protected override void Release()
        {
            m_DescriptorHeapFactory.Free(descriptorIndex);
        }
    }
}
