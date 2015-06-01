using Math_Library;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Sensor_Library
{
    public delegate void RazorDataCaptured(float[] data, float deltaT);
    

    /// <summary>
    /// 
    /// Object for Sparkfun's 9 Degrees of Freedom - Razor IMU
    /// Product ID SEN-10736
    ///     https://www.sparkfun.com/products/10736
    /// 
    /// Running Sample Firmware
    ///     https://github.com/a1ronzo/SparkFun-9DOF-Razor-IMU-Test-Firmware
    ///     
    /// </summary>
    public class RazorIMU : IDisposable
    {
        private SerialPort _com;
        private static byte[] TOGGLE_AUTORUN = new byte[] { 0x1A };

        public string Port { get; private set; }

        private Thread _updater;
        private string[] _parts;

        public event RazorDataCaptured CapturedData = delegate { };
        private float[] _data = new float[9];
        private Quaternion _qtrn;
        private DateTime oldTime;

        private static Boolean running = false;

        /// <summary>
        /// Create a new instance of a 9DOF Razor IMU
        /// </summary>
        /// <param name="portName">Serial port name. Ex: COM1.</param>
        public RazorIMU(string portName)
        {
            // Create and open the port connection.
            _com = new SerialPort(portName, 57600, Parity.None, 8, StopBits.One);
            _com.Open();
            Port = portName;
            // Set the IMU to automatically collect data if it has not done yet.
            //Thread.Sleep(3000);
            //_com.Write("#s00");
            //_com.DiscardInBuffer();
        }

        /// <summary>
        /// Start continuous collection of data.
        /// </summary>
        public void StartCollection()
        {
            running = true;
            _updater = new Thread(new ThreadStart(ContinuousCollect));
            _updater.Start();
        }

        /// <summary>
        /// Stop continuous collect of data.
        /// </summary>
        public void StopCollection()
        {
            if (_updater != null)
            {
                running = false;
                _updater.Join();
                _updater = null;
            }
        }

        /// <summary>
        /// This method is extremely important. It continously updates the data array.
        /// Data is read and the change in time since the last read is calculated.
        /// The CapturedData event is triggered, sending the new data and the change in time.
        /// </summary>
        private void ContinuousCollect()
        {
            float deltaT = 0;
            oldTime = DateTime.Now;

            while (running) //Static Boolean that controls whether or not to keep running.
            {
                lock (_data)
                    ReadData(ref _data);

                deltaT = (DateTime.Now - oldTime).Ticks / 10000f;
                oldTime = DateTime.Now;
                CapturedData(_data, deltaT);
            }
        }

        /// <summary>
        /// Get a single sample of the 9DOF Razor IMU data. 
        /// <para>Format: [accel_x,accel_y,accel_z,gyro_x,gyro_y,gyro_z,mag_x,mag_y,mag_z]</para>
        /// </summary>
        /// <param name="result">double array of length 9 required.</param>
        private void ReadData(ref float[] result)
        {
            _parts = _com.ReadLine().Split(',');

            if (_parts.Length == 9)
                for (int i = 0; i < 9; i++)
                    result[i] = float.Parse(_parts[i]);
        }

        public void Dispose()
        {
            StopCollection();
            if(_com != null) //Make sure _com exists before closing it.
                _com.Close();
            _com = null;
            _data = null;
            _parts = null;
        }

    }
}
