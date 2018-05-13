using JGameEngine.Utils;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JGameEngine.Terrains
{
    public static class JNoise
    {
        /// <summary>
        /// Generate a float[mapWidth,mapHeight] containing values in the range [0,1] based on PerlinNoise algorithm.
        /// </summary>
        /// <param name="mapWidth"></param>
        /// <param name="mapHeight"></param>
        /// <param name="scale"></param>
        /// <param name="octaves">The number of octives. Must be greater than 0.</param>
        /// <param name="persistance">A value, in the range 0 to 1, commonly 0.5.</param>
        /// <param name="lacunarity">A value, greater than 1, commonly 2.</param>
        /// <returns>A float array of "randomly" distributed values.</returns>
        public static float[,] GenerateNoiseMap(int mapWidth,int mapHeight,float scale,int octaves,float persistance,float lacunarity)
        {
            float[,] noiseMap = new float[mapWidth, mapHeight];

            float maxNoiseHeight = float.MinValue;
            float minNoiseHeight = float.MaxValue;

            if(scale <= 0)
            {
                scale = 0.0001f;
            }

            for (int y=0; y < mapHeight; y++)
            {
                for(int x=0; x < mapWidth; x++)
                {
                    float amplitude = 1;
                    float frequency = 1;
                    float noiseHeight = 0;

                    for(int i = 0; i < octaves; i++)
                    {
                        float sampleX = (x / scale) * frequency;
                        float sampleY = (y / scale) * frequency;

                        float perlinValue = (float)JPerlinNoise.GetNoise((double)sampleX, (double)sampleY, (double)1.0f) * 2 - 1;
                        noiseHeight += perlinValue * amplitude;

                        // Amplitude decreases each octive.
                        amplitude *= persistance;

                        // Frequency increases each octive.
                        frequency *= lacunarity;
                    }

                    // Track the minimum and maximum values within noiseMap.
                    if(noiseHeight > maxNoiseHeight)
                    {
                        maxNoiseHeight = noiseHeight;
                    }
                    else if(noiseHeight < minNoiseHeight)
                    {
                        minNoiseHeight = noiseHeight;
                    }

                    noiseMap[x, y] = noiseHeight;
                }
            }

            // Map the noiseMap values to the range [0,1].
            for (int y = 0; y < mapHeight; y++)
            {
                for (int x = 0; x < mapWidth; x++)
                {
                    noiseMap[x, y] = JMathUtils.InvLerp(minNoiseHeight, maxNoiseHeight, noiseMap[x, y]);
                }
            }

            return noiseMap;
        }

        /// <summary>
        /// Use an offset to sample JPerlinNoise at a random location rather than a consistent location. Not working quite right yet.
        /// </summary>
        /// <param name="mapWidth"></param>
        /// <param name="mapHeight"></param>
        /// <param name="seed"></param>
        /// <param name="scale"></param>
        /// <param name="octaves">The number of octives. Must be greater than 0.</param>
        /// <param name="persistance">A value, in the range 0 to 1, commonly 0.5.</param>
        /// <param name="lacunarity">A value, greater than 1, commonly 2.</param>
        /// <returns></returns>
        public static float[,] GenerateNoiseMap(int mapWidth, int mapHeight, int seed, float scale, int octaves, float persistance, float lacunarity)
        {
            float[,] noiseMap = new float[mapWidth, mapHeight];

            float maxNoiseHeight = float.MinValue;
            float minNoiseHeight = float.MaxValue;

            Random rand = new Random(seed);
            Vector2[] octaveOffsets = new Vector2[octaves];

            for(int i = 0; i < octaves; i++)
            {
                float offsetX = rand.Next(-5, 5) / scale;
                float offsetY = rand.Next(-5, 5) / scale;
                octaveOffsets[i] = new Vector2(offsetX, offsetY);
            }

            if (scale <= 0)
            {
                scale = 0.0001f;
            }

            for (int y = 0; y < mapHeight; y++)
            {
                for (int x = 0; x < mapWidth; x++)
                {
                    float amplitude = 1;
                    float frequency = 1;
                    float noiseHeight = 0;

                    for (int i = 0; i < octaves; i++)
                    {
                        float sampleX = (x / scale) * frequency + octaveOffsets[i].X;
                        float sampleY = (y / scale) * frequency + octaveOffsets[i].Y;

                        float perlinValue = (float)JPerlinNoise.GetNoise((double)sampleX, (double)sampleY, (double)1.0f) * 2 - 1;
                        noiseHeight += perlinValue * amplitude;

                        // Amplitude decreases each octive.
                        amplitude *= persistance;

                        // Frequency increases each octive.
                        frequency *= lacunarity;
                    }

                    // Track the minimum and maximum values with noiseMap.
                    if (noiseHeight > maxNoiseHeight)
                    {
                        maxNoiseHeight = noiseHeight;
                    }
                    else if (noiseHeight < minNoiseHeight)
                    {
                        minNoiseHeight = noiseHeight;
                    }

                    noiseMap[x, y] = noiseHeight;
                }
            }

            for (int y = 0; y < mapHeight; y++)
            {
                for (int x = 0; x < mapWidth; x++)
                {
                    noiseMap[x, y] = JMathUtils.InvLerp(minNoiseHeight, maxNoiseHeight, noiseMap[x, y]);
                }
            }

            return noiseMap;
        }
    }
}
