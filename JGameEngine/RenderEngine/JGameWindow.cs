using JGameEngine.Entities;
using JGameEngine.Entities.Camera;
using JGameEngine.Models;
using JGameEngine.Terrains;
using JGameEngine.Textures;
using JGameEngine.Utils;
using JGameEngine.Water;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System;
using System.Collections.Generic;

namespace JGameEngine.RenderEngine
{
    public class JGameWindow : GameWindow
    {
        private readonly static int WIDTH = 1920;
        private readonly static int HEIGHT = 900;

        private static ulong LastFrameTime { get; set; }
        public static float Delta { get; set; }

        public string PlayerTexturePath { get; set; }
        public string PlayerModelPath { get; set; }

        private JLoader Loader;
        private JRawModel PlayerModel;
        private JTexturedModel TexturedModel;

        private JBoundedEntity Entity;
        private JBoundedEntity Entity2;
        private List<JBoundedEntity> EntityList;
        private List<JEntity> StaticEntities;

        private JCameraFree Camera;
        private JLight Light;
        private JMasterRenderer MasterRenderer;
        private JModelData PlayerModelData;

        private JMousePicker picker;

        private JWaterRenderer waterRenderer;
        private List<JWaterTile> waterTiles;

        JPerlinTerrain terrain;

        /// <summary>
        /// GraphicsMode used for anitaliasing.
        /// </summary>
        public JGameWindow() : base(WIDTH, HEIGHT, new OpenTK.Graphics.GraphicsMode(32,24,0,8), "JModelViewer", GameWindowFlags.Default, DisplayDevice.Default, 3, 0, GraphicsContextFlags.ForwardCompatible)
        {
            PlayerTexturePath = JFileUtils.GetPathToResFile("Cowboy\\CowboyTexture.png");
            PlayerModelPath = JFileUtils.GetPathToResFile("Cowboy\\Cowboy.obj");

            Version version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
            DateTime buildDate = new DateTime(2000, 1, 1).AddDays(version.Build).AddSeconds(version.Revision * 2);
            string displayVersion = $"{version} ({buildDate})";

            Console.WriteLine("JGameEngine v." + displayVersion);

            Console.WriteLine("OpenGL version: " + GL.GetString(StringName.Version));

            Loader = new JLoader();
            PlayerModelData = JObjFileLoader.LoadObj(PlayerModelPath);

            Console.WriteLine("Using .obj file: " + PlayerModelPath);
            PlayerModel = Loader.LoadToVAO(PlayerModelData.Vertices, PlayerModelData.TextureCoords, PlayerModelData.Normals, PlayerModelData.Indices);

            Console.WriteLine("Using .png texture file: " + PlayerTexturePath + "...");
            JModelTexture texture = new JModelTexture(Loader.loadTexture(PlayerTexturePath));
            
            TexturedModel = new JTexturedModel(PlayerModel, texture);
            Light = new JLight(new Vector3(0,0,0),new Vector3(1,1,1));
            

            JTerrainTexture textureWaterDeep = new JTerrainTexture(Loader.loadTexture(JFileUtils.GetPathToResFile("Terrain\\WaterDeep.png")));
            JTerrainTexture textureWaterShallow = new JTerrainTexture(Loader.loadTexture(JFileUtils.GetPathToResFile("Terrain\\WaterShallow.png")));
            JTerrainTexture textureSand = new JTerrainTexture(Loader.loadTexture(JFileUtils.GetPathToResFile("Terrain\\Sand.png")));
            JTerrainTexture textureGrassNatural = new JTerrainTexture(Loader.loadTexture(JFileUtils.GetPathToResFile("Terrain\\GrassNatural.png")));
            JTerrainTexture textureGrassLush = new JTerrainTexture(Loader.loadTexture(JFileUtils.GetPathToResFile("Terrain\\GrassLush.png")));
            JTerrainTexture textureMountainNatural = new JTerrainTexture(Loader.loadTexture(JFileUtils.GetPathToResFile("Terrain\\MountainNatural.png")));
            JTerrainTexture textureMountainRocky = new JTerrainTexture(Loader.loadTexture(JFileUtils.GetPathToResFile("Terrain\\MountainRocky.png")));
            JTerrainTexture textureSnow = new JTerrainTexture(Loader.loadTexture(JFileUtils.GetPathToResFile("Terrain\\Snow.png")));

            JTerrainTexturePack texturePack = new JTerrainTexturePack();
            texturePack.AddTerrainTexture(textureWaterDeep, 0.1f);
            texturePack.AddTerrainTexture(textureWaterShallow, 0.2f);
            texturePack.AddTerrainTexture(textureSand, 0.3f);
            texturePack.AddTerrainTexture(textureGrassNatural, 0.4f);
            texturePack.AddTerrainTexture(textureGrassLush, 0.5f);
            texturePack.AddTerrainTexture(textureMountainNatural, 0.6f);
            texturePack.AddTerrainTexture(textureMountainRocky, 0.7f);
            texturePack.AddTerrainTexture(textureSnow, 0.8f);

            MasterRenderer = new JMasterRenderer(texturePack);

            JWaterShader waterShader = new JWaterShader();
            waterRenderer = new JWaterRenderer(Loader, waterShader, MasterRenderer.projectionMatrix);
            waterTiles = new List<JWaterTile>();
            waterTiles.Add(new JWaterTile(400, -400, 8));

            terrain = new JPerlinTerrain(0, -1, Loader, texturePack);

            Entity = new JBoundedEntity(TexturedModel, new Vector3(50, 0, -50), new Vector3(0,0,1), 0.1f, Loader);
            Entity2 = new JBoundedEntity(TexturedModel, new Vector3(100, 0, -50), new Vector3(0, 0, 1), 0.1f, Loader);
            EntityList = new List<JBoundedEntity>();
            EntityList.Add(Entity);
            EntityList.Add(Entity2);

            // Generate Trees
            JEntityGenerator entityGenerator = new JEntityGenerator(Loader,terrain);
            StaticEntities = entityGenerator.GenerateTrees();

            Camera = new JCameraFree();

            picker = new JMousePicker(Camera, MasterRenderer.projectionMatrix,terrain,this);

            LastFrameTime = GetCurrentTime();
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
            ulong currentFrameTime = GetCurrentTime();
            Delta = (currentFrameTime - LastFrameTime) / 1000.0f; // seconds
            LastFrameTime = currentFrameTime;
            Camera.Move();

            if(Mouse.GetCursorState().LeftButton == ButtonState.Pressed)
            {
                picker.Update();
                foreach(JBoundedEntity entity in EntityList)
                {
                    if (picker.CheckIntersection(entity))
                    {
                        entity.Select(Loader);
                    }
                    else
                    {
                        entity.DeSelect(Loader);
                    }
                }
            }

            if(Mouse.GetCursorState().RightButton == ButtonState.Pressed)
            {
                foreach (JBoundedEntity entity in EntityList)
                {
                    if (entity.IsSelected)
                    {
                        picker.Update();
                        entity.UpdateDestination(picker.CurrentTerrainPoint);
                    }
                }
            }

            foreach (JBoundedEntity entity in EntityList)
            {
                entity.Move(terrain);
            }
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            MasterRenderer.RenderScene(EntityList, StaticEntities, terrain, Light, Camera);
            waterRenderer.Render(waterTiles, Camera);
            this.SwapBuffers();
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            MasterRenderer.CleanUp();
            Loader.cleanUp();
        }

        private static ulong GetCurrentTime()
        {
            return (((ulong)System.DateTime.Now.Ticks * 1000) / (ulong)TimeSpan.TicksPerSecond);
        }
    }
}
