using Vortice.Direct3D;
using Vortice.Direct3D12;
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
        internal ID3D12GraphicsCommandList5 nativeCmdList;
        internal ID3D12CommandAllocator nativeCmdAllocator;

        internal FRHICommandList(FRHIDevice device, EContextType contextType)
        {
            this.name = null;
            this.IsClose = false;
            this.contextType = contextType;
            this.nativeCmdAllocator = device.nativeDevice.CreateCommandAllocator<ID3D12CommandAllocator>((CommandListType)contextType);
            this.nativeCmdList = device.nativeDevice.CreateCommandList<ID3D12GraphicsCommandList5>(0, (CommandListType)contextType, nativeCmdAllocator, null);
            this.nativeCmdList.QueryInterface<ID3D12GraphicsCommandList5>();
        }

        internal FRHICommandList(string name, FRHIDevice device, EContextType contextType)
        {
            this.name = name;
            this.IsClose = false;
            this.contextType = contextType;
            this.nativeCmdAllocator = device.nativeDevice.CreateCommandAllocator<ID3D12CommandAllocator>((CommandListType)contextType);
            this.nativeCmdList = device.nativeDevice.CreateCommandList<ID3D12GraphicsCommandList5>(0, (CommandListType)contextType, nativeCmdAllocator, null);
            this.nativeCmdList.QueryInterface<ID3D12GraphicsCommandList5>();
        }

        public void Clear()
        {
            if(!IsClose) { return; }

            IsClose = false;
            nativeCmdAllocator.Reset();
            nativeCmdList.Reset(nativeCmdAllocator, null);
        }

        internal void Close()
        {
            IsClose = true;
            nativeCmdList.Close();
        }

        public void Barriers(FRHIResource resource)
        {

        }

        public void Transitions(FRHIResource resource)
        {

        }

        public void ClearBuffer(FRHIBuffer buffer)
        {

        }

        public void ClearTexture(FRHITexture texture)
        {

        }

        public void GenerateMipmaps(FRHITexture texture)
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

        public void BeginQuery(FRHIQuery query)
        {
            nativeCmdList.EndQuery(query.queryPool.queryHeap, query.queryPool.queryType.GetNativeQueryType(), query.indexHead);
        }

        public void EndQuery(FRHIQuery query)
        {
            nativeCmdList.EndQuery(query.queryPool.queryHeap, query.queryPool.queryType.GetNativeQueryType(), query.indexLast);
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

        public void SetScissor()
        {

        }

        public void SetViewport()
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
            nativeCmdList.EndRenderPass();
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
            nativeCmdList.RSSetShadingRate(shadingRate, combineMathdo);
        }

        public void SetShadingRate(FRHITexture texture)
        {
            nativeCmdList.RSSetShadingRateImage(texture.defaultResource);
        }

        public void SetGraphicsPipelineState(FRHIGraphicsShader graphicsShader, FRHIGraphicsPipelineState graphcisState)
        {

        }

        public void DrawInstance(FRHIIndexBufferView indexBufferView, FRHIVertexBufferView vertexBufferView, PrimitiveTopology topologyType, int indexCount, int startIndex, int startVertex, int instanceCount, int startInstance)
        {
            nativeCmdList.IASetPrimitiveTopology(topologyType);
            nativeCmdList.IASetIndexBuffer(indexBufferView.nativeBufferView);
            nativeCmdList.IASetVertexBuffers(0, vertexBufferView.nativeBufferView);
            nativeCmdList.DrawIndexedInstanced(indexCount, instanceCount, startIndex, startVertex, startInstance);
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
            nativeCmdList?.Dispose();
            nativeCmdAllocator?.Dispose();
        }

        public static implicit operator ID3D12GraphicsCommandList5(FRHICommandList cmdList) { return cmdList.nativeCmdList; }
    }

    internal class FRHICommandListPool : FDisposable
    {
        private FRHIDevice m_Device;
        private EContextType m_ContextType;
        readonly bool m_CollectionCheck = true;
        readonly Stack<FRHICommandList> m_StackPool;
        public int countAll { get; private set; }
        public int countActive { get { return countAll - countInactive; } }
        public int countInactive { get { return m_StackPool.Count; } }

        internal FRHICommandListPool(FRHIDevice device, EContextType contextType, bool collectionCheck = true)
        {
            m_Device = device;
            m_ContextType = contextType;
            m_CollectionCheck = collectionCheck;
            m_StackPool = new Stack<FRHICommandList>(64);
        }

        public FRHICommandList GetTemporary(string name)
        {
            FRHICommandList element;
            if (m_StackPool.Count == 0)
            {
                element = new FRHICommandList(m_Device, m_ContextType);
                countAll++;
            } else {
                element = m_StackPool.Pop();
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
            m_StackPool.Push(element);
        }

        protected override void Release()
        {
            m_Device = null;
            foreach (FRHICommandList cmdList in m_StackPool)
            {
                cmdList.Dispose();
            }
        }
    }
}
