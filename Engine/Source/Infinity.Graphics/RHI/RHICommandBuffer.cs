using Vortice.Direct3D;
using Vortice.Direct3D12;
using InfinityEngine.Core.Object;

namespace InfinityEngine.Graphics.RHI
{
    public enum ECmdBufferExecuteType
    {
        Execute = 1,
        WaitFence = 2,
        WriteFence = 3
    }

    public class FRHICommandBuffer : UObject
    {
        public string name;
        internal ID3D12GraphicsCommandList6 NativeCmdList;
        internal ID3D12CommandAllocator NativeCmdAllocator;

        public FRHICommandBuffer(string InName, ID3D12Device6 NativeDevice, CommandListType CommandBufferType)
        {
            name = InName;
            NativeCmdAllocator = NativeDevice.CreateCommandAllocator(CommandBufferType);
            NativeCmdList = NativeDevice.CreateCommandList<ID3D12GraphicsCommandList6>(0, CommandBufferType, NativeCmdAllocator, null);
        }

        public void Reset()
        {
            NativeCmdAllocator.Reset();
            NativeCmdList.Reset(NativeCmdAllocator, null);
        }

        internal void Close()
        {
            NativeCmdList.Close();
        }

        public void ClearTexture(FRHITexture GPUTexture)
        {

        }

        public void ClearBuffer(FRHIBuffer GPUBuffer)
        {

        }

        public void CopyBufferToBuffer(FRHIBuffer SourceBuffer, FRHIBuffer DescBuffer)
        {

        }

        public void CopyBufferToTexture(FRHIBuffer SourceBuffer, FRHITexture DescTexture)
        {

        }

        public void CopyTextureToBuffer(FRHITexture SourceTexture, FRHIBuffer DescBuffer)
        {

        }

        public void CopyTextureToTexture(FRHITexture SourceTexture, FRHITexture DescTexture)
        {

        }

        public void GenerateMipmaps(FRHITexture GPUTexture)
        {

        }

        public void BeginTimeQuery(FRHITimeQuery TimeQuery)
        {
            TimeQuery.Begin();
        }

        public void EndTimeQuery(FRHITimeQuery TimeQuery)
        {
            TimeQuery.End();
        }

        public float GetTimeQueryResult(FRHITimeQuery TimeQuery)
        {
            return 1;
            //return TimeQuery.GetQueryResult(NativeCmdQueue.TimestampFrequency);
        }

        public void SetComputePipelineState(FRHIComputeShader ComputeShader, FRHIComputePipelineState ComputeState)
        {

        }

        public void SetComputeSamplerState(FRHIComputeShader ComputeShader)
        {

        }

        public void SetComputeBuffer(FRHIComputeShader ComputeShader, FRHIBuffer GPUBuffer)
        {

        }

        public void SetComputeTexture(FRHIComputeShader ComputeShader, FRHITexture GPUTexture)
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

        public void SetRayTraceBuffer(FRHIRayGenShader RayGenShader, FRHIBuffer GPUBuffer)
        {

        }

        public void SetRayTraceTexture(FRHIRayGenShader RayGenShader, FRHITexture GPUTexture)
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
            OcclusionQuery.Begin();
        }

        public void EndOcclusionQuery(FRHIOcclusionQuery OcclusionQuery)
        {
            OcclusionQuery.End();
        }

        public int GetOcclusionQueryResult(FRHIOcclusionQuery OcclusionQuery)
        {
            return OcclusionQuery.GetQueryResult();
        }

        public void BeginStatisticsQuery(FRHIStatisticsQuery StatisticsQuery)
        {

        }

        public void EndStatisticsQuery(FRHIStatisticsQuery StatisticsQuery)
        {

        }

        public float GetStatisticsQueryResult(FRHIStatisticsQuery StatisticsQuery)
        {
            return 1;
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

        public void BeginRenderPass(FRHITexture[] ColorBuffer, FRHITexture DepthBuffer)
        {

        }

        public void EndRenderPass(FRHITexture[] ColorBuffer, FRHITexture DepthBuffer)
        {

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

        public void SetGraphicsBuffer(FRHIGraphicsShader GraphicsShader, string PropertyName, FRHIBuffer GPUBuffer)
        {

        }

        public void SetGraphicsTexture(FRHIGraphicsShader GraphicsShader, string PropertyName, FRHITexture GPUTexture)
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
        public void ResourceBarrier()
        {

        }

        protected override void DisposeManaged()
        {

        }

        protected override void DisposeUnManaged()
        {
            NativeCmdList.Dispose();
            NativeCmdAllocator.Dispose();
        }
    }
}
