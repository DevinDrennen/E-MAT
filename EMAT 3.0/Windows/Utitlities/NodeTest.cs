using Sensor_Library;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EMAT3.Windows.Utitlities
{
    public partial class NodeTest : Form
    {
        RazorIMU[] nodes;
        public NodeTest()
        {
            InitializeComponent();
        }

        private void NodeTest_Load(object sender, EventArgs e)
        {
            if (Nodes.Ports.Count == 0)
            {
                MessageBox.Show("There are no nodes connected, closing node test.", "EMAT - NodeTest - Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //Close();
                //return;
            }

            MessageBox.Show("There are " + Nodes.Ports.Count + " nodes connected.", "EMAT - NodeTest", MessageBoxButtons.OK, MessageBoxIcon.Information);

            nodes = new RazorIMU[Nodes.Ports.Count];
            Width = 114 * nodes.Length + 12;
            
            CreateDataDisplays();
        }

        private void CreateDataDisplays()
        {
            NodeDataDisplay display;

            for(int i = 0; i < nodes.Length; i++)
            {
                display = new NodeDataDisplay(i, Nodes.Ports[i], nodes[i]);
                display.Name = "ndd_" + i;
                display.Location = new Point(i * 102 + 12, 12);
                Controls.Add(display);
            }
        }
    }
}
