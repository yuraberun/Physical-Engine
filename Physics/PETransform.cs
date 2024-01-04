using System;
using PhysicalEngine.Physics;

namespace PhysicalEngine.Physics
{
    internal readonly struct PETransform
    {
        public readonly float PositionX;
        public readonly float PositionY;
        public readonly float Sin;
        public readonly float Cos;

        public readonly static PETransform Zero = new PETransform(0f, 0f, 0f);

        public PETransform(PEVector2 position, float angle)
        {
            this.PositionX = position.X;
            this.PositionY = position.Y;
            this.Sin = MathF.Sin(angle);
            this.Cos = MathF.Cos(angle);
        }

        public PETransform(float x, float y, float angle)
        {
            this.PositionX = x;
            this.PositionY = y;
            this.Sin = MathF.Sin(angle);
            this.Cos = MathF.Cos(angle);
        }
    }
}