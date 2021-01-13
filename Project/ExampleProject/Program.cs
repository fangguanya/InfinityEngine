using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using InfinityEngine.Core.TaskSystem;

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

    public class MyClass
    {
        public float A;

        public MyClass(float Value)
        {
            A = Value;
            Console.WriteLine("Spawn");
        }

        ~MyClass()
        {
            Console.WriteLine("Destroy");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            /*FApplication App = new FApplication("Example", 1280, 720);
            App.Init();
            App.Run();*/

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
            ChildTask.Schedule(TaskRefC).Wait();

            Console.WriteLine("ReadKey");*/

            // SerializeExample
            /*string path = @"d:\test.material";

            string WritContext = "123456789";
            System.IO.File.WriteAllText(path, WritContext);

            string ReadContext = System.IO.File.ReadAllText(path);
            Console.WriteLine(ReadContext);*/

            // GC Test
            MyClass Class = new MyClass(1);
            MyClass ClassRef = Class;
            ClassRef.A = 2;
            ClassRef = null;
            //Class = null;
            GC.Collect();

            Console.ReadKey();
            Console.ReadKey();
        }

        public static void Quit()
        {

        }
    }
}
