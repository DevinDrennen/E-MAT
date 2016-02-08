using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Exocortex.DSP;
using System.Diagnostics;

namespace EMAT3.Utility_Classes
{
    public delegate void TremorCalculated(float frequency, float amplitude);

    /// <summary>
    /// Handles the data collected for usage with Tremor Detection. Consider changing the name?
    /// Also consider remaking this in F#. 
    /// </summary>
    public static class TremorProcessing
    {
        public static event TremorCalculated CalculatedTremor = delegate { };

        /// <summary>
        /// Number of data points needed
        /// </summary>
        private const int POINTS_NEEDED = 256;

        /// <summary>
        /// Number of data points needed after padding
        /// </summary>
        private const int PADDING_NEEDED = 2048 - POINTS_NEEDED;

        private static ComplexF[] accelAmplitudes;

        static Stopwatch timer = new Stopwatch();

        /// <summary>
        /// Constants used to make array addressing more clear
        /// </summary>
        private const int X = 0, Y = 1, Z = 2;

        public static float[,] Accel_Data
        { get; private set; }

        public static float[] Time_Data
        { get; private set; }

        public static int Points_Collected
        { get; private set; }

        public static float Threshold_Frequency
        { get; set; }
        public static float Threshold_Amplitude
        { get; set; }

        public static float Frequency
        { get; private set; }

        public static float Average_Frequency
        { get; private set; }
        public static float Sample_Frequency
        { get; private set; }

        public static float Amplitude
        { get; private set; }

        public static float Largest_Bin
        { get; private set; }

        /// <summary>
        /// Initialize the acceleration processing class
        /// </summary>
        public static void Initialize()
        {
            Accel_Data = new float[POINTS_NEEDED, 3];
            Time_Data = new float[POINTS_NEEDED];
            accelAmplitudes = new ComplexF[POINTS_NEEDED + PADDING_NEEDED];
            Sample_Frequency = 46;
            Points_Collected = 0;
        }

        /// <summary>
        /// Add a data point to the float arrays used to calculate frequency and amplitude
        /// </summary>
        public static void LogPoint(float x, float y, float z, float deltaT)
        {
            if (Points_Collected < POINTS_NEEDED)
            {
                if (Points_Collected == 0)
                    timer.Restart();

                Accel_Data[Points_Collected, X] = x;
                Accel_Data[Points_Collected, Y] = y;
                Accel_Data[Points_Collected, Z] = z;

                Points_Collected++; 
            }
            else
            {
                timer.Stop();

                Sample_Frequency = (float)((float)POINTS_NEEDED * 1000F / timer.ElapsedMilliseconds);
                Debug.Print("Sample Frequecy: " + Sample_Frequency);
                Points_Collected = 0;
                CalculateTremor();
                CalculatedTremor(Frequency, Amplitude);
            }
        }

        private static void CalculateTremor()
        {
            GetAccelAmplitudes();
            Fourier.FFT(accelAmplitudes, FourierDirection.Forward);
            float[] fftAmplitudes = GetFFTAmplitudes(accelAmplitudes);
            float[] binFrequencies = new float[(int)(fftAmplitudes.Length)];
            float halfSampleFreq = Sample_Frequency / 2;
            float freq = 0, maxAmp = Threshold_Amplitude;

            for (int binIndex = 0; binIndex < binFrequencies.Length; binIndex++)
            {
                freq = binIndex * halfSampleFreq / fftAmplitudes.Length; // From: (bin_id * freq/2) / N
                binFrequencies[binIndex] = freq;

                if (fftAmplitudes[binIndex] > maxAmp && freq > Threshold_Frequency)
                {
                    maxAmp = fftAmplitudes[binIndex];
                    Amplitude = maxAmp;
                    Frequency = freq;
                }
            }
            
            DataLogging.LogProcessedData(binFrequencies, fftAmplitudes);
        }

        private static void GetAccelAmplitudes()
        {
            float xSqrd, ySqrd, zSqrd;

            for (int i = 0; i < Accel_Data.GetLength(0); i++)
            {
                xSqrd = Accel_Data[i, X] * Accel_Data[i, X];
                ySqrd = Accel_Data[i, Y] * Accel_Data[i, Y];
                zSqrd = Accel_Data[i, Z] * Accel_Data[i, Z];

                accelAmplitudes[i] = new ComplexF((float)Math.Sqrt(xSqrd + ySqrd + zSqrd), 0);
            }

            for (int i = POINTS_NEEDED; i < accelAmplitudes.Length; i++)
                accelAmplitudes[i] = new ComplexF(0, 0);
        }

        private static float[] GetFFTAmplitudes(ComplexF[] accelFFT)
        {
            float[] fftAmplitudes = new float[accelFFT.Length / 2];

            for (int i = 0; i < fftAmplitudes.Length; i++)
                fftAmplitudes[i] = (float)Math.Sqrt(accelFFT[i].Re * accelFFT[i].Re + accelFFT[i].Im * accelFFT[i].Im);

            return fftAmplitudes;
        }
    }

}
