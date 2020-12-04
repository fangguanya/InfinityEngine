using Vortice.DXGI;
using Vortice.Direct3D12;
using Infinity.Runtime.Graphics.Core;

namespace Infinity.Runtime.Graphics.RHI
{
    public class RHIRenderContext : TObject
    {
        internal RHIDevice GPUDevice;
        internal RHIComputeCmdContext CopyContext;
        internal RHIComputeCmdContext ComputeContext;
        internal RHIGraphicsCmdContext GraphicsContext;
        internal DynamicArray<RHICommandBuffer> CmdBufferArray;
        internal RHIDescriptorHeapFactory CbvSrvUavDescriptorFactory;

        public RHIRenderContext() : base()
        {
            GPUDevice = new RHIDevice();

            CopyContext = new RHIComputeCmdContext(GPUDevice.NativeDevice, CommandListType.Copy);
            ComputeContext = new RHIComputeCmdContext(GPUDevice.NativeDevice, CommandListType.Compute);
            GraphicsContext = new RHIGraphicsCmdContext(GPUDevice.NativeDevice, CommandListType.Direct);

            CbvSrvUavDescriptorFactory = new RHIDescriptorHeapFactory(GPUDevice.NativeDevice, DescriptorHeapType.ConstantBufferViewShaderResourceViewUnorderedAccessView, 32768);

            CmdBufferArray = new DynamicArray<RHICommandBuffer>();
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

        private void ExecuteNativeCommand(RHICommandBuffer InCmdBuffer)
        {
            for (int CoordIndex = 0; CoordIndex < InCmdBuffer.Size(); CoordIndex++)
            {
                IRenderCommand RenderCmd = InCmdBuffer.CmdList[CoordIndex];

                switch (RenderCmd.GetRenderCmdType)
                {
                    case ERenderCommandType.GenerateMipmap:
                        RenderCommandGenerateMipmap RenderCmdGenerateMipmap = (RenderCommandGenerateMipmap)RenderCmd;
                        //CmdContext.XXXX;
                        break;

                    case ERenderCommandType.ResourceBarrier:
                        RenderCommandResourceBarrier RenderCmdResourceBarrier = (RenderCommandResourceBarrier)RenderCmd;
                        //CmdContext.XXXX;
                        break;

                    default:
                        //
                        break;
                }
            }
        }

        public void Submit()
        {
            for (int i = 0; i != CmdBufferArray.size; i++)
            {
                RHICommandBuffer CmdBuffer = CmdBufferArray[i];
                //RHICommandContext CmdContext = CmdBuffer.bASyncCompute ? ComputeCmdContext : GraphicsCmdContext
                //CmdContext.Reset();

                //Translating CmdBuffer to CmdList and recording it
                ExecuteNativeCommand(CmdBuffer);
                //CmdContext.Execute();
            }

            // Wait GPU to execute CmdList
            //CmdContext.Flush();

            // Clear CommandBuffers
            CmdBufferArray.Clear();
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
            return new RHIFence(GPUDevice.NativeDevice, GraphicsContext, ComputeContext);
        }

        public RHIFence CreateComputeFence()
        {
            return new RHIFence(GPUDevice.NativeDevice, ComputeContext, GraphicsContext);
        }

        public void CreateViewport()
        {

        }

        public RHITimeQuery CreateTimeQuery(bool bComputeQueue)
        {
            ID3D12GraphicsCommandList6 NativeCmdList = (!bComputeQueue) ? GraphicsContext.NativeCmdList : ComputeContext.NativeCmdList;
            return new RHITimeQuery(GPUDevice.NativeDevice, NativeCmdList);
        }

        public RHIOcclusionQuery CreateOcclusionQuery(bool bComputeQueue)
        {
            ID3D12GraphicsCommandList6 NativeCmdList = (!bComputeQueue) ? GraphicsContext.NativeCmdList : ComputeContext.NativeCmdList;
            return new RHIOcclusionQuery(GPUDevice.NativeDevice, NativeCmdList);
        }

        public RHIStatisticsQuery CreateStatisticsQuery(bool bComputeQueue)
        {
            ID3D12GraphicsCommandList6 NativeCmdList = (!bComputeQueue) ? GraphicsContext.NativeCmdList : ComputeContext.NativeCmdList;
            return new RHIStatisticsQuery(GPUDevice.NativeDevice, NativeCmdList);
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
            RHIBuffer GPUBuffer = new RHIBuffer(GPUDevice.NativeDevice, CopyContext.NativeCmdList, InUseFlag, InBufferType, InCount, InStride);
            return GPUBuffer;
        }

        public RHITexture CreateTexture(EUseFlag InUseFlag, ETextureType InTextureType)
        {
            RHITexture Texture = new RHITexture(GPUDevice.NativeDevice, CopyContext.NativeCmdList, InUseFlag, InTextureType);
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
            GPUDevice.NativeDevice.CreateShaderResourceView(Buffer.DefaultResource, SRVDescriptor, DescriptorHandle);

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
            GPUDevice.NativeDevice.CreateUnorderedAccessView(Buffer.DefaultResource, null, UAVDescriptor, DescriptorHandle);

            return new RHIUnorderedAccessView(CbvSrvUavDescriptorFactory.GetDescriptorSize(), DescriptorIndex, DescriptorHandle);
        }

        public RHIUnorderedAccessView CreateUnorderedAccessView(RHITexture Texture)
        {
            RHIUnorderedAccessView UAV = new RHIUnorderedAccessView();
            return UAV;
        }

        public RHIResourceViewRange CreateRHIResourceViewRange(int Count)
        {
            return new RHIResourceViewRange(GPUDevice.NativeDevice, CbvSrvUavDescriptorFactory, Count);
        }

        protected override void DisposeManaged()
        {

        }

        protected override void DisposeUnManaged()
        {
            GPUDevice.Dispose();
            CopyContext.Dispose();
            ComputeContext.Dispose();
            GraphicsContext.Dispose();
            CbvSrvUavDescriptorFactory.Dispose();
        }
    }
}
