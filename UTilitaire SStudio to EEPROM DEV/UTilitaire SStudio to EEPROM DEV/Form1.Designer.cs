namespace WindowsFormsApplication1
{
    partial class Form1
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.BROWSE = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.Preview = new System.Windows.Forms.GroupBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.button1 = new System.Windows.Forms.Button();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.stateDisplay = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.comboBoxCOM = new System.Windows.Forms.ComboBox();
            this.button6 = new System.Windows.Forms.Button();
            this.button11 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.button3 = new System.Windows.Forms.Button();
            this.PortNOK = new System.Windows.Forms.PictureBox();
            this.PortOK = new System.Windows.Forms.PictureBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.dateDisplay = new System.Windows.Forms.Label();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.checkBox3 = new System.Windows.Forms.CheckBox();
            this.comboBoxEEPROM = new System.Windows.Forms.ComboBox();
            this.button2 = new System.Windows.Forms.Button();
            this.Preview.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PortNOK)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PortOK)).BeginInit();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(28, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Void";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 16);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(31, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "none";
            // 
            // BROWSE
            // 
            this.BROWSE.Location = new System.Drawing.Point(21, 121);
            this.BROWSE.Name = "BROWSE";
            this.BROWSE.Size = new System.Drawing.Size(145, 33);
            this.BROWSE.TabIndex = 6;
            this.BROWSE.Text = "Browse for E2Prom.Hex";
            this.BROWSE.UseVisualStyleBackColor = true;
            this.BROWSE.Click += new System.EventHandler(this.BROWSE_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 16);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(31, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "none";
            // 
            // Preview
            // 
            this.Preview.Controls.Add(this.label2);
            this.Preview.Location = new System.Drawing.Point(302, 176);
            this.Preview.Name = "Preview";
            this.Preview.Size = new System.Drawing.Size(337, 155);
            this.Preview.TabIndex = 14;
            this.Preview.TabStop = false;
            this.Preview.Tag = "";
            this.Preview.Text = "Hex file preview";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(627, 35);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Input file";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Location = new System.Drawing.Point(12, 49);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(627, 35);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Output file";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(464, 121);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(175, 33);
            this.button1.TabIndex = 15;
            this.button1.Text = "Save as Intel Hex file";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // stateDisplay
            // 
            this.stateDisplay.AutoSize = true;
            this.stateDisplay.Location = new System.Drawing.Point(6, 16);
            this.stateDisplay.Name = "stateDisplay";
            this.stateDisplay.Size = new System.Drawing.Size(24, 13);
            this.stateDisplay.TabIndex = 17;
            this.stateDisplay.Text = "Idle";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.stateDisplay);
            this.groupBox3.Location = new System.Drawing.Point(302, 337);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(337, 44);
            this.groupBox3.TabIndex = 18;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Status";
            // 
            // comboBoxCOM
            // 
            this.comboBoxCOM.FormattingEnabled = true;
            this.comboBoxCOM.Location = new System.Drawing.Point(21, 204);
            this.comboBoxCOM.Name = "comboBoxCOM";
            this.comboBoxCOM.Size = new System.Drawing.Size(91, 21);
            this.comboBoxCOM.TabIndex = 22;
            this.comboBoxCOM.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(127, 278);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(145, 33);
            this.button6.TabIndex = 23;
            this.button6.Text = "Program EEPROM";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // button11
            // 
            this.button11.Location = new System.Drawing.Point(142, 348);
            this.button11.Name = "button11";
            this.button11.Size = new System.Drawing.Size(130, 33);
            this.button11.TabIndex = 28;
            this.button11.Text = "Verify";
            this.button11.UseVisualStyleBackColor = true;
            this.button11.Click += new System.EventHandler(this.button11_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(18, 97);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(400, 13);
            this.label1.TabIndex = 29;
            this.label1.Text = "Step 1 - Open the file \"E2Prom.Hex\" from Sigma Studio or open a standard HEX file" +
                "";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(18, 176);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(140, 13);
            this.label6.TabIndex = 31;
            this.label6.Text = "Step 2 - Choose a COM port";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(18, 254);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(206, 13);
            this.label9.TabIndex = 34;
            this.label9.Text = "Step 3 - Select and program the EEPROM";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(18, 327);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(149, 13);
            this.label10.TabIndex = 35;
            this.label10.Text = "Step 4 - Verify the data written";
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(302, 121);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(145, 33);
            this.button3.TabIndex = 36;
            this.button3.Text = "Browse for Intel Hex file";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click_1);
            // 
            // PortNOK
            // 
            this.PortNOK.Image = ((System.Drawing.Image)(resources.GetObject("PortNOK.Image")));
            this.PortNOK.Location = new System.Drawing.Point(127, 200);
            this.PortNOK.Name = "PortNOK";
            this.PortNOK.Size = new System.Drawing.Size(39, 34);
            this.PortNOK.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.PortNOK.TabIndex = 40;
            this.PortNOK.TabStop = false;
            // 
            // PortOK
            // 
            this.PortOK.Image = ((System.Drawing.Image)(resources.GetObject("PortOK.Image")));
            this.PortOK.Location = new System.Drawing.Point(127, 200);
            this.PortOK.Name = "PortOK";
            this.PortOK.Size = new System.Drawing.Size(39, 34);
            this.PortOK.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.PortOK.TabIndex = 41;
            this.PortOK.TabStop = false;
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(172, 131);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(116, 17);
            this.checkBox1.TabIndex = 43;
            this.checkBox1.Text = "Check for changes";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 200;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // dateDisplay
            // 
            this.dateDisplay.AutoSize = true;
            this.dateDisplay.Location = new System.Drawing.Point(19, 157);
            this.dateDisplay.Name = "dateDisplay";
            this.dateDisplay.Size = new System.Drawing.Size(30, 13);
            this.dateDisplay.TabIndex = 44;
            this.dateDisplay.Text = "Date";
            // 
            // checkBox2
            // 
            this.checkBox2.AutoSize = true;
            this.checkBox2.Location = new System.Drawing.Point(173, 200);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(107, 17);
            this.checkBox2.TabIndex = 45;
            this.checkBox2.Text = "Invert RTS (SCL)";
            this.checkBox2.UseVisualStyleBackColor = true;
            this.checkBox2.CheckedChanged += new System.EventHandler(this.checkBox2_CheckedChanged);
            // 
            // checkBox3
            // 
            this.checkBox3.AutoSize = true;
            this.checkBox3.Location = new System.Drawing.Point(173, 219);
            this.checkBox3.Name = "checkBox3";
            this.checkBox3.Size = new System.Drawing.Size(110, 17);
            this.checkBox3.TabIndex = 46;
            this.checkBox3.Text = "Invert DTR (SDA)";
            this.checkBox3.UseVisualStyleBackColor = true;
            this.checkBox3.CheckedChanged += new System.EventHandler(this.checkBox3_CheckedChanged);
            // 
            // comboBoxEEPROM
            // 
            this.comboBoxEEPROM.FormattingEnabled = true;
            this.comboBoxEEPROM.Location = new System.Drawing.Point(21, 285);
            this.comboBoxEEPROM.Name = "comboBoxEEPROM";
            this.comboBoxEEPROM.Size = new System.Drawing.Size(91, 21);
            this.comboBoxEEPROM.TabIndex = 47;
            this.comboBoxEEPROM.SelectedIndexChanged += new System.EventHandler(this.comboBoxEEPROM_SelectedIndexChanged);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(12, 348);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(130, 33);
            this.button2.TabIndex = 48;
            this.button2.Text = "Read EEPROM";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // Form1
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.ClientSize = new System.Drawing.Size(655, 392);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.comboBoxEEPROM);
            this.Controls.Add(this.checkBox3);
            this.Controls.Add(this.checkBox2);
            this.Controls.Add(this.dateDisplay);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.PortOK);
            this.Controls.Add(this.PortNOK);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.button11);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.button6);
            this.Controls.Add(this.comboBoxCOM);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.BROWSE);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.Preview);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "130232-I Sigma Studio Serial I²C EEPROM Programmer";
            this.Preview.ResumeLayout(false);
            this.Preview.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PortNOK)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PortOK)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button BROWSE;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox Preview;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.Label stateDisplay;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.ComboBox comboBoxCOM;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Button button11;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.PictureBox PortNOK;
        private System.Windows.Forms.PictureBox PortOK;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label dateDisplay;
        private System.Windows.Forms.CheckBox checkBox2;
        private System.Windows.Forms.CheckBox checkBox3;
        private System.Windows.Forms.ComboBox comboBoxEEPROM;
        private System.Windows.Forms.Button button2;
    }
}

