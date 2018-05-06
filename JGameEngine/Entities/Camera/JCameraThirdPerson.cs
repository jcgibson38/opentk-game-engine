using JGameEngine.Utils;
using OpenTK;
using OpenTK.Input;
using System;

namespace JGameEngine.Entities.Camera

{
    public class JCameraThirdPerson
    {
        /// <summary>
        /// The distance of the Camera from the Player which it is following, in junits.
        /// </summary>
        private float DistanceFromPlayer { get; set; }

        /// <summary>
        /// The angle of the Camera around the Player which it is following, in radians.
        /// </summary>
        private float AngleAroundPlayer { get; set; }

        /// <summary>
        /// The position of the Mouse Wheel from the previous frame.
        /// </summary>
        private float MouseWheelPreviousPosition { get; set; }

        /// <summary>
        /// The Y-Position of the Mouse Cursor from the previous frame.
        /// </summary>
        private float MouseYPreviousPosition { get; set; }

        /// <summary>
        /// The X-Position of the Mouse Cursor from the previous frame.
        /// </summary>
        private float MouseXPreviousPosition { get; set; }

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

        /// <summary>
        /// The Player object which the Camera is going to follow.
        /// </summary>
        private JEntity Player { get; set; }

        /// <summary>
        /// The horizontal distance of the Camera from the Player, in junits.
        /// </summary>
        private float HorizontalDistanceFromPlayer
        {
            get
            {
                return (float)(DistanceFromPlayer * Math.Cos(Pitch));
            }
        }

        /// <summary>
        /// The vertical distance of the Camera from the Player, in junits.
        /// </summary>
        private float VerticalDisanceFromPlayer
        {
            get
            {
                return (float)(DistanceFromPlayer * Math.Sin(Pitch));
            }
        }

        public JCameraThirdPerson(JEntity player)
        {
            MouseWheelPreviousPosition = 0;
            MouseYPreviousPosition = 0;
            MouseXPreviousPosition = 0;

            Pitch = 0;
            Yaw = 0;
            Roll = 0;

            DistanceFromPlayer = 35;
            AngleAroundPlayer = 0;

            Player = player;
            Position = new Vector3(10,10,10);
        }

        /// <summary>
        /// Calculate the change in Position and Rotation of the Camera from the previous frame.
        /// </summary>
        public void Move()
        {
            UpdateZoom();
            UpdatePitch();
            UpdateAngleAroundPlayer();
            UpdateCameraPosition();
            //Yaw = JMathUtils.ToRadians(180) + Player.RotY + AngleAroundPlayer;
        }

        /// <summary>
        /// Update the Zoom of the camera from the previous frame.
        /// </summary>
        private void UpdateZoom()
        {
            MouseState mouse = Mouse.GetState();
            float mouseWheelCurrentPosition = mouse.WheelPrecise;
            float mouseWheelDeltaPosition = mouseWheelCurrentPosition - MouseWheelPreviousPosition;

            float zoomAmount = mouseWheelDeltaPosition * 1.5f;
            DistanceFromPlayer -= zoomAmount;
            
            MouseWheelPreviousPosition = mouseWheelCurrentPosition;
        }

        /// <summary>
        /// Update the Pitch of the Camera from the previous frame.
        /// </summary>
        private void UpdatePitch()
        {
            MouseState mouse = Mouse.GetState();
            float mouseYCurrentPosition = mouse.Y;

            if (mouse.IsButtonDown(MouseButton.Right))
            {
                float mouseYDeltaPosition = mouseYCurrentPosition - MouseYPreviousPosition;

                float pitchChange = mouseYDeltaPosition * 0.001f;
                Pitch += pitchChange;
            }
            MouseYPreviousPosition = mouseYCurrentPosition;
        }

        /// <summary>
        /// Update the AngleAroundPlayer of the Camera from the previous frame.
        /// </summary>
        private void UpdateAngleAroundPlayer()
        {
            MouseState mouse = Mouse.GetState();
            float mouseXCurrentPosition = mouse.X;

            if (mouse.IsButtonDown(MouseButton.Right))
            {
                float mouseXDeltaPosition = mouseXCurrentPosition - MouseXPreviousPosition;

                float angleChange = mouseXDeltaPosition * 0.001f;
                AngleAroundPlayer -= angleChange;
            }
            MouseXPreviousPosition = mouseXCurrentPosition;
        }

        /// <summary>
        /// Update the position of the camera for the upcoming frame.
        /// </summary>
        private void UpdateCameraPosition()
        {
            /*
            float theta = Player.RotY + AngleAroundPlayer;
            float offsetX = (float) (HorizontalDistanceFromPlayer * Math.Sin(theta));
            float offsetZ = (float) (HorizontalDistanceFromPlayer * Math.Cos(theta));
            Position = new Vector3(Player.Position.X - offsetX, Player.Position.Y + VerticalDisanceFromPlayer, Player.Position.Z - offsetZ);
            */
        }
    }
}
