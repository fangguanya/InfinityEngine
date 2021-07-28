using InfinityEngine.Core.Object;
using InfinityEngine.Graphics.RDG;
using InfinityEngine.Graphics.RHI;
using InfinityEngine.Rendering.RenderLoop;

namespace InfinityEngine.Rendering.RenderPipeline
{
    public abstract class FRenderPipeline : FDisposer
    {
        public string name;
        protected FRDGGraphBuilder graphBuilder;

        public FRenderPipeline(string name)
        {
            this.name = name;
            this.graphBuilder = new FRDGGraphBuilder("UniversalGraphBuilder");
        }

        public abstract void Init(FRenderContext renderContext, FRHIGraphicsContext graphicsContext);

        public abstract void Render(FRenderContext renderContext, FRHIGraphicsContext graphicsContext);

        protected override void Disposed()
        {
            graphBuilder?.Dispose();
        }
    }
}
