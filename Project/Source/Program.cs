using System;
using System.Threading;
using InfinityEngine.Core.Container;
using System.Runtime.InteropServices;
using InfinityEngine.Core.Thread.TaskSystem;

namespace ExampleProject
{
    public struct FAsyncTask : ITaskAsync
    {
        public void Execute()
        {
            Thread.Sleep(1000);
            Console.WriteLine("yes i can!");

            /*for (int i = 0; i < 10; ++i)
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

            /*for (int i = 0; i < 10; ++i)
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
            /*for (int i = 0; i < 10; ++i)
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

    unsafe class Program
    {
        static void Main(string[] args)
        {
            TestApplication App = new TestApplication("InfinityExample", 1280, 720);
            App.Run();

            //packing data test
            /*float testFloatBA = 0.25f;
            float testFloatBB = 0.35f;
            int encodeDataA = ((int)(testFloatBA * 65535) << 16) + (int)(testFloatBB * 65535);
            float decodeFloatBA = (encodeDataA >> 16 & 65535) / 65535.0f;
            float decodeFloatBB = (encodeDataA >> 32 & 65535) / 65535.0f;

            float testFloatA = 0.25f;
            float testFloatB = 0.35f;
            float testFloatC = 0.55f;
            float testFloatD = 0.65f;
            int encodeDataB = ((int)(testFloatA * 255) << 24) + ((int)(testFloatB * 255) << 16) + ((int)(testFloatC * 255) << 8) + (int)(testFloatD * 255);
            float decodeFloatA = (encodeDataB >> 24 & 255) / 255.0f;
            float decodeFloatB = (encodeDataB >> 16 & 255) / 255.0f;
            float decodeFloatC = (encodeDataB >> 8 & 255) / 255.0f;
            float decodeFloatD = (encodeDataB >> 32 & 255) / 255.0f;

            Console.ReadKey();

            // Bitmask Test
            int bitMask = 0;

            int[] maskValue = new int[8] { 0, 1, 1, 0, 0, 0, 1, 1 };
            for (int i = 0; i < 8; ++i)
            {
                bitMask += maskValue[i] << i;
            }

            int[] outValue = new int[8];
            for (int i = 0; i < 8; ++i)
            {
                outValue[i] = bitMask >> i & 1;
            }
            Console.ReadKey();

            // PtrTest
            int aValue = 100;
            int** aArray = (int**)FMemoryUtil.Malloc(8, 2);
            for (int i = 0; i < 2; i++)
            {
                aArray[i] = &aValue;
            }
            Console.WriteLine(aValue);
            Console.WriteLine(*aArray[0]);
            Console.WriteLine(*aArray[1]);
            *aArray[0] = 200;
            Console.WriteLine(aValue);
            Console.WriteLine(*aArray[0]);
            Console.WriteLine(*aArray[1]);

            FMemoryUtil.Free(aArray);
            Console.ReadKey();

            // TaskExample
            int[] IntArray = new int[10];

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

            ref float value = ref MyArray[5];
            value += 0.5f;

            MyArray.RemoveAtIndex(5);
            Console.ReadKey();*/

            //TSparseArray Test
            /*TSparseArray<float> MyArray = new TSparseArray<float>(3);
            MyArray.Add(0.1f);
            MyArray.Add(0.2f);
            MyArray.Add(0.3f);
            MyArray.Add(0.4f);
            MyArray.Add(0.5f);
            MyArray.Add(0.6f);

            MyArray.Remove(3);
            MyArray.Add(0.5f);

            ref float value = ref MyArray[5];
            value += 0.5f;

            MyArray.Remove(5);
            Console.ReadKey();

            string shaderSource = @"
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

            /*string hlslCode1 = new string(
            @"struct Attributes
	        {
		        float2 uv : TEXCOORD0;
		        float4 vertex : POSITION;
	        };

	        struct Varyings
	        {
		        float2 uv : TEXCOORD0;
		        float4 vertex : SV_POSITION;
	        };
            
            float4x4 Matrix_Model, Matrix_ViewJitterProj;
		    Varyings VS(Attributes In)
		    {
			    Varyings Out = (Varyings)0;

			    Out.uv = In.uv;
			    float4 WorldPos = mul(Matrix_Model, float4(In.vertex.xyz, 1.0));
			    Out.vertex = mul(Matrix_ViewJitterProj, WorldPos);
			    return Out;
		    }

		    float4 PS(Varyings In) : SV_Target
		    {
			    return 0;
		    }");

            string hlslCode2 = new string(
            @"cbuffer ParamsBuffer : register(b0)
            {
	            float2 Tiling;
            }

            Texture2D<float4> _DiffuseTexture : register(t0);
            SamplerState Sampler_DiffuseTexture : register(s0);

            struct Varyings
            {
	            float2 uv : TEXCOORD;
	            float4 pos : SV_POSITION;
            };

            Varyings VS(uint id: SV_VertexID)
            {
	            Varyings output = (Varyings)0;
	            output.uv = float2((id << 1) & 2, id & 2);
	            output.pos = float4(output.uv * float2(2, -2) + float2(-1, 1), 0, 1);
	            return output;
            }

            float4 PS(Varyings input) : SV_Target
            {
	            return _DiffuseTexture.Sample(Sampler_DiffuseTexture, input.uv * Tiling);
            }");

            Vortice.Vulkan.VkShaderModuleCreateInfo programInfo;
            byte[] shaderByte = WaveEngine.HLSLEverywhere.HLSLTranslator.HLSLToBinarySPIRV(hlslCode2, WaveEngine.Common.Graphics.ShaderStages.Pixel, WaveEngine.Common.Graphics.GraphicsProfile.Level_12_1, "PS");
            uint shaderUInt = BitConverter.ToUInt32(shaderByte, 0);

            string shaderCode = WaveEngine.HLSLEverywhere.HLSLTranslator.HLSLTo(hlslCode2, WaveEngine.Common.Graphics.ShaderStages.Pixel, WaveEngine.Common.Graphics.GraphicsProfile.Level_12_1, "PS", WaveEngine.HLSLEverywhere.ShadingLanguage.Msl_iOS, 460);
            Vortice.Dxc.IDxcResult result = Vortice.Dxc.DxcCompiler.Compile(hlslCode2, new string[] { "PS" });
            Console.ReadKey();*/
        }
    }
}
