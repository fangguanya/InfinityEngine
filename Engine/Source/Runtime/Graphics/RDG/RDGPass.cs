using System;
using System.Collections.Generic;
using InfinityEngine.Graphics.RHI;

namespace InfinityEngine.Graphics.RDG
{
    internal abstract class IRDGPass
    {
        internal int index;
        internal string name;

        public int refCount;
        public int colorBufferMaxIndex;
        public bool enablePassCulling;
        public bool enableAsyncCompute;
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

        public abstract void Execute(ref FRDGContext graphContext);
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

        public void EnableAsyncCompute(in bool value)
        {
            enableAsyncCompute = value;
        }

        public void AllowPassCulling(in bool value)
        {
            enablePassCulling = value;
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

    public delegate void FRDGExecuteFunc<T>(ref T passData, ref FRDGContext graphContext) where T : struct;

    internal sealed class FRDGPass<T> : IRDGPass where T : struct
    {
        internal T passData;
        internal FRDGExecuteFunc<T> ExcuteFunc;

        public override void Execute(ref FRDGContext graphContext)
        {
            ExcuteFunc(ref passData, ref graphContext);
        }

        public override void Release(FRDGObjectPool graphObjectPool)
        {
            Clear();
            ExcuteFunc = null;
            graphObjectPool.Release(this);
        }
    }

    public struct FRDGPassRef : IDisposable
    {
        bool bDisposed;
        IRDGPass m_RenderPass;
        FRDGResourceFactory m_ResourceFactory;

        internal FRDGPassRef(IRDGPass renderPass, FRDGResourceFactory resourceFactory)
        {
            bDisposed = false;
            m_RenderPass = renderPass;
            m_ResourceFactory = resourceFactory;
        }

        public ref T GetPassData<T>() where T : struct => ref ((FRDGPass<T>)m_RenderPass).passData;

        public void EnableAsyncCompute(bool value)
        {
            m_RenderPass.EnableAsyncCompute(value);
        }

        public void AllowPassCulling(bool value)
        {
            m_RenderPass.AllowPassCulling(value);
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

        public FRDGTextureRef CreateTemporalTexture(in FRHITextureDescription description)
        {
            var result = m_ResourceFactory.CreateTexture(description, 0, m_RenderPass.index);
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

        public FRDGBufferRef CreateTemporalBuffer(in FRHIBufferDescription description)
        {
            var result = m_ResourceFactory.CreateBuffer(description, m_RenderPass.index);
            m_RenderPass.AddTemporalResource(result.handle);
            return result;
        }

        public FRDGTextureRef UseDepthBuffer(in FRDGTextureRef input, EDepthAccess accessFlag)
        {
            m_RenderPass.SetDepthBuffer(input, accessFlag);
            return input;
        }

        public FRDGTextureRef UseColorBuffer(in FRDGTextureRef input, int index)
        {
            m_RenderPass.SetColorBuffer(input, index);
            return input;
        }

        public void SetRenderFunc<T>(FRDGExecuteFunc<T> ExcuteFunc) where T : struct
        {
            ((FRDGPass<T>)m_RenderPass).ExcuteFunc = ExcuteFunc;
        }

        void Dispose(bool disposing)
        {
            if (bDisposed)
                return;

            bDisposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
        }
    }
}
