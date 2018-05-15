using JGameEngine.Entities;
using JGameEngine.Entities.Camera;
using JGameEngine.Models;
using JGameEngine.Terrains;
using JGameEngine.Textures;
using JGameEngine.Utils;
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

        JPerlinTerrain terrain;

        /// <summary>
        /// GraphicsMode used for anitaliasing.
        /// </summary>
        public JGameWindow() : base(WIDTH, HEIGHT, new OpenTK.Graphics.GraphicsMode(32,24,0,8), "JModelViewer", GameWindowFlags.Default, DisplayDevice.Default, 3, 0, GraphicsContextFlags.ForwardCompatible)
        {
            PlayerTexturePath = JFileUtils.GetPathToFile("Cowboy\\CowboyTexture.png");
            PlayerModelPath = JFileUtils.GetPathToFile("Cowboy\\Cowboy.obj");

            Console.WriteLine("OpenGL version: " + GL.GetString(StringName.Version));

            Loader = new JLoader();
            PlayerModelData = JObjFileLoader.LoadObj(PlayerModelPath);

            Console.WriteLine("Using .obj file: " + PlayerModelPath);
            PlayerModel = Loader.loadToVAO(PlayerModelData.Vertices, PlayerModelData.TextureCoords, PlayerModelData.Normals, PlayerModelData.Indices);

            Console.WriteLine("Using .png texture file: " + PlayerTexturePath + "...");
            JModelTexture texture = new JModelTexture(Loader.loadTexture(PlayerTexturePath));
            
            TexturedModel = new JTexturedModel(PlayerModel, texture);
            Light = new JLight(new Vector3(0,0,0),new Vector3(1,1,1));
            MasterRenderer = new JMasterRenderer();

            JTerrainTexture textureWaterDeep = new JTerrainTexture(Loader.loadTexture(JFileUtils.GetPathToFile("Terrain\\WaterDeep.png")));
            JTerrainTexture textureWaterShallow = new JTerrainTexture(Loader.loadTexture(JFileUtils.GetPathToFile("Terrain\\WaterShallow.png")));
            JTerrainTexture textureSand = new JTerrainTexture(Loader.loadTexture(JFileUtils.GetPathToFile("Terrain\\Sand.png")));
            JTerrainTexture textureGrassNatural = new JTerrainTexture(Loader.loadTexture(JFileUtils.GetPathToFile("Terrain\\GrassNatural.png")));
            JTerrainTexture textureGrassLush = new JTerrainTexture(Loader.loadTexture(JFileUtils.GetPathToFile("Terrain\\GrassLush.png")));
            JTerrainTexture textureMountainNatural = new JTerrainTexture(Loader.loadTexture(JFileUtils.GetPathToFile("Terrain\\MountainNatural.png")));
            JTerrainTexture textureMountainRocky = new JTerrainTexture(Loader.loadTexture(JFileUtils.GetPathToFile("Terrain\\MountainRocky.png")));
            JTerrainTexture textureSnow = new JTerrainTexture(Loader.loadTexture(JFileUtils.GetPathToFile("Terrain\\Snow.png")));

            JTerrainTexturePack texturePack = new JTerrainTexturePack(textureWaterDeep, textureWaterShallow, textureSand, textureGrassNatural, textureGrassLush, textureMountainNatural, textureMountainRocky, textureSnow);

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
            MasterRenderer.processTerrain(terrain);
            foreach (JBoundedEntity entity in EntityList)
            {
                MasterRenderer.ProcessEntity(entity);
                MasterRenderer.ProcessEntity(entity.BoundingSphere.SphereEntity);
            }
            foreach(JEntity entity in StaticEntities)
            {
                MasterRenderer.ProcessEntity(entity);
            }
            MasterRenderer.Render(Light, Camera);
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
