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
        private int highlightedIndex1 = -1;
        private int highlightedIndex2 = -1;
        private int[] arrayCopy;

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
            if (pictureBox1.Width <= 0 || pictureBox1.Height <= 0)
                return;

            bitmap?.Dispose();
            graphics?.Dispose();

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

            highlightedIndex1 = -1;
            highlightedIndex2 = -1;
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
            trackBarSpeed.Enabled = false;

            // Создаем копию массива для сортировки
            arrayCopy = (int[])numbers.Clone();

            // Получаем текущую задержку из TrackBar
            delay = 100 - trackBarSpeed.Value;

            // Устанавливаем задержку в алгоритме
            if (selectedAlgorithm is BubbleSort bubbleSort)
                bubbleSort.SetDelay(delay);
            else if (selectedAlgorithm is SelectionSort selectionSort)
                selectionSort.SetDelay(delay);
            else if (selectedAlgorithm is QuickSort quickSort)
                quickSort.SetDelay(delay);

            try
            {
                // Запускаем сортировку в отдельном потоке
                await Task.Run(() =>
                {
                    selectedAlgorithm.Sort(
                        arrayCopy,
                        UpdateVisualizationCallback,
                        HighlightElementsCallback
                    );
                });
            }
            finally
            {
                // Обновляем основной массив
                numbers = arrayCopy;

                // Сбрасываем подсветку
                highlightedIndex1 = -1;
                highlightedIndex2 = -1;
                VisualizeArray();

                isSorting = false;
                btnSort.Enabled = true;
                btnShuffle.Enabled = true;
                cmbAlgorithm.Enabled = true;
                numArraySize.Enabled = true;
                trackBarSpeed.Enabled = true;
            }
        }

        private void UpdateVisualizationCallback(int[] array)
        {
            // Этот метод вызывается из другого потока
            if (this.InvokeRequired)
            {
                this.Invoke(new Action<int[]>(UpdateVisualizationCallback), array);
                return;
            }

            numbers = array;
            VisualizeArray();
            Application.DoEvents(); // Обновляем UI
        }

        private void HighlightElementsCallback(int index1, int index2)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action<int, int>(HighlightElementsCallback), index1, index2);
                return;
            }

            highlightedIndex1 = index1;
            highlightedIndex2 = index2;
            VisualizeArray();
            Application.DoEvents(); // Обновляем UI
        }

        private void VisualizeArray()
        {
            if (bitmap == null || graphics == null || numbers == null)
                return;

            graphics.Clear(Color.White);

            if (numbers.Length == 0) return;

            int barWidth = Math.Max(1, pictureBox1.Width / numbers.Length);
            int maxValue = numbers.Length;

            for (int i = 0; i < numbers.Length; i++)
            {
                int barHeight = (int)((numbers[i] / (float)maxValue) * pictureBox1.Height * 0.9);
                int x = i * barWidth;
                int y = pictureBox1.Height - barHeight;

                // Выбираем цвет в зависимости от подсветки
                Color barColor;
                if (i == highlightedIndex1 || i == highlightedIndex2)
                {
                    barColor = Color.Red; // Подсвеченные элементы
                }
                else if (numbers[i] == i + 1)
                {
                    barColor = Color.Green; // Уже на своих местах
                }
                else
                {
                    int colorValue = (int)((numbers[i] / (float)maxValue) * 200);
                    barColor = Color.FromArgb(50, 100, colorValue);
                }

                using (Brush brush = new SolidBrush(barColor))
                {
                    graphics.FillRectangle(brush, x, y, barWidth - 1, barHeight);
                }

                using (Pen pen = new Pen(Color.Black, 1))
                {
                    graphics.DrawRectangle(pen, x, y, barWidth - 1, barHeight);
                }

                // Отображаем значения для маленьких массивов
                if (numbers.Length <= 30)
                {
                    using (Font font = new Font("Arial", 8))
                    using (Brush textBrush = new SolidBrush(Color.Black))
                    {
                        string text = numbers[i].ToString();
                        SizeF textSize = graphics.MeasureString(text, font);
                        graphics.DrawString(text, font, textBrush,
                            x + (barWidth - textSize.Width) / 2,
                            y - 15);
                    }
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
            SetupDrawing();
            VisualizeArray();
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            if (WindowState != FormWindowState.Minimized)
            {
                SetupDrawing();
                VisualizeArray();
            }
        }
    }
}