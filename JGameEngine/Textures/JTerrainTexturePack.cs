using JGameEngine.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JGameEngine.Textures
{
    public class JTerrainTexturePack
    {
        public List<JTerrainTexture> TerrainTextures { get; set; }
        public float[] TerrainHeights { get; set; }
        public int Count { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public JTerrainTexturePack()
        {
            TerrainTextures = new List<JTerrainTexture>();
            TerrainHeights = new float[8];
            Count = 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="terrainTexture">The texture to apply to the terrain.</param>
        /// <param name="height">A value in the range [0,1]. terrainTexture will be applied to the Terrain from the height of the preceeding terrainTexture until height.</param>
        public void AddTerrainTexture(JTerrainTexture terrainTexture,float height)
        {
            TerrainTextures.Add(terrainTexture);
            TerrainHeights[Count] = height * JConfig.MAX_TERRAIN_HEIGHT;
            Count++;
        }
    }
}
