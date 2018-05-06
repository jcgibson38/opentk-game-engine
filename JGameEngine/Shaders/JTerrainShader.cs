﻿using JGameEngine.Entities;
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
        private static readonly string VERTEX_FILE = "C:\\Users\\Jordan\\source\\repos\\JGameEngine - Copy\\JGameEngine\\shaders\\TerrainVertexShader.txt";
        private static readonly string FRAGMENT_FILE = "C:\\Users\\Jordan\\source\\repos\\JGameEngine - Copy\\JGameEngine\\shaders\\TerrainFragmentShader.txt";

        private int location_transformationMatrix;
        private int location_projectionMatrix;
        private int location_viewMatrix;
        private int location_lightPosition;
        private int location_lightColor;
        private int location_reflectivity;
        private int location_shineDamper;
        private int location_skyColor;

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
    }
}