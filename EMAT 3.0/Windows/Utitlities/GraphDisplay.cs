using OxyPlot;
using OxyPlot.Series;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EMAT3.Windows.Utilities
{
    public partial class GraphForm : Form
    {
        PlotModel PointsModel { get; set; }
        public GraphForm(string graphTitle, string dependentTitle, string functionTitle)
        {
            InitializeComponent();

            PlotModel tmpPlot = new PlotModel(graphTitle);
            tmpPlot.PlotType = PlotType.XY;

            var yAxis = new OxyPlot.Axes.LinearAxis();
            yAxis.Title = dependentTitle;
            tmpPlot.Axes.Add(yAxis);

            PointsModel = tmpPlot;
        }

        private void GraphForm_Load(object sender, EventArgs e)
        {
            Plot.Model = PointsModel;
        }

        public void AddPoint(float x, float y)
        {

        }
    }
}
