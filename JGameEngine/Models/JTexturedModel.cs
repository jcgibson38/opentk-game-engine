using JGameEngine.Textures;

namespace JGameEngine.Models
{
    public class JTexturedModel
    {
        public JRawModel RawModel { get; private set; }
        public JModelTexture Texture { get; set; }

        public JTexturedModel(JRawModel model, JModelTexture texture)
        {
            RawModel = model;
            Texture = texture;
        }
    }
}
