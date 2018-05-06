using JGameEngine.Entities;
using JGameEngine.Models;
using JGameEngine.RenderEngine;
using JGameEngine.Textures;
using JGameEngine.Utils;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JGameEngine.Entities.Collision
{
    public class JBoundingSphere
    {
        /// <summary>
        /// The radius of the JBoundingSphere.
        /// </summary>
        public float Radius { get; set; }

        /// <summary>
        /// The JRawModel of the JBoundingSphere. Used if we want to render the JBoundingSphere in the world.
        /// </summary>
        private JRawModel SphereModel { get; set; }

        /// <summary>
        /// The path to the *.png texture file to use for the JBoundingSphere when it is selected.
        /// </summary>
        private string SphereTexturePathSelected { get; set; }

        /// <summary>
        /// The path to the *.png texture file to use for the JBoundingSphere when it is not selected.
        /// </summary>
        private string SphereTexturePathUnselected { get; set; }

        /// <summary>
        /// The path to the *.obj model file to use for the JBoundingSphere.
        /// </summary>
        private string SphereModelPath { get; set; }

        /// <summary>
        /// The JTexturedModel created from SphereTexturePath and SphereModelPath.
        /// </summary>
        private JTexturedModel SphereTexturedModel { get; set; }

        /// <summary>
        /// The JEntity of the JBoundingSphere used if we want to render the JBoundingSphere in the world.
        /// </summary>
        public JEntity SphereEntity { get; set; }

        /// <summary>
        /// Contains the data loaded from SphereModelPath.
        /// </summary>
        private JModelData SphereModelData { get; set; }

        /// <summary>
        /// Assign a JBoundingSphere to a JEntity and use JMousePicker to check for mouse selection of the JEntity.
        /// </summary>
        public JBoundingSphere()
        {
            SphereTexturePathSelected = JFileUtils.GetPathToFile("BoundingSphereTextureSelected.png");
            SphereTexturePathUnselected = JFileUtils.GetPathToFile("BoundingSphereTextureUnselected.png");
            SphereModelPath = JFileUtils.GetPathToFile("CollisionSphere.obj");

            Radius = 1f;
        }

        /// <summary>
        /// Load the model and texture of the JBoundingSphere if we want to render it in the world.
        /// </summary>
        /// <param name="loader"></param>
        public void LoadSphereModel(JLoader loader)
        {
            SphereModelData = JObjFileLoader.LoadObj(SphereModelPath);
            SphereModel = loader.loadToVAO(SphereModelData.Vertices, SphereModelData.TextureCoords, SphereModelData.Normals, SphereModelData.Indices);
            JModelTexture texture = new JModelTexture(loader.loadTexture(SphereTexturePathUnselected));
            SphereTexturedModel = new JTexturedModel(SphereModel, texture);
            SphereEntity = new JEntity(SphereTexturedModel, new Vector3(50, 0, -50), new Vector3(0, 0, 1), 0.5f);
        }

        public void Select(JLoader loader)
        {
            JModelTexture texture = new JModelTexture(loader.loadTexture(SphereTexturePathSelected));
            SphereTexturedModel = new JTexturedModel(SphereModel, texture);
            SphereEntity = new JEntity(SphereTexturedModel, SphereEntity.Position, new Vector3(0, 0, 1), 0.5f);
        }

        public void DeSelect(JLoader loader)
        {
            JModelTexture texture = new JModelTexture(loader.loadTexture(SphereTexturePathUnselected));
            SphereTexturedModel = new JTexturedModel(SphereModel, texture);
            SphereEntity = new JEntity(SphereTexturedModel, SphereEntity.Position, new Vector3(0, 0, 1), 0.5f);
        }
    }
}
