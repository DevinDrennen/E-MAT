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
    public static class RawDataCollection
    {
        public static string patient; //Current Patient.
        private static string path; //Path to location.
        private static string fileRaw; //Current file being used to save raw data.
        private static string fileProc; //Current file being used to save processed data.
        private static string fileClin; //Current file being used to save the data for the clinician.

        /// <summary>
        /// Prepares the files.
        /// </summary>
        /// <param name="name">Name of the patient.</param>
        public static void StreamCreation(string name)
        {
            patient = name;

            path = Path.Combine(Environment.CurrentDirectory, "Patients", patient); //Creates a path to the patient's personal folder.
            Directory.CreateDirectory(path); //Creates the folder.

            fileRaw = Path.Combine(path, patient + "Raw.csv");
            if (!File.Exists(fileRaw)) //If there isn't a file for the patient, set up the heading for the .CSV file.
            {
                using (StreamWriter stream = new StreamWriter(fileRaw))
                    stream.WriteLine("Date and Time, Tick, Current Task, Ax, Ay, Az, Gx, Gy, Gz, Mx, My, Mz"); //Heading for the NameRaw.CSV
            }

            fileProc = Path.Combine(path, patient + "Processed.csv");
            if (!File.Exists(fileProc)) //If there isn't a file for the patient, set up the heading for the .CSV file.
            {
                using (StreamWriter stream = new StreamWriter(fileProc))
                    stream.WriteLine("Date and Time, Tick, Current Task, n0, fs, fft"); //Heading for the NameProcessed.CSV
            }

            fileClin = Path.Combine(path, patient + "Clinical.csv");
            if (!File.Exists(fileClin)) //If there isn't a file for the patient, set up the heading for the .CSV file.
            {
                using (StreamWriter stream = new StreamWriter(fileClin))
                    stream.WriteLine("Date, Time, Frequency (Hz), Reported Amplitude (cm)"); //Heading for the NameClinical.CSV
            }

        }

        /// <summary>
        /// Writes the data to the patient file for raw data collection.
        /// </summary>
        /// <param name="data">The data to be printed. Should be in Axyz Gxyz Mxyz format.</param>
        /// <param name="deltaT">The delta T reported by RazorIMU.cs</param>
        /// <param name="time">The DateTime representing when this data was taken.</param>
        /// <param name="type">What was the task when this was taken?</param>
        public static void LogData(float[] data, DateTime time, float deltaT, string type)
        {
            using(StreamWriter stream = new StreamWriter(fileRaw, true))
                stream.WriteLine(time.ToString() + ", " + deltaT.ToString() + ", " + type + ", " + String.Join(", ", data));
        }


        /// <summary>
        /// Writes the data to the patient file for processed data colleciton.
        /// </summary>
        /// <param name="data">The data to be printed. Should be FFTMag</param>
        /// <param name="time">The DateTime representing when this data was taken.</param>
        /// <param name="n0"></param>
        /// <param name="fs"></param>
        public static void LogData(float[] data, DateTime time, float n0, float fs)
        {
            using (StreamWriter file = new StreamWriter(fileProc, true))
                file.WriteLine(time.ToString() + ", " + time.Ticks.ToString() + ", " + n0 + ", " + fs + ", " + String.Join(", ", data));
        }


        /// <summary>
        /// Writes the data to the patient file for the clinician to use.
        /// </summary>
        /// <param name="amp">The reported amplitude.</param>
        /// <param name="freq">The reported frequency.</param>
        /// <param name="time">The DataTime representing when this data was taken.</param>
        public static void LogData(float amp, float freq, DateTime time)
        {
            using (StreamWriter file = new StreamWriter(fileClin, true))
                file.WriteLine(time.ToShortDateString() + ", " + time.ToLongTimeString() + ", " + freq.ToString() + ", " + amp.ToString());
        }   

    }
}
