using System;
using InfinityEngine.Graphics.RHI;

namespace InfinityEngine.Graphics.RDG
{
    public struct FRDGPassBuilder : IDisposable
    {
        bool bDisposed;
        IRDGPass m_RenderPass;
        FRDGResourceFactory m_ResourceFactory;

        internal FRDGPassBuilder(IRDGPass renderPass, FRDGResourceFactory resourceFactory)
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
