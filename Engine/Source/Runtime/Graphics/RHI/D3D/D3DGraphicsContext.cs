using Vortice.DXGI;
using Vortice.Direct3D12;
using InfinityEngine.Core.Container;

namespace InfinityEngine.Graphics.RHI.D3D
{
    public class FD3DGraphicsContext : FRHIGraphicsContext
    {
        public override ulong copyFrequency
        {
            get
            {
                return m_TransContext.nativeCmdQueue.TimestampFrequency;
            }
        }
        public override ulong computeFrequency
        {
            get
            {
                return m_ComputeContext.nativeCmdQueue.TimestampFrequency;
            }
        }
        public override ulong graphicsFrequency
        {
            get
            {
                return m_GraphicsContext.nativeCmdQueue.TimestampFrequency;
            }
        }
      
        private FD3DDevice m_Device;
        private FRHIFencePool m_FencePool;
        private FRHIResourcePool m_ResourcePool;
        private FD3DQueryContext[] m_QueryContext;
        private FD3DCommandContext m_TransContext;
        private FD3DCommandContext m_ComputeContext;
        private FD3DCommandContext m_GraphicsContext;
        private TArray<FExecuteInfo> m_ExecuteGPUInfos;
        private FRHICommandBufferPool m_CopyCmdBufferPool;
        private FRHICommandBufferPool m_ComputeCmdBufferPool;
        private FRHICommandBufferPool m_GraphicsCmdBufferPool;
        private TArray<FRHICommandBuffer> m_ManagedCmdBuffers;
        private FRHIDescriptorHeapFactory m_DescriptorFactory;

        public FD3DGraphicsContext() : base()
        {
            m_Device = new FD3DDevice();
            m_FencePool = new FRHIFencePool(this);
            m_ResourcePool = new FRHIResourcePool(this);
            m_ExecuteGPUInfos = new TArray<FExecuteInfo>(32);
            m_ManagedCmdBuffers = new TArray<FRHICommandBuffer>(32);

            m_QueryContext = new FD3DQueryContext[2];
            m_QueryContext[0] = new FD3DQueryContext(m_Device, EQueryType.Timestamp, 64);
            m_QueryContext[1] = new FD3DQueryContext(m_Device, EQueryType.CopyTimestamp, 64);

            m_TransContext = new FD3DCommandContext(m_Device, EContextType.Copy);
            m_ComputeContext = new FD3DCommandContext(m_Device, EContextType.Compute);
            m_GraphicsContext = new FD3DCommandContext(m_Device, EContextType.Graphics);

            m_CopyCmdBufferPool = new FRHICommandBufferPool(this, EContextType.Copy);
            m_ComputeCmdBufferPool = new FRHICommandBufferPool(this, EContextType.Compute);
            m_GraphicsCmdBufferPool = new FRHICommandBufferPool(this, EContextType.Graphics);

            //TerraFX.Interop.D3D12MemAlloc.D3D12MA_CreateAllocator
            m_DescriptorFactory = new FRHIDescriptorHeapFactory(m_Device, DescriptorHeapType.ConstantBufferViewShaderResourceViewUnorderedAccessView, 32768);
        }

        // Context
        internal override FRHICommandContext SelectContext(in EContextType contextType)
        {
            FRHICommandContext commandContext = m_GraphicsContext;

            switch (contextType)
            {
                case EContextType.Copy:
                    commandContext = m_TransContext;
                    break;

                case EContextType.Compute:
                    commandContext = m_ComputeContext;
                    break;
            }

            return (FD3DCommandContext)commandContext;
        }
        
        public override FRHICommandBuffer CreateCommandBuffer(in EContextType contextType, string name = null)
        {
            return new FD3DCommandBuffer(name, m_Device, contextType);
        }

        public override FRHICommandBuffer GetCommandBuffer(in EContextType contextType, string name = null, bool bAutoRelease = false)
        {
            FRHICommandBuffer cmdBuffer = null;
            switch (contextType)
            {
                case EContextType.Copy:
                    cmdBuffer = m_CopyCmdBufferPool.GetTemporary(name);
                    break;

                case EContextType.Compute:
                    cmdBuffer = m_ComputeCmdBufferPool.GetTemporary(name);
                    break;

                case EContextType.Graphics:
                    cmdBuffer = m_GraphicsCmdBufferPool.GetTemporary(name);
                    break;
            }

            if (bAutoRelease) { m_ManagedCmdBuffers.Add(cmdBuffer); }

            return cmdBuffer;
        }

        public override void ReleaseCommandBuffer(FRHICommandBuffer cmdBuffer)
        {
            switch (cmdBuffer.contextType)
            {
                case EContextType.Copy:
                    m_CopyCmdBufferPool.ReleaseTemporary(cmdBuffer);
                    break;

                case EContextType.Compute:
                    m_ComputeCmdBufferPool.ReleaseTemporary(cmdBuffer);
                    break;

                case EContextType.Graphics:
                    m_GraphicsCmdBufferPool.ReleaseTemporary(cmdBuffer);
                    break;
            }
        }

        public override void WriteFence(in EContextType contextType, FRHIFence fence)
        {
            FExecuteInfo executeInfo;
            executeInfo.fence = fence;
            executeInfo.cmdBuffer = null;
            executeInfo.executeType = EExecuteType.Signal;
            executeInfo.cmdContext = SelectContext(contextType);
            m_ExecuteGPUInfos.Add(executeInfo);
        }

        public override void WaitFence(in EContextType contextType, FRHIFence fence)
        {
            FExecuteInfo executeInfo;
            executeInfo.fence = fence;
            executeInfo.cmdBuffer = null;
            executeInfo.executeType = EExecuteType.Wait;
            executeInfo.cmdContext = SelectContext(contextType);
            m_ExecuteGPUInfos.Add(executeInfo);
        }

        public override void ExecuteCommandBuffer(FRHICommandBuffer cmdBuffer)
        {
            FExecuteInfo executeInfo;
            executeInfo.fence = null;
            executeInfo.cmdBuffer = cmdBuffer;
            executeInfo.executeType = EExecuteType.Execute;
            executeInfo.cmdContext = SelectContext(cmdBuffer.contextType);
            m_ExecuteGPUInfos.Add(executeInfo);
        }

        public override void Flush()
        {
            for (int i = 0; i < m_ManagedCmdBuffers.length; ++i)
            {
                ReleaseCommandBuffer(m_ManagedCmdBuffers[i]);
                m_ManagedCmdBuffers[i] = null;
            }
            m_ManagedCmdBuffers.Clear();

            m_QueryContext[1].Submit(m_TransContext);
            m_QueryContext[0].Submit(m_GraphicsContext);

            m_GraphicsContext.Flush();

            m_QueryContext[0].GetData();
            m_QueryContext[1].GetData();
        }

        public override void Submit()
        {
            for (int i = 0; i < m_ExecuteGPUInfos.length; ++i)
            {
                FExecuteInfo executeInfo = m_ExecuteGPUInfos[i];
                FD3DCommandContext cmdContext = (FD3DCommandContext)executeInfo.cmdContext;

                switch (executeInfo.executeType)
                {
                    case EExecuteType.Signal:
                        cmdContext.SignalQueue(executeInfo.fence);
                        break;

                    case EExecuteType.Wait:
                        cmdContext.WaitQueue(executeInfo.fence);
                        break;

                    case EExecuteType.Execute:
                        cmdContext.ExecuteQueue(executeInfo.cmdBuffer);
                        break;
                }
            }

            m_ExecuteGPUInfos.Clear();
        }

        // Resource
        public override void CreateViewport()
        {

        }

        public override FRHIFence CreateFence(string name = null)
        {
            return new FD3DFence(m_Device, name);
        }
        
        public override FRHIFence GetFence(string name = null)
        {
            return m_FencePool.GetTemporary(name);
        }

        public override void ReleaseFence(FRHIFence fence)
        {
            m_FencePool.ReleaseTemporary((FD3DFence)fence);
        }

        public override FRHIQuery CreateQuery(in EQueryType queryType, string name = null)
        {
            FD3DQuery outQuery = null;
            switch (queryType)
            {
                case EQueryType.Occlusion:
                    outQuery = null;
                    break;
                case EQueryType.Timestamp:
                    outQuery = new FD3DQuery(m_QueryContext[0]);
                    break;
                case EQueryType.Statistics:
                    outQuery = null;
                    break;
                case EQueryType.CopyTimestamp:
                    outQuery = new FD3DQuery(m_QueryContext[1]);
                    break;
            }
            return outQuery;
        }

        public override FRHIQuery GetQuery(in EQueryType queryType, string name = null)
        {
            FRHIQuery outQuery = null;
            switch (queryType)
            {
                case EQueryType.Occlusion:
                    outQuery = null;
                    break;
                case EQueryType.Timestamp:
                    outQuery = m_QueryContext[0].GetTemporary(name);
                    break;
                case EQueryType.Statistics:
                    outQuery = null;
                    break;
                case EQueryType.CopyTimestamp:
                    outQuery = m_QueryContext[1].GetTemporary(name);
                    break;
            }
            return outQuery;
        }

        public override void ReleaseQuery(FRHIQuery query)
        {
            FD3DQuery d3dQuery = (FD3DQuery)query;

            switch (d3dQuery.context.queryType)
            {
                case EQueryType.Occlusion:
                    break;
                case EQueryType.Timestamp:
                    m_QueryContext[0].ReleaseTemporary(query);
                    break;
                case EQueryType.Statistics:
                    break;
                case EQueryType.CopyTimestamp:
                    m_QueryContext[1].ReleaseTemporary(query);
                    break;
            }
        }

        public override FRHIComputePipelineState CreateComputePipelineState(in FRHIComputePipelineDescription description)
        {
            return new FRHIComputePipelineState();
        }

        public override FRHIRayTracePipelineState CreateRayTracePipelineState(in FRHIRayTracePipelineDescription description)
        {
            return new FRHIRayTracePipelineState();
        }

        public override FRHIGraphicsPipelineState CreateGraphicsPipelineState(in FRHIGraphicsPipelineDescription description)
        {
            return new FRHIGraphicsPipelineState();
        }

        public override void CreateSamplerState()
        {

        }

        public override void CreateVertexInputLayout()
        {

        }

        public override void CreateResourceInputLayout()
        {

        }

        public override FRHIBuffer CreateBuffer(in FRHIBufferDescription description)
        {
            return new FD3DBuffer(m_Device, description);
        }

        public override FRHIBufferRef GetBuffer(in FRHIBufferDescription description)
        {
            return m_ResourcePool.GetBuffer(description);
        }

        public override void ReleaseBuffer(FRHIBufferRef bufferRef)
        {
            m_ResourcePool.ReleaseBuffer(bufferRef);
        }

        public override FRHITexture CreateTexture(in FRHITextureDescription description)
        {
            return new FD3DTexture(m_Device, description);
        }
        
        public override FRHITextureRef GetTexture(in FRHITextureDescription description)
        {
            return m_ResourcePool.GetTexture(description);
        }

        public override void ReleaseTexture(FRHITextureRef textureRef)
        {
            m_ResourcePool.ReleaseTexture(textureRef);
        }

        public override FRHIIndexBufferView CreateIndexBufferView(FRHIBuffer buffer)
        {
            FRHIIndexBufferView ibv = new FRHIIndexBufferView();
            return ibv;
        }

        public override FRHIVertexBufferView CreateVertexBufferView(FRHIBuffer buffer)
        {
            FRHIVertexBufferView vbo = new FRHIVertexBufferView();
            return vbo;
        }

        public override FRHIDeptnStencilView CreateDepthStencilView(FRHITexture texture)
        {
            FRHIDeptnStencilView dsv = new FRHIDeptnStencilView();
            return dsv;
        }

        public override FRHIRenderTargetView CreateRenderTargetView(FRHITexture texture)
        {
            FRHIRenderTargetView rtv = new FRHIRenderTargetView();
            return rtv;
        }

        public override FRHIConstantBufferView CreateConstantBufferView(FRHIBuffer buffer)
        {
            FRHIConstantBufferView cbv = new FRHIConstantBufferView();
            return cbv;
        }

        public override FRHIShaderResourceView CreateShaderResourceView(FRHIBuffer buffer)
        {
            FD3DBuffer d3dBuffer = (FD3DBuffer)buffer;
            ShaderResourceViewDescription srvDescriptor = new ShaderResourceViewDescription
            {
                Format = Format.Unknown,
                ViewDimension = ShaderResourceViewDimension.Buffer,
                Shader4ComponentMapping = 256,
                Buffer = new BufferShaderResourceView { FirstElement = 0, NumElements = (int)d3dBuffer.description.count, StructureByteStride = (int)d3dBuffer.description.stride }
            };
            int descriptorIndex = m_DescriptorFactory.Allocator(1);
            CpuDescriptorHandle descriptorHandle = m_DescriptorFactory.GetCPUHandleStart() + m_DescriptorFactory.GetDescriptorSize() * descriptorIndex;
            m_Device.nativeDevice.CreateShaderResourceView(d3dBuffer.defaultResource, srvDescriptor, descriptorHandle);

            return new FRHIShaderResourceView(m_DescriptorFactory.GetDescriptorSize(), descriptorIndex, descriptorHandle);
        }

        public override FRHIShaderResourceView CreateShaderResourceView(FRHITexture texture)
        {
            FRHIShaderResourceView srv = new FRHIShaderResourceView();
            return srv;
        }

        public override FRHIUnorderedAccessView CreateUnorderedAccessView(FRHIBuffer buffer)
        {
            FD3DBuffer d3dBuffer = (FD3DBuffer)buffer;
            UnorderedAccessViewDescription uavDescriptor = new UnorderedAccessViewDescription
            {
                Format = Format.Unknown,
                ViewDimension = UnorderedAccessViewDimension.Buffer,
                Buffer = new BufferUnorderedAccessView { NumElements = (int)d3dBuffer.description.count, StructureByteStride = (int)d3dBuffer.description.stride }
            };
            int descriptorIndex = m_DescriptorFactory.Allocator(1);
            CpuDescriptorHandle descriptorHandle = m_DescriptorFactory.GetCPUHandleStart() + m_DescriptorFactory.GetDescriptorSize() * descriptorIndex;
            m_Device.nativeDevice.CreateUnorderedAccessView(d3dBuffer.defaultResource, null, uavDescriptor, descriptorHandle);

            return new FRHIUnorderedAccessView(m_DescriptorFactory.GetDescriptorSize(), descriptorIndex, descriptorHandle);
        }

        public override FRHIUnorderedAccessView CreateUnorderedAccessView(FRHITexture texture)
        {
            FRHIUnorderedAccessView uav = new FRHIUnorderedAccessView();
            return uav;
        }

        public override FRHIResourceSet CreateResourceSet(in int count)
        {
            return new FRHIResourceSet(m_Device, m_DescriptorFactory, count);
        }

        protected override void Release()
        {
            m_Device?.Dispose();
            m_FencePool?.Dispose();
            m_ResourcePool?.Dispose();
            m_TransContext?.Dispose();
            m_ComputeContext?.Dispose();
            m_GraphicsContext?.Dispose();
            m_QueryContext[0]?.Dispose();
            m_QueryContext[1]?.Dispose();
            m_DescriptorFactory?.Dispose();
            m_CopyCmdBufferPool?.Dispose();
            m_ComputeCmdBufferPool?.Dispose();
            m_GraphicsCmdBufferPool?.Dispose();
        }
    }
}
