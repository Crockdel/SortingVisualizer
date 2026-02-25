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
        private Button btnStop;
        private Button btnResetStats;
        private NumericUpDown numArraySize;
        private TrackBar trackSpeed;
        private ProgressBar progressBar;
        private Label lblAlgorithm;
        private Label lblSpeed;
        private Label lblStatus;
        private Label lblStats;
        private CheckBox chkSound;
        private NumericUpDown numExactDelay;  // Добавляем NumericUpDown для точной задержки

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
            this.components = new System.ComponentModel.Container();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.cmbAlgorithm = new System.Windows.Forms.ComboBox();
            this.btnSort = new System.Windows.Forms.Button();
            this.btnShuffle = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.btnResetStats = new System.Windows.Forms.Button();
            this.numArraySize = new System.Windows.Forms.NumericUpDown();
            this.trackSpeed = new System.Windows.Forms.TrackBar();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.lblAlgorithm = new System.Windows.Forms.Label();
            this.lblSpeed = new System.Windows.Forms.Label();
            this.lblStatus = new System.Windows.Forms.Label();
            this.lblStats = new System.Windows.Forms.Label();
            this.chkSound = new System.Windows.Forms.CheckBox();
            this.numExactDelay = new System.Windows.Forms.NumericUpDown();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);

            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numArraySize)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackSpeed)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numExactDelay)).BeginInit();
            this.SuspendLayout();

            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox1.BackColor = System.Drawing.Color.White;
            this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox1.Location = new System.Drawing.Point(12, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(860, 400);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Normal;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.SizeChanged += new System.EventHandler(this.pictureBox1_SizeChanged);

            // 
            // cmbAlgorithm
            // 
            this.cmbAlgorithm.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cmbAlgorithm.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbAlgorithm.FormattingEnabled = true;
            this.cmbAlgorithm.Location = new System.Drawing.Point(12, 420);
            this.cmbAlgorithm.Name = "cmbAlgorithm";
            this.cmbAlgorithm.Size = new System.Drawing.Size(150, 24);
            this.cmbAlgorithm.TabIndex = 1;
            this.toolTip.SetToolTip(this.cmbAlgorithm, "Выберите алгоритм сортировки");
            this.cmbAlgorithm.SelectedIndexChanged += new System.EventHandler(this.cmbAlgorithm_SelectedIndexChanged);

            // 
            // btnSort
            // 
            this.btnSort.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSort.BackColor = System.Drawing.Color.LightGreen;
            this.btnSort.Location = new System.Drawing.Point(170, 420);
            this.btnSort.Name = "btnSort";
            this.btnSort.Size = new System.Drawing.Size(100, 30);
            this.btnSort.TabIndex = 2;
            this.btnSort.Text = "Сортировать";
            this.toolTip.SetToolTip(this.btnSort, "Запустить сортировку");
            this.btnSort.UseVisualStyleBackColor = false;
            this.btnSort.Click += new System.EventHandler(this.btnSort_Click);

            // 
            // btnShuffle
            // 
            this.btnShuffle.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnShuffle.Location = new System.Drawing.Point(280, 420);
            this.btnShuffle.Name = "btnShuffle";
            this.btnShuffle.Size = new System.Drawing.Size(100, 30);
            this.btnShuffle.TabIndex = 3;
            this.btnShuffle.Text = "Перемешать";
            this.toolTip.SetToolTip(this.btnShuffle, "Перемешать массив");
            this.btnShuffle.UseVisualStyleBackColor = true;
            this.btnShuffle.Click += new System.EventHandler(this.btnShuffle_Click);

            // 
            // btnStop
            // 
            this.btnStop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnStop.BackColor = System.Drawing.Color.LightCoral;
            this.btnStop.Enabled = false;
            this.btnStop.Location = new System.Drawing.Point(390, 420);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(80, 30);
            this.btnStop.TabIndex = 4;
            this.btnStop.Text = "Стоп";
            this.toolTip.SetToolTip(this.btnStop, "Остановить сортировку");
            this.btnStop.UseVisualStyleBackColor = false;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);

            // 
            // btnResetStats
            // 
            this.btnResetStats.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnResetStats.Location = new System.Drawing.Point(480, 420);
            this.btnResetStats.Name = "btnResetStats";
            this.btnResetStats.Size = new System.Drawing.Size(100, 30);
            this.btnResetStats.TabIndex = 5;
            this.btnResetStats.Text = "Сброс стат.";
            this.toolTip.SetToolTip(this.btnResetStats, "Сбросить статистику");
            this.btnResetStats.Click += new System.EventHandler(this.btnResetStats_Click);

            // 
            // numArraySize
            // 
            this.numArraySize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.numArraySize.Location = new System.Drawing.Point(12, 460);
            this.numArraySize.Minimum = new decimal(new int[] { 5, 0, 0, 0 });
            this.numArraySize.Maximum = new decimal(new int[] { 500, 0, 0, 0 });
            this.numArraySize.Name = "numArraySize";
            this.numArraySize.Size = new System.Drawing.Size(80, 22);
            this.numArraySize.TabIndex = 6;
            this.numArraySize.Value = new decimal(new int[] { 100, 0, 0, 0 });
            this.toolTip.SetToolTip(this.numArraySize, "Размер массива (10-500)");
            this.numArraySize.ValueChanged += new System.EventHandler(this.numArraySize_ValueChanged);

            // 
            // trackSpeed
            // 
            this.trackSpeed.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.trackSpeed.Location = new System.Drawing.Point(170, 460);
            this.trackSpeed.Minimum = 0;
            this.trackSpeed.Maximum = 200;
            this.trackSpeed.Name = "trackSpeed";
            this.trackSpeed.Size = new System.Drawing.Size(200, 56);
            this.trackSpeed.TabIndex = 7;
            this.trackSpeed.TickFrequency = 10;
            this.trackSpeed.Value = 10;
            this.trackSpeed.Scroll += new System.EventHandler(this.trackSpeed_Scroll);

            // 
            // numExactDelay
            // 
            this.numExactDelay.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.numExactDelay.Location = new System.Drawing.Point(380, 460);
            this.numExactDelay.Name = "numExactDelay";
            this.numExactDelay.Size = new System.Drawing.Size(80, 22);
            this.numExactDelay.TabIndex = 14;
            this.numExactDelay.DecimalPlaces = 0;
            this.numExactDelay.Increment = 1m;
            this.numExactDelay.Minimum = 0;
            this.numExactDelay.Maximum = 1000;
            this.numExactDelay.Value = 10;
            this.toolTip.SetToolTip(this.numExactDelay, "Точная задержка в миллисекундах (0.01 - 1000)");
            this.numExactDelay.ValueChanged += new System.EventHandler(this.numExactDelay_ValueChanged);

            // 
            // progressBar
            // 
            this.progressBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar.Location = new System.Drawing.Point(12, 500);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(860, 20);
            this.progressBar.TabIndex = 8;

            // 
            // lblAlgorithm
            // 
            this.lblAlgorithm.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblAlgorithm.AutoSize = true;
            this.lblAlgorithm.Location = new System.Drawing.Point(600, 425);
            this.lblAlgorithm.Name = "lblAlgorithm";
            this.lblAlgorithm.Size = new System.Drawing.Size(70, 16);
            this.lblAlgorithm.TabIndex = 9;
            this.lblAlgorithm.Text = "Алгоритм:";

            // 
            // lblSpeed
            // 
            this.lblSpeed.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblSpeed.AutoSize = true;
            this.lblSpeed.Location = new System.Drawing.Point(470, 465);
            this.lblSpeed.Name = "lblSpeed";
            this.lblSpeed.Size = new System.Drawing.Size(94, 16);
            this.lblSpeed.TabIndex = 10;
            this.lblSpeed.Text = "Задержка: 10 мс";

            // 
            // lblStatus
            // 
            this.lblStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblStatus.AutoSize = true;
            this.lblStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold);
            this.lblStatus.Location = new System.Drawing.Point(780, 425);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(63, 18);
            this.lblStatus.TabIndex = 11;
            this.lblStatus.Text = "Готово";

            // 
            // lblStats
            // 
            this.lblStats.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblStats.AutoSize = true;
            this.lblStats.Location = new System.Drawing.Point(12, 530);
            this.lblStats.Name = "lblStats";
            this.lblStats.Size = new System.Drawing.Size(56, 16);
            this.lblStats.TabIndex = 12;
            this.lblStats.Text = "Статистика";

            // 
            // chkSound
            // 
            this.chkSound.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.chkSound.AutoSize = true;
            this.chkSound.Checked = true;
            this.chkSound.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkSound.Location = new System.Drawing.Point(800, 530);
            this.chkSound.Name = "chkSound";
            this.chkSound.Size = new System.Drawing.Size(62, 20);
            this.chkSound.TabIndex = 13;
            this.chkSound.Text = "Звук";
            this.toolTip.SetToolTip(this.chkSound, "Звуковое оповещение по окончании");

            // 
            // MainForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(884, 561);
            this.Controls.Add(this.numExactDelay);
            this.Controls.Add(this.chkSound);
            this.Controls.Add(this.lblStats);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.lblSpeed);
            this.Controls.Add(this.lblAlgorithm);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.trackSpeed);
            this.Controls.Add(this.numArraySize);
            this.Controls.Add(this.btnResetStats);
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.btnShuffle);
            this.Controls.Add(this.btnSort);
            this.Controls.Add(this.cmbAlgorithm);
            this.Controls.Add(this.pictureBox1);
            this.MinimumSize = new System.Drawing.Size(900, 600);
            this.MaximumSize = new System.Drawing.Size(1920, 1080);
            this.Name = "MainForm";
            this.Text = "Визуализатор сортировок";
            this.ResizeRedraw = true;

            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numArraySize)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackSpeed)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numExactDelay)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}