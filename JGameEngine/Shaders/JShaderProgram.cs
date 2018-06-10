using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.IO;
using System.Text;

namespace JGameEngine.Shaders
{
    public abstract class JShaderProgram
    {
        private int programID;
        private int vertexShaderID;
        private int fragmentShaderID;

        public JShaderProgram(string vertexFile,string fragmentFile)
        {
            vertexShaderID = loadShader(vertexFile,ShaderType.VertexShader);
            fragmentShaderID = loadShader(fragmentFile,ShaderType.FragmentShader);
            programID = GL.CreateProgram();
            GL.AttachShader(programID, vertexShaderID);
            GL.AttachShader(programID, fragmentShaderID);
            bindAttributes();
            GL.LinkProgram(programID);
            GL.ValidateProgram(programID);
            getAllUniformLocations();
        }

        #region ShaderControl

        public void start()
        {
            GL.UseProgram(programID);
        }

        public void stop()
        {
            GL.UseProgram(0);
        }

        public void cleanUp()
        {
            stop();
            GL.DetachShader(programID, vertexShaderID);
            GL.DetachShader(programID, fragmentShaderID);
            GL.DeleteShader(vertexShaderID);
            GL.DeleteShader(fragmentShaderID);
            GL.DeleteProgram(programID);
        }

        #endregion ShaderControl

        protected abstract void bindAttributes();

        protected void bindAttribute(int attribute,string variableName)
        {
            GL.BindAttribLocation(programID, attribute, variableName);
        }

        private static int loadShader(string file,ShaderType type)
        {
            StringBuilder shaderSource = new StringBuilder();
            try
            {
                string line;

                using (FileStream stream = new FileStream(file, FileMode.Open))
                {
                    using(StreamReader rdr = new StreamReader(stream))
                    {
                        while((line = rdr.ReadLine()) != null)
                        {
                            shaderSource.Append(line).Append("\n");
                        }
                    }
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.StackTrace);
            }

            int shaderID = GL.CreateShader(type);
            GL.ShaderSource(shaderID, shaderSource.ToString());
            GL.CompileShader(shaderID);

            int shaderStatus;
            GL.GetShader(shaderID, ShaderParameter.CompileStatus, out shaderStatus);
            if (shaderStatus == 0)
            {
                Console.WriteLine(GL.GetShaderInfoLog(shaderID));
                Console.WriteLine("Shader could not be compiled.");
                System.Environment.Exit(-1);
            }

            return shaderID;
        }

        #region UniformVariables

        /// <summary>
        /// 
        /// </summary>
        /// <param name="uniformName"></param>
        /// <returns></returns>
        protected int getUnifromLocation(string uniformName)
        {
            return GL.GetUniformLocation(programID, uniformName);
        }

        /// <summary>
        /// 
        /// </summary>
        protected abstract void getAllUniformLocations();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="location"></param>
        /// <param name="value"></param>
        protected void loadFloat(int location,float value)
        {
            GL.Uniform1(location, value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="location"></param>
        /// <param name="values"></param>
        protected void LoadFloatArray(int location, float[] values)
        {
            GL.Uniform1(location, values.Length, values);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="location"></param>
        /// <param name="vector"></param>
        protected void LoadVector(int location, Vector3 vector)
        {
            GL.Uniform3(location, ref vector);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="location"></param>
        /// <param name="vector"></param>
        protected void LoadVector(int location, Vector4 vector)
        {
            GL.Uniform4(location, ref vector);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="location"></param>
        /// <param name="value"></param>
        protected void loadBoolean(int location,bool value)
        {
            float toLoad = 0;
            if (value)
            {
                toLoad = 1;
            }
            GL.Uniform1(location, toLoad);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="location"></param>
        /// <param name="matrix"></param>
        protected void loadMatrix(int location, Matrix4 matrix)
        {
            GL.UniformMatrix4(location, false, ref matrix);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="location"></param>
        /// <param name="value"></param>
        protected void LoadInt(int location, int value)
        {
            GL.Uniform1(location, value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="location"></param>
        /// <param name="values"></param>
        protected void LoadIntArray(int location, int[] values)
        {
            GL.Uniform1(location, values.Length, values);
        }

        #endregion UniformVariables
    }
}
