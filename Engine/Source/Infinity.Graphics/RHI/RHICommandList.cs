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

    public class FRHICommandList : UObject
    {
        public string name;
        internal ID3D12GraphicsCommandList5 d3D12CmdList;
        internal ID3D12CommandAllocator d3D12CmdAllocator;

        public FRHICommandList(string name, ID3D12Device6 d3d12Device, EContextType cmdListType)
        {
            this.name = name;
            this.d3D12CmdAllocator = d3d12Device.CreateCommandAllocator<ID3D12CommandAllocator>((CommandListType)cmdListType);
            this.d3D12CmdList = d3d12Device.CreateCommandList<ID3D12GraphicsCommandList5>(0, (CommandListType)cmdListType, d3D12CmdAllocator, null);
            this.d3D12CmdList.QueryInterface<ID3D12GraphicsCommandList5>();
        }

        public void Clear()
        {
            d3D12CmdAllocator.Reset();
            d3D12CmdList.Reset(d3D12CmdAllocator, null);
        }

        internal void Close()
        {
            d3D12CmdList.Close();
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

        public void BeginTimeQuery(FRHITimeQuery timeQuery)
        {
            timeQuery.Begin(d3D12CmdList);
        }

        public void EndTimeQuery(FRHITimeQuery timeQuery)
        {
            timeQuery.End(d3D12CmdList);
        }

        public void SetComputePipelineState(FRHIComputeShader computeShader, FRHIComputePipelineState computeState)
        {

        }

        public void SetComputeSamplerState(FRHIComputeShader computeShader)
        {

        }

        public void SetComputeBuffer(FRHIComputeShader computeShader, FRHIBuffer buffer)
        {

        }

        public void SetComputeTexture(FRHIComputeShader computeShader, FRHITexture texture)
        {

        }

        public void DispatchCompute(FRHIComputeShader computeShader, uint sizeX, uint sizeY, uint sizeZ)
        {

        }

        public void DispatchComputeIndirect(FRHIComputeShader computeShader, FRHIBuffer argsBuffer, uint argsOffset)
        {

        }

        public void BuildAccelerationStructure()
        {

        }

        public void CopyAccelerationStructure()
        {

        }

        public void SetAccelerationStructure(FRHIRayGenShader rayGenShader)
        {

        }

        public void SetRayTracePipelineState(FRHIRayGenShader rayGenShader, FRHIRayTracePipelineState rayTraceState)
        {

        }

        public void SetRayTraceSamplerState(FRHIRayGenShader rayGenShader)
        {

        }

        public void SetRayTraceBuffer(FRHIRayGenShader rayGenShader, FRHIBuffer buffer)
        {

        }

        public void SetRayTraceTexture(FRHIRayGenShader rayGenShader, FRHITexture texture)
        {

        }

        public void DispatchRay(FRHIRayGenShader rayGenShader, uint sizeX, uint sizeY, uint sizeZ)
        {

        }

        public void DispatchRayIndirect(FRHIRayGenShader rayGenShader, FRHIBuffer argsBuffer, uint argsOffset)
        {

        }

        public void BeginOcclusionQuery(FRHIOcclusionQuery occlusionQuery)
        {
            occlusionQuery.Begin(d3D12CmdList);
        }

        public void EndOcclusionQuery(FRHIOcclusionQuery occlusionQuery)
        {
            occlusionQuery.End(d3D12CmdList);
        }

        public void BeginStatisticsQuery(FRHIStatisticsQuery statisticsQuery)
        {

        }

        public void EndStatisticsQuery(FRHIStatisticsQuery statisticsQuery)
        {

        }

        public void SetViewport()
        {

        }

        public void SetScissorRect()
        {

        }

        public void BeginFrame()
        {

        }

        public void EndFrame()
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
            d3D12CmdList.EndRenderPass();
        }

        public void SetStencilRef()
        {

        }

        public void SetBlendFactor()
        {

        }

        public void SetDepthBounds(float min, float max)
        {

        }

        public void SetShadingRate(ShadingRate shadingRate, ShadingRateCombiner[] combineMathdo)
        {
            d3D12CmdList.RSSetShadingRate(shadingRate, combineMathdo);
        }

        public void SetShadingRateIndirect(FRHITexture indirectTexture)
        {
            d3D12CmdList.RSSetShadingRateImage(indirectTexture.defaultResource);
        }

        public void SetGraphicsPipelineState(FRHIGraphicsShader graphicsShader, FRHIGraphicsPipelineState graphcisState)
        {

        }

        public void SetGraphicsSamplerState(FRHIGraphicsShader graphicsShader, string propertyName)
        {

        }

        public void SetGraphicsBuffer(FRHIGraphicsShader graphicsShader, string propertyName, FRHIBuffer buffer)
        {

        }

        public void SetGraphicsTexture(FRHIGraphicsShader graphicsShader, string propertyName, FRHITexture texture)
        {

        }

        public void DrawPrimitiveInstance(FRHIIndexBufferView indexBufferView, FRHIVertexBufferView vertexBufferView, PrimitiveTopology topologyType, int indexCount, int startIndex, int startVertex, int instanceCount, int startInstance)
        {
            d3D12CmdList.IASetPrimitiveTopology(topologyType);
            d3D12CmdList.IASetIndexBuffer(indexBufferView.d3DIBV);
            d3D12CmdList.IASetVertexBuffers(0, vertexBufferView.d3DVBO);
            d3D12CmdList.DrawIndexedInstanced(indexCount, instanceCount, startIndex, startVertex, startInstance);
        }

        public void DrawPrimitiveInstanceIndirect(FRHIIndexBufferView indexBufferView, FRHIVertexBufferView vertexBufferView, PrimitiveTopology topologyType, FRHIBuffer argsBuffer, uint argsOffset)
        {

        }

        public void DrawMultiPrimitiveInstance(FRHIBuffer cmdsBuffer, PrimitiveTopology topologyType, FRHIBuffer argsBuffer, uint argsOffset)
        {

        }

        public void DrawMultiPrimitiveInstanceIndirect()
        {

        }

        protected override void Disposed()
        {
            d3D12CmdList?.Dispose();
            d3D12CmdAllocator?.Dispose();
        }

        public static implicit operator ID3D12GraphicsCommandList5(FRHICommandList rhiCmdList) { return rhiCmdList.d3D12CmdList; }
    }
}
