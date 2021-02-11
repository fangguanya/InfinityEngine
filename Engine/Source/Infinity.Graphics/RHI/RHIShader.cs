using InfinityEngine.Core.Object;

namespace InfinityEngine.Graphics.RHI
{
    public class FRHIShader : UObject
    {
        public string name;

        public FRHIShader() : base()
        {

        }

        protected override void Disposed()
        {

        }
    }

    public class FRHIComputeShader : FRHIShader
    {
        public FRHIComputeShader() : base()
        {

        }

        protected override void Disposed()
        {
            base.Disposed();
        }
    }

    public class FRHIRayGenShader : FRHIShader
    {
        //Intersection, AnyHit, ClosestHit, Miss, RayGeneration
        public FRHIRayGenShader() : base()
        {

        }

        protected override void Disposed()
        {
            base.Disposed();
        }
    }

    public class FRHIGraphicsShader : FRHIShader
    {
        public FRHIGraphicsShader() : base()
        {

        }

        protected override void Disposed()
        {
            base.Disposed();
        }
    }
}
