using System;
using System.Collections.Generic;
using InfinityEngine.Core.Object;
using InfinityEngine.Core.Mathmatics.Geometry;

namespace InfinityEngine.Graphics.RHI
{
    public enum EExecuteType
    {
        Signal = 0,
        Wait = 1,
        Execute = 2
    }

    public enum EShadingRate
    {
        Rate1x1 = 0,
        Rate1x2 = 1,
        Rate2x1 = 4,
        Rate2x2 = 5,
        Rate2x4 = 6,
        Rate4x2 = 9,
        Rate4x4 = 10
    }

    public enum EPrimitiveTopology
    {
        LineList = 0,
        LineListWithAdjacency = 1,
        LineStrip = 2,
        LineStripWithAdjacency = 3,
        Patch_List = 4,
        PointList = 5,
        TriangleList = 6,
        TriangleListWithAdjacency = 7,
        TriangleStrip = 8,
        TriangleStripWithAdjacency = 9,
        Undefined = 10
    }

    public enum EShadingRateCombiner
    {
        Min = 0,
        Max = 1,
        Sum = 2,
        Override = 3,
        Passthrough = 4
    }

    internal struct FExecuteInfo
    {
        public FRHIFence fence;
        public EExecuteType executeType;
        public FRHICommandBuffer cmdBuffer;
        public FRHICommandContext cmdContext;
    }

    public abstract class FRHICommandBuffer : FDisposal
    {
        public string name;

        internal bool IsClose;
        internal EContextType contextType;

        protected EPrimitiveTopology m_TopologyType;

        internal FRHICommandBuffer(string name, FRHIDevice device, EContextType contextType) { }
        public abstract void Clear();
        internal abstract void Close();
        public abstract void BeginEvent(string name);
        public abstract void EndEvent();
        public abstract void BeginQuery(FRHIQuery query);
        public abstract void EndQuery(FRHIQuery query);
        public abstract void Barriers(in ReadOnlySpan<FResourceBarrierBatch> barrierBatch);
        public abstract void Transition(FRHIResource resource, EResourceState stateBefore, EResourceState stateAfter, int subresource = -1);
        public abstract void ClearBuffer(FRHIBuffer buffer);
        public abstract void ClearTexture(FRHITexture texture);
        public abstract void CopyBufferToBuffer(FRHIBuffer srcBuffer, FRHIBuffer dscBuffer);
        public abstract void CopyBufferToTexture(FRHIBuffer srcBuffer, FRHITexture dscTexture);
        public abstract void CopyTextureToBuffer(FRHITexture srcTexture, FRHIBuffer dscBuffer);
        public abstract void CopyTextureToTexture(FRHITexture srcTexture, FRHITexture dscTexture);
        public abstract void CopyAccelerationStructure();
        public abstract void BuildAccelerationStructure();
        public abstract void SetComputePipelineState(FRHIComputePipelineState computePipelineState);
        public abstract void SetComputeResourceBind(in uint slot, FRHIResourceSet resourceSet);
        public abstract void DispatchCompute(in uint sizeX, in uint sizeY, in uint sizeZ);
        public abstract void DispatchComputeIndirect(FRHIBuffer argsBuffer, in uint argsOffset);
        public abstract void SetRayTracePipelineState(FRHIRayTracePipelineState rayTracePipelineState);
        public abstract void SetRayTraceResourceBind(in uint slot, FRHIResourceSet resourceSet);
        public abstract void DispatchRay(in uint sizeX, in uint sizeY, in uint sizeZ);
        public abstract void DispatchRayIndirect(FRHIBuffer argsBuffer, in uint argsOffset);
        public abstract void SetScissor(in FRect rect);
        public abstract void SetViewport(in FViewport viewport);
        public abstract void BeginRenderPass(FRHITexture depthBuffer, params FRHITexture[] colorBuffer);
        public abstract void EndRenderPass();
        public abstract void SetStencilRef(in uint refValue);
        public abstract void SetBlendFactor(in float blendFactor);
        public abstract void SetDepthBound(in float min, in float max);
        public abstract void SetShadingRate(FRHITexture texture);
        public abstract void SetShadingRate(in EShadingRate shadingRate, in EShadingRateCombiner combiner);
        public abstract void SetPrimitiveTopology(in EPrimitiveTopology topologyType);
        public abstract void SetRenderPipelineState(FRHIRenderPipelineState renderPipelineState);
        public abstract void SetIndexBuffer(FRHIIndexBufferView indexBufferView);
        public abstract void SetVertexBuffer(in uint slot, FRHIVertexBufferView vertexBufferView);
        public abstract void SetRenderResourceBind(in uint slot, FRHIResourceSet resourceSet);
        public abstract void DrawIndexInstanced(in uint indexCount, in uint startIndex, in int startVertex, in uint instanceCount, in uint startInstance);
        public abstract void DrawMultiIndexInstanced(FRHIBuffer argsBuffer, in uint argsOffset, FRHIBuffer countBuffer, in uint countOffset);
        public abstract void DrawIndexInstancedIndirect(FRHIBuffer argsBuffer, in uint argsOffset);
    }

    internal class FRHICommandBufferPool : FDisposal
    {
        EContextType m_ContextType;
        Stack<FRHICommandBuffer> m_Pooled;
        FRHIGraphicsContext m_GraphicsContext;

        public int countAll { get; private set; }
        public int countActive { get { return countAll - countInactive; } }
        public int countInactive { get { return m_Pooled.Count; } }

        internal FRHICommandBufferPool(FRHIGraphicsContext graphicsContext, EContextType contextType)
        {
            m_ContextType = contextType;
            m_GraphicsContext = graphicsContext;
            m_Pooled = new Stack<FRHICommandBuffer>(64);
        }

        public FRHICommandBuffer GetTemporary(string name = null)
        {
            FRHICommandBuffer cmdBuffer;
            if (m_Pooled.Count == 0) {
                ++countAll;
                cmdBuffer = m_GraphicsContext.CreateCommandBuffer(m_ContextType, name);
            } else {
                cmdBuffer = m_Pooled.Pop();
            }
            cmdBuffer.name = name;
            return cmdBuffer;
        }

        public void ReleaseTemporary(FRHICommandBuffer cmdBuffer)
        {
            m_Pooled.Push(cmdBuffer);
        }

        protected override void Release()
        {
            m_GraphicsContext = null;
            foreach (FRHICommandBuffer cmdBuffer in m_Pooled)
            {
                cmdBuffer.Dispose();
            }
        }
    }
}
