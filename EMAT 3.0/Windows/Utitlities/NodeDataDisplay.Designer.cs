namespace EMAT3.Windows.Utitlities
{
    partial class NodeDataDisplay
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lbl_nodeNum = new System.Windows.Forms.Label();
            this.groupBox_accel = new System.Windows.Forms.GroupBox();
            this.lbl_accel_z = new System.Windows.Forms.Label();
            this.lbl_accel_y = new System.Windows.Forms.Label();
            this.lbl_accel_x = new System.Windows.Forms.Label();
            this.groupBox_gyro = new System.Windows.Forms.GroupBox();
            this.lbl_gyro_z = new System.Windows.Forms.Label();
            this.lbl_gyro_y = new System.Windows.Forms.Label();
            this.lbl_gyro_x = new System.Windows.Forms.Label();
            this.groupBox_magno = new System.Windows.Forms.GroupBox();
            this.lbl_mag_z = new System.Windows.Forms.Label();
            this.lbl_mag_y = new System.Windows.Forms.Label();
            this.lbl_mag_x = new System.Windows.Forms.Label();
            this.groupBox_quaternion = new System.Windows.Forms.GroupBox();
            this.lbl_qtn_w = new System.Windows.Forms.Label();
            this.lbl_qtn_z = new System.Windows.Forms.Label();
            this.lbl_qtn_y = new System.Windows.Forms.Label();
            this.lbl_qtn_x = new System.Windows.Forms.Label();
            this.btn_graph = new System.Windows.Forms.Button();
            this.groupBox_accel.SuspendLayout();
            this.groupBox_gyro.SuspendLayout();
            this.groupBox_magno.SuspendLayout();
            this.groupBox_quaternion.SuspendLayout();
            this.SuspendLayout();
            // 
            // lbl_nodeNum
            // 
            this.lbl_nodeNum.AutoSize = true;
            this.lbl_nodeNum.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_nodeNum.Location = new System.Drawing.Point(9, 0);
            this.lbl_nodeNum.Name = "lbl_nodeNum";
            this.lbl_nodeNum.Size = new System.Drawing.Size(49, 13);
            this.lbl_nodeNum.TabIndex = 0;
            this.lbl_nodeNum.Text = "Node #";
            // 
            // groupBox_accel
            // 
            this.groupBox_accel.Controls.Add(this.lbl_accel_z);
            this.groupBox_accel.Controls.Add(this.lbl_accel_y);
            this.groupBox_accel.Controls.Add(this.lbl_accel_x);
            this.groupBox_accel.Location = new System.Drawing.Point(3, 139);
            this.groupBox_accel.Name = "groupBox_accel";
            this.groupBox_accel.Size = new System.Drawing.Size(96, 86);
            this.groupBox_accel.TabIndex = 1;
            this.groupBox_accel.TabStop = false;
            this.groupBox_accel.Text = "Accelerometer";
            // 
            // lbl_accel_z
            // 
            this.lbl_accel_z.AutoSize = true;
            this.lbl_accel_z.Location = new System.Drawing.Point(6, 68);
            this.lbl_accel_z.Name = "lbl_accel_z";
            this.lbl_accel_z.Size = new System.Drawing.Size(17, 13);
            this.lbl_accel_z.TabIndex = 3;
            this.lbl_accel_z.Text = "Z:";
            // 
            // lbl_accel_y
            // 
            this.lbl_accel_y.AutoSize = true;
            this.lbl_accel_y.Location = new System.Drawing.Point(6, 42);
            this.lbl_accel_y.Name = "lbl_accel_y";
            this.lbl_accel_y.Size = new System.Drawing.Size(17, 13);
            this.lbl_accel_y.TabIndex = 2;
            this.lbl_accel_y.Text = "Y:";
            // 
            // lbl_accel_x
            // 
            this.lbl_accel_x.AutoSize = true;
            this.lbl_accel_x.Location = new System.Drawing.Point(6, 16);
            this.lbl_accel_x.Name = "lbl_accel_x";
            this.lbl_accel_x.Size = new System.Drawing.Size(17, 13);
            this.lbl_accel_x.TabIndex = 0;
            this.lbl_accel_x.Text = "X:";
            // 
            // groupBox_gyro
            // 
            this.groupBox_gyro.Controls.Add(this.lbl_gyro_z);
            this.groupBox_gyro.Controls.Add(this.lbl_gyro_y);
            this.groupBox_gyro.Controls.Add(this.lbl_gyro_x);
            this.groupBox_gyro.Location = new System.Drawing.Point(3, 231);
            this.groupBox_gyro.Name = "groupBox_gyro";
            this.groupBox_gyro.Size = new System.Drawing.Size(96, 86);
            this.groupBox_gyro.TabIndex = 4;
            this.groupBox_gyro.TabStop = false;
            this.groupBox_gyro.Text = "Gyroscope";
            // 
            // lbl_gyro_z
            // 
            this.lbl_gyro_z.AutoSize = true;
            this.lbl_gyro_z.Location = new System.Drawing.Point(6, 68);
            this.lbl_gyro_z.Name = "lbl_gyro_z";
            this.lbl_gyro_z.Size = new System.Drawing.Size(17, 13);
            this.lbl_gyro_z.TabIndex = 3;
            this.lbl_gyro_z.Text = "Z:";
            // 
            // lbl_gyro_y
            // 
            this.lbl_gyro_y.AutoSize = true;
            this.lbl_gyro_y.Location = new System.Drawing.Point(6, 42);
            this.lbl_gyro_y.Name = "lbl_gyro_y";
            this.lbl_gyro_y.Size = new System.Drawing.Size(17, 13);
            this.lbl_gyro_y.TabIndex = 2;
            this.lbl_gyro_y.Text = "Y:";
            // 
            // lbl_gyro_x
            // 
            this.lbl_gyro_x.AutoSize = true;
            this.lbl_gyro_x.Location = new System.Drawing.Point(6, 16);
            this.lbl_gyro_x.Name = "lbl_gyro_x";
            this.lbl_gyro_x.Size = new System.Drawing.Size(17, 13);
            this.lbl_gyro_x.TabIndex = 0;
            this.lbl_gyro_x.Text = "X:";
            // 
            // groupBox_magno
            // 
            this.groupBox_magno.Controls.Add(this.lbl_mag_z);
            this.groupBox_magno.Controls.Add(this.lbl_mag_y);
            this.groupBox_magno.Controls.Add(this.lbl_mag_x);
            this.groupBox_magno.Location = new System.Drawing.Point(3, 323);
            this.groupBox_magno.Name = "groupBox_magno";
            this.groupBox_magno.Size = new System.Drawing.Size(96, 86);
            this.groupBox_magno.TabIndex = 4;
            this.groupBox_magno.TabStop = false;
            this.groupBox_magno.Text = "Magnometer";
            // 
            // lbl_mag_z
            // 
            this.lbl_mag_z.AutoSize = true;
            this.lbl_mag_z.Location = new System.Drawing.Point(6, 68);
            this.lbl_mag_z.Name = "lbl_mag_z";
            this.lbl_mag_z.Size = new System.Drawing.Size(17, 13);
            this.lbl_mag_z.TabIndex = 3;
            this.lbl_mag_z.Text = "Z:";
            // 
            // lbl_mag_y
            // 
            this.lbl_mag_y.AutoSize = true;
            this.lbl_mag_y.Location = new System.Drawing.Point(6, 42);
            this.lbl_mag_y.Name = "lbl_mag_y";
            this.lbl_mag_y.Size = new System.Drawing.Size(17, 13);
            this.lbl_mag_y.TabIndex = 2;
            this.lbl_mag_y.Text = "Y:";
            // 
            // lbl_mag_x
            // 
            this.lbl_mag_x.AutoSize = true;
            this.lbl_mag_x.Location = new System.Drawing.Point(6, 16);
            this.lbl_mag_x.Name = "lbl_mag_x";
            this.lbl_mag_x.Size = new System.Drawing.Size(17, 13);
            this.lbl_mag_x.TabIndex = 0;
            this.lbl_mag_x.Text = "X:";
            // 
            // groupBox_quaternion
            // 
            this.groupBox_quaternion.Controls.Add(this.lbl_qtn_w);
            this.groupBox_quaternion.Controls.Add(this.lbl_qtn_z);
            this.groupBox_quaternion.Controls.Add(this.lbl_qtn_y);
            this.groupBox_quaternion.Controls.Add(this.lbl_qtn_x);
            this.groupBox_quaternion.Location = new System.Drawing.Point(3, 16);
            this.groupBox_quaternion.Name = "groupBox_quaternion";
            this.groupBox_quaternion.Size = new System.Drawing.Size(96, 117);
            this.groupBox_quaternion.TabIndex = 4;
            this.groupBox_quaternion.TabStop = false;
            this.groupBox_quaternion.Text = "Quaternion";
            // 
            // lbl_qtn_w
            // 
            this.lbl_qtn_w.AutoSize = true;
            this.lbl_qtn_w.Location = new System.Drawing.Point(6, 16);
            this.lbl_qtn_w.Name = "lbl_qtn_w";
            this.lbl_qtn_w.Size = new System.Drawing.Size(21, 13);
            this.lbl_qtn_w.TabIndex = 4;
            this.lbl_qtn_w.Text = "W:";
            // 
            // lbl_qtn_z
            // 
            this.lbl_qtn_z.AutoSize = true;
            this.lbl_qtn_z.Location = new System.Drawing.Point(6, 94);
            this.lbl_qtn_z.Name = "lbl_qtn_z";
            this.lbl_qtn_z.Size = new System.Drawing.Size(17, 13);
            this.lbl_qtn_z.TabIndex = 3;
            this.lbl_qtn_z.Text = "Z:";
            // 
            // lbl_qtn_y
            // 
            this.lbl_qtn_y.AutoSize = true;
            this.lbl_qtn_y.Location = new System.Drawing.Point(6, 68);
            this.lbl_qtn_y.Name = "lbl_qtn_y";
            this.lbl_qtn_y.Size = new System.Drawing.Size(17, 13);
            this.lbl_qtn_y.TabIndex = 2;
            this.lbl_qtn_y.Text = "Y:";
            // 
            // lbl_qtn_x
            // 
            this.lbl_qtn_x.AutoSize = true;
            this.lbl_qtn_x.Location = new System.Drawing.Point(6, 42);
            this.lbl_qtn_x.Name = "lbl_qtn_x";
            this.lbl_qtn_x.Size = new System.Drawing.Size(17, 13);
            this.lbl_qtn_x.TabIndex = 0;
            this.lbl_qtn_x.Text = "X:";
            // 
            // btn_graph
            // 
            this.btn_graph.Location = new System.Drawing.Point(3, 415);
            this.btn_graph.Name = "btn_graph";
            this.btn_graph.Size = new System.Drawing.Size(96, 42);
            this.btn_graph.TabIndex = 5;
            this.btn_graph.Text = "Show Angle Graph";
            this.btn_graph.UseVisualStyleBackColor = true;
            this.btn_graph.Click += new System.EventHandler(this.btn_graph_Click);
            // 
            // NodeDataDisplay
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LightCyan;
            this.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.Controls.Add(this.btn_graph);
            this.Controls.Add(this.groupBox_quaternion);
            this.Controls.Add(this.groupBox_magno);
            this.Controls.Add(this.groupBox_gyro);
            this.Controls.Add(this.groupBox_accel);
            this.Controls.Add(this.lbl_nodeNum);
            this.Name = "NodeDataDisplay";
            this.Size = new System.Drawing.Size(102, 460);
            this.Load += new System.EventHandler(this.NodeDataDisplay_Load);
            this.groupBox_accel.ResumeLayout(false);
            this.groupBox_accel.PerformLayout();
            this.groupBox_gyro.ResumeLayout(false);
            this.groupBox_gyro.PerformLayout();
            this.groupBox_magno.ResumeLayout(false);
            this.groupBox_magno.PerformLayout();
            this.groupBox_quaternion.ResumeLayout(false);
            this.groupBox_quaternion.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbl_nodeNum;
        private System.Windows.Forms.GroupBox groupBox_accel;
        private System.Windows.Forms.Label lbl_accel_z;
        private System.Windows.Forms.Label lbl_accel_y;
        private System.Windows.Forms.Label lbl_accel_x;
        private System.Windows.Forms.GroupBox groupBox_gyro;
        private System.Windows.Forms.Label lbl_gyro_z;
        private System.Windows.Forms.Label lbl_gyro_y;
        private System.Windows.Forms.Label lbl_gyro_x;
        private System.Windows.Forms.GroupBox groupBox_magno;
        private System.Windows.Forms.Label lbl_mag_z;
        private System.Windows.Forms.Label lbl_mag_y;
        private System.Windows.Forms.Label lbl_mag_x;
        private System.Windows.Forms.GroupBox groupBox_quaternion;
        private System.Windows.Forms.Label lbl_qtn_w;
        private System.Windows.Forms.Label lbl_qtn_z;
        private System.Windows.Forms.Label lbl_qtn_y;
        private System.Windows.Forms.Label lbl_qtn_x;
        private System.Windows.Forms.Button btn_graph;
    }
}
