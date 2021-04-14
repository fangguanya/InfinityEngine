using InfinityEngine.Graphics.RHI;

namespace InfinityEngine.Rendering.RenderPipeline
{
    public class FUniversalRenderPipeline : FRenderPipeline
    {
        FRHIBuffer buffer;
        FRHICommandList commandList;

        public FUniversalRenderPipeline(string pipelineName) : base(pipelineName)
        {

        }

        public override void Init(FRHIGraphicsContext graphicsContext)
        {
            buffer = graphicsContext.CreateBuffer(5, 4, EUseFlag.CPUWrite, EBufferType.Structured);
            commandList = graphicsContext.CreateCmdList("DefaultCmdList", Vortice.Direct3D12.CommandListType.Copy);
        }

        public override void Render(FRHIGraphicsContext graphicsContext)
        {
            commandList.Clear();
            buffer.SetData<int>(commandList, 1, 2, 3, 4, 5);

            graphicsContext.ExecuteCmdList(EContextType.Copy, commandList);
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
            commandList.DrawPrimitiveInstance(null, null, PrimitiveTopology.TriangleList, 0, 0);
            graphicsContext.ExecuteCmdList(EContextType.Graphics, commandList);
            graphicsContext.WritFence(EContextType.Graphics, graphicsFence);

            //Pass-B in GraphicsQueue
            commandList.DrawPrimitiveInstance(null, null, PrimitiveTopology.TriangleList, 0, 0);
            graphicsContext.ExecuteCmdList(EContextType.Graphics, commandList);

            //Pass-C in ComputeQueue and Wait Pass-A
            graphicsContext.WaitFence(EContextType.Compute, graphicsFence);
            commandList.DispatchCompute(null, 16, 16, 1);
            graphicsContext.ExecuteCmdList(EContextType.Compute, commandList);
            graphicsContext.WritFence(EContextType.Compute, computeFence);

            //Pass-D in ComputeQueue
            commandList.DispatchCompute(null, 16, 16, 1);
            graphicsContext.ExecuteCmdList(EContextType.Compute, commandList);

            //Pass-E in GraphicsQueue and Wait Pass-C
            graphicsContext.WaitFence(EContextType.Graphics, computeFence);
            commandList.DrawPrimitiveInstance(null, null, PrimitiveTopology.TriangleList, 128, 16);
            graphicsContext.ExecuteCmdList(EContextType.Graphics, commandList);*/
        }

        protected override void Disposed()
        {
            base.Disposed();

            buffer?.Dispose();
            commandList?.Dispose();
        }
    }
}
