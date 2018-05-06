using JGameEngine.Models;
using OpenTK;
using System;

namespace JGameEngine.Entities
{
    public class JEntity
    {
        /// <summary>
        /// The TexturedModel of the Entity.
        /// </summary>
        public JTexturedModel TexturedModel { get; set; }

        /// <summary>
        /// The Position of the Entity in x,y,z space. Values in junits.
        /// </summary>
        public Vector3 Position { get; set; }

        /// <summary>
        /// The vector indicating the orientation of the entity in x,y,z space. Replaces Rotx, Roty, anmd Rotz.
        /// </summary>
        public Vector3 Orientation { get; set; }

        /// <summary>
        /// The scale of the Entity.
        /// </summary>
        public float Scale { get; set; }

        public JEntity(JTexturedModel model, Vector3 position, Vector3 orientation, float scale)
        {
            TexturedModel = model;
            Position = position;
            Scale = scale;
            Orientation = orientation;
        }

        /// <summary>
        /// Increase the current position of the Entity.
        /// </summary>
        /// <param name="x">Amount by which to increase the Entities position along the x-axis.</param>
        /// <param name="y">Amount by which to increase the Entities position along the y-axis.</param>
        /// <param name="z">Amount by which to increase the Entities position along the z-axis.</param>
        public void IncreasePosition(float x, float y, float z)
        {
            Position += new Vector3(x, y, z);
        }

        /// <summary>
        /// Increase the current scale of the Entity.
        /// </summary>
        /// <param name="s">The amount by which to increase the Scale of the entity.</param>
        public void IncreaseScale(float s)
        {
            Scale += s;
        }
    }
}
