using OpenTK;

namespace JGameEngine.RenderEngine
{
    public class JVertex
    {
        private static readonly int NO_INDEX = -1;

        public Vector3 Position { get; set; }
        public int TextureIndex { get; set; }
        public int NormalIndex { get; set; }
        public JVertex DuplicateVertex { get; set; }
        public uint Index { get; set; }
        public float Length { get; set; }

        public bool IsSet
        {
            get { return TextureIndex != NO_INDEX && NormalIndex != NO_INDEX; }
        }

        public JVertex(uint index, Vector3 position)
        {
            Index = index;
            Position = position;
            Length = position.Length;
        }

        public bool HasSameTextureAndNormal(int textureIndexOther,int normalIndexOther)
        {
            return textureIndexOther == TextureIndex && normalIndexOther == NormalIndex;
        }
    }
}
