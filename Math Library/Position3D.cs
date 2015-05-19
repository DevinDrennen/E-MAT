using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Math_Library
{
    public class Position3D
    {
        public float[,] Coordinates = new float[3,2];
        public float X0 { get { return Coordinates[0, 0]; } set { Coordinates[0, 0] = value; } }
        public float Y0 { get { return Coordinates[0, 1]; } set { Coordinates[0, 1] = value; } }
        public float X1 { get { return Coordinates[1, 0]; } set { Coordinates[1, 0] = value; } }
        public float Y1 { get { return Coordinates[1, 1]; } set { Coordinates[1, 1] = value; } }
        public float X2 { get { return Coordinates[2, 0]; } set { Coordinates[2, 0] = value; } }
        public float Y2 { get { return Coordinates[2, 1]; } set { Coordinates[2, 1] = value; } }

        /// <summary>
        /// Constructs a Position from each individual value.
        /// </summary>
        /// <param name="x0"></param>
        /// <param name="y0"></param>
        /// <param name="x1"></param>
        /// <param name="y1"></param>
        /// <param name="x2"></param>
        /// <param name="y2"></param>
        public Position3D(float x0 = 0, float y0 = 0, float x1 = 0, float y1 = 0, float x2 = 0, float y2 = 0)
        {
            Coordinates = new float[3, 2] { { x0, y0 }, { x1, y1 }, { x2, y2 } };
        }
    }
}
