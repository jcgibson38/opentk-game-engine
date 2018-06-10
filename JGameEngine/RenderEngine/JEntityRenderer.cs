using JGameEngine.Entities;
using JGameEngine.Models;
using JGameEngine.Shaders;
using JGameEngine.Textures;
using JGameEngine.Utils;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Collections.Generic;

namespace JGameEngine.RenderEngine
{
    class JEntityRenderer
    {
        public JStaticShader Shader { get; set; }

        public JEntityRenderer(JStaticShader shader,Matrix4 projectionMatrix)
        {
            this.Shader = shader;
            shader.start();
            shader.LoadProjectionMatrix(projectionMatrix);
            shader.stop();
        }

        public void render(Dictionary<JTexturedModel, List<JEntity>> entities)
        {
            foreach (JTexturedModel texturedModel in entities.Keys)
            {
                prepareTexturedModel(texturedModel);
                List<JEntity> batch = entities[texturedModel];
                foreach(JEntity entity in batch)
                {
                    prepareInstance(entity);
                    GL.DrawElements(PrimitiveType.Triangles, texturedModel.RawModel.vertexCount, DrawElementsType.UnsignedInt, 0);
                }
                unbindTexturedModel();
            }
        }

        private void prepareTexturedModel(JTexturedModel texturedModel)
        {
            JRawModel rawModel = texturedModel.RawModel;
            GL.BindVertexArray(rawModel.vaoID);
            enableAttribArrays();
            JModelTexture texture = texturedModel.Texture;
            Shader.LoadShineVariables(texture.ShineDamper, texture.Reflectivity);
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, texturedModel.Texture.TextureID);
        }

        private void unbindTexturedModel()
        {
            disableAttribArrays();
            unbindVAOs();
        }

        private void prepareInstance(JEntity entity)
        {
            //Matrix4 transformationMatrix = JMathUtils.createTransformationMatrix(entity.Position, entity.RotX, entity.RotY, entity.RotZ, entity.Scale);
            //JMathUtils.testTransformationMatrix(entity.Position, entity.Orientation, entity.Scale);
            Matrix4 transformationMatrix = JMathUtils.testTransformationMatrix(entity.Position, entity.Orientation, entity.Scale);
            Shader.LoadTransformationMatrix(transformationMatrix);
        }

        #region VAOControl

        /// <summary>
        /// 
        /// </summary>
        private void enableAttribArrays()
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
