using System;
using InfinityEngine.Graphics.RHI;
using InfinityEngine.Core.Profiler;
using InfinityEngine.Rendering.RenderLoop;

namespace InfinityEngine.Rendering.RenderPipeline
{
    public class FUniversalRenderPipeline : FRenderPipeline
    {
        /*FRHIFence fence;
        FRHIBuffer buffer;
        FRHICommandList cmdList;

        bool dataReady;
        int[] readData;
        FTimeProfiler timeProfiler;*/

        public FUniversalRenderPipeline(string pipelineName) : base(pipelineName) { }

        public override void Init()
        {
            Console.WriteLine("Initialize RenderPipeline");

            /*dataReady = true;
            timeProfiler = new FTimeProfiler();

            readData = new int[10000000];
            int[] data = new int[10000000];
            for (int i = 0; i < 10000000; ++i) { data[i] = 10000000 - i; }

            fence = graphicsContext.CreateFence();
            buffer = graphicsContext.CreateBuffer(10000000, 4, EUsageType.Dynamic | EUsageType.Staging);
            cmdList = graphicsContext.CreateCmdList("CmdList", EContextType.Copy);

            cmdList.Clear();
            buffer.SetData<int>(cmdList, data);
            graphicsContext.ExecuteCmdList(EContextType.Copy, cmdList);
            graphicsContext.Submit();*/
        }

        public override void Render(FRenderContext renderContext, FRHIGraphicsContext graphicsContext)
        {
            /*timeProfiler.Restart();

            if (dataReady)
            {
                cmdList.Clear();
                buffer.RequestReadback<int>(cmdList);
                graphicsContext.ExecuteCmdList(EContextType.Copy, cmdList);
                graphicsContext.WritFence(EContextType.Copy, fence);
                //graphicsContext.WaitFence(EContextType.Graphics, fence);
            }

            dataReady = fence.Completed();
            if (dataReady)
            {
                buffer.GetData<int>(readData);
            }

            timeProfiler.Stop();
            graphicsContext.Submit();
            Console.WriteLine(timeProfiler.milliseconds + "ms");*/
        }

        protected override void Release()
        {
            base.Release();
            /*fence?.Dispose();
            buffer?.Dispose();
            cmdList?.Dispose();*/
            Console.WriteLine("Release RenderPipeline");
        }
    }
}




















/*buffer.GetData<int>(cmdList, readbackData);
graphicsContext.ExecuteCmdList(EContextType.Copy, cmdList);
graphicsContext.WritFence(EContextType.Copy, fence);
graphicsContext.WaitFence(EContextType.Graphics, fence);*/

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