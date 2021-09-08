using Vortice.Direct3D;
using Vortice.Direct3D12;
using InfinityEngine.Core.Object;

namespace InfinityEngine.Graphics.RHI
{
    public enum EExecuteType
    {
        Signal = 0,
        Wait = 1,
        Execute = 2
    }

    internal struct FExecuteInfo
    {
        public FRHIFence fence;
        public EExecuteType executeType;
        public FRHICommandList cmdList;
        public FRHICommandContext cmdContext;
    }

    public class FRHICommandList : FDisposable
    {
        public string name;
        internal bool bClose;
        internal ID3D12GraphicsCommandList5 d3dCmdList;
        internal ID3D12CommandAllocator d3dCmdAllocator;

        public FRHICommandList(string name, ID3D12Device6 d3d12Device, EContextType contextType)
        {
            this.name = name;
            this.bClose = false;
            this.d3dCmdAllocator = d3d12Device.CreateCommandAllocator<ID3D12CommandAllocator>((CommandListType)contextType);
            this.d3dCmdList = d3d12Device.CreateCommandList<ID3D12GraphicsCommandList5>(0, (CommandListType)contextType, d3dCmdAllocator, null);
            this.d3dCmdList.QueryInterface<ID3D12GraphicsCommandList5>();
        }

        public void Clear()
        {
            if(!bClose) { return; }

            bClose = false;
            d3dCmdAllocator.Reset();
            d3dCmdList.Reset(d3dCmdAllocator, null);
        }

        internal void Close()
        {
            bClose = true;
            d3dCmdList.Close();
        }

        public void ClearBuffer(FRHIBuffer buffer)
        {

        }

        public void ClearTexture(FRHITexture texture)
        {

        }

        public void CopyBufferToBuffer(FRHIBuffer srcBuffer, FRHIBuffer dscBuffer)
        {

        }

        public void CopyBufferToTexture(FRHIBuffer srcBuffer, FRHITexture dscTexture)
        {

        }

        public void CopyTextureToBuffer(FRHITexture srcTexture, FRHIBuffer dscBuffer)
        {

        }

        public void CopyTextureToTexture(FRHITexture srcTexture, FRHITexture dscTexture)
        {

        }

        public void GenerateMipmaps(FRHITexture texture)
        {

        }

        public void TransitionResource()
        {

        }

        public void BeginQuery(FRHITimeQuery timeQuery)
        {
            timeQuery.Begin(d3dCmdList);
        }

        public void EndQuery(FRHITimeQuery timeQuery)
        {
            timeQuery.End(d3dCmdList);
        }

        public void BeginQuery(FRHIOcclusionQuery occlusionQuery)
        {
            occlusionQuery.Begin(d3dCmdList);
        }

        public void EndQuery(FRHIOcclusionQuery occlusionQuery)
        {
            occlusionQuery.End(d3dCmdList);
        }

        public void BeginQuery(FRHIStatisticsQuery statisticsQuery)
        {

        }

        public void EndQuery(FRHIStatisticsQuery statisticsQuery)
        {

        }

        public void SetComputePipelineState(FRHIComputePipelineState computeState)
        {

        }

        public void DispatchCompute(FRHIComputeShader computeShader, uint sizeX, uint sizeY, uint sizeZ)
        {

        }

        public void DispatchComputeIndirect(FRHIComputeShader computeShader, FRHIBuffer argsBuffer, uint argsOffset)
        {

        }

        public void SetRayTracePipelineState(FRHIRayGenShader rayGenShader, FRHIRayTracePipelineState rayTraceState)
        {

        }

        public void BuildAccelerationStructure()
        {

        }

        public void CopyAccelerationStructure()
        {

        }

        public void DispatchRay(FRHIRayGenShader rayGenShader, uint sizeX, uint sizeY, uint sizeZ)
        {

        }

        public void DispatchRayIndirect(FRHIRayGenShader rayGenShader, FRHIBuffer argsBuffer, uint argsOffset)
        {

        }

        public void SetViewport()
        {

        }

        public void SetScissorRect()
        {

        }

        public void BeginEvent()
        {

        }

        public void EndEvent()
        {

        }

        public void BeginRenderPass(FRHITexture depthBuffer, params FRHITexture[] colorBuffer)
        {

        }

        public void EndRenderPass()
        {
            d3dCmdList.EndRenderPass();
        }

        public void SetStencilRef()
        {

        }

        public void SetBlendFactor()
        {

        }

        public void SetDepthBounds(in float min, in float max)
        {

        }

        public void SetShadingRate(in ShadingRate shadingRate, in ShadingRateCombiner[] combineMathdo)
        {
            d3dCmdList.RSSetShadingRate(shadingRate, combineMathdo);
        }

        public void SetShadingRate(FRHITexture indirectTexture)
        {
            d3dCmdList.RSSetShadingRateImage(indirectTexture.defaultResource);
        }

        public void SetGraphicsPipelineState(FRHIGraphicsShader graphicsShader, FRHIGraphicsPipelineState graphcisState)
        {

        }

        public void DrawInstance(FRHIIndexBufferView indexBufferView, FRHIVertexBufferView vertexBufferView, PrimitiveTopology topologyType, int indexCount, int startIndex, int startVertex, int instanceCount, int startInstance)
        {
            d3dCmdList.IASetPrimitiveTopology(topologyType);
            d3dCmdList.IASetIndexBuffer(indexBufferView.d3dView);
            d3dCmdList.IASetVertexBuffers(0, vertexBufferView.d3dView);
            d3dCmdList.DrawIndexedInstanced(indexCount, instanceCount, startIndex, startVertex, startInstance);
        }

        public void DrawInstanceIndirect(FRHIIndexBufferView indexBufferView, FRHIVertexBufferView vertexBufferView, PrimitiveTopology topologyType, FRHIBuffer argsBuffer, uint argsOffset)
        {

        }

        public void DrawMultiInstance(FRHIBuffer cmdBuffer, PrimitiveTopology topologyType, FRHIBuffer argsBuffer, uint argsOffset)
        {

        }

        public void DrawMultiInstanceIndirect()
        {

        }

        protected override void Release()
        {
            d3dCmdList?.Dispose();
            d3dCmdAllocator?.Dispose();
        }

        public static implicit operator ID3D12GraphicsCommandList5(FRHICommandList cmdList) { return cmdList.d3dCmdList; }
    }
}
