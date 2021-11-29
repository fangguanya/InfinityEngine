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

        public override void Transitions(FRHIResource resource)
        {

        }

        public override void ClearBuffer(FRHIBuffer buffer)
        {

        }

        public override void ClearTexture(FRHITexture texture)
        {

        }

        public override void GenerateMipmaps(FRHITexture texture)
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

        public override void BeginQuery(FRHIQuery query)
        {

        }

        public override void EndQuery(FRHIQuery query)
        {

        }

        public override void SetComputePipelineState(FRHIComputePipelineState computePipelineState)
        {

        }

        public override void DispatchCompute(uint sizeX, uint sizeY, uint sizeZ)
        {
            vkCmdDispatch(nativeCmdBuffer, sizeX, sizeY, sizeZ);
        }

        public override void DispatchComputeIndirect(FRHIBuffer argsBuffer, uint argsOffset)
        {

        }

        public override void SetRayTracePipelineState(FRHIRayTracePipelineState rayTracePipelineState)
        {

        }

        public override void CopyAccelerationStructure()
        {

        }

        public override void BuildAccelerationStructure()
        {

        }

        public override void DispatchRay(uint sizeX, uint sizeY, uint sizeZ)
        {
            
        }

        public override void DispatchRayIndirect(FRHIBuffer argsBuffer, uint argsOffset)
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

        public override void SetStencilRef()
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

        public override void SetShadingRate(in EShadingRate shadingRate, in EShadingRateCombiner[] combineMathdo)
        {

        }

        public override void SetGraphicsPipelineState(FRHIGraphicsShader graphicsShader, FRHIGraphicsPipelineState graphcisPipelineState)
        {

        }

        public override void SetPrimitiveTopology(EPrimitiveTopology topologyType)
        {
            this.topologyType = topologyType;
        }

        public override void SetIndexBuffer(FRHIIndexBufferView indexBufferView)
        {
            
        }

        public override void SetVertexBuffer(FRHIVertexBufferView vertexBufferView)
        {
            
        }

        public override void DrawIndexInstance(int indexCount, int startIndex, int startVertex, int instanceCount, int startInstance)
        {
            
        }

        public override void DrawIndexInstanceIndirect(FRHIBuffer argsBuffer, uint argsOffset)
        {
  
        }

        public override void DrawMultiIndexInstanceIndirect(FRHIBuffer cmdsBuffer, uint cmdsOffset, FRHIBuffer argsBuffer, uint argsOffset)
        {

        }

        protected override void Release()
        {

        }
    }
}
