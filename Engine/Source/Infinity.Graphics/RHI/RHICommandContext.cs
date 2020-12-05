using System.Threading;
using Vortice.Direct3D;
using Vortice.Direct3D12;
using InfinityEngine.Core.Object;

namespace InfinityEngine.Graphics.RHI
{
    public class RHICommandContext : FObject
    {
        public RHIFence FrameFence;
        public ManualResetEvent FenceEvent;

        public ID3D12CommandQueue NativeCmdQueue;
        public ID3D12GraphicsCommandList6 NativeCmdList;
        public ID3D12CommandAllocator NativeCmdAllocator;


        public RHICommandContext(ID3D12Device6 NativeDevice, CommandListType CommandBufferType) : base()
        {
            FenceEvent = new ManualResetEvent(false);
            FrameFence = new RHIFence(NativeDevice, this, this);
            NativeCmdQueue = NativeDevice.CreateCommandQueue(CommandBufferType);
            NativeCmdAllocator = NativeDevice.CreateCommandAllocator(CommandBufferType);
            NativeCmdList = NativeDevice.CreateCommandList<ID3D12GraphicsCommandList6>(0, CommandBufferType, NativeCmdAllocator, null);
        }

        public void WriteFence(RHIFence GPUFence)
        {
            GPUFence.Signal();
        }

        public void WaitFence(RHIFence GPUFence)
        {
            GPUFence.WaitOnGPU();
        }

        public void Reset()
        {
            NativeCmdAllocator.Reset();
            NativeCmdList.Reset(NativeCmdAllocator, null);
        }

        public void Execute()
        {
            NativeCmdList.Close();
            NativeCmdQueue.ExecuteCommandList(NativeCmdList);
        }

        public void Flush()
        {
            FrameFence.Signal();
            FrameFence.WaitOnCPU(FenceEvent);
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

    public class RHICopyCmdContext : RHICommandContext
    {
        public RHICopyCmdContext(ID3D12Device6 NativeDevice, CommandListType CommandBufferType) : base(NativeDevice, CommandBufferType)
        {

        }

        public void ClearTexture(RHITexture GPUTexture)
        {

        }

        public void ClearBuffer(RHIBuffer GPUBuffer)
        {

        }

        public void CopyBufferToBuffer(RHIBuffer SourceBuffer, RHIBuffer DescBuffer)
        {

        }

        public void CopyBufferToTexture(RHIBuffer SourceBuffer, RHITexture DescTexture)
        {

        }

        public void CopyTextureToBuffer(RHITexture SourceTexture, RHIBuffer DescBuffer)
        {

        }

        public void CopyTextureToTexture(RHITexture SourceTexture, RHITexture DescTexture)
        {

        }

        public void GenerateMipmaps(RHITexture GPUTexture)
        {

        }

        public void ResourceBarrier()
        {

        }

        protected override void DisposeManaged()
        {
            base.DisposeManaged();
        }

        protected override void DisposeUnManaged()
        {
            base.DisposeUnManaged();
        }
    }

    public class RHIComputeCmdContext : RHICopyCmdContext
    {
        public RHIComputeCmdContext(ID3D12Device6 NativeDevice, CommandListType CommandBufferType) : base(NativeDevice, CommandBufferType)
        {

        }

        public void BeginTimeQuery(RHITimeQuery TimeQuery)
        {
            TimeQuery.Begin();
        }

        public void EndTimeQuery(RHITimeQuery TimeQuery)
        {
            TimeQuery.End();
        }

        public float GetTimeQueryResult(RHITimeQuery TimeQuery)
        {
            return TimeQuery.GetQueryResult(NativeCmdQueue.TimestampFrequency);
        }

        public void SetComputePipelineState(RHIComputeShader ComputeShader, RHIComputePipelineState ComputeState)
        {

        }

        public void SetComputeSamplerState(RHIComputeShader ComputeShader)
        {

        }

        public void SetComputeBuffer(RHIComputeShader ComputeShader, RHIBuffer GPUBuffer)
        {

        }

        public void SetComputeTexture(RHIComputeShader ComputeShader, RHITexture GPUTexture)
        {

        }

        public void DispatchCompute(RHIComputeShader ComputeShader, uint SizeX, uint SizeY, uint SizeZ)
        {

        }

        public void DispatchComputeIndirect(RHIComputeShader ComputeShader, RHIBuffer ArgsBuffer, uint ArgsOffset)
        {

        }

        public void BuildAccelerationStructure()
        {

        }

        public void CopyAccelerationStructure()
        {

        }

        public void SetAccelerationStructure(RHIRayGenShader RayGenShader)
        {

        }

        public void SetRayTracePipelineState(RHIRayGenShader RayGenShader, RHIRayTracePipelineState RayTraceState)
        {

        }

        public void SetRayTraceSamplerState(RHIRayGenShader RayGenShader)
        {

        }

        public void SetRayTraceBuffer(RHIRayGenShader RayGenShader, RHIBuffer GPUBuffer)
        {

        }

        public void SetRayTraceTexture(RHIRayGenShader RayGenShader, RHITexture GPUTexture)
        {

        }

        public void DispatchRay(RHIRayGenShader RayGenShader, uint SizeX, uint SizeY, uint SizeZ)
        {

        }

        public void DispatchRayIndirect(RHIRayGenShader RayGenShader, RHIBuffer ArgsBuffer, uint ArgsOffset)
        {

        }

        protected override void DisposeManaged()
        {
            base.DisposeManaged();
        }

        protected override void DisposeUnManaged()
        {
            base.DisposeUnManaged();
        }
    }

    public class RHIGraphicsCmdContext : RHIComputeCmdContext
    {
        public RHIGraphicsCmdContext(ID3D12Device6 NativeDevice, CommandListType CommandBufferType) : base(NativeDevice, CommandBufferType)
        {

        }

        public void BeginOcclusionQuery(RHIOcclusionQuery OcclusionQuery)
        {
            OcclusionQuery.Begin();
        }

        public void EndOcclusionQuery(RHIOcclusionQuery OcclusionQuery)
        {
            OcclusionQuery.End();
        }

        public int GetOcclusionQueryResult(RHIOcclusionQuery OcclusionQuery)
        {
            return OcclusionQuery.GetQueryResult();
        }

        public void BeginStatisticsQuery(RHIStatisticsQuery StatisticsQuery)
        {

        }

        public void EndStatisticsQuery(RHIStatisticsQuery StatisticsQuery)
        {

        }

        public float GetStatisticsQueryResult(RHIStatisticsQuery StatisticsQuery)
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

        public void BeginRenderPass(RHITexture[] ColorBuffer, RHITexture DepthBuffer)
        {

        }

        public void EndRenderPass(RHITexture[] ColorBuffer, RHITexture DepthBuffer)
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

        public void SetShadingRateIndirect(RHITexture IndirectTexture)
        {
            NativeCmdList.RSSetShadingRateImage(IndirectTexture.DefaultResource);
        }

        public void SetGraphicsPipelineState(RHIGraphicsShader GraphicsShader, RHIGraphicsPipelineState GraphcisState)
        {

        }

        public void SetGraphicsSamplerState(RHIGraphicsShader GraphicsShader, string PropertyName)
        {

        }

        public void SetGraphicsBuffer(RHIGraphicsShader GraphicsShader, string PropertyName, RHIBuffer GPUBuffer)
        {

        }

        public void SetGraphicsTexture(RHIGraphicsShader GraphicsShader, string PropertyName, RHITexture GPUTexture)
        {

        }

        public void DrawPrimitiveInstance(RHIBuffer IndexBuffer, RHIBuffer VertexBuffer, PrimitiveTopology TopologyType, uint IndexCount, uint InstanceCount)
        {

        }

        public void DrawPrimitiveInstanceIndirect(RHIBuffer IndexBuffer, RHIBuffer VertexBuffer, PrimitiveTopology TopologyType, RHIBuffer ArgsBuffer, uint ArgsOffset)
        {

        }

        public void DrawMultiPrimitiveInstance()
        {

        }

        public void DrawMultiPrimitiveInstanceIndirect()
        {

        }

        protected override void DisposeManaged()
        {
            base.DisposeManaged();
        }

        protected override void DisposeUnManaged()
        {
            base.DisposeUnManaged();
        }
    }
}
