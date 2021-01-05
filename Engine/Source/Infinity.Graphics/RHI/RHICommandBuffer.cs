using Vortice.Direct3D;
using System.Collections.Generic;
using InfinityEngine.Core.Object;

namespace InfinityEngine.Graphics.RHI
{
    internal enum ERenderCommandType
    {
        GenerateMipmap,
        ResourceBarrier,
        WriteFence,
        WaitOnFence,
        BeginFrame,
        BeginEvent,
        BuildAccelerationStructure,
        BeginTimeQuery,
        BeginOcclusionQuery,
        BeginStatisticsQuery,
        EndFrame,
        EndEvent,
        EndTimeQuery,
        EndOcclusionQuery,
        EndStatisticsQuery,
        GetOcclusionQueryResult,
        GetStatisticsQueryResult,
        GetTimeQueryResult,
        ClearBuffer,
        ClearTexture,
        CopyBufferToBuffer,
        CopyBufferToTexture,
        CopyTextureToBuffer,
        CopyTextureToTexture,
        CopyAccelerationStructure,
        SetAccelerationStructure,
        SetComputeState,
        SetComputeSampler,
        SetComputeBuffer,
        SetComputeTexture,
        SetRayGenState,
        SetRayGenSampler,
        SetRayGenBuffer,
        SetRayGenTexture,
        SetViewport,
        SetScissorRect,
        SetRenderTarget,
        SetRenderTargets,
        SetRandomWriteTarget,
        SetStencilRef,
        SetBlendFactor,
        SetDepthBounds,
        SetShadingRate,
        SetShadingRateIndirect,
        SetGraphicsState,
        SetGraphicsSampler,
        SetGraphicsBuffer,
        SetGraphicsTexture,
        DispatchRay,
        DispatchRayIndirect,
        DispatchCompute,
        DispatchComputeIndirect,
        DrawPrimitiveInstance,
        DrawPrimitiveInstanceIndirect,
        DrawMultiPrimitiveInstance,
        DrawMultiPrimitiveInstanceIndirect
    };

    internal interface IRenderCommand
    {
        ERenderCommandType GetRenderCmdType { get;}
    }

    internal struct RenderCommandGenerateMipmap : IRenderCommand
    {
        internal ERenderCommandType RenderCmdType;

        public ERenderCommandType GetRenderCmdType => RenderCmdType;
    };

    internal struct RenderCommandResourceBarrier : IRenderCommand
    {
        internal ERenderCommandType RenderCmdType;

        public ERenderCommandType GetRenderCmdType => RenderCmdType;
    };

    internal struct RenderCommandWriteFence : IRenderCommand
    {
        internal ERenderCommandType RenderCmdType;

        internal RenderCommandWriteFence(ERenderCommandType InRenderCmdType)
        {
            this.RenderCmdType = InRenderCmdType;

        }

        public ERenderCommandType GetRenderCmdType => RenderCmdType;
    };

    internal struct RenderCommandWaitOnFence : IRenderCommand
    {
        internal ERenderCommandType RenderCmdType;

        public ERenderCommandType GetRenderCmdType => RenderCmdType;
    };

    internal struct RenderCommandBeginFrame : IRenderCommand
    {
        internal ERenderCommandType RenderCmdType;

        public ERenderCommandType GetRenderCmdType => RenderCmdType;
    };

    internal struct RenderCommandBeginEvent : IRenderCommand
    {
        internal ERenderCommandType RenderCmdType;

        public ERenderCommandType GetRenderCmdType => RenderCmdType;
    };

    internal struct RenderCommandBeginTimeQuery : IRenderCommand
    {
        internal ERenderCommandType RenderCmdType;

        public ERenderCommandType GetRenderCmdType => RenderCmdType;
    };

    internal struct RenderCommandBeginOcclusionQuery : IRenderCommand
    {
        internal ERenderCommandType RenderCmdType;

        public ERenderCommandType GetRenderCmdType => RenderCmdType;
    };

    internal struct RenderCommandBeginStatisticsQuery : IRenderCommand
    {
        internal ERenderCommandType RenderCmdType;

        public ERenderCommandType GetRenderCmdType => RenderCmdType;
    };

    internal struct RenderCommandBuildAccelerationStructure : IRenderCommand
    {
        internal ERenderCommandType RenderCmdType;

        public ERenderCommandType GetRenderCmdType => RenderCmdType;
    };

    internal struct RenderCommandEndFrame : IRenderCommand
    {
        internal ERenderCommandType RenderCmdType;

        public ERenderCommandType GetRenderCmdType => RenderCmdType;
    };

    internal struct RenderCommandEndEvent : IRenderCommand
    {
        internal ERenderCommandType RenderCmdType;

        public ERenderCommandType GetRenderCmdType => RenderCmdType;
    };

    internal struct RenderCommandEndTimeQuery : IRenderCommand
    {
        internal ERenderCommandType RenderCmdType;

        public ERenderCommandType GetRenderCmdType => RenderCmdType;
    };

    internal struct RenderCommandEndOcclusionQuery : IRenderCommand
    {
        internal ERenderCommandType RenderCmdType;

        public ERenderCommandType GetRenderCmdType => RenderCmdType;
    };

    internal struct RenderCommandEndStatisticsQuery : IRenderCommand
    {
        internal ERenderCommandType RenderCmdType;

        public ERenderCommandType GetRenderCmdType => RenderCmdType;
    };

    internal struct RenderCommandGetTimeQueryResult : IRenderCommand
    {
        internal ERenderCommandType RenderCmdType;

        public ERenderCommandType GetRenderCmdType => RenderCmdType;
    };

    internal struct RenderCommandGetOcclusionQueryResult : IRenderCommand
    {
        internal ERenderCommandType RenderCmdType;

        public ERenderCommandType GetRenderCmdType => RenderCmdType;
    };

    internal struct RenderCommandGetStatisticsQueryResult : IRenderCommand
    {
        internal ERenderCommandType RenderCmdType;

        public ERenderCommandType GetRenderCmdType => RenderCmdType;
    };

    internal struct RenderCommandClearBuffer : IRenderCommand
    {
        internal ERenderCommandType RenderCmdType;

        public ERenderCommandType GetRenderCmdType => RenderCmdType;
    };

    internal struct RenderCommandClearTexture : IRenderCommand
    {
        internal ERenderCommandType RenderCmdType;

        public ERenderCommandType GetRenderCmdType => RenderCmdType;
    };

    internal struct RenderCommandCopyBufferToBuffer : IRenderCommand
    {
        internal ERenderCommandType RenderCmdType;

        public ERenderCommandType GetRenderCmdType => RenderCmdType;
    };

    internal struct RenderCommandCopyBufferToTexture : IRenderCommand
    {
        internal ERenderCommandType RenderCmdType;

        public ERenderCommandType GetRenderCmdType => RenderCmdType;
    };

    internal struct RenderCommandCopyTextureToBuffer : IRenderCommand
    {
        internal ERenderCommandType RenderCmdType;

        public ERenderCommandType GetRenderCmdType => RenderCmdType;
    };

    internal struct RenderCommandCopyTextureToTexture : IRenderCommand
    {
        internal ERenderCommandType RenderCmdType;

        public ERenderCommandType GetRenderCmdType => RenderCmdType;
    };

    internal struct RenderCommandCopyAccelerationStructure : IRenderCommand
    {
        internal ERenderCommandType RenderCmdType;

        public ERenderCommandType GetRenderCmdType => RenderCmdType;
    };

    internal struct RenderCommandSetAccelerationStructure : IRenderCommand
    {
        internal ERenderCommandType RenderCmdType;

        public ERenderCommandType GetRenderCmdType => RenderCmdType;
    };

    internal struct RenderCommandSetComputeState : IRenderCommand
    {
        internal ERenderCommandType RenderCmdType;

        public ERenderCommandType GetRenderCmdType => RenderCmdType;
    };

    internal struct RenderCommandSetComputeSampler : IRenderCommand
    {
        internal ERenderCommandType RenderCmdType;

        public ERenderCommandType GetRenderCmdType => RenderCmdType;
    };

    internal struct RenderCommandSetComputeBuffer : IRenderCommand
    {
        internal ERenderCommandType RenderCmdType;

        public ERenderCommandType GetRenderCmdType => RenderCmdType;
    };

    internal struct RenderCommandSetComputeTexture : IRenderCommand
    {
        internal ERenderCommandType RenderCmdType;

        public ERenderCommandType GetRenderCmdType => RenderCmdType;
    };

    internal struct RenderCommandSetRayGenState : IRenderCommand
    {
        internal ERenderCommandType RenderCmdType;

        public ERenderCommandType GetRenderCmdType => RenderCmdType;
    };

    internal struct RenderCommandSetRayGenSampler : IRenderCommand
    {
        internal ERenderCommandType RenderCmdType;

        public ERenderCommandType GetRenderCmdType => RenderCmdType;
    };

    internal struct RenderCommandSetRayGenBuffer : IRenderCommand
    {
        internal ERenderCommandType RenderCmdType;

        public ERenderCommandType GetRenderCmdType => RenderCmdType;
    };

    internal struct RenderCommandSetRayGenTexture : IRenderCommand
    {
        internal ERenderCommandType RenderCmdType;

        public ERenderCommandType GetRenderCmdType => RenderCmdType;
    };

    internal struct RenderCommandSetViewport : IRenderCommand
    {
        internal ERenderCommandType RenderCmdType;

        public ERenderCommandType GetRenderCmdType => RenderCmdType;
    };

    internal struct RenderCommandSetScissorRect : IRenderCommand
    {
        internal ERenderCommandType RenderCmdType;

        public ERenderCommandType GetRenderCmdType => RenderCmdType;
    };

    internal struct RenderCommandSetRenderTarget : IRenderCommand
    {
        internal ERenderCommandType RenderCmdType;

        public ERenderCommandType GetRenderCmdType => RenderCmdType;
    };

    internal struct RenderCommandSetRenderTargets : IRenderCommand
    {
        internal ERenderCommandType RenderCmdType;

        public ERenderCommandType GetRenderCmdType => RenderCmdType;
    };

    internal struct RenderCommandSetRandomWriteTarget : IRenderCommand
    {
        internal ERenderCommandType RenderCmdType;

        public ERenderCommandType GetRenderCmdType => RenderCmdType;
    };

    internal struct RenderCommandSetStencilRef : IRenderCommand
    {
        internal ERenderCommandType RenderCmdType;

        public ERenderCommandType GetRenderCmdType => RenderCmdType;
    };

    internal struct RenderCommandSetBlendFactor : IRenderCommand
    {
        internal ERenderCommandType RenderCmdType;

        public ERenderCommandType GetRenderCmdType => RenderCmdType;
    };

    internal struct RenderCommandSetDepthBounds : IRenderCommand
    {
        internal ERenderCommandType RenderCmdType;

        public ERenderCommandType GetRenderCmdType => RenderCmdType;
    };

    internal struct RenderCommandSetShadingRate : IRenderCommand
    {
        internal ERenderCommandType RenderCmdType;

        public ERenderCommandType GetRenderCmdType => RenderCmdType;
    };

    internal struct RenderCommandSetShadingRateIndirect : IRenderCommand
    {
        internal ERenderCommandType RenderCmdType;

        public ERenderCommandType GetRenderCmdType => RenderCmdType;
    };

    internal struct RenderCommandSetGraphicsState : IRenderCommand
    {
        internal ERenderCommandType RenderCmdType;

        public ERenderCommandType GetRenderCmdType => RenderCmdType;
    };

    internal struct RenderCommandSetGraphicsSampler : IRenderCommand
    {
        internal ERenderCommandType RenderCmdType;

        public ERenderCommandType GetRenderCmdType => RenderCmdType;
    };

    internal struct RenderCommandSetGraphicsBuffer : IRenderCommand
    {
        internal ERenderCommandType RenderCmdType;

        public ERenderCommandType GetRenderCmdType => RenderCmdType;
    };

    internal struct RenderCommandSetGraphicsTexture : IRenderCommand
    {
        internal ERenderCommandType RenderCmdType;

        public ERenderCommandType GetRenderCmdType => RenderCmdType;
    };

    internal struct RenderCommandDispatchRay : IRenderCommand
    {
        internal ERenderCommandType RenderCmdType;

        public ERenderCommandType GetRenderCmdType => RenderCmdType;
    };

    internal struct RenderCommandDispatchRayIndirect : IRenderCommand
    {
        internal ERenderCommandType RenderCmdType;

        public ERenderCommandType GetRenderCmdType => RenderCmdType;
    };

    internal struct RenderCommandDispatchCompute : IRenderCommand
    {
        internal ERenderCommandType RenderCmdType;
        internal RHIComputeShader shader;
        internal uint x;
        internal uint y;
        internal uint z;
        public ERenderCommandType GetRenderCmdType => RenderCmdType;
    };

    internal struct RenderCommandDispatchComputeIndirect : IRenderCommand
    {
        internal ERenderCommandType RenderCmdType;

        public ERenderCommandType GetRenderCmdType => RenderCmdType;
    };

    internal struct RenderCommandDrawPrimitiveInstance : IRenderCommand
    {
        internal ERenderCommandType RenderCmdType;
        internal uint IndexCount;
        internal uint InstanceCount;
        internal RHIBuffer IndexBuffer;
        internal RHIBuffer VertexBuffer;
        internal PrimitiveTopology TopologyType; 
        public ERenderCommandType GetRenderCmdType => RenderCmdType;
    };

    internal struct RenderCommandDrawPrimitiveInstanceIndirect : IRenderCommand
    {
        internal ERenderCommandType RenderCmdType;

        public ERenderCommandType GetRenderCmdType => RenderCmdType;
    };

    internal struct RenderCommandDrawMultiPrimitiveInstance : IRenderCommand
    {
        internal ERenderCommandType RenderCmdType;

        public ERenderCommandType GetRenderCmdType => RenderCmdType;
    };

    internal struct RenderCommandDrawMultiPrimitiveInstanceIndirect : IRenderCommand
    {
        internal ERenderCommandType RenderCmdType;

        public ERenderCommandType GetRenderCmdType => RenderCmdType;
    };

    public class RHICommandBuffer : UObject
    {
        public string name;
        internal bool bASyncCompute;
        internal List<IRenderCommand> CmdList;

        public RHICommandBuffer(string InName)
        {
            name = InName;
            bASyncCompute = false;
            CmdList = new List<IRenderCommand>(128);
        }

        public void Clear()
        {
            CmdList.Clear();
        }

        internal int Size()
        {
            return CmdList.Count;
        }

        public void WriteFence(RHIFence GPUFence)
        {
            RenderCommandWriteFence RenderCmdWriteFence = new RenderCommandWriteFence(ERenderCommandType.WriteFence);
            CmdList.Add(RenderCmdWriteFence);
        }

        public void WaitFence(RHIFence GPUFence)
        {

        }

        public void ClearBuffer()
        {

        }

        public void ClearTexture()
        {

        }

        public void CopyBufferToBuffer()
        {

        }

        public void CopyBufferToTexture()
        {

        }

        public void CopyTextureToBuffer()
        {

        }

        public void CopyTextureToTexture()
        {

        }

        public void GenerateMipmaps()
        {

        }

        public void ResourceBarrier()
        {

        }

        public void BeginTimeQuery()
        {

        }

        public void EndTimeQuery()
        {

        }

        public float GetTimeQueryResult()
        {
            return 1;
        }

        public void SetComputePipelineState()
        {

        }

        public void SetComputeSamplerState()
        {

        }

        public void SetComputeBuffer()
        {

        }

        public void SetComputeTexture()
        {

        }

        public void DispatchCompute()
        {

        }

        public void DispatchComputeIndirect()
        {

        }

        public void BuildAccelerationStructure()
        {

        }

        public void CopyAccelerationStructure()
        {

        }

        public void SetAccelerationStructure()
        {

        }

        public void SetRayGenPipelineState()
        {

        }

        public void SetRayGenSamplerState()
        {

        }

        public void SetRayGenBuffer()
        {

        }

        public void SetRayGenTexture()
        {

        }

        public void DispatchRay()
        {

        }

        public void DispatchRayIndirect()
        {

        }

        public void BeginOcclusionQuery()
        {

        }

        public void EndOcclusionQuery()
        {

        }

        public float GetOcclusionQueryResult()
        {
            return 1;
        }

        public void BeginStatisticsQuery()
        {

        }

        public void EndStatisticsQuery()
        {

        }

        public float GetStatisticsQueryResult()
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

        public void SetRenderTarget()
        {

        }

        public void SetRenderTargets()
        {

        }

        public void SetRandomWriteTarget()
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

        public void SetShadingRate()
        {

        }

        public void SetShadingRateIndirect()
        {

        }

        public void SetGraphicsPipelineState()
        {

        }

        public void SetGraphicsSamplerState()
        {

        }

        public void SetGraphicsBuffer()
        {

        }

        public void SetGraphicsTexture()
        {

        }

        public void DrawPrimitiveInstance()
        {

        }

        public void DrawPrimitiveInstanceIndirect()
        {

        }

        public void DrawMultiPrimitiveInstance()
        {

        }

        public void DrawMultiPrimitiveInstanceIndirect()
        {

        }

        internal RHICommandBuffer Clone()
        {
            RHICommandBuffer CopyCmdBuffer = new RHICommandBuffer(this.name);
            CopyCmdBuffer.CmdList = new List<IRenderCommand>(CmdList);
            return CopyCmdBuffer;
        }

        protected override void DisposeManaged()
        {

        }

        protected override void DisposeUnManaged()
        {

        }
    }
}
