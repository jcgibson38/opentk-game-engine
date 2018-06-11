using JGameEngine.Entities.Camera;
using JGameEngine.Shaders;
using JGameEngine.Utils;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JGameEngine.Water
{
    class JWaterShader : JShaderProgram
    {
        private static readonly string VERTEX_FILE = JFileUtils.GetPathToFile("Water\\WaterVertexShader.txt");
        private static readonly string FRAGMENT_FILE = JFileUtils.GetPathToFile("Water\\WaterFragmentShader.txt");

        private int location_modelMatrix;
        private int location_viewMatrix;
        private int location_projectectionMatrix;
        private int location_reflectionTexture;
        private int location_refractionTexture;
        private int location_waterTexture;
        private int location_dudvMap;
        private int location_distortionVariance;

        public JWaterShader() : base(VERTEX_FILE, FRAGMENT_FILE)
        {

        }

        protected override void bindAttributes()
        {
            bindAttribute(0, "position");
        }

        protected override void getAllUniformLocations()
        {
            location_projectectionMatrix = getUnifromLocation("projectionMatrix");
            location_viewMatrix = getUnifromLocation("viewMatrix");
            location_modelMatrix = getUnifromLocation("modelMatrix");
            location_reflectionTexture = getUnifromLocation("reflectionTexture");
            location_refractionTexture = getUnifromLocation("refractionTexture");
            location_waterTexture = getUnifromLocation("waterTexture");
            location_dudvMap = getUnifromLocation("dudvMap");
            location_distortionVariance = getUnifromLocation("distortionVariance");
        }

        public void LoadTextures()
        {
            base.LoadInt(location_reflectionTexture, 0);
            base.LoadInt(location_refractionTexture, 1);
            base.LoadInt(location_waterTexture, 2);
            base.LoadInt(location_dudvMap, 3);
        }

        public void LoadDistortionVariance(float value)
        {
            base.loadFloat(location_distortionVariance, value);
        }

        public void LoadProjectionMatrix(Matrix4 projection)
        {
            base.loadMatrix(location_projectectionMatrix, projection);
        }

        public void LoadViewMatrix(JCamera camera)
        {
            Matrix4 viewMatrix = JMathUtils.createViewMatrix(camera);
            base.loadMatrix(location_viewMatrix, viewMatrix);
        }

        public void LoadModelMatrix(Matrix4 modelMatrix)
        {
            base.loadMatrix(location_modelMatrix, modelMatrix);
        }
    }
}
