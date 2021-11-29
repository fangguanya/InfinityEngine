using InfinityEngine.Core.Object;

namespace InfinityEngine.Graphics.RHI
{
    public enum EContextType
    {
        Copy = 3,
        Compute = 2,
        Render = 0
    }

    public class FRHIGraphicsContext : FDisposable
    {
        public virtual ulong copyFrequency => 0;
        public virtual ulong computeFrequency => 0;
        public virtual ulong graphicsFrequency => 0;

        public FRHIGraphicsContext() { }

        internal virtual FRHICommandContext SelectContext(in EContextType contextType) { return null; }
        public virtual FRHICommandBuffer CreateCommandBuffer(in EContextType contextType, string name = null) { return null; }
        public virtual FRHICommandBuffer GetCommandBuffer(in EContextType contextType, string name = null, bool bAutoRelease = false) { return null; }
        public virtual void ReleaseCommandBuffer(FRHICommandBuffer cmdBuffer) { }
        public virtual void WriteToFence(in EContextType contextType, FRHIFence fence) { }
        public virtual void WaitForFence(in EContextType contextType, FRHIFence fence) { }
        public virtual void ExecuteCommandBuffer(FRHICommandBuffer cmdBuffer) { }
        public virtual void Flush() { }
        public virtual void Submit() { }
        public virtual void CreateViewport() { }
        public virtual FRHIFence CreateFence(string name = null) { return null; }
        public virtual FRHIFence GetFence(string name = null) { return null; }
        public virtual void ReleaseFence(FRHIFence fence) { }
        public virtual FRHIQuery CreateQuery(in EQueryType queryType, string name = null) { return null; }
        public virtual FRHIQuery GetQuery(in EQueryType queryType, string name = null) { return null; }
        public virtual void ReleaseQuery(FRHIQuery query) { }
        public virtual FRHIComputePipelineState CreateComputePipelineState(in FRHIComputePipelineDescription description) { return default; }
        public virtual FRHIRayTracePipelineState CreateRayTracePipelineState(in FRHIRayTracePipelineDescription description) { return default; }
        public virtual FRHIGraphicsPipelineState CreateGraphicsPipelineState(in FRHIGraphicsPipelineDescription description) { return default; }
        public virtual void CreateSamplerState() { }
        public virtual void CreateVertexInputLayout() { }
        public virtual void CreateResourceInputLayout() { }
        public virtual FRHIBuffer CreateBuffer(in FRHIBufferDescription description) { return null; }
        public virtual FRHIBufferRef GetBuffer(in FRHIBufferDescription description) { return default; }
        public virtual void ReleaseBuffer(FRHIBufferRef bufferRef) { }
        public virtual FRHITexture CreateTexture(in FRHITextureDescription description) { return null; }
        public virtual FRHITextureRef GetTexture(in FRHITextureDescription description) { return default; }
        public virtual void ReleaseTexture(FRHITextureRef textureRef) { }
        public virtual FRHIIndexBufferView CreateIndexBufferView(FRHIBuffer buffer) { return default; }
        public virtual FRHIVertexBufferView CreateVertexBufferView(FRHIBuffer buffer) { return default; }
        public virtual FRHIDeptnStencilView CreateDepthStencilView(FRHITexture texture) { return default; }
        public virtual FRHIRenderTargetView CreateRenderTargetView(FRHITexture texture) { return default; }
        public virtual FRHIConstantBufferView CreateConstantBufferView(FRHIBuffer buffer) { return default; }
        public virtual FRHIShaderResourceView CreateShaderResourceView(FRHIBuffer buffer) { return default; }
        public virtual FRHIShaderResourceView CreateShaderResourceView(FRHITexture texture) { return default; }
        public virtual FRHIUnorderedAccessView CreateUnorderedAccessView(FRHIBuffer buffer) { return default; }
        public virtual FRHIUnorderedAccessView CreateUnorderedAccessView(FRHITexture texture) { return default; }
        public virtual FRHIResourceSet CreateResourceSet(in int count) { return null; }
    }
}
