using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JGameEngine.Textures
{
    public class JModelTexture
    {
        public int TextureID { get; private set; }

        public float ShineDamper { get; set; }
        public float Reflectivity { get; set; }

        public JModelTexture(int textureID)
        {
            TextureID = textureID;
            Reflectivity = 1;
            ShineDamper = 10;
        }
    }
}
