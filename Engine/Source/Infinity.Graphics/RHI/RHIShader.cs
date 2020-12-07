using InfinityEngine.Core.Object;

namespace InfinityEngine.Graphics.RHI
{
    public class RHIShader : UObject
    {
        public string name;

        public RHIShader() : base()
        {

        }

        protected override void DisposeManaged()
        {

        }

        protected override void DisposeUnManaged()
        {

        }
    }

    public class RHIComputeShader : RHIShader
    {
        public RHIComputeShader() : base()
        {

        }

        protected override void DisposeManaged()
        {
            base.DisposeManaged();
        }

        protected override void DisposeUnManaged()
        {
            base.DisposeUnManaged();
        }
    }

    public class RHIRayGenShader : RHIShader
    {
        //Intersection, AnyHit, ClosestHit, Miss, RayGeneration
        public RHIRayGenShader() : base()
        {

        }

        protected override void DisposeManaged()
        {
            base.DisposeManaged();
        }

        protected override void DisposeUnManaged()
        {
            base.DisposeUnManaged();
        }
    }

    public class RHIGraphicsShader : RHIShader
    {
        public RHIGraphicsShader() : base()
        {

        }

        protected override void DisposeManaged()
        {
            base.DisposeManaged();
        }

        protected override void DisposeUnManaged()
        {
            base.DisposeUnManaged();
        }
    }
}
