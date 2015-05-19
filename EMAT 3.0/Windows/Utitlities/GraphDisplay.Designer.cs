using OxyPlot.WindowsForms;
namespace EMAT3.Windows.Utilities
{
    partial class GraphForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GraphForm));
            this.Plot = new OxyPlot.WindowsForms.PlotView();
            this.SuspendLayout();
            // 
            // Plot
            // 
            this.Plot.Dock = System.Windows.Forms.DockStyle.Fill;
            //this.Plot.KeyboardPanHorizontalStep = 0.1D;
            //this.Plot.KeyboardPanVerticalStep = 0.1D;
            this.Plot.Location = new System.Drawing.Point(0, 0);
            this.Plot.Margin = new System.Windows.Forms.Padding(4);
            this.Plot.Name = "Plot";
            this.Plot.Padding = new System.Windows.Forms.Padding(2);
            this.Plot.PanCursor = System.Windows.Forms.Cursors.Hand;
            this.Plot.Size = new System.Drawing.Size(570, 455);
            this.Plot.TabIndex = 0;
            this.Plot.ZoomHorizontalCursor = System.Windows.Forms.Cursors.SizeWE;
            this.Plot.ZoomRectangleCursor = System.Windows.Forms.Cursors.SizeNWSE;
            this.Plot.ZoomVerticalCursor = System.Windows.Forms.Cursors.SizeNS;
            // 
            // GraphForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(570, 455);
            this.Controls.Add(this.Plot);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "GraphForm";
            this.Text = "GraphForm";
            this.Load += new System.EventHandler(this.GraphForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private PlotView Plot;
    }
}