using System;
using InfinityEngine.Core.Object;
using InfinityEngine.Graphics.RHI;
using System.Runtime.CompilerServices;

namespace InfinityEngine.Rendering.RenderLoop
{
    public sealed class FRenderContext : FDisposal
    {
        public ulong copyFrequency => m_DeviceContext.copyFrequency;
        public ulong computeFrequency => m_DeviceContext.computeFrequency;
        public ulong graphicsFrequency => m_DeviceContext.graphicsFrequency;

        private FRHIDeviceContext m_DeviceContext;

        public FRenderContext(FRHIDeviceContext deviceContext)
        {
            m_DeviceContext = deviceContext;
        }

        public void Cull()
        {
            CullLight();
            CullTerrain();
            CullFoliage();
            CullPrimitive();
            CullLightProbe();
        }

        private void CullLight()
        {

        }

        private void CullTerrain()
        {

        }

        private void CullFoliage()
        {

        }

        private void CullPrimitive()
        {

        }

        private void CullLightProbe()
        {

        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public FRHICommandBuffer CreateCommandBuffer(in EContextType contextType, string name)
        {
            return m_DeviceContext.CreateCommandBuffer(contextType, name);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public FRHICommandBuffer GetCommandBuffer(in EContextType contextType, string name, in bool bAutoRelease = false)
        {
            return m_DeviceContext.GetCommandBuffer(contextType, name, bAutoRelease);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ReleaseCommandBuffer(FRHICommandBuffer cmdBuffer)
        {
            m_DeviceContext.ReleaseCommandBuffer(cmdBuffer);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void WriteToFence(in EContextType contextType, FRHIFence fence)
        {
            m_DeviceContext.WriteToFence(contextType, fence);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void WaitForFence(in EContextType contextType, FRHIFence fence)
        {
            m_DeviceContext.WaitForFence(contextType, fence);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ExecuteCommandBuffer(FRHICommandBuffer cmdBuffer)
        {
            m_DeviceContext.ExecuteCommandBuffer(cmdBuffer);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public FRHISwapChain CreateSwapChain(in uint width, in uint height, in IntPtr windowPtr, string name)
        {
            return m_DeviceContext.CreateSwapChain(name, width, height, windowPtr);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public FRHIFence CreateFence(string name)
        {
            return m_DeviceContext.CreateFence(name);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public FRHIFence GetFence(string name)
        {
            return m_DeviceContext.GetFence(name);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ReleaseFence(FRHIFence fence)
        {
            m_DeviceContext.ReleaseFence(fence);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public FRHIQuery CreateQuery(in EQueryType queryType, string name)
        {
            return m_DeviceContext.CreateQuery(queryType, name);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public FRHIQuery GetQuery(in EQueryType queryType, string name)
        {
            return m_DeviceContext.GetQuery(queryType, name);;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ReleaseQuery(FRHIQuery query)
        {
            m_DeviceContext.ReleaseQuery(query);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public FRHIComputePipelineState CreateComputePipelineState(in FRHIComputePipelineDescriptor descriptor)
        {
            return m_DeviceContext.CreateComputePipelineState(descriptor);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public FRHIRayTracePipelineState CreateRayTracePipelineState(in FRHIRayTracePipelineDescriptor descriptor)
        {
            return m_DeviceContext.CreateRayTracePipelineState(descriptor);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public FRHIGraphicsPipelineState CreateGraphicsPipelineState(in FRHIGraphicsPipelineDescriptor descriptor)
        {
            return m_DeviceContext.CreateGraphicsPipelineState(descriptor);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void CreateSamplerState()
        {

        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void CreateVertexInputLayout()
        {

        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void CreateResourceInputLayout()
        {

        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public FRHIBuffer CreateBuffer(in FBufferDescriptor descriptor)
        {
            return m_DeviceContext.CreateBuffer(descriptor);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public FRHIBufferRef GetBuffer(in FBufferDescriptor descriptor)
        {
            return m_DeviceContext.GetBuffer(descriptor);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ReleaseBuffer(in FRHIBufferRef bufferRef)
        {
            m_DeviceContext.ReleaseBuffer(bufferRef);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public FRHITexture CreateTexture(in FTextureDescriptor descriptor)
        {
            return m_DeviceContext.CreateTexture(descriptor);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public FRHITextureRef GetTexture(in FTextureDescriptor descriptor)
        {
            return m_DeviceContext.GetTexture(descriptor);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ReleaseTexture(FRHITextureRef textureRef)
        {
            m_DeviceContext.ReleaseTexture(textureRef);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public FRHIIndexBufferView CreateIndexBufferView(FRHIBuffer buffer)
        {
            return m_DeviceContext.CreateIndexBufferView(buffer);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public FRHIVertexBufferView CreateVertexBufferView(FRHIBuffer buffer)
        {
            return m_DeviceContext.CreateVertexBufferView(buffer);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public FRHIDeptnStencilView CreateDepthStencilView(FRHITexture texture)
        {
            return m_DeviceContext.CreateDepthStencilView(texture);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public FRHIRenderTargetView CreateRenderTargetView(FRHITexture texture)
        {
            return m_DeviceContext.CreateRenderTargetView(texture);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public FRHIConstantBufferView CreateConstantBufferView(FRHIBuffer buffer)
        {
            return m_DeviceContext.CreateConstantBufferView(buffer);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public FRHIShaderResourceView CreateShaderResourceView(FRHIBuffer buffer)
        {
            return m_DeviceContext.CreateShaderResourceView(buffer);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public FRHIShaderResourceView CreateShaderResourceView(FRHITexture texture)
        {
            return m_DeviceContext.CreateShaderResourceView(texture);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public FRHIUnorderedAccessView CreateUnorderedAccessView(FRHIBuffer buffer)
        {
            return m_DeviceContext.CreateUnorderedAccessView(buffer);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public FRHIUnorderedAccessView CreateUnorderedAccessView(FRHITexture texture)
        {
            return m_DeviceContext.CreateUnorderedAccessView(texture);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public FRHIResourceSet CreateResourceSet(in uint count)
        {
            return m_DeviceContext.CreateResourceSet(count);
        }

        protected override void Release()
        {

        }
    }
}
