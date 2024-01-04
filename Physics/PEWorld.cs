using System;
using System.Collections.Generic;

namespace PhysicalEngine.Physics
{
    public sealed class PEWorld
    {
        public static readonly float MinBodySize = 0.01f * 0.01f;
        public static readonly float MaxBodySize = 64f * 64f;

        public static readonly float MinDensity = 0.5f;     // g/cm^3
        public static readonly float MaxDensity = 21.4f;

        public static readonly int MinIterations = 1;
        public static readonly int MaxIterations = 128;

        private PEVector2 gravity;
        private List<PEBody> bodyList;

        public int BodyCount
        {
            get { return this.bodyList.Count; }
        }

        public PEWorld()
        {
            this.gravity = new PEVector2(0f, -9.81f);
            this.bodyList = new List<PEBody>();
        }

        public void AddBody(PEBody body)
        {
            this.bodyList.Add(body);
        }

        public bool RemoveBody(PEBody body)
        {
            return this.bodyList.Remove(body);
        }

        public bool GetBody(int index, out PEBody body)
        {
            body = null;

            if(index < 0 || index >= this.bodyList.Count)
            {
                return false;
            }

            body = this.bodyList[index];
            return true;
        }

        public void Step(float time, int iterations)
        {
            iterations = PEMath.Clamp(iterations, PEWorld.MinIterations, PEWorld.MaxIterations);

            for (int it = 0; it < iterations; it++)
            {
                // Movement step
                for (int i = 0; i < this.bodyList.Count; i++)
                {
                    this.bodyList[i].Step(time, this.gravity, iterations);
                }

                // collision step
                for (int i = 0; i < this.bodyList.Count - 1; i++)
                {
                    PEBody bodyA = this.bodyList[i];

                    for (int j = i + 1; j < this.bodyList.Count; j++)
                    {
                        PEBody bodyB = this.bodyList[j];

                        if (bodyA.IsStatic && bodyB.IsStatic)
                        {
                            continue;
                        }

                        if (this.Collide(bodyA, bodyB, out PEVector2 normal, out float depth))
                        {
                            if (bodyA.IsStatic)
                            {
                                bodyB.Move(normal * depth);
                            }
                            else if (bodyB.IsStatic)
                             {
                                bodyA.Move(-normal * depth);
                            }
                            else
                            {
                                bodyA.Move(-normal * depth / 2f);
                                bodyB.Move(normal * depth / 2f);
                            }

                            this.ResolveCollision(bodyA, bodyB, normal, depth);
                        }
                    }
                }
            }
        }

        public void ResolveCollision(PEBody bodyA, PEBody bodyB, PEVector2 normal, float depth)
        {
            PEVector2 relativeVelocity = bodyB.LinearVelocity - bodyA.LinearVelocity;

            if (PEMath.Dot(relativeVelocity, normal) > 0f)
            {
                 return;
            }

            float e = MathF.Min(bodyA.Restitution, bodyB.Restitution);
            float j = -(1f + e) * PEMath.Dot(relativeVelocity, normal);
            j /= bodyA.InvMass + bodyB.InvMass;

            PEVector2 impulse = j * normal;

            bodyA.LinearVelocity -= impulse * bodyA.InvMass;
            bodyB.LinearVelocity += impulse * bodyB.InvMass;
        }

        public bool Collide(PEBody bodyA, PEBody bodyB, out PEVector2 normal, out float depth)
        {
            normal = PEVector2.Zero;
            depth = 0f;

            ShapeType shapeTypeA = bodyA.ShapeType;
            ShapeType shapeTypeB = bodyB.ShapeType;

            if(shapeTypeA is ShapeType.Box)
            {
                if (shapeTypeB is ShapeType.Box)
                {
                    return Collisions.IntersectPolygons(
                        bodyA.Position, bodyA.GetTransformedVertices(), 
                        bodyB.Position, bodyB.GetTransformedVertices(),
                        out normal, out depth);
                }
                else if (shapeTypeB is ShapeType.Circle)
                {
                    bool result = Collisions.IntersectCirclePolygon(
                        bodyB.Position, bodyB.Radius, 
                        bodyA.Position, bodyA.GetTransformedVertices(),
                        out normal, out depth);

                    normal = -normal;
                    return result;
                }
            }
            else if(shapeTypeA is ShapeType.Circle)
            {
                if (shapeTypeB is ShapeType.Box)
                {
                    return Collisions.IntersectCirclePolygon(
                        bodyA.Position, bodyA.Radius, 
                        bodyB.Position, bodyB.GetTransformedVertices(),
                        out normal, out depth);
                }
                else if (shapeTypeB is ShapeType.Circle)
                {
                    return Collisions.IntersectCircles(
                        bodyA.Position, bodyA.Radius, 
                        bodyB.Position, bodyB.Radius,
                        out normal, out depth);
                }
            }

            return false;
        }
    }
}