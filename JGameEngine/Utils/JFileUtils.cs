using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JGameEngine.Utils
{
    class JFileUtils
    {
        public static string GetPathToFile(string fileName)
        {
            string resPath = "C:\\Users\\Jordan\\source\\repos\\JGameEngine_v1\\JGameEngine\\res\\";

            return resPath + fileName;
        }
    }
}
