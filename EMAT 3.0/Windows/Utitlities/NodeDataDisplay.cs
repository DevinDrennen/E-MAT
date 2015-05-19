using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Math_Library;
using Sensor_Library;
using EMAT3.Windows.Utilities;

namespace EMAT3.Windows.Utitlities
{
    public partial class NodeDataDisplay : UserControl
    {
        private RazorIMU _node;
        private string _nodePort;
        private int _nodeID;

        private GraphForm[] graphs;

        public NodeDataDisplay(int id, string port, RazorIMU node)
        {
            InitializeComponent();
            _nodeID = id;
            _nodePort = port;
            _node = node;
        }

        private void NodeDataDisplay_Load(object sender, EventArgs e)
        {
            lbl_nodeNum.Text = "Node: " + _nodeID + "," + _nodePort;
            _node.CapturedData += _node_CapturedData;
        }
        private void btn_graph_Click(object sender, EventArgs e)
        {
            graphs = new GraphForm[3];
            graphs[0] = new GraphForm(lbl_nodeNum.Text, "Angle (r)", "X Angle");
            graphs[1] = new GraphForm(lbl_nodeNum.Text, "Angle (r)", "Y Angle");
            graphs[2] = new GraphForm(lbl_nodeNum.Text, "Angle (r)", "Z Angle");

            foreach (GraphForm graph in graphs)
                graph.Show();
        }

        private void _node_CapturedData(float[] data, float deltaT)
        {
            Quaternion q = new Quaternion();
            // Quaternion Data
            lbl_qtn_w.Text = "W: " + q.w.ToString("{0:0.0000}");
            lbl_qtn_x.Text = "X: " + q.x.ToString("{0:0.0000}");
            lbl_qtn_y.Text = "Y: " + q.y.ToString("{0:0.0000}");
            lbl_qtn_z.Text = "Z: " + q.z.ToString("{0:0.0000}");

            // Accelerometer Data
            lbl_accel_x.Text = "X: " + data[0].ToString("{0:0.0000}");
            lbl_accel_y.Text = "Y: " + data[1].ToString("{0:0.0000}");
            lbl_accel_z.Text = "Z: " + data[2].ToString("{0:0.0000}");

            // Gyroscope Data
            lbl_gyro_x.Text = "X: " + data[3].ToString("{0:0.0000}");
            lbl_gyro_y.Text = "Y: " + data[4].ToString("{0:0.0000}");
            lbl_gyro_z.Text = "Z: " + data[5].ToString("{0:0.0000}");

            // Magnometer Data
            lbl_mag_x.Text = "X: " + data[6].ToString("{0:0.0000}");
            lbl_mag_y.Text = "Y: " + data[7].ToString("{0:0.0000}");
            lbl_mag_z.Text = "Z: " + data[8].ToString("{0:0.0000}");

            if (graphs != null)
            {
                float[] angles = Fusion.GetTiltArray(data, deltaT);

                for (int i = 0; i < 3; i++)
                    graphs[i].AddPoint(deltaT, angles[i]);
            }
        }
    }
}
