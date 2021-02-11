using System;
using System.Threading;
using InfinityEngine.Graphics.RHI;
using InfinityEngine.Core.TaskSystem;
using Vortice.Direct3D12;
using Vortice.DXGI;
using Vortice.Direct3D;

namespace ExampleProject
{
    public struct FTestTask : ITask
    {
        public string PrintData;
        public int SleepTime;
        public int[] TArray;

        public void Execute()
        {
            Thread.Sleep(SleepTime);
            Console.WriteLine(PrintData);

            /*for (int i = 0; i < 10; i++)
            {
                TArray[i] = i;
                Console.WriteLine(TArray[i]);
            }*/
        }
    }

    public struct FChildTask : ITask
    {
        public int[] TArray;

        public void Execute()
        {
            Console.WriteLine("FatherTask");
            /*for (int i = 0; i < 10; i++)
            {
                TArray[i] = TArray[i] + 5;
                Console.WriteLine(TArray[i]);
            }*/

            FTestTask ChildTask;
            ChildTask.PrintData = "ChildTask";
            ChildTask.SleepTime = 0;
            ChildTask.TArray = TArray;
            ChildTask.Run();
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            /*IDXGIFactory7 NativeFactory;
            IDXGIAdapter1 NativeAdapter;
            ID3D12Device6 NativeDevice;

            DXGI.CreateDXGIFactory1<IDXGIFactory7>(out NativeFactory);
            NativeFactory.EnumAdapters1(0, out NativeAdapter);

            D3D12.D3D12CreateDevice<ID3D12Device6>(NativeAdapter, FeatureLevel.Level_12_1, out NativeDevice);
            //NativeDevice.QueryInterface<ID3D12Device6>();

            NativeDevice.Release();
            //NativeDevice.Dispose();
            //NativeDevice = null;

            NativeAdapter.Release();
            //NativeAdapter.Dispose();
            //NativeAdapter = null;

            NativeFactory.Release();
            //NativeFactory.Dispose();
            //NativeFactory = null;

            NativeDevice.QueryInterface<ID3D12Device6>();
            Console.ReadKey();*/

            TestApplication App = new TestApplication("InfinityExample", 1280, 720);
            App.Run();

            // TaskExample
            /*int[] IntArray = new int[10];

            FTestTask TaskA;
            TaskA.SleepTime = 0;
            TaskA.TArray = IntArray;
            TaskA.PrintData = "TaskA";
            FTaskHandle TaskRefA = TaskA.Schedule();

            FTestTask TaskB;
            TaskB.SleepTime = 2000;
            TaskB.TArray = IntArray;
            TaskB.PrintData = "TaskB";
            FTaskHandle TaskRefB = TaskB.Schedule(TaskRefA);

            FTestTask TaskC;
            TaskC.SleepTime = 0;
            TaskC.TArray = IntArray;
            TaskC.PrintData = "TaskC";
            FTaskHandle TaskRefC = TaskC.Schedule(TaskRefA, TaskRefB);

            FChildTask ChildTask;
            ChildTask.TArray = IntArray;
            ChildTask.Schedule(TaskRefC).Wait();*/

            // SerializeExample
            /*string path = @"d:\test.material";

            string WritContext = "123456789";
            System.IO.File.WriteAllText(path, WritContext);

            string ReadContext = System.IO.File.ReadAllText(path);
            Console.WriteLine(ReadContext);*/

            //Console.ReadKey();
        }
    }
}
