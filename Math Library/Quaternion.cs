using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Math_Library
{
    public class Quaternion
    {
        // If you're not sure how Quaternions work, it's best to just leave this class alone
        // Quaternions are somewhat like compact rotation matrices that have different multiplication rules

        #region Variables
        private float q0;
        private Vector3D v;

        public float w { get { return q0; } set { q0 = value; } }
        public float x { get { return v[0]; } set { v[0] = value; } }
        public float y { get { return v[1]; } set { v[1] = value; } }
        public float z { get { return v[2]; } set { v[2] = value; } }

        public float this[int key] { get { return Get(key); } set { Set(key, value); } }

        private float Get(int key)
        {
            switch (key)
            {
                case 0:
                    return w;
                case 1:
                    return x;
                case 2:
                    return y;
                case 3:
                    return z;
                default:
                    return 0;
            }
        }

        private void Set(int key, float value)
        {
            switch (key)
            {
                case 0:
                    w = value;
                    break;
                case 1:
                    x = value;
                    break;
                case 2:
                    y = value;
                    break;
                case 3:
                    z = value;
                    break;
            }
        }
        #endregion


        #region Constructors
        /// <summary>
        /// Default Quaternion constructor.
        /// Constructs the identity Quaternion
        /// </summary>
        public Quaternion()
        {
            q0 = 1;
            v = new Vector3D();
        }

        /// <summary>
        /// Constructs a Quaternion from w, x, y, z
        /// </summary>
        /// <param name="w"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        public Quaternion(float w, float x, float y, float z)
        {
            q0 = w;
            v = new Vector3D(x, y, z);
        }

        /// <summary>
        /// Constructs a Quaternion by copying a float value and a Vector3D
        /// </summary>
        /// <param name="w"></param>
        /// <param name="n"></param>
        public Quaternion(float w, Vector3D n)
        {
            q0 = w;
            v = n;
        }

        /// <summary>
        /// Costructs a Quaternion from an array of floats
        /// </summary>
        /// <param name="n"></param>
        public Quaternion(float[] n)
        {
            q0 = n[0];
            v = new Vector3D(n[1], n[2], n[3]);
        }

        /// <summary>
        /// Constructs a Quaternion from an axis of rotation and an amount of rotation (in radians)
        /// </summary>
        /// <param name="axis"></param>
        /// <param name="rotation"></param>
        public Quaternion(Vector3D axis, double rotation)
        {
            q0 = (float)Math.Cos(rotation / 2);
            v = axis * (float)Math.Sin(rotation / 2);
        }
        #endregion


        #region Operator Overloading
        public static Quaternion operator *(Quaternion q, Quaternion p)
        {
            Quaternion r = new Quaternion(0, 0, 0, 0);

            r.w = q.w * p.w - (float)QMath.Dot(q.v, p.v);
            r.v = q.w * p.v + p.w * q.v + QMath.Cross(q.v, p.v);

            return r;
        }


        public static Quaternion operator *(Vector3D v, Quaternion p)
        {
            Quaternion r = new Quaternion(0, 0, 0, 0);

            r.w = 0;
            r.v = v;

            return r * p;
        }

        public static Quaternion operator *(Quaternion p, Vector3D v)
        {
            Quaternion r = new Quaternion(0, 0, 0, 0);

            r.w = 0;
            r.v = v;

            return p * r;
        }
        #endregion


        /// <summary>
        /// Returns the conjugate of this Quaternion
        /// </summary>
        /// <returns></returns>
        public Quaternion Conjugate()
        {
            Quaternion r = new Quaternion(w, 0, 0, 0);

            r.v = -1 * v;

            return r;
        }
    }
}
