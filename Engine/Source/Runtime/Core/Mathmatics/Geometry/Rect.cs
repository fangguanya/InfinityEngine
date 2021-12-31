using System;

namespace InfinityEngine.Core.Mathmatics.Geometry
{
    public struct FRect : IEquatable<FRect>
    {
        public int left;

        public int top;

        public int right;

        public int bottom;

        public FRect(int Left, int Top, int Right, int Bottom)
        {
            left = Left;
            top = Top;
            right = Right;
            bottom = Bottom;
        }

        public static bool operator ==(in FRect l, in FRect r)
        {
            if (l.left == r.left && l.top == r.top && l.right == r.right)
            {
                return l.bottom == r.bottom;
            }

            return false;
        }

        public static bool operator !=(in FRect l, in FRect r)
        {
            return !(l == r);
        }

        public override bool Equals(object obj)
        {
            if (obj is FRect)
            {
                FRect other = (FRect)obj;
                return Equals(other);
            }

            return false;
        }

        public bool Equals(FRect other)
        {
            return this == other;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(left, top, right, bottom);
        }
    }
}
