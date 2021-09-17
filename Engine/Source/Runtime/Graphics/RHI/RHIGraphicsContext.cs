using Vortice.DXGI;
using Vortice.Direct3D12;
using System.Collections.Generic;
using InfinityEngine.Core.Object;

namespace InfinityEngine.Graphics.RHI
{
    public enum EContextType
    {
        Copy = 3,
        Compute = 2,
        Graphics = 0
    }

    public class FRHIGraphicsContext : FDisposable
    {
        public float copyFrequency
        {
            get
            {
                return copyCommands.d3dCmdQueue.TimestampFrequency;
            }
        }
        public float computeFrequency
        {
            get
            {
                return computeCommands.d3dCmdQueue.TimestampFrequency;
            }
        }
        public float graphicsFrequency
        {
            get
            {
                return graphicsCommands.d3dCmdQueue.TimestampFrequency;
            }
        }
      
        internal FRHIDevice device;
        internal FRHIFencePool fencePool;
        internal FRHIResourcePool resourcePool;
        internal List<FExecuteInfo> executeInfos;
        internal FRHICommandContext copyCommands;
        internal FRHICommandContext computeCommands;
        internal FRHICommandContext graphicsCommands;
        internal FRHICommandListPool copyCommandListPool;
        internal FRHICommandListPool computeCommandListPool;
        internal FRHICommandListPool graphicsCommandListPool;
        internal FRHIDescriptorHeapFactory cbvSrvUavDescriptorFactory;

        public FRHIGraphicsContext() : base()
        {
            device = new FRHIDevice();
            fencePool = new FRHIFencePool(device);
            executeInfos = new List<FExecuteInfo>(64);
            resourcePool = new FRHIResourcePool(device);
            copyCommands = new FRHICommandContext(device, EContextType.Copy);
            computeCommands = new FRHICommandContext(device, EContextType.Compute);
            graphicsCommands = new FRHICommandContext(device, EContextType.Graphics);
            copyCommandListPool = new FRHICommandListPool(device, EContextType.Copy);
            computeCommandListPool = new FRHICommandListPool(device, EContextType.Compute);
            graphicsCommandListPool = new FRHICommandListPool(device, EContextType.Graphics);
            cbvSrvUavDescriptorFactory = new FRHIDescriptorHeapFactory(device, DescriptorHeapType.ConstantBufferViewShaderResourceViewUnorderedAccessView, 32768);
        }

        // Context
        private FRHICommandContext SelectContext(in EContextType contextType)
        {
            FRHICommandContext commands = graphicsCommands;

            switch (contextType)
            {
                case EContextType.Copy:
                    commands = copyCommands;
                    break;

                case EContextType.Compute:
                    commands = computeCommands;
                    break;
            }

            return commands;
        }
        
        public FRHICommandList CreateCommandList(in EContextType contextType, string name = null)
        {
            FRHICommandList cmdList = new FRHICommandList(name, device, contextType);
            cmdList.Close();
            return cmdList;
        }

        public FRHICommandList GetCommandList(in EContextType contextType, string name = null, bool bAutoRelease = false)
        {
            FRHICommandList cmdList = null;
            switch (contextType)
            {
                case EContextType.Copy:
                    cmdList = copyCommandListPool.GetTemporary(name);
                    break;

                case EContextType.Compute:
                    cmdList = computeCommandListPool.GetTemporary(name);
                    break;

                case EContextType.Graphics:
                    cmdList = graphicsCommandListPool.GetTemporary(name);
                    break;
            }

            return cmdList;
        }

        public void ReleaseCommandList(FRHICommandList cmdList)
        {
            switch (cmdList.contextType)
            {
                case EContextType.Copy:
                    copyCommandListPool.ReleaseTemporary(cmdList);
                    break;

                case EContextType.Compute:
                    computeCommandListPool.ReleaseTemporary(cmdList);
                    break;

                case EContextType.Graphics:
                    graphicsCommandListPool.ReleaseTemporary(cmdList);
                    break;
            }
        }

        public void WriteFence(in EContextType contextType, FRHIFence fence)
        {
            FExecuteInfo executeInfo;
            executeInfo.fence = fence;
            executeInfo.cmdList = null;
            executeInfo.executeType = EExecuteType.Signal;
            executeInfo.cmdContext = SelectContext(contextType);
            executeInfos.Add(executeInfo);
        }

        public void WaitFence(in EContextType contextType, FRHIFence fence)
        {
            FExecuteInfo executeInfo;
            executeInfo.fence = fence;
            executeInfo.cmdList = null;
            executeInfo.executeType = EExecuteType.Wait;
            executeInfo.cmdContext = SelectContext(contextType);
            executeInfos.Add(executeInfo);
        }

        public void ExecuteCommandList(in EContextType contextType, FRHICommandList cmdList)
        {
            FExecuteInfo executeInfo;
            executeInfo.fence = null;
            executeInfo.cmdList = cmdList;
            executeInfo.executeType = EExecuteType.Execute;
            executeInfo.cmdContext = SelectContext(contextType);
            executeInfos.Add(executeInfo);
        }

        public void Flush()
        {
            graphicsCommands.Flush();
        }

        public void Submit()
        {
            for(int i = 0; i < executeInfos.Count; ++i)
            {
                FExecuteInfo executeInfo = executeInfos[i];
                FRHICommandContext cmdContext = executeInfo.cmdContext;

                switch (executeInfo.executeType)
                {
                    case EExecuteType.Signal:
                        cmdContext.SignalQueue(executeInfo.fence);
                        break;

                    case EExecuteType.Wait:
                        cmdContext.WaitQueue(executeInfo.fence);
                        break;

                    case EExecuteType.Execute:
                        cmdContext.ExecuteQueue(executeInfo.cmdList);
                        break;
                }
            }

            executeInfos.Clear();
        }

        // Resource
        public void CreateViewport()
        {

        }

        public FRHIFence CreateFence(string name = null)
        {
            return new FRHIFence(device, name);
        }
        
        public FRHIFence GetFence(string name = null)
        {
            return fencePool.GetTemporary(name);
        }

        public void ReleaseFence(FRHIFence fence)
        {
            fencePool.ReleaseTemporary(fence);
        }

        public FRHIQuery CreateQuery(in EQueryType queryType)
        {
            return new FRHIQuery(device, queryType, 2);
        }

        public FRHIComputePipelineState CreateComputePipelineState()
        {
            return new FRHIComputePipelineState();
        }

        public FRHIGraphicsPipelineState CreateGraphicsPipelineState()
        {
            return new FRHIGraphicsPipelineState();
        }

        public FRHIRayTracePipelineState CreateRayTracePipelineState()
        {
            return new FRHIRayTracePipelineState();
        }

        public void CreateSamplerState()
        {

        }

        public void CreateVertexInputLayout()
        {

        }

        public void CreateResourceInputLayout()
        {

        }

        public FRHIBuffer CreateBuffer(in FRHIBufferDescription description)
        {
            return new FRHIBuffer(device, description);
        }

        public FRHIBufferRef GetBuffer(in FRHIBufferDescription description)
        {
            return resourcePool.GetBuffer(description);
        }

        public void ReleaseBuffer(FRHIBufferRef bufferRef)
        {
            resourcePool.ReleaseBuffer(bufferRef);
        }

        public FRHITexture CreateTexture(in FRHITextureDescription description)
        {
            return new FRHITexture(device, description);
        }
        
        public FRHITextureRef GetTexture(in FRHITextureDescription description)
        {
            return resourcePool.GetTexture(description);
        }

        public void ReleaseTexture(FRHITextureRef textureRef)
        {
            resourcePool.ReleaseTexture(textureRef);
        }

        public FRHIIndexBufferView CreateIndexBufferView(FRHIBuffer buffer)
        {
            FRHIIndexBufferView ibv = new FRHIIndexBufferView();
            return ibv;
        }

        public FRHIVertexBufferView CreateVertexBufferView(FRHIBuffer buffer)
        {
            FRHIVertexBufferView vbo = new FRHIVertexBufferView();
            return vbo;
        }

        public FRHIDeptnStencilView CreateDepthStencilView(FRHITexture texture)
        {
            FRHIDeptnStencilView dsv = new FRHIDeptnStencilView();
            return dsv;
        }

        public FRHIRenderTargetView CreateRenderTargetView(FRHITexture texture)
        {
            FRHIRenderTargetView rtv = new FRHIRenderTargetView();
            return rtv;
        }

        public FRHIConstantBufferView CreateConstantBufferView(FRHIBuffer buffer)
        {
            FRHIConstantBufferView cbv = new FRHIConstantBufferView();
            return cbv;
        }

        public FRHIShaderResourceView CreateShaderResourceView(FRHIBuffer buffer)
        {
            ShaderResourceViewDescription srvDescriptor = new ShaderResourceViewDescription
            {
                Format = Format.Unknown,
                ViewDimension = ShaderResourceViewDimension.Buffer,
                Shader4ComponentMapping = 256,
                Buffer = new BufferShaderResourceView { FirstElement = 0, NumElements = (int)buffer.description.count, StructureByteStride = (int)buffer.description.stride }
            };
            int descriptorIndex = cbvSrvUavDescriptorFactory.Allocator(1);
            CpuDescriptorHandle descriptorHandle = cbvSrvUavDescriptorFactory.GetCPUHandleStart() + cbvSrvUavDescriptorFactory.GetDescriptorSize() * descriptorIndex;
            device.d3dDevice.CreateShaderResourceView(buffer.defaultResource, srvDescriptor, descriptorHandle);

            return new FRHIShaderResourceView(cbvSrvUavDescriptorFactory.GetDescriptorSize(), descriptorIndex, descriptorHandle);
        }

        public FRHIShaderResourceView CreateShaderResourceView(FRHITexture texture)
        {
            FRHIShaderResourceView srv = new FRHIShaderResourceView();
            return srv;
        }

        public FRHIUnorderedAccessView CreateUnorderedAccessView(FRHIBuffer buffer)
        {
            UnorderedAccessViewDescription uavDescriptor = new UnorderedAccessViewDescription
            {
                Format = Format.Unknown,
                ViewDimension = UnorderedAccessViewDimension.Buffer,
                Buffer = new BufferUnorderedAccessView { NumElements = (int)buffer.description.count, StructureByteStride = (int)buffer.description.stride }
            };
            int descriptorIndex = cbvSrvUavDescriptorFactory.Allocator(1);
            CpuDescriptorHandle descriptorHandle = cbvSrvUavDescriptorFactory.GetCPUHandleStart() + cbvSrvUavDescriptorFactory.GetDescriptorSize() * descriptorIndex;
            device.d3dDevice.CreateUnorderedAccessView(buffer.defaultResource, null, uavDescriptor, descriptorHandle);

            return new FRHIUnorderedAccessView(cbvSrvUavDescriptorFactory.GetDescriptorSize(), descriptorIndex, descriptorHandle);
        }

        public FRHIUnorderedAccessView CreateUnorderedAccessView(FRHITexture texture)
        {
            FRHIUnorderedAccessView uav = new FRHIUnorderedAccessView();
            return uav;
        }

        public FRHIResourceViewRange CreateResourceViewRange(in int count)
        {
            return new FRHIResourceViewRange(device, cbvSrvUavDescriptorFactory, count);
        }

        protected override void Release()
        {
            device?.Dispose();
            fencePool?.Dispose();
            resourcePool?.Dispose();
            copyCommands?.Dispose();
            computeCommands?.Dispose();
            graphicsCommands?.Dispose();
            copyCommandListPool?.Dispose();
            computeCommandListPool?.Dispose();
            graphicsCommandListPool?.Dispose();
            cbvSrvUavDescriptorFactory?.Dispose();
        }
    }
}
