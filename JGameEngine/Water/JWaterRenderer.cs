using JGameEngine.Entities;
using JGameEngine.Entities.Camera;
using JGameEngine.Models;
using JGameEngine.RenderEngine;
using JGameEngine.Utils;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JGameEngine.Water
{
    class JWaterRenderer
    {
        private static string dudvMapTexture;
        private static int dudvMap;

        private static string normalMapTexture;
        private static int normalMap;

        private static readonly float DISTORTION_SPEED = 0.01f;
        private float distortionVariance;

        public JRawModel WaterQuad { get; set; }
        private JWaterShader WaterShader { get; set; }
        private JWaterFrameBuffer FrameBuffer { get; set; }
        private JWaterTile WaterTile { get; set; }

        public JWaterRenderer(JLoader loader, JWaterShader waterShader, Matrix4 projectionMatrix, JWaterFrameBuffer frameBuffer, JWaterTile waterTile)
        {
            this.WaterShader = waterShader;
            this.FrameBuffer = frameBuffer;
            this.WaterTile = waterTile;
            dudvMapTexture = JFileUtils.GetPathToResFile("waterDUDV.png");
            normalMapTexture = JFileUtils.GetPathToResFile("matchingNormalMap.png");
            dudvMap = loader.loadTexture(dudvMapTexture);
            normalMap = loader.loadTexture(normalMapTexture);
            distortionVariance = 0;
            WaterShader.start();
            WaterShader.LoadTextures();
            WaterShader.LoadProjectionMatrix(projectionMatrix);
            WaterShader.stop();
            SetupVAO(loader);
        }

        public void Render(List<JWaterTile> waterTiles, JCamera camera, JLight light)
        {
            PrepareRender(camera, light);
            foreach(JWaterTile tile in waterTiles)
            {
                Matrix4 modelMatrix = JMathUtils.createTransformationMatrix(new Vector3(tile.X, tile.Height, tile.Z), 0, 0, 0, JWaterTile.TILE_SIZE);
                WaterShader.LoadModelMatrix(modelMatrix);
                GL.DrawArrays(PrimitiveType.Triangles, 0, WaterQuad.vertexCount);
            }
            Unbind();
        }

        private void PrepareRender(JCamera camera, JLight light)
        {
            WaterShader.start();
            WaterShader.LoadViewMatrix(camera);

            distortionVariance += DISTORTION_SPEED * JGameWindow.FrameTimeSeconds();
            distortionVariance %= 1.0f;
            WaterShader.LoadDistortionVariance(distortionVariance);
            WaterShader.LoadLight(light);

            GL.BindVertexArray(WaterQuad.vaoID);
            GL.EnableVertexAttribArray(0);
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, FrameBuffer.ReflectionTexture);
            GL.ActiveTexture(TextureUnit.Texture1);
            GL.BindTexture(TextureTarget.Texture2D, FrameBuffer.RefractionTexture);
            GL.ActiveTexture(TextureUnit.Texture2);
            GL.BindTexture(TextureTarget.Texture2D, WaterTile.ColorTexture.TextureID);
            GL.ActiveTexture(TextureUnit.Texture3);
            GL.BindTexture(TextureTarget.Texture2D, dudvMap);
            GL.ActiveTexture(TextureUnit.Texture4);
            GL.BindTexture(TextureTarget.Texture2D, normalMap);
        }

        private void Unbind()
        {
            GL.DisableVertexAttribArray(0);
            GL.BindVertexArray(0);
            WaterShader.stop();
        }

        private void SetupVAO(JLoader loader)
        {
            // Just X and Z vertices. We will set Y to 0 everywhere for flat quad.
            float[] vertices = { -1, -1, -1, 1, 1, -1, 1, -1, -1, 1, 1, 1 };
            WaterQuad = loader.LoadToVAO(vertices, 2);
        }
    }
}
