using InfinityEngine.Graphics.RHI;

namespace InfinityEngine.Renderer.RenderPipeline
{
    public class FUniversalRenderPipeline : FRenderPipeline
    {
        FRHIBuffer buffer;
        FRHICommandList rhiCmdList;

        public FUniversalRenderPipeline(string pipelineName) : base(pipelineName)
        {

        }

        public override void Init(FRHIGraphicsContext graphicsContext)
        {
            buffer = graphicsContext.CreateBuffer(5, 4, EUseFlag.CPUWrite, EBufferType.Structured);
            rhiCmdList = graphicsContext.CreateCmdBuffer("DefaultCmdList", Vortice.Direct3D12.CommandListType.Copy);
        }

        public override void Render(FRHIGraphicsContext graphicsContext)
        {
            rhiCmdList.Clear();
            buffer.SetData<int>(rhiCmdList, 1, 2, 3, 4, 5);

            graphicsContext.ExecuteCmdBuffer(EContextType.Copy, rhiCmdList);
            graphicsContext.Submit();


            //Console.WriteLine("Rendering");
            //ResourceBind Example
            /*FRHIBuffer Buffer = GraphicsContext.CreateBuffer(16, 4, EUseFlag.CPUWrite, EBufferType.Structured);

            FRHIShaderResourceView SRV = GraphicsContext.CreateShaderResourceView(Buffer);
            FRHIUnorderedAccessView UAV = GraphicsContext.CreateUnorderedAccessView(Buffer);

            FRHIResourceViewRange ResourceViewRange = GraphicsContext.CreateResourceViewRange(2);
            ResourceViewRange.SetShaderResourceView(0, SRV);
            ResourceViewRange.SetUnorderedAccessView(1, UAV);*/


            //ASyncCompute Example
            /*FRHIFence ComputeFence = GraphicsContext.CreateGPUFence();
            FRHIFence GraphicsFence = GraphicsContext.CreateGPUFence();

            //Pass-A in GraphicsQueue
            CmdBuffer.DrawPrimitiveInstance(null, null, PrimitiveTopology.TriangleList, 0, 0);
            GraphicsContext.ExecuteCmdBuffer(EContextType.Graphics, CmdBuffer);
            GraphicsContext.WritFence(EContextType.Graphics, GraphicsFence);

            //Pass-B in GraphicsQueue
            CmdBuffer.DrawPrimitiveInstance(null, null, PrimitiveTopology.TriangleList, 0, 0);
            GraphicsContext.ExecuteCmdBuffer(EContextType.Graphics, CmdBuffer);

            //Pass-C in ComputeQueue and Wait Pass-A
            GraphicsContext.WaitFence(EContextType.Compute, GraphicsFence);
            CmdBuffer.DispatchCompute(null, 16, 16, 1);
            GraphicsContext.ExecuteCmdBuffer(EContextType.Compute, CmdBuffer);
            GraphicsContext.WritFence(EContextType.Compute, ComputeFence);

            //Pass-D in ComputeQueue
            CmdBuffer.DispatchCompute(null, 16, 16, 1);
            GraphicsContext.ExecuteCmdBuffer(EContextType.Compute, CmdBuffer);

            //Pass-E in GraphicsQueue and Wait Pass-C
            GraphicsContext.WaitFence(EContextType.Graphics, ComputeFence);
            CmdBuffer.DrawPrimitiveInstance(null, null, PrimitiveTopology.TriangleList, 128, 16);
            GraphicsContext.ExecuteCmdBuffer(EContextType.Graphics, CmdBuffer);*/
        }

        protected override void Disposed()
        {
            base.Disposed();

            buffer?.Dispose();
            rhiCmdList?.Dispose();
        }
    }
}
