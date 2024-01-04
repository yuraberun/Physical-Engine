using System;

namespace PhysicalEngine.Physics
{
    public static class PEMath
    {
        public static float Clamp(float value, float min, float max)
        {
            if(min == max)
            {
                return min;
            }

            if(min > max)
            {
                throw new ArgumentOutOfRangeException("min is greater than the max.");
            }

            if(value < min)
            {
                return min;
            }

            if(value > max)
            {
                return max;
            }

            return value;
        }

        public static int Clamp(int value, int min, int max)
        {
            if (min == max)
            {
                return min;
            }

            if (min > max)
            {
                throw new ArgumentOutOfRangeException("min is greater than the max.");
            }

            if (value < min)
            {
                return min;
            }

            if (value > max)
            {
                return max;
            }

            return value;
        }


        public static float Length(PEVector2 v)
        {
            return MathF.Sqrt(v.X * v.X + v.Y * v.Y);
        }

        public static float Distance(PEVector2 a, PEVector2 b)
        {
            float dx = a.X - b.X;
            float dy = a.Y - b.Y;
            
            return MathF.Sqrt(dx * dx + dy * dy);
        }

        public static PEVector2 Normalize(PEVector2 v)
        {
            float len = PEMath.Length(v);

            return new PEVector2(v.X / len, v.Y / len);
        }

        public static float Dot(PEVector2 a, PEVector2 b)
        {
            return a.X * b.X + a.Y * b.Y;
        }

        public static float Cross(PEVector2 a, PEVector2 b)
        {
            // cz = ax * by − ay * bx
            return a.X * b.Y - a.Y * b.X;
        }

    }
}