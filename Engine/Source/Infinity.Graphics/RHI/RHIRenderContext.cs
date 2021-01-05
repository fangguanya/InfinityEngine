using Vortice.DXGI;
using Vortice.Direct3D12;
using InfinityEngine.Core.Object;
using InfinityEngine.Core.Container;

namespace InfinityEngine.Graphics.RHI
{
    public class FRHIRenderContext : UObject
    {
        internal FRHIDevice Device;
        internal FRHICommandContext CopyContext;
        internal FRHICommandContext ComputeContext;
        internal FRHICommandContext GraphicsContext;
        internal FRHIDescriptorHeapFactory CbvSrvUavDescriptorFactory;

        public FRHIRenderContext() : base()
        {
            Device = new FRHIDevice();

            CopyContext = new FRHICommandContext(Device.NativeDevice, CommandListType.Copy);
            ComputeContext = new FRHICommandContext(Device.NativeDevice, CommandListType.Compute);
            GraphicsContext = new FRHICommandContext(Device.NativeDevice, CommandListType.Direct);

            CbvSrvUavDescriptorFactory = new FRHIDescriptorHeapFactory(Device.NativeDevice, DescriptorHeapType.ConstantBufferViewShaderResourceViewUnorderedAccessView, 32768);
        }

        public void ExecuteCmdBuffer(FRHICommandBuffer CmdBuffer)
        {

        }

        public void ExecuteCmdBufferASync(FRHICommandBuffer CmdBuffer)
        {

        }

        public void Submit()
        {

        }

        public FRHIFence CreateGPUFence()
        {
            return new FRHIFence(Device.NativeDevice);
        }

        public void CreateViewport()
        {

        }

        public FRHITimeQuery CreateTimeQuery(FRHICommandBuffer CmdBuffer)
        {
            return new FRHITimeQuery(Device.NativeDevice, CmdBuffer.NativeCmdList);
        }

        public FRHIOcclusionQuery CreateOcclusionQuery(FRHICommandBuffer CmdBuffer)
        {
            return new FRHIOcclusionQuery(Device.NativeDevice, CmdBuffer.NativeCmdList);
        }

        public FRHIStatisticsQuery CreateStatisticsQuery(FRHICommandBuffer CmdBuffer)
        {
            return new FRHIStatisticsQuery(Device.NativeDevice, CmdBuffer.NativeCmdList);
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

        public FRHIBuffer CreateBuffer(ulong InCount, ulong InStride, EUseFlag InUseFlag, EBufferType InBufferType)
        {
            FRHIBuffer GPUBuffer = new FRHIBuffer(Device.NativeDevice, null, InUseFlag, InBufferType, InCount, InStride);
            return GPUBuffer;
        }

        public FRHITexture CreateTexture(EUseFlag InUseFlag, ETextureType InTextureType)
        {
            FRHITexture Texture = new FRHITexture(Device.NativeDevice, null, InUseFlag, InTextureType);
            return Texture;
        }

        public FRHIDeptnStencilView CreateDepthStencilView(FRHITexture Texture)
        {
            FRHIDeptnStencilView DSV = new FRHIDeptnStencilView();
            return DSV;
        }

        public FRHIRenderTargetView CreateRenderTargetView(FRHITexture Texture)
        {
            FRHIRenderTargetView RTV = new FRHIRenderTargetView();
            return RTV;
        }

        public FRHIIndexBufferView CreateIndexBufferView(FRHIBuffer IndexBuffer)
        {
            FRHIIndexBufferView IBO = new FRHIIndexBufferView();
            return IBO;
        }

        public FRHIVertexBufferView CreateVertexBufferView(FRHIBuffer VertexBuffer)
        {
            FRHIVertexBufferView VBO = new FRHIVertexBufferView();
            return VBO;
        }

        public FRHIConstantBufferView CreateConstantBufferView(FRHIBuffer ConstantBuffer)
        {
            FRHIConstantBufferView CBV = new FRHIConstantBufferView();
            return CBV;
        }

        public FRHIShaderResourceView CreateShaderResourceView(FRHIBuffer Buffer)
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

            return new FRHIShaderResourceView(CbvSrvUavDescriptorFactory.GetDescriptorSize(), DescriptorIndex, DescriptorHandle);
        }

        public FRHIShaderResourceView CreateShaderResourceView(FRHITexture Texture)
        {
            FRHIShaderResourceView SRV = new FRHIShaderResourceView();
            return SRV;
        }

        public FRHIUnorderedAccessView CreateUnorderedAccessView(FRHIBuffer Buffer)
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

            return new FRHIUnorderedAccessView(CbvSrvUavDescriptorFactory.GetDescriptorSize(), DescriptorIndex, DescriptorHandle);
        }

        public FRHIUnorderedAccessView CreateUnorderedAccessView(FRHITexture Texture)
        {
            FRHIUnorderedAccessView UAV = new FRHIUnorderedAccessView();
            return UAV;
        }

        public FRHIResourceViewRange CreateRHIResourceViewRange(int Count)
        {
            return new FRHIResourceViewRange(Device.NativeDevice, CbvSrvUavDescriptorFactory, Count);
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
