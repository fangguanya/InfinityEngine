using InfinityEngine.Core.UObject;
using InfinityEngine.Graphics.RDG;
using InfinityEngine.Graphics.RHI;

namespace Infinity.Runtime.Render.RenderPipeline
{
    public abstract class RenderPipeline : UObject
    {
        public string name;
        protected RDGGraphBuilder GraphBuilder;

        public RenderPipeline(string Name)
        {
            name = Name;
            GraphBuilder = new RDGGraphBuilder(name + "Graph");
        }

        protected abstract void Init(RHIRenderContext RenderContext, RHICommandBuffer CmdBuffer);

        protected abstract void Render(RHIRenderContext RenderContext, RHICommandBuffer CmdBuffer);

        protected override void DisposeManaged()
        {
            
        }

        protected override void DisposeUnManaged()
        {
            GraphBuilder.Dispose();
        }
    }
}
