using Vortice.DXGI;
using Vortice.Direct3D12;
using System.Collections.Generic;
using InfinityEngine.Core.Object;
using InfinityEngine.Core.Container;

namespace InfinityEngine.Graphics.RHI
{
    public enum EContextType
    {
        Copy = 0,
        Compute = 1,
        Graphics = 2
    }

    public class FRHIGraphicsContext : UObject
    {
        internal FRHIDevice rhiDevice;
        internal FRHICommandContext rhiCopyContext;
        internal FRHICommandContext rhiComputeContext;
        internal FRHICommandContext rhiGraphicsContext;
        internal List<FExecuteInfo> rhiCmdListExecuteInfos;
        internal FRHIDescriptorHeapFactory rhiCbvSrvUavDescriptorFactory;

        public FRHIGraphicsContext() : base()
        {
            rhiDevice = new FRHIDevice();
            rhiCopyContext = new FRHICommandContext(rhiDevice, CommandListType.Copy);
            rhiComputeContext = new FRHICommandContext(rhiDevice, CommandListType.Compute);
            rhiGraphicsContext = new FRHICommandContext(rhiDevice, CommandListType.Direct);
            rhiCmdListExecuteInfos = new List<FExecuteInfo>(64);
            rhiCbvSrvUavDescriptorFactory = new FRHIDescriptorHeapFactory(rhiDevice, DescriptorHeapType.ConstantBufferViewShaderResourceViewUnorderedAccessView, 32768);
        }

        private FRHICommandContext SelectContext(in EContextType contextType)
        {
            FRHICommandContext outContext = rhiGraphicsContext;

            switch (contextType)
            {
                case EContextType.Copy:
                    outContext = rhiCopyContext;
                    break;

                case EContextType.Compute:
                    outContext = rhiComputeContext;
                    break;
            }

            return outContext;
        }

        public void ExecuteCmdList(in EContextType contextType, FRHICommandList rhiCmdList)
        {
            FExecuteInfo executeInfo;
            executeInfo.rhiFence = null;
            executeInfo.rhiCmdList = rhiCmdList;
            executeInfo.executeType = EExecuteType.Execute;
            executeInfo.rhiCmdContext = SelectContext(contextType);
            rhiCmdListExecuteInfos.Add(executeInfo);
        }

        public void WritFence(in EContextType contextType, FRHIFence rhiFence)
        {
            FExecuteInfo executeInfo;
            executeInfo.rhiFence = rhiFence;
            executeInfo.rhiCmdList = null;
            executeInfo.executeType = EExecuteType.Signal;
            executeInfo.rhiCmdContext = SelectContext(contextType);
            rhiCmdListExecuteInfos.Add(executeInfo);
        }

        public void WaitFence(in EContextType contextType, FRHIFence rhiFence)
        {
            FExecuteInfo executeInfo;
            executeInfo.rhiFence = rhiFence;
            executeInfo.rhiCmdList = null;
            executeInfo.executeType = EExecuteType.Wait;
            executeInfo.rhiCmdContext = SelectContext(contextType);
            rhiCmdListExecuteInfos.Add(executeInfo);
        }

        public void Submit()
        {
            for(int i = 0; i < rhiCmdListExecuteInfos.Count; ++i)
            {
                FExecuteInfo executeInfo = rhiCmdListExecuteInfos[i];
                switch (executeInfo.executeType)
                {
                    case EExecuteType.Signal:
                        executeInfo.rhiCmdContext.SignalQueue(executeInfo.rhiFence);
                        break;

                    case EExecuteType.Wait:
                        executeInfo.rhiCmdContext.WaitQueue(executeInfo.rhiFence);
                        break;

                    case EExecuteType.Execute:
                        executeInfo.rhiCmdContext.ExecuteQueue(executeInfo.rhiCmdList);
                        break;
                }
            }

            rhiCmdListExecuteInfos.Clear();

            rhiCopyContext.Flush();
            rhiComputeContext.Flush();
            rhiGraphicsContext.Flush();
        }

        public FRHICommandList CreateCmdList(string name, CommandListType cmdListType)
        {
            FRHICommandList rhiCmdList = new FRHICommandList(name, rhiDevice, cmdListType);
            rhiCmdList.Close();
            return rhiCmdList;
        }

        public void CreateViewport()
        {

        }

        public FRHIFence CreateFence()
        {
            return new FRHIFence(rhiDevice);
        }

        public FRHITimeQuery CreateTimeQuery()
        {
            return new FRHITimeQuery(rhiDevice);
        }

        public FRHIOcclusionQuery CreateOcclusionQuery()
        {
            return new FRHIOcclusionQuery(rhiDevice);
        }

        public FRHIStatisticsQuery CreateStatisticsQuery()
        {
            return new FRHIStatisticsQuery(rhiDevice);
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
            FRHIBuffer buffer = new FRHIBuffer(rhiDevice, useFlag, bufferType, count, stride);
            return buffer;
        }

        public FRHITexture CreateTexture(in EUseFlag useFlag, in ETextureType textureType)
        {
            FRHITexture texture = new FRHITexture(rhiDevice, useFlag, textureType);
            return texture;
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

        public FRHIIndexBufferView CreateIndexBufferView(FRHIBuffer indexBuffer)
        {
            FRHIIndexBufferView ibv = new FRHIIndexBufferView();
            return ibv;
        }

        public FRHIVertexBufferView CreateVertexBufferView(FRHIBuffer vertexBuffer)
        {
            FRHIVertexBufferView vbo = new FRHIVertexBufferView();
            return vbo;
        }

        public FRHIConstantBufferView CreateConstantBufferView(FRHIBuffer constantBuffer)
        {
            FRHIConstantBufferView cbv = new FRHIConstantBufferView();
            return cbv;
        }

        public FRHIShaderResourceView CreateShaderResourceView(FRHIBuffer buffer)
        {
            ShaderResourceViewDescription SRVDescriptor = new ShaderResourceViewDescription
            {
                Format = Format.Unknown,
                ViewDimension = ShaderResourceViewDimension.Buffer,
                Shader4ComponentMapping = 256,
                Buffer = new BufferShaderResourceView { FirstElement = 0, NumElements = (int)buffer.count, StructureByteStride = (int)buffer.stride }
            };
            int DescriptorIndex = rhiCbvSrvUavDescriptorFactory.Allocator(1);
            CpuDescriptorHandle DescriptorHandle = rhiCbvSrvUavDescriptorFactory.GetCPUHandleStart() + rhiCbvSrvUavDescriptorFactory.GetDescriptorSize() * DescriptorIndex;
            rhiDevice.d3D12Device.CreateShaderResourceView(buffer.defaultResource, SRVDescriptor, DescriptorHandle);

            return new FRHIShaderResourceView(rhiCbvSrvUavDescriptorFactory.GetDescriptorSize(), DescriptorIndex, DescriptorHandle);
        }

        public FRHIShaderResourceView CreateShaderResourceView(FRHITexture texture)
        {
            FRHIShaderResourceView srv = new FRHIShaderResourceView();
            return srv;
        }

        public FRHIUnorderedAccessView CreateUnorderedAccessView(FRHIBuffer buffer)
        {
            UnorderedAccessViewDescription UAVDescriptor = new UnorderedAccessViewDescription
            {
                Format = Format.Unknown,
                ViewDimension = UnorderedAccessViewDimension.Buffer,
                Buffer = new BufferUnorderedAccessView { NumElements = (int)buffer.count, StructureByteStride = (int)buffer.stride }
            };
            int DescriptorIndex = rhiCbvSrvUavDescriptorFactory.Allocator(1);
            CpuDescriptorHandle DescriptorHandle = rhiCbvSrvUavDescriptorFactory.GetCPUHandleStart() + rhiCbvSrvUavDescriptorFactory.GetDescriptorSize() * DescriptorIndex;
            rhiDevice.d3D12Device.CreateUnorderedAccessView(buffer.defaultResource, null, UAVDescriptor, DescriptorHandle);

            return new FRHIUnorderedAccessView(rhiCbvSrvUavDescriptorFactory.GetDescriptorSize(), DescriptorIndex, DescriptorHandle);
        }

        public FRHIUnorderedAccessView CreateUnorderedAccessView(FRHITexture texture)
        {
            FRHIUnorderedAccessView uav = new FRHIUnorderedAccessView();
            return uav;
        }

        public FRHIResourceViewRange CreateResourceViewRange(in int count)
        {
            return new FRHIResourceViewRange(rhiDevice, rhiCbvSrvUavDescriptorFactory, count);
        }

        protected override void Disposed()
        {
            rhiDevice?.Dispose();
            rhiCopyContext?.Dispose();
            rhiComputeContext?.Dispose();
            rhiGraphicsContext?.Dispose();
            rhiCbvSrvUavDescriptorFactory?.Dispose();
        }
    }
}
