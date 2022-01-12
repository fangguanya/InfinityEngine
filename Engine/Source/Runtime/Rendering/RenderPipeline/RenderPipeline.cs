﻿using InfinityEngine.Core.Object;
using InfinityEngine.Graphics.RDG;
using InfinityEngine.Graphics.RHI;
using InfinityEngine.Rendering.RenderLoop;

namespace InfinityEngine.Rendering.RenderPipeline
{
    public abstract class FRenderPipeline : FDisposal
    {
        public string name;
        protected FRDGBuilder m_GraphBuilder;

        public FRenderPipeline(string name)
        {
            this.name = name;
            this.m_GraphBuilder = new FRDGBuilder("GraphBuilder");
        }

        public abstract void Init(FRHIDeviceContext deviceContext, FRenderContext renderContext);

        public abstract void Render(FRHIDeviceContext deviceContext, FRenderContext renderContext);

        public abstract void Release(FRHIDeviceContext deviceContext, FRenderContext renderContext);

        protected override void Release()
        {
            m_GraphBuilder?.Dispose();
        }
    }
}
