using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JGameEngine.Textures
{
    public class JTerrainTexturePack
    {
        public JTerrainTexture TextureWaterDeep { get; set; }
        public JTerrainTexture TextureWaterShallow { get; set; }
        public JTerrainTexture TextureSand { get; set; }
        public JTerrainTexture TextureGrassNatural { get; set; }
        public JTerrainTexture TextureGrassLush { get; set; }
        public JTerrainTexture TextureMountainNatural { get; set; }
        public JTerrainTexture TextureMountainRocky { get; set; }
        public JTerrainTexture TextureSnow { get; set; }

        public JTerrainTexturePack(JTerrainTexture textureWaterDeep, JTerrainTexture textureWaterShallow, JTerrainTexture textureSand, JTerrainTexture textureGrassNatural, JTerrainTexture textureGrassLush, JTerrainTexture textureMountainNatural, JTerrainTexture textureMountainRocky, JTerrainTexture textureSnow)
        {
            TextureWaterDeep = textureWaterDeep;
            TextureWaterShallow = textureWaterShallow;
            TextureSand = textureSand;
            TextureGrassNatural = textureGrassNatural;
            TextureGrassLush = textureGrassLush;
            TextureMountainNatural = textureMountainNatural;
            TextureMountainRocky = textureMountainRocky;
            TextureSnow = textureSnow;
        }
    }
}
