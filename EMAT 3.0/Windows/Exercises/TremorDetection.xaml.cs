using EMAT3.Utility_Classes;
using Xceed.Wpf.Toolkit;
using OxyPlot;
using OxyPlot.Series;
using Sensor_Library;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Forms;

namespace EMAT3.Windows.Exercises
{
    public partial class TremorDetectionWindow : Window
    {
        private int timeLeft = 0;
        private static int n = 0;
        static int tremorinfo = 0;
        private Boolean readyToStop = false;

        System.Windows.Threading.DispatcherTimer dtmr_countdown = new System.Windows.Threading.DispatcherTimer();

        //Various graphing things.
        float[,] graphData = new float[2, 10];
        LineSeries fLine = new LineSeries { Title = "Frequency", StrokeThickness = 1 };
        LineSeries aLine = new LineSeries { Title = "Amplitude", StrokeThickness = 1 };

        public OxyPlot.Wpf.Plot Plot;

        public TremorDetectionWindow()
        {
            InitializeComponent();

            AccelProcessing.threshold = (decimal)numUpDown_ThresholdFreq.Value;

            dtmr_countdown.Interval = new TimeSpan(0,0,0,0,1000); //1 Second interval
            dtmr_countdown.Tick += new EventHandler(dtmr_countdown_Tick); //Timer event handler.

            grid_advancedSettingsHolder.Visibility = Visibility.Hidden;
            grid_graphHolder.Visibility = Visibility.Hidden;

            //Makes the plot.
            Plot = new OxyPlot.Wpf.Plot();
            Plot.Model = new PlotModel();
            //Plot.Dock = DockStyle.Fill;
            groupBox_graph.Content = Plot;

            Plot.Model.PlotType = PlotType.XY;
            Plot.Model.Background = OxyColor.FromRgb(255, 255, 255);
            //Plot.Model.TextColor = OxyColor.FromRGB(0, 0, 0);

            for (int i = 0; i < 10; i++) //Our data is filled with zeros for now.
            {
                graphData[0, i] = 0;
                fLine.Points.Add(new DataPoint(i, graphData[0, i]));
                graphData[1, i] = 0;
                aLine.Points.Add(new DataPoint(i, graphData[1, i]));
            }

            // add Series and Axis to plot model
            OxyPlot.Axes.LinearAxis fAxis = new OxyPlot.Axes.LinearAxis(OxyPlot.Axes.AxisPosition.Left, 0.0, 15.0); //Makes the axes.
            OxyPlot.Axes.LinearAxis aAxis = new OxyPlot.Axes.LinearAxis(OxyPlot.Axes.AxisPosition.Right, 0.0, 20.0);
            fAxis.Key = "Frequency"; //Sets the key for the and amplitude.
            aAxis.Key = "Amplitude";
            fAxis.Title = "Frequency (Hz)";
            aAxis.Title = "Amplitude (?)";
            fLine.YAxisKey = fAxis.Key; //Assigns the key to the series.
            aLine.YAxisKey = aAxis.Key;
            Plot.Model.Series.Add(fLine); //Adds the data for the frequency.
            Plot.Model.Series.Add(aLine); //Adds the data for the amplitude.
            Plot.Model.Axes.Add(new OxyPlot.Axes.LinearAxis(OxyPlot.Axes.AxisPosition.Bottom, 0.0, 10.0)); //Adds the X axis.
            Plot.Model.Axes.Add(fAxis); //Adds the Y Axis for the frequency
            Plot.Model.Axes.Add(aAxis); //Adds the Y Axis for the amplitude
        }

        private void combobox_nodeSelect_DropDown(object sender, EventArgs e)
        {
            combobox_nodeSelect.Items.Clear();
            Nodes.UpdateAvailableSensors();

            foreach (string sensor in Nodes.Ports)
                combobox_nodeSelect.Items.Add(sensor);
        }

        private void Acceleration_Load(object sender, EventArgs e)
        {
            foreach (RazorIMU node in Nodes.NodesList)
                combobox_nodeSelect.Items.Add(node.Port);
        }

        private void btn_start_Click(object sender, EventArgs e)
        {
            if (combobox_nodeSelect.SelectedItem != null) //Makes sure something is selected.
            {
                if (!dtmr_countdown.IsEnabled)
                {
                    // Disables the changing of the interval and changing the node.
                    numUpDown_Time.IsEnabled = false;
                    combobox_nodeSelect.IsEnabled = false;

                    //Starts the timer.
                    timeLeft = (int)numUpDown_Time.Value;
                    dtmr_countdown.Start();
                    btn_start.Content = "Stop Collection";
                    lbl_countDown.Content = "Countdown: " + timeLeft;

                    //Sets the node.
                    Nodes.NodesList.Clear();
                    Nodes.NodesList.Add(new RazorIMU(combobox_nodeSelect.Text));


                    // Start collecting data.
                    Nodes.NodesList[0].StartCollection();

                    // Start triggering the recieved data event.
                    Nodes.NodesList[0].CapturedData += new RazorDataCaptured(Fusion_CapturedData);

                    //Gets the current time.
                    AccelProcessing.startTime = DateTime.Now.Ticks;
                }
                else
                {
                    readyToStop = true;
                    btn_start.Content = "Stopping...";
                }
            }
        }

        private static void Fusion_CapturedData(float[] data, float deltaT)
        {
            // This is where the data goes. Do your mathimatical magic here.
            // Remember, the structure of data is [accel_x,accel_y,accel_z,gyro_x,gyro_y,gyro_z,mag_x,mag_y,mag_z]
            float[] xyz = new float[] { data[0], data[1], data[2] };
            AccelProcessing.accelData.Add(xyz);
            Utility_Classes.RawDataCollection.LogData(xyz, DateTime.Now, deltaT, "Tremor Detection");
        }

        private void dtmr_countdown_Tick(object sender, EventArgs e)
        {
            if (timeLeft != 0)
            {
                timeLeft -= 1;
                lbl_countDown.Content = "Countdown: " + timeLeft; // Display time left
            }
            else
            {
                // Things here happen after every countdown.
                lbl_countDown.Content = "Countdown: Processing Data...";
                AccelProcessing.time_elapsed = DateTime.Now.Ticks - AccelProcessing.startTime;
                AccelProcessing.ProcessAccel();
                AccelProcessing.accelData.Clear();
                Utility_Classes.RawDataCollection.LogData(AccelProcessing.amp, AccelProcessing.freq, DateTime.Now);
                if (AccelProcessing.freq == -1 || AccelProcessing.amp < (float)numUpDown_ThresholdAmp.Value) //Print out "No Tremor Detected" if Frequency is below a certain threshold. 
                {
                    lbl_tremor.Content = "Frequency: No Tremor Detected";
                    lbl_tremorAmp.Content = "Amplitude: No Tremor Detected";
                    updateGraph(0, 0);
                }
                else
                {
                    lbl_tremor.Content = "Frequency: " + Math.Round(AccelProcessing.freq, 2).ToString() + " Hz";
                    lbl_tremorAmp.Content = "Amplitude: " + Math.Round(AccelProcessing.amp, 2).ToString() + " cm";
                    updateGraph(AccelProcessing.freq, AccelProcessing.amp);
                }

                lbl_averageThreshold.Content = "Average x Threshold: " + AccelProcessing.averageFreqs * (float)AccelProcessing.threshold;
                lbl_average.Content = "Average: " + AccelProcessing.averageFreqs;
                lbl_maxBin.Content = "Largest Value: " + AccelProcessing.largestBin;

                if (!readyToStop) // Unless stop button is pressed
                {
                    timeLeft = (int)numUpDown_Time.Value;
                    lbl_countDown.Content = "Countdown: " + timeLeft;
                    AccelProcessing.accelData.Clear();
                    AccelProcessing.startTime = DateTime.Now.Ticks;
                }
                else
                {
                    Debug.Print("READY TO STOP");
                    //Let the user know it's stopping.
                    lbl_countDown.Content = "Countdown: Stopping...";

                    // Stop triggering the data recieved event.
                    Nodes.NodesList[0].CapturedData -= new RazorDataCaptured(Fusion_CapturedData);

                    //Closes off the node.
                    Nodes.NodesList[0].Dispose();

                    // Things here happen after the last countdown.
                    readyToStop = false;
                    btn_start.Content = "Start Collection";
                    lbl_countDown.Content = "Countdown:";

                    //Reenable various options.
                    numUpDown_Time.IsEnabled = true;
                    combobox_nodeSelect.IsEnabled = true;

                    dtmr_countdown.Stop(); // Stop this timer.
                }
            }
        }

        private void Acceleration_FormClosing(object sender, FormClosingEventArgs e) //Fix this
        {
            AccelProcessing.threshold = 4.25M; //Return it to its default value.
        }

        private void checkBox_graph_Checked(object sender, EventArgs e)
        {
            grid_graphHolder.Visibility = Visibility.Visible;
        }

        private void checkBox_advancedSettings_Checked(object sender, EventArgs e)
        {
            grid_advancedSettingsHolder.Visibility = Visibility.Visible;
        }

        private void checkBox_graph_Unchecked(object sender, EventArgs e)
        {
            grid_graphHolder.Visibility = Visibility.Hidden;
        }

        private void checkBox_advancedSettings_Unchecked(object sender, EventArgs e)
        {
            grid_advancedSettingsHolder.Visibility = Visibility.Hidden;
        }


        /// <summary>
        /// Updates the graph known as plot. It pushes the old data down and puts in the new.
        /// </summary>
        /// <param name="freq"></param>
        /// <param name="amp"></param>
        private void updateGraph(float freq, float amp)
        {
            fLine.Points.Clear();
            aLine.Points.Clear();
            for (int i = 0; i < 9; i++)  //Move all the data down a slot so the new data can go in slot 10.
            {
                graphData[0, i] = graphData[0, i + 1];
                graphData[1, i] = graphData[1, i + 1];

            }
            if (freq == -1) //If the F or A is non existant, store it as 0.
                freq = 0;
            if (amp == -1)
                amp = 0;
            graphData[0, 9] = freq;
            graphData[1, 9] = amp;
            for (int i = 0; i < 10; i++) //Adds the data to the graph.
            {
                fLine.Points.Add(new DataPoint(i, graphData[0, i]));
                aLine.Points.Add(new DataPoint(i, graphData[1, i]));
            }
            Plot.Model.InvalidatePlot(true); //Displays the changed data.
        }

        private void numUpDown_ThresholdFreq_ValueChanged(object sender, EventArgs e)
        {
            AccelProcessing.threshold = (decimal) numUpDown_ThresholdFreq.Value;
        }

        private void btn_exit_Click(object sender, EventArgs e)
        {
            if (dtmr_countdown.IsEnabled)
            {
                Debug.Print("PRESSING STOP");
                btn_start_Click(sender, e);
            }

            this.Close();
        }
    }
}
