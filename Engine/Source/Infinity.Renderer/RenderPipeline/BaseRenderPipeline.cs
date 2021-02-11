using InfinityEngine.Core.Object;
using InfinityEngine.Graphics.RDG;
using InfinityEngine.Graphics.RHI;

namespace Infinity.Runtime.Render.RenderPipeline
{
    public abstract class FRenderPipeline : UObject
    {
        public string name;
        protected FRDGBuilder GraphBuilder;

        public FRenderPipeline(string Name)
        {
            name = Name;
            GraphBuilder = new FRDGBuilder(name + "Graph");
        }

        protected abstract void Init(FRHIRenderContext RenderContext);

        protected abstract void Render(FRHIRenderContext RenderContext);

        protected override void Disposed()
        {
            GraphBuilder?.Dispose();
        }
    }
}
