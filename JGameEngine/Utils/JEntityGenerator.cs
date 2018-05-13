using JGameEngine.Entities;
using JGameEngine.Models;
using JGameEngine.RenderEngine;
using JGameEngine.Terrains;
using JGameEngine.Textures;
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="loader"></param>
        /// <param name="terrain"></param>
        public JEntityGenerator(JLoader loader, JPerlinTerrain terrain)
        {
            Loader = loader;
            Terrain = terrain;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public List<JEntity> GenerateTrees()
        {
            Console.WriteLine("Generating Trees...");

            List<JEntity> TreeEntities = new List<JEntity>();

            string TreeTexturePath = JFileUtils.GetPathToFile("Tree\\tree_texture_green_brown.png");
            string TreeModelPath = JFileUtils.GetPathToFile("Tree\\tree.obj");
            JModelData TreeModelData = JObjFileLoader.LoadObj(TreeModelPath);
            JRawModel TreeModel = Loader.loadToVAO(TreeModelData.Vertices, TreeModelData.TextureCoords, TreeModelData.Normals, TreeModelData.Indices);
            JModelTexture TreeTexture = new JModelTexture(Loader.loadTexture(TreeTexturePath));
            JTexturedModel TreeTexturedModel = new JTexturedModel(TreeModel, TreeTexture);
            Random r = new Random();

            for (int i = 0; i < 100; i++)
            {
                float posX = (float)r.NextDouble() * 800;
                float posZ = -(float)r.NextDouble() * 800;
                float posY = Terrain.GetHeightOfTerrain(posX, posZ);

                Vector3 orientation = new Vector3((float)r.NextDouble(),0,(float)r.NextDouble());
                orientation.NormalizeFast();

                JEntity entity = new JEntity(TreeTexturedModel, new Vector3(posX, posY, posZ), orientation, 0.25f);
                TreeEntities.Add(entity);
            }

            return TreeEntities;
        }
    }
}
