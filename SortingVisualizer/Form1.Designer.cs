using System;
using System.Drawing;
using System.Windows.Forms;

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
        private Button btnResetStats;
        private NumericUpDown numArraySize;
        private Label lblArraySize;
        private NumericUpDown numDelay;
        private Label lblDelay;
        private Label lblStepInfo;
        private Label lblStatus;
        private Label lblAlgorithmInfo;
        private Label lblTitle;
        private ProgressBar progressBar;
        private Label lblSteps;
        private Label lblComparisons;
        private Label lblSwaps;
        private Label lblOpsPerSec;
        private CheckBox chkSound;
        private GroupBox grpStats;

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
            this.btnResetStats = new System.Windows.Forms.Button();
            this.numArraySize = new System.Windows.Forms.NumericUpDown();
            this.lblArraySize = new System.Windows.Forms.Label();
            this.numDelay = new System.Windows.Forms.NumericUpDown();
            this.lblDelay = new System.Windows.Forms.Label();
            this.lblStepInfo = new System.Windows.Forms.Label();
            this.lblStatus = new System.Windows.Forms.Label();
            this.lblAlgorithmInfo = new System.Windows.Forms.Label();
            this.lblTitle = new System.Windows.Forms.Label();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.lblSteps = new System.Windows.Forms.Label();
            this.lblComparisons = new System.Windows.Forms.Label();
            this.lblSwaps = new System.Windows.Forms.Label();
            this.lblOpsPerSec = new System.Windows.Forms.Label();
            this.chkSound = new System.Windows.Forms.CheckBox();
            this.grpStats = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numArraySize)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numDelay)).BeginInit();
            this.grpStats.SuspendLayout();
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
            this.btnSort.Location = new System.Drawing.Point(300, 455);
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
            this.btnShuffle.Location = new System.Drawing.Point(426, 455);
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
            this.btnStopSort.Location = new System.Drawing.Point(552, 455);
            this.btnStopSort.Name = "btnStopSort";
            this.btnStopSort.Size = new System.Drawing.Size(120, 30);
            this.btnStopSort.TabIndex = 4;
            this.btnStopSort.Text = "Остановить";
            this.btnStopSort.UseVisualStyleBackColor = false;
            this.btnStopSort.Click += new System.EventHandler(this.btnStopSort_Click);
            // 
            // btnResetStats
            // 
            this.btnResetStats.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnResetStats.Location = new System.Drawing.Point(696, 455);
            this.btnResetStats.Name = "btnResetStats";
            this.btnResetStats.Size = new System.Drawing.Size(120, 30);
            this.btnResetStats.TabIndex = 5;
            this.btnResetStats.Text = "Сбросить стат.";
            this.btnResetStats.UseVisualStyleBackColor = true;
            this.btnResetStats.Click += new System.EventHandler(this.btnResetStats_Click);
            // 
            // numArraySize
            // 
            this.numArraySize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.numArraySize.Location = new System.Drawing.Point(100, 500);
            this.numArraySize.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numArraySize.Minimum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numArraySize.Name = "numArraySize";
            this.numArraySize.Size = new System.Drawing.Size(80, 20);
            this.numArraySize.TabIndex = 6;
            this.numArraySize.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.numArraySize.ValueChanged += new System.EventHandler(this.numArraySize_ValueChanged);
            // 
            // lblArraySize
            // 
            this.lblArraySize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblArraySize.AutoSize = true;
            this.lblArraySize.Location = new System.Drawing.Point(12, 503);
            this.lblArraySize.Name = "lblArraySize";
            this.lblArraySize.Size = new System.Drawing.Size(66, 13);
            this.lblArraySize.TabIndex = 7;
            this.lblArraySize.Text = "Элементов:";
            // 
            // numDelay
            // 
            this.numDelay.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.numDelay.Location = new System.Drawing.Point(300, 500);
            this.numDelay.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numDelay.Name = "numDelay";
            this.numDelay.Size = new System.Drawing.Size(80, 20);
            this.numDelay.TabIndex = 8;
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
            this.lblDelay.Location = new System.Drawing.Point(200, 503);
            this.lblDelay.Name = "lblDelay";
            this.lblDelay.Size = new System.Drawing.Size(84, 13);
            this.lblDelay.TabIndex = 9;
            this.lblDelay.Text = "Задержка (мс):";
            // 
            // lblStepInfo
            // 
            this.lblStepInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblStepInfo.AutoSize = true;
            this.lblStepInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblStepInfo.Location = new System.Drawing.Point(400, 503);
            this.lblStepInfo.Name = "lblStepInfo";
            this.lblStepInfo.Size = new System.Drawing.Size(82, 15);
            this.lblStepInfo.TabIndex = 10;
            this.lblStepInfo.Text = "Текущий шаг:";
            // 
            // lblStatus
            // 
            this.lblStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblStatus.AutoSize = true;
            this.lblStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblStatus.Location = new System.Drawing.Point(625, 505);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(55, 15);
            this.lblStatus.TabIndex = 11;
            this.lblStatus.Text = "Готово";
            // 
            // lblAlgorithmInfo
            // 
            this.lblAlgorithmInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblAlgorithmInfo.AutoSize = true;
            this.lblAlgorithmInfo.Location = new System.Drawing.Point(12, 463);
            this.lblAlgorithmInfo.Name = "lblAlgorithmInfo";
            this.lblAlgorithmInfo.Size = new System.Drawing.Size(59, 13);
            this.lblAlgorithmInfo.TabIndex = 12;
            this.lblAlgorithmInfo.Text = "Алгоритм:";
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblTitle.Location = new System.Drawing.Point(12, 9);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(231, 20);
            this.lblTitle.TabIndex = 13;
            this.lblTitle.Text = "Визуализатор сортировок";
            // 
            // progressBar
            // 
            this.progressBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar.Location = new System.Drawing.Point(12, 530);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(860, 20);
            this.progressBar.TabIndex = 14;
            this.progressBar.Visible = false;
            // 
            // lblSteps
            // 
            this.lblSteps.AutoSize = true;
            this.lblSteps.Location = new System.Drawing.Point(10, 20);
            this.lblSteps.Name = "lblSteps";
            this.lblSteps.Size = new System.Drawing.Size(51, 13);
            this.lblSteps.TabIndex = 0;
            this.lblSteps.Text = "Шагов: 0";
            // 
            // lblComparisons
            // 
            this.lblComparisons.AutoSize = true;
            this.lblComparisons.Location = new System.Drawing.Point(10, 40);
            this.lblComparisons.Name = "lblComparisons";
            this.lblComparisons.Size = new System.Drawing.Size(74, 13);
            this.lblComparisons.TabIndex = 1;
            this.lblComparisons.Text = "Сравнений: 0";
            // 
            // lblSwaps
            // 
            this.lblSwaps.AutoSize = true;
            this.lblSwaps.Location = new System.Drawing.Point(150, 20);
            this.lblSwaps.Name = "lblSwaps";
            this.lblSwaps.Size = new System.Drawing.Size(65, 13);
            this.lblSwaps.TabIndex = 2;
            this.lblSwaps.Text = "Обменов: 0";
            // 
            // lblOpsPerSec
            // 
            this.lblOpsPerSec.AutoSize = true;
            this.lblOpsPerSec.Location = new System.Drawing.Point(150, 40);
            this.lblOpsPerSec.Name = "lblOpsPerSec";
            this.lblOpsPerSec.Size = new System.Drawing.Size(56, 13);
            this.lblOpsPerSec.TabIndex = 3;
            this.lblOpsPerSec.Text = "Оп/сек: 0";
            // 
            // chkSound
            // 
            this.chkSound.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.chkSound.AutoSize = true;
            this.chkSound.Checked = true;
            this.chkSound.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkSound.Location = new System.Drawing.Point(822, 463);
            this.chkSound.Name = "chkSound";
            this.chkSound.Size = new System.Drawing.Size(50, 17);
            this.chkSound.TabIndex = 15;
            this.chkSound.Text = "Звук";
            this.chkSound.UseVisualStyleBackColor = true;
            // 
            // grpStats
            // 
            this.grpStats.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.grpStats.Controls.Add(this.lblSteps);
            this.grpStats.Controls.Add(this.lblComparisons);
            this.grpStats.Controls.Add(this.lblSwaps);
            this.grpStats.Controls.Add(this.lblOpsPerSec);
            this.grpStats.Location = new System.Drawing.Point(12, 550);
            this.grpStats.Name = "grpStats";
            this.grpStats.Size = new System.Drawing.Size(250, 70);
            this.grpStats.TabIndex = 16;
            this.grpStats.TabStop = false;
            this.grpStats.Text = "Статистика";
            // 
            // MainForm
            // 
            this.ClientSize = new System.Drawing.Size(884, 640);
            this.Controls.Add(this.grpStats);
            this.Controls.Add(this.chkSound);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.lblAlgorithmInfo);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.lblStepInfo);
            this.Controls.Add(this.lblDelay);
            this.Controls.Add(this.numDelay);
            this.Controls.Add(this.lblArraySize);
            this.Controls.Add(this.numArraySize);
            this.Controls.Add(this.btnResetStats);
            this.Controls.Add(this.btnStopSort);
            this.Controls.Add(this.btnShuffle);
            this.Controls.Add(this.btnSort);
            this.Controls.Add(this.cmbAlgorithm);
            this.Controls.Add(this.pictureBox1);
            this.MinimumSize = new System.Drawing.Size(900, 679);
            this.Name = "MainForm";
            this.Text = "Визуализатор алгоритмов сортировки";
            this.Load += new System.EventHandler(this.MainForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numArraySize)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numDelay)).EndInit();
            this.grpStats.ResumeLayout(false);
            this.grpStats.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}