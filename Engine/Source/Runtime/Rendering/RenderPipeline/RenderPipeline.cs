using InfinityEngine.Core.Object;
using InfinityEngine.Graphics.RDG;
using InfinityEngine.Graphics.RHI;
using InfinityEngine.Rendering.RenderLoop;

namespace InfinityEngine.Rendering.RenderPipeline
{
    public abstract class FRenderPipeline : FDisposable
    {
        public string name;
        protected FRDGBuilder m_GraphBuilder;

        public FRenderPipeline(string name)
        {
            this.name = name;
            this.m_GraphBuilder = new FRDGBuilder("GraphBuilder");
        }

        public abstract void Init(FRenderContext renderContext, FRHIGraphicsContext graphicsContext);

        public abstract void Render(FRenderContext renderContext, FRHIGraphicsContext graphicsContext);

        public abstract void Release(FRenderContext renderContext, FRHIGraphicsContext graphicsContext);

        protected override void Release()
        {
            m_GraphBuilder?.Dispose();
        }
    }
}
