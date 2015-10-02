using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Math_Library.Quaternion;
using Math_Library.QMath;

namespace Math_Library
{
    class EulerAngle
    {
        private double _alpha, _beta, _gamma;

        /// <summary>
        /// First rotation, about the z axis with angle alpha
        /// </summary>
        public double Alpha
        {
            get { return _alpha;  }
            private set { _alpha = value; }
        }

        /// <summary>
        /// Second rotation, about the x axis by angle betta
        /// </summary>
        public double Beta
        {
            get { return _beta; }
            private set { _beta = value; }
        }

        /// <summary>
        /// Third rotation, about the z axis by angle gamma
        /// </summary>
        public double Gamma
        {
            get { return _gamma; }
            private set { _gamma = value; }
        }

        /// <summary>
        /// Creates a new euler angle given the 3 rotations
        /// </summary>
        /// <param name="alpha">First rotation, about the z axis by angle alpha</param>
        /// <param name="beta">Second rotation, about the x axis by angle beta</param>
        /// <param name="gamma">Third Rotation, about the z axis by angle gamma</param>
        EulerAngle(double alpha, double beta, double gamma)
        {
            Alpha = alpha;
            Beta = beta;
            Gamma = gamma;
        }

        /// <summary>
        /// Creates a new eueler angle using a quaternion
        /// </summary>
        /// <param name="q1">Quaternion being used to create the euler angle</param>
        EulerAngle(Quaternion q1)
        {
            Alpha = Math.Atan2(2 * q1.x * q1.w + 2 * q1.y * q1.z, 1 - 2 * (q1.z * q1.z) + (q1.w * q1.w));
            Beta = Math.Asin(2 * (q1.x * q1.z * q1.w * q1.y));
            Gamma = Math.Atan2(2 * q1.x * q1.y + 2 * q1.z * q1.w, 1 - 2 * (q1.y * q1.y) + (q1.z * q1.z));
        }
    }
}
