using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JGameEngine.Shaders
{
    /// <summary>
    /// The JShaderProgram for use in shading a 2D entity. Used 2DVertexShader and 2DFragmentShader.
    /// </summary>
    class JStaticShader2D : JShaderProgram
    {
        private static readonly string VERTEX_FILE = "C:\\Users\\Jordan\\source\\repos\\JGameEngine_v1\\JGameEngine\\Shaders\\2DVertexShader.txt";
        private static readonly string FRAGMENT_FILE = "C:\\Users\\Jordan\\source\\repos\\JGameEngine_v1\\JGameEngine\\Shaders\\2DFragmentShader.txt";

        public JStaticShader2D() : base(VERTEX_FILE, FRAGMENT_FILE)
        {
            
        }

        /// <summary>
        /// Defines the 'in' variables in the vertexShader.
        /// </summary>
        protected override void bindAttributes()
        {
            base.bindAttribute(0, "position");
            base.bindAttribute(1, "textureCoords");
        }

        protected override void getAllUniformLocations()
        {

        }
    }
}
