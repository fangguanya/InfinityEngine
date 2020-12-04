using System;

namespace ExampleProject
{
    class Program
    {
        static Timer MyTimer;

        static void Init()
        {
            MyTimer = new Timer();
            MyTimer.Start();
        }

        static void Main(string[] args)
        {
            Init();

            while (true)
            {
                Console.WriteLine(MyTimer.ElapsedMilliseconds);
            }

            //Console.ReadKey();
        }

        public static void Quit()
        {
            MyTimer.Stop();
            Console.WriteLine("Destroy");
            Console.ReadKey();
        }
    }
}
