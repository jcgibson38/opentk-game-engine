using JGameEngine.Models;
using JGameEngine.Shaders;
using JGameEngine.Terrains;
using JGameEngine.Textures;
using JGameEngine.Utils;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Collections.Generic;

namespace JGameEngine.RenderEngine
{
    public class JTerrainRenderer
    {
        private JTerrainShader TerrainShader { get; set; }

        public JTerrainRenderer(JTerrainShader shader,Matrix4 projectionMatrix)
        {
            TerrainShader = shader;
            shader.start();
            shader.loadProjectionMatrix(projectionMatrix);
            shader.stop();
        }

        public void Render(List<JTerrain> terrains)
        {
            foreach(JTerrain terrain in terrains)
            {
                PrepareTerrainModel(terrain);
                LoadModelMatrix(terrain);
                GL.DrawElements(PrimitiveType.Triangles, terrain.TerrainModel.vertexCount, DrawElementsType.UnsignedInt, 0);
                UnbindTerrainModel();
            }
        }

        private void PrepareTerrainModel(JTerrain terrain)
        {
            JRawModel rawModel = terrain.TerrainModel;
            GL.BindVertexArray(rawModel.vaoID);
            EnableAttribArrays();
            JModelTexture texture = terrain.TerrainTexture;
            TerrainShader.loadShineVariables(texture.ShineDamper, texture.Reflectivity);
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, terrain.TerrainTexture.TextureID);
        }

        private void UnbindTerrainModel()
        {
            disableAttribArrays();
            unbindVAOs();
        }

        private void LoadModelMatrix(JTerrain terrain)
        {
            Matrix4 transformationMatrix = JMathUtils.createTransformationMatrix(new Vector3(terrain.X,0,terrain.Z), 0, 0, 0, 1);
            TerrainShader.loadTransformationMatrix(transformationMatrix);
        }

        #region VAOControl

        /// <summary>
        /// 
        /// </summary>
        private void EnableAttribArrays()
        {
            GL.EnableVertexAttribArray(0);
            GL.EnableVertexAttribArray(1);
            GL.EnableVertexAttribArray(2);
        }

        /// <summary>
        /// 
        /// </summary>
        private void disableAttribArrays()
        {
            GL.DisableVertexAttribArray(0);
            GL.DisableVertexAttribArray(1);
            GL.DisableVertexAttribArray(2);
        }

        /// <summary>
        /// 
        /// </summary>
        private void unbindVAOs()
        {
            GL.BindVertexArray(0);
        }

        #endregion VAOControl
    }
}
