using Vortice.Vulkan;
using static Vortice.Vulkan.Vulkan;

namespace InfinityEngine.Graphics.RHI.Vulkan
{
    public class FVkCommandBuffer : FRHICommandBuffer
    {
        internal VkCommandPool nativeCmdPool;
        internal VkCommandBuffer nativeCmdBuffer;

        internal FVkCommandBuffer(FRHIDevice device, EContextType contextType) : base(device, contextType)
        {

        }

        internal FVkCommandBuffer(string name, FRHIDevice device, EContextType contextType) : base(name, device, contextType)
        {

        }

        public override void Clear()
        {

        }

        internal override void Close()
        {

        }

        public override void BeginEvent()
        {

        }

        public override void EndEvent()
        {

        }

        public override void BeginQuery(FRHIQuery query)
        {

        }

        public override void EndQuery(FRHIQuery query)
        {

        }

        public override void Barriers(FRHIResource resource)
        {

        }

        public override void Transition(FRHIResource resource)
        {

        }

        public override void ClearBuffer(FRHIBuffer buffer)
        {

        }

        public override void ClearTexture(FRHITexture texture)
        {

        }

        public override void CopyBufferToBuffer(FRHIBuffer srcBuffer, FRHIBuffer dscBuffer)
        {

        }

        public override void CopyBufferToTexture(FRHIBuffer srcBuffer, FRHITexture dscTexture)
        {

        }

        public override void CopyTextureToBuffer(FRHITexture srcTexture, FRHIBuffer dscBuffer)
        {

        }

        public override void CopyTextureToTexture(FRHITexture srcTexture, FRHITexture dscTexture)
        {

        }

        public override void CopyAccelerationStructure()
        {

        }

        public override void BuildAccelerationStructure()
        {

        }

        public override void SetComputePipelineState(FRHIComputePipelineState computePipelineState)
        {

        }

        public override void SetComputeConstantBufferView(in uint slot, FRHIConstantBufferView constantBufferView)
        {
            
        }

        public override void SetComputeShaderResourceView(in uint slot, FRHIShaderResourceView shaderResourceView)
        {
           
        }

        public override void SetComputeUnorderedAccessView(in uint slot, FRHIUnorderedAccessView unorderedAccessView)
        {
            
        }

        public override void DispatchCompute(in uint sizeX, in uint sizeY, in uint sizeZ)
        {
            vkCmdDispatch(nativeCmdBuffer, sizeX, sizeY, sizeZ);
        }

        public override void DispatchComputeIndirect(FRHIBuffer argsBuffer, in uint argsOffset)
        {

        }

        public override void SetRayTracePipelineState(FRHIRayTracePipelineState rayTracePipelineState)
        {

        }

        public override void DispatchRay(in uint sizeX, in uint sizeY, in uint sizeZ)
        {
            
        }

        public override void DispatchRayIndirect(FRHIBuffer argsBuffer, in uint argsOffset)
        {

        }

        public override void SetScissor()
        {
            
        }

        public override void SetViewport()
        {

        }

        public override void BeginRenderPass(FRHITexture depthBuffer, params FRHITexture[] colorBuffer)
        {

        }

        public override void EndRenderPass()
        {

        }

        public override void SetStencilRef(in uint refValue)
        {

        }

        public override void SetBlendFactor(in float blendFactor)
        {

        }

        public override void SetDepthBounds(in float min, in float max)
        {

        }

        public override void SetShadingRate(FRHITexture texture)
        {
            
        }

        public override void SetShadingRate(in EShadingRate shadingRate, in EShadingRateCombiner[] combiners)
        {

        }

        public override void SetPrimitiveTopology(in EPrimitiveTopology topologyType)
        {
            this.topologyType = topologyType;
        }

        public override void SetRenderPipelineState(FRHIRenderPipelineState renderPipelineState)
        {

        }

        public override void SetIndexBuffer(FRHIBuffer indexBuffer)
        {
            
        }

        public override void SetVertexBuffer(in uint slot,FRHIBuffer vertexBuffer)
        {
            
        }
        
        public override void SetRenderConstantBufferView(in uint slot, FRHIConstantBufferView constantBufferView)
        {
            
        }

        public override void SetRenderShaderResourceView(in uint slot, FRHIShaderResourceView shaderResourceView)
        {
            
        }

        public override void SetRenderUnorderedAccessView(in uint slot, FRHIUnorderedAccessView unorderedAccessView)
        {
            
        }

        public override void DrawIndexInstanced(in uint indexCount, in uint startIndex, in int startVertex, in uint instanceCount, in uint startInstance)
        {
            
        }

        public override void DrawMultiIndexInstanced(FRHIBuffer argsBuffer, in uint argsOffset, FRHIBuffer countBuffer, in uint countOffset)
        {

        }

        public override void DrawIndexInstancedIndirect(FRHIBuffer argsBuffer, in uint argsOffset)
        {
  
        }

        protected override void Release()
        {

        }
    }
}
