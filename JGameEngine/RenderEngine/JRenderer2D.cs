using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JGameEngine.Models;
using OpenTK.Graphics.OpenGL;

namespace JGameEngine.RenderEngine
{
    /// <summary>
    /// Renders a JTexturedModel.
    /// </summary>
    class JRenderer2D
    {
        public void prepare()
        {
            GL.ClearColor(181.0f / 256.0f, 209.0f / 256.0f, 189.0f / 256.0f, 1);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
        }

        public void Render(JTexturedModel texturedModel)
        {
            JRawModel model = texturedModel.RawModel;
            GL.BindVertexArray(model.vaoID);
            GL.EnableVertexAttribArray(0);
            GL.EnableVertexAttribArray(1);
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, texturedModel.Texture.TextureID);
            GL.DrawElements(PrimitiveType.Triangles, model.vertexCount, DrawElementsType.UnsignedInt, 0);
            GL.DisableVertexAttribArray(0);
            GL.DisableVertexAttribArray(1);
            GL.BindVertexArray(0);
        }
    }
}
