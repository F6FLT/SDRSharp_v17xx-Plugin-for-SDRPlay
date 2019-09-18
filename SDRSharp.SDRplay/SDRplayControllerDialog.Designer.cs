namespace SDRSharp.SDRplay
{
    public partial class SDRplayControllerDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SDRplayControllerDialog));
            this.cboBandwidth = new System.Windows.Forms.ComboBox();
            this.cboIFType = new System.Windows.Forms.ComboBox();
            this.AGCEnabled = new System.Windows.Forms.CheckBox();
            this.numGR = new System.Windows.Forms.NumericUpDown();
            this.ADCsetpoint = new System.Windows.Forms.NumericUpDown();
            this.DefaultBtn = new System.Windows.Forms.Button();
            this.LNAState = new System.Windows.Forms.Label();
            this.MixerStatus = new System.Windows.Forms.Label();
            this.BBGRState = new System.Windows.Forms.Label();
            this.CloseBtn = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.MXRGRState = new System.Windows.Forms.Label();
            this.LNAGRState = new System.Windows.Forms.Label();
            this.LNAGRSlider = new System.Windows.Forms.TrackBar();
            this.numGRSlider = new System.Windows.Forms.TrackBar();
            this.label8 = new System.Windows.Forms.Label();
            this.LNAGRThreshTB = new System.Windows.Forms.TextBox();
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.FQCorrection = new System.Windows.Forms.NumericUpDown();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.label9 = new System.Windows.Forms.Label();
            this.SampleRate = new System.Windows.Forms.ComboBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.numGR)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ADCsetpoint)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LNAGRSlider)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numGRSlider)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.FQCorrection)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // cboBandwidth
            // 
            this.cboBandwidth.DisplayMember = "3";
            this.cboBandwidth.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboBandwidth.FormattingEnabled = true;
            this.cboBandwidth.Items.AddRange(new object[] {
            "0.200 MHz",
            "0.300 MHz",
            "0.600 MHz",
            "1.536 MHz",
            "5.000 MHz",
            "6.000 MHz",
            "7.000 MHz ",
            "8.000 MHz"});
            this.cboBandwidth.Location = new System.Drawing.Point(408, 193);
            this.cboBandwidth.Name = "cboBandwidth";
            this.cboBandwidth.Size = new System.Drawing.Size(78, 21);
            this.cboBandwidth.TabIndex = 25;
            this.cboBandwidth.ValueMember = "3";
            this.cboBandwidth.SelectionChangeCommitted += new System.EventHandler(this.cboBandwidth_SelectedIndexChanged);
            // 
            // cboIFType
            // 
            this.cboIFType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboIFType.FormattingEnabled = true;
            this.cboIFType.Items.AddRange(new object[] {
            "Zero IF",
            "Low IF"});
            this.cboIFType.Location = new System.Drawing.Point(408, 157);
            this.cboIFType.Name = "cboIFType";
            this.cboIFType.Size = new System.Drawing.Size(78, 21);
            this.cboIFType.TabIndex = 25;
            this.cboIFType.SelectionChangeCommitted += new System.EventHandler(this.cboIFType_SelectionChangeCommitted);
            // 
            // AGCEnabled
            // 
            this.AGCEnabled.AutoSize = true;
            this.AGCEnabled.Checked = true;
            this.AGCEnabled.CheckState = System.Windows.Forms.CheckState.Checked;
            this.AGCEnabled.Location = new System.Drawing.Point(794, 349);
            this.AGCEnabled.Name = "AGCEnabled";
            this.AGCEnabled.Size = new System.Drawing.Size(15, 14);
            this.AGCEnabled.TabIndex = 44;
            this.AGCEnabled.UseVisualStyleBackColor = true;
            this.AGCEnabled.CheckedChanged += new System.EventHandler(this.AGCEnabled_CheckedChanged);
            // 
            // numGR
            // 
            this.numGR.Location = new System.Drawing.Point(755, 313);
            this.numGR.Margin = new System.Windows.Forms.Padding(2);
            this.numGR.Maximum = new decimal(new int[] {
            102,
            0,
            0,
            0});
            this.numGR.Name = "numGR";
            this.numGR.Size = new System.Drawing.Size(39, 20);
            this.numGR.TabIndex = 37;
            this.numGR.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.numGR, "The level in dB that has been removed from the maximum receiver gain ");
            this.numGR.Value = new decimal(new int[] {
            60,
            0,
            0,
            0});
            this.numGR.ValueChanged += new System.EventHandler(this.numGR_ValueChanged);
            // 
            // ADCsetpoint
            // 
            this.ADCsetpoint.Location = new System.Drawing.Point(597, 194);
            this.ADCsetpoint.Maximum = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.ADCsetpoint.Minimum = new decimal(new int[] {
            60,
            0,
            0,
            -2147483648});
            this.ADCsetpoint.Name = "ADCsetpoint";
            this.ADCsetpoint.Size = new System.Drawing.Size(58, 20);
            this.ADCsetpoint.TabIndex = 48;
            this.ADCsetpoint.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.ADCsetpoint.ValueChanged += new System.EventHandler(this.ADCsetpoint_ValueChanged);
            // 
            // DefaultBtn
            // 
            this.DefaultBtn.Location = new System.Drawing.Point(642, 393);
            this.DefaultBtn.Name = "DefaultBtn";
            this.DefaultBtn.Size = new System.Drawing.Size(92, 27);
            this.DefaultBtn.TabIndex = 48;
            this.DefaultBtn.Text = "Load Defaults";
            this.DefaultBtn.UseVisualStyleBackColor = true;
            this.DefaultBtn.Click += new System.EventHandler(this.DefaultBtn_Click);
            // 
            // LNAState
            // 
            this.LNAState.AutoSize = true;
            this.LNAState.BackColor = System.Drawing.SystemColors.Window;
            this.LNAState.Location = new System.Drawing.Point(110, 335);
            this.LNAState.Name = "LNAState";
            this.LNAState.Size = new System.Drawing.Size(45, 13);
            this.LNAState.TabIndex = 50;
            this.LNAState.Text = "LNA On";
            // 
            // MixerStatus
            // 
            this.MixerStatus.AutoSize = true;
            this.MixerStatus.BackColor = System.Drawing.SystemColors.Window;
            this.MixerStatus.Location = new System.Drawing.Point(205, 335);
            this.MixerStatus.Name = "MixerStatus";
            this.MixerStatus.Size = new System.Drawing.Size(49, 13);
            this.MixerStatus.TabIndex = 51;
            this.MixerStatus.Text = "Mixer Off";
            // 
            // BBGRState
            // 
            this.BBGRState.AutoSize = true;
            this.BBGRState.BackColor = System.Drawing.SystemColors.Window;
            this.BBGRState.Location = new System.Drawing.Point(418, 350);
            this.BBGRState.Name = "BBGRState";
            this.BBGRState.Size = new System.Drawing.Size(19, 13);
            this.BBGRState.TabIndex = 52;
            this.BBGRState.Text = "36";
            // 
            // CloseBtn
            // 
            this.CloseBtn.Location = new System.Drawing.Point(740, 393);
            this.CloseBtn.Name = "CloseBtn";
            this.CloseBtn.Size = new System.Drawing.Size(75, 27);
            this.CloseBtn.TabIndex = 53;
            this.CloseBtn.Text = "Close";
            this.CloseBtn.UseVisualStyleBackColor = true;
            this.CloseBtn.Click += new System.EventHandler(this.CloseBtn_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(82, 400);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(185, 13);
            this.label1.TabIndex = 54;
            this.label1.Text = "SDRplay Ltd  Version 2.2   Build 1012";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.SystemColors.Window;
            this.label2.Location = new System.Drawing.Point(319, 350);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(93, 13);
            this.label2.TabIndex = 55;
            this.label2.Text = "IF Gain Reduction";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.SystemColors.Window;
            this.label3.Location = new System.Drawing.Point(443, 350);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(20, 13);
            this.label3.TabIndex = 56;
            this.label3.Text = "dB";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.SystemColors.Window;
            this.label4.Location = new System.Drawing.Point(200, 355);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(23, 13);
            this.label4.TabIndex = 57;
            this.label4.Text = "GR";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.SystemColors.Window;
            this.label5.Location = new System.Drawing.Point(156, 355);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(20, 13);
            this.label5.TabIndex = 58;
            this.label5.Text = "dB";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.BackColor = System.Drawing.SystemColors.Window;
            this.label6.Location = new System.Drawing.Point(243, 355);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(20, 13);
            this.label6.TabIndex = 59;
            this.label6.Text = "dB";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.BackColor = System.Drawing.SystemColors.Window;
            this.label7.Location = new System.Drawing.Point(93, 355);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(47, 13);
            this.label7.TabIndex = 60;
            this.label7.Text = "LNA GR";
            // 
            // MXRGRState
            // 
            this.MXRGRState.AutoSize = true;
            this.MXRGRState.BackColor = System.Drawing.SystemColors.Window;
            this.MXRGRState.Location = new System.Drawing.Point(228, 355);
            this.MXRGRState.Name = "MXRGRState";
            this.MXRGRState.Size = new System.Drawing.Size(13, 13);
            this.MXRGRState.TabIndex = 61;
            this.MXRGRState.Text = "0";
            // 
            // LNAGRState
            // 
            this.LNAGRState.AutoSize = true;
            this.LNAGRState.BackColor = System.Drawing.SystemColors.Window;
            this.LNAGRState.Location = new System.Drawing.Point(139, 355);
            this.LNAGRState.Name = "LNAGRState";
            this.LNAGRState.Size = new System.Drawing.Size(13, 13);
            this.LNAGRState.TabIndex = 62;
            this.LNAGRState.Text = "0";
            // 
            // LNAGRSlider
            // 
            this.LNAGRSlider.LargeChange = 1;
            this.LNAGRSlider.Location = new System.Drawing.Point(125, 185);
            this.LNAGRSlider.Maximum = 59;
            this.LNAGRSlider.Name = "LNAGRSlider";
            this.LNAGRSlider.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.LNAGRSlider.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.LNAGRSlider.RightToLeftLayout = true;
            this.LNAGRSlider.Size = new System.Drawing.Size(45, 86);
            this.LNAGRSlider.TabIndex = 63;
            this.LNAGRSlider.TickFrequency = 10;
            this.toolTip1.SetToolTip(this.LNAGRSlider, "Used to set the point at which the LNA changes State");
            this.LNAGRSlider.Value = 59;
            this.LNAGRSlider.Scroll += new System.EventHandler(this.LNAGRSlider_Scroll);
            // 
            // numGRSlider
            // 
            this.numGRSlider.LargeChange = 10;
            this.numGRSlider.Location = new System.Drawing.Point(718, 134);
            this.numGRSlider.Maximum = 102;
            this.numGRSlider.Name = "numGRSlider";
            this.numGRSlider.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.numGRSlider.Size = new System.Drawing.Size(45, 155);
            this.numGRSlider.TabIndex = 64;
            this.numGRSlider.TickFrequency = 5;
            this.numGRSlider.TickStyle = System.Windows.Forms.TickStyle.Both;
            this.toolTip1.SetToolTip(this.numGRSlider, "Used to set the overall gain of the tuner");
            this.numGRSlider.Value = 102;
            this.numGRSlider.Scroll += new System.EventHandler(this.numGRSlider_ValueChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(669, 315);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(81, 13);
            this.label8.TabIndex = 66;
            this.label8.Text = "Gain Reduction";
            // 
            // LNAGRThreshTB
            // 
            this.LNAGRThreshTB.BackColor = System.Drawing.SystemColors.Window;
            this.LNAGRThreshTB.Enabled = false;
            this.LNAGRThreshTB.Location = new System.Drawing.Point(96, 216);
            this.LNAGRThreshTB.Name = "LNAGRThreshTB";
            this.LNAGRThreshTB.ReadOnly = true;
            this.LNAGRThreshTB.Size = new System.Drawing.Size(37, 20);
            this.LNAGRThreshTB.TabIndex = 69;
            this.LNAGRThreshTB.Text = "50";
            this.LNAGRThreshTB.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // FQCorrection
            // 
            this.FQCorrection.DecimalPlaces = 2;
            this.FQCorrection.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.FQCorrection.Location = new System.Drawing.Point(203, 185);
            this.FQCorrection.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.FQCorrection.Minimum = new decimal(new int[] {
            1000,
            0,
            0,
            -2147483648});
            this.FQCorrection.Name = "FQCorrection";
            this.FQCorrection.Size = new System.Drawing.Size(71, 20);
            this.FQCorrection.TabIndex = 70;
            this.FQCorrection.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.toolTip1.SetToolTip(this.FQCorrection, "Used to trim out frequency errors in the reference Xtal");
            this.FQCorrection.ValueChanged += new System.EventHandler(this.FQCorrection_ValueChanged);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.BackColor = System.Drawing.SystemColors.Window;
            this.label9.Location = new System.Drawing.Point(795, 315);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(20, 13);
            this.label9.TabIndex = 71;
            this.label9.Text = "dB";
            // 
            // SampleRate
            // 
            this.SampleRate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.SampleRate.FormattingEnabled = true;
            this.SampleRate.Items.AddRange(new object[] {
            "0.20",
            "0.30",
            "0.40",
            "0.50",
            "0.60",
            "0.75",
            "0.80",
            "1.00",
            "2.00",
            "3.00",
            "4.00",
            "5.00",
            "6.00",
            "7.00",
            "8.00"});
            this.SampleRate.Location = new System.Drawing.Point(597, 157);
            this.SampleRate.MaxDropDownItems = 15;
            this.SampleRate.Name = "SampleRate";
            this.SampleRate.Size = new System.Drawing.Size(58, 21);
            this.SampleRate.TabIndex = 72;
            this.SampleRate.SelectedIndexChanged += new System.EventHandler(this.SampleRate_SelectedIndexChanged);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(11, 55);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(831, 370);
            this.pictureBox1.TabIndex = 73;
            this.pictureBox1.TabStop = false;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox2.Image")));
            this.pictureBox2.Location = new System.Drawing.Point(85, 4);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(169, 45);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox2.TabIndex = 74;
            this.pictureBox2.TabStop = false;
            // 
            // SDRplayControllerDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(844, 432);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.SampleRate);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.LNAGRThreshTB);
            this.Controls.Add(this.FQCorrection);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.LNAGRState);
            this.Controls.Add(this.MXRGRState);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.CloseBtn);
            this.Controls.Add(this.BBGRState);
            this.Controls.Add(this.MixerStatus);
            this.Controls.Add(this.LNAState);
            this.Controls.Add(this.AGCEnabled);
            this.Controls.Add(this.ADCsetpoint);
            this.Controls.Add(this.DefaultBtn);
            this.Controls.Add(this.numGR);
            this.Controls.Add(this.cboIFType);
            this.Controls.Add(this.cboBandwidth);
            this.Controls.Add(this.numGRSlider);
            this.Controls.Add(this.LNAGRSlider);
            this.Controls.Add(this.pictureBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SDRplayControllerDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "SDRplay Configuration";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SDRplayControlDialog_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.numGR)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ADCsetpoint)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LNAGRSlider)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numGRSlider)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.FQCorrection)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cboBandwidth;
        private System.Windows.Forms.ComboBox cboIFType;
        private System.Windows.Forms.CheckBox AGCEnabled;
        private System.Windows.Forms.NumericUpDown numGR;
        private System.Windows.Forms.NumericUpDown ADCsetpoint;
        private System.Windows.Forms.Button DefaultBtn;
        private System.Windows.Forms.Label LNAState;
        private System.Windows.Forms.Label MixerStatus;
        private System.Windows.Forms.Label BBGRState;
        private System.Windows.Forms.Button CloseBtn;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label MXRGRState;
        private System.Windows.Forms.Label LNAGRState;
        private System.Windows.Forms.TrackBar LNAGRSlider;
        private System.Windows.Forms.TrackBar numGRSlider;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox LNAGRThreshTB;
        private System.Windows.Forms.ColorDialog colorDialog1;
        private System.Windows.Forms.NumericUpDown FQCorrection;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox SampleRate;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox pictureBox2;
    }
}