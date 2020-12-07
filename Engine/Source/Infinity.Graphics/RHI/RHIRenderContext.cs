using Vortice.DXGI;
using Vortice.Direct3D12;
using InfinityEngine.Core.Object;
using InfinityEngine.Core.Container;

namespace InfinityEngine.Graphics.RHI
{
    public class RHIRenderContext : UObject
    {
        internal RHIDevice Device;
        internal RHICopyCmdContext CopyContext;
        internal RHIComputeCmdContext ComputeContext;
        internal RHIGraphicsCmdContext GraphicsContext;
        internal TArray<RHICommandBuffer> CmdBufferArray;
        internal RHIDescriptorHeapFactory CbvSrvUavDescriptorFactory;

        public RHIRenderContext() : base()
        {
            Device = new RHIDevice();

            CmdBufferArray = new TArray<RHICommandBuffer>();

            CopyContext = new RHICopyCmdContext(Device.NativeDevice, CommandListType.Copy);
            ComputeContext = new RHIComputeCmdContext(Device.NativeDevice, CommandListType.Compute);
            GraphicsContext = new RHIGraphicsCmdContext(Device.NativeDevice, CommandListType.Direct);

            CbvSrvUavDescriptorFactory = new RHIDescriptorHeapFactory(Device.NativeDevice, DescriptorHeapType.ConstantBufferViewShaderResourceViewUnorderedAccessView, 32768);
        }

        public void ExecuteCmdBuffer(RHICommandBuffer CmdBuffer)
        {
            RHICommandBuffer CopyCmdBuffer = CmdBuffer.Clone();
            CopyCmdBuffer.bASyncCompute = false;
            CmdBufferArray.Add(CopyCmdBuffer);
            CmdBuffer.Clear();
        }

        public void ExecuteCmdBufferASync(RHICommandBuffer CmdBuffer)
        {
            RHICommandBuffer CopyCmdBuffer = CmdBuffer.Clone();
            CopyCmdBuffer.bASyncCompute = true;
            CmdBufferArray.Add(CopyCmdBuffer);
        }

        internal void TranslateToNativeCommand(RHICommandBuffer CmdBuffer, RHICommandContext CmdContext)
        {
            RHICopyCmdContext CopyCmdContex = (RHICopyCmdContext)CmdContext;
            RHIComputeCmdContext ComputeCmdContex = (RHIComputeCmdContext)CmdContext;
            RHIGraphicsCmdContext GraphicsCmdContex = (RHIGraphicsCmdContext)CmdContext;

            for (int CoordIndex = 0; CoordIndex < CmdBuffer.Size(); CoordIndex++)
            {
                IRenderCommand RenderCmd = CmdBuffer.CmdList[CoordIndex];

                switch (RenderCmd.GetRenderCmdType)
                {
                    case ERenderCommandType.GenerateMipmap:
                        //RenderCommandGenerateMipmap RenderCmdGenerateMipmap = (RenderCommandGenerateMipmap)RenderCmd;
                        //CmdContext.GenerateMipmaps(null);
                        break;

                    case ERenderCommandType.ResourceBarrier:
                        //RenderCommandResourceBarrier RenderCmdResourceBarrier = (RenderCommandResourceBarrier)RenderCmd;
                        CopyCmdContex.ResourceBarrier();
                        break;

                    case ERenderCommandType.DispatchCompute:
                        RenderCommandDispatchCompute RenderCmdDispatchCompute = (RenderCommandDispatchCompute)RenderCmd;
                        ComputeCmdContex.DispatchCompute(RenderCmdDispatchCompute.shader, RenderCmdDispatchCompute.x, RenderCmdDispatchCompute.y, RenderCmdDispatchCompute.z);
                        break;

                    case ERenderCommandType.DrawPrimitiveInstance:
                        RenderCommandDrawPrimitiveInstance RenderCmdDrawInstance = (RenderCommandDrawPrimitiveInstance)RenderCmd;
                        GraphicsCmdContex.DrawPrimitiveInstance(RenderCmdDrawInstance.IndexBuffer, RenderCmdDrawInstance.VertexBuffer, RenderCmdDrawInstance.TopologyType, RenderCmdDrawInstance.IndexCount, RenderCmdDrawInstance.InstanceCount);
                        break;

                    default:
                        //
                        break;
                }
            }
        }

        public void Submit()
        {
            // Reset CmdList
            //CmdContext.Reset();

            for (int i = 0; i != CmdBufferArray.size; i++)
            {
                RHICommandBuffer CmdBuffer = CmdBufferArray[i];
                RHICommandContext CmdContext = CmdBuffer.bASyncCompute ? ComputeContext : GraphicsContext;

                TranslateToNativeCommand(CmdBuffer, CmdContext);
            }

            // Execute CmdList
            CmdBufferArray.Clear();
            //CmdContext.Execute();
            //CmdContext.Flush();
        }

        public RHIComputeCmdContext GetComputeContext()
        {
            return ComputeContext;
        }

        public RHIGraphicsCmdContext GetGraphicsContext()
        {
            return GraphicsContext;
        }

        public RHIFence CreateGraphicsFence()
        {
            return new RHIFence(Device.NativeDevice, GraphicsContext, ComputeContext);
        }

        public RHIFence CreateComputeFence()
        {
            return new RHIFence(Device.NativeDevice, ComputeContext, GraphicsContext);
        }

        public void CreateViewport()
        {

        }

        public RHITimeQuery CreateTimeQuery(bool bComputeQueue)
        {
            ID3D12GraphicsCommandList6 NativeCmdList = (!bComputeQueue) ? GraphicsContext.NativeCmdList : ComputeContext.NativeCmdList;
            return new RHITimeQuery(Device.NativeDevice, NativeCmdList);
        }

        public RHIOcclusionQuery CreateOcclusionQuery(bool bComputeQueue)
        {
            ID3D12GraphicsCommandList6 NativeCmdList = (!bComputeQueue) ? GraphicsContext.NativeCmdList : ComputeContext.NativeCmdList;
            return new RHIOcclusionQuery(Device.NativeDevice, NativeCmdList);
        }

        public RHIStatisticsQuery CreateStatisticsQuery(bool bComputeQueue)
        {
            ID3D12GraphicsCommandList6 NativeCmdList = (!bComputeQueue) ? GraphicsContext.NativeCmdList : ComputeContext.NativeCmdList;
            return new RHIStatisticsQuery(Device.NativeDevice, NativeCmdList);
        }

        public void CreateInputVertexLayout()
        {

        }

        public void CreateInputResourceLayout()
        {

        }

        public RHIComputePipelineState CreateComputePipelineState()
        {
            return new RHIComputePipelineState();
        }

        public RHIRayTracePipelineState CreateRayTracePipelineState()
        {
            return new RHIRayTracePipelineState();
        }

        public RHIGraphicsPipelineState CreateGraphicsPipelineState()
        {
            return new RHIGraphicsPipelineState();
        }

        public void CreateSamplerState()
        {

        }

        public RHIBuffer CreateBuffer(ulong InCount, ulong InStride, EUseFlag InUseFlag, EBufferType InBufferType)
        {
            RHIBuffer GPUBuffer = new RHIBuffer(Device.NativeDevice, CopyContext.NativeCmdList, InUseFlag, InBufferType, InCount, InStride);
            return GPUBuffer;
        }

        public RHITexture CreateTexture(EUseFlag InUseFlag, ETextureType InTextureType)
        {
            RHITexture Texture = new RHITexture(Device.NativeDevice, CopyContext.NativeCmdList, InUseFlag, InTextureType);
            return Texture;
        }

        public RHIDeptnStencilView CreateDepthStencilView(RHITexture Texture)
        {
            RHIDeptnStencilView DSV = new RHIDeptnStencilView();
            return DSV;
        }

        public RHIRenderTargetView CreateRenderTargetView(RHITexture Texture)
        {
            RHIRenderTargetView RTV = new RHIRenderTargetView();
            return RTV;
        }

        public RHIIndexBufferView CreateIndexBufferView(RHIBuffer IndexBuffer)
        {
            RHIIndexBufferView IBO = new RHIIndexBufferView();
            return IBO;
        }

        public RHIVertexBufferView CreateVertexBufferView(RHIBuffer VertexBuffer)
        {
            RHIVertexBufferView VBO = new RHIVertexBufferView();
            return VBO;
        }

        public RHIConstantBufferView CreateConstantBufferView(RHIBuffer ConstantBuffer)
        {
            RHIConstantBufferView CBV = new RHIConstantBufferView();
            return CBV;
        }

        public RHIShaderResourceView CreateShaderResourceView(RHIBuffer Buffer)
        {
            ShaderResourceViewDescription SRVDescriptor = new ShaderResourceViewDescription
            {
                Format = Format.Unknown,
                ViewDimension = ShaderResourceViewDimension.Buffer,
                Shader4ComponentMapping = 256,
                Buffer = new BufferShaderResourceView { FirstElement = 0, NumElements = (int)Buffer.Count, StructureByteStride = (int)Buffer.Stride }
            };
            int DescriptorIndex = CbvSrvUavDescriptorFactory.Allocator(1);
            CpuDescriptorHandle DescriptorHandle = CbvSrvUavDescriptorFactory.GetCPUHandleStart() + CbvSrvUavDescriptorFactory.GetDescriptorSize() * DescriptorIndex;
            Device.NativeDevice.CreateShaderResourceView(Buffer.DefaultResource, SRVDescriptor, DescriptorHandle);

            return new RHIShaderResourceView(CbvSrvUavDescriptorFactory.GetDescriptorSize(), DescriptorIndex, DescriptorHandle);
        }

        public RHIShaderResourceView CreateShaderResourceView(RHITexture Texture)
        {
            RHIShaderResourceView SRV = new RHIShaderResourceView();
            return SRV;
        }

        public RHIUnorderedAccessView CreateUnorderedAccessView(RHIBuffer Buffer)
        {
            UnorderedAccessViewDescription UAVDescriptor = new UnorderedAccessViewDescription
            {
                Format = Format.Unknown,
                ViewDimension = UnorderedAccessViewDimension.Buffer,
                Buffer = new BufferUnorderedAccessView { NumElements = (int)Buffer.Count, StructureByteStride = (int)Buffer.Stride }
            };
            int DescriptorIndex = CbvSrvUavDescriptorFactory.Allocator(1);
            CpuDescriptorHandle DescriptorHandle = CbvSrvUavDescriptorFactory.GetCPUHandleStart() + CbvSrvUavDescriptorFactory.GetDescriptorSize() * DescriptorIndex;
            Device.NativeDevice.CreateUnorderedAccessView(Buffer.DefaultResource, null, UAVDescriptor, DescriptorHandle);

            return new RHIUnorderedAccessView(CbvSrvUavDescriptorFactory.GetDescriptorSize(), DescriptorIndex, DescriptorHandle);
        }

        public RHIUnorderedAccessView CreateUnorderedAccessView(RHITexture Texture)
        {
            RHIUnorderedAccessView UAV = new RHIUnorderedAccessView();
            return UAV;
        }

        public RHIResourceViewRange CreateRHIResourceViewRange(int Count)
        {
            return new RHIResourceViewRange(Device.NativeDevice, CbvSrvUavDescriptorFactory, Count);
        }

        protected override void DisposeManaged()
        {

        }

        protected override void DisposeUnManaged()
        {
            Device.Dispose();
            CopyContext.Dispose();
            ComputeContext.Dispose();
            GraphicsContext.Dispose();
            CbvSrvUavDescriptorFactory.Dispose();
        }
    }
}
