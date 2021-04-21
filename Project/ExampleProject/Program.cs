using System;
using System.Threading;
using InfinityEngine.Core.Container;
using InfinityEngine.Core.TaskSystem;

namespace ExampleProject
{
    public struct FAsyncTask : ITaskASync
    {
        public void Execute()
        {
            Thread.Sleep(1000);
            Console.WriteLine("yes i can!");

            /*for (int i = 0; i < 10; i++)
            {
                TArray[i] = i;
                Console.WriteLine(TArray[i]);
            }*/
        }
    }

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
            Console.WriteLine(ReadContext);

            Console.ReadKey();*/


            //TArray Test
            /*TArray<float> MyArray = new TArray<float>(3);
            MyArray.Add(0.1f);
            MyArray.Add(0.2f);
            MyArray.Add(0.3f);

            MyArray.Add(0.4f);
            MyArray.Add(0.5f);
            MyArray.Add(0.6f);

            MyArray.RemoveAtIndex(4);
            MyArray.Add(0.5f);

            MyArray.AddUnique(0.6f);
            MyArray.AddUnique(0.7f);

            MyArray.Remove(0.6f);

            ref float value = ref MyArray[1];
            value += 0.5f;
            Console.ReadKey();*/


            /*string shaderSource = @"
            struct PSInput 
            {
                float4 position : SV_POSITION;
                float4 color : COLOR;
            };

            PSInput VSMain(float4 position : POSITION, float4 color : COLOR) 
            {
                PSInput result;
                result.position = position;
                result.color = color;
                return result;
            }

            float4 PSMain(PSInput input) : SV_TARGET 
            {
                return input.color;
            }";

            string ShaderSource = @"
		    HLSLPROGRAM
		    #pragma vertex vert
		    #pragma fragment frag

            struct Attributes
	        {
		        float2 uv : TEXCOORD0;
		        float4 vertex : POSITION;
	        };

	        struct Varyings
	        {
		        float2 uv : TEXCOORD0;
		        float4 vertex : SV_POSITION;
	        };

		    Varyings vert(Attributes In)
		    {
			    Varyings Out = (Varyings)0;

			    Out.uv = In.uv;
			    float4 WorldPos = mul(UNITY_MATRIX_M, float4(In.vertex.xyz, 1.0));
			    Out.vertex = mul(Matrix_ViewJitterProj, WorldPos);
			    return Out;
		    }

		    float4 frag(Varyings In) : SV_Target
		    {
			    return 0;
		    }
            ENDHLSL";

            int StartIndex = ShaderSource.IndexOf("HLSLPROGRAM") + 11;
            int EndIndex = ShaderSource.IndexOf("ENDHLSL");
            Console.WriteLine(ShaderSource.Substring(StartIndex, EndIndex - StartIndex));
            Console.ReadKey();*/
        }
    }
}
