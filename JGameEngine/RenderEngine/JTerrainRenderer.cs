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

        public JTerrainRenderer(JTerrainShader shader,Matrix4 projectionMatrix,JTerrainTexturePack texturePack)
        {
            TerrainShader = shader;
            shader.start();
            shader.LoadProjectionMatrix(projectionMatrix);
            shader.LoadTextures(texturePack.TerrainHeights, texturePack.Count);
            shader.stop();
        }

        public void Render(List<JPerlinTerrain> terrains)
        {
            foreach(JPerlinTerrain terrain in terrains)
            {
                PrepareTerrainModel(terrain);
                LoadModelMatrix(terrain);
                GL.DrawElements(PrimitiveType.Triangles, terrain.TerrainModel.vertexCount, DrawElementsType.UnsignedInt, 0);
                UnbindTerrainModel();
            }
        }

        private void PrepareTerrainModel(JPerlinTerrain terrain)
        {
            JRawModel rawModel = terrain.TerrainModel;
            GL.BindVertexArray(rawModel.vaoID);
            EnableAttribArrays();
            BindTextures(terrain);
            TerrainShader.LoadShineVariables(1, 0);
        }

        private void BindTextures(JPerlinTerrain terrain)
        {
            JTerrainTexturePack texturePack = terrain.TexturePack;

            TextureUnit texture = TextureUnit.Texture0;

            for (int i = 0; i < texturePack.TerrainTextures.Count; i++)
            {
                GL.ActiveTexture(texture);
                GL.BindTexture(TextureTarget.Texture2D, texturePack.TerrainTextures[i].TextureID);
                texture++;
            }
        }

        private void UnbindTerrainModel()
        {
            disableAttribArrays();
            unbindVAOs();
        }

        private void LoadModelMatrix(JPerlinTerrain terrain)
        {
            Matrix4 transformationMatrix = JMathUtils.createTransformationMatrix(new Vector3(terrain.X,0,terrain.Z), 0, 0, 0, 1);
            TerrainShader.LoadTransformationMatrix(transformationMatrix);
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
