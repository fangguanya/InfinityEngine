using InfinityEngine.Graphics.RHI;
using Vortice.Direct3D;

namespace Infinity.Runtime.Render.RenderPipeline
{
    public class FInfinityRenderPipeline : FRenderPipeline
    {
        public FInfinityRenderPipeline(string PipelineName) : base(PipelineName)
        {

        }

        protected override void Init(FRHIRenderContext RenderContext, FRHICommandBuffer CmdBuffer)
        {

        }

        protected override void Render(FRHIRenderContext RenderContext, FRHICommandBuffer CmdBuffer)
        {
            //ResourceBind Example
            FRHIBuffer Buffer = RenderContext.CreateBuffer(16, 4, EUseFlag.CPUWrite, EBufferType.Structured);

            FRHIShaderResourceView SRV = RenderContext.CreateShaderResourceView(Buffer);
            FRHIUnorderedAccessView UAV = RenderContext.CreateUnorderedAccessView(Buffer);

            FRHIResourceViewRange ResourceViewRange = RenderContext.CreateResourceViewRange(2);
            ResourceViewRange.SetShaderResourceView(0, SRV);
            ResourceViewRange.SetUnorderedAccessView(1, UAV);


            //ASyncCompute Example
            FRHIFence ComputeFence = RenderContext.CreateGPUFence();
            FRHIFence GraphicsFence = RenderContext.CreateGPUFence();

            CmdBuffer.DrawPrimitiveInstance(null, null, PrimitiveTopology.TriangleList, 0, 0);
            RenderContext.ExecuteCmdBuffer(EContextType.Graphics, CmdBuffer);
            RenderContext.WritFence(EContextType.Graphics, GraphicsFence);

            RenderContext.WaitFence(EContextType.Compute, GraphicsFence);
            CmdBuffer.DispatchCompute(null, 16, 16, 1);
            RenderContext.ExecuteCmdBuffer(EContextType.Compute, CmdBuffer);
            RenderContext.WritFence(EContextType.Compute, ComputeFence);

            RenderContext.WaitFence(EContextType.Graphics, ComputeFence);
            CmdBuffer.DrawPrimitiveInstance(null, null, PrimitiveTopology.TriangleList, 128, 16);
            RenderContext.ExecuteCmdBuffer(EContextType.Graphics, CmdBuffer);

            //Submit Context
            RenderContext.Submit();
        }

        protected override void DisposeManaged()
        {
            base.DisposeManaged();
        }

        protected override void DisposeUnManaged()
        {
            base.DisposeUnManaged();
        }
    }
}
