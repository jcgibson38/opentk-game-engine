using OpenTK;

namespace JGameEngine.Entities
{
    /// <summary>
    /// Represents a lightsource in the world.
    /// </summary>
    public class JLight
    {
        /// <summary>
        /// The position of the Light.
        /// </summary>
        public Vector3 Position { get; set; }

        /// <summary>
        /// The Color of the Light.
        /// </summary>
        public Vector3 Color { get; set; }

        /// <summary>
        /// Create a Light instance.
        /// </summary>
        /// <param name="position">The position of the Light in x,y,z space. Units in junits.</param>
        /// <param name="color">The r,g,b color of the Light. Values in the range [0,1.0].</param>
        public JLight(Vector3 position, Vector3 color)
        {
            this.Position = position;
            this.Color = color;
        }
    }
}
