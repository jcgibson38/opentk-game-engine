using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JGameEngine.RenderEngine;
using OpenTK.Graphics.OpenGL;

namespace JGameEngine.Water
{
    class JWaterFrameBuffer
    {
        private static readonly int REFLECTION_WIDTH = 1280;
        private static readonly int REFLECTION_HEIGHT = 720;

        private static readonly int REFRACTION_WIDTH = 640;
        private static readonly int REFRACTION_HEIGHT = 360;

        public int ReflectionFrameBuffer { get; set; }
        public int ReflectionTexture { get; set; }
        public int ReflectionDepthBuffer { get; set; }

        public int RefractionFrameBuffer { get; set; }
        public int RefractionTexture { get; set; }
        public int RefractionDepthTexture { get; set; }

        public JWaterFrameBuffer(JGameWindow gameWindow)
        {
            InitializeReflectionFrameBuffer(gameWindow);
            InitializeRefractionFrameBuffer(gameWindow);
        }

        private void InitializeReflectionFrameBuffer(JGameWindow gameWindow)
        {
            ReflectionFrameBuffer = CreateFrameBuffer();
            ReflectionTexture = CreateTextureAttachment(REFLECTION_WIDTH, REFLECTION_HEIGHT);
            ReflectionDepthBuffer = CreateDepthAttachment(REFLECTION_WIDTH, REFLECTION_HEIGHT);
            UnbindFrameBuffer(gameWindow);
        }

        private void InitializeRefractionFrameBuffer(JGameWindow gameWindow)
        {
            RefractionFrameBuffer = CreateFrameBuffer();
            RefractionTexture = CreateTextureAttachment(REFRACTION_WIDTH, REFRACTION_HEIGHT);
            RefractionDepthTexture = CreateDepthTextureAttachment(REFRACTION_WIDTH, REFRACTION_HEIGHT);
            UnbindFrameBuffer(gameWindow);
        }

        public void BindReflectionFrameBuffer()
        {
            BindFrameBuffer(ReflectionFrameBuffer, REFLECTION_WIDTH, REFLECTION_HEIGHT);
        }

        public void BindRefractionFrameBuffer()
        {
            BindFrameBuffer(RefractionFrameBuffer, REFRACTION_WIDTH, REFRACTION_HEIGHT);
        }

        /// <summary>
        /// Generate a FrameBuffer with a single attached ColorBuffer.
        /// </summary>
        /// <returns>The ID of the newly created FrameBuffer.</returns>
        private int CreateFrameBuffer()
        {
            int frameBuffer = GL.GenFramebuffer();
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, frameBuffer);
            GL.DrawBuffer(DrawBufferMode.ColorAttachment0);
            return frameBuffer;
        }

        /// <summary>
        /// Generate a 2D ColorTextureAttachment of size height x width and attach it to the FrameBuffer.
        /// </summary>
        /// <param name="width">Width of the TextureAttachment.</param>
        /// <param name="height">Height of the TextureAttachment.</param>
        /// <returns>The ID of the newly created TextureAttachment.</returns>
        private int CreateTextureAttachment(int width, int height)
        {
            int texture = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, texture);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgb, width, height, 0, PixelFormat.Rgb, PixelType.UnsignedByte, (IntPtr) null);
            GL.TextureParameter(texture, TextureParameterName.TextureMagFilter, (float) TextureMagFilter.Linear);
            GL.TextureParameter(texture, TextureParameterName.TextureMinFilter, (float) TextureMinFilter.Linear);
            GL.FramebufferTexture(FramebufferTarget.Framebuffer, FramebufferAttachment.ColorAttachment0, texture, 0);
            return texture;
        }

        /// <summary>
        /// Generate a 2D DepthTextureAttachment of size height x width and attach it to the FrameBuffer.
        /// </summary>
        /// <param name="width">Width of the TextureAttachment.</param>
        /// <param name="height">Height of the TextureAttachment.</param>
        /// <returns>The ID of the newly created TextureAttachment.</returns>
        private int CreateDepthTextureAttachment(int width, int height)
        {
            int texture = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, texture);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.DepthComponent32, width, height, 0, PixelFormat.DepthComponent, PixelType.Float, (IntPtr) null);
            GL.TextureParameter(texture, TextureParameterName.TextureMagFilter, (float)TextureMagFilter.Linear);
            GL.TextureParameter(texture, TextureParameterName.TextureMinFilter, (float)TextureMinFilter.Linear);
            GL.FramebufferTexture(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthAttachment, texture, 0);
            return texture;
        }

        /// <summary>
        /// Generate a 2D RenderBuffer of size height x width, to use as a DepthBuffer, and attach it to the FrameBuffer.
        /// </summary>
        /// <param name="width">Width of the DepthBuffer Attachment.</param>
        /// <param name="height">Height of the DepthBuffer Attachment.</param>
        /// <returns>The ID of the newly created DepthBuffer Attachment.</returns>
        private int CreateDepthAttachment(int width, int height)
        {
            int depth = GL.GenRenderbuffer();
            GL.BindRenderbuffer(RenderbufferTarget.Renderbuffer, depth);
            GL.RenderbufferStorage(RenderbufferTarget.Renderbuffer, RenderbufferStorage.DepthComponent, width, height);
            GL.FramebufferRenderbuffer(FramebufferTarget.Framebuffer, FramebufferAttachment.DepthAttachment, RenderbufferTarget.Renderbuffer, depth);
            return depth;
        }

        /// <summary>
        /// Bind the FrameBuffer to Render to it instead of the screen.
        /// </summary>
        /// <param name="frameBuffer">ID of the FrameBuffer to be bound.</param>
        /// <param name="width">The width of the ViewPort.</param>
        /// <param name="height">The hight of the ViewPort.</param>
        private void BindFrameBuffer(int frameBuffer, int width, int height)
        {
            GL.BindTexture(TextureTarget.Texture2D, 0);
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, frameBuffer);
            GL.Viewport(0, 0, width, height);
        }

        /// <summary>
        /// Unbind any FrameBuffers to begin rendering to the screen.
        /// </summary>
        /// <param name="gameWindow">The running JGameWindow.</param>
        public void UnbindFrameBuffer(JGameWindow gameWindow)
        {
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
            GL.Viewport(0, 0, gameWindow.Width, gameWindow.Height);
        }

        /// <summary>
        /// Delete any existing FrameBuffers and FrameBufferAttachments.
        /// </summary>
        public void CleanUp()
        {
            GL.DeleteFramebuffer(ReflectionFrameBuffer);
            GL.DeleteTexture(ReflectionTexture);
            GL.DeleteRenderbuffer(ReflectionDepthBuffer);
            GL.DeleteFramebuffer(RefractionFrameBuffer);
            GL.DeleteTexture(RefractionTexture);
            GL.DeleteTexture(RefractionDepthTexture);
        }
    }
}
