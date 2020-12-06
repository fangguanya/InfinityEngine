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

    class Program
    {
        static void Main(string[] args)
        {
            //FApplication App = new FApplication("Example", 1280, 720);
            //App.Init();
            //App.Run();

            // TaskExample
            int[] IntArray = new int[10];

            FTestTaskA TaskA;
            TaskA.TArray = IntArray;
            Task TaskRefA = TaskA.Dispatch();

            FTestTaskB TaskB;
            TaskB.TArray = IntArray;
            Task TaskRefB = TaskB.Dispatch(TaskRefA);

            FTestTaskC TaskC;
            TaskC.TArray = IntArray;
            Task TaskRefC = TaskC.Dispatch(TaskRefA, TaskRefB);

            Console.WriteLine(TaskRefC.IsCompleted);
            TaskRefC.Wait();
            Console.WriteLine(TaskRefC.IsCompleted);

            Console.ReadKey();
        }

        public static void Quit()
        {

        }
    }
}
