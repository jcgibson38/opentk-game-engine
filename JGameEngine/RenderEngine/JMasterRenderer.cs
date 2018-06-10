using JGameEngine.Entities;
using JGameEngine.Shaders;
using System.Collections.Generic;
using OpenTK.Graphics.OpenGL;
using OpenTK.Graphics;
using JGameEngine.Utils;
using OpenTK;
using JGameEngine.Terrains;
using JGameEngine.Models;
using JGameEngine.Entities.Camera;
using JGameEngine.Config;
using JGameEngine.Textures;

namespace JGameEngine.RenderEngine
{
    class JMasterRenderer
    {
        public JStaticShader Shader { get; set; }
        public JEntityRenderer Renderer { get; set; }

        public Matrix4 projectionMatrix { get; set; }

        public Dictionary<JTexturedModel, List<JEntity>> Entities { get; set; }
        private List<JPerlinTerrain> terrains;

        private static float skyRed { get; set; }
        private static float skyGreen { get; set; }
        private static float skyBlue { get; set; }

        private JTerrainRenderer TerrainRenderer { get; set; }
        private JTerrainShader TerrainShader { get; set; }

        public JMasterRenderer(JTerrainTexturePack texturePack)
        {
            terrains = new List<JPerlinTerrain>();
            skyRed = 0.5f;
            skyGreen = 0.5f;
            skyBlue = 0.5f;

            GL.Enable(EnableCap.CullFace);
            GL.CullFace(CullFaceMode.Back);
            this.projectionMatrix = JMathUtils.createProjectionMatrix(JConfig.FOV,JConfig.NEAR_PLANE,JConfig.FAR_PLANE);
            this.Shader = new JStaticShader();
            this.TerrainShader = new JTerrainShader();
            this.Renderer = new JEntityRenderer(Shader,projectionMatrix);
            this.TerrainRenderer = new JTerrainRenderer(TerrainShader,projectionMatrix,texturePack);
            this.Entities = new Dictionary<JTexturedModel, List<JEntity>>();
        }

        public void ProcessEntity(JEntity entity)
        {
            JTexturedModel texturedModel = entity.TexturedModel;
            try
            {
                List<JEntity> batch = Entities[texturedModel];
                batch.Add(entity);
            }
            catch(KeyNotFoundException e)
            {
                List<JEntity> newBatch = new List<JEntity>
                {
                    entity
                };
                Entities.Add(texturedModel, newBatch);
            }
        }

        public void Render(JLight light,JCamera camera, Vector4 clippingPlane)
        {
            prepare();

            Shader.start();
            Shader.LoadClippingPlane(clippingPlane);
            Shader.LoadSkyColor(skyRed,skyGreen,skyBlue);
            Shader.LoadLight(light);
            Shader.LoadViewMatrix(camera);
            Renderer.render(Entities);
            Shader.stop();

            TerrainShader.start();
            TerrainShader.LoadClippingPlane(clippingPlane);
            TerrainShader.LoadSkyColor(skyRed, skyGreen, skyBlue);
            TerrainShader.LoadLight(light);
            TerrainShader.LoadViewMatrix(camera);
            TerrainRenderer.Render(terrains);
            TerrainShader.stop();

            Entities.Clear();
            terrains.Clear();
        }

        public void processTerrain(JPerlinTerrain terrain)
        {
            terrains.Add(terrain);
        }

        public void CleanUp()
        {
            Shader.cleanUp();
            TerrainShader.cleanUp();
        }

        public void prepare()
        {
            GL.Enable(EnableCap.DepthTest);
            GL.ClearColor(skyRed,skyGreen,skyBlue,1);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
        }

        public void RenderScene(List<JBoundedEntity> entities, List<JEntity> staticEntities, JPerlinTerrain terrain, JLight light, JCamera camera, Vector4 clippingPlane)
        {
            processTerrain(terrain);
            foreach(JBoundedEntity entity in entities){
                ProcessEntity(entity);
                ProcessEntity(entity.BoundingSphere.SphereEntity);
            }
            foreach (JEntity entity in staticEntities)
            {
                ProcessEntity(entity);
            }
            Render(light, camera, clippingPlane);
        }
    }
}
