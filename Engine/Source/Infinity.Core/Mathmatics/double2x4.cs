//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using System;
using System.Runtime.CompilerServices;

#pragma warning disable 0660, 0661

namespace InfinityEngine.Core.Mathmatics
{
    [System.Serializable]
    public partial struct double2x4 : System.IEquatable<double2x4>, IFormattable
    {
        public double2 c0;
        public double2 c1;
        public double2 c2;
        public double2 c3;

        /// <summary>double2x4 zero value.</summary>
        public static readonly double2x4 zero;

        /// <summary>Constructs a double2x4 matrix from four double2 vectors.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public double2x4(double2 c0, double2 c1, double2 c2, double2 c3)
        {
            this.c0 = c0;
            this.c1 = c1;
            this.c2 = c2;
            this.c3 = c3;
        }

        /// <summary>Constructs a double2x4 matrix from 8 double values given in row-major order.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public double2x4(double m00, double m01, double m02, double m03,
                         double m10, double m11, double m12, double m13)
        {
            this.c0 = new double2(m00, m10);
            this.c1 = new double2(m01, m11);
            this.c2 = new double2(m02, m12);
            this.c3 = new double2(m03, m13);
        }

        /// <summary>Constructs a double2x4 matrix from a single double value by assigning it to every component.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public double2x4(double v)
        {
            this.c0 = v;
            this.c1 = v;
            this.c2 = v;
            this.c3 = v;
        }

        /// <summary>Constructs a double2x4 matrix from a single bool value by converting it to double and assigning it to every component.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public double2x4(bool v)
        {
            this.c0 = math.select(new double2(0.0), new double2(1.0), v);
            this.c1 = math.select(new double2(0.0), new double2(1.0), v);
            this.c2 = math.select(new double2(0.0), new double2(1.0), v);
            this.c3 = math.select(new double2(0.0), new double2(1.0), v);
        }

        /// <summary>Constructs a double2x4 matrix from a bool2x4 matrix by componentwise conversion.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public double2x4(bool2x4 v)
        {
            this.c0 = math.select(new double2(0.0), new double2(1.0), v.c0);
            this.c1 = math.select(new double2(0.0), new double2(1.0), v.c1);
            this.c2 = math.select(new double2(0.0), new double2(1.0), v.c2);
            this.c3 = math.select(new double2(0.0), new double2(1.0), v.c3);
        }

        /// <summary>Constructs a double2x4 matrix from a single int value by converting it to double and assigning it to every component.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public double2x4(int v)
        {
            this.c0 = v;
            this.c1 = v;
            this.c2 = v;
            this.c3 = v;
        }

        /// <summary>Constructs a double2x4 matrix from a int2x4 matrix by componentwise conversion.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public double2x4(int2x4 v)
        {
            this.c0 = v.c0;
            this.c1 = v.c1;
            this.c2 = v.c2;
            this.c3 = v.c3;
        }

        /// <summary>Constructs a double2x4 matrix from a single uint value by converting it to double and assigning it to every component.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public double2x4(uint v)
        {
            this.c0 = v;
            this.c1 = v;
            this.c2 = v;
            this.c3 = v;
        }

        /// <summary>Constructs a double2x4 matrix from a uint2x4 matrix by componentwise conversion.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public double2x4(uint2x4 v)
        {
            this.c0 = v.c0;
            this.c1 = v.c1;
            this.c2 = v.c2;
            this.c3 = v.c3;
        }

        /// <summary>Constructs a double2x4 matrix from a single float value by converting it to double and assigning it to every component.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public double2x4(float v)
        {
            this.c0 = v;
            this.c1 = v;
            this.c2 = v;
            this.c3 = v;
        }

        /// <summary>Constructs a double2x4 matrix from a float2x4 matrix by componentwise conversion.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public double2x4(float2x4 v)
        {
            this.c0 = v.c0;
            this.c1 = v.c1;
            this.c2 = v.c2;
            this.c3 = v.c3;
        }


        /// <summary>Implicitly converts a single double value to a double2x4 matrix by assigning it to every component.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator double2x4(double v) { return new double2x4(v); }

        /// <summary>Explicitly converts a single bool value to a double2x4 matrix by converting it to double and assigning it to every component.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator double2x4(bool v) { return new double2x4(v); }

        /// <summary>Explicitly converts a bool2x4 matrix to a double2x4 matrix by componentwise conversion.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator double2x4(bool2x4 v) { return new double2x4(v); }

        /// <summary>Implicitly converts a single int value to a double2x4 matrix by converting it to double and assigning it to every component.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator double2x4(int v) { return new double2x4(v); }

        /// <summary>Implicitly converts a int2x4 matrix to a double2x4 matrix by componentwise conversion.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator double2x4(int2x4 v) { return new double2x4(v); }

        /// <summary>Implicitly converts a single uint value to a double2x4 matrix by converting it to double and assigning it to every component.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator double2x4(uint v) { return new double2x4(v); }

        /// <summary>Implicitly converts a uint2x4 matrix to a double2x4 matrix by componentwise conversion.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator double2x4(uint2x4 v) { return new double2x4(v); }

        /// <summary>Implicitly converts a single float value to a double2x4 matrix by converting it to double and assigning it to every component.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator double2x4(float v) { return new double2x4(v); }

        /// <summary>Implicitly converts a float2x4 matrix to a double2x4 matrix by componentwise conversion.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator double2x4(float2x4 v) { return new double2x4(v); }


        /// <summary>Returns the result of a componentwise multiplication operation on two double2x4 matrices.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double2x4 operator * (double2x4 lhs, double2x4 rhs) { return new double2x4 (lhs.c0 * rhs.c0, lhs.c1 * rhs.c1, lhs.c2 * rhs.c2, lhs.c3 * rhs.c3); }

        /// <summary>Returns the result of a componentwise multiplication operation on a double2x4 matrix and a double value.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double2x4 operator * (double2x4 lhs, double rhs) { return new double2x4 (lhs.c0 * rhs, lhs.c1 * rhs, lhs.c2 * rhs, lhs.c3 * rhs); }

        /// <summary>Returns the result of a componentwise multiplication operation on a double value and a double2x4 matrix.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double2x4 operator * (double lhs, double2x4 rhs) { return new double2x4 (lhs * rhs.c0, lhs * rhs.c1, lhs * rhs.c2, lhs * rhs.c3); }


        /// <summary>Returns the result of a componentwise addition operation on two double2x4 matrices.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double2x4 operator + (double2x4 lhs, double2x4 rhs) { return new double2x4 (lhs.c0 + rhs.c0, lhs.c1 + rhs.c1, lhs.c2 + rhs.c2, lhs.c3 + rhs.c3); }

        /// <summary>Returns the result of a componentwise addition operation on a double2x4 matrix and a double value.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double2x4 operator + (double2x4 lhs, double rhs) { return new double2x4 (lhs.c0 + rhs, lhs.c1 + rhs, lhs.c2 + rhs, lhs.c3 + rhs); }

        /// <summary>Returns the result of a componentwise addition operation on a double value and a double2x4 matrix.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double2x4 operator + (double lhs, double2x4 rhs) { return new double2x4 (lhs + rhs.c0, lhs + rhs.c1, lhs + rhs.c2, lhs + rhs.c3); }


        /// <summary>Returns the result of a componentwise subtraction operation on two double2x4 matrices.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double2x4 operator - (double2x4 lhs, double2x4 rhs) { return new double2x4 (lhs.c0 - rhs.c0, lhs.c1 - rhs.c1, lhs.c2 - rhs.c2, lhs.c3 - rhs.c3); }

        /// <summary>Returns the result of a componentwise subtraction operation on a double2x4 matrix and a double value.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double2x4 operator - (double2x4 lhs, double rhs) { return new double2x4 (lhs.c0 - rhs, lhs.c1 - rhs, lhs.c2 - rhs, lhs.c3 - rhs); }

        /// <summary>Returns the result of a componentwise subtraction operation on a double value and a double2x4 matrix.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double2x4 operator - (double lhs, double2x4 rhs) { return new double2x4 (lhs - rhs.c0, lhs - rhs.c1, lhs - rhs.c2, lhs - rhs.c3); }


        /// <summary>Returns the result of a componentwise division operation on two double2x4 matrices.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double2x4 operator / (double2x4 lhs, double2x4 rhs) { return new double2x4 (lhs.c0 / rhs.c0, lhs.c1 / rhs.c1, lhs.c2 / rhs.c2, lhs.c3 / rhs.c3); }

        /// <summary>Returns the result of a componentwise division operation on a double2x4 matrix and a double value.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double2x4 operator / (double2x4 lhs, double rhs) { return new double2x4 (lhs.c0 / rhs, lhs.c1 / rhs, lhs.c2 / rhs, lhs.c3 / rhs); }

        /// <summary>Returns the result of a componentwise division operation on a double value and a double2x4 matrix.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double2x4 operator / (double lhs, double2x4 rhs) { return new double2x4 (lhs / rhs.c0, lhs / rhs.c1, lhs / rhs.c2, lhs / rhs.c3); }


        /// <summary>Returns the result of a componentwise modulus operation on two double2x4 matrices.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double2x4 operator % (double2x4 lhs, double2x4 rhs) { return new double2x4 (lhs.c0 % rhs.c0, lhs.c1 % rhs.c1, lhs.c2 % rhs.c2, lhs.c3 % rhs.c3); }

        /// <summary>Returns the result of a componentwise modulus operation on a double2x4 matrix and a double value.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double2x4 operator % (double2x4 lhs, double rhs) { return new double2x4 (lhs.c0 % rhs, lhs.c1 % rhs, lhs.c2 % rhs, lhs.c3 % rhs); }

        /// <summary>Returns the result of a componentwise modulus operation on a double value and a double2x4 matrix.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double2x4 operator % (double lhs, double2x4 rhs) { return new double2x4 (lhs % rhs.c0, lhs % rhs.c1, lhs % rhs.c2, lhs % rhs.c3); }


        /// <summary>Returns the result of a componentwise increment operation on a double2x4 matrix.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double2x4 operator ++ (double2x4 val) { return new double2x4 (++val.c0, ++val.c1, ++val.c2, ++val.c3); }


        /// <summary>Returns the result of a componentwise decrement operation on a double2x4 matrix.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double2x4 operator -- (double2x4 val) { return new double2x4 (--val.c0, --val.c1, --val.c2, --val.c3); }


        /// <summary>Returns the result of a componentwise less than operation on two double2x4 matrices.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool2x4 operator < (double2x4 lhs, double2x4 rhs) { return new bool2x4 (lhs.c0 < rhs.c0, lhs.c1 < rhs.c1, lhs.c2 < rhs.c2, lhs.c3 < rhs.c3); }

        /// <summary>Returns the result of a componentwise less than operation on a double2x4 matrix and a double value.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool2x4 operator < (double2x4 lhs, double rhs) { return new bool2x4 (lhs.c0 < rhs, lhs.c1 < rhs, lhs.c2 < rhs, lhs.c3 < rhs); }

        /// <summary>Returns the result of a componentwise less than operation on a double value and a double2x4 matrix.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool2x4 operator < (double lhs, double2x4 rhs) { return new bool2x4 (lhs < rhs.c0, lhs < rhs.c1, lhs < rhs.c2, lhs < rhs.c3); }


        /// <summary>Returns the result of a componentwise less or equal operation on two double2x4 matrices.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool2x4 operator <= (double2x4 lhs, double2x4 rhs) { return new bool2x4 (lhs.c0 <= rhs.c0, lhs.c1 <= rhs.c1, lhs.c2 <= rhs.c2, lhs.c3 <= rhs.c3); }

        /// <summary>Returns the result of a componentwise less or equal operation on a double2x4 matrix and a double value.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool2x4 operator <= (double2x4 lhs, double rhs) { return new bool2x4 (lhs.c0 <= rhs, lhs.c1 <= rhs, lhs.c2 <= rhs, lhs.c3 <= rhs); }

        /// <summary>Returns the result of a componentwise less or equal operation on a double value and a double2x4 matrix.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool2x4 operator <= (double lhs, double2x4 rhs) { return new bool2x4 (lhs <= rhs.c0, lhs <= rhs.c1, lhs <= rhs.c2, lhs <= rhs.c3); }


        /// <summary>Returns the result of a componentwise greater than operation on two double2x4 matrices.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool2x4 operator > (double2x4 lhs, double2x4 rhs) { return new bool2x4 (lhs.c0 > rhs.c0, lhs.c1 > rhs.c1, lhs.c2 > rhs.c2, lhs.c3 > rhs.c3); }

        /// <summary>Returns the result of a componentwise greater than operation on a double2x4 matrix and a double value.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool2x4 operator > (double2x4 lhs, double rhs) { return new bool2x4 (lhs.c0 > rhs, lhs.c1 > rhs, lhs.c2 > rhs, lhs.c3 > rhs); }

        /// <summary>Returns the result of a componentwise greater than operation on a double value and a double2x4 matrix.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool2x4 operator > (double lhs, double2x4 rhs) { return new bool2x4 (lhs > rhs.c0, lhs > rhs.c1, lhs > rhs.c2, lhs > rhs.c3); }


        /// <summary>Returns the result of a componentwise greater or equal operation on two double2x4 matrices.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool2x4 operator >= (double2x4 lhs, double2x4 rhs) { return new bool2x4 (lhs.c0 >= rhs.c0, lhs.c1 >= rhs.c1, lhs.c2 >= rhs.c2, lhs.c3 >= rhs.c3); }

        /// <summary>Returns the result of a componentwise greater or equal operation on a double2x4 matrix and a double value.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool2x4 operator >= (double2x4 lhs, double rhs) { return new bool2x4 (lhs.c0 >= rhs, lhs.c1 >= rhs, lhs.c2 >= rhs, lhs.c3 >= rhs); }

        /// <summary>Returns the result of a componentwise greater or equal operation on a double value and a double2x4 matrix.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool2x4 operator >= (double lhs, double2x4 rhs) { return new bool2x4 (lhs >= rhs.c0, lhs >= rhs.c1, lhs >= rhs.c2, lhs >= rhs.c3); }


        /// <summary>Returns the result of a componentwise unary minus operation on a double2x4 matrix.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double2x4 operator - (double2x4 val) { return new double2x4 (-val.c0, -val.c1, -val.c2, -val.c3); }


        /// <summary>Returns the result of a componentwise unary plus operation on a double2x4 matrix.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double2x4 operator + (double2x4 val) { return new double2x4 (+val.c0, +val.c1, +val.c2, +val.c3); }


        /// <summary>Returns the result of a componentwise equality operation on two double2x4 matrices.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool2x4 operator == (double2x4 lhs, double2x4 rhs) { return new bool2x4 (lhs.c0 == rhs.c0, lhs.c1 == rhs.c1, lhs.c2 == rhs.c2, lhs.c3 == rhs.c3); }

        /// <summary>Returns the result of a componentwise equality operation on a double2x4 matrix and a double value.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool2x4 operator == (double2x4 lhs, double rhs) { return new bool2x4 (lhs.c0 == rhs, lhs.c1 == rhs, lhs.c2 == rhs, lhs.c3 == rhs); }

        /// <summary>Returns the result of a componentwise equality operation on a double value and a double2x4 matrix.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool2x4 operator == (double lhs, double2x4 rhs) { return new bool2x4 (lhs == rhs.c0, lhs == rhs.c1, lhs == rhs.c2, lhs == rhs.c3); }


        /// <summary>Returns the result of a componentwise not equal operation on two double2x4 matrices.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool2x4 operator != (double2x4 lhs, double2x4 rhs) { return new bool2x4 (lhs.c0 != rhs.c0, lhs.c1 != rhs.c1, lhs.c2 != rhs.c2, lhs.c3 != rhs.c3); }

        /// <summary>Returns the result of a componentwise not equal operation on a double2x4 matrix and a double value.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool2x4 operator != (double2x4 lhs, double rhs) { return new bool2x4 (lhs.c0 != rhs, lhs.c1 != rhs, lhs.c2 != rhs, lhs.c3 != rhs); }

        /// <summary>Returns the result of a componentwise not equal operation on a double value and a double2x4 matrix.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool2x4 operator != (double lhs, double2x4 rhs) { return new bool2x4 (lhs != rhs.c0, lhs != rhs.c1, lhs != rhs.c2, lhs != rhs.c3); }



        /// <summary>Returns the double2 element at a specified index.</summary>
        unsafe public ref double2 this[int index]
        {
            get
            {
#if ENABLE_UNITY_COLLECTIONS_CHECKS
                if ((uint)index >= 4)
                    throw new System.ArgumentException("index must be between[0...3]");
#endif
                fixed (double2x4* array = &this) { return ref ((double2*)array)[index]; }
            }
        }

        /// <summary>Returns true if the double2x4 is equal to a given double2x4, false otherwise.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(double2x4 rhs) { return c0.Equals(rhs.c0) && c1.Equals(rhs.c1) && c2.Equals(rhs.c2) && c3.Equals(rhs.c3); }

        /// <summary>Returns true if the double2x4 is equal to a given double2x4, false otherwise.</summary>
        public override bool Equals(object o) { return Equals((double2x4)o); }


        /// <summary>Returns a hash code for the double2x4.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode() { return (int)math.hash(this); }


        /// <summary>Returns a string representation of the double2x4.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override string ToString()
        {
            return string.Format("double2x4({0}, {1}, {2}, {3},  {4}, {5}, {6}, {7})", c0.x, c1.x, c2.x, c3.x, c0.y, c1.y, c2.y, c3.y);
        }

        /// <summary>Returns a string representation of the double2x4 using a specified format and culture-specific format information.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public string ToString(string format, IFormatProvider formatProvider)
        {
            return string.Format("double2x4({0}, {1}, {2}, {3},  {4}, {5}, {6}, {7})", c0.x.ToString(format, formatProvider), c1.x.ToString(format, formatProvider), c2.x.ToString(format, formatProvider), c3.x.ToString(format, formatProvider), c0.y.ToString(format, formatProvider), c1.y.ToString(format, formatProvider), c2.y.ToString(format, formatProvider), c3.y.ToString(format, formatProvider));
        }

    }

    public static partial class math
    {
        /// <summary>Returns a double2x4 matrix constructed from four double2 vectors.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double2x4 double2x4(double2 c0, double2 c1, double2 c2, double2 c3) { return new double2x4(c0, c1, c2, c3); }

        /// <summary>Returns a double2x4 matrix constructed from from 8 double values given in row-major order.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double2x4 double2x4(double m00, double m01, double m02, double m03,
                                          double m10, double m11, double m12, double m13)
        {
            return new double2x4(m00, m01, m02, m03,
                                 m10, m11, m12, m13);
        }

        /// <summary>Returns a double2x4 matrix constructed from a single double value by assigning it to every component.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double2x4 double2x4(double v) { return new double2x4(v); }

        /// <summary>Returns a double2x4 matrix constructed from a single bool value by converting it to double and assigning it to every component.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double2x4 double2x4(bool v) { return new double2x4(v); }

        /// <summary>Return a double2x4 matrix constructed from a bool2x4 matrix by componentwise conversion.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double2x4 double2x4(bool2x4 v) { return new double2x4(v); }

        /// <summary>Returns a double2x4 matrix constructed from a single int value by converting it to double and assigning it to every component.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double2x4 double2x4(int v) { return new double2x4(v); }

        /// <summary>Return a double2x4 matrix constructed from a int2x4 matrix by componentwise conversion.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double2x4 double2x4(int2x4 v) { return new double2x4(v); }

        /// <summary>Returns a double2x4 matrix constructed from a single uint value by converting it to double and assigning it to every component.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double2x4 double2x4(uint v) { return new double2x4(v); }

        /// <summary>Return a double2x4 matrix constructed from a uint2x4 matrix by componentwise conversion.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double2x4 double2x4(uint2x4 v) { return new double2x4(v); }

        /// <summary>Returns a double2x4 matrix constructed from a single float value by converting it to double and assigning it to every component.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double2x4 double2x4(float v) { return new double2x4(v); }

        /// <summary>Return a double2x4 matrix constructed from a float2x4 matrix by componentwise conversion.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double2x4 double2x4(float2x4 v) { return new double2x4(v); }

        /// <summary>Return the double4x2 transpose of a double2x4 matrix.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double4x2 transpose(double2x4 v)
        {
            return double4x2(
                v.c0.x, v.c0.y,
                v.c1.x, v.c1.y,
                v.c2.x, v.c2.y,
                v.c3.x, v.c3.y);
        }

        /// <summary>Returns a uint hash code of a double2x4 vector.</summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint hash(double2x4 v)
        {
            return csum(fold_to_uint(v.c0) * uint2(0x91475DF7u, 0x55E84827u) +
                        fold_to_uint(v.c1) * uint2(0x90A285BBu, 0x5D19E1D5u) +
                        fold_to_uint(v.c2) * uint2(0xFAAF07DDu, 0x625C45BDu) +
                        fold_to_uint(v.c3) * uint2(0xC9F27FCBu, 0x6D2523B1u)) + 0x6E2BF6A9u;
        }

        /// <summary>
        /// Returns a uint2 vector hash code of a double2x4 vector.
        /// When multiple elements are to be hashes together, it can more efficient to calculate and combine wide hash
        /// that are only reduced to a narrow uint hash at the very end instead of at every step.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint2 hashwide(double2x4 v)
        {
            return (fold_to_uint(v.c0) * uint2(0xCC74B3B7u, 0x83B58237u) +
                    fold_to_uint(v.c1) * uint2(0x833E3E29u, 0xA9D919BFu) +
                    fold_to_uint(v.c2) * uint2(0xC3EC1D97u, 0xB8B208C7u) +
                    fold_to_uint(v.c3) * uint2(0x5D3ED947u, 0x4473BBB1u)) + 0xCBA11D5Fu;
        }

    }
}