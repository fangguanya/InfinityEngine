using System;
using System.Collections.Generic;
using InfinityEngine.Graphics.RHI;

namespace InfinityEngine.Graphics.RDG
{
    internal abstract class IRDGPass
    {
        public int index;
        public string name;
        public int refCount;
        public int colorBufferMaxIndex;
        public bool enablePassCulling;
        public bool enableAsyncCompute;
        public virtual bool hasExecuteFunc => false;
        public FRDGTextureRef depthBuffer;
        public FRDGTextureRef[] colorBuffers;

        public List<FRDGResourceRef>[] resourceReadLists = new List<FRDGResourceRef>[2];
        public List<FRDGResourceRef>[] resourceWriteLists = new List<FRDGResourceRef>[2];
        public List<FRDGResourceRef>[] temporalResourceList = new List<FRDGResourceRef>[2];

        public IRDGPass()
        {
            colorBuffers = new FRDGTextureRef[8];
            colorBufferMaxIndex = -1;

            for (int i = 0; i < 2; ++i)
            {
                resourceReadLists[i] = new List<FRDGResourceRef>();
                resourceWriteLists[i] = new List<FRDGResourceRef>();
                temporalResourceList[i] = new List<FRDGResourceRef>();
            }
        }

        public abstract void Execute(in FRDGContext graphContext, FRHICommandBuffer cmdBuffer);
        public abstract void Release(FRDGObjectPool objectPool);

        public void AddResourceWrite(in FRDGResourceRef res)
        {
            resourceWriteLists[res.iType].Add(res);
        }

        public void AddResourceRead(in FRDGResourceRef res)
        {
            resourceReadLists[res.iType].Add(res);
        }

        public void AddTemporalResource(in FRDGResourceRef res)
        {
            temporalResourceList[res.iType].Add(res);
        }

        public void SetColorBuffer(in FRDGTextureRef resource, int index)
        {
            colorBufferMaxIndex = Math.Max(colorBufferMaxIndex, index);
            colorBuffers[index] = resource;
            AddResourceWrite(resource.handle);
        }

        public void SetDepthBuffer(in FRDGTextureRef resource, in EDepthAccess flags)
        {
            depthBuffer = resource;
            if ((flags & EDepthAccess.Read) != 0)
                AddResourceRead(resource.handle);
            if ((flags & EDepthAccess.Write) != 0)
                AddResourceWrite(resource.handle);
        }

        public void EnablePassCulling(in bool value)
        {
            enablePassCulling = value;
        }

        public void EnableAsyncCompute(in bool value)
        {
            enableAsyncCompute = value;
        }

        public void Clear()
        {
            name = "";
            index = -1;

            for (int i = 0; i < 2; ++i)
            {
                resourceReadLists[i].Clear();
                resourceWriteLists[i].Clear();
                temporalResourceList[i].Clear();
            }

            refCount = 0;
            enablePassCulling = true;
            enableAsyncCompute = false;

            // Invalidate everything
            colorBufferMaxIndex = -1;
            depthBuffer = new FRDGTextureRef();
            for (int i = 0; i < 8; ++i)
            {
                colorBuffers[i] = new FRDGTextureRef();
            }
        }
    }

    public delegate void FRDGExecuteFunc<T>(in T passData, in FRDGContext graphContext, FRHICommandBuffer cmdBuffer) where T : struct;

    internal sealed class FRDGPass<T> : IRDGPass where T : struct
    {
        public T passData;
        public FRDGExecuteFunc<T> m_ExcuteFunc;
        public override bool hasExecuteFunc { get { return m_ExcuteFunc != null; } }

        public override void Execute(in FRDGContext graphContext, FRHICommandBuffer cmdBuffer)
        {
            m_ExcuteFunc(passData, graphContext, cmdBuffer);
        }

        public override void Release(FRDGObjectPool graphObjectPool)
        {
            Clear();
            m_ExcuteFunc = null;
            graphObjectPool.Release(this);
        }
    }

    public struct FRDGPassRef : IDisposable
    {
        bool IsDisposed;
        IRDGPass m_RenderPass;
        FRDGResourceFactory m_ResourceFactory;

        internal FRDGPassRef(IRDGPass renderPass, FRDGResourceFactory resourceFactory)
        {
            IsDisposed = false;
            m_RenderPass = renderPass;
            m_ResourceFactory = resourceFactory;
        }

        public ref T GetPassData<T>() where T : struct => ref ((FRDGPass<T>)m_RenderPass).passData;

        public void EnablePassCulling(in bool value)
        {
            m_RenderPass.EnablePassCulling(value);
        }

        public void EnableAsyncCompute(in bool value)
        {
            m_RenderPass.EnableAsyncCompute(value);
        }

        public FRDGTextureRef ReadTexture(in FRDGTextureRef input)
        {
            m_RenderPass.AddResourceRead(input.handle);
            return input;
        }

        public FRDGTextureRef WriteTexture(in FRDGTextureRef input)
        {
            m_RenderPass.AddResourceWrite(input.handle);
            return input;
        }

        public FRDGTextureRef CreateTemporalTexture(in FRHITextureDescriptor descriptor)
        {
            var result = m_ResourceFactory.CreateTexture(descriptor, 0, m_RenderPass.index);
            m_RenderPass.AddTemporalResource(result.handle);
            return result;
        }

        public FRDGBufferRef ReadBuffer(in FRDGBufferRef input)
        {
            m_RenderPass.AddResourceRead(input.handle);
            return input;
        }

        public FRDGBufferRef WriteBuffer(in FRDGBufferRef input)
        {
            m_RenderPass.AddResourceWrite(input.handle);
            return input;
        }

        public FRDGBufferRef CreateTemporalBuffer(in FRHIBufferDescriptor descriptor)
        {
            var result = m_ResourceFactory.CreateBuffer(descriptor, m_RenderPass.index);
            m_RenderPass.AddTemporalResource(result.handle);
            return result;
        }

        public FRDGTextureRef UseDepthBuffer(in FRDGTextureRef input, in EDepthAccess accessFlag)
        {
            m_RenderPass.SetDepthBuffer(input, accessFlag);
            return input;
        }

        public FRDGTextureRef UseColorBuffer(in FRDGTextureRef input, int index)
        {
            m_RenderPass.SetColorBuffer(input, index);
            return input;
        }

        public void SetRenderFunc<T>(FRDGExecuteFunc<T> excuteFunc) where T : struct
        {
            ((FRDGPass<T>)m_RenderPass).m_ExcuteFunc = excuteFunc;
        }

        void Dispose(in bool disposing)
        {
            if (IsDisposed)
                return;

            IsDisposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
        }
    }
}
