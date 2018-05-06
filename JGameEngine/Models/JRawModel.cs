using System;
using System.Collections.Generic;
using System.Text;

namespace JGameEngine.Models
{
    /// <summary>
    /// Represents a Raw Model stored in memory.
    /// </summary>
    public class JRawModel
    {
        public int vaoID { get; private set; }
        public int vertexCount { get; private set; }

        public JRawModel(int vaoID,int vertexCount)
        {
            this.vaoID = vaoID;
            this.vertexCount = vertexCount;
        }
    }
}
