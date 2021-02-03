using System;
using System.Threading.Tasks;
using InfinityEngine.Core.Engine;
using InfinityEngine.Core.EntitySystem;

namespace ExampleProject
{
    [Serializable]
    public class TestComponent : UComponent
    {
        public TestComponent()
        {

        }

        public override void OnCreate()
        {
            Console.WriteLine("Create Component");
        }

        public override void OnEnable()
        {
            Console.WriteLine("Enable Component");
        }

        public override void OnUpdate()
        {
            //Console.WriteLine("Update Component");
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
        }

        protected override void Tick()
        {
            Entity.OnUpdate();
        }
    }
}
