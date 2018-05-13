using JGameEngine.Config;
using JGameEngine.Entities;
using JGameEngine.Entities.Camera;
using JGameEngine.RenderEngine;
using JGameEngine.Terrains;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JGameEngine.Utils
{
    class JMousePicker
    {
        /// <summary>
        /// The unit vector of the Ray projected from the mouse cursor into the World Coordinate system.
        /// </summary>
        public Vector3 CurrentRay { get; set; }

        /// <summary>
        /// The (x,y,z) intersection point of CurrentRay with Terrain.
        /// </summary>
        public Vector3 CurrentTerrainPoint { get; set; }

        /// <summary>
        /// The JMasterRenderers Projection Matrix based off of the JCameras properties.
        /// </summary>
        private Matrix4 ProjectionMatrix { get; set; }
        
        /// <summary>
        /// The View Matrix created form the JCamera.
        /// </summary>
        private Matrix4 ViewMatrix { get; set; }

        /// <summary>
        /// The JCamera entity.
        /// </summary>
        private JCamera Camera { get; set; }

        /// <summary>
        /// The JTerrain the Mouse Ray will intersect with.
        /// </summary>
        private JPerlinTerrain Terrain { get; set; }
        
        /// <summary>
        /// The active JGameWindow. We need this to get it's height and width.
        /// </summary>
        private JGameWindow GameWindow { get; set; }

        /// <summary>
        /// Create a JMousePicker instance. JMousePicker will project a 2D screen coordinate into a 3D unit vector within the world.
        /// It can determine intersection with terrain as well as check for intersection with a JBoundingSphere.
        /// </summary>
        /// <param name="camera">The JCamera entity.</param>
        /// <param name="projectionMatrix">The JMasterRenderers Projection Matrix based off of the JCameras properties.</param>
        /// <param name="terrain">The JTerrain the Mouse Ray will intersect with.</param>
        /// <param name="gameWindow">The active JGameWindow. We need this to get it's height and width.</param>
        public JMousePicker(JCamera camera,Matrix4 projectionMatrix, JPerlinTerrain terrain, JGameWindow gameWindow)
        {
            Camera = camera;
            ProjectionMatrix = projectionMatrix;
            ViewMatrix = JMathUtils.createViewMatrix(camera);
            Terrain = terrain;
            GameWindow = gameWindow;
        }

        /// <summary>
        /// Update CurrentRay each frame from the current 2D mouse coordinate. Also updates CurrentTerrainPoint reflecting CurrentRays intersection with
        /// Terrain.
        /// </summary>
        public void Update()
        {
            ViewMatrix = JMathUtils.createViewMatrix(Camera);
            CurrentRay = CalculateMouseRay();
            if (IntersectionInRange(0, JConfig.MOUSE_PICKER_RAY_RANGE, CurrentRay))
            {
                CurrentTerrainPoint = BinarySearch(0, 0, JConfig.MOUSE_PICKER_RAY_RANGE, CurrentRay);
            }
            else
            {
                CurrentTerrainPoint = new Vector3(0,0,0);
            }
        }

        /// <summary>
        /// Use the current 2D position of the mouse cursor to determine the 3D unit vector it projects into the world.
        /// </summary>
        /// <returns></returns>
        private Vector3 CalculateMouseRay()
        {
            float mouseX = GameWindow.Mouse.X;
            float mouseY = GameWindow.Mouse.Y;

            Vector2 normalizedDeviceCoords = GetNormalizedDeviceCoords(mouseX, mouseY);

            Vector4 clipCoords = new Vector4(normalizedDeviceCoords.X, normalizedDeviceCoords.Y, -1f, 1f);

            Vector4 eyeCoords = ToEyeCoords(clipCoords);

            Vector3 worldRay = ToWorldCoords(eyeCoords);

            return worldRay;
        }

        /// <summary>
        /// Normalize the current mouse coordinates based on GameWindows height and width.
        /// </summary>
        /// <param name="mouseX"></param>
        /// <param name="mouseY"></param>
        /// <returns></returns>
        private Vector2 GetNormalizedDeviceCoords(float mouseX,float mouseY)
        {

            float x = (2f * mouseX) / GameWindow.Width - 1f;
            float y = (2f * mouseY) / GameWindow.Height - 1f;

            return new Vector2(x, y);
        }

        /// <summary>
        /// Translate the Clip-space coordinates into eye-space coordinates.
        /// </summary>
        /// <param name="clipCoords"></param>
        /// <returns></returns>
        private Vector4 ToEyeCoords(Vector4 clipCoords)
        {
            Matrix4 invertedProjection = Matrix4.Invert(ProjectionMatrix);
            Vector4 eyeCoords = invertedProjection * clipCoords;
            return new Vector4(eyeCoords.X, eyeCoords.Y, -1f, 0);
        }

        /// <summary>
        /// Translate eye-space coordinates into world-space coordinates.
        /// </summary>
        /// <param name="eyeCoords"></param>
        /// <returns></returns>
        private Vector3 ToWorldCoords(Vector4 eyeCoords)
        {
            Matrix4 invertedViewMatrix = Matrix4.Invert(ViewMatrix);
            Vector4 rayWorld = invertedViewMatrix * eyeCoords;
            Vector3 mouseRay = new Vector3(rayWorld.X, rayWorld.Y, rayWorld.Z);
            mouseRay.Normalize();
            return mouseRay;
        }

        /// <summary>
        /// Get the (x,y,z) point on ray at distance from it's origin.
        /// </summary>
        /// <param name="ray"></param>
        /// <param name="distance"></param>
        /// <returns></returns>
        private Vector3 GetPointOnRay(Vector3 ray,float distance)
        {
            Vector3 camPos = Camera.Position;
            Vector3 start = new Vector3(camPos.X, camPos.Y, camPos.Z);
            Vector3 scaledRay = new Vector3(ray.X * distance, -ray.Y * distance, ray.Z * distance);

            Vector3 test = Vector3.Add(start, scaledRay);
            return test;
        }

        /// <summary>
        /// Recursive binary search to determine rays (x,y,z) intersection point with Terrain.
        /// </summary>
        /// <param name="count"></param>
        /// <param name="start"></param>
        /// <param name="finish"></param>
        /// <param name="ray"></param>
        /// <returns></returns>
        private Vector3 BinarySearch(int count,float start,float finish,Vector3 ray)
        {
            float half = start + ((finish - start) / 2f);
            if (count >= JConfig.MOUSE_PICKER_RECURSION_COUNT)
            {
                Vector3 endPoint = GetPointOnRay(ray, half);
                JPerlinTerrain terrain = GetTerrain(endPoint.X,endPoint.Z);
                if(terrain != null)
                {
                    return endPoint;
                }
                else
                {
                    return new Vector3(0, 0, 0);
                }
            }

            if (IntersectionInRange(start, half, ray))
            {
                return BinarySearch(count + 1, start, half, ray);
            }
            else
            {
                return BinarySearch(count + 1, half, finish, ray);
            }
        }

        /// <summary>
        /// Given two points on the ray, determine whether Terrain falls between them.
        /// </summary>
        /// <param name="start"></param>
        /// <param name="finish"></param>
        /// <param name="ray"></param>
        /// <returns></returns>
        private Boolean IntersectionInRange(float start,float finish,Vector3 ray)
        {
            Vector3 startPoint = GetPointOnRay(ray, start);
            Vector3 endPoint = GetPointOnRay(ray, finish);
            if(!IsUnderGround(startPoint) && IsUnderGround(endPoint))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Determine whether point falls below Terrain.
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        private Boolean IsUnderGround(Vector3 point)
        {
            JPerlinTerrain terrain = GetTerrain(point.X, point.Z);
            float height = 0;
            if(terrain != null)
            {
                height = terrain.GetHeightOfTerrain(point.X, point.Z);
            }
            if(point.Y < height)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Given a 2D world-space coordinate, determine the nearest JTerrain.
        /// </summary>
        /// <param name="worldX"></param>
        /// <param name="worldZ"></param>
        /// <returns></returns>
        private JPerlinTerrain GetTerrain(float worldX,float worldZ)
        {
            return Terrain;
        }

        /// <summary>
        /// Check whether CurrentRay intersects with boundedEntity.
        /// </summary>
        /// <param name="boundedEntity"></param>
        /// <returns></returns>
        public Boolean CheckIntersection(JBoundedEntity boundedEntity)
        {
            Vector3 fixedCurrentRay = new Vector3(CurrentRay.X, -CurrentRay.Y, CurrentRay.Z);
            Vector3 OminusC = Vector3.Subtract(Camera.Position, boundedEntity.BoundingSphere.SphereEntity.Position);
            float b = Vector3.Dot(fixedCurrentRay, OminusC);
            float c = (float)(Vector3.Dot(OminusC, OminusC) - Math.Pow(boundedEntity.BoundingSphere.Radius,2));
            float prod = (float)(Math.Pow(b, 2) - c);
            if (prod >= 0)
            {
                return true;
            }
            return false;
        }
    }
}