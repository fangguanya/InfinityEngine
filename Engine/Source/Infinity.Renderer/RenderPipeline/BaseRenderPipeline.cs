using InfinityEngine.Core.Object;
using InfinityEngine.Graphics.RDG;
using InfinityEngine.Graphics.RHI;

namespace InfinityEngine.Renderer.RenderPipeline
{
    public abstract class FRenderPipeline : UObject
    {
        public string name;
        protected FRDGBuilder GraphBuilder;

        public FRenderPipeline(string Name)
        {
            name = Name;
            GraphBuilder = new FRDGBuilder("InfinityRenderGraph");
        }

        public abstract void Init(FRHIRenderContext RenderContext);

        public abstract void Render(FRHIRenderContext RenderContext);

        protected override void Disposed()
        {
            GraphBuilder?.Dispose();
        }
    }
}
