using System;
using InfinityEngine.Game.System;
using InfinityEngine.Graphics.RHI;
using InfinityEngine.Core.Profiler;
using System.Runtime.CompilerServices;
using InfinityEngine.Game.Application;
using InfinityEngine.Game.ActorFramework;
using InfinityEngine.Rendering.RenderLoop;

namespace ExampleProject
{
    [Serializable]
    public unsafe class TestComponent : UComponent
    {
        bool dataReady;
        int[] readData;
        float cpuTime
        {
            get { return (float)timeProfiler.microseconds / 1000.0f; }
        }
        float gpuTime;

        FRHIFence fence;
        FRHIQuery query;
        FRHIBufferRef bufferRef;
        FTimeProfiler timeProfiler;
        //private int* m_UnsafeDatas;
        //private int[] m_ManageDatas;

        public override void OnEnable()
        {
            Console.WriteLine("Enable Component");
            timeProfiler = new FTimeProfiler();

            dataReady = true;
            readData = new int[10000000];
            FGraphics.AddTask(
            (FRenderContext renderContext, FRHIGraphicsContext graphicsContext) =>
            {
                FRHIBufferDescriptor descriptor = new FRHIBufferDescriptor(10000000, 4, EUsageType.Dynamic | EUsageType.Staging);

                fence = graphicsContext.GetFence();
                query = graphicsContext.GetQuery(EQueryType.CopyTimestamp);
                bufferRef = graphicsContext.GetBuffer(descriptor);
                FRHICommandBuffer cmdBuffer = graphicsContext.GetCommandBuffer(EContextType.Copy, "CmdBuffer1");
                cmdBuffer.Clear();

                int[] data = new int[10000000];
                for (int i = 0; i < 10000000; ++i) { data[i] = 10000000 - i; }
                bufferRef.buffer.SetData(cmdBuffer, data);
                graphicsContext.ExecuteCommandBuffer(cmdBuffer);
                graphicsContext.Submit();
            });

            //m_ManageDatas = new int[32768];
            //m_UnsafeDatas = (int*)Marshal.AllocHGlobal(sizeof(int) * 32768);
        }

        public override void OnUpdate(in float deltaTime)
        {
            FGraphics.AddTask(
            (FRenderContext renderContext, FRHIGraphicsContext graphicsContext) =>
            {
                timeProfiler.Restart();

                if (dataReady)
                {
                    FRHICommandBuffer cmdBuffer = graphicsContext.GetCommandBuffer(EContextType.Copy, "CmdBuffer2");
                    cmdBuffer.Clear();
                    cmdBuffer.BeginQuery(query);
                    bufferRef.buffer.RequestReadback<int>(cmdBuffer);
                    cmdBuffer.EndQuery(query);
                    graphicsContext.ExecuteCommandBuffer(cmdBuffer);
                    graphicsContext.WriteToFence(EContextType.Copy, fence);
                    //graphicsContext.WaitForFence(EContextType.Render, fence);
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
                Console.WriteLine("GPU  : " + gpuTime + "ms");
            });

            Console.WriteLine("||");
            Console.WriteLine("Game");
            //m_TimeProfiler.Restart();
            //RunNative(500, 32768);
            //RunUnsafe(500, 32768);
            //RunManaged(500, 32768);
            //m_TimeProfiler.Stop();

            //Console.WriteLine(cpuTimer.GetMillisecond() + "ms");
            //Console.WriteLine(m_TimeProfiler.milliseconds + "ms");
        }

        public override void OnDisable()
        {
            FGraphics.AddTask(
            (FRenderContext renderContext, FRHIGraphicsContext graphicsContext) =>
            {
                graphicsContext.ReleaseFence(fence);
                graphicsContext.ReleaseQuery(query);
                graphicsContext.ReleaseBuffer(bufferRef);
                Console.WriteLine("Release RenderProxy");
            });

            //m_ManageDatas = null;
            Console.WriteLine("Disable Component");
            //Marshal.FreeHGlobal((IntPtr)m_UnsafeDatas);
        }

        private void RunNative(in int count, in int length)
        {
            //CPUTimer.DoTask(m_UnsafeDatas, count, length);
        }

        //[MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void RunUnsafe(in int count, in int length)
        {
            /*for (int i = 0; i < count; ++i)
            {
                for (int j = 0; j < length; ++j)
                {
                    ref int unsafeData = ref m_UnsafeDatas[j];
                    unsafeData = i * j;
                }
            }*/
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void RunManaged(in int count, in int length)
        {
            /*for (int i = 0; i < count; ++i)
            {
                for (int j = 0; j < length; ++j)
                {
                    ref int manageData = ref m_ManageDatas[j];
                    manageData = i * j;
                }
            }*/
        }
    }

    [Serializable]
    public unsafe class TestActor : AActor
    {
        //FTaskHandle m_AsynTaskRef;
        private TestComponent m_Component;

        public TestActor() : base()
        {
            m_Component = new TestComponent();
            //AddComponent(m_Component);
        }

        public TestActor(string name) : base(name)
        {
            m_Component = new TestComponent();
            //AddComponent(m_Component);
        }

        public TestActor(string name, AActor parent) : base(name, parent)
        {
            m_Component = new TestComponent();
            //AddComponent(m_Component);
        }

        public override void OnEnable()
        {
            base.OnEnable();
            AddComponent(m_Component);
            Console.WriteLine("Enable Actor");
        }

        public override void OnUpdate(in float deltaTime)
        {
            base.OnUpdate(deltaTime);
            //Console.WriteLine("Update Actor");
            //Console.WriteLine(deltaTime);

            //Async Task
            /*Thread.Sleep(100);
            bool isReady = m_AsynTaskRef.Complete();
            if (isReady)
            {
                FAsyncTask asynTask;
                asynTask.Run(ref m_AsynTaskRef);
            }
            Console.WriteLine("Can you hear me?");*/
        }

        public override void OnDisable()
        {
            base.OnDisable();
            RemoveComponent(m_Component);
            Console.WriteLine("Disable Actor");
        }
    }

    public unsafe class TestApplication : FApplication
    {
        private TestActor m_Actor;

        public TestApplication(string name, int width, int height) : base(width, height, name)
        {
            m_Actor = new TestActor("TestActor");
        }

        protected override void Play()
        {
            m_Actor.OnEnable();
        }

        protected override void Tick()
        {
            m_Actor.OnUpdate(FGameTime.DeltaTime);
        }

        protected override void End()
        {
            m_Actor.OnDisable();
        }
    }
}
