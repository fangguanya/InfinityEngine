using System;
using InfinityEngine.Graphics.RHI;
using InfinityEngine.Core.Profiler;
using InfinityEngine.Rendering.RenderLoop;

namespace InfinityEngine.Rendering.RenderPipeline
{
    public class FUniversalRenderPipeline : FRenderPipeline
    {
        /*bool dataReady;
        int[] readData;
        float cpuTime
        {
            get { return (float)timeProfiler.microseconds / 1000.0f; }
        }
        float gpuTime;

        FRHIFence fence;
        FRHIQuery query;
        FRHIBufferRef bufferRef;
        FTimeProfiler timeProfiler;*/

        public FUniversalRenderPipeline(string pipelineName) : base(pipelineName) 
        { 

        }

        public override void Init(FRenderContext renderContext, FRHIGraphicsContext graphicsContext)
        {
            Console.WriteLine("Init RenderPipeline");

            /*dataReady = true;
            readData = new int[10000000];
            timeProfiler = new FTimeProfiler();
            FRHIBufferDescription description = new FRHIBufferDescription(10000000, 4, EUsageType.Dynamic | EUsageType.Staging);

            fence = graphicsContext.GetFence();
            query = graphicsContext.GetQuery(EQueryType.CopyTimestamp);
            bufferRef = graphicsContext.GetBuffer(description);
            FRHICommandBuffer cmdBuffer = graphicsContext.GetCommandBuffer(EContextType.Copy, "CmdBuffer1", true);
            cmdBuffer.Clear();

            int[] data = new int[10000000];
            for (int i = 0; i < 10000000; ++i) { data[i] = 10000000 - i; }
            bufferRef.buffer.SetData(cmdBuffer, data);
            graphicsContext.ExecuteCommandBuffer(EContextType.Copy, cmdBuffer);
            graphicsContext.Submit();*/
        }

        public override void Render(FRenderContext renderContext, FRHIGraphicsContext graphicsContext)
        {
            /*timeProfiler.Restart();

            if (dataReady)
            {
                FRHICommandBuffer cmdBuffer = graphicsContext.GetCommandBuffer(EContextType.Copy, "CmdBuffer2", true);
                cmdBuffer.Clear();
                cmdBuffer.BeginQuery(query);
                bufferRef.buffer.RequestReadback<int>(cmdBuffer);
                cmdBuffer.EndQuery(query);
                graphicsContext.ExecuteCommandBuffer(EContextType.Copy, cmdBuffer);
                graphicsContext.WriteFence(EContextType.Copy, fence);
                //graphicsContext.WaitFence(EContextType.Graphics, fence);
            }

            if (dataReady = fence.IsCompleted)
            {
                bufferRef.buffer.GetData(readData);
                gpuTime = query.GetResult(graphicsContext.copyFrequency);
            }

            timeProfiler.Stop();
            graphicsContext.Submit();

            //Console.WriteLine("||");
            Console.WriteLine("Draw : " + cpuTime + "ms");
            Console.WriteLine("GPU  : " + gpuTime + "ms");*/
        }

        public override void Release(FRenderContext renderContext, FRHIGraphicsContext graphicsContext)
        {
            /*graphicsContext.ReleaseFence(fence);
            graphicsContext.ReleaseQuery(query);
            graphicsContext.ReleaseBuffer(bufferRef);*/
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