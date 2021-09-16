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
        internal EContextType contextType;
        internal ID3D12GraphicsCommandList5 d3dCmdList;
        internal ID3D12CommandAllocator d3dCmdAllocator;

        internal FRHICommandList(FRHIDevice device, EContextType contextType)
        {
            this.name = null;
            this.bClose = false;
            this.contextType = contextType;
            this.d3dCmdAllocator = device.d3dDevice.CreateCommandAllocator<ID3D12CommandAllocator>((CommandListType)contextType);
            this.d3dCmdList = device.d3dDevice.CreateCommandList<ID3D12GraphicsCommandList5>(0, (CommandListType)contextType, d3dCmdAllocator, null);
            this.d3dCmdList.QueryInterface<ID3D12GraphicsCommandList5>();
        }

        internal FRHICommandList(string name, FRHIDevice device, EContextType contextType)
        {
            this.name = name;
            this.bClose = false;
            this.contextType = contextType;
            this.d3dCmdAllocator = device.d3dDevice.CreateCommandAllocator<ID3D12CommandAllocator>((CommandListType)contextType);
            this.d3dCmdList = device.d3dDevice.CreateCommandList<ID3D12GraphicsCommandList5>(0, (CommandListType)contextType, d3dCmdAllocator, null);
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
            d3dCmdList.EndQuery(timeQuery.timestamp_Heap, QueryType.Timestamp, 0);
        }

        public void EndQuery(FRHITimeQuery timeQuery)
        {
            d3dCmdList.EndQuery(timeQuery.timestamp_Heap, QueryType.Timestamp, 1);
            d3dCmdList.ResolveQueryData(timeQuery.timestamp_Heap, QueryType.Timestamp, 0, 2, timeQuery.timestamp_Result, 0);
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

        public void SetRayTracePipelineState(FRHIRayTraceShader rayTraceShader, FRHIRayTracePipelineState rayTraceState)
        {

        }

        public void BuildAccelerationStructure()
        {

        }

        public void CopyAccelerationStructure()
        {

        }

        public void DispatchRay(FRHIRayTraceShader rayTraceShader, uint sizeX, uint sizeY, uint sizeZ)
        {

        }

        public void DispatchRayIndirect(FRHIRayTraceShader rayTraceShader, FRHIBuffer argsBuffer, uint argsOffset)
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

        public void SetShadingRate(FRHITexture texture)
        {
            d3dCmdList.RSSetShadingRateImage(texture.defaultResource);
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

    internal class FRHICommandListPool : FDisposable
    {
        private FRHIDevice m_Device;
        private EContextType m_ContextType;
        readonly bool m_CollectionCheck = true;
        readonly Stack<FRHICommandList> m_Stack = new Stack<FRHICommandList>();
        public int countAll { get; private set; }
        public int countActive { get { return countAll - countInactive; } }
        public int countInactive { get { return m_Stack.Count; } }

        internal FRHICommandListPool(FRHIDevice device, EContextType contextType, bool collectionCheck = true)
        {
            m_Device = device;
            m_ContextType = contextType;
            m_CollectionCheck = collectionCheck;
        }

        public FRHICommandList GetTemporary(string name)
        {
            FRHICommandList element;
            if (m_Stack.Count == 0)
            {
                element = new FRHICommandList(m_Device, m_ContextType);
                countAll++;
            } else {
                element = m_Stack.Pop();
            }
            element.name = name;
            return element;
        }

        public void ReleaseTemporary(FRHICommandList element)
        {
#if WITH_EDITOR // keep heavy checks in editor
            if (m_CollectionCheck && m_Stack.Count > 0)
            {
                if (m_Stack.Contains(element))
                    Console.WriteLine("Internal error. Trying to destroy object that is already released to pool.");
            }
#endif
            m_Stack.Push(element);
        }

        protected override void Release()
        {
            m_Device = null;
            foreach (FRHICommandList cmdList in m_Stack)
            {
                cmdList.Dispose();
            }
        }
    }
}
