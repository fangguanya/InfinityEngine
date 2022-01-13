using System;
using Vortice.Vulkan;
using static Vortice.Vulkan.Vulkan;
using InfinityEngine.Core.Mathmatics;
using InfinityEngine.Core.Mathmatics.Geometry;

namespace InfinityEngine.Graphics.RHI.Vulkan
{
    public class FVkCommandBuffer : FRHICommandBuffer
    {
        internal VkCommandPool nativeCmdPool;
        internal VkCommandBuffer nativeCmdBuffer;

        internal FVkCommandBuffer(string name, FRHIDevice device, EContextType contextType) : base(name, device, contextType)
        {

        }

        public override void Clear()
        {

        }

        internal override void Close()
        {

        }

        public override void BeginEvent(string name)
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

        public override void Barriers(in ReadOnlySpan<FResourceBarrierInfo> barrierBatch)
        {

        }

        public override void Transition(FRHIResource resource, EResourceState stateBefore, EResourceState stateAfter, int subresource = -1)
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

        public override void SetComputeResourceBind(in uint slot, FRHIResourceSet resourceSet)
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

        public override void SetRayTraceResourceBind(in uint slot, FRHIResourceSet resourceSet)
        {

        }

        public override void DispatchRay(in uint sizeX, in uint sizeY, in uint sizeZ)
        {
            
        }

        public override void DispatchRayIndirect(FRHIBuffer argsBuffer, in uint argsOffset)
        {

        }

        public override void SetScissor(in FRect rect)
        {

        }

        public override void SetViewport(in FViewport viewport)
        {

        }

        public override void BeginRenderPass(FRHITexture depthBuffer, params FRHITexture[] colorBuffer)
        {

        }

        public override void EndRenderPass()
        {

        }

        public override void ClearRenderTarget(FRHIRenderTargetView renderTargetView, float4 color)
        {

        }

        public override void SetStencilRef(in uint refValue)
        {

        }

        public override void SetBlendFactor(in float blendFactor)
        {

        }

        public override void SetDepthBound(in float min, in float max)
        {

        }

        public override void SetShadingRate(FRHITexture texture)
        {
            
        }

        public override void SetShadingRate(in EShadingRate shadingRate, in EShadingRateCombiner combiner)
        {

        }

        public override void SetPrimitiveTopology(in EPrimitiveTopology topologyType)
        {
            m_TopologyType = topologyType;
        }

        public override void SetGraphicsPipelineState(FRHIGraphicsPipelineState graphicsPipelineState)
        {

        }

        public override void SetIndexBuffer(FRHIIndexBufferView indexBufferView)
        {
            throw new System.NotImplementedException();
        }

        public override void SetVertexBuffer(in uint slot, FRHIVertexBufferView vertexBufferView)
        {
            throw new System.NotImplementedException();
        }

        public override void SetRenderResourceBind(in uint slot, FRHIResourceSet resourceSet)
        {
            throw new System.NotImplementedException();
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
