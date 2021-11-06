using System.Collections.Generic;
using InfinityEngine.Core.Object;

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

    public enum EShadingRateCombiner
    {
        Min = 0,
        Max = 1,
        Sum = 2,
        Override = 3,
        Passthrough = 4
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
        internal bool IsClose;
        internal EContextType contextType;

        internal FRHICommandList(FRHIDevice device, EContextType contextType) { }
        internal FRHICommandList(string name, FRHIDevice device, EContextType contextType) { }

        public virtual void Clear() { }
        internal virtual void Close() { }
        public virtual void Barriers(FRHIResource resource) { }
        public virtual void Transitions(FRHIResource resource) { }
        public virtual void ClearBuffer(FRHIBuffer buffer) { }
        public virtual void ClearTexture(FRHITexture texture) { }
        public virtual void GenerateMipmaps(FRHITexture texture) { }
        public virtual void CopyBufferToBuffer(FRHIBuffer srcBuffer, FRHIBuffer dscBuffer) { }
        public virtual void CopyBufferToTexture(FRHIBuffer srcBuffer, FRHITexture dscTexture) { }
        public virtual void CopyTextureToBuffer(FRHITexture srcTexture, FRHIBuffer dscBuffer) { }
        public virtual void CopyTextureToTexture(FRHITexture srcTexture, FRHITexture dscTexture) { }
        public virtual void BeginQuery(FRHIQuery query) { }
        public virtual void EndQuery(FRHIQuery query) { }
        public virtual void SetComputePipelineState(FRHIComputePipelineState computeState) { }
        public virtual void DispatchCompute(FRHIComputeShader computeShader, uint sizeX, uint sizeY, uint sizeZ) { }
        public virtual void DispatchComputeIndirect(FRHIComputeShader computeShader, FRHIBuffer argsBuffer, uint argsOffset) { }
        public virtual void SetRayTracePipelineState(FRHIRayTraceShader rayTraceShader, FRHIRayTracePipelineState rayTraceState) { }
        public virtual void BuildAccelerationStructure() { }
        public virtual void CopyAccelerationStructure() { }
        public virtual void DispatchRay(FRHIRayTraceShader rayTraceShader, uint sizeX, uint sizeY, uint sizeZ) { }
        public virtual void DispatchRayIndirect(FRHIRayTraceShader rayTraceShader, FRHIBuffer argsBuffer, uint argsOffset) { }
        public virtual void SetScissor() { }
        public virtual void SetViewport() { }
        public virtual void BeginEvent() { }
        public virtual void EndEvent() { }
        public virtual void BeginRenderPass(FRHITexture depthBuffer, params FRHITexture[] colorBuffer) { }
        public virtual void EndRenderPass() { }
        public virtual void SetStencilRef() { }
        public virtual void SetBlendFactor() { }
        public virtual void SetDepthBounds(in float min, in float max) { }
        public virtual void SetShadingRate(in EShadingRate shadingRate, in EShadingRateCombiner[] combineMathdo) { }
        public virtual void SetShadingRate(FRHITexture texture) { }
        public virtual void SetGraphicsPipelineState(FRHIGraphicsShader graphicsShader, FRHIGraphicsPipelineState graphcisState) { }
        public virtual void DrawInstance(FRHIIndexBufferView indexBufferView, FRHIVertexBufferView vertexBufferView, EPrimitiveTopology topologyType, int indexCount, int startIndex, int startVertex, int instanceCount, int startInstance) { }
        public virtual void DrawInstanceIndirect(FRHIIndexBufferView indexBufferView, FRHIVertexBufferView vertexBufferView, EPrimitiveTopology topologyType, FRHIBuffer argsBuffer, uint argsOffset) { }
        public virtual void DrawMultiInstance(FRHIBuffer cmdBuffer, EPrimitiveTopology topologyType, FRHIBuffer argsBuffer, uint argsOffset) { }
        public virtual void DrawMultiInstanceIndirect() { }
    }

    internal class FRHICommandListPool : FDisposable
    {
        EContextType m_ContextType;
        bool m_CollectionCheck = true;
        Stack<FRHICommandList> m_StackPool;
        FRHIGraphicsContext m_GraphicsContext;

        public int countAll { get; private set; }
        public int countActive { get { return countAll - countInactive; } }
        public int countInactive { get { return m_StackPool.Count; } }

        internal FRHICommandListPool(FRHIGraphicsContext graphicsContext, EContextType contextType, bool collectionCheck = true)
        {
            m_ContextType = contextType;
            m_CollectionCheck = collectionCheck;
            m_GraphicsContext = graphicsContext;
            m_StackPool = new Stack<FRHICommandList>(64);
        }

        public FRHICommandList GetTemporary(string name = null)
        {
            FRHICommandList element;
            if (m_StackPool.Count == 0)
            {
                ++countAll;
                element = m_GraphicsContext.CreateCommandList(m_ContextType, name);
            }
            else
            {
                element = m_StackPool.Pop();
            }
            element.name = name;
            return element;
        }

        public void ReleaseTemporary(FRHICommandList element)
        {
            m_StackPool.Push(element);
        }

        protected override void Release()
        {
            m_GraphicsContext = null;
            foreach (FRHICommandList cmdList in m_StackPool)
            {
                cmdList.Dispose();
            }
        }
    }
}
