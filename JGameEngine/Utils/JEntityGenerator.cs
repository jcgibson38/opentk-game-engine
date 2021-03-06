﻿using JGameEngine.Entities;
using JGameEngine.Models;
using JGameEngine.RenderEngine;
using JGameEngine.Terrains;
using JGameEngine.Textures;
using JGameEngine.Water;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JGameEngine.Utils
{
    class JEntityGenerator
    {
        private JLoader Loader { get; set; }
        private JPerlinTerrain Terrain { get; set; }
        private JWaterTile WaterTile { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="loader"></param>
        /// <param name="terrain"></param>
        public JEntityGenerator(JLoader loader, JPerlinTerrain terrain, JWaterTile waterTile)
        {
            Loader = loader;
            Terrain = terrain;
            WaterTile = waterTile;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<JEntity> GenerateTrees()
        {
            Console.WriteLine("Generating Trees...");

            List<JEntity> TreeEntities = new List<JEntity>();

            string TreeTexturePath = JFileUtils.GetPathToResFile("Tree\\tree_texture_green_brown.png");
            string TreeModelPath = JFileUtils.GetPathToResFile("Tree\\tree.obj");
            JModelData TreeModelData = JObjFileLoader.LoadObj(TreeModelPath);
            JRawModel TreeModel = Loader.LoadToVAO(TreeModelData.Vertices, TreeModelData.TextureCoords, TreeModelData.Normals, TreeModelData.Indices);
            JModelTexture TreeTexture = new JModelTexture(Loader.loadTexture(TreeTexturePath));
            JTexturedModel TreeTexturedModel = new JTexturedModel(TreeModel, TreeTexture);
            Random r = new Random();

            for (int i = 0; i < 200; i++)
            {
                float posX = (float)r.NextDouble() * 800;
                float posZ = -(float)r.NextDouble() * 800;
                float posY = Terrain.GetHeightOfTerrain(posX, posZ);

                while (posY < WaterTile.Height)
                {
                    posX = (float)r.NextDouble() * 800;
                    posZ = -(float)r.NextDouble() * 800;
                    posY = Terrain.GetHeightOfTerrain(posX, posZ);
                }

                Vector3 orientation = new Vector3((float)r.NextDouble(),0,(float)r.NextDouble());
                orientation.NormalizeFast();

                JEntity entity = new JEntity(TreeTexturedModel, new Vector3(posX, posY, posZ), orientation, 0.25f);
                TreeEntities.Add(entity);
            }

            return TreeEntities;
        }
    }
}
