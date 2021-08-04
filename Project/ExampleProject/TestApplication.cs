using System;
using InfinityEngine.Game.System;
using InfinityEngine.Graphics.RHI;
using InfinityEngine.Core.Profiler;
using System.Runtime.InteropServices;
using InfinityEngine.Game.ActorSystem;
using InfinityEngine.Game.Application;
using InfinityEngine.Rendering.RenderLoop;

namespace ExampleProject
{
    [Serializable]
    public unsafe class TestComponent : UComponent
    {
        bool dataReady;
        int[] readData;
        FRHIFence fence;
        FRHIBuffer buffer;
        FRHICommandList cmdList;

        private int* m_UnsafeDatas;
        private int[] m_ManageDatas;
        private FTimeProfiler m_TimeProfiler;

        public override void OnEnable()
        {
            Console.WriteLine("Enable Component");
            m_TimeProfiler = new FTimeProfiler();

            dataReady = true;
            readData = new int[10000000];

            FGraphicsSystem.EnqueueTask(
            (FRenderContext renderContext, FRHIGraphicsContext graphicsContext) =>
            {
                fence = graphicsContext.CreateFence();
                buffer = graphicsContext.CreateBuffer(10000000, 4, EUseFlag.CPURW, EBufferType.Structured);
                cmdList = graphicsContext.CreateCmdList("CmdList", EContextType.Copy);

                cmdList.Clear();
                int[] data = new int[10000000];
                for (int i = 0; i < 10000000; ++i) { data[i] = 10000000 - i; }
                buffer.SetData<int>(cmdList, data);
                graphicsContext.ExecuteCmdList(EContextType.Copy, cmdList);
                graphicsContext.Submit();
            });

            m_ManageDatas = new int[32768];
            m_UnsafeDatas = (int*)Marshal.AllocHGlobal(sizeof(int) * 32768);
        }

        public override void OnUpdate()
        {
            FGraphicsSystem.EnqueueTask(
            (FRenderContext renderContext, FRHIGraphicsContext graphicsContext) =>
            {
                cmdList.Clear();
                m_TimeProfiler.Restart();

                if (dataReady)
                {
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

                m_TimeProfiler.Stop();
                graphicsContext.Submit();
                Console.WriteLine(m_TimeProfiler.milliseconds + "ms");
            });

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
            FGraphicsSystem.EnqueueTask(
            (FRenderContext renderContext, FRHIGraphicsContext graphicsContext) =>
            {
                fence?.Dispose();
                buffer?.Dispose();
                cmdList?.Dispose();
                Console.WriteLine("Release Proxy");
            });

            Console.WriteLine("Disable Component");
            Marshal.FreeHGlobal((IntPtr)m_UnsafeDatas);
        }

        private void RunNative(in int count, in int length)
        {
            CPUTimer.DoTask(m_UnsafeDatas, count, length);
        }

        private void RunUnsafe(in int count, in int length)
        {
            for (int i = 0; i < count; ++i)
            {
                for (int j = 0; j < length; ++j)
                {
                    ref int unsafeData = ref m_UnsafeDatas[j];
                    unsafeData = i + j;
                }
            }
        }

        private void RunManaged(in int count, in int length)
        {
            for (int i = 0; i < count; ++i)
            {
                for (int j = 0; j < length; ++j)
                {
                    ref int manageData = ref m_ManageDatas[j];
                    manageData = i + j;
                }
            }
        }
    }

    [Serializable]
    public class TestActor : AActor
    {
        //FTaskHandle m_AsynTaskRef;
        private TestComponent m_Component;

        
        public TestActor() : base()
        {
            m_Component = new TestComponent();
            //AddComponent(m_Component);
        }

        public TestActor(string InName) : base(InName)
        {
            m_Component = new TestComponent();
            //AddComponent(m_Component);
        }

        public TestActor(string InName, AActor InParent) : base(InName, InParent)
        {
            m_Component = new TestComponent();
            //AddComponent(m_Component);
        }

        public override void OnEnable()
        {
            base.OnEnable();
            AddComponent(m_Component);
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            //Console.WriteLine("Update Actor");

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
            Console.WriteLine("Disable Actor");
        }
    }

    public class TestApplication : FApplication
    {
        private TestActor m_Actor;

        public TestApplication(string Name, int Width, int Height) : base(Name, Width, Height)
        {
            m_Actor = new TestActor("TestActor");
        }

        protected override void Play()
        {
            m_Actor.OnEnable();
        }

        protected override void Tick()
        {
            m_Actor.OnUpdate();
        }

        protected override void End()
        {
            m_Actor.OnDisable();
        }
    }
}
