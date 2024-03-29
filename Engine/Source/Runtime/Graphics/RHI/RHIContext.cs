﻿using System;
using InfinityEngine.Core.Object;
using System.Runtime.CompilerServices;

namespace InfinityEngine.Graphics.RHI
{
    public enum EContextType
    {
        Copy = 3,
        Compute = 2,
        Graphics = 0
    }

    internal struct FExecuteInfo
    {
        public FRHIFence fence;
        public EExecuteType executeType;
        public FRHICommandBuffer cmdBuffer;
        public FRHICommandContext cmdContext;
    }

    public abstract class FRHIContext : FDisposal
    {
        public virtual bool copyQueueState => false;
        public virtual ulong copyFrequency => 0;
        public virtual ulong computeFrequency => 0;
        public virtual ulong graphicsFrequency => 0;

        internal abstract FRHICommandContext SelectContext(in EContextType contextType);
        public abstract FRHICommandBuffer CreateCommandBuffer(in EContextType contextType, string name);
        public abstract FRHICommandBuffer GetCommandBuffer(in EContextType contextType, string name);
        public abstract void ReleaseCommandBuffer(FRHICommandBuffer cmdBuffer);
        public abstract void WriteToFence(in EContextType contextType, FRHIFence fence);
        public abstract void WaitForFence(in EContextType contextType, FRHIFence fence);
        public abstract void ExecuteCommandBuffer(FRHICommandBuffer cmdBuffer);
        internal abstract void Submit();
        public abstract FRHISwapChain CreateSwapChain(string name, in uint width, in uint height, in IntPtr windowPtr);
        public abstract FRHIFence CreateFence(string name);
        public abstract FRHIFence GetFence(string name);
        public abstract void ReleaseFence(FRHIFence fence);
        public abstract FRHIQuery CreateQuery(in EQueryType queryType, string name);
        public abstract FRHIQuery GetQuery(in EQueryType queryType, string name);
        public abstract void ReleaseQuery(FRHIQuery query);
        public abstract FRHIComputePipelineState CreateComputePipelineState(in FRHIComputePipelineDescriptor descriptor);
        public abstract FRHIRayTracePipelineState CreateRayTracePipelineState(in FRHIRayTracePipelineDescriptor descriptor);
        public abstract FRHIGraphicsPipelineState CreateGraphicsPipelineState(in FRHIGraphicsPipelineDescriptor descriptor);
        public abstract void CreateSamplerState();
        public abstract void CreateVertexInputLayout();
        public abstract void CreateResourceInputLayout();
        public abstract FRHIBuffer CreateBuffer(in FBufferDescriptor descriptor);
        public abstract FRHIBufferRef GetBuffer(in FBufferDescriptor descriptor);
        public abstract void ReleaseBuffer(in FRHIBufferRef bufferRef);
        public abstract FRHITexture CreateTexture(in FTextureDescriptor descriptor);
        public abstract FRHITextureRef GetTexture(in FTextureDescriptor descriptor);
        public abstract void ReleaseTexture(FRHITextureRef textureRef);
        public abstract FRHIIndexBufferView CreateIndexBufferView(FRHIBuffer buffer);
        public abstract FRHIVertexBufferView CreateVertexBufferView(FRHIBuffer buffer);
        public abstract FRHIDeptnStencilView CreateDepthStencilView(FRHITexture texture);
        public abstract FRHIRenderTargetView CreateRenderTargetView(FRHITexture texture);
        public abstract FRHIConstantBufferView CreateConstantBufferView(FRHIBuffer buffer);
        public abstract FRHIShaderResourceView CreateShaderResourceView(FRHIBuffer buffer);
        public abstract FRHIShaderResourceView CreateShaderResourceView(FRHITexture texture);
        public abstract FRHIUnorderedAccessView CreateUnorderedAccessView(FRHIBuffer buffer);
        public abstract FRHIUnorderedAccessView CreateUnorderedAccessView(FRHITexture texture);
        public abstract FRHIResourceSet CreateResourceSet(in uint count);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SubmitContext(FRHIContext context)
        {
            context.Submit();
        }
    }
}
