using System;

namespace JGameEngine.Terrains
{
    public class JHeightGenerator
    {
        private static readonly float AMPLITUDE = 10f;

        private Random Random { get; set; }
        private int Seed { get; set; }

        private int XMult { get; set; }
        private int ZMult { get; set; }

        public JHeightGenerator()
        {
            Random = new Random();
            Seed = Random.Next(10);
        }

        public float GenerateHeight(int x,int z)
        {
            float total = GetInterpolatedNoise(x / 4f, z / 4f) * AMPLITUDE;
            total += GetInterpolatedNoise(x / 2f, z / 2f) * AMPLITUDE / 4f;
            return total;
        }

        private float GetSmoothNoise(int x, int z)
        {
            float corners = (GetNoise(x - 1, z - 1) + GetNoise(x + 1, z - 1) + GetNoise(x - 1, z + 1) + GetNoise(x + 1, z + 1)) / 16f;
            float sides = (GetNoise(x - 1, z) + GetNoise(x + 1, z) + GetNoise(x, z + 1) + GetNoise(x, z + 1)) / 8f;
            float center = (GetNoise(x, z)) / 4f;

            return corners + sides + center;
        }

        private float GetInterpolatedNoise(float x, float z)
        {
            int intX = (int)x;
            int intZ = (int)z;

            float fracX = x - intX;
            float fracZ = z - intZ;

            float v1 = GetSmoothNoise(intX, intZ);
            float v2 = GetSmoothNoise(intX + 1, intZ);
            float v3 = GetSmoothNoise(intX, intZ + 1);
            float v4 = GetSmoothNoise(intX + 1, intZ + 1);
            float i1 = Interpolate(v1, v2, fracX);
            float i2 = Interpolate(v3, v4, fracX);
            return Interpolate(i1, i2, fracZ);
        }

        private float Interpolate(float a, float b, float blend)
        {
            double theta = blend * Math.PI;
            float f = (float)(1f - Math.Cos(theta)) * 0.5f;
            return a * (1f - f) + b * f;
        }

        public float GetNoise(int x, int z)
        {
            Random tempRandom = new Random(x * z * Seed);
            return ((float)tempRandom.NextDouble() * 2f - 1f);
        }
    }
}
