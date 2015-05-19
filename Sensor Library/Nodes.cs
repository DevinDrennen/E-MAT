using Sensor_Library;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sensor_Library
{
    public static class Nodes
    {
        public static List<RazorIMU> NodesList = new List<RazorIMU>();
        public static List<string> Ports = new List<string>();

        public static void UpdateAvailableSensors()
        {
            string[] ports = SerialPort.GetPortNames();
            SerialPort testCom;
            string line;
            Clear();

            foreach(string port in ports)
            {
                testCom = new SerialPort(port, 57600, Parity.None, 8, StopBits.One);
                Ports.Add(port);
            }
        }

        public static void Initialize(string parent, string child)
        {
            Clear();

            NodesList.Add(new RazorIMU(parent));
            NodesList.Add(new RazorIMU(child));
        }

        public static void Clear()
        {
            Ports.Clear();
            foreach (RazorIMU node in NodesList)
            {
                node.Dispose();
            }
            NodesList.Clear();
        }
    }
}
