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

            FRHIResourceViewRange ResourceViewRange = RenderContext.CreateRHIResourceViewRange(2);
            ResourceViewRange.SetShaderResourceView(0, SRV);
            ResourceViewRange.SetUnorderedAccessView(1, UAV);


            //ASyncCompute Example
            /*RHIFence ComputeFence = RenderContext.CreateComputeFence();
            RHIFence GraphicsFence = RenderContext.CreateGraphicsFence();

            CmdBuffer.DrawPrimitiveInstance(null, null, PrimitiveTopology.TriangleList, 0, 0);
            CmdBuffer.WriteFence(GraphicsFence);
            RenderContext.ExecuteCmdBuffer(CmdBuffer);

            CmdBuffer.WaitFence(GraphicsFence);
            CmdBuffer.DispatchCompute();
            CmdBuffer.WriteFence(ComputeFence);
            RenderContext.ExecuteCmdBufferASync(CmdBuffer);

            CmdBuffer.WaitFence(ComputeFence);
            CmdBuffer.DrawPrimitiveInstance();
            RenderContext.ExecuteCmdBuffer(CmdBuffer);

            RenderContext.Submit();*/
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
