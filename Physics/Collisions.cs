using System;

namespace PhysicalEngine.Physics
{
    public static class Collisions
    {
        public static bool IntersectCirclePolygon(PEVector2 circleCenter, float circleRadius,
                                                    PEVector2 polygonCenter, PEVector2[] vertices,
                                                    out PEVector2 normal, out float depth)
        {
            normal = PEVector2.Zero;
            depth = float.MaxValue;

            PEVector2 axis = PEVector2.Zero;
            float axisDepth = 0f;
            float minA, maxA, minB, maxB;

            for (int i = 0; i < vertices.Length; i++)
            {
                PEVector2 va = vertices[i];
                PEVector2 vb = vertices[(i + 1) % vertices.Length];

                PEVector2 edge = vb - va;
                axis = new PEVector2(-edge.Y, edge.X);
                axis = PEMath.Normalize(axis);

                Collisions.ProjectVertices(vertices, axis, out minA, out maxA);
                Collisions.ProjectCircle(circleCenter, circleRadius, axis, out minB, out maxB);

                if (minA >= maxB || minB >= maxA)
                {
                    return false;
                }

                axisDepth = MathF.Min(maxB - minA, maxA - minB);

                if (axisDepth < depth)
                {
                    depth = axisDepth;
                    normal = axis;
                }
            }

            int cpIndex = Collisions.FindClosestPointOnPolygon(circleCenter, vertices);
            PEVector2 cp = vertices[cpIndex];

            axis = cp - circleCenter;
            axis = PEMath.Normalize(axis);

            Collisions.ProjectVertices(vertices, axis, out minA, out maxA);
            Collisions.ProjectCircle(circleCenter, circleRadius, axis, out minB, out maxB);

            if (minA >= maxB || minB >= maxA)
            {
                return false;
            }

            axisDepth = MathF.Min(maxB - minA, maxA - minB);

            if (axisDepth < depth)
            {
                depth = axisDepth;
                normal = axis;
            }

            PEVector2 direction = polygonCenter - circleCenter;

            if (PEMath.Dot(direction, normal) < 0f)
            {
                normal = -normal;
            }

            return true;
        }


        public static bool IntersectCirclePolygon(PEVector2 circleCenter, float circleRadius, 
            PEVector2[] vertices, 
            out PEVector2 normal, out float depth)
        {
            normal = PEVector2.Zero;
            depth = float.MaxValue;

            PEVector2 axis = PEVector2.Zero;
            float axisDepth = 0f;
            float minA, maxA, minB, maxB;

            for (int i = 0; i < vertices.Length; i++)
            {
                PEVector2 va = vertices[i];
                PEVector2 vb = vertices[(i + 1) % vertices.Length];

                PEVector2 edge = vb - va;
                axis = new PEVector2(-edge.Y, edge.X);
                axis = PEMath.Normalize(axis);

                Collisions.ProjectVertices(vertices, axis, out minA, out maxA);
                Collisions.ProjectCircle(circleCenter, circleRadius, axis, out minB, out maxB);

                if (minA >= maxB || minB >= maxA)
                {
                    return false;
                }

                axisDepth = MathF.Min(maxB - minA, maxA - minB);

                if (axisDepth < depth)
                {
                    depth = axisDepth;
                    normal = axis;
                }
            }

            int cpIndex = Collisions.FindClosestPointOnPolygon(circleCenter, vertices);
            PEVector2 cp = vertices[cpIndex];

            axis = cp - circleCenter;
            axis = PEMath.Normalize(axis);

            Collisions.ProjectVertices(vertices, axis, out minA, out maxA);
            Collisions.ProjectCircle(circleCenter, circleRadius, axis, out minB, out maxB);

            if (minA >= maxB || minB >= maxA)
            {
                return false;
            }

            axisDepth = MathF.Min(maxB - minA, maxA - minB);

            if (axisDepth < depth)
            {
                depth = axisDepth;
                normal = axis;
            }

            PEVector2 polygonCenter = Collisions.FindArithmeticMean(vertices);

            PEVector2 direction = polygonCenter - circleCenter;

            if (PEMath.Dot(direction, normal) < 0f)
            {
                normal = -normal;
            }

            return true;
        }

        private static int FindClosestPointOnPolygon(PEVector2 circleCenter, PEVector2[] vertices)
        {
            int result = -1;
            float minDistance = float.MaxValue;

            for(int i = 0; i < vertices.Length; i++)
            {
                PEVector2 v = vertices[i];
                float distance = PEMath.Distance(v, circleCenter);

                if(distance < minDistance)
                {
                    minDistance = distance;
                    result = i;
                }
            }

            return result;
        }

        private static void ProjectCircle(PEVector2 center, float radius, PEVector2 axis, out float min, out float max)
        {
            PEVector2 direction = PEMath.Normalize(axis);
            PEVector2 directionAndRadius = direction * radius;

            PEVector2 p1 = center + directionAndRadius;
            PEVector2 p2 = center - directionAndRadius;

            min = PEMath.Dot(p1, axis);
            max = PEMath.Dot(p2, axis);

            if(min > max)
            {
                // swap the min and max values.
                float t = min;
                min = max;
                max = t;
            }
        }

        public static bool IntersectPolygons(PEVector2 centerA, PEVector2[] verticesA, PEVector2 centerB, PEVector2[] verticesB, out PEVector2 normal, out float depth)
        {
            normal = PEVector2.Zero;
            depth = float.MaxValue;

            for (int i = 0; i < verticesA.Length; i++)
            {
                PEVector2 va = verticesA[i];
                PEVector2 vb = verticesA[(i + 1) % verticesA.Length];

                PEVector2 edge = vb - va;
                PEVector2 axis = new PEVector2(-edge.Y, edge.X);
                axis = PEMath.Normalize(axis);

                Collisions.ProjectVertices(verticesA, axis, out float minA, out float maxA);
                Collisions.ProjectVertices(verticesB, axis, out float minB, out float maxB);

                if (minA >= maxB || minB >= maxA)
                {
                    return false;
                }

                float axisDepth = MathF.Min(maxB - minA, maxA - minB);

                if (axisDepth < depth)
                {
                    depth = axisDepth;
                    normal = axis;
                }
            }

            for (int i = 0; i < verticesB.Length; i++)
            {
                PEVector2 va = verticesB[i];
                PEVector2 vb = verticesB[(i + 1) % verticesB.Length];

                PEVector2 edge = vb - va;
                PEVector2 axis = new PEVector2(-edge.Y, edge.X);
                axis = PEMath.Normalize(axis);

                Collisions.ProjectVertices(verticesA, axis, out float minA, out float maxA);
                Collisions.ProjectVertices(verticesB, axis, out float minB, out float maxB);

                if (minA >= maxB || minB >= maxA)
                {
                    return false;
                }

                float axisDepth = MathF.Min(maxB - minA, maxA - minB);

                if (axisDepth < depth)
                {
                    depth = axisDepth;
                    normal = axis;
                }
            }

            PEVector2 direction = centerB - centerA;

            if (PEMath.Dot(direction, normal) < 0f)
            {
                normal = -normal;
            }

            return true;
        }

        public static bool IntersectPolygons(PEVector2[] verticesA, PEVector2[] verticesB, out PEVector2 normal, out float depth)
        {
            normal = PEVector2.Zero;
            depth = float.MaxValue;

            for(int i = 0; i < verticesA.Length; i++)
            {
                PEVector2 va = verticesA[i];
                PEVector2 vb = verticesA[(i + 1) % verticesA.Length];

                PEVector2 edge = vb - va;
                PEVector2 axis = new PEVector2(-edge.Y, edge.X);
                axis = PEMath.Normalize(axis);

                Collisions.ProjectVertices(verticesA, axis, out float minA, out float maxA);
                Collisions.ProjectVertices(verticesB, axis, out float minB, out float maxB);

                if(minA >= maxB || minB >= maxA)
                {
                    return false;
                }

                float axisDepth = MathF.Min(maxB - minA, maxA - minB);

                if(axisDepth < depth)
                {
                    depth = axisDepth;
                    normal = axis;
                }
            }

            for (int i = 0; i < verticesB.Length; i++)
            {
                PEVector2 va = verticesB[i];
                PEVector2 vb = verticesB[(i + 1) % verticesB.Length];

                PEVector2 edge = vb - va;
                PEVector2 axis = new PEVector2(-edge.Y, edge.X);
                axis = PEMath.Normalize(axis);

                Collisions.ProjectVertices(verticesA, axis, out float minA, out float maxA);
                Collisions.ProjectVertices(verticesB, axis, out float minB, out float maxB);

                if (minA >= maxB || minB >= maxA)
                {
                    return false;
                }

                float axisDepth = MathF.Min(maxB - minA, maxA - minB);

                if (axisDepth < depth)
                {
                    depth = axisDepth;
                    normal = axis;
                }
            }

            PEVector2 centerA = Collisions.FindArithmeticMean(verticesA);
            PEVector2 centerB = Collisions.FindArithmeticMean(verticesB);

            PEVector2 direction = centerB - centerA;

            if(PEMath.Dot(direction, normal) < 0f)
            {
                normal = -normal;
            }

            return true;
        }

        private static PEVector2 FindArithmeticMean(PEVector2[] vertices)
        {
            float sumX = 0f;
            float sumY = 0f;

            for(int i = 0; i < vertices.Length; i++)
            {
                PEVector2 v = vertices[i];
                sumX += v.X;
                sumY += v.Y;
            }

            return new PEVector2(sumX / (float)vertices.Length, sumY / (float)vertices.Length);
        }

        private static void ProjectVertices(PEVector2[] vertices, PEVector2 axis, out float min, out float max)
        {
            min = float.MaxValue;
            max = float.MinValue;

            for(int i = 0; i < vertices.Length; i++)
            {
                PEVector2 v = vertices[i];
                float proj = PEMath.Dot(v, axis);

                if(proj < min) { min = proj; }
                if(proj > max) { max = proj; }
            }
        }

        public static bool IntersectCircles(
            PEVector2 centerA, float radiusA, 
            PEVector2 centerB, float radiusB, 
            out PEVector2 normal, out float depth)
        {
            normal = PEVector2.Zero;
            depth = 0f;

            float distance = PEMath.Distance(centerA, centerB);
            float radii = radiusA + radiusB;

            if(distance >= radii)
            {
                return false;
            }

            normal = PEMath.Normalize(centerB - centerA);
            depth = radii - distance;

            return true;
        }

    }
}