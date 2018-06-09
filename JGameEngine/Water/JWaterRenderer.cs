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
        public JRawModel WaterQuad { get; set; }
        private JWaterShader WaterShader { get; set; }

        public JWaterRenderer(JLoader loader, JWaterShader waterShader, Matrix4 projectionMatrix)
        {
            this.WaterShader = waterShader;
            WaterShader.start();
            WaterShader.LoadProjectionMatrix(projectionMatrix);
            WaterShader.stop();
            SetupVAO(loader);
        }

        public void Render(List<JWaterTile> waterTiles, JCamera camera)
        {
            PrepareRender(camera);
            foreach(JWaterTile tile in waterTiles)
            {
                Matrix4 modelMatrix = JMathUtils.createTransformationMatrix(new Vector3(tile.X, tile.Height, tile.Z), 0, 0, 0, JWaterTile.TILE_SIZE);
                WaterShader.LoadModelMatrix(modelMatrix);
                GL.DrawArrays(PrimitiveType.Triangles, 0, WaterQuad.vertexCount);
            }
            Unbind();
        }

        private void PrepareRender(JCamera camera)
        {
            WaterShader.start();
            WaterShader.LoadViewMatrix(camera);
            GL.BindVertexArray(WaterQuad.vaoID);
            GL.EnableVertexAttribArray(0);
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
