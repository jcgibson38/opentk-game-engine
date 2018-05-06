using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Input;
using OpenTK;

namespace JGameEngine.Entities.Camera
{
    class JCameraFree : JCamera
    {
        /// <summary>
        /// The position of the Mouse Wheel from the previous frame.
        /// </summary>
        private float MouseWheelPreviousPosition { get; set; }

        public JCameraFree() : base()
        {
            Pitch = 0.3f;
            MouseWheelPreviousPosition = 0f;

            Position = new Vector3(10, 30, 10);
        }

        public override void Move()
        {
            KeyboardState keyState = Keyboard.GetState();
            if (keyState.IsKeyDown(Key.W))
            {
                Position -= new Vector3(0, 0, 10f);
            }
            if (keyState.IsKeyDown(Key.S))
            {
                Position += new Vector3(0, 0, 10f);
            }
            if (keyState.IsKeyDown(Key.D))
            {
                Position += new Vector3(10f, 0, 0);
            }
            if (keyState.IsKeyDown(Key.A))
            {
                Position -= new Vector3(10f, 0, 0);
            }

            UpdateZoom();
        }

        /// <summary>
        /// Update the Zoom of the camera from the previous frame.
        /// </summary>
        private void UpdateZoom()
        {
            MouseState mouse = Mouse.GetState();
            float mouseWheelCurrentPosition = mouse.WheelPrecise;
            float mouseWheelDeltaPosition = mouseWheelCurrentPosition - MouseWheelPreviousPosition;

            float zoomAmount = mouseWheelDeltaPosition * 20f;
            Position += new Vector3(zoomAmount * (float)Math.Sin(Yaw), -zoomAmount * (float)Math.Sin(Pitch), -zoomAmount * (float)Math.Cos(Pitch));

            MouseWheelPreviousPosition = mouseWheelCurrentPosition;
        }
    }
}
