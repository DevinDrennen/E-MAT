using EMAT3.Utility_Classes;
using Xceed.Wpf.Toolkit;
using OxyPlot;
using OxyPlot.Series;
using Sensor_Library;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Forms;
using System.Threading;

namespace EMAT3.Windows.Exercises
{
    public partial class TremorDetectionWindow : Window
    {
        private readonly SynchronizationContext _syncContext;

        // Various ploting things
        public OxyPlot.Wpf.PlotView Plot;
        float[,] graphData = new float[2, 10];
        LineSeries fLine = new LineSeries { Title = "Frequency", StrokeThickness = 1 };
        LineSeries aLine = new LineSeries { Title = "Amplitude", StrokeThickness = 1 };

        private bool running = false;

        /// <summary>
        /// 
        /// </summary>
        public TremorDetectionWindow()
        {
            InitializeComponent();
            _syncContext = SynchronizationContext.Current;

            grid_advancedSettingsHolder.Visibility = Visibility.Hidden;
            grid_graphHolder.Visibility = Visibility.Hidden;

            combobox_nodeSelect.Items.Clear();
            Nodes.UpdateAvailableSensors();

            foreach (string sensor in Nodes.Ports)
                combobox_nodeSelect.Items.Add(sensor);

            if (!combobox_nodeSelect.Items.IsEmpty)
                combobox_nodeSelect.SelectedIndex = 0;

            // Makes the plot.
            Plot = new OxyPlot.Wpf.PlotView();
            Plot.Model = new PlotModel();
            Plot.Model.PlotType = PlotType.XY;
            Plot.Model.Background = OxyColor.FromRgb(255, 255, 255);

            //Plot.Dock = DockStyle.Fill;
            groupBox_graph.Content = Plot;
            //Plot.Model.TextColor = OxyColor.FromRGB(0, 0, 0);

            //Our graph data is filled with zeros for now.
            for (int i = 0; i < 10; i++) 
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
        
        private void btn_start_Click(object sender, RoutedEventArgs e)
        {
            if (!running)
            {
                // If a node has been selected
                if (combobox_nodeSelect.SelectedItem != null)
                {
                    running = true;
                    // Use/Change UI before starting
                    TremorProcessing.Threshold_Frequency = (float)numUpDown_ThresholdFreq.Value;
                    TremorProcessing.Threshold_Amplitude = (float)numUpDown_ThresholdAmp.Value;
                    btn_start.Content = "Stop Collection";

                    // Disables the UI
                    combobox_nodeSelect.IsEnabled = false;
                    numUpDown_ThresholdAmp.IsEnabled = false;
                    numUpDown_ThresholdFreq.IsEnabled = false;

                    /* Clear nodes and add the one selected.
                     * Start collecting data on this node.
                     * Set up the data received event
                    */
                    Nodes.NodesList.Clear();
                    Nodes.NodesList.Add(new RazorIMU(combobox_nodeSelect.Text));
                    Nodes.NodesList[0].StartCollection();
                    Nodes.NodesList[0].CapturedData += new RazorDataCaptured(Fusion_CapturedData);

                    // Set up the tremor processing class and tremor calculated event
                    TremorProcessing.Initialize();
                    TremorProcessing.CalculatedTremor += TremorProcessing_CalculatedTremor;

                }
                else
                    System.Windows.MessageBox.Show("Please select a node", "Collection Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                btn_start.Content = "Start Collection";

                // Enables the UI
                combobox_nodeSelect.IsEnabled = true;
                numUpDown_ThresholdAmp.IsEnabled = true;
                numUpDown_ThresholdFreq.IsEnabled = true;

                // Stop the collection and calculation events
                Nodes.NodesList[0].CapturedData -= Fusion_CapturedData;
                Nodes.NodesList[0].Dispose();
                Nodes.NodesList.Clear();
                TremorProcessing.CalculatedTremor -= TremorProcessing_CalculatedTremor;
                running = false;
            }
        }

        /// <summary>
        /// This is where the sensor data goes when it has been captured.
        /// </summary>
        /// <param name="data">Data from he sensor. Remember, the structure of data is [accel_x,accel_y,accel_z,gyro_x,gyro_y,gyro_z,mag_x,mag_y,mag_z]</param>
        /// <param name="deltaT">Time since previous data was given.</param>
        private static void Fusion_CapturedData(float[] data, float deltaT)
        {
            TremorProcessing.LogPoint(data[0], data[1], data[2], deltaT);
            DataLogging.LogRawData(deltaT, data[0], data[1], data[2], data[3], data[4], data[5], 0, 0, 0);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="frequency"></param>
        /// <param name="amplitude"></param>
        private void TremorProcessing_CalculatedTremor(float frequency, float amplitude)
        {
            // Log clinician data
            DataLogging.LogClinicianData(amplitude, frequency, DateTime.Now);

            // Display and plot no tremor if the freq or magnitude are below threshold
            // Otherwise, plot frequency and magnitude, and display rounded values
            if (amplitude < TremorProcessing.Threshold_Amplitude)
            {
                _syncContext.Send(o =>
                {
                    lbl_tremor.Content = "Frequency: No Tremor Detected";
                    lbl_tremorAmp.Content = "Amplitude: No Tremor Detected";
                    updateGraph();
                }, null);
                
            }
            else
            {
                _syncContext.Send(o =>
                {
                    lbl_tremor.Content = "Frequency: " + Math.Round(frequency, 1) + " Hz";
                    lbl_tremorAmp.Content = "Amplitude: " + Math.Round(amplitude, 2) + " mm";
                    updateGraph(TremorProcessing.Frequency, TremorProcessing.Amplitude);
                }, null);
            }

            _syncContext.Send(o =>
            {
                lbl_averageThreshold.Content = "Average x Threshold: " + TremorProcessing.Average_Frequency * TremorProcessing.Threshold_Frequency;
                lbl_average.Content = "Average: " + TremorProcessing.Average_Frequency;
                lbl_maxBin.Content = "Largest Value: " + TremorProcessing.Largest_Bin;
            }, null);
        }
        
        /// <summary>
        /// Updates the graph known as plot. It pushes the old data down and puts in the new.
        /// </summary>
        /// <param name="freq"></param>
        /// <param name="amp"></param>
        private void updateGraph(float freq = 0, float amp = 0)
        {
            // Shift data down, add new points to end (Queue).
            for (int i = 0; i < 9; i++)
            {
                graphData[0, i] = graphData[0, i + 1];
                graphData[1, i] = graphData[1, i + 1];

            }
            graphData[0, 9] = freq;
            graphData[1, 9] = amp;

            // Clear and update the graph lines.
            fLine.Points.Clear();
            aLine.Points.Clear();
            for (int i = 0; i < 10; i++)
            {
                fLine.Points.Add(new DataPoint(i, graphData[0, i]));
                aLine.Points.Add(new DataPoint(i, graphData[1, i]));
            }
            Plot.Model.InvalidatePlot(true); // Displays the changed data.
        }

        #region UI Things

        /// <summary>
        /// 
        /// </summary>
        private void combobox_nodeSelect_DropDown(object sender, EventArgs e)
        {
            combobox_nodeSelect.Items.Clear();
            Nodes.UpdateAvailableSensors();

            foreach (string sensor in Nodes.Ports)
                combobox_nodeSelect.Items.Add(sensor);
        }

        private void btn_exit_Click(object sender, EventArgs e)
        {
            if(running)
            {
                // Stop the collection and calculation events
                Nodes.NodesList[0].CapturedData -= Fusion_CapturedData;
                TremorProcessing.CalculatedTremor -= TremorProcessing_CalculatedTremor;
                // Stop the collection and calculation events
                Nodes.NodesList[0].Dispose();
                Nodes.NodesList.Clear();
            }
            DataLogging.Dispose();
            this.Close();
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
        private void btn_break_Click(object sender, RoutedEventArgs e)
        {
            DataLogging.InsertBreak();
        }

        #endregion

    }
}
