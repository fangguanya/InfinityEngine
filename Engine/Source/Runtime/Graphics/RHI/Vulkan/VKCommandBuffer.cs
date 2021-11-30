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

        public override void BeginQuery(FRHIQuery query)
        {

        }

        public override void EndQuery(FRHIQuery query)
        {

        }

        public override void SetComputePipelineState(FRHIComputePipelineState computePipelineState)
        {

        }

        public override void SetComputeConstantBufferView(in int slot, FRHIConstantBufferView constantBufferView)
        {
            
        }

        public override void SetComputeShaderResourceView(in int slot, FRHIShaderResourceView shaderResourceView)
        {
           
        }

        public override void SetComputeUnorderedAccessView(in int slot, FRHIUnorderedAccessView unorderedAccessView)
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

        public override void BeginEvent()
        {

        }

        public override void EndEvent()
        {

        }

        public override void BeginRenderPass(FRHITexture depthBuffer, params FRHITexture[] colorBuffer)
        {

        }

        public override void EndRenderPass()
        {

        }

        public override void SetStencilRef(in int stencilRef)
        {

        }

        public override void SetBlendFactor()
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

        public override void SetPrimitiveTopology(EPrimitiveTopology topologyType)
        {
            this.topologyType = topologyType;
        }

        public override void SetRenderPipelineState(FRHIRenderPipelineState renderPipelineState)
        {

        }

        public override void SetIndexBuffer(FRHIIndexBufferView indexBufferView)
        {
            
        }

        public override void SetVertexBuffer(in int slot,FRHIVertexBufferView vertexBufferView)
        {
            
        }
        
        public override void SetRenderConstantBufferView(in int slot, FRHIConstantBufferView constantBufferView)
        {
            
        }

        public override void SetRenderShaderResourceView(in int slot, FRHIShaderResourceView shaderResourceView)
        {
            
        }

        public override void SetRenderUnorderedAccessView(in int slot, FRHIUnorderedAccessView unorderedAccessView)
        {
            
        }

        public override void DrawIndexInstanced(in int indexCount, in int startIndex, in int startVertex, in int instanceCount, in int startInstance)
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
