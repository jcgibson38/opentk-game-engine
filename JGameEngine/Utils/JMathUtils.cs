﻿using JGameEngine.Entities.Camera;
using OpenTK;
using System;

namespace JGameEngine.Utils
{
    class JMathUtils
    {
        public static Matrix4 createTransformationMatrix(Vector3 translation, float rx, float ry, float rz, float scale)
        {
            Matrix4 returnMatrix = Matrix4.Transpose(Matrix4.CreateTranslation(translation)) * Matrix4.CreateRotationX(rx) * Matrix4.CreateRotationY(-ry) * Matrix4.CreateRotationZ(rz) * Matrix4.CreateScale(scale);

            //Console.WriteLine("CreateTransformationMatrix: ");
            //Console.WriteLine(Matrix4.CreateRotationX(rx));
            //Console.WriteLine(Matrix4.CreateRotationY(-ry));
            //Console.WriteLine(Matrix4.CreateRotationZ(rz));
            //Console.WriteLine(returnMatrix);

            return Matrix4.Transpose(returnMatrix);
        }

        public static Matrix4 testTransformationMatrix(Vector3 translation,Vector3 orientation,float scale)
        {
            Matrix4 returnMatrix = Matrix4.Transpose(Matrix4.CreateTranslation(translation));

            float sinThetaX = orientation.Z / orientation.Length;
            float cosThetaX = orientation.Y / orientation.Length;

            float sinThetaY = orientation.X / orientation.Length;
            float cosThetaY = orientation.Z / orientation.Length;

            float sinThetaZ = orientation.Y / orientation.Length;
            float cosThetaZ = orientation.X / orientation.Length;

            Matrix4 Rx = new Matrix4(1,     0,          0,              0,
                                     0,     1,  0,     0,
                                     0,     0,  1,      0,
                                     0,     0,          0,              1);

            Matrix4 Ry = new Matrix4(cosThetaY,     0,     sinThetaY,       0,
                                     0,             1,      0,              0,
                                     -sinThetaY,    0,     cosThetaY,       0,
                                     0,             0,      0,              1);

            Matrix4 Rz = new Matrix4(1,     0,     0,  0,
                                     0,     1,      0,  0,
                                     0,             0,              1,  0,
                                     0,             0,              0,  1);

            returnMatrix = returnMatrix * Rx * Ry * Rz;
            returnMatrix = returnMatrix * Matrix4.CreateScale(scale);

            //Console.WriteLine("testTransformationMatrix: ");
            //Console.WriteLine(Rx);
            //Console.WriteLine(Ry);
            //Console.WriteLine(Rz);
            //Console.WriteLine(returnMatrix);

            return Matrix4.Transpose(returnMatrix);
        }

        public static Matrix4 createViewMatrix(JCamera camera)
        {
            Vector3 oppositeCameraTranslation = new Vector3(-camera.Position.X, -camera.Position.Y, -camera.Position.Z);
            Matrix4 returnMatrix = Matrix4.CreateRotationX(-camera.Pitch) * Matrix4.CreateRotationY(camera.Yaw) * Matrix4.Transpose(Matrix4.CreateTranslation(oppositeCameraTranslation));
            return Matrix4.Transpose(returnMatrix);
        }

        public static Matrix4 createProjectionMatrix(float fov, float near_plane, float far_plane)
        {
            float aspectRatio = (float)DisplayDevice.Default.Width / (float)DisplayDevice.Default.Height;
            float yScale = (1f / (float)Math.Tan(ToRadians(fov / 2f))) * aspectRatio;
            float xScale = yScale / aspectRatio;
            float frustrumLength = far_plane - near_plane;

            Matrix4 projectionMatrix = Matrix4.Identity;
            projectionMatrix.M11 = xScale;
            projectionMatrix.M22 = yScale;
            projectionMatrix.M33 = -((far_plane + near_plane) / frustrumLength);
            projectionMatrix.M34 = -1;
            projectionMatrix.M43 = -((2 * near_plane * far_plane) / frustrumLength);
            projectionMatrix.M44 = 0;

            return projectionMatrix;
        }

        public static float ToRadians(float deg)
        {
            return (float)(Math.PI / 180) * deg;
        }

        public static float BarryCentric(Vector3 p1, Vector3 p2, Vector3 p3, Vector2 pos)
        {
            float det = (p2.Z - p3.Z) * (p1.X - p3.X) + (p3.X - p2.X) * (p1.Z - p3.Z);
            float l1 = ((p2.Z - p3.Z) * (pos.X - p3.X) + (p3.X - p2.X) * (pos.Y - p3.Z)) / det;
            float l2 = ((p3.Z - p1.Z) * (pos.X - p3.X) + (p1.X - p3.X) * (pos.Y - p3.Z)) / det;
            float l3 = 1.0f - l1 - l2;
            return l1 * p1.Y + l2 * p2.Y + l3 * p3.Y;
        }
    }
}
