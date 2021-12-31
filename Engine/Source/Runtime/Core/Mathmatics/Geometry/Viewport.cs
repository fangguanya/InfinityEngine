using System;

namespace InfinityEngine.Core.Mathmatics.Geometry
{
    public struct FViewport : IEquatable<FViewport>
    {
        public float TopLeftX;

        public float TopLeftY;

        public float Width;

        public float Height;

        public float MinDepth;

        public float MaxDepth;

        public static bool operator ==(in FViewport l, in FViewport r)
        {
            if (l.TopLeftX == r.TopLeftX && l.TopLeftY == r.TopLeftY && l.Width == r.Width && l.Height == r.Height && l.MinDepth == r.MinDepth)
            {
                return l.MaxDepth == r.MaxDepth;
            }

            return false;
        }

        public static bool operator !=(in FViewport l, in FViewport r)
        {
            return !(l == r);
        }

        public FViewport(float topLeftX, float topLeftY, float width, float height, float minDepth = 0f, float maxDepth = 1f)
        {
            TopLeftX = topLeftX;
            TopLeftY = topLeftY;
            Width = width;
            Height = height;
            MinDepth = minDepth;
            MaxDepth = maxDepth;
        }

        public override bool Equals(object? obj)
        {
            if (obj is FViewport)
            {
                FViewport other = (FViewport)obj;
                return Equals(other);
            }

            return false;
        }

        public bool Equals(FViewport other)
        {
            return this == other;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(TopLeftX, TopLeftY, Width, Height, MinDepth, MaxDepth);
        }
    }
}
