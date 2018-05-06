using JGameEngine.Models;
using OpenTK;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JGameEngine.RenderEngine
{
    public class JObjFileLoader
    {
        public static JModelData LoadObj(string fileName)
        {
            List<JVertex> vertices = new List<JVertex>();
            List<Vector2> textures = new List<Vector2>();
            List<Vector3> normals = new List<Vector3>();
            List<uint> indices = new List<uint>();
            try
            {
                string line;
                using (FileStream stream = new FileStream(fileName, FileMode.Open))
                {
                    using (StreamReader rdr = new StreamReader(stream))
                    {
                        while (true)
                        {
                            line = rdr.ReadLine();

                            if (line.StartsWith("v "))
                            {
                                string[] currentLine = line.Split(' ');
                                Vector3 vertex = new Vector3(float.Parse(currentLine[1]), float.Parse(currentLine[2]), float.Parse(currentLine[3]));
                                JVertex newVertex = new JVertex((uint)vertices.Count, vertex);
                                vertices.Add(newVertex);
                            }
                            else if (line.StartsWith("vt "))
                            {
                                string[] currentLine = line.Split(' ');
                                Vector2 texture = new Vector2(float.Parse(currentLine[1]), float.Parse(currentLine[2]));
                                textures.Add(texture);
                            }
                            else if (line.StartsWith("vn "))
                            {
                                string[] currentLine = line.Split(' ');
                                Vector3 normal = new Vector3(float.Parse(currentLine[1]), float.Parse(currentLine[2]), float.Parse(currentLine[3]));
                                normals.Add(normal);
                            }
                            else if (line.StartsWith("f "))
                            {
                                break;
                            }
                        }
                        while(line != null && line.StartsWith("f "))
                        {
                            string[] currentLine = line.Split(' ');
                            string[] vertex1 = currentLine[1].Split('/');
                            string[] vertex2 = currentLine[2].Split('/');
                            string[] vertex3 = currentLine[3].Split('/');

                            ProcessVertex(vertex1, vertices, indices);
                            ProcessVertex(vertex2, vertices, indices);
                            ProcessVertex(vertex3, vertices, indices);

                            line = rdr.ReadLine();
                        }
                    }
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }

            RemoveUnusedVertices(vertices);
            float[] verticesArray = new float[vertices.Count * 3];
            float[] normalsArray = new float[vertices.Count * 3];
            float[] textureArray = new float[vertices.Count * 2];
            float furthest = convertDataToArrays(vertices, textures, normals, verticesArray, textureArray, normalsArray);
            uint[] indicesArray = ConvertIndicesListToArray(indices);

            JModelData modelData = new JModelData(verticesArray, textureArray, normalsArray, indicesArray, furthest);
            return modelData;
        }

        private static void ProcessVertex(string[] vertex,List<JVertex> vertices,List<uint> indices)
        {
            uint index = uint.Parse(vertex[0]) - 1;
            JVertex currentVertex = vertices[(int) index];

            int textureIndex = int.Parse(vertex[1]) - 1;
            int normalIndex = int.Parse(vertex[2]) - 1;
            if (!currentVertex.IsSet)
            {
                currentVertex.TextureIndex = textureIndex;
                currentVertex.NormalIndex = normalIndex;
                indices.Add(index);
            }
            else
            {
                DealWithAlreadyProcessedVertex(currentVertex, textureIndex, normalIndex, indices, vertices);
            }
        }

        private static void DealWithAlreadyProcessedVertex(JVertex previousVertex,int newTextureIndex,int newNormalIndex, List<uint> indices,List<JVertex> vertices)
        {
            if (previousVertex.HasSameTextureAndNormal(newTextureIndex, newNormalIndex))
            {
                indices.Add(previousVertex.Index);
            }
            else
            {
                JVertex anotherVertex = previousVertex.DuplicateVertex;
                if (anotherVertex != null)
                {
                    DealWithAlreadyProcessedVertex(anotherVertex, newTextureIndex, newNormalIndex, indices, vertices);
                }
                else
                {
                    JVertex duplicateVertex = new JVertex((uint)vertices.Count, previousVertex.Position);
                    duplicateVertex.TextureIndex = newTextureIndex;
                    duplicateVertex.NormalIndex = newNormalIndex;
                    previousVertex.DuplicateVertex = duplicateVertex;
                    vertices.Add(duplicateVertex);
                    indices.Add(duplicateVertex.Index);
                }
            }
        }

        private static void RemoveUnusedVertices(List<JVertex> vertices)
        {
            foreach(JVertex vertex in vertices)
            {
                if(!vertex.IsSet)
                {
                    vertex.TextureIndex = 0;
                    vertex.NormalIndex = 0;
                }
            }
        }

        private static uint[] ConvertIndicesListToArray(List<uint> indices)
        {
            uint[] indicesArray = new uint[indices.Count];
            for(int i = 0; i < indicesArray.Length; i++)
            {
                indicesArray[i] = indices[i];
            }
            return indicesArray;
        }

        private static float convertDataToArrays(List<JVertex> vertices,List<Vector2> textures,List<Vector3> normals,float[] verticesArray,float[] texturesArray,float[] normalsArray)
        {
            float furthestPoint = 0;
            for(int i = 0;i < vertices.Count; i++)
            {
                JVertex currentVertex = vertices[i];
                if(currentVertex.Length > furthestPoint)
                {
                    furthestPoint = currentVertex.Length;
                }

                Vector3 position = currentVertex.Position;
                Vector2 texture = textures[currentVertex.TextureIndex];
                Vector3 normal = normals[currentVertex.NormalIndex];

                verticesArray[i * 3] = position.X;
                verticesArray[i * 3 + 1] = position.Y;
                verticesArray[i * 3 + 2] = position.Z;
                texturesArray[i * 2] = texture.X;
                texturesArray[i * 2 + 1] = 1 - texture.Y;
                normalsArray[i * 3] = normal.X;
                normalsArray[i * 3 + 1] = normal.Y;
                normalsArray[i * 3 + 2] = normal.Z;
            }

            return furthestPoint;
        }
    }
}
