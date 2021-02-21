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
        internal FRHIDevice PhyscisDevice;
        internal FRHICommandContext CopyContext;
        internal FRHICommandContext ComputeContext;
        internal FRHICommandContext GraphicsContext;
        internal List<FExecuteInfo> ExecuteInfos;
        internal FRHIDescriptorHeapFactory CbvSrvUavDescriptorFactory;

        public FRHIGraphicsContext() : base()
        {
            PhyscisDevice = new FRHIDevice();

            ExecuteInfos = new List<FExecuteInfo>(64);

            CopyContext = new FRHICommandContext(PhyscisDevice, CommandListType.Copy);
            ComputeContext = new FRHICommandContext(PhyscisDevice, CommandListType.Compute);
            GraphicsContext = new FRHICommandContext(PhyscisDevice, CommandListType.Direct);

            CbvSrvUavDescriptorFactory = new FRHIDescriptorHeapFactory(PhyscisDevice, DescriptorHeapType.ConstantBufferViewShaderResourceViewUnorderedAccessView, 32768);
        }

        private FRHICommandContext SelectContext(EContextType ContextType)
        {
            FRHICommandContext OutContext = GraphicsContext;

            switch (ContextType)
            {
                case EContextType.Copy:
                    OutContext = CopyContext;
                    break;

                case EContextType.Compute:
                    OutContext = ComputeContext;
                    break;

                case EContextType.Graphics:
                    OutContext = GraphicsContext;
                    break;
            }

            return OutContext;
        }

        public void ExecuteCmdBuffer(EContextType ContextType, FRHICommandBuffer CmdBuffer)
        {
            FExecuteInfo ExecuteInfo;
            ExecuteInfo.Fence = null;
            ExecuteInfo.CmdBuffer = CmdBuffer;
            ExecuteInfo.ExecuteType = EExecuteType.Execute;
            ExecuteInfo.CmdContext = SelectContext(ContextType);
            ExecuteInfos.Add(ExecuteInfo);
        }

        public void WritFence(EContextType ContextType, FRHIFence Fence)
        {
            FExecuteInfo ExecuteInfo;
            ExecuteInfo.Fence = Fence;
            ExecuteInfo.CmdBuffer = null;
            ExecuteInfo.ExecuteType = EExecuteType.Signal;
            ExecuteInfo.CmdContext = SelectContext(ContextType);
            ExecuteInfos.Add(ExecuteInfo);
        }

        public void WaitFence(EContextType ContextType, FRHIFence Fence)
        {
            FExecuteInfo ExecuteInfo;
            ExecuteInfo.Fence = Fence;
            ExecuteInfo.CmdBuffer = null;
            ExecuteInfo.ExecuteType = EExecuteType.Wait;
            ExecuteInfo.CmdContext = SelectContext(ContextType);
            ExecuteInfos.Add(ExecuteInfo);
        }

        public void Submit()
        {
            for(int i = 0; i < ExecuteInfos.Count; ++i)
            {
                FExecuteInfo ExecuteInfo = ExecuteInfos[i];
                switch (ExecuteInfo.ExecuteType)
                {
                    case EExecuteType.Signal:
                        ExecuteInfo.CmdContext.SignalQueue(ExecuteInfo.Fence);
                        break;

                    case EExecuteType.Wait:
                        ExecuteInfo.CmdContext.WaitQueue(ExecuteInfo.Fence);
                        break;

                    case EExecuteType.Execute:
                        ExecuteInfo.CmdContext.ExecuteQueue(ExecuteInfo.CmdBuffer);
                        break;
                }
            }

            ExecuteInfos.Clear();

            CopyContext.Flush();
            ComputeContext.Flush();
            GraphicsContext.Flush();
        }

        public FRHICommandBuffer CreateCmdBuffer(string Name, CommandListType CmdBufferType)
        {
            FRHICommandBuffer CmdBuffer = new FRHICommandBuffer(Name, PhyscisDevice, CmdBufferType);
            CmdBuffer.Close();
            return CmdBuffer;
        }

        public void CreateViewport()
        {

        }

        public FRHIFence CreateFence()
        {
            return new FRHIFence(PhyscisDevice);
        }

        public FRHITimeQuery CreateTimeQuery()
        {
            return new FRHITimeQuery(PhyscisDevice);
        }

        public FRHIOcclusionQuery CreateOcclusionQuery()
        {
            return new FRHIOcclusionQuery(PhyscisDevice);
        }

        public FRHIStatisticsQuery CreateStatisticsQuery()
        {
            return new FRHIStatisticsQuery(PhyscisDevice);
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

        public FRHIBuffer CreateBuffer(ulong Count, ulong Stride, EUseFlag UseFlag, EBufferType BufferType)
        {
            FRHIBuffer GPUBuffer = new FRHIBuffer(PhyscisDevice, UseFlag, BufferType, Count, Stride);
            return GPUBuffer;
        }

        public FRHITexture CreateTexture(EUseFlag UseFlag, ETextureType TextureType)
        {
            FRHITexture Texture = new FRHITexture(PhyscisDevice, UseFlag, TextureType);
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
            PhyscisDevice.NativeDevice.CreateShaderResourceView(Buffer.DefaultResource, SRVDescriptor, DescriptorHandle);

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
            PhyscisDevice.NativeDevice.CreateUnorderedAccessView(Buffer.DefaultResource, null, UAVDescriptor, DescriptorHandle);

            return new FRHIUnorderedAccessView(CbvSrvUavDescriptorFactory.GetDescriptorSize(), DescriptorIndex, DescriptorHandle);
        }

        public FRHIUnorderedAccessView CreateUnorderedAccessView(FRHITexture Texture)
        {
            FRHIUnorderedAccessView UAV = new FRHIUnorderedAccessView();
            return UAV;
        }

        public FRHIResourceViewRange CreateResourceViewRange(int Count)
        {
            return new FRHIResourceViewRange(PhyscisDevice, CbvSrvUavDescriptorFactory, Count);
        }

        protected override void Disposed()
        {
            PhyscisDevice?.Dispose();
            CopyContext?.Dispose();
            ComputeContext?.Dispose();
            GraphicsContext?.Dispose();
            CbvSrvUavDescriptorFactory?.Dispose();
        }
    }
}
