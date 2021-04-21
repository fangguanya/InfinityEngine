using InfinityEngine.Graphics.RHI;

namespace InfinityEngine.Rendering.RenderPipeline
{
    public class FUniversalRenderPipeline : FRenderPipeline
    {
        FRHIBuffer buffer;
        FRHICommandList cmdList;

        public FUniversalRenderPipeline(string pipelineName) : base(pipelineName)
        {

        }

        public override void Init(FRHIGraphicsContext graphicsContext)
        {
            buffer = graphicsContext.CreateBuffer(5, 4, EUseFlag.CPUWrite, EBufferType.Structured);
            cmdList = graphicsContext.CreateCmdList("DefaultCmdList", EContextType.Copy);
        }

        public override void Render(FRHIGraphicsContext graphicsContext)
        {
            cmdList.Clear();
            buffer.SetData<int>(cmdList, 5, 4, 3, 2, 1);

            int[] data = new int[5];
            buffer.GetData<int>(cmdList, data);

            graphicsContext.ExecuteCmdList(EContextType.Copy, cmdList);
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
            cmdList.DrawPrimitiveInstance(null, null, PrimitiveTopology.TriangleList, 0, 0);
            graphicsContext.ExecuteCmdList(EContextType.Graphics, cmdList);
            graphicsContext.WritFence(EContextType.Graphics, graphicsFence);

            //Pass-B in GraphicsQueue
            cmdList.DrawPrimitiveInstance(null, null, PrimitiveTopology.TriangleList, 0, 0);
            graphicsContext.ExecuteCmdList(EContextType.Graphics, cmdList);

            //Pass-C in ComputeQueue and Wait Pass-A
            graphicsContext.WaitFence(EContextType.Compute, graphicsFence);
            cmdList.DispatchCompute(null, 16, 16, 1);
            graphicsContext.ExecuteCmdList(EContextType.Compute, cmdList);
            graphicsContext.WritFence(EContextType.Compute, computeFence);

            //Pass-D in ComputeQueue
            cmdList.DispatchCompute(null, 16, 16, 1);
            graphicsContext.ExecuteCmdList(EContextType.Compute, cmdList);

            //Pass-E in GraphicsQueue and Wait Pass-C
            graphicsContext.WaitFence(EContextType.Graphics, computeFence);
            cmdList.DrawPrimitiveInstance(null, null, PrimitiveTopology.TriangleList, 128, 16);
            graphicsContext.ExecuteCmdList(EContextType.Graphics, cmdList);*/
        }

        protected override void Disposed()
        {
            base.Disposed();
            buffer?.Dispose();
            cmdList?.Dispose();
        }
    }
}
