using JGameEngine.Entities.Collision;
using JGameEngine.Models;
using JGameEngine.RenderEngine;
using JGameEngine.Terrains;
using JGameEngine.Utils;
using OpenTK;
using System;

namespace JGameEngine.Entities
{
    class JBoundedEntity : JEntity
    {
        /// <summary>
        /// The turn speed of the Player when rotating. Units of radians/second.
        /// </summary>
        public static readonly float TURN_SPEED = 7.2f;
        /// <summary>
        /// The JBoundingSphere attached to the JBoundedEntity.
        /// </summary>
        public JBoundingSphere BoundingSphere { get; set; }

        /// <summary>
        /// The (x,y,z) location for the JBoundedEntity to move to.
        /// </summary>
        public JVector3 Destination { get; set; }

        /// <summary>
        /// Indicator of whether JBoundedEntity has reached Destination.
        /// </summary>
        private bool IsAtDestination { get; set; }

        /// <summary>
        /// Indicates whether the JBooundedEntity is selected.
        /// </summary>
        public bool IsSelected { get; set; }

        /// <summary>
        /// The current speed of the Player. Units of junits/second.
        /// </summary>
        private float currentSpeed = 0;

        /// <summary>
        /// The current turn speed of the Player. Units of radians/second.
        /// </summary>
        public float currentTurnSpeed = 0;

        /// <summary>
        /// The current vertical speed of the Player. Units of junits/second.
        /// </summary>
        private float upwardSpeed = 0;

        /// <summary>
        /// Indicates whether the Player is currently in the air or on the ground.
        /// </summary>
        private bool isInAir = false;

        /// <summary>
        /// The speed of the Player when moving. Units of junits/second.
        /// </summary>
        private static readonly float RUN_SPEED = 15;

        /// <summary>
        /// The acceleration due to gravity. Units of junits/s^2.
        /// </summary>
        private static readonly float GRAVITY = -50;

        /// <summary>
        /// The vertical acceleration of the player when they jump. Units of junits/s^2.
        /// </summary>
        private static readonly float JUMP_POWER = 30;

        /// <summary>
        /// JBoundedEntity has a JBoundingSphere which allows selection of the JBoundedEntity with a mouse click.
        /// </summary>
        /// <param name="model"></param>
        /// <param name="position"></param>
        /// <param name="rotX"></param>
        /// <param name="rotY"></param>
        /// <param name="rotZ"></param>
        /// <param name="scale"></param>
        /// <param name="loader"></param>
        public JBoundedEntity(JTexturedModel model, Vector3 position, Vector3 orientation, float scale, JLoader loader) : base(model,position,orientation,scale)
        {
            BoundingSphere = new JBoundingSphere();
            BoundingSphere.LoadSphereModel(loader);
            IsSelected = false;
            IsAtDestination = true;

            Destination = new JVector3();
        }

        /// <summary>
        /// Update the position of JBoundedEntity each frame and update the position of JBoundingSphere to match.
        /// </summary>
        /// <param name="terrain"></param>
        public void Move(JPerlinTerrain terrain)
        {
            if (Destination.IsSet)
            {
                DistanceToDestination();

                if (!IsAtDestination)
                {
                    currentSpeed = RUN_SPEED;
                }
                else
                {
                    currentSpeed = 0;
                    IsAtDestination = true;
                    Destination.IsSet = false;
                }
            }

            Translate(terrain);
            BoundingSphere.SphereEntity.Position = new Vector3(Position.X, Position.Y + 1, Position.Z);
        }

        /// <summary>
        /// Set a new destination for JBoundedEntity. Update JBoundedEntities Orientation to face Destination.
        /// </summary>
        /// <param name="destination">The (x,y,z) world coordinate for the JBoundedEntity to move to.</param>
        public void UpdateDestination(Vector3 destination)
        {
            Destination.Vector = destination;
            Destination.IsSet = true;

            // Update JBoundedEntities orientation to face the destination.
            Vector3 diff = Vector3.Subtract(Destination.Vector, Position);
            Orientation = diff;

            DistanceToDestination();
        }

        /// <summary>
        /// Change the JBoundingSphere texture to indicate it is currently selected.
        /// </summary>
        /// <param name="loader"></param>
        public void Select(JLoader loader)
        {
            BoundingSphere.Select(loader);
            IsSelected = true;
        }

        /// <summary>
        /// Change the JBoundingSphere texture to indicate it is not currently selected.
        /// </summary>
        /// <param name="loader"></param>
        public void DeSelect(JLoader loader)
        {
            BoundingSphere.DeSelect(loader);
            IsSelected = false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="terrain"></param>
        public void Translate(JPerlinTerrain terrain)
        {
            float distance = currentSpeed * JGameWindow.Delta;

            float dX = distance * (Orientation.X / Orientation.Length);
            float dZ = distance * (Orientation.Z / Orientation.Length);

            upwardSpeed += GRAVITY * JGameWindow.Delta;
            base.IncreasePosition(dX, upwardSpeed * JGameWindow.Delta, dZ);
            float terrainHeight = terrain.GetHeightOfTerrain(base.Position.X, base.Position.Z);

            if (base.Position.Y < terrainHeight)
            {
                upwardSpeed = 0;
                isInAir = false;
                base.Position = new Vector3(Position.X, terrainHeight, Position.Z);
            }
        }

        /// <summary>
        /// Calculate the distance between JBoundedEntities current Position and it's Destination.
        /// </summary>
        private void DistanceToDestination()
        {
            Vector2 p = new Vector2(Position.X,Position.Z);
            Vector2 d = new Vector2(Destination.Vector.X, Destination.Vector.Z);
            Vector2 diff = Vector2.Subtract(p, d);
            Vector3 distance = Vector3.Subtract(Position, Destination.Vector);

            if (diff.Length < 1.0)
            {
                IsAtDestination = true;
            }
            else
            {
                IsAtDestination = false;
            }
        }
    }
}
