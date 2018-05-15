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
        private int location_textureWaterDeep;
        private int location_textureWaterShallow;
        private int location_textureSand;
        private int location_textureGrassNatural;
        private int location_textureGrassLush;
        private int location_textureMountainNatural;
        private int location_textureMountainRocky;
        private int location_textureSnow;

        public JTerrainShader() : base(VERTEX_FILE,FRAGMENT_FILE)
        { }

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
            location_textureWaterDeep = base.getUnifromLocation("textureSamplerWaterDeep");
            location_textureWaterShallow = base.getUnifromLocation("textureSamplerWaterShallow");
            location_textureSand = base.getUnifromLocation("textureSamplerSand");
            location_textureGrassNatural = base.getUnifromLocation("textureSamplerGrassNatural");
            location_textureGrassLush = base.getUnifromLocation("textureSamplerGrassLush");
            location_textureMountainNatural = base.getUnifromLocation("textureSamplerMountainNatural");
            location_textureMountainRocky = base.getUnifromLocation("textureSamplerMountainRocky");
            location_textureSnow = base.getUnifromLocation("textureSamplerSnow");
        }

        public void loadSkyColor(float r, float g, float b)
        {
            base.loadVector(location_skyColor, new Vector3(r, g, b));
        }

        public void loadTransformationMatrix(Matrix4 matrix)
        {
            base.loadMatrix(location_transformationMatrix, matrix);
        }

        public void LoadLight(JLight light)
        {
            base.loadVector(location_lightColor, light.Color);
            base.loadVector(location_lightPosition, light.Position);
        }

        public void loadProjectionMatrix(Matrix4 matrix)
        {
            base.loadMatrix(location_projectionMatrix, matrix);
        }

        public void LoadViewMatrix(JCamera camera)
        {
            Matrix4 viewMatrix = JMathUtils.createViewMatrix(camera);
            base.loadMatrix(location_viewMatrix, viewMatrix);
        }

        public void loadShineVariables(float damper, float reflectivity)
        {
            base.loadFloat(location_reflectivity, reflectivity);
            base.loadFloat(location_shineDamper, damper);
        }

        public void LoadTextures()
        {
            base.LoadInt(location_textureWaterDeep, 0);
            base.LoadInt(location_textureWaterShallow, 1);
            base.LoadInt(location_textureSand, 2);
            base.LoadInt(location_textureGrassNatural, 3);
            base.LoadInt(location_textureGrassLush, 4);
            base.LoadInt(location_textureMountainNatural, 5);
            base.LoadInt(location_textureMountainRocky, 6);
            base.LoadInt(location_textureSnow, 7);
        }
    }
}
