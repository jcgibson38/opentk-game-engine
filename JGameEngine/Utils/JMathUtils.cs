using JGameEngine.Entities.Camera;
using OpenTK;
using System;

namespace JGameEngine.Utils
{
    class JMathUtils
    {
        public static Matrix4 createTransformationMatrix(Vector3 translation, float rx, float ry, float rz, float scale)
        {
            Matrix4 returnMatrix = Matrix4.Transpose(Matrix4.CreateTranslation(translation)) * Matrix4.CreateRotationX(rx) * Matrix4.CreateRotationY(-ry) * Matrix4.CreateRotationZ(rz) * Matrix4.CreateScale(scale);

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
                                     0,     1,          0,              0,
                                     0,     0,          1,              0,
                                     0,     0,          0,              1);

            Matrix4 Ry = new Matrix4(cosThetaY,     0,     sinThetaY,       0,
                                     0,             1,      0,              0,
                                     -sinThetaY,    0,     cosThetaY,       0,
                                     0,             0,      0,              1);

            Matrix4 Rz = new Matrix4(1,             0,              0,  0,
                                     0,             1,              0,  0,
                                     0,             0,              1,  0,
                                     0,             0,              0,  1);

            returnMatrix = returnMatrix * Rx * Ry * Rz;
            returnMatrix = returnMatrix * Matrix4.CreateScale(scale);

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

        /// <summary>
        /// Returns a value within the range of [a,b] by linearly interpolating w. Assuming that w is in the range [0,1].
        /// Lerp(10.0f,30.0f,0.5f) = 20.0f
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="w"></param>
        /// <returns></returns>
        public static float Lerp(float a,float b, float w)
        {
            return (1.0f - w) * a + w * b;
        }

        /// <summary>
        /// Returns a value within the range of [a,b] by linearly interpolating w. Assuming that w is in the range [0,1].
        /// Lerp(10.0f,30.0f,0.5f) = 20.0f
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="w"></param>
        /// <returns></returns>
        public static double Lerp(double a, double b, double w)
        {
            return (1.0f - w) * a + w * b;
        }

        /// <summary>
        /// Returns a value in the range [0,1] which represents the linear parameter that would produce the interpolant value within the range [a,b].
        /// InvLerp(10.0f,30.0f,20.0f) = 0.5f
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="v"></param>
        /// <returns></returns>
        public static float InvLerp(float a, float b, float v)
        {
            return (v - a) / (-a + b);
        }

        /// <summary>
        /// A cubic function to transform the Terrain Height for a JPerlinTerrain.
        /// </summary>
        /// <param name="x"></param>
        /// <returns>0.6343434f * x - 1.742424f * x^2 + 2.474747f * x^3</returns>
        public static float HeightCurve(float x)
        {
            return 0.6343434f * x - 1.742424f * x * x + 2.474747f * x * x * x;
        }

        public static float HeightCurve2(float x)
        {
            float retVal = -0.3f + 3.929681f * x - 12.85256f * x *x + 18.57032f * x * x * x - 8.449883f * x * x * x * x;
            if (retVal < 0)
            {
                retVal = 0;
            }else if(retVal > 1.0f)
            {
                retVal = 1.0f;
            }

            return retVal;
        }
    }
}
