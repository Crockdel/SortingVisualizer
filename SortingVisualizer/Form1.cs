using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SortingAlgorithms;

namespace SortingVisualizer
{
    public partial class MainForm : Form
    {
        private int[] numbers;
        private Random random = new Random();
        private Dictionary<string, ISortingAlgorithm> sortingAlgorithms;
        private ISortingAlgorithm selectedAlgorithm;
        private int delay = 50;
        private Graphics graphics;
        private Bitmap bitmap;
        private bool isSorting = false;

        public MainForm()
        {
            InitializeComponent();
            InitializeAlgorithms();
            InitializeArray();
            SetupDrawing();
        }

        private void InitializeAlgorithms()
        {
            sortingAlgorithms = SortingAlgorithmsList.GetAlgorithms();

            cmbAlgorithm.Items.Clear();
            foreach (var algorithm in sortingAlgorithms.Keys)
            {
                cmbAlgorithm.Items.Add(algorithm);
            }

            if (cmbAlgorithm.Items.Count > 0)
            {
                cmbAlgorithm.SelectedIndex = 0;
            }
        }

        private void InitializeArray()
        {
            numbers = new int[(int)numArraySize.Value];
            for (int i = 0; i < numbers.Length; i++)
            {
                numbers[i] = i + 1;
            }
            ShuffleArray();
        }

        private void SetupDrawing()
        {
            bitmap = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            graphics = Graphics.FromImage(bitmap);
            pictureBox1.Image = bitmap;
        }

        private void ShuffleArray()
        {
            if (isSorting) return;

            for (int i = numbers.Length - 1; i > 0; i--)
            {
                int j = random.Next(i + 1);
                int temp = numbers[i];
                numbers[i] = numbers[j];
                numbers[j] = temp;
            }
            VisualizeArray();
        }

        private async void SortArray()
        {
            if (isSorting || selectedAlgorithm == null) return;

            isSorting = true;
            btnSort.Enabled = false;
            btnShuffle.Enabled = false;
            cmbAlgorithm.Enabled = false;
            numArraySize.Enabled = false;

            // Создаем копию массива для сортировки
            int[] arrayToSort = (int[])numbers.Clone();

            // Запускаем сортировку в отдельном потоке
            await Task.Run(() =>
            {
                selectedAlgorithm.Sort(arrayToSort);
            });

            // Обновляем основной массив
            numbers = arrayToSort;
            VisualizeArray();

            isSorting = false;
            btnSort.Enabled = true;
            btnShuffle.Enabled = true;
            cmbAlgorithm.Enabled = true;
            numArraySize.Enabled = true;
        }

        private void VisualizeArray()
        {
            if (bitmap == null || graphics == null) return;

            graphics.Clear(Color.White);

            int barWidth = pictureBox1.Width / numbers.Length;
            int maxValue = numbers.Length;

            for (int i = 0; i < numbers.Length; i++)
            {
                int barHeight = (int)((numbers[i] / (float)maxValue) * pictureBox1.Height * 0.9);
                int x = i * barWidth;
                int y = pictureBox1.Height - barHeight;

                // Используем градиентный цвет
                int colorValue = (int)((numbers[i] / (float)maxValue) * 255);
                Color barColor = Color.FromArgb(50, 100, colorValue);

                using (Brush brush = new SolidBrush(barColor))
                {
                    graphics.FillRectangle(brush, x, y, barWidth - 1, barHeight);
                }

                using (Pen pen = new Pen(Color.Black, 1))
                {
                    graphics.DrawRectangle(pen, x, y, barWidth - 1, barHeight);
                }
            }

            pictureBox1.Invalidate();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            VisualizeArray();
        }

        private void btnShuffle_Click(object sender, EventArgs e)
        {
            ShuffleArray();
        }

        private void btnSort_Click(object sender, EventArgs e)
        {
            SortArray();
        }

        private void cmbAlgorithm_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbAlgorithm.SelectedItem != null)
            {
                string selected = cmbAlgorithm.SelectedItem.ToString();
                selectedAlgorithm = sortingAlgorithms[selected];
            }
        }

        private void numArraySize_ValueChanged(object sender, EventArgs e)
        {
            if (!isSorting)
            {
                InitializeArray();
            }
        }

        private void trackBarSpeed_Scroll(object sender, EventArgs e)
        {
            delay = 100 - trackBarSpeed.Value;
            lblSpeed.Text = $"Скорость: {trackBarSpeed.Value}%";
        }

        private void pictureBox1_SizeChanged(object sender, EventArgs e)
        {
            if (bitmap != null)
            {
                bitmap.Dispose();
            }
            if (graphics != null)
            {
                graphics.Dispose();
            }
            SetupDrawing();
            VisualizeArray();
        }
    }
}