using JGameEngine.Models;
using JGameEngine.Shaders;
using JGameEngine.Terrains;
using JGameEngine.Textures;
using JGameEngine.Utils;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JGameEngine.RenderEngine
{
    class JNoiseWindow : GameWindow
    {
        private readonly static int WIDTH = 1920;
        private readonly static int HEIGHT = 900;

        private JLoader Loader { get; set; }
        private JRenderer2D Renderer { get; set; }
        private JStaticShader2D shader { get; set; }
        private JTexturedModel tm { get; set; }

        JRawModel model { get; set; }

        public JNoiseWindow() : base(WIDTH, HEIGHT, new OpenTK.Graphics.GraphicsMode(32, 24, 0, 8), "JNoiseWindow", GameWindowFlags.Default, DisplayDevice.Default, 3, 0, GraphicsContextFlags.ForwardCompatible)
        {
            Loader = new JLoader();
            Renderer = new JRenderer2D();
            shader = new JStaticShader2D();

            float[] vertices = {
                -0.5f, 0.5f, 0f,//v0
				-0.5f, -0.5f, 0f,//v1
				0f, -0.5f, 0f,//v2
				0f, 0.5f, 0f,//v3
		    };

            uint[] indices = {
                0,1,3,//top left triangle (v0, v1, v3)
				3,1,2//bottom right triangle (v3, v1, v2)
		    };

            float[] textureCoords =
            {
                0,0,
                0,1,
                1,1,
                1,0
            };

            float[,] test = JNoise.GenerateNoiseMap(100, 100, 27.6f, 4, 0.5f, 1.87f);

            model = Loader.LoadToVAO(vertices,textureCoords,indices);
            //JModelTexture mt = new JModelTexture(Loader.loadTexture(JFileUtils.GetPathToFile("Cowboy\\CowboyTexture.png")));
            JModelTexture mt = new JModelTexture(Loader.LoadColorBitmapTexture(test));
            tm = new JTexturedModel(model, mt);
        }

        protected override void OnResize(EventArgs e)
        {
            GL.Viewport(0, 0, this.Width, this.Height);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            Renderer.prepare();
            shader.start();
            Renderer.Render(tm);
            shader.stop();
            this.SwapBuffers();
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            Loader.cleanUp();
            shader.cleanUp();
        }
    }
}
