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
            shader.LoadTextures();
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
            TerrainShader.loadShineVariables(1, 0);
        }

        private void BindTextures(JPerlinTerrain terrain)
        {
            JTerrainTexturePack texturePack = terrain.TexturePack;

            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, texturePack.TextureWaterDeep.TextureID);
            GL.ActiveTexture(TextureUnit.Texture1);
            GL.BindTexture(TextureTarget.Texture2D, texturePack.TextureWaterShallow.TextureID);
            GL.ActiveTexture(TextureUnit.Texture2);
            GL.BindTexture(TextureTarget.Texture2D, texturePack.TextureSand.TextureID);
            GL.ActiveTexture(TextureUnit.Texture3);
            GL.BindTexture(TextureTarget.Texture2D, texturePack.TextureGrassNatural.TextureID);
            GL.ActiveTexture(TextureUnit.Texture4);
            GL.BindTexture(TextureTarget.Texture2D, texturePack.TextureGrassLush.TextureID);
            GL.ActiveTexture(TextureUnit.Texture5);
            GL.BindTexture(TextureTarget.Texture2D, texturePack.TextureMountainNatural.TextureID);
            GL.ActiveTexture(TextureUnit.Texture6);
            GL.BindTexture(TextureTarget.Texture2D, texturePack.TextureMountainRocky.TextureID);
            GL.ActiveTexture(TextureUnit.Texture7);
            GL.BindTexture(TextureTarget.Texture2D, texturePack.TextureSnow.TextureID);
        }

        private void UnbindTerrainModel()
        {
            disableAttribArrays();
            unbindVAOs();
        }

        private void LoadModelMatrix(JPerlinTerrain terrain)
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
