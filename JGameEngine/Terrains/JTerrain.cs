using JGameEngine.Models;
using JGameEngine.RenderEngine;
using JGameEngine.Textures;
using JGameEngine.Utils;
using OpenTK;
using System;

namespace JGameEngine.Terrains
{
    public class JTerrain
    {
        private static readonly float SIZE = 800;
        private static readonly int VERTEX_COUNT = 128;

        public float X { get; set; }
        public float Z { get; set; }
        public JRawModel TerrainModel { get; set; }
        public JModelTexture TerrainTexture { get; set; }

        private float[,] heights;

        public JTerrain(int gridX,int gridZ,JLoader loader,JModelTexture texture)
        {
            X = gridX * SIZE;
            Z = gridZ * SIZE;
            TerrainTexture = texture;
            TerrainModel = generateTerrain(loader);
        }

        private JRawModel generateTerrain(JLoader loader)
        {
            Console.WriteLine("Generating terrain...");
            JHeightGenerator heightGenerator = new JHeightGenerator();

            int count = VERTEX_COUNT * VERTEX_COUNT;
            heights = new float[VERTEX_COUNT,VERTEX_COUNT];
            float[] vertices = new float[count * 3];
            float[] normals = new float[count * 3];
            float[] textureCoords = new float[count * 2];
            uint[] indices = new uint[6 * (VERTEX_COUNT - 1) * (VERTEX_COUNT - 1)];
            int vertexPointer = 0;
            for (int i = 0; i < VERTEX_COUNT; i++)
            {
                for (int j = 0; j < VERTEX_COUNT; j++)
                {
                    vertices[vertexPointer * 3] = (float)j / ((float)VERTEX_COUNT - 1) * SIZE;
                    float height = GetHeight(j, i, heightGenerator);
                    vertices[vertexPointer * 3 + 1] = height;
                    heights[j, i] = height;
                    vertices[vertexPointer * 3 + 2] = (float)i / ((float)VERTEX_COUNT - 1) * SIZE;
                    Vector3 normal = CalculateNormal(j, i, heightGenerator);
                    normals[vertexPointer * 3] = normal.X;
                    normals[vertexPointer * 3 + 1] = normal.Y;
                    normals[vertexPointer * 3 + 2] = normal.Z;
                    textureCoords[vertexPointer * 2] = (float)j / ((float)VERTEX_COUNT - 1);
                    textureCoords[vertexPointer * 2 + 1] = (float)i / ((float)VERTEX_COUNT - 1);
                    vertexPointer++;
                }
            }
            int pointer = 0;
            for (uint gz = 0; gz < VERTEX_COUNT - 1; gz++)
            {
                for (uint gx = 0; gx < VERTEX_COUNT - 1; gx++)
                {
                    uint topLeft = (uint)(gz * VERTEX_COUNT) + gx;
                    uint topRight = topLeft + 1;
                    uint bottomLeft = (uint)((gz + 1) * VERTEX_COUNT) + gx;
                    uint bottomRight = bottomLeft + 1;
                    indices[pointer++] = topLeft;
                    indices[pointer++] = bottomLeft;
                    indices[pointer++] = topRight;
                    indices[pointer++] = topRight;
                    indices[pointer++] = bottomLeft;
                    indices[pointer++] = bottomRight;
                }
            }

            Console.WriteLine("Terrain generation complete...");
            return loader.loadToVAO(vertices, textureCoords, normals, indices);
        }

        private Vector3 CalculateNormal(int x, int z, JHeightGenerator generator)
        {
            float heightL = GetHeight(x - 1, z, generator);
            float heightR = GetHeight(x + 1, z, generator);
            float heightD = GetHeight(x, z - 1, generator);
            float heightU = GetHeight(x, z + 1, generator);

            Vector3 normal = new Vector3(heightL - heightR, 2f, heightD - heightU);
            normal.Normalize();
            return normal;
        }

        private float GetHeight(int x,int z,JHeightGenerator generator)
        {
            return generator.GenerateHeight(x, z);
        }

        public float GetHeightOfTerrain(float worldX,float worldZ)
        {
            float terrainX = worldX - this.X;
            float terrainZ = worldZ - this.Z;
            float gridSquareSize = SIZE / ((float)heights.GetLength(0) - 1);
            int gridX = (int)Math.Floor(terrainX / gridSquareSize);
            int gridZ = (int)Math.Floor(terrainZ / gridSquareSize);
            if (gridX >= heights.GetLength(0) - 1 || gridZ >= heights.GetLength(1) - 1 || gridX < 0 || gridZ < 0)
            {
                return 0;
            }

            float xCoord = (terrainX % gridSquareSize) / gridSquareSize;
            float zCoord = (terrainZ % gridSquareSize) / gridSquareSize;
            float answer;
            
            if(xCoord <= (1 - zCoord))
            {
                answer = JMathUtils.BarryCentric(new Vector3(0, heights[gridX,gridZ], 0), new Vector3(1,
                            heights[gridX + 1,gridZ], 0), new Vector3(0,
                            heights[gridX,gridZ + 1], 1), new Vector2(xCoord, zCoord));
            }
            else
            {
                answer = JMathUtils.BarryCentric(new Vector3(1, heights[gridX + 1,gridZ], 0), new Vector3(1,
                            heights[gridX + 1,gridZ + 1], 1), new Vector3(0,
                            heights[gridX,gridZ + 1], 1), new Vector2(xCoord, zCoord));
            }
            return answer;
        }
    }
}
