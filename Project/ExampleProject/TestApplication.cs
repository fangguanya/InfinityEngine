using System;
using InfinityEngine.Core.Profiler;
using System.Runtime.InteropServices;
using InfinityEngine.Game.ActorSystem;
using InfinityEngine.Game.Application;

namespace ExampleProject
{
    [Serializable]
    public unsafe class TestComponent : UComponent
    {
        private Timer timer;
        private CPUTimer cpuTimer;

        private int* MyData;
        private int[] IntArray;

        public TestComponent()
        {

        }

        public override void OnEnable()
        {
            Console.WriteLine("Enable Component");

            timer = new Timer();
            cpuTimer = new CPUTimer();

            IntArray = new int[32768];
            MyData = (int*)Marshal.AllocHGlobal(sizeof(int) * 32768);

            //Console.WriteLine((0 >> 16) + (3 << 16 | 1));
            //Console.WriteLine((1 >> 16) + (3 << 16 | 0));
        }

        public override void OnUpdate()
        {
            //cpuTimer.Begin();
            //timer.Restart();
            //RunNative(4000, 32768);
            //RunUnsafe(4000, 32768);
            //RunManaged(4000, 32768);
            //timer.Stop();
            //cpuTimer.End();

            //Console.WriteLine(cpuTimer.GetMillisecond() + "ms");
            //Console.WriteLine(timer.ElapsedMilliseconds + "ms");
        }

        public override void OnDisable()
        {
            //Console.WriteLine("Disable Component");

            cpuTimer?.Dispose();
            Marshal.FreeHGlobal((IntPtr)MyData);
        }

        private void RunNative(in int Count, in int Length)
        {
            CPUTimer.DoTask(MyData, Count, Length);
        }

        private void RunUnsafe(in int Count, in int Length)
        {
            for (int i = 0; i < Count; i++)
            {
                for (int j = 0; j < Length; j++)
                {
                    ref int Value = ref MyData[j];
                    Value = i + j;
                }
            }
        }

        private void RunManaged(in int Count, in int Length)
        {
            for (int i = 0; i < Count; i++)
            {
                for (int j = 0; j < Length; j++)
                {
                    ref int Value = ref MyData[j];
                    Value = i + j;
                }
            }
        }
    }

    [Serializable]
    public class TestActor : AActor
    {
        private TestComponent Component;

        
        public TestActor() : base()
        {
            Component = new TestComponent();
            AddComponent(Component);
        }

        public TestActor(string InName) : base(InName)
        {
            Component = new TestComponent();
            AddComponent(Component);
        }

        public TestActor(string InName, AActor InParent) : base(InName, InParent)
        {
            Component = new TestComponent();
            AddComponent(Component);
        }

        public override void OnEnable()
        {
            base.OnEnable();
            //AddComponent(Component);
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            //Console.WriteLine("Update Actor");
        }

        public override void OnDisable()
        {
            base.OnDisable();
            Console.WriteLine("Disable Actor");
        }
    }

    public class TestApplication : FApplication
    {
        private TestActor Actor;

        
        public TestApplication(string Name, int Width, int Height) : base(Name, Width, Height)
        {
            Actor = new TestActor("TestActor");
        }

        protected override void Play()
        {
            Actor.OnEnable();
        }

        protected override void Tick()
        {
            Actor.OnUpdate();
        }

        protected override void End()
        {
            Actor.OnDisable();
        }
    }
}
