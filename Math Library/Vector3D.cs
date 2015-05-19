using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Math_Library
{
    public class Vector3D
    {
        #region Variables
        private float[] v = new float[3];

        public float x { get { return v[0]; } set { v[0] = value; } }
        public float y { get { return v[1]; } set { v[1] = value; } }
        public float z { get { return v[2]; } set { v[2] = value; } }
        public float this[int key] { get { return v[key]; } set { v[key] = value; } }
        #endregion


        #region Constructors
        /// <summary>
        /// Constructs a Vector from the x, y, and z parts of a Quaternion
        /// </summary>
        /// <param name="q"></param>
        public Vector3D(Quaternion q)
        {
            v = new float[] { q.x, q.y, q.z };
        }

        /// <summary>
        /// Constructs a Vector from an array float variables
        /// </summary>
        /// <param name="n"></param>
        public Vector3D(float[] n)
        {
            v[0] = n[0];
            v[1] = n[1];
            v[2] = n[2];
        }

        /// <summary>
        /// Constructs a Vector from x, y, z
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        public Vector3D(float x = 0, float y = 0, float z = 0)
        {
            v = new float[] { x, y, z };
        }
        #endregion


        #region Operator Overloading
        public static Vector3D operator +(Vector3D v, Vector3D u)
        {
            return new Vector3D(v.x + u.x, v.y + u.y, v.z + u.z);
        }

        public static Vector3D operator *(float s, Vector3D v)
        {
            return new Vector3D(v.x * s, v.y * s, v.z * s);
        }

        public static Vector3D operator *(Vector3D v, float s)
        {
            return new Vector3D(v.x * s, v.y * s, v.z * s);
        }
        #endregion


        /// <summary>
        /// Returns the magnitude of this Vector
        /// </summary>
        /// <returns></returns>
        public float Magnitude()
        {
            return (float)Math.Sqrt(v[0] * v[0] + v[1] * v[1] + v[2] * v[2]);
        }

        /// <summary>
        /// Normalizes this Vector
        /// </summary>
        /// <returns></returns>
        public Vector3D Normalize()
        {
            float sum = Magnitude();

            x /= sum;
            y /= sum;
            z /= sum;

            return this;
        }

        /// <summary>
        /// Returns a Vector that would be the result of this Vector rotated by a Quaternion.
        /// This method creates a new Vector and does not alter this Vector
        /// </summary>
        /// <param name="q">The quaternion by which to rotate the vector</param>
        /// <returns></returns>
        public Vector3D Rotate(Quaternion q)
        {
            return new Vector3D(q * this * q.Conjugate());
        }
    }
}
