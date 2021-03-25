using InfinityEngine.Core.Object;
using InfinityEngine.Graphics.RDG;
using InfinityEngine.Graphics.RHI;

namespace InfinityEngine.Renderer.RenderPipeline
{
    public abstract class FRenderPipeline : UObject
    {
        public string name;
        protected FRDGBuilder graphBuilder;

        public FRenderPipeline(string Name)
        {
            name = Name;
            graphBuilder = new FRDGBuilder("UniversalGraphBuilder");
        }

        public abstract void Init(FRHIGraphicsContext GraphicsContext);

        public abstract void Render(FRHIGraphicsContext GraphicsContext);

        protected override void Disposed()
        {
            graphBuilder?.Dispose();
        }
    }
}
