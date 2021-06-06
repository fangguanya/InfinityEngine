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

    public class FRHIGraphicsContext : FDisposer
    {
        internal FRHIDevice device;
        internal List<FExecuteInfo> executeInfos;
        internal FRHICommandContext copyCmdContext;
        internal FRHICommandContext computeCmdContext;
        internal FRHICommandContext graphicsCmdContext;
        internal FRHIDescriptorHeapFactory cbvSrvUavDescriptorFactory;

        public FRHIGraphicsContext() : base()
        {
            device = new FRHIDevice();

            copyCmdContext = new FRHICommandContext(device, CommandListType.Copy);
            computeCmdContext = new FRHICommandContext(device, CommandListType.Compute);
            graphicsCmdContext = new FRHICommandContext(device, CommandListType.Direct);

            executeInfos = new List<FExecuteInfo>(64);
            cbvSrvUavDescriptorFactory = new FRHIDescriptorHeapFactory(device, DescriptorHeapType.ConstantBufferViewShaderResourceViewUnorderedAccessView, 32768);
        }

        private FRHICommandContext SelectContext(in EContextType contextType)
        {
            FRHICommandContext outContext = graphicsCmdContext;

            switch (contextType)
            {
                case EContextType.Copy:
                    outContext = copyCmdContext;
                    break;

                case EContextType.Compute:
                    outContext = computeCmdContext;
                    break;
            }

            return outContext;
        }

        public void ExecuteCmdList(in EContextType contextType, FRHICommandList cmdList)
        {
            FExecuteInfo executeInfo;
            executeInfo.fence = null;
            executeInfo.cmdList = cmdList;
            executeInfo.executeType = EExecuteType.Execute;
            executeInfo.cmdContext = SelectContext(contextType);
            executeInfos.Add(executeInfo);
        }

        public void WritFence(in EContextType contextType, FRHIFence fence)
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

        public void Submit()
        {
            for(int i = 0; i < executeInfos.Count; ++i)
            {
                FExecuteInfo executeInfo = executeInfos[i];
                switch (executeInfo.executeType)
                {
                    case EExecuteType.Signal:
                        executeInfo.cmdContext.SignalQueue(executeInfo.fence);
                        break;

                    case EExecuteType.Wait:
                        executeInfo.cmdContext.WaitQueue(executeInfo.fence);
                        break;

                    case EExecuteType.Execute:
                        executeInfo.cmdContext.ExecuteQueue(executeInfo.cmdList);
                        break;
                }
            }

            executeInfos.Clear();
            copyCmdContext.Flush();
            computeCmdContext.Flush();
            graphicsCmdContext.Flush();
        }

        public FRHICommandList CreateCmdList(string name, EContextType cmdListType)
        {
            FRHICommandList cmdList = new FRHICommandList(name, device, cmdListType);
            cmdList.Close();
            return cmdList;
        }

        public void CreateViewport()
        {

        }

        public FRHIFence CreateFence()
        {
            return new FRHIFence(device);
        }

        public FRHITimeQuery CreateTimeQuery()
        {
            return new FRHITimeQuery(device);
        }

        public FRHIOcclusionQuery CreateOcclusionQuery()
        {
            return new FRHIOcclusionQuery(device);
        }

        public FRHIStatisticsQuery CreateStatisticsQuery()
        {
            return new FRHIStatisticsQuery(device);
        }

        public void CreateInputVertexLayout()
        {

        }

        public void CreateInputResourceLayout()
        {

        }

        public FRHIComputePipelineState CreateComputePipelineState()
        {
            return new FRHIComputePipelineState();
        }

        public FRHIRayTracePipelineState CreateRayTracePipelineState()
        {
            return new FRHIRayTracePipelineState();
        }

        public FRHIGraphicsPipelineState CreateGraphicsPipelineState()
        {
            return new FRHIGraphicsPipelineState();
        }

        public void CreateSamplerState()
        {

        }

        public FRHIBuffer CreateBuffer(in ulong count, in ulong stride, in EUseFlag useFlag, in EBufferType bufferType)
        {
            FRHIBuffer buffer = new FRHIBuffer(device, useFlag, bufferType, count, stride);
            return buffer;
        }

        public FRHITexture CreateTexture(in EUseFlag useFlag, in ETextureType textureType)
        {
            FRHITexture texture = new FRHITexture(device, useFlag, textureType);
            return texture;
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
                Buffer = new BufferShaderResourceView { FirstElement = 0, NumElements = (int)buffer.count, StructureByteStride = (int)buffer.stride }
            };
            int descriptorIndex = cbvSrvUavDescriptorFactory.Allocator(1);
            CpuDescriptorHandle descriptorHandle = cbvSrvUavDescriptorFactory.GetCPUHandleStart() + cbvSrvUavDescriptorFactory.GetDescriptorSize() * descriptorIndex;
            device.d3D12Device.CreateShaderResourceView(buffer.defaultResource, srvDescriptor, descriptorHandle);

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
                Buffer = new BufferUnorderedAccessView { NumElements = (int)buffer.count, StructureByteStride = (int)buffer.stride }
            };
            int descriptorIndex = cbvSrvUavDescriptorFactory.Allocator(1);
            CpuDescriptorHandle descriptorHandle = cbvSrvUavDescriptorFactory.GetCPUHandleStart() + cbvSrvUavDescriptorFactory.GetDescriptorSize() * descriptorIndex;
            device.d3D12Device.CreateUnorderedAccessView(buffer.defaultResource, null, uavDescriptor, descriptorHandle);

            return new FRHIUnorderedAccessView(cbvSrvUavDescriptorFactory.GetDescriptorSize(), descriptorIndex, descriptorHandle);
        }

        public FRHIUnorderedAccessView CreateUnorderedAccessView(FRHITexture texture)
        {
            FRHIUnorderedAccessView uav = new FRHIUnorderedAccessView();
            return uav;
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

        public FRHIResourceViewRange CreateResourceViewRange(in int count)
        {
            return new FRHIResourceViewRange(device, cbvSrvUavDescriptorFactory, count);
        }

        protected override void Disposed()
        {
            device?.Dispose();
            copyCmdContext?.Dispose();
            computeCmdContext?.Dispose();
            graphicsCmdContext?.Dispose();
            cbvSrvUavDescriptorFactory?.Dispose();
        }
    }
}
