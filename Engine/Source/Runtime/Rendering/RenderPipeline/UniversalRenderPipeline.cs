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

        public override void Init(FRHIDeviceContext deviceContext, FRenderContext renderContext)
        {
            Console.WriteLine("Init RenderPipeline");

            /*dataReady = true;
            readData = new int[10000000];
            timeProfiler = new FTimeProfiler();
            FRHIBufferDescriptor descriptor = new FRHIBufferDescriptor(10000000, 4, EUsageType.Dynamic | EUsageType.Staging);

            fence = deviceContext.GetFence();
            query = deviceContext.GetQuery(EQueryType.CopyTimestamp);
            bufferRef = deviceContext.GetBuffer(descriptor);
            FRHICommandBuffer cmdBuffer = deviceContext.GetCommandBuffer(EContextType.Copy, "CmdBuffer1", true);
            cmdBuffer.Clear();

            int[] data = new int[10000000];
            for (int i = 0; i < 10000000; ++i) { data[i] = 10000000 - i; }
            bufferRef.buffer.SetData(cmdBuffer, data);
            deviceContext.ExecuteCommandBuffer(EContextType.Copy, cmdBuffer);
            deviceContext.Submit();*/
        }

        public override void Render(FRHIDeviceContext deviceContext, FRenderContext renderContext)
        {
            /*timeProfiler.Restart();

            if (dataReady)
            {
                FRHICommandBuffer cmdBuffer = deviceContext.GetCommandBuffer(EContextType.Copy, "CmdBuffer2", true);
                cmdBuffer.Clear();
                cmdBuffer.BeginQuery(query);
                bufferRef.buffer.RequestReadback<int>(cmdBuffer);
                cmdBuffer.EndQuery(query);
                deviceContext.ExecuteCommandBuffer(EContextType.Copy, cmdBuffer);
                deviceContext.WriteFence(EContextType.Copy, fence);
                //deviceContext.WaitFence(EContextType.Graphics, fence);
            }

            if (dataReady = fence.IsCompleted)
            {
                bufferRef.buffer.GetData(readData);
                gpuTime = query.GetResult(deviceContext.copyFrequency);
            }

            timeProfiler.Stop();
            deviceContext.Submit();

            //Console.WriteLine("||");
            Console.WriteLine("Draw : " + cpuTime + "ms");
            Console.WriteLine("GPU  : " + gpuTime + "ms");*/
        }

        public override void Release(FRHIDeviceContext deviceContext, FRenderContext renderContext)
        {
            /*deviceContext.ReleaseFence(fence);
            deviceContext.ReleaseQuery(query);
            deviceContext.ReleaseBuffer(bufferRef);*/
            Console.WriteLine("Release RenderPipeline");
        }
    }
}




















/*buffer.GetData<int>(cmdList, readbackData);
deviceContext.ExecuteCmdList(EContextType.Copy, cmdList);
deviceContext.WritFence(EContextType.Copy, fence);
deviceContext.WaitFence(EContextType.Graphics, fence);*/

//ResourceBind Example
/*FRHIBuffer Buffer = deviceContext.CreateBuffer(16, 4, EUseFlag.CPUWrite, EBufferType.Structured);

FRHIShaderResourceView SRV = deviceContext.CreateShaderResourceView(Buffer);
FRHIUnorderedAccessView UAV = deviceContext.CreateUnorderedAccessView(Buffer);

FRHIResourceViewRange ResourceViewRange = deviceContext.CreateResourceViewRange(2);
ResourceViewRange.SetShaderResourceView(0, SRV);
ResourceViewRange.SetUnorderedAccessView(1, UAV);*/


//ASyncCompute Example
/*FRHIFence computeFence = deviceContext.CreateFence();
FRHIFence graphicsFence = deviceContext.CreateFence();

//Pass-A in GraphicsQueue
cmdList.DrawPrimitiveInstance(null, null, PrimitiveTopology.TriangleList, 0, 0);
deviceContext.ExecuteCmdList(EContextType.Graphics, cmdList);
deviceContext.WritFence(EContextType.Graphics, graphicsFence);

//Pass-B in GraphicsQueue
cmdList.DrawPrimitiveInstance(null, null, PrimitiveTopology.TriangleList, 0, 0);
deviceContext.ExecuteCmdList(EContextType.Graphics, cmdList);

//Pass-C in ComputeQueue and Wait Pass-A
deviceContext.WaitFence(EContextType.Compute, graphicsFence);
cmdList.DispatchCompute(null, 16, 16, 1);
deviceContext.ExecuteCmdList(EContextType.Compute, cmdList);
deviceContext.WritFence(EContextType.Compute, computeFence);

//Pass-D in ComputeQueue
cmdList.DispatchCompute(null, 16, 16, 1);
deviceContext.ExecuteCmdList(EContextType.Compute, cmdList);

//Pass-E in GraphicsQueue and Wait Pass-C
deviceContext.WaitFence(EContextType.Graphics, computeFence);
cmdList.DrawPrimitiveInstance(null, null, PrimitiveTopology.TriangleList, 128, 16);
deviceContext.ExecuteCmdList(EContextType.Graphics, cmdList);*/