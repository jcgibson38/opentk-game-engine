using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JGameEngine.Utils
{
    class JVector3
    {
        /// <summary>
        /// The 3D vector.
        /// </summary>
        public Vector3 Vector { get; set; }

        /// <summary>
        /// Indicates whether the value in Vector is ready to be used since Vector3 is non-nullable.
        /// </summary>
        public bool IsSet { get; set; }

        /// <summary>
        /// Initializes a new JVector with Vector set to the Zero Vector and IsSet set to false.
        /// </summary>
        public JVector3()
        {
            Vector = Vector3.Zero;
            IsSet = false;
        }
    }
}
