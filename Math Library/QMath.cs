using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Math_Library
{
    public class QMath
    {
        /// <summary>
        /// Returns the dot product of two Vector3Ds
        /// </summary>
        /// <param name="v"></param>
        /// <param name="u"></param>
        /// <returns></returns>
        public static double Dot(Vector3D v, Vector3D u)
        {
            return v.x * u.x + v.y * u.y + v.z * u.z;
        }

        /// <summary>
        /// Returns the cross product of two Vector3Ds
        /// </summary>
        /// <param name="v"></param>
        /// <param name="u"></param>
        /// <returns></returns>
        public static Vector3D Cross(Vector3D v, Vector3D u)
        {
            Vector3D r = new Vector3D(0, 0, 0);

            r.x = v.y * u.z - v.z * u.y;
            r.y = v.z * u.x - v.x * u.z;
            r.z = v.x * u.y - v.y * u.x;

            return r;
        }

        public static List<Quaternion> Average(List<Quaternion[]> list)
        {
            List<Quaternion> avg = new List<Quaternion>();
            float W, X, Y, Z;
            for (int i = 0; i < list[0].Length; i++)
            {
                W = 0; X = 0; Y = 0; Z = 0;
                for (int j = 0; j < list.Count; j++)
                {
                    W += list[j][i].w;
                    X += list[j][i].x;
                    Y += list[j][i].y;
                    Z += list[j][i].z;
                }
                W /= list.Count;
                X /= list.Count;
                Y /= list.Count;
                Z /= list.Count;
                avg.Add(new Quaternion(W, X, Y, Z));
            }
            return avg;
        }


        /// <summary>
        /// Gets the "anotomic" angles from the orientation of a Quaternion
        /// </summary>
        /// <param name="q">The Quaternion to calculate the angles from</param>
        /// <param name="flexion">Returns the "flexion/extension" of the orientation</param>
        /// <param name="abduction">Returns the "abduction/adduction" of the orientation</param>
        /// <param name="external">Returns the "internal/external rotation" of the orientation</param>
        public static void getAnatomicAngles(Quaternion q, out double flexion, out double abduction, out double external)
        {
            // This method dynamically switches between 2 rotation sequences
            // Both sequences assume '1' is the final axis of rotation
            // This means if you're only flexing from the starting position, you will get only flexion
            // And if you're abducting from the starting position, you will only get abduction
            // This creates a switch in the middle that may make data look noisy in certain spots
            // This was a design decision

            Vector3D vx = new Vector3D(1, 0, 0);
            double angle1 = 0, angle2 = 0, angle3 = 0;

            vx = vx.Rotate(q);

            if (Math.Abs(vx.y) > Math.Abs(vx.z))    // if you're pointing more in the direction of "flexion"
            {
                getEuler321(q, out angle1, out angle2, out angle3);
                flexion = angle1;
                abduction = angle2;
                external = angle3;
            }
            else    // if you're pointing more in the direction of "abduction"
            {
                getEuler231(q, out angle1, out angle2, out angle3);
                abduction = angle1;
                flexion = angle2;
                external = angle3;
            }
        }


        /**
         * The getEuler() methods use equations taken from "Quaternion to Euler Angle Conversion for Arbitrary Rotation Sequence Using Geometric Methods" by Noel H. Hughes
         *
         * The numbers in the methods names represent the desired rotation sequece
         *       For example: 
         *          getEuler231() gets the euler rotations using the 2-3-1 sequence
         *          QG is the quaternion from which you want euler angles
         *          theta1 would represent a rotation about the 2 axis
         *          theta2 would represent a rotation about the 3 axis
         *          theta3 would represent a rotation about the 1 axis
         *          
         * Note: DO NOT USE the getEulerCNR(), getEulerNCNR(), or getEulerCR() methods directly, they are private for a reason
         **/
        #region Circular, Non-Repeated Axis (123, 231, 312)
        private static void getEulerCNR(Quaternion QG, int i1, int i2, int i3, out double theta1, out double theta2, out double theta3)
        {
            Vector3D v3 = new Vector3D(0, 0, 0);
            v3[i3] = 1;

            Vector3D v3n = new Vector3D(0, 0, 0);
            v3n[(i3 + 1) % 3] = 1;

            v3 = v3.Rotate(QG);

            theta1 = Math.Atan2(-v3[(i1 + 1) % 3], v3[(i1 + 2) % 3]);
            theta2 = Math.Asin(v3[i1]);

            Quaternion Q1 = new Quaternion((float)Math.Cos(theta1 / 2), 0, 0, 0);
            Q1[i1 + 1] = (float)Math.Sin(theta1 / 2);

            Quaternion Q2 = new Quaternion((float)Math.Cos(theta2 / 2), 0, 0, 0);
            Q2[i2 + 1] = (float)Math.Sin(theta2 / 2);

            theta3 = getThirdAngle(Q1 * Q2, QG, v3, v3n, theta1, theta2);
        }

        public static void getEuler123(Quaternion QG, out double theta1, out double theta2, out double theta3)
        {
            getEulerCNR(QG, 0, 1, 2, out theta1, out theta2, out theta3);
        }

        public static void getEuler231(Quaternion QG, out double theta1, out double theta2, out double theta3)
        {
            getEulerCNR(QG, 1, 2, 0, out theta1, out theta2, out theta3);
        }

        public static void getEuler312(Quaternion QG, out double theta1, out double theta2, out double theta3)
        {
            getEulerCNR(QG, 2, 0, 1, out theta1, out theta2, out theta3);
        }
        #endregion

        #region Non-circular, Non-repeated Axis (132, 213, 321)
        private static void getEulerNCNR(Quaternion QG, int i1, int i2, int i3, out double theta1, out double theta2, out double theta3)
        {
            Vector3D v3 = new Vector3D(0, 0, 0);
            v3[i3] = 1;

            Vector3D v3n = new Vector3D(0, 0, 0);
            v3n[(i3 + 1) % 3] = 1;

            v3 = v3.Rotate(QG);

            theta1 = Math.Atan2(v3[(i1 + 2) % 3], v3[(i1 + 1) % 3]);
            theta2 = -Math.Asin(v3[i1]);

            Quaternion Q1 = new Quaternion((float)Math.Cos(theta1 / 2), 0, 0, 0);
            Q1[i1 + 1] = (float)Math.Sin(theta1 / 2);

            Quaternion Q2 = new Quaternion((float)Math.Cos(theta2 / 2), 0, 0, 0);
            Q2[i2 + 1] = (float)Math.Sin(theta2 / 2);

            theta3 = getThirdAngle(Q1 * Q2, QG, v3, v3n, theta1, theta2);
        }

        public static void getEuler132(Quaternion QG, out double theta1, out double theta2, out double theta3)
        {
            getEulerNCNR(QG, 0, 2, 1, out theta1, out theta2, out theta3);
        }

        public static void getEuler213(Quaternion QG, out double theta1, out double theta2, out double theta3)
        {
            getEulerNCNR(QG, 1, 0, 2, out theta1, out theta2, out theta3);
        }

        public static void getEuler321(Quaternion QG, out double theta1, out double theta2, out double theta3)
        {
            getEulerNCNR(QG, 2, 1, 0, out theta1, out theta2, out theta3);
        }
        #endregion

        #region Circular, Repeated Axis (121, 232, 313)
        private static void getEulerCR(Quaternion QG, int i1, int i2, int i3, out double theta1, out double theta2, out double theta3)
        {
            Vector3D v3 = new Vector3D(0, 0, 0);
            v3[i3] = 1;

            Vector3D v3n = new Vector3D(0, 0, 0);
            v3n[(i3 + 1) % 3] = 1;

            v3 = v3.Rotate(QG);

            theta1 = Math.Atan2(v3[(i1 + 1) % 3], -v3[(i1 + 2) % 3]);
            theta2 = Math.Acos(v3[i1]);

            Quaternion Q1 = new Quaternion((float)Math.Cos(theta1 / 2), 0, 0, 0);
            Q1[i1 + 1] = (float)Math.Sin(theta1 / 2);

            Quaternion Q2 = new Quaternion((float)Math.Cos(theta2 / 2), 0, 0, 0);
            Q2[i2 + 1] = (float)Math.Sin(theta2 / 2);


            theta3 = getThirdAngle(Q1 * Q2, QG, v3, v3n, theta1, theta2);
        }

        public static void getEuler121(Quaternion QG, out double theta1, out double theta2, out double theta3)
        {
            getEulerCR(QG, 0, 1, 0, out theta1, out theta2, out theta3);
        }

        public static void getEuler232(Quaternion QG, out double theta1, out double theta2, out double theta3)
        {
            getEulerCR(QG, 1, 2, 1, out theta1, out theta2, out theta3);
        }

        public static void getEuler313(Quaternion QG, out double theta1, out double theta2, out double theta3)
        {
            getEulerCR(QG, 2, 0, 2, out theta1, out theta2, out theta3);
        }
        #endregion


        /// <summary>
        /// Calculates theta3 from the given information.
        /// Refer to the paper documented above for detailed info
        /// </summary>
        /// <param name="Q12"></param>
        /// <param name="QG"></param>
        /// <param name="v3"></param>
        /// <param name="v3n"></param>
        /// <param name="theta1"></param>
        /// <param name="theta2"></param>
        /// <returns></returns>
        private static double getThirdAngle(Quaternion Q12, Quaternion QG, Vector3D v3, Vector3D v3n, double theta1, double theta2)
        {
            Vector3D v3n12, v3nG;

            v3n12 = v3nG = v3n;
            v3n12 = v3n12.Rotate(Q12);
            v3nG = v3nG.Rotate(QG);

            return Math.Sign(QMath.Dot(QMath.Cross(v3n12, v3nG), v3)) * Math.Acos(QMath.Dot(v3n12, v3nG));
        }
    }
}
