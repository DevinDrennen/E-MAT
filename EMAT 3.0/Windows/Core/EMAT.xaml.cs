using EMAT_3._0.Windows;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using EMAT3.Windows.Utilities;
using Sensor_Library;
using EMAT3.Windows.Utitlities;
using EMAT3.Windows.Exercises;
//using Xceed.Wpf.Toolkit;

namespace EMAT3.Windows.Core
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Upon the iwndow being loaded, enable and disable certain boxes.
        /// </summary>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            groupBox_Session.IsEnabled = true;
            groupBox_Sensors.IsEnabled = false;
            groupBox_Motion.IsEnabled = false;
        }

        private void btn_beginSession_Click(object sender, RoutedEventArgs e)
        {
            if (Session.SessionStatus == Session.Status.Uninitialized)
            {
                if (txtBox_patientName.Text != "")
                {
                    txtBox_patientName.IsEnabled = false; //Disable and enable boxes now that the name has been set.
                    btn_beginSession.Content = "End Session";
                    groupBox_Sensors.IsEnabled = true;
                    groupBox_Motion.IsEnabled = true;
                    updateSensors(); //Display all connected sensors.
                    Utility_Classes.RawDataCollection.StreamCreation(txtBox_patientName.Text); //Sets up the file for recording the patient's data.
                    Session.Initialize();
                }
                else
                    System.Windows.MessageBox.Show("Please enter a patient name.", "EMAT - Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if (Session.SessionStatus == Session.Status.Initialized || Session.SessionStatus == Session.Status.Free)
            {
                txtBox_patientName.IsEnabled = true;
                btn_beginSession.Content = "Begin Session";
                groupBox_Sensors.IsEnabled = false;
                groupBox_Motion.IsEnabled = false;
                txtBox_patientName.Text = "";
                Session.Quit();

            }
            else if (Session.SessionStatus == Session.Status.Busy)
            {
                
            }
        }

        private void button_UpdateSensors_Click(object sender, RoutedEventArgs e)
        {
            updateSensors();
        }

        /// <summary>
        /// Gets the list of sensors connected and displays them.
        /// </summary>
        private void updateSensors()
        {
            Nodes.Clear();
            //groupBox_Motion.IsEnabled = false;
            comboBox_Parent.Items.Clear();
            comboBox_Child.Items.Clear();

            Nodes.UpdateAvailableSensors();

            foreach (string port in Nodes.Ports)
            {
                comboBox_Parent.Items.Add(port);
                comboBox_Child.Items.Add(port);
            }

            button_SetSensors.IsEnabled = true;
            comboBox_Parent.IsEnabled = true;
            comboBox_Child.IsEnabled = true;
        }

        private void comboBox_Parent_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (comboBox_Child.SelectedItem == comboBox_Parent.SelectedItem && comboBox_Parent.SelectedIndex!=-1)
            {
                System.Windows.MessageBox.Show("Error, can not select the same nodes.", "EMAT3 - Error");
                comboBox_Parent.SelectedIndex = -1;
            }
            
        }

        private void comboBox_Child_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (comboBox_Child.SelectedItem == comboBox_Parent.SelectedItem && comboBox_Child.SelectedIndex != -1)
            {
                System.Windows.MessageBox.Show("Error, can not select the same nodes.", "EMAT3 - Error");
                comboBox_Child.SelectedIndex = -1;
            }
        }

        private void button_SetSensors_Click(object sender, RoutedEventArgs e)
        {
            if (comboBox_Child.Text != "" && comboBox_Parent.Text != "")
            {
                Nodes.Initialize(comboBox_Parent.Text, comboBox_Child.Text);
                comboBox_Parent.IsEnabled = false;
                comboBox_Child.IsEnabled = false;
                groupBox_Motion.IsEnabled = true;
                btn_tools.IsEnabled = false;
            }
            else
                System.Windows.MessageBox.Show("Error, select nodes for both child and parent.", "EMAT3 - Error");
        }

        private void btn_about_Click(object sender, RoutedEventArgs e)
        {
            (new About()).Show();
        }

        private void btn_tools_Click(object sender, RoutedEventArgs e)
        {
            (new GraphForm("This is a test graph.", "Y-Axis Title" , "Function Title")).Show();
            (new NodeTest()).Show();
        }

        private void btn_exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btn_motion_collect_Click(object sender, RoutedEventArgs e) 
        {
            if (!comboBox_Parent.IsEnabled)
            {
                grid_Controls.Visibility = Visibility.Hidden;
                (new CollectionWindow()).ShowDialog();
                grid_Controls.Visibility = Visibility.Visible;
            }
            else
            {
                System.Windows.MessageBox.Show("You must set your sensors first.");
            }
        }

        private void btn_motion_collectAccel_Click(object sender, RoutedEventArgs e)
        {
            grid_Controls.Visibility = Visibility.Hidden;
            (new TremorDetectionWindow()).ShowDialog();
            grid_Controls.Visibility = Visibility.Visible;
        }

        private void btn_motion_emulate_Click(object sender, RoutedEventArgs e)
        {
            grid_Controls.Visibility = Visibility.Hidden;
            (new NodeVisualizationWindow()).ShowDialog();
            grid_Controls.Visibility = Visibility.Visible;
        }
    }
}
