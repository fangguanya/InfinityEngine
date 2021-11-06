using Vortice.Direct3D;
using Vortice.Direct3D12;
using System.Collections.Generic;
using InfinityEngine.Core.Object;

namespace InfinityEngine.Graphics.RHI
{
    public class FD3DCommandList : FRHICommandList
    {
        internal ID3D12GraphicsCommandList5 nativeCmdList;
        internal ID3D12CommandAllocator nativeCmdAllocator;

        internal FD3DCommandList(FRHIDevice device, EContextType contextType) : base(device, contextType)
        {
            FD3DDevice d3dDevice = (FD3DDevice)device;

            this.name = null;
            this.IsClose = false;
            this.contextType = contextType;
            this.nativeCmdAllocator = d3dDevice.nativeDevice.CreateCommandAllocator<ID3D12CommandAllocator>((CommandListType)contextType);
            this.nativeCmdList = d3dDevice.nativeDevice.CreateCommandList<ID3D12GraphicsCommandList5>(0, (CommandListType)contextType, nativeCmdAllocator, null);
            this.nativeCmdList.QueryInterface<ID3D12GraphicsCommandList5>();
        }

        internal FD3DCommandList(string name, FRHIDevice device, EContextType contextType) : base(name, device, contextType)
        {
            FD3DDevice d3dDevice = (FD3DDevice)device;

            this.name = name;
            this.IsClose = false;
            this.contextType = contextType;
            this.nativeCmdAllocator = d3dDevice.nativeDevice.CreateCommandAllocator<ID3D12CommandAllocator>((CommandListType)contextType);
            this.nativeCmdList = d3dDevice.nativeDevice.CreateCommandList<ID3D12GraphicsCommandList5>(0, (CommandListType)contextType, nativeCmdAllocator, null);
            this.nativeCmdList.QueryInterface<ID3D12GraphicsCommandList5>();
        }

        public override void Clear()
        {
            if(!IsClose) { return; }

            IsClose = false;
            nativeCmdAllocator.Reset();
            nativeCmdList.Reset(nativeCmdAllocator, null);
        }

        internal override void Close()
        {
            IsClose = true;
            nativeCmdList.Close();
        }

        public override void Barriers(FRHIResource resource)
        {

        }

        public override void Transitions(FRHIResource resource)
        {

        }

        public override void ClearBuffer(FRHIBuffer buffer)
        {

        }

        public override void ClearTexture(FRHITexture texture)
        {

        }

        public override void GenerateMipmaps(FRHITexture texture)
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

        public override void BeginQuery(FRHIQuery query)
        {
            FD3DQuery d3dQuery = (FD3DQuery)query;
            nativeCmdList.EndQuery(d3dQuery.context.queryHeap, d3dQuery.context.queryType.GetNativeQueryType(), query.indexHead);
        }

        public override void EndQuery(FRHIQuery query)
        {
            FD3DQuery d3dQuery = (FD3DQuery)query;
            nativeCmdList.EndQuery(d3dQuery.context.queryHeap, d3dQuery.context.queryType.GetNativeQueryType(), query.indexLast);
        }

        public override void SetComputePipelineState(FRHIComputePipelineState computeState)
        {

        }

        public override void DispatchCompute(FRHIComputeShader computeShader, uint sizeX, uint sizeY, uint sizeZ)
        {

        }

        public override void DispatchComputeIndirect(FRHIComputeShader computeShader, FRHIBuffer argsBuffer, uint argsOffset)
        {

        }

        public override void SetRayTracePipelineState(FRHIRayTraceShader rayTraceShader, FRHIRayTracePipelineState rayTraceState)
        {

        }

        public override void BuildAccelerationStructure()
        {

        }

        public override void CopyAccelerationStructure()
        {

        }

        public override void DispatchRay(FRHIRayTraceShader rayTraceShader, uint sizeX, uint sizeY, uint sizeZ)
        {

        }

        public override void DispatchRayIndirect(FRHIRayTraceShader rayTraceShader, FRHIBuffer argsBuffer, uint argsOffset)
        {

        }

        public override void SetScissor()
        {

        }

        public override void SetViewport()
        {

        }

        public override void BeginEvent()
        {

        }

        public override void EndEvent()
        {

        }

        public override void BeginRenderPass(FRHITexture depthBuffer, params FRHITexture[] colorBuffer)
        {

        }

        public override void EndRenderPass()
        {
            nativeCmdList.EndRenderPass();
        }

        public override void SetStencilRef()
        {

        }

        public override void SetBlendFactor()
        {

        }

        public override void SetDepthBounds(in float min, in float max)
        {

        }

        public override void SetShadingRate(in EShadingRate shadingRate, in EShadingRateCombiner[] combineMathdo)
        {

        }

        public override void SetShadingRate(FRHITexture texture)
        {
            FD3DTexture d3dTexture = (FD3DTexture)texture;
            nativeCmdList.RSSetShadingRateImage(d3dTexture.defaultResource);
        }

        public override void SetGraphicsPipelineState(FRHIGraphicsShader graphicsShader, FRHIGraphicsPipelineState graphcisState)
        {

        }

        public override void DrawInstance(FRHIIndexBufferView indexBufferView, FRHIVertexBufferView vertexBufferView, EPrimitiveTopology topologyType, int indexCount, int startIndex, int startVertex, int instanceCount, int startInstance)
        {
            nativeCmdList.DrawIndexedInstanced(indexCount, instanceCount, startIndex, startVertex, startInstance);
        }

        public override void DrawInstanceIndirect(FRHIIndexBufferView indexBufferView, FRHIVertexBufferView vertexBufferView, EPrimitiveTopology topologyType, FRHIBuffer argsBuffer, uint argsOffset)
        {

        }

        public override void DrawMultiInstance(FRHIBuffer cmdBuffer, EPrimitiveTopology topologyType, FRHIBuffer argsBuffer, uint argsOffset)
        {

        }

        public override void DrawMultiInstanceIndirect()
        {

        }

        protected override void Release()
        {
            nativeCmdList?.Dispose();
            nativeCmdAllocator?.Dispose();
        }

        public static implicit operator ID3D12GraphicsCommandList5(FD3DCommandList cmdList) { return cmdList.nativeCmdList; }
    }

    internal class FD3DCommandListPool : FDisposable
    {
        private FRHIDevice m_Device;
        private EContextType m_ContextType;
        readonly bool m_CollectionCheck = true;
        readonly Stack<FD3DCommandList> m_StackPool;
        public int countAll { get; private set; }
        public int countActive { get { return countAll - countInactive; } }
        public int countInactive { get { return m_StackPool.Count; } }

        internal FD3DCommandListPool(FRHIDevice device, EContextType contextType, bool collectionCheck = true)
        {
            m_Device = device;
            m_ContextType = contextType;
            m_CollectionCheck = collectionCheck;
            m_StackPool = new Stack<FD3DCommandList>(64);
        }

        public FD3DCommandList GetTemporary(string name)
        {
            FD3DCommandList element;
            if (m_StackPool.Count == 0)
            {
                ++countAll;
                element = new FD3DCommandList(m_Device, m_ContextType);
            }
            else
            {
                element = m_StackPool.Pop();
            }
            element.name = name;
            return element;
        }

        public void ReleaseTemporary(FD3DCommandList element)
        {
            m_StackPool.Push(element);
        }

        protected override void Release()
        {
            m_Device = null;
            foreach (FD3DCommandList cmdList in m_StackPool)
            {
                cmdList.Dispose();
            }
        }
    }
}
