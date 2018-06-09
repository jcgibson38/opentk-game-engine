using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JGameEngine.Utils
{
    class JFileUtils
    {
        public static string GetPathToResFile(string fileName)
        {
            string resPath = "C:\\Users\\Jordan\\source\\repos\\JGameEngine_v1\\JGameEngine\\res\\";

            Console.WriteLine("Reading file: " + resPath + fileName);

            return resPath + fileName;
        }

        public static string GetPathToFile(string fileName)
        {
            string resPath = "C:\\Users\\Jordan\\source\\repos\\JGameEngine_v1\\JGameEngine\\";

            Console.WriteLine("Reading file: " + resPath + fileName);

            return resPath + fileName;
        }

        public static string GetPathToShader(string fileName)
        {
            string resPath = "C:\\Users\\Jordan\\source\\repos\\JGameEngine_v1\\JGameEngine\\Shaders\\";

            Console.WriteLine("Reading file: " + resPath + fileName);

            return resPath + fileName;
        }
    }
}
