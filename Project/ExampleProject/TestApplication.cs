using System;
using System.Diagnostics;
using System.Threading.Tasks;
using InfinityEngine.Core.Engine;
using System.Runtime.InteropServices;
using InfinityEngine.Core.EntitySystem;

namespace ExampleProject
{
    public unsafe class CPUTimer : IDisposable
    {
        [DllImport("CPUTimer")]
        public static extern IntPtr CreateCPUTimer();

        [DllImport("CPUTimer")]
        public static extern void BeginCPUTimer(IntPtr cpuTimer);

        [DllImport("CPUTimer")]
        public static extern void EndCPUTimer(IntPtr cpuTimer);

        [DllImport("CPUTimer")]
        public static extern long GetCPUTimer(IntPtr cpuTimer);

        [DllImport("CPUTimer")]
        public static extern void ReleaseCPUTimer(IntPtr cpuTimer);

        [DllImport("CPUTimer")]
        public static extern void DoTask(int* IntArray, int BaseCount, int SecondCount);


        //
        private IntPtr cpuTimer;

        public CPUTimer()
        {
            cpuTimer = CreateCPUTimer();
        }

        public void Begin()
        {
            BeginCPUTimer(cpuTimer);
        }

        public void End()
        {
            EndCPUTimer(cpuTimer);
        }

        public long GetMillisecond()
        {
            return GetCPUTimer(cpuTimer);
        }

        public void Dispose()
        {
            ReleaseCPUTimer(cpuTimer);
        }
    }

    [Serializable]
    public unsafe class TestComponent : UComponent
    {
        int[] IntArray;
        CPUTimer cpuTimer;

        public TestComponent()
        {

        }

        public override void OnCreate()
        {
            IntArray = new int[32768];
            cpuTimer = new CPUTimer();
            Console.WriteLine("Create Component");
        }

        public override void OnEnable()
        {
            Console.WriteLine("Enable Component");
        }

        public override void OnUpdate()
        {
            cpuTimer.Begin();
            //Stopwatch Timer = Stopwatch.StartNew();
            Managed();
            //Timer.Stop();
            cpuTimer.End();
            //Console.WriteLine(Timer.Elapsed.TotalMilliseconds + "ms");
            Console.WriteLine(cpuTimer.GetMillisecond() + "ms");
        }

        void Managed()
        {
            //int* MyData = (int*)Marshal.AllocHGlobal(sizeof(int) * 32768);
            //CPUTimer.DoTask(MyData, 1000, 32768);
            /*for (int i = 0; i < 1000; i++)
            {
                for (int j = 0; j < 32768; j++)
                {
                    ref int Value = ref MyData[j];
                    Value = i + j;
                }
            }*/
            //Marshal.FreeHGlobal((IntPtr)MyData);

            for (int i = 0; i < 1000; i++)
            {
                for (int j = 0; j < IntArray.Length; j++)
                {
                    ref int Value = ref IntArray[j];
                    Value = i + j;
                }
            }
        }
    }

    [Serializable]
    public class TestEntity : AEntity
    {
        TestComponent Component;

        public TestEntity() : base()
        {
            Component = new TestComponent();
            AddComponent(Component);
        }

        public TestEntity(string InName) : base(InName)
        {
            Component = new TestComponent();
            AddComponent(Component);
        }

        public TestEntity(string InName, AEntity InParent) : base(InName, InParent)
        {
            Component = new TestComponent();
            AddComponent(Component);
        }

        public override void OnCreate()
        {
            base.OnCreate();
            //AddComponent(Component);
        }

        public override void OnEnable()
        {
            base.OnEnable();
            //AddComponent(Component);
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            //AddComponent(Component);
        }

        protected override void DisposeManaged()
        {
            Console.WriteLine("DisposeManaged");
        }

        protected override void DisposeUnManaged()
        {
            Console.WriteLine("DisposeUnManaged");
        }
    }

    public class TestApplication : FApplication
    {
        TestEntity Entity;

        public TestApplication(string Name, int Width, int Height) : base(Name, Width, Height)
        {
            Entity = new TestEntity("TestEntity");
        }

        protected override void Init()
        {
            Entity.OnCreate();
            Entity.OnEnable();

            //Entity.Dispose();
        }

        protected override void Tick()
        {
            Entity.OnUpdate();
        }
    }
}
