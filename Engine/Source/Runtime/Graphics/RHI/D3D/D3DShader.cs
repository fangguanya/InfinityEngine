using System;
using Vortice.Dxc;
using Vortice.Direct3D12.Shader;

namespace InfinityEngine.Graphics.RHI
{
    internal class FD3DShaderCompiler
    {
        private static Span<byte> CompileBytecode(DxcShaderStage stage, string shaderSource, string entryPoint)
        {
            IDxcResult results = DxcCompiler.Compile(stage, shaderSource, entryPoint, null);
            return results.GetObjectBytecode();
        }

        private static Span<byte> CompileBytecodeWithReflection(DxcShaderStage stage, string shaderSource, string entryPoint, out ID3D12ShaderReflection reflection)
        {
            IDxcResult results = DxcCompiler.Compile(stage, shaderSource, entryPoint, null, null, null, null);
            using (IDxcBlob reflectionData = results.GetOutput(DxcOutKind.Reflection))
            {
                reflection = DxcCompiler.Utils.CreateReflection<ID3D12ShaderReflection>(reflectionData);
            }

            return results.GetObjectBytecode();
        }
    }

    public class FD3DShader : FRHIShader
    {
        public FD3DShader() : base()
        {

        }

        protected override void Release()
        {

        }
    }

    public class FD3DComputeShader : FRHIComputeShader
    {
        public FD3DComputeShader() : base()
        {

        }

        protected override void Release()
        {
            base.Release();
        }
    }

    public class FD3DGraphicsShader : FRHIGraphicsShader
    {
        public FD3DGraphicsShader() : base()
        {

        }

        protected override void Release()
        {
            base.Release();
        }
    }

    public class FD3DRayTraceShader : FRHIRayTraceShader
    {
        //Intersection, AnyHit, ClosestHit, Miss, RayGeneration
        public FD3DRayTraceShader() : base()
        {

        }

        protected override void Release()
        {
            base.Release();
        }
    }
}
