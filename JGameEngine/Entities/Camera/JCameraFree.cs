﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Input;
using OpenTK;
using JGameEngine.Config;

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
            Yaw = 0;
            MouseWheelPreviousPosition = 0f;

            Position = new Vector3(10, 30, 10);
        }

        /// <summary>
        /// Update the Camera Position in 3D space.
        /// </summary>
        public override void Move()
        {
            KeyboardState keyState = Keyboard.GetState();

            if (keyState.IsKeyDown(Key.Q))
            {
                Yaw += 0.1f;
            }
            if (keyState.IsKeyDown(Key.E))
            {
                Yaw -= 0.1f;
            }

            float dz = JConfig.JCAMERA_MOVEMENT_SPEED * (float)Math.Cos(Yaw);
            float dx = JConfig.JCAMERA_MOVEMENT_SPEED * (float)Math.Sin(Yaw);

            float lz = dx;
            float lx = dz;

            if (keyState.IsKeyDown(Key.W))
            {
                Position += new Vector3(-dx, 0, -dz);
            }
            if (keyState.IsKeyDown(Key.S))
            {
                Position += new Vector3(dx, 0, dz);
            }
            if (keyState.IsKeyDown(Key.D))
            {
                Position += new Vector3(lx, 0, -lz);
            }
            if (keyState.IsKeyDown(Key.A))
            {
                Position += new Vector3(-lx, 0, lz);
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
            Position += new Vector3(-zoomAmount * (float)Math.Sin(Yaw), -zoomAmount * (float)Math.Sin(Pitch), -zoomAmount * (float)Math.Cos(Pitch) * (float)Math.Cos(Yaw));

            MouseWheelPreviousPosition = mouseWheelCurrentPosition;
        }
    }
}
