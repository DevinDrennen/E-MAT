using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Math_Library
{
    class RotationMatrix
    {
        public float[] rotationMatrix = new float[9];
        public float[] transformationMatrix = new float[12];
        public float[] translationVector = new float[3];


        public void Update(Quaternion q)
        {
            float x = q.x;
            float y = q.y;
            float z = q.z;
            float w = q.w;

            rotationMatrix[0] = 1f - ( 2f * y * y ) - ( 2f * z * z );  //11
            rotationMatrix[1] = ( 2f * x * y ) - ( 2f * z * w );       //12
            rotationMatrix[2] = ( 2f * x * z ) + ( 2f * y * w );       //13

            rotationMatrix[2] =  (2f * x * y ) + ( 2f * z * w );       //21
            rotationMatrix[0] = 1f - ( 2f * x * x ) - ( 2f * z * z );  //22
            rotationMatrix[1] = ( 2f * y * z ) - ( 2f * x * w );       //23

            rotationMatrix[1] = ( 2f * x * z ) - ( 2f * y * w );       //31
            rotationMatrix[2] = ( 2f * y * z ) + ( 2f * x * w );       //32
            rotationMatrix[0] = 1f - ( 2f * x * x ) - ( 2f * y * y );  //33



        }
    }
}
