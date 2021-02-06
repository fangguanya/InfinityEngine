using System;
using System.Diagnostics;
using System.Threading.Tasks;
using InfinityEngine.Core.Engine;
using InfinityEngine.Core.EntitySystem;

namespace ExampleProject
{
    [Serializable]
    public class TestComponent : UComponent
    {
        int[] IntArray;

        public TestComponent()
        {

        }

        public override void OnCreate()
        {
            IntArray = new int[32768];
            Console.WriteLine("Create Component");
        }

        public override void OnEnable()
        {
            Console.WriteLine("Enable Component");
        }

        public override void OnUpdate()
        {
            Stopwatch Timer = Stopwatch.StartNew();
            Managed();
            Timer.Stop();
            Console.WriteLine(Timer.Elapsed.TotalMilliseconds + "ms");
        }

        void Managed()
        {
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
