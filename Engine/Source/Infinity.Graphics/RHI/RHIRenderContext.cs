using Vortice.DXGI;
using Vortice.Direct3D12;
using InfinityEngine.Core.Object;
using InfinityEngine.Core.Container;

namespace InfinityEngine.Graphics.RHI
{
    public class RHIRenderContext : UObject
    {
        internal RHIDevice Device;
        internal RHICommandContext CopyContext;
        internal RHICommandContext ComputeContext;
        internal RHICommandContext GraphicsContext;
        internal RHIDescriptorHeapFactory CbvSrvUavDescriptorFactory;

        public RHIRenderContext() : base()
        {
            Device = new RHIDevice();

            CopyContext = new RHICommandContext(Device.NativeDevice, CommandListType.Copy);
            ComputeContext = new RHICommandContext(Device.NativeDevice, CommandListType.Compute);
            GraphicsContext = new RHICommandContext(Device.NativeDevice, CommandListType.Direct);

            CbvSrvUavDescriptorFactory = new RHIDescriptorHeapFactory(Device.NativeDevice, DescriptorHeapType.ConstantBufferViewShaderResourceViewUnorderedAccessView, 32768);
        }

        public void ExecuteCmdBuffer(RHICommandBuffer CmdBuffer)
        {

        }

        public void ExecuteCmdBufferASync(RHICommandBuffer CmdBuffer)
        {

        }

        public void Submit()
        {

        }

        public RHIFence CreateGPUFence()
        {
            return new RHIFence(Device.NativeDevice);
        }

        public void CreateViewport()
        {

        }

        public RHITimeQuery CreateTimeQuery(RHICommandBuffer CmdBuffer)
        {
            return new RHITimeQuery(Device.NativeDevice, CmdBuffer.NativeCmdList);
        }

        public RHIOcclusionQuery CreateOcclusionQuery(RHICommandBuffer CmdBuffer)
        {
            return new RHIOcclusionQuery(Device.NativeDevice, CmdBuffer.NativeCmdList);
        }

        public RHIStatisticsQuery CreateStatisticsQuery(RHICommandBuffer CmdBuffer)
        {
            return new RHIStatisticsQuery(Device.NativeDevice, CmdBuffer.NativeCmdList);
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
            RHIBuffer GPUBuffer = new RHIBuffer(Device.NativeDevice, null, InUseFlag, InBufferType, InCount, InStride);
            return GPUBuffer;
        }

        public RHITexture CreateTexture(EUseFlag InUseFlag, ETextureType InTextureType)
        {
            RHITexture Texture = new RHITexture(Device.NativeDevice, null, InUseFlag, InTextureType);
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
