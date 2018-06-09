using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JGameEngine.Water
{
    class JWaterTile
    {
        public static readonly float TILE_SIZE = 400;

        public float Height { get; set; }
        public float X { get; set; }
        public float Z { get; set; }

        public JWaterTile(float centerX, float centerZ, float height)
        {
            this.X = centerX;
            this.Z = centerZ;
            this.Height = height;
        }
    }
}
