using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Sensor_Library;

namespace EMAT3.Windows.Exercises
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class CollectionWindow : Window
    {
        System.Windows.Threading.DispatcherTimer dtmr_countdown = new System.Windows.Threading.DispatcherTimer();
        static float[] xyzagm = new float[9]; //Stores the data - XYZ Acceleration, Gyroscope, Magnetic


        public CollectionWindow()
        {
            InitializeComponent();

            Nodes.NodesList[0].StartCollection();
            Nodes.NodesList[0].CapturedData += new RazorDataCaptured(Fusion_CapturedData);

            dtmr_countdown.Interval = new TimeSpan(0, 0, 0, 0, 20); //20 ms interval
            dtmr_countdown.Tick += new EventHandler(dtmr_countdown_Tick); //Timer event handler.
            dtmr_countdown.Start();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private static void Fusion_CapturedData(float[] data, float deltaT)
        {
            // This is where the data goes. Do your mathimatical magic here.
            // Remember, the structure of data is [accel_x,accel_y,accel_z,gyro_x,gyro_y,gyro_z,mag_x,mag_y,mag_z]
            Sensor_Library.Fusion.Update(data, deltaT);
        }

        private void dtmr_countdown_Tick(object sender, EventArgs e)
        {
            lbl_output.Content = "W:" + Sensor_Library.Fusion.q.w.ToString() + " X:" + Sensor_Library.Fusion.q.x.ToString() + " Y:" + Sensor_Library.Fusion.q.y.ToString() + " Z:" + Sensor_Library.Fusion.q.z.ToString();
        }
    }
}
