using Infinity.Runtime.Graphics.RDG;
using Infinity.Runtime.Graphics.RHI;
using Infinity.Runtime.Graphics.Core;

namespace Infinity.Runtime.Render.RenderPipeline
{
    public abstract class RenderPipeline : TObject
    {
        public string name;
        protected RenderGraph GraphBuilder;

        public RenderPipeline(string Name)
        {
            name = Name;
            GraphBuilder = new RenderGraph(name + "Graph");
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
