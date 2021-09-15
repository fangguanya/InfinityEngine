﻿using System;
using Vortice.Dxc;
using Vortice.Direct3D12.Shader;
using InfinityEngine.Core.Object;

namespace InfinityEngine.Graphics.RHI
{
    internal class FRHIShaderCompiler
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

    public class FRHIShader : FDisposable
    {
        public string name;

        public FRHIShader() : base()
        {

        }

        protected override void Release()
        {

        }
    }

    public class FRHIComputeShader : FRHIShader
    {
        public FRHIComputeShader() : base()
        {

        }

        protected override void Release()
        {
            base.Release();
        }
    }

    public enum EGraphicsShaderType
    {
        Vertex = 0,
        Hull = 1,
        Domain = 2,
        Geometry = 3,
        Pixel = 4
    }

    public class FRHIGraphicsShader : FRHIShader
    {
        public FRHIGraphicsShader() : base()
        {

        }

        protected override void Release()
        {
            base.Release();
        }
    }

    public enum ERayTraceShaderType
    {
        RayGen = 0,
        RayMiss = 1,
        RayHitGroup = 2
    }

    public class FRHIRayTraceShader : FRHIShader
    {
        //Intersection, AnyHit, ClosestHit, Miss, RayGeneration
        public FRHIRayTraceShader() : base()
        {

        }

        protected override void Release()
        {
            base.Release();
        }
    }
}
