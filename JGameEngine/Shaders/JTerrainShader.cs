using JGameEngine.Entities;
using JGameEngine.Entities.Camera;
using JGameEngine.Shaders;
using JGameEngine.Utils;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JGameEngine.Shaders
{
    public class JTerrainShader : JShaderProgram
    {
        private static readonly string VERTEX_FILE = JFileUtils.GetPathToShader("TerrainVertexShader.txt");
        private static readonly string FRAGMENT_FILE = JFileUtils.GetPathToShader("TerrainFragmentShader.txt");

        private int location_transformationMatrix;
        private int location_projectionMatrix;
        private int location_viewMatrix;
        private int location_lightPosition;
        private int location_lightColor;
        private int location_reflectivity;
        private int location_shineDamper;
        private int location_skyColor;
        private int location_textureSamplerLand;
        private int location_terrainTextureHeights;
        private int location_numTerrainHeights;
        private int location_clippingPlane;

        public JTerrainShader() : base(VERTEX_FILE,FRAGMENT_FILE)
        {

        }

        /// <summary>
        /// Defines the 'in' variables in the vertexShader.
        /// </summary>
        protected override void bindAttributes()
        {
            base.bindAttribute(0, "position");
            base.bindAttribute(1, "textureCoords");
            base.bindAttribute(2, "normal");
        }

        protected override void getAllUniformLocations()
        {
            location_transformationMatrix = base.getUnifromLocation("transformationMatrix");
            location_projectionMatrix = base.getUnifromLocation("projectionMatrix");
            location_viewMatrix = base.getUnifromLocation("viewMatrix");
            location_lightColor = base.getUnifromLocation("lightColor");
            location_lightPosition = base.getUnifromLocation("lightPosition");
            location_reflectivity = base.getUnifromLocation("reflectivity");
            location_shineDamper = base.getUnifromLocation("shineDamper");
            location_skyColor = base.getUnifromLocation("skyColor");
            location_textureSamplerLand = base.getUnifromLocation("textureSamplerLand");
            location_terrainTextureHeights = base.getUnifromLocation("terrainTextureHeights");
            location_numTerrainHeights = base.getUnifromLocation("numTerrainHeights");
            location_clippingPlane = base.getUnifromLocation("clippingPlane");
        }

        public void LoadClippingPlane(Vector4 clippingPlane)
        {
            base.LoadVector(location_clippingPlane, clippingPlane);
        }

        public void LoadSkyColor(float r, float g, float b)
        {
            base.LoadVector(location_skyColor, new Vector3(r, g, b));
        }

        public void LoadTransformationMatrix(Matrix4 matrix)
        {
            base.loadMatrix(location_transformationMatrix, matrix);
        }

        public void LoadLight(JLight light)
        {
            base.LoadVector(location_lightColor, light.Color);
            base.LoadVector(location_lightPosition, light.Position);
        }

        public void LoadProjectionMatrix(Matrix4 matrix)
        {
            base.loadMatrix(location_projectionMatrix, matrix);
        }

        public void LoadViewMatrix(JCamera camera)
        {
            Matrix4 viewMatrix = JMathUtils.createViewMatrix(camera);
            base.loadMatrix(location_viewMatrix, viewMatrix);
        }

        public void LoadShineVariables(float damper, float reflectivity)
        {
            base.loadFloat(location_reflectivity, reflectivity);
            base.loadFloat(location_shineDamper, damper);
        }

        public void LoadTextures(float[] textureHeights, int numTerrainHeights)
        {
            int[] textureSamplers = new int[8];
            for (int i = 0; i < textureSamplers.Length; i++)
            {
                textureSamplers[i] = i;
            }

            base.LoadIntArray(location_textureSamplerLand, textureSamplers);
            base.LoadFloatArray(location_terrainTextureHeights, textureHeights);
            base.LoadInt(location_numTerrainHeights, numTerrainHeights);
        }
    }
}
