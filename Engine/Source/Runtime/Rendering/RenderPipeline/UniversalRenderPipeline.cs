using System;
using InfinityEngine.Graphics.RHI;
using InfinityEngine.Core.Profiler;
using InfinityEngine.Rendering.RenderLoop;

namespace InfinityEngine.Rendering.RenderPipeline
{
    public class FUniversalRenderPipeline : FRenderPipeline
    {
        /*int numData = 100000;
        bool dataReady;
        int[] readData;
        float cpuTime
        {
            get { return (float)timeProfiler.microseconds / 1000.0f; }
        }
        float gpuTime;

        FRHIFence fence;
        FRHIQuery query;
        FRHIBuffer buffer
        {
            get { return bufferRef.buffer; }
        }
        FRHIBufferRef bufferRef;
        FTimeProfiler timeProfiler;*/

        public FUniversalRenderPipeline(string pipelineName) : base(pipelineName) 
        {
            /*dataReady = true;
            readData = new int[numData];
            timeProfiler = new FTimeProfiler();*/
        }

        public override void Init(FRenderContext renderContext)
        {
            Console.WriteLine("Init RenderPipeline");

            /*int[] data = new int[numData];
            for (int i = 0; i < numData; ++i)
            {
                data[i] = numData - i;
            }

            FBufferDescriptor descriptor = new FBufferDescriptor((ulong)numData, 4, EUsageType.Dynamic | EUsageType.Staging);
            descriptor.name = "TestBuffer";
            fence = renderContext.GetFence("Readback");
            query = renderContext.GetQuery(EQueryType.CopyTimestamp, "Readback");
            bufferRef = renderContext.GetBuffer(descriptor);
            FRHICommandBuffer cmdBuffer = renderContext.GetCommandBuffer(EContextType.Copy, "Upload");

            cmdBuffer.Clear();
            cmdBuffer.BeginEvent("Upload");
            buffer.SetData(cmdBuffer, data);
            cmdBuffer.EndEvent();
            renderContext.ExecuteCommandBuffer(cmdBuffer);*/
        }

        public override void Render(FRenderContext renderContext)
        {
            /*timeProfiler.Start();

            if (dataReady)
            {
                FRHICommandBuffer cmdBuffer = renderContext.GetCommandBuffer(EContextType.Copy, "Readback");
                cmdBuffer.Clear();
                cmdBuffer.BeginEvent("Readback");
                cmdBuffer.BeginQuery(query);
                buffer.Readback<int>(cmdBuffer);
                cmdBuffer.EndQuery(query);
                cmdBuffer.EndEvent();
                renderContext.ExecuteCommandBuffer(cmdBuffer);
                renderContext.WriteToFence(EContextType.Copy, fence);
                //deviceContext.WaitForFence(EContextType.Render, fence);
            }

            if (dataReady = fence.IsCompleted)
            {
                buffer.GetData(readData);
                gpuTime = query.GetResult(renderContext.copyFrequency);
            }

            timeProfiler.Stop();

            Console.WriteLine("||");
            Console.WriteLine("CPU : " + cpuTime + "ms");
            Console.WriteLine("GPU : " + gpuTime + "ms");*/
        }

        public override void Release(FRenderContext renderContext)
        {
            /*renderContext.ReleaseFence(fence);
            renderContext.ReleaseQuery(query);
            renderContext.ReleaseBuffer(bufferRef);*/
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