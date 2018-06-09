using JGameEngine.Models;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;

namespace JGameEngine.RenderEngine
{
    /// <summary>
    /// Load Models into memory.
    /// </summary>
    public class JLoader
    {
        private List<Int32> vaos = new List<Int32>();
        private List<Int32> vbos = new List<Int32>();
        private List<Int32> textures = new List<Int32>();

        public JRawModel LoadToVAO(float[] positions,float[] textureCoords, uint[] indices)
        {
            int vaoID = CreateVAO();
            bindIndicesBuffer(indices);
            storeDataInVAO(0, 3, positions);
            storeDataInVAO(1, 2, textureCoords);
            unbindVAO();

            return new JRawModel(vaoID, indices.Length);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="positions">1D array of vertex positions. Groups of 3 represent position in x,y,z space.</param>
        /// <param name="indices">1D array of indices into positions. Each index is a reference to a vertex position in positions.</param>
        /// <returns></returns>
        public JRawModel LoadToVAO(float[] positions,float[] textureCoords, float[] normals,uint[] indices)
        {
            int vaoID = CreateVAO();
            bindIndicesBuffer(indices);
            storeDataInVAO(0, 3, positions);
            storeDataInVAO(1, 2, textureCoords);
            storeDataInVAO(2, 3, normals);
            unbindVAO();

            return new JRawModel(vaoID, indices.Length);
        }

        public JRawModel LoadToVAO(float[] positions, int dimensions)
        {
            int vaoID = CreateVAO();
            this.storeDataInVAO(0, dimensions, positions);
            unbindVAO();
            return new JRawModel(vaoID, positions.Length / dimensions);
        }

        /// <summary>
        /// Create a VAO and get it's ID.
        /// </summary>
        /// <returns></returns>
        private int CreateVAO()
        {
            int vaoID;
            GL.GenVertexArrays(1, out vaoID);
            vaos.Add(vaoID);
            GL.BindVertexArray(vaoID);

            return vaoID;
        }

        /// <summary>
        /// Create a VBO and get it's ID.
        /// </summary>
        /// <returns></returns>
        private int CreateVBO()
        {
            int vboID;
            GL.GenBuffers(1, out vboID);
            vbos.Add(vboID);
            
            return vboID;
        }

        /// <summary>
        /// Store data into the VAO.
        /// </summary>
        /// <param name="vaoIndex"></param>
        /// <param name="data"></param>
        private void storeDataInVAO(int vaoIndex,int coordinateSize,float[] data)
        {
            int vboID = CreateVBO();
            GL.BindBuffer(BufferTarget.ArrayBuffer, vboID);
            GL.EnableVertexAttribArray(vaoIndex);
            GL.BufferData<float>(BufferTarget.ArrayBuffer, data.Length * sizeof(float), data, BufferUsageHint.StaticDraw);
            GL.VertexAttribPointer(vaoIndex, coordinateSize, VertexAttribPointerType.Float, true, 0, 0);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
        }

        /// <summary>
        /// Unbind VAO after use.
        /// </summary>
        private void unbindVAO()
        {
            GL.BindVertexArray(0);
        }

        /// <summary>
        /// Bind and buffer the index data.
        /// </summary>
        /// <param name="indices"></param>
        private void bindIndicesBuffer(uint[] indices)
        {
            int vboID = CreateVBO();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, vboID);
            GL.BufferData<uint>(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(uint), indices, BufferUsageHint.StaticDraw);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileName"></param>
        public int loadTexture(string fileName)
        {
            int textureID = GL.GenTexture();
            textures.Add(textureID);

            GL.BindTexture(TextureTarget.Texture2D, textureID);
            Bitmap bitmap = new Bitmap(fileName);
            BitmapData bitmapData = bitmap.LockBits(new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, bitmapData.Width, bitmapData.Height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, bitmapData.Scan0);
            bitmap.UnlockBits(bitmapData);
            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
            GL.TextureParameter((int)TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (float)TextureMinFilter.LinearMipmapLinear);
            GL.TextureParameter((int)TextureTarget.Texture2D, TextureParameterName.TextureLodBias, -0.4f);

            return textureID;
        }

        /// <summary>
        /// Delete any VAOs or VBOs which have been created.
        /// </summary>
        public void cleanUp()
        {
            foreach(int vao in vaos)
            {
                GL.DeleteVertexArray(vao);
            }

            foreach(int vbo in vbos)
            {
                GL.DeleteBuffer(vbo);
            }

            foreach(int texture in textures)
            {
                GL.DeleteTexture(texture);
            }
        }

        public int LoadBitmapTexture(float[,] noiseMap)
        {
            int textureID = GL.GenTexture();
            textures.Add(textureID);

            GL.BindTexture(TextureTarget.Texture2D, textureID);
            Bitmap b = new Bitmap(noiseMap.GetLength(0), noiseMap.GetLength(1), System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            for (int y = 0; y < b.Height; y++)
            {
                for (int x = 0; x < b.Width; x++)
                {
                    int rgb = (int)Lerp(0, 255, noiseMap[x, y]);
                    b.SetPixel(x, y, Color.FromArgb(rgb, rgb, rgb));
                }
            }

            BitmapData bitmapData = b.LockBits(new System.Drawing.Rectangle(0, 0, b.Width, b.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, bitmapData.Width, bitmapData.Height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, bitmapData.Scan0);
            b.UnlockBits(bitmapData);
            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
            GL.TextureParameter((int)TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (float)TextureMinFilter.LinearMipmapLinear);
            GL.TextureParameter((int)TextureTarget.Texture2D, TextureParameterName.TextureLodBias, -0.4f);

            return textureID;
        }

        public int LoadColorBitmapTexture(float[,] noiseMap)
        {
            int textureID = GL.GenTexture();
            textures.Add(textureID);

            GL.BindTexture(TextureTarget.Texture2D, textureID);
            Bitmap b = new Bitmap(noiseMap.GetLength(0), noiseMap.GetLength(1), System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            for (int y = 0; y < b.Height; y++)
            {
                for (int x = 0; x < b.Width; x++)
                {
                    b.SetPixel(x, y, GetColor(noiseMap[x,y]));
                }
            }

            BitmapData bitmapData = b.LockBits(new System.Drawing.Rectangle(0, 0, b.Width, b.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, bitmapData.Width, bitmapData.Height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, bitmapData.Scan0);
            b.UnlockBits(bitmapData);
            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
            GL.TextureParameter((int)TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (float)TextureMinFilter.LinearMipmapLinear);
            GL.TextureParameter((int)TextureTarget.Texture2D, TextureParameterName.TextureLodBias, -0.4f);

            return textureID;
        }

        private Color GetColor(float height)
        {
            if(height < 0.2)
            {
                int r = 11;
                int g = 76;
                int b = 93;
                return Color.FromArgb(r, g, b);
            }
            else if(height < 0.3)
            {
                int r = 109;
                int g = 182;
                int b = 169;
                return Color.FromArgb(r, g, b);
            }
            else if(height < 0.35)
            {
                int r = 204;
                int g = 169;
                int b = 140;
                return Color.FromArgb(r, g, b);
            }
            else if(height < 0.55)
            {
                int r = 122;
                int g = 137;
                int b = 70;
                return Color.FromArgb(r, g, b);
            }
            else if (height < 0.65)
            {
                int r = 68;
                int g = 95;
                int b = 26;
                return Color.FromArgb(r, g, b);
            }
            else if(height < 0.80)
            {
                int r = 113;
                int g = 85;
                int b = 74;
                return Color.FromArgb(r, g, b);
            }
            else if (height < 0.85)
            {
                int r = 110;
                int g = 96;
                int b = 96;
                return Color.FromArgb(r, g, b);
            }
            else
            {
                int r = 241;
                int g = 245;
                int b = 255;
                return Color.FromArgb(r, g, b);
            }
        }

        private float Lerp(int low, int high, float w)
        {
            return (1.0f - w) * low + w * high;
        }
    }
}
