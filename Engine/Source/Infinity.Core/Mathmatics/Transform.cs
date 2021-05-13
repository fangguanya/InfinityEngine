using System;

namespace InfinityEngine.Core.Mathmatics
{
    [Serializable]
    public struct FTransform : IEquatable<FTransform>
    {
        public float3 scale;
        public float3 position;
        public quaternion rotation;

        public FTransform(in float3 position, in quaternion rotation, in float3 scale)
        {
            this.scale = scale;
            this.position = position;
            this.rotation = rotation;
        }

        public bool Equals(FTransform target)
        {
            return scale.Equals(target.scale) && position.Equals(target.position) && rotation.Equals(target.rotation);
        }

        public override bool Equals(object target)
        {
            return Equals((FTransform)target);
        }

        public override int GetHashCode()
        {
            return new int3(scale.GetHashCode(), position.GetHashCode(), rotation.GetHashCode()).GetHashCode();
        }
    }
}
