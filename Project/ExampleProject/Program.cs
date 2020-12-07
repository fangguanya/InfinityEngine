using System;
using System.Threading;
using System.Threading.Tasks;
using InfinityEngine.Core.TaskSystem;

namespace ExampleProject
{
    public struct FTestTaskA : ITask
    {
        public int[] TArray;

        public void Execute()
        {
            Console.WriteLine("TaskA");
            for (int i = 0; i < 10; i++)
            {
                TArray[i] = i;
                Console.WriteLine(TArray[i]);
            }
        }
    }

    public struct FTestTaskB : ITask
    {
        public int[] TArray;

        public void Execute()
        {
            Thread.Sleep(2000);
            Console.WriteLine("TaskB");
            for (int i = 0; i < 10; i++)
            {
                TArray[i] = TArray[i] + 1;
                Console.WriteLine(TArray[i]);
            }
        }
    }

    public struct FTestTaskC : ITask
    {
        public int[] TArray;

        public void Execute()
        {
            Console.WriteLine("TaskC");
            for (int i = 0; i < 10; i++)
            {
                TArray[i] = TArray[i] + 2;
                Console.WriteLine(TArray[i]);
            }
        }
    }

    public struct FChildTask : ITask
    {
        public int[] TArray;

        public void Execute()
        {
            Console.WriteLine("ChildTask");
            for (int i = 0; i < 10; i++)
            {
                TArray[i] = TArray[i] + 5;
                Console.WriteLine(TArray[i]);
            }

            FTestTaskA TaskA;
            TaskA.TArray = TArray;
            TaskA.Run();
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

            FTestTaskA TaskA;
            TaskA.TArray = IntArray;
            Task TaskRefA = TaskA.Schedule();

            FTestTaskB TaskB;
            TaskB.TArray = IntArray;
            Task TaskRefB = TaskB.Schedule(TaskRefA);

            FTestTaskC TaskC;
            TaskC.TArray = IntArray;
            //TaskC.Run();
            Task TaskRefC = TaskC.Schedule(TaskRefA, TaskRefB);

            FChildTask ChildTask;
            ChildTask.TArray = IntArray;
            ChildTask.Schedule(TaskRefC).Wait();

            Console.WriteLine("ReadKey");*/

            // SerializeExample
            string path = @"d:\test.material";

            string WritContext = "123456789";
            System.IO.File.WriteAllText(path, WritContext);

            string ReadContext = System.IO.File.ReadAllText(path);
            Console.WriteLine(ReadContext);


            Console.ReadKey();
        }

        public static void Quit()
        {

        }
    }
}
