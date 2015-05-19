using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Exocortex.DSP;

namespace EMAT3.Utility_Classes
{
    /// <summary>
    /// Handles the data collected for usage with Tremor Detection. Consider changing the name?
    /// Also consider remaking this in F#. 
    /// </summary>
    public static class AccelProcessing
    {
        private static int smoothness = -1;
        public static double startTime = 0;
        public static float freq = 0;
        public static float amp = 0;
        public static float averageFreqs;
        public static float largestBin;
        public static double time_elapsed = 0;
        public static List<float[]> accelData = new List<float[]>();
        public static decimal threshold = 4.25M;

        /// <summary>
        /// Takes in the raw data from the interval and finds the frequency and amplitude, storing them above.
        /// </summary>
        /// <param name="data">The raw data recieved.</param>
        public static void ProcessAccel()
        {
            lock (accelData)
            {
                List<float> magnitudes = new List<float>(); //A list for the magnitudes at each point.

                foreach(float[] part in accelData)
                    magnitudes.Add((float)Math.Sqrt((part[0] * part[0] + part[1] * part[1] + part[2] * part[2])));

                time_elapsed = time_elapsed / TimeSpan.TicksPerSecond; //Calcuatlutes the time elapsed during the collection.

                freq = findFreq(magnitudes.ToArray(), magnitudes.Count / (float)time_elapsed); //Finds the frequecny of the tremor.
                amp = findAmp(magnitudes.ToArray(), freq); //Finds the amplitude of the tremor.
            }
        }

        /// <summary>
        /// calculates the frequency of the tremor from the filtered magnitude of the acceleration data
        /// </summary>
        private static float findFreq(float[] filt, float fs)
        {
            int base2 = (int)Math.Pow(2, Math.Ceiling(Math.Log(filt.Length, 2)));  // gets the smallest power of 2 greater than filt.Length
            float[] comp = RealtoComp(filt);
            Fourier.FFT(comp, base2, FourierDirection.Forward);
            float[] fftMag = getMagnitude(comp);
            float n0 = findPeak(fftMag, fs);
            if (n0 == -1)
            {  // if -1, no peak detected

                RawDataCollection.LogData(fftMag, DateTime.Now, n0, fs);
                return -1;
            }

            RawDataCollection.LogData(fftMag, DateTime.Now, n0, fs);
            return (fs * n0) / (fftMag.Length);  //*2?????
        }

        /// <summary>
        /// converts the float array to a complex number as pair of floats
        /// also ensures that array is base 2 so that FFT can be computed by padding zeros in the center
        /// </summary>
        private static float[] RealtoComp(float[] data)
        {
            float[] comp;
            if (Math.Log(data.Length * 2, 2) == Math.Floor(Math.Log(data.Length * 2, 2)))
            {
                comp = new float[data.Length * 2];
                for (int i = 0; i < data.Length; i++)
                {
                    comp[i * 2] = data[i];
                    comp[i * 2 + 1] = 0;
                }
            }
            else
            {
                int base2 = (int)Math.Pow(2, Math.Ceiling(Math.Log(data.Length * 2, 2)));
                int diff = base2 - data.Length * 2;
                comp = new float[base2];
                int index;
                for (index = 0; index < data.Length / 2; index += 2)
                {
                    comp[index] = data[index / 2];
                    comp[index + 1] = 0;
                }
                for (; index < diff + data.Length / 2; index++)
                {
                    comp[index] = 0;
                }
                for (index = index + 1; index < comp.Length; index += 2)
                {
                    comp[index - 1] = data[(index - diff) / 2];
                    comp[index] = 0;
                }
            }

            return comp;
        }

        /// <summary>
        /// Returns the bin number for the peak of the frequency spectrum from the fft. 
        /// </summary>
        private static float findPeak(float[] data, float fs)
        {
            float max = 0;
            double average=0;
            int count=-1;
            int maxIndex = 0;
            for (int i = (int)(data.Length / (fs / 3)); i < (int)(data.Length / (fs / 15)); i++)   // starts at a frequency of 3 Hz (<3 no tremor detected), stops at a frequency of 15 Hz (Termor is HIGHLY unlikely at this point already.)
            {
                if (!(data[i] > max))
                {
                    average += data[i];
                    count++;
                }
                else
                {
                    if (data[i] > data[i - 1]) //If the first location it checks is on a downward slope, it'll identify it
                    {                          //as a peak, which it isn't. This checks the previous one to make sure that
                        average += max;        //the location actually is a peak.
                        count++;
                        max = data[i];
                        maxIndex = i;
                    }
                    else 
                    {
                        average += data[i]; 
                        count++;
                    }
                }
            }
            averageFreqs = (float) average / count;
            largestBin = max;
            if (data[maxIndex] > (average/count) * Convert.ToDouble(threshold))    //TODO: This value may need changed (threshold)
                return ((float)(maxIndex)); // Only return if this is greater than the average*threshold.
            return -1;  // otherwise return -1
        }

        // calculates the magnitude of the FFT array
        private static float[] getMagnitude(float[] fft)
        {
            float[] mag = new float[fft.Length / 2];
            for (int i = 0; i < mag.Length; i++)
            {
                mag[i] = (float)Math.Sqrt(fft[2 * i] * fft[2 * i] + fft[2 * i + 1] * fft[2 * i + 1]);
            }
            return mag;
        }

        /// <summary>
        /// finds the amplitude of the tremor
        /// </summary>
        private static float findAmp(float[] mag, float f0)
        {
            if (f0 == -1)
                return -1;
            float aveAmp = getAveAmp(mag);
            float f = (((float)1.572 * aveAmp) / (f0 * f0 * (float)Math.PI * 2)) * 10 / 3;
            return f;
        }

        /// <summary>
        /// finds the average max value of the filtered magnitude of the acceleration
        /// </summary>
        private static float getAveAmp(float[] mag)
        {
            float[] mags = mag;
            float aveMag = mags.Average();

            for (int i = 0; i < mags.Length; i++)
            {
                mags[i] = Math.Abs(mags[i]-aveMag);
            }
            return mags.Average();
        }
    }

}
