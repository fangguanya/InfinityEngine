﻿using System;
using InfinityEngine.Core.Profiler;
using System.Runtime.InteropServices;
using InfinityEngine.Game.ActorSystem;
using InfinityEngine.Game.Application;

namespace ExampleProject
{
    [Serializable]
    public unsafe class TestComponent : UComponent
    {
        int* MyData;
        int[] IntArray;
        CPUTimer cpuTimer;

        public TestComponent()
        {

        }

        public override void OnEnable()
        {
            Console.WriteLine("Enable Component");
            IntArray = new int[32768];
            cpuTimer = new CPUTimer();
            MyData = (int*)Marshal.AllocHGlobal(sizeof(int) * 32768);
        }

        public override void OnUpdate()
        {
            //cpuTimer.Begin();
            //ExecuteFunc(100, 32768);
            //cpuTimer.End();

            //Console.WriteLine(cpuTimer.GetMillisecond() + "ms");
            //Console.WriteLine(Timer.Elapsed.TotalMilliseconds + "ms");
        }

        public override void OnDisable()
        {
            //Console.WriteLine("Disable Component");

            cpuTimer?.Dispose();
            Marshal.FreeHGlobal((IntPtr)MyData);
        }

        void ExecuteFunc(in int Count, in int Length)
        {
            //CPUTimer.DoTask(MyData, Count, Length);
            for (int i = 0; i < Count; i++)
            {
                for (int j = 0; j < Length; j++)
                {
                    ref int Value = ref MyData[j];
                    Value = i + j;
                }
            }

            /*for (int i = 0; i < Count; i++)
            {
                for (int j = 0; j < Length; j++)
                {
                    ref int Value = ref IntArray[j];
                    Value = i + j;
                }
            }*/
        }
    }

    [Serializable]
    public class TestActor : AActor
    {
        TestComponent Component;

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
        }

        public override void OnDisable()
        {
            base.OnDisable();
            Console.WriteLine("Disable Actor");
        }
    }

    public class TestApplication : FApplication
    {
        TestActor Actor;

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
