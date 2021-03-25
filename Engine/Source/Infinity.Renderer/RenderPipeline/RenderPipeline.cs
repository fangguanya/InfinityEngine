using InfinityEngine.Core.Object;
using InfinityEngine.Graphics.RDG;
using InfinityEngine.Graphics.RHI;

namespace InfinityEngine.Renderer.RenderPipeline
{
    public abstract class FRenderPipeline : UObject
    {
        public string name;
        protected FRDGBuilder graphBuilder;

        public FRenderPipeline(string name)
        {
            this.name = name;
            this.graphBuilder = new FRDGBuilder("UniversalGraphBuilder");
        }

        public abstract void Init(FRHIGraphicsContext graphicsContext);

        public abstract void Render(FRHIGraphicsContext graphicsContext);

        protected override void Disposed()
        {
            graphBuilder?.Dispose();
        }
    }
}
