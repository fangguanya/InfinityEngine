﻿using InfinityEngine.Core.Object;

namespace InfinityEngine.Graphics.RHI
{
    public class FRHIShader : FDisposal
    {
        public string name;

        public FRHIShader() : base()
        {

        }
    }

    public class FRHIComputeShader : FRHIShader
    {
        public FRHIComputeShader() : base()
        {

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
    }
}
