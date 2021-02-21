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
        internal FRHIFence Fence;
        internal EExecuteType ExecuteType;
        internal FRHICommandBuffer CmdBuffer;
        internal FRHICommandContext CmdContext;
    }

    public class FRHICommandBuffer : UObject
    {
        public string name;
        internal ID3D12GraphicsCommandList5 NativeCmdList;
        internal ID3D12CommandAllocator NativeCmdAllocator;

        public FRHICommandBuffer(string Name, ID3D12Device6 NativeDevice, CommandListType CommandBufferType)
        {
            name = Name;
            NativeCmdAllocator = NativeDevice.CreateCommandAllocator<ID3D12CommandAllocator>(CommandBufferType);
            NativeCmdList = NativeDevice.CreateCommandList<ID3D12GraphicsCommandList5>(0, CommandBufferType, NativeCmdAllocator, null);
            NativeCmdList.QueryInterface<ID3D12GraphicsCommandList5>();
        }

        public static implicit operator ID3D12GraphicsCommandList5(FRHICommandBuffer CmdBuffer) { return CmdBuffer.NativeCmdList; }

        public void Clear()
        {
            NativeCmdAllocator.Reset();
            NativeCmdList.Reset(NativeCmdAllocator, null);
        }

        public void Close()
        {
            NativeCmdList.Close();
        }

        public void ClearBuffer(FRHIBuffer Buffer)
        {

        }

        public void ClearTexture(FRHITexture Texture)
        {

        }

        public void CopyBufferToBuffer(FRHIBuffer SrcBuffer, FRHIBuffer DstBuffer)
        {

        }

        public void CopyBufferToTexture(FRHIBuffer SrcBuffer, FRHITexture DstTexture)
        {

        }

        public void CopyTextureToBuffer(FRHITexture SrcTexture, FRHIBuffer DstBuffer)
        {

        }

        public void CopyTextureToTexture(FRHITexture SrcTexture, FRHITexture DstTexture)
        {

        }

        public void GenerateMipmaps(FRHITexture Texture)
        {

        }

        public void TransitionResource()
        {

        }

        public void BeginTimeQuery(FRHITimeQuery TimeQuery)
        {
            TimeQuery.Begin(NativeCmdList);
        }

        public void EndTimeQuery(FRHITimeQuery TimeQuery)
        {
            TimeQuery.End(NativeCmdList);
        }

        public void SetComputePipelineState(FRHIComputeShader ComputeShader, FRHIComputePipelineState ComputeState)
        {

        }

        public void SetComputeSamplerState(FRHIComputeShader ComputeShader)
        {

        }

        public void SetComputeBuffer(FRHIComputeShader ComputeShader, FRHIBuffer Buffer)
        {

        }

        public void SetComputeTexture(FRHIComputeShader ComputeShader, FRHITexture Texture)
        {

        }

        public void DispatchCompute(FRHIComputeShader ComputeShader, uint SizeX, uint SizeY, uint SizeZ)
        {

        }

        public void DispatchComputeIndirect(FRHIComputeShader ComputeShader, FRHIBuffer ArgsBuffer, uint ArgsOffset)
        {

        }

        public void BuildAccelerationStructure()
        {

        }

        public void CopyAccelerationStructure()
        {

        }

        public void SetAccelerationStructure(FRHIRayGenShader RayGenShader)
        {

        }

        public void SetRayTracePipelineState(FRHIRayGenShader RayGenShader, FRHIRayTracePipelineState RayTraceState)
        {

        }

        public void SetRayTraceSamplerState(FRHIRayGenShader RayGenShader)
        {

        }

        public void SetRayTraceBuffer(FRHIRayGenShader RayGenShader, FRHIBuffer Buffer)
        {

        }

        public void SetRayTraceTexture(FRHIRayGenShader RayGenShader, FRHITexture Texture)
        {

        }

        public void DispatchRay(FRHIRayGenShader RayGenShader, uint SizeX, uint SizeY, uint SizeZ)
        {

        }

        public void DispatchRayIndirect(FRHIRayGenShader RayGenShader, FRHIBuffer ArgsBuffer, uint ArgsOffset)
        {

        }

        public void BeginOcclusionQuery(FRHIOcclusionQuery OcclusionQuery)
        {
            OcclusionQuery.Begin(NativeCmdList);
        }

        public void EndOcclusionQuery(FRHIOcclusionQuery OcclusionQuery)
        {
            OcclusionQuery.End(NativeCmdList);
        }

        public void BeginStatisticsQuery(FRHIStatisticsQuery StatisticsQuery)
        {

        }

        public void EndStatisticsQuery(FRHIStatisticsQuery StatisticsQuery)
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

        public void BeginRenderPass(FRHITexture DepthBuffer, params FRHITexture[] ColorBuffer)
        {

        }

        public void EndRenderPass()
        {
            NativeCmdList.EndRenderPass();
        }

        public void SetStencilRef()
        {

        }

        public void SetBlendFactor()
        {

        }

        public void SetDepthBounds(float Min, float Max)
        {

        }

        public void SetShadingRate(ShadingRate EShadingRate, ShadingRateCombiner[] CombineMathdo)
        {
            NativeCmdList.RSSetShadingRate(EShadingRate, CombineMathdo);
        }

        public void SetShadingRateIndirect(FRHITexture IndirectTexture)
        {
            NativeCmdList.RSSetShadingRateImage(IndirectTexture.DefaultResource);
        }

        public void SetGraphicsPipelineState(FRHIGraphicsShader GraphicsShader, FRHIGraphicsPipelineState GraphcisState)
        {

        }

        public void SetGraphicsSamplerState(FRHIGraphicsShader GraphicsShader, string PropertyName)
        {

        }

        public void SetGraphicsBuffer(FRHIGraphicsShader GraphicsShader, string PropertyName, FRHIBuffer Buffer)
        {

        }

        public void SetGraphicsTexture(FRHIGraphicsShader GraphicsShader, string PropertyName, FRHITexture Texture)
        {

        }

        public void DrawPrimitiveInstance(FRHIBuffer IndexBuffer, FRHIBuffer VertexBuffer, PrimitiveTopology TopologyType, uint IndexCount, uint InstanceCount)
        {

        }

        public void DrawPrimitiveInstanceIndirect(FRHIBuffer IndexBuffer, FRHIBuffer VertexBuffer, PrimitiveTopology TopologyType, FRHIBuffer ArgsBuffer, uint ArgsOffset)
        {

        }

        public void DrawMultiPrimitiveInstance()
        {

        }

        public void DrawMultiPrimitiveInstanceIndirect()
        {

        }

        protected override void Disposed()
        {
            NativeCmdList?.Dispose();
            NativeCmdAllocator?.Dispose();
        }
    }
}
