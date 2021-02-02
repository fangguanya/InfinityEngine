using System;
using InfinityEngine.Core.Engine;
using InfinityEngine.Core.EntitySystem;

namespace ExampleProject
{
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

        public override void OnUpdate(float frameTime)
        {
            //Console.WriteLine("Update Component");
        }
    }

    public class TestEntity : AEntity
    {
        TestComponent Component;

        public TestEntity()
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
        }

        public override void OnUpdate(float frameTime)
        {
            base.OnUpdate(frameTime);
        }
    }

    public class TestApplication : FApplication
    {
        TestEntity Entity;

        public TestApplication(string Name, int Width, int Height) : base(Name, Width, Height)
        {
            Entity = new TestEntity();
        }

        protected override void Init()
        {
            Entity.OnCreate();
            Entity.OnEnable();
        }

        protected override void Tick()
        {
            Entity.OnUpdate(0);
        }
    }
}
