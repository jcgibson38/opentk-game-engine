using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JGameEngine.Entities.Camera
{
    public abstract class JCamera
    {
        /// <summary>
        /// The position of the Camera in x,y,z space. Values in junits.
        /// </summary>
        public Vector3 Position { get; set; }

        /// <summary>
        /// The rotation of the Camera around the X-Axis, in radians.
        /// </summary>
        public float Pitch { get; set; }

        /// <summary>
        /// The rotation of the Camera around the Y-Axis, in radians.
        /// </summary>
        public float Yaw { get; set; }

        /// <summary>
        /// The rotation of the Camera around the Z-Axis, in radians.
        /// </summary>
        public float Roll { get; set; }

        public JCamera()
        {
            Pitch = 0;
            Yaw = 0;
            Roll = 0;

            Position = new Vector3(10, 10, 10);
        }

        public abstract void Move();
    }
}
