using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JGameEngine.Utils
{
    public class JPerlinNoise
    {
        private static readonly int[] HashLookupTable =
        {
            151,160,137,91,90,15,
            131,13,201,95,96,53,194,233,7,225,140,36,103,30,69,142,8,99,37,240,21,10,23,
            190, 6,148,247,120,234,75,0,26,197,62,94,252,219,203,117,35,11,32,57,177,33,
            88,237,149,56,87,174,20,125,136,171,168, 68,175,74,165,71,134,139,48,27,166,
            77,146,158,231,83,111,229,122,60,211,133,230,220,105,92,41,55,46,245,40,244,
            102,143,54, 65,25,63,161, 1,216,80,73,209,76,132,187,208, 89,18,169,200,196,
            135,130,116,188,159,86,164,100,109,198,173,186, 3,64,52,217,226,250,124,123,
            5,202,38,147,118,126,255,82,85,212,207,206,59,227,47,16,58,17,182,189,28,42,
            223,183,170,213,119,248,152, 2,44,154,163, 70,221,153,101,155,167, 43,172,9,
            129,22,39,253, 19,98,108,110,79,113,224,232,178,185, 112,104,218,246,97,228,
            251,34,242,193,238,210,144,12,191,179,162,241, 81,51,145,235,249,14,239,107,
            49,192,214, 31,181,199,106,157,184, 84,204,176,115,121,50,45,127, 4,150,254,
            138,236,205,93,222,114,67,29,24,72,243,141,128,195,78,66,215,61,156,180
        };

        private static readonly int[] p;

        /// <summary>
        /// Static constructor, initialize p from HashLookupTable.
        /// </summary>
        static JPerlinNoise()
        {
            p = new int[512];
            for(int i=0;i < p.Length; i++)
            {
                p[i] = HashLookupTable[i % 256];
            }
        }

        /// <summary>
        /// Sample the Perlin Noise Map at a coordinate (x,y,z).
        /// </summary>
        /// <param name="x">Coordinate from which to sample the Perlin Noise Map.</param>
        /// <param name="y">Coordinate from which to sample the Perlin Noise Map.</param>
        /// <param name="z">Coordinate from which to sample the Perlin Noise Map.</param>
        /// <returns>The Perlin Noise Map value at (x,y,z).</returns>
        public static double GetNoise(double x, double y, double z)
        {
            int xindex = (int)Math.Floor(x);
            int yindex = (int)Math.Floor(y);
            int zindex = (int)Math.Floor(z);

            // Logival bitwise AND caps index values at 255.
            xindex = xindex & 255;
            yindex = yindex & 255;
            zindex = zindex & 255;

            double dx = x - (int)x;
            double dy = y - (int)y;
            double dz = z - (int)z;

            double u = PerlinFade(dx);
            double v = PerlinFade(dy);
            double w = PerlinFade(dz);

            int m000 = p[p[p[ xindex        ] + yindex      ] + zindex      ];
            int m010 = p[p[p[ xindex        ] + yindex + 1  ] + zindex      ];
            int m100 = p[p[p[ xindex + 1    ] + yindex      ] + zindex      ];
            int m110 = p[p[p[ xindex + 1    ] + yindex + 1  ] + zindex      ];
            int m001 = p[p[p[ xindex        ] + yindex      ] + zindex + 1  ];
            int m011 = p[p[p[ xindex        ] + yindex + 1  ] + zindex + 1  ];
            int m101 = p[p[p[ xindex + 1    ] + yindex      ] + zindex + 1  ];
            int m111 = p[p[p[ xindex + 1    ] + yindex + 1  ] + zindex + 1  ];

            double x1 = JMathUtils.Lerp(    Grad(m000, dx, dy, dz),
                                            Grad(m100, dx - 1, dy, dz),
                                            u
                        );

            double x2 = JMathUtils.Lerp(    Grad(m010, dx, dy - 1, dz),
                                            Grad(m110, dx - 1, dy - 1, dz),
                                            u
                        );

            double y1 = JMathUtils.Lerp(    x1,
                                            x2,
                                            v
                        );

            x1 = JMathUtils.Lerp(           Grad(m001, dx, dy, dz - 1),
                                            Grad(m101, dx-1, dy, dz - 1),
                                            u
                        );

            x2 = JMathUtils.Lerp(           Grad(m011, dx, dy - 1, dz - 1),
                                            Grad(m111, dx - 1, dy - 1, dz - 1),
                                            u
                        );

            double y2 = JMathUtils.Lerp(    x1,
                                            x2,
                                            v
                        );

            return (JMathUtils.Lerp(y1, y2, w) + 1) / 2;
        }

        /// <summary>
        /// Function to smooth the input value, t.
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        private static double PerlinFade(double t)
        {
            return 6 * Math.Pow(t, 5) - 15 * Math.Pow(t, 4) + 10 * Math.Pow(t, 3);
        }

        /// <summary>
        /// Retrieve a pseudorandom gradient vector based on the hash value passed in.
        /// </summary>
        /// <param name="hash"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <returns>A value representing the gradient at (x,y,z).</returns>
        private static double Grad(int hash, double x, double y, double z)
        {
            switch(hash & 0xF)
            {
                case 0x0: return  x + y;
                case 0x1: return -x + y;
                case 0x2: return  x - y;
                case 0x3: return -x - y;
                case 0x4: return  x + z;
                case 0x5: return -x + z;
                case 0x6: return  x - z;
                case 0x7: return -x - z;
                case 0x8: return  y + z;
                case 0x9: return -y + z;
                case 0xA: return  y - z;
                case 0xB: return -y - z;
                case 0xC: return  y + x;
                case 0xD: return -y + z;
                case 0xE: return  y - x;
                case 0xF: return -y - z;
                default:  return  0;
            }
        }
    }
}
