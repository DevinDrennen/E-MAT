using Math_Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sensor_Library
{
    /// <summary>
    /// This is where code should be added to fuse all three sensors 
    /// in order to prevent drift and to smoothen data.
    /// All work here ATM is based on paper, An Efficient Filter for Intertial and Inertial/magnetic sensor arrays
    /// </summary>
    public static class Fusion
    {
        public const float R2D = (float)(180 / Math.PI);
        private static float[] _angleR = new float[3];

        private static float Kp = 2F; //Rate of convergance to acc/mag
        private static float Ki = 0.005F; // Integral gain.
        private static float halfT = .5F; //Half the sample period.
        public static Quaternion q = new Quaternion(1, 0, 0, 0);
        private static float exInt = 0;
        private static float eyInt = 0;
        private static float ezInt = 0;
        private static float bx = 0;
        private static float bz = 0;

        private static float gyroMeasError = (float)Math.PI * (5.0F / 180.0F);
        private static float gyroMeasDrift = (float) Math.PI * (.2F / 180.0F);

        private static float beta = (float) Math.Sqrt(3.0/4.0) * gyroMeasError; //Find this
        private static float zeta = (float) Math.Sqrt(3.0/4.0) * gyroMeasDrift; //FInd this

        /// <summary>
        /// Fuse accelerometer and gyro data to determine tilt angle.
        /// Uses Complementary Angle Filter. 
        /// http://www.pieter-jan.com/node/11
        /// </summary>
        /// <param name="data"></param>
        public static float[] GetTiltArray(float[] data, float dt)
        {
            Vector3D accelVector = new Vector3D(data[0], data[1], data[2]);
            accelVector.Normalize();

            // [x,y,z]
            _angleR[0] = (0.98F) * (_angleR[0] + data[6] * dt) + (0.02F) * (accelVector.x);
            _angleR[1] = (0.98F) * (_angleR[1] + data[7] * dt) + (0.02F) * (accelVector.y);
            _angleR[2] = (0.98F) * (_angleR[2] + data[8] * dt) + (0.02F) * (accelVector.z);

            return _angleR;
        }

        public static void Update(float[] data, float t) //Ax, Ay, Az, Gx, Gy, Gz, Mx, My, Mz
        {
            float norm, vx, vy, vz, hx, hy, hz, wx, wy, wz;
            float ex, ey, ez; //Estimated gyroscope error in x, y and z
            float qHatDotW, qHatDotX, qHatDotY, qHatDotZ; //Gradient
            float wbx = 0, wby = 0, wbz = 0 ; //Estimated gyroscope bias error in x, y and z
            float qDotOmegaW, qDotOmegaX, qDotOmegaY, qDotOmegaZ; //Quaternion rate

            float qwqw = q.w * q.w;
            float qwqx = q.w * q.x;
            float qwqy = q.w * q.y;
            float qwqz = q.w * q.z;
            float qxqx = q.x * q.x;
            float qxqy = q.x * q.y;
            float qxqz = q.x * q.z;
            float qyqy = q.y * q.y;
            float qyqz = q.y * q.z;
            float qzqz = q.z * q.z;

            float twoqw = 2 * q.w;
            float twoqx = 2 * q.x;
            float twoqy = 2 * q.y;
            float twoqz = 2 * q.z;

            float halfqw = .5F * q.w;
            float halfqx = .5F * q.x;
            float halfqy = .5F * q.y;
            float halfqz = .5F * q.z;

            float twobx = 2 * bx;
            float twobz = 2 * bz;

            float twobxqw = twobx * q.w;
            float twobxqx = twobx * q.x;
            float twobxqy = twobx * q.y;
            float twobxqz = twobx * q.z;

            float twobzqw = twobx * q.w;
            float twobzqx = twobx * q.x;
            float twobzqy = twobx * q.y;
            float twobzqz = twobx * q.z;

            t = t / 1000; //Convert t to seconds
            halfT = .5f * t; //Half of t

            //Convert w to rad/s
            data[3] = data[3] * (float) 14.375 / (float) (2 * Math.PI);
            data[4] = data[4] * (float) 14.375 / (float) (2 * Math.PI);
            data[5] = data[5] * (float) 14.375 / (float) (2 * Math.PI);

            //Normalize the accel. data.
            norm = (float)Math.Sqrt((data[0] * data[0]) + (data[1] * data[1]) + (data[2] * data[2]));
            data[0] /= norm;
            data[1] /= norm;
            data[2] /= norm;

            //Normalize the magnet. data.
            norm = (float)Math.Sqrt((data[6] * data[6]) + (data[7] * data[7]) + (data[8] * data[8]));
            data[6] /= norm;
            data[7] /= norm;
            data[8] /= norm;

            // compute the objective function and Jacobian
            float f_1 = twoqx * q.z - twoqw * q.y - data[0];
            float f_2 = twoqw * q.x + twoqy * q.z - data[1];
            float f_3 = 1.0f - twoqx * q.x - twoqy * q.y - data[2];
            float f_4 = twobx * (0.5f - q.y * q.y - q.z * q.z) + twobz * (qxqz - qwqy) - data[6];
            float f_5 = twobx * (q.x * q.y - q.w * q.z) + twobz * (q.w * q.x + q.y * q.z) - data[7];
            float f_6 = twobx * (qwqy + qxqz) + twobz * (0.5f - q.x * q.x - q.y * q.y) - data[8];
            float J_11or24 = twoqy; // J_11 negated in matrix multiplication
            float J_12or23 = 2.0f * q.z;
            float J_13or22 = twoqw; // J_12 negated in matrix multiplication
            float J_14or21 = twoqx;
            float J_32 = 2.0f * J_14or21; // negated in matrix multiplication
            float J_33 = 2.0f * J_11or24; // negated in matrix multiplication
            float J_41 = twobzqy; // negated in matrix multiplication
            float J_42 = twobzqz;
            float J_43 = 2.0f * twobxqy + twobzqw; // negated in matrix multiplication
            float J_44 = 2.0f * twobxqz - twobzqx; // negated in matrix multiplication
            float J_51 = twobxqz - twobzqx; // negated in matrix multiplication
            float J_52 = twobxqy + twobzqw;
            float J_53 = twobxqx + twobzqz;
            float J_54 = twobxqw - twobzqy; // negated in matrix multiplication
            float J_61 = twobxqy;
            float J_62 = twobxqz - 2.0f * twobzqx;
            float J_63 = twobxqw - 2.0f * twobzqy;
            float J_64 = twobxqx;

            //Compute the gradient.
            qHatDotW = J_14or21 * f_2 - J_11or24 * f_1 - J_41 * f_4 - J_51 * f_5 + J_61 * f_6;
            qHatDotX = J_12or23 * f_1 + J_13or22 * f_2 - J_32 * f_3 + J_42 * f_4 + J_52 * f_5 + J_62 * f_6;
            qHatDotY = J_12or23 * f_2 - J_33 * f_3 - J_13or22 * f_1 - J_43 * f_4 + J_53 * f_5 + J_63 * f_6;
            qHatDotZ = J_14or21 * f_1 + J_11or24 * f_2 - J_44 * f_4 - J_54 * f_5 + J_64 * f_6;

            //Normalize the gradient.
            norm = (float) Math.Sqrt(qHatDotW * qHatDotW + qHatDotX * qHatDotX + qHatDotY * qHatDotY + qHatDotZ * qHatDotZ);
            qHatDotW /= norm;
            qHatDotX /= norm;
            qHatDotY /= norm;
            qHatDotZ /= norm;

            //Compute the Gyroscope error based on the gradient.
            ex = twoqw * qHatDotX - twoqx * qHatDotW - twoqy * qHatDotZ + twoqz * qHatDotY;
            ey = twoqw * qHatDotY - twoqy * qHatDotW - twoqz * qHatDotY + twoqy * qHatDotZ;
            ez = twoqw * qHatDotZ - twoqz * qHatDotW - twoqx * qHatDotY + twoqy * qHatDotX;

            wbx += ex * t * zeta;
            wby += ey * t * zeta;
            wbz += ez * t * zeta;
            data[3] -= wbx;
            data[4] -= wby;
            data[5] -= wbz;
            
            //Compute the quaternion rate.
            qDotOmegaW = -halfqx * data[3] - halfqy * data[4] - halfqz * data[5];
            qDotOmegaX = halfqw * data[3] + halfqy * data[5] - halfqz * data[4];
            qDotOmegaY = halfqw * data[4] - halfqx * data[5] + halfqz * data[3];
            qDotOmegaZ = halfqw * data[5] + halfqx * data[4] - halfqy * data[3];

            //Integrate the estimates.
            q.w += (qDotOmegaW - (beta * qHatDotW)) * t;
            q.x += (qDotOmegaX - (beta * qHatDotX)) * t;
            q.y += (qDotOmegaY - (beta * qHatDotY)) * t;
            q.z += (qDotOmegaZ - (beta * qHatDotZ)) * t;

            //Normalize the quaternion
            norm = (float) Math.Sqrt(q.w * q.w + q.x * q.x + q.y * q.y + q.z * q.z);
            q.w = q.w / norm;
            q.x /= norm;
            q.y /= norm;
            q.z /= norm;

            //Compute ze flux
            hx = 2 * data[6] * (0.5F - qyqy - qzqz) + 2 * data[7] * (qxqy - qwqz) + 2 * data[8] * (qxqz + qwqy); //Good
            hy = 2 * data[6] * (qxqy + qwqz) + 2 * data[7] * (0.5F - qxqx - qzqz) + 2 * data[8] * (qyqz - qwqx); //Good
            hz = 2 * data[6] * (qxqz - qwqy) + 2 * data[7] * (qyqz + qwqx) + 2 * data[8] * (0.5F - qxqx - qyqy); //Good
            bx = (float)Math.Sqrt((hx * hx) + (hy * hy)); 
            bz = hz;  
            //return q;
        }

    }
}
