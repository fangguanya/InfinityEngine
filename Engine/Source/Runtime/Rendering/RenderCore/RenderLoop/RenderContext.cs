using System;
using InfinityEngine.Core.Object;
using InfinityEngine.Graphics.RHI;
using System.Runtime.CompilerServices;

namespace InfinityEngine.Rendering.RenderLoop
{
    public sealed class FRenderContext : FDisposal
    {
        public ulong copyFrequency => m_Context.copyFrequency;
        public ulong computeFrequency => m_Context.computeFrequency;
        public ulong graphicsFrequency => m_Context.graphicsFrequency;
        public FRHITexture backBuffer => m_SwapChain.backBuffer;
        public FRHIRenderTargetView backBufferView => m_SwapChain.backBufferView;

        private FRHIContext m_Context;
        private FRHISwapChain m_SwapChain;

        public FRenderContext(FRHIContext context, FRHISwapChain swapChain)
        {
            m_Context = context;
            m_SwapChain = swapChain;
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
            return m_Context.CreateCommandBuffer(contextType, name);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public FRHICommandBuffer GetCommandBuffer(in EContextType contextType, string name, in bool bAutoRelease = true)
        {
            return m_Context.GetCommandBuffer(contextType, name, bAutoRelease);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ReleaseCommandBuffer(FRHICommandBuffer cmdBuffer)
        {
            m_Context.ReleaseCommandBuffer(cmdBuffer);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void WriteToFence(in EContextType contextType, FRHIFence fence)
        {
            m_Context.WriteToFence(contextType, fence);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void WaitForFence(in EContextType contextType, FRHIFence fence)
        {
            m_Context.WaitForFence(contextType, fence);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ExecuteCommandBuffer(FRHICommandBuffer cmdBuffer)
        {
            m_Context.ExecuteCommandBuffer(cmdBuffer);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public FRHISwapChain CreateSwapChain(in uint width, in uint height, in IntPtr windowPtr, string name)
        {
            return m_Context.CreateSwapChain(name, width, height, windowPtr);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public FRHIFence CreateFence(string name)
        {
            return m_Context.CreateFence(name);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public FRHIFence GetFence(string name)
        {
            return m_Context.GetFence(name);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ReleaseFence(FRHIFence fence)
        {
            m_Context.ReleaseFence(fence);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public FRHIQuery CreateQuery(in EQueryType queryType, string name)
        {
            return m_Context.CreateQuery(queryType, name);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public FRHIQuery GetQuery(in EQueryType queryType, string name)
        {
            return m_Context.GetQuery(queryType, name);;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ReleaseQuery(FRHIQuery query)
        {
            m_Context.ReleaseQuery(query);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public FRHIComputePipelineState CreateComputePipelineState(in FRHIComputePipelineDescriptor descriptor)
        {
            return m_Context.CreateComputePipelineState(descriptor);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public FRHIRayTracePipelineState CreateRayTracePipelineState(in FRHIRayTracePipelineDescriptor descriptor)
        {
            return m_Context.CreateRayTracePipelineState(descriptor);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public FRHIGraphicsPipelineState CreateGraphicsPipelineState(in FRHIGraphicsPipelineDescriptor descriptor)
        {
            return m_Context.CreateGraphicsPipelineState(descriptor);
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
            return m_Context.CreateBuffer(descriptor);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public FRHIBufferRef GetBuffer(in FBufferDescriptor descriptor)
        {
            return m_Context.GetBuffer(descriptor);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ReleaseBuffer(in FRHIBufferRef bufferRef)
        {
            m_Context.ReleaseBuffer(bufferRef);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public FRHITexture CreateTexture(in FTextureDescriptor descriptor)
        {
            return m_Context.CreateTexture(descriptor);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public FRHITextureRef GetTexture(in FTextureDescriptor descriptor)
        {
            return m_Context.GetTexture(descriptor);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ReleaseTexture(FRHITextureRef textureRef)
        {
            m_Context.ReleaseTexture(textureRef);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public FRHIIndexBufferView CreateIndexBufferView(FRHIBuffer buffer)
        {
            return m_Context.CreateIndexBufferView(buffer);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public FRHIVertexBufferView CreateVertexBufferView(FRHIBuffer buffer)
        {
            return m_Context.CreateVertexBufferView(buffer);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public FRHIDeptnStencilView CreateDepthStencilView(FRHITexture texture)
        {
            return m_Context.CreateDepthStencilView(texture);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public FRHIRenderTargetView CreateRenderTargetView(FRHITexture texture)
        {
            return m_Context.CreateRenderTargetView(texture);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public FRHIConstantBufferView CreateConstantBufferView(FRHIBuffer buffer)
        {
            return m_Context.CreateConstantBufferView(buffer);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public FRHIShaderResourceView CreateShaderResourceView(FRHIBuffer buffer)
        {
            return m_Context.CreateShaderResourceView(buffer);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public FRHIShaderResourceView CreateShaderResourceView(FRHITexture texture)
        {
            return m_Context.CreateShaderResourceView(texture);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public FRHIUnorderedAccessView CreateUnorderedAccessView(FRHIBuffer buffer)
        {
            return m_Context.CreateUnorderedAccessView(buffer);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public FRHIUnorderedAccessView CreateUnorderedAccessView(FRHITexture texture)
        {
            return m_Context.CreateUnorderedAccessView(texture);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public FRHIResourceSet CreateResourceSet(in uint count)
        {
            return m_Context.CreateResourceSet(count);
        }

        protected override void Release()
        {

        }
    }
}
