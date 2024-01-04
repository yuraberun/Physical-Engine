using System;
using PhysicalEngine.Physics;

namespace PhysicalEngine.Physics
{
    public readonly struct PEAABB
    {
        public readonly PEVector2 Min;
        public readonly PEVector2 Max;

        public PEAABB(PEVector2 min, PEVector2 max)
        {
            this.Min = min;
            this.Max = max;
        }

        public PEAABB(float minX, float minY, float maxX, float maxY)
        {
            this.Min = new PEVector2(minX, minY);
            this.Max = new PEVector2(maxX, maxY);
        }
    }
}