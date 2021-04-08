using InfinityEngine.Graphics.RHI;

namespace InfinityEngine.Rendering.RenderPipeline
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
            rhiCmdList = graphicsContext.CreateCmdList("DefaultCmdList", Vortice.Direct3D12.CommandListType.Copy);
        }

        public override void Render(FRHIGraphicsContext graphicsContext)
        {
            rhiCmdList.Clear();
            buffer.SetData<int>(rhiCmdList, 1, 2, 3, 4, 5);

            graphicsContext.ExecuteCmdList(EContextType.Copy, rhiCmdList);
            graphicsContext.Submit();


            //Console.WriteLine("Rendering");
            //ResourceBind Example
            /*FRHIBuffer Buffer = graphicsContext.CreateBuffer(16, 4, EUseFlag.CPUWrite, EBufferType.Structured);

            FRHIShaderResourceView SRV = graphicsContext.CreateShaderResourceView(Buffer);
            FRHIUnorderedAccessView UAV = graphicsContext.CreateUnorderedAccessView(Buffer);

            FRHIResourceViewRange ResourceViewRange = graphicsContext.CreateResourceViewRange(2);
            ResourceViewRange.SetShaderResourceView(0, SRV);
            ResourceViewRange.SetUnorderedAccessView(1, UAV);*/


            //ASyncCompute Example
            /*FRHIFence computeFence = graphicsContext.CreateFence();
            FRHIFence graphicsFence = graphicsContext.CreateFence();

            //Pass-A in GraphicsQueue
            rhiCmdList.DrawPrimitiveInstance(null, null, PrimitiveTopology.TriangleList, 0, 0);
            graphicsContext.ExecuteCmdList(EContextType.Graphics, rhiCmdList);
            graphicsContext.WritFence(EContextType.Graphics, graphicsFence);

            //Pass-B in GraphicsQueue
            rhiCmdList.DrawPrimitiveInstance(null, null, PrimitiveTopology.TriangleList, 0, 0);
            graphicsContext.ExecuteCmdList(EContextType.Graphics, rhiCmdList);

            //Pass-C in ComputeQueue and Wait Pass-A
            graphicsContext.WaitFence(EContextType.Compute, graphicsFence);
            rhiCmdList.DispatchCompute(null, 16, 16, 1);
            graphicsContext.ExecuteCmdList(EContextType.Compute, rhiCmdList);
            graphicsContext.WritFence(EContextType.Compute, computeFence);

            //Pass-D in ComputeQueue
            rhiCmdList.DispatchCompute(null, 16, 16, 1);
            graphicsContext.ExecuteCmdList(EContextType.Compute, rhiCmdList);

            //Pass-E in GraphicsQueue and Wait Pass-C
            graphicsContext.WaitFence(EContextType.Graphics, computeFence);
            rhiCmdList.DrawPrimitiveInstance(null, null, PrimitiveTopology.TriangleList, 128, 16);
            graphicsContext.ExecuteCmdList(EContextType.Graphics, rhiCmdList);*/
        }

        protected override void Disposed()
        {
            base.Disposed();

            buffer?.Dispose();
            rhiCmdList?.Dispose();
        }
    }
}
