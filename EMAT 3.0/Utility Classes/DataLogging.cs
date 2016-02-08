using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text;
using System.Windows;
using System.Threading.Tasks;

namespace EMAT3.Utility_Classes
{
    /// <summary>
    /// Handles writing raw data to the file. For next version, include hashing of the patient name?
    /// Also, consider making a switch for StreamCreation to allow for different headers.
    /// </summary>
    public static class DataLogging
    {
        // File streams
        private static StreamWriter raw_stream, processed_stream, clinician_stream;

        private static double elapsed = 0;

        #region Properties
        /// <summary>
        /// Current patient's name.
        /// </summary>
        public static string Patient
        {
            get;
            private set;
        }

        /// <summary>
        /// Base file path.
        /// </summary>
        public static string Base_Path
        {
            get;
            private set;
        }

        /// <summary>
        /// Raw data file path.
        /// </summary>
        public static string Raw_Path
        {
            get;
            private set;
        }

        /// <summary>
        /// Raw data file path.
        /// </summary>
        public static string Processed_Path
        {
            get;
            private set;
        }

        /// <summary>
        /// Clinician data file path.
        /// </summary>
        public static string Clinician_Path
        {
            get;
            private set;
        }
        #endregion

        /// <summary>
        /// Prepares the log files.
        /// </summary>
        /// <param name="patientName">Name of the patient.</param>
        public static void InitiateStreams(string patientName)
        {
            Patient = patientName;

            // Creates the patient's personal folder and log file paths.
            Base_Path = Path.Combine(Environment.CurrentDirectory, "Patients", Patient);
            Raw_Path = Path.Combine(Base_Path, Patient + "_Raw.csv");
            Processed_Path = Path.Combine(Base_Path, Patient + "_Processed.csv");
            Clinician_Path = Path.Combine(Base_Path, Patient + "_Clinical.csv");
            Directory.CreateDirectory(Base_Path);

            raw_stream = new StreamWriter(Raw_Path, true);

            raw_stream.WriteLine("");
            raw_stream.WriteLine(Patient + "," + DateTime.Now.ToString());
            raw_stream.WriteLine("Time Delta, Ax, Ay, Az, Gx, Gy, Gz, Mx, My, Mz");
            
            processed_stream = new StreamWriter(Processed_Path, true);

            processed_stream.WriteLine("");
            processed_stream.WriteLine(Patient + "," + DateTime.Now.ToString());
            processed_stream.WriteLine("Time Delta, n0, fs, fft");

            clinician_stream = new StreamWriter(Clinician_Path, true);

            clinician_stream.WriteLine("");
            clinician_stream.WriteLine(Patient + "," + DateTime.Now.ToString());
            clinician_stream.WriteLine("Date, Time, Frequency (Hz), Reported Amplitude (cm)");
        }

        /// <summary>
        /// Writes the raw data to the patient file.
        /// </summary>
        /// <param name="deltaT">Time since previous data capture.</param>
        /// <param name="aX">Acceleration X</param>
        /// <param name="aY">Acceleration Y</param>
        /// <param name="aZ">Acceleration Z</param>
        /// <param name="gX">Gyroscope X</param>
        /// <param name="gY">Gyroscope Y</param>
        /// <param name="gZ">Gyroscope Z</param>
        /// <param name="Mx">Magnometer X</param>
        /// <param name="My">Magnometer Y</param>
        /// <param name="Mz">Magnometer Z</param>
        public static void LogRawData(float deltaT, float aX, float aY, float aZ, float gX, float gY, float gZ, float mX, float mY, float mZ)
        {
            raw_stream.WriteLine(deltaT +
                "," + aX + "," + aY + "," + aZ + 
                "," + gX + "," + gY + "," + gZ + 
                "," + mX + "," + mY + "," + mZ);
        }


        /// <summary>
        /// Writes the data to the patient file for processed data colleciton.
        /// </summary>
        /// <param name="freq">The data to be printed. Should be FFTMag</param>
        /// <param name="freqMag">The DateTime representing when this data was taken.</param>
        /// <param name="n0"></param>
        /// <param name="fs"></param>
        public static void LogProcessedData(float[] freq, float[] freqMag)
        {
            for(int i = 0; i < freq.Length; i++)
                processed_stream.WriteLine(freq[i] + "," + freqMag[i]);
        }


        /// <summary>
        /// Writes the data to the patient file for the clinician to use.
        /// </summary>
        /// <param name="amp">The reported amplitude.</param>
        /// <param name="freq">The reported frequency.</param>
        /// <param name="time">The DataTime representing when this data was taken.</param>
        public static void LogClinicianData(float amp, float freq, DateTime time)
        {
            clinician_stream.WriteLine(time.ToShortDateString() + ", " + time.ToLongTimeString() + ", " + freq.ToString() + ", " + amp.ToString());
        }

        public static void InsertBreak()
        {
            raw_stream.WriteLine(",,,,,,,,,");
            raw_stream.WriteLine("BREAK,,,,,,,,,");
            raw_stream.WriteLine(",,,,,,,,,");

            processed_stream.WriteLine(",");
            processed_stream.WriteLine("BREAK,");
            processed_stream.WriteLine(",");
        }

        /// <summary>
        /// 
        /// </summary>
        public static void Dispose()
        {
            if (raw_stream != null)
                raw_stream.Close();

            if (processed_stream != null)
                processed_stream.Close();

            if (clinician_stream != null)
                clinician_stream.Close();
        }
    }
}
