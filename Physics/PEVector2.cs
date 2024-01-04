using System;
using PhysicalEngine.Tools;

namespace PhysicalEngine.Physics
{
    public readonly struct PEVector2
    {
        public readonly float X;
        public readonly float Y;

        public PEVector2(float x, float y)
        {
            this.X = x;
            this.Y = y;
        }

        public static readonly PEVector2 Zero = new PEVector2(0f, 0f);

        public static PEVector2 operator +(PEVector2 a, PEVector2 b)
        {
            return new PEVector2(a.X + b.X, a.Y + b.Y);
        }

        public static PEVector2 operator -(PEVector2 a, PEVector2 b)
        {
            return new PEVector2(a.X - b.X, a.Y - b.Y);
        }

        public static PEVector2 operator -(PEVector2 v)
        {
            return new PEVector2(-v.X, -v.Y);
        }

        public static PEVector2 operator *(PEVector2 v, float s)
        {
            return new PEVector2(v.X * s, v.Y * s);
        }

        public static PEVector2 operator *(float s, PEVector2 v)
        {
            return new PEVector2(v.X * s, v.Y * s);
        }

        public static PEVector2 operator /(PEVector2 v, float s)
        {
            return new PEVector2(v.X / s, v.Y / s);
        }

        internal static PEVector2 Transform(PEVector2 v, PETransform transform)
        {
             return new PEVector2(
                 transform.Cos * v.X - transform.Sin * v.Y + transform.PositionX, 
                 transform.Sin * v.X + transform.Cos * v.Y + transform.PositionY);
        }

        public bool Equals(PEVector2 other)
        {
            return X == other.X && this.Y == other.Y;
        }

        public override bool Equals(object obj)
        {
            if (obj is PEVector2 other)
            {
                return this.Equals(other);
            }

            return false;
        }

        public override int GetHashCode()
        {
            return new { this.X, this.Y }.GetHashCode();
        }

        public override string ToString()
        {
            return $"X: {this.X}, Y: {this.Y}";
        }
    }
}