﻿//#define DeferredExecute

using System;
using InfinityEngine.Core.Container;

namespace InfinityEngine.Graphics.RHI.D3D
{
    public unsafe class FD3DContext : FRHIContext
    {
        public override bool copyQueueState
        {
            get
            {
                return m_CopyContext.IsReady;
            }
        }
        public override ulong copyFrequency
        {
            get
            {
                ulong frequency;
                m_CopyContext.nativeCmdQueue->GetTimestampFrequency(&frequency);
                return frequency;
            }
        }
        public override ulong computeFrequency
        {
            get
            {
                ulong frequency;
                m_ComputeContext.nativeCmdQueue->GetTimestampFrequency(&frequency);
                return frequency;
            }
        }
        public override ulong graphicsFrequency
        {
            get
            {
                ulong frequency;
                m_GraphicsContext.nativeCmdQueue->GetTimestampFrequency(&frequency);
                return frequency;
            }
        }

        internal FD3DDevice m_Device;
        internal FRHIFencePool m_FencePool;
        internal FRHIResourcePool m_ResourcePool;
        internal FD3DQueryContext[] m_QueryContext;
        internal FD3DCommandContext m_CopyContext;
        internal FD3DCommandContext m_ComputeContext;
        internal FD3DCommandContext m_GraphicsContext;
        internal TArray<FExecuteInfo> m_ExecuteInfos;
        internal FRHICommandBufferPool m_CopyBufferPool;
        internal FRHICommandBufferPool m_ComputeBufferPool;
        internal FRHICommandBufferPool m_GraphicsBufferPool;
        internal TArray<FRHICommandBuffer> m_ManagedBuffers;
        internal FD3DDescriptorHeapFactory m_DSVDescriptorFactory;
        internal FD3DDescriptorHeapFactory m_RTVDescriptorFactory;
        internal FD3DDescriptorHeapFactory m_SamplerDescriptorFactory;
        internal FD3DDescriptorHeapFactory m_CbvSrvUavDescriptorFactory;

        public FD3DContext()
        {
            m_Device = new FD3DDevice();
            m_FencePool = new FRHIFencePool(this);
            m_ResourcePool = new FRHIResourcePool(this);
            m_ExecuteInfos = new TArray<FExecuteInfo>(32);
            m_ManagedBuffers = new TArray<FRHICommandBuffer>(32);

            m_CopyContext = new FD3DCommandContext(m_Device, EContextType.Copy, "Copy");
            m_ComputeContext = new FD3DCommandContext(m_Device, EContextType.Compute, "Compute");
            m_GraphicsContext = new FD3DCommandContext(m_Device, EContextType.Graphics, "Graphics");

            m_QueryContext = new FD3DQueryContext[2];
            m_QueryContext[0] = new FD3DQueryContext(m_Device, SelectContext(EContextType.Copy), EQueryType.CopyTimestamp, 128, "CopyTimestamp");
            m_QueryContext[1] = new FD3DQueryContext(m_Device, SelectContext(EContextType.Graphics), EQueryType.GenericTimestamp, 128, "GenericTimestamp");

            m_CopyBufferPool = new FRHICommandBufferPool(this, EContextType.Copy);
            m_ComputeBufferPool = new FRHICommandBufferPool(this, EContextType.Compute);
            m_GraphicsBufferPool = new FRHICommandBufferPool(this, EContextType.Graphics);

            //TerraFX.Interop.D3D12MemAlloc.D3D12MA_CreateAllocator
            m_DSVDescriptorFactory = new FD3DDescriptorHeapFactory(m_Device, EDescriptorType.DSV, 256, "DSVDescriptorHeap");
            m_RTVDescriptorFactory = new FD3DDescriptorHeapFactory(m_Device, EDescriptorType.RTV, 256, "RTVDescriptorHeap");
            m_SamplerDescriptorFactory = new FD3DDescriptorHeapFactory(m_Device, EDescriptorType.Sampler, 32768, "SamplerDescriptorHeap");
            m_CbvSrvUavDescriptorFactory = new FD3DDescriptorHeapFactory(m_Device, EDescriptorType.CbvSrvUav, 32768, "CbvSrvUavDescriptorHeap");
        }

        internal override FRHICommandContext SelectContext(in EContextType contextType)
        {
            FRHICommandContext commandContext = m_GraphicsContext;

            switch (contextType)
            {
                case EContextType.Copy:
                    commandContext = m_CopyContext;
                    break;

                case EContextType.Compute:
                    commandContext = m_ComputeContext;
                    break;
            }

            return commandContext;
        }
        
        public override FRHICommandBuffer CreateCommandBuffer(in EContextType contextType, string name)
        {
            return new FD3DCommandBuffer(name, m_Device, SelectContext(contextType), contextType);
        }

        public override FRHICommandBuffer GetCommandBuffer(in EContextType contextType, string name)
        {
            FRHICommandBuffer cmdBuffer = null;
            switch (contextType)
            {
                case EContextType.Copy:
                    cmdBuffer = m_CopyBufferPool.GetTemporary(name);
                    break;

                case EContextType.Compute:
                    cmdBuffer = m_ComputeBufferPool.GetTemporary(name);
                    break;

                case EContextType.Graphics:
                    cmdBuffer = m_GraphicsBufferPool.GetTemporary(name);
                    break;
            }

            cmdBuffer.poolIndex = m_ManagedBuffers.Add(cmdBuffer);

            return cmdBuffer;
        }

        public override void ReleaseCommandBuffer(FRHICommandBuffer cmdBuffer)
        {
            switch (cmdBuffer.contextType)
            {
                case EContextType.Copy:
                    m_CopyBufferPool.ReleaseTemporary(cmdBuffer);
                    break;

                case EContextType.Compute:
                    m_ComputeBufferPool.ReleaseTemporary(cmdBuffer);
                    break;

                case EContextType.Graphics:
                    m_GraphicsBufferPool.ReleaseTemporary(cmdBuffer);
                    break;
            }

            m_ManagedBuffers.RemoveSwapAtIndex(cmdBuffer.poolIndex);
        }

        public override void WriteToFence(in EContextType contextType, FRHIFence fence)
        {
            #if DeferredExecute
                FExecuteInfo executeInfo;
                executeInfo.fence = fence;
                executeInfo.cmdBuffer = null;
                executeInfo.executeType = EExecuteType.Signal;
                executeInfo.cmdContext = SelectContext(contextType);
                m_ExecuteInfos.Add(executeInfo);
            #else
                SelectContext(contextType).SignalQueue(fence);
            #endif
        }

        public override void WaitForFence(in EContextType contextType, FRHIFence fence)
        {
            #if DeferredExecute
                FExecuteInfo executeInfo;
                executeInfo.fence = fence;
                executeInfo.cmdBuffer = null;
                executeInfo.executeType = EExecuteType.Wait;
                executeInfo.cmdContext = SelectContext(contextType);
                m_ExecuteInfos.Add(executeInfo);
            #else
                SelectContext(contextType).WaitQueue(fence);
            #endif
        }

        public override void ExecuteCommandBuffer(FRHICommandBuffer cmdBuffer)
        {
            #if DeferredExecute
                FExecuteInfo executeInfo;
                executeInfo.fence = null;
                executeInfo.cmdBuffer = cmdBuffer;
                executeInfo.executeType = EExecuteType.Execute;
                executeInfo.cmdContext = SelectContext(cmdBuffer.contextType);
                m_ExecuteInfos.Add(executeInfo);
            #else
                SelectContext(cmdBuffer.contextType).ExecuteQueue(cmdBuffer);
            #endif
        }

        internal override void Submit()
        {
            #region submit cmdBuffer
            #if DeferredExecute
                for (int i = 0; i < m_ExecuteInfos.length; ++i)
                {
                    ref FExecuteInfo executeInfo = ref m_ExecuteInfos[i];
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
                m_ExecuteInfos.Clear();
            #endif
            #endregion submit cmdBuffer

            #region release temporal resource
            for (int i = 0; i < m_ManagedBuffers.length; ++i) 
            {
                ReleaseCommandBuffer(m_ManagedBuffers[i]);
                m_ManagedBuffers[i] = null;
            }
            m_ManagedBuffers.Clear();
            #endregion release temporal resource

            m_QueryContext[0].Submit(m_CopyContext);
            m_QueryContext[1].Submit(m_GraphicsContext);

            m_ComputeContext.Flush();
            m_GraphicsContext.Flush();

            m_CopyContext.AsyncFlush();

            m_QueryContext[0].ResolveData();
            m_QueryContext[1].ResolveData();
        }

        public override FRHISwapChain CreateSwapChain(string name, in uint width, in uint height, in IntPtr windowPtr)
        {
            return new FD3DSwapChain(m_Device, m_GraphicsContext, windowPtr.ToPointer(), width, height, name);
        }

        public override FRHIFence CreateFence(string name)
        {
            return new FD3DFence(m_Device, name);
        }
        
        public override FRHIFence GetFence(string name)
        {
            return m_FencePool.GetTemporary(name);
        }

        public override void ReleaseFence(FRHIFence fence)
        {
            m_FencePool.ReleaseTemporary((FD3DFence)fence);
        }

        public override FRHIQuery CreateQuery(in EQueryType queryType, string name)
        {
            FD3DQuery outQuery = null;
            switch (queryType)
            {
                case EQueryType.Occlusion:
                    outQuery = null;
                    break;

                case EQueryType.Statistic:
                    outQuery = null;
                    break;

                case EQueryType.CopyTimestamp:
                    outQuery = new FD3DQuery(m_QueryContext[0]);
                    break;

                case EQueryType.GenericTimestamp:
                    outQuery = new FD3DQuery(m_QueryContext[1]);
                    break;
            }
            return outQuery;
        }

        public override FRHIQuery GetQuery(in EQueryType queryType, string name)
        {
            FRHIQuery outQuery = null;
            switch (queryType)
            {
                case EQueryType.Occlusion:
                    outQuery = null;
                    break;

                case EQueryType.Statistic:
                    outQuery = null;
                    break;

                case EQueryType.CopyTimestamp:
                    outQuery = m_QueryContext[0].GetTemporary(name);
                    break;

                case EQueryType.GenericTimestamp:
                    outQuery = m_QueryContext[1].GetTemporary(name);
                    break;
            }
            return outQuery;
        }

        public override void ReleaseQuery(FRHIQuery query)
        {
            FD3DQuery d3dQuery = (FD3DQuery)query;

            switch (d3dQuery.queryContext.queryType)
            {
                case EQueryType.Occlusion:
                    break;

                case EQueryType.Statistic:
                    break;

                case EQueryType.CopyTimestamp:
                    m_QueryContext[0].ReleaseTemporary(query);
                    break;

                case EQueryType.GenericTimestamp:
                    m_QueryContext[1].ReleaseTemporary(query);
                    break;
            }
        }

        public override FRHIComputePipelineState CreateComputePipelineState(in FRHIComputePipelineDescriptor descriptor)
        {
            return new FRHIComputePipelineState();
        }

        public override FRHIRayTracePipelineState CreateRayTracePipelineState(in FRHIRayTracePipelineDescriptor descriptor)
        {
            return new FRHIRayTracePipelineState();
        }

        public override FRHIGraphicsPipelineState CreateGraphicsPipelineState(in FRHIGraphicsPipelineDescriptor descriptor)
        {
            return null;
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

        public override FRHIBuffer CreateBuffer(in FBufferDescriptor descriptor)
        {
            return new FD3DBuffer(m_Device, descriptor);
        }

        public override FRHIBufferRef GetBuffer(in FBufferDescriptor descriptor)
        {
            return m_ResourcePool.GetBuffer(descriptor);
        }

        public override void ReleaseBuffer(in FRHIBufferRef bufferRef)
        {
            m_ResourcePool.ReleaseBuffer(bufferRef);
        }

        public override FRHITexture CreateTexture(in FTextureDescriptor descriptor)
        {
            return new FD3DTexture(m_Device, descriptor);
        }
        
        public override FRHITextureRef GetTexture(in FTextureDescriptor descriptor)
        {
            return m_ResourcePool.GetTexture(descriptor);
        }

        public override void ReleaseTexture(FRHITextureRef textureRef)
        {
            m_ResourcePool.ReleaseTexture(textureRef);
        }

        public override FRHIIndexBufferView CreateIndexBufferView(FRHIBuffer buffer)
        {
            throw new NotImplementedException();
        }

        public override FRHIVertexBufferView CreateVertexBufferView(FRHIBuffer buffer)
        {
            throw new NotImplementedException();
        }

        public override FRHIDeptnStencilView CreateDepthStencilView(FRHITexture texture)
        {
            return new FD3DDeptnStencilView(m_Device, m_DSVDescriptorFactory, texture);
        }

        public override FRHIRenderTargetView CreateRenderTargetView(FRHITexture texture)
        {
            return new FD3DRenderTargetView(m_Device, m_RTVDescriptorFactory, texture);
        }

        public override FRHIConstantBufferView CreateConstantBufferView(FRHIBuffer buffer)
        {
            return null;
        }

        public override FRHIShaderResourceView CreateShaderResourceView(FRHIBuffer buffer)
        {
            return null;

            /*FD3DBuffer d3dBuffer = (FD3DBuffer)buffer;
            ShaderResourceViewDescription srvDescriptor = new ShaderResourceViewDescription
            {
                Format = Format.Unknown,
                ViewDimension = ShaderResourceViewDimension.Buffer,
                Shader4ComponentMapping = 256,
                Buffer = new BufferShaderResourceView { FirstElement = 0, NumElements = (int)d3dBuffer.descriptor.count, StructureByteStride = (int)d3dBuffer.descriptor.stride }
            };
            int descriptorIndex = m_DescriptorFactory.Allocator(1);
            CpuDescriptorHandle descriptorHandle = m_DescriptorFactory.GetCPUHandleStart() + m_DescriptorFactory.GetDescriptorSize() * descriptorIndex;
            m_Device.nativeDevice.CreateShaderResourceView(d3dBuffer.defaultResource, srvDescriptor, descriptorHandle);

            return new FRHIShaderResourceView(m_DescriptorFactory.GetDescriptorSize(), descriptorIndex, descriptorHandle);*/
        }

        public override FRHIShaderResourceView CreateShaderResourceView(FRHITexture texture)
        {
            return null;
        }

        public override FRHIUnorderedAccessView CreateUnorderedAccessView(FRHIBuffer buffer)
        {
            return null;

            /*FD3DBuffer d3dBuffer = (FD3DBuffer)buffer;
            UnorderedAccessViewDescription uavDescriptor = new UnorderedAccessViewDescription
            {
                Format = Format.Unknown,
                ViewDimension = UnorderedAccessViewDimension.Buffer,
                Buffer = new BufferUnorderedAccessView { NumElements = (int)d3dBuffer.descriptor.count, StructureByteStride = (int)d3dBuffer.descriptor.stride }
            };
            int descriptorIndex = m_DescriptorFactory.Allocator(1);
            CpuDescriptorHandle descriptorHandle = m_DescriptorFactory.GetCPUHandleStart() + m_DescriptorFactory.GetDescriptorSize() * descriptorIndex;
            m_Device.nativeDevice.CreateUnorderedAccessView(d3dBuffer.defaultResource, null, uavDescriptor, descriptorHandle);

            return new FRHIUnorderedAccessView(m_DescriptorFactory.GetDescriptorSize(), descriptorIndex, descriptorHandle);*/
        }

        public override FRHIUnorderedAccessView CreateUnorderedAccessView(FRHITexture texture)
        {
            return null;
        }

        public override FRHIResourceSet CreateResourceSet(in uint count)
        {
            return null;
            //return new FRHIResourceSet(m_Device, m_DescriptorFactory, count);
        }

        protected override void Release()
        {
            m_Device?.Dispose();
            m_FencePool?.Dispose();
            m_ResourcePool?.Dispose();
            m_CopyContext?.Dispose();
            m_ComputeContext?.Dispose();
            m_GraphicsContext?.Dispose();
            m_QueryContext[0]?.Dispose();
            m_QueryContext[1]?.Dispose();
            m_CopyBufferPool?.Dispose();
            m_ComputeBufferPool?.Dispose();
            m_GraphicsBufferPool?.Dispose();
            m_DSVDescriptorFactory?.Dispose();
            m_RTVDescriptorFactory?.Dispose();
            m_SamplerDescriptorFactory?.Dispose();
            m_CbvSrvUavDescriptorFactory?.Dispose();
        }
    }
}
