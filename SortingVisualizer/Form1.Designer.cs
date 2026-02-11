using System.Windows.Forms;
using System;

namespace SortingVisualizer
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;
        private PictureBox pictureBox1;
        private ComboBox cmbAlgorithm;
        private Button btnSort;
        private Button btnShuffle;
        private Button btnStopSort;
        private NumericUpDown numArraySize;
        private Label lblArraySize;
        private NumericUpDown numDelay;
        private Label lblDelay;
        private Label lblStepInfo;
        private Label lblStatus;
        private Label lblAlgorithmInfo;
        private Label lblTitle;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.cmbAlgorithm = new System.Windows.Forms.ComboBox();
            this.btnSort = new System.Windows.Forms.Button();
            this.btnShuffle = new System.Windows.Forms.Button();
            this.btnStopSort = new System.Windows.Forms.Button();
            this.numArraySize = new System.Windows.Forms.NumericUpDown();
            this.lblArraySize = new System.Windows.Forms.Label();
            this.numDelay = new System.Windows.Forms.NumericUpDown();
            this.lblDelay = new System.Windows.Forms.Label();
            this.lblStepInfo = new System.Windows.Forms.Label();
            this.lblStatus = new System.Windows.Forms.Label();
            this.lblAlgorithmInfo = new System.Windows.Forms.Label();
            this.lblTitle = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numArraySize)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numDelay)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox1.BackColor = System.Drawing.Color.White;
            this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pictureBox1.Location = new System.Drawing.Point(12, 40);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(860, 400);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.SizeChanged += new System.EventHandler(this.pictureBox1_SizeChanged);
            // 
            // cmbAlgorithm
            // 
            this.cmbAlgorithm.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cmbAlgorithm.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbAlgorithm.FormattingEnabled = true;
            this.cmbAlgorithm.Location = new System.Drawing.Point(100, 460);
            this.cmbAlgorithm.Name = "cmbAlgorithm";
            this.cmbAlgorithm.Size = new System.Drawing.Size(180, 21);
            this.cmbAlgorithm.TabIndex = 1;
            this.cmbAlgorithm.SelectedIndexChanged += new System.EventHandler(this.cmbAlgorithm_SelectedIndexChanged);
            // 
            // btnSort
            // 
            this.btnSort.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSort.BackColor = System.Drawing.Color.LightGreen;
            this.btnSort.Location = new System.Drawing.Point(300, 460);
            this.btnSort.Name = "btnSort";
            this.btnSort.Size = new System.Drawing.Size(120, 30);
            this.btnSort.TabIndex = 2;
            this.btnSort.Text = "Сортировать";
            this.btnSort.UseVisualStyleBackColor = false;
            this.btnSort.Click += new System.EventHandler(this.btnSort_Click);
            // 
            // btnShuffle
            // 
            this.btnShuffle.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnShuffle.Location = new System.Drawing.Point(430, 460);
            this.btnShuffle.Name = "btnShuffle";
            this.btnShuffle.Size = new System.Drawing.Size(120, 30);
            this.btnShuffle.TabIndex = 3;
            this.btnShuffle.Text = "Перемешать";
            this.btnShuffle.UseVisualStyleBackColor = true;
            this.btnShuffle.Click += new System.EventHandler(this.btnShuffle_Click);
            // 
            // btnStopSort
            // 
            this.btnStopSort.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnStopSort.BackColor = System.Drawing.Color.LightCoral;
            this.btnStopSort.Enabled = false;
            this.btnStopSort.Location = new System.Drawing.Point(560, 460);
            this.btnStopSort.Name = "btnStopSort";
            this.btnStopSort.Size = new System.Drawing.Size(120, 30);
            this.btnStopSort.TabIndex = 4;
            this.btnStopSort.Text = "Остановить";
            this.btnStopSort.UseVisualStyleBackColor = false;
            this.btnStopSort.Click += new System.EventHandler(this.btnStopSort_Click);
            // 
            // numArraySize
            // 
            this.numArraySize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.numArraySize.Location = new System.Drawing.Point(788, 463);
            this.numArraySize.Maximum = new decimal(new int[] {
            300,
            0,
            0,
            0});
            this.numArraySize.Minimum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.numArraySize.Name = "numArraySize";
            this.numArraySize.Size = new System.Drawing.Size(60, 20);
            this.numArraySize.TabIndex = 5;
            this.numArraySize.Value = new decimal(new int[] {
            30,
            0,
            0,
            0});
            this.numArraySize.ValueChanged += new System.EventHandler(this.numArraySize_ValueChanged);
            // 
            // lblArraySize
            // 
            this.lblArraySize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblArraySize.AutoSize = true;
            this.lblArraySize.Location = new System.Drawing.Point(686, 465);
            this.lblArraySize.Name = "lblArraySize";
            this.lblArraySize.Size = new System.Drawing.Size(96, 13);
            this.lblArraySize.TabIndex = 6;
            this.lblArraySize.Text = "Размер массива:";
            // 
            // numDelay
            // 
            this.numDelay.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.numDelay.DecimalPlaces = 1;
            this.numDelay.Increment = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.numDelay.Location = new System.Drawing.Point(100, 500);
            this.numDelay.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numDelay.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.numDelay.Name = "numDelay";
            this.numDelay.Size = new System.Drawing.Size(80, 20);
            this.numDelay.TabIndex = 7;
            this.numDelay.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.numDelay.ValueChanged += new System.EventHandler(this.numDelay_ValueChanged);
            // 
            // lblDelay
            // 
            this.lblDelay.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblDelay.AutoSize = true;
            this.lblDelay.Location = new System.Drawing.Point(12, 503);
            this.lblDelay.Name = "lblDelay";
            this.lblDelay.Size = new System.Drawing.Size(81, 13);
            this.lblDelay.TabIndex = 8;
            this.lblDelay.Text = "Задержка(мс):";
            // 
            // lblStepInfo
            // 
            this.lblStepInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblStepInfo.AutoSize = true;
            this.lblStepInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblStepInfo.Location = new System.Drawing.Point(200, 503);
            this.lblStepInfo.Name = "lblStepInfo";
            this.lblStepInfo.Size = new System.Drawing.Size(82, 15);
            this.lblStepInfo.TabIndex = 9;
            this.lblStepInfo.Text = "Текущий шаг:";
            // 
            // lblStatus
            // 
            this.lblStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblStatus.AutoSize = true;
            this.lblStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblStatus.Location = new System.Drawing.Point(720, 503);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(55, 15);
            this.lblStatus.TabIndex = 10;
            this.lblStatus.Text = "Готово";
            // 
            // lblAlgorithmInfo
            // 
            this.lblAlgorithmInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblAlgorithmInfo.AutoSize = true;
            this.lblAlgorithmInfo.Location = new System.Drawing.Point(12, 463);
            this.lblAlgorithmInfo.Name = "lblAlgorithmInfo";
            this.lblAlgorithmInfo.Size = new System.Drawing.Size(59, 13);
            this.lblAlgorithmInfo.TabIndex = 11;
            this.lblAlgorithmInfo.Text = "Алгоритм:";
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblTitle.Location = new System.Drawing.Point(12, 9);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(231, 20);
            this.lblTitle.TabIndex = 12;
            this.lblTitle.Text = "Визуализатор сортировок";
            // 
            // MainForm
            // 
            this.ClientSize = new System.Drawing.Size(884, 538);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.lblAlgorithmInfo);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.lblStepInfo);
            this.Controls.Add(this.lblDelay);
            this.Controls.Add(this.numDelay);
            this.Controls.Add(this.lblArraySize);
            this.Controls.Add(this.numArraySize);
            this.Controls.Add(this.btnStopSort);
            this.Controls.Add(this.btnShuffle);
            this.Controls.Add(this.btnSort);
            this.Controls.Add(this.cmbAlgorithm);
            this.Controls.Add(this.pictureBox1);
            this.MinimumSize = new System.Drawing.Size(900, 577);
            this.Name = "MainForm";
            this.Text = "Визуализатор алгоритмов сортировки";
            this.Load += new System.EventHandler(this.MainForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numArraySize)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numDelay)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}