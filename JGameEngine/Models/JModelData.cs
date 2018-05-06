using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JGameEngine.Models
{
    public class JModelData
    {
        public float[] Vertices { get; private set; }
        public float[] TextureCoords { get; private set; }
        public float[] Normals { get; private set; }
        public uint[] Indices { get; private set; }
        public float FurthestPoint { get; private set; }

        public JModelData(float[] vertices, float[] textureCoords, float[] normals, uint[] indices, float furthestPoint)
        {
            Vertices = vertices;
            TextureCoords = textureCoords;
            Normals = normals;
            Indices = indices;
            FurthestPoint = furthestPoint;
        }
    }
}
