using System;
using Microsoft.Xna.Framework;
using PhysicalEngine.Physics;
using PhysicalEngine.Tools;

namespace PhysicalEngine
{
    public static class PERandom
    {
        private static Random Rand = new Random();

        public static int RandomInteger()
        {
            return Rand.Next();
        }

        public static int RandomInteger(Random rand)
        {
            return rand.Next();
        }

        public static int RandomInteger(int min, int max)
        {
            if(min == max)
            {
                return min;
            }

            if (min > max)
            {
                PEUtils.Swap(ref min, ref max);
            }

            int result = min + Rand.Next() % (max - min);
            return result;
        }

        public static int RandomInteger(Random rand, int min, int max)
        {
            if (min > max)
            {
                PEUtils.Swap(ref min, ref max);
            }

            int result = min + rand.Next() % (max - min);
            return result;
        }

        public static bool RandomBooleon()
        {
            int value = PERandom.RandomInteger(0, 2);

            if(value == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public static float RandomSingle()
        {
            return (float)Rand.NextDouble();
        }

        public static float RandomSingle(Random rand)
        {
            return (float)rand.NextDouble();
        }

        public static float RandomSingle(float min, float max)
        {
            if (min > max)
            {
                PEUtils.Swap(ref min, ref max);
            }

            float result = min + (float)Rand.NextDouble() * (max - min);
            return result;
        }

        public static float RandomSingle(Random rand, float min, float max)
        {
            if (min > max)
            {
                PEUtils.Swap(ref min, ref max);
            }

            float result = min + (float)rand.NextDouble() * (max - min);
            return result;
        }

        public static Color RandomColor()
        {
            Color result = new Color((float)Rand.NextDouble(), (float)Rand.NextDouble(), (float)Rand.NextDouble());
            return result;
        }
        
        public static Color RandomColor(Random rand)
        {
            Color result = new Color((float)rand.NextDouble(), (float)rand.NextDouble(), (float)rand.NextDouble());
            return result;
        }

        public static Color RandomColor(float brightness)
        {
            brightness = PEMath.Clamp(brightness, 0f, 1f);

            float r = PERandom.RandomSingle(0f, 1f);
            float g = PERandom.RandomSingle(0f, 1f);
            float b = PERandom.RandomSingle(0f, 1f);

            float dec = 0.98f;
            float inc = 1f / dec;

            for(int i = 0; i < 64; i++)
            {
                float perceivedBrightness = PEUtils.PercievedBrightness(r, g, b);
                
                if(perceivedBrightness < brightness)
                {
                    r *= inc;
                    g *= inc;
                    b *= inc;
                }
                else if(perceivedBrightness > brightness)
                {
                    r *= dec;
                    g *= dec;
                    b *= dec;
                }

                dec += 0.0001f;
                inc -= 0.0001f;

                if(dec > 1f) { dec = 1f; }
                if(inc < 1f) { inc = 1f; }
            }

            return new Color(r, g, b);
        }



        public static Vector2 RandomDirection()
        {
            float angle = RandomSingle(0, MathHelper.TwoPi);
            Vector2 result = new Vector2(MathF.Cos(angle), MathF.Sin(angle));
            return result;
        }

        public static Vector2 RandomDirection(Random rand)
        {
            float angle = RandomSingle(rand, 0, MathHelper.TwoPi);
            Vector2 result = new Vector2(MathF.Cos(angle), MathF.Sin(angle));
            return result;
        }
    }
}