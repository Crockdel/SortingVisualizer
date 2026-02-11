using SortingAlgorithms;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using ToolTip = System.Windows.Forms.ToolTip;

namespace SortingVisualizer
{
    public partial class MainForm : Form
    {
        // Основные переменные
        private int[] numbers;
        private Random random = new Random();
        private Dictionary<string, ISortingAlgorithm> sortingAlgorithms;
        private ISortingAlgorithm selectedAlgorithm;
        private Graphics graphics;
        private Bitmap bitmap;
        private CancellationTokenSource cancellationTokenSource;

        // Для визуализации
        private int currentIndex1 = -1;
        private int currentIndex2 = -1;
        private bool isSorted = false;
        private int maxArraySize = 10000;

        // Режимы отображения
        private enum DisplayMode { Default, LargeArray, HugeArray }
        private DisplayMode currentDisplayMode = DisplayMode.Default;

        // Статистика
        private DateTime sortStartTime;
        private int totalSteps = 0;
        private int comparisons = 0;
        private int swaps = 0;

        public MainForm()
        {
            InitializeComponent();
            InitializeAlgorithms();
            InitializeArray();
            SetupDrawing();
            UpdateUIState();
            SetupToolTips();
            VisualizeArray(); // Добавляем начальную визуализацию
        }

        private void SetupToolTips()
        {
            ToolTip toolTip = new ToolTip();
            toolTip.SetToolTip(numArraySize, "Рекомендуется:\n1-1000 для детальной визуализации\n1000-10000 для наблюдения за паттернами");
            toolTip.SetToolTip(numDelay, "Для больших массивов используйте задержку 0-10 мс");
            toolTip.SetToolTip(btnSort, "Запуск сортировки с текущими параметрами");
            toolTip.SetToolTip(btnStopSort, "Остановка текущей сортировки");
            toolTip.SetToolTip(btnShuffle, "Перемешать массив случайным образом");
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
                selectedAlgorithm = sortingAlgorithms[cmbAlgorithm.SelectedItem.ToString()];
            }
        }

        private void InitializeArray()
        {
            int size = (int)numArraySize.Value;
            numbers = new int[size];

            if (size <= 1000)
            {
                for (int i = 0; i < size; i++)
                {
                    numbers[i] = i + 1;
                }
            }
            else
            {
                int maxValue = Math.Min(size, 10000);
                for (int i = 0; i < size; i++)
                {
                    numbers[i] = random.Next(1, maxValue + 1);
                }
            }

            ShuffleArray();
            isSorted = false;
            UpdateDisplayMode();
        }

        private void UpdateDisplayMode()
        {
            int size = numbers.Length;

            if (size <= 1000)
            {
                currentDisplayMode = DisplayMode.Default;
                numDelay.Maximum = 1000;
                numDelay.Value = Math.Min(50, numDelay.Value);
            }
            else if (size <= 5000)
            {
                currentDisplayMode = DisplayMode.LargeArray;
                numDelay.Maximum = 100;
                numDelay.Value = Math.Min(10, numDelay.Value);
            }
            else
            {
                currentDisplayMode = DisplayMode.HugeArray;
                numDelay.Maximum = 50;
                numDelay.Value = Math.Min(1, numDelay.Value);
            }

            UpdateStatus($"Режим: {currentDisplayMode}, Элементов: {size}");
        }

        private void SetupDrawing()
        {
            if (bitmap != null) bitmap.Dispose();
            if (graphics != null) graphics.Dispose();

            bitmap = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            graphics = Graphics.FromImage(bitmap);
            graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighSpeed;
            pictureBox1.Image = bitmap;
        }

        private void ShuffleArray()
        {
            if (btnStopSort.Enabled) return;

            int n = numbers.Length;

            if (n <= 1000)
            {
                for (int i = n - 1; i > 0; i--)
                {
                    int j = random.Next(i + 1);
                    int temp = numbers[i];
                    numbers[i] = numbers[j];
                    numbers[j] = temp;
                }
            }
            else
            {
                for (int i = 0; i < n; i += 10)
                {
                    int j = random.Next(n);
                    int k = random.Next(n);
                    int temp = numbers[j];
                    numbers[j] = numbers[k];
                    numbers[k] = temp;
                }
            }

            ResetVisualization();
            isSorted = false;
            VisualizeArray();
        }

        private void ResetVisualization()
        {
            currentIndex1 = -1;
            currentIndex2 = -1;
            totalSteps = 0;
            comparisons = 0;
            swaps = 0;
            UpdateStats();
        }

        private async void StartSorting()
        {
            if (selectedAlgorithm == null || btnStopSort.Enabled) return;

            ResetVisualization();
            sortStartTime = DateTime.Now;

            cancellationTokenSource = new CancellationTokenSource();
            var cancellationToken = cancellationTokenSource.Token;

            btnSort.Enabled = false;
            btnShuffle.Enabled = false;
            btnStopSort.Enabled = true;
            cmbAlgorithm.Enabled = false;
            numArraySize.Enabled = false;
            numDelay.Enabled = false;

            progressBar.Visible = true;
            progressBar.Value = 0;

            try
            {
                int[] arrayToSort = (int[])numbers.Clone();
                int delay = (int)numDelay.Value;

                await Task.Run(() =>
                {
                    selectedAlgorithm.Sort(arrayToSort,
                        VisualizeStep,
                        UpdateProgress,
                        delay,
                        cancellationToken);
                }, cancellationToken);

                if (!cancellationToken.IsCancellationRequested)
                {
                    numbers = arrayToSort;
                    isSorted = true;
                    currentIndex1 = -1;
                    currentIndex2 = -1;
                    VisualizeArray();

                    TimeSpan elapsed = DateTime.Now - sortStartTime;
                    UpdateStatus($"Сортировка завершена за {elapsed.TotalSeconds:F2} секунд");

                    if (chkSound.Checked)
                        System.Media.SystemSounds.Beep.Play();
                }
            }
            catch (OperationCanceledException)
            {
                UpdateStatus("Сортировка отменена");
            }
            finally
            {
                btnStopSort.Enabled = false;
                btnSort.Enabled = true;
                btnShuffle.Enabled = true;
                cmbAlgorithm.Enabled = true;
                numArraySize.Enabled = true;
                numDelay.Enabled = true;
                progressBar.Visible = false;

                cancellationTokenSource?.Dispose();
                cancellationTokenSource = null;
            }

            UpdateUIState();
        }

        private void VisualizeStep(int[] array, int index1, int index2)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => VisualizeStep(array, index1, index2)));
                return;
            }

            totalSteps++;

            if (index1 >= 0 && index2 >= 0)
            {
                comparisons++;
                if (array[index1] != numbers[index1] || array[index2] != numbers[index2])
                {
                    swaps++;
                }
            }

            if (numbers.Length > 1000)
            {
                if (totalSteps % 100 != 0) return;
            }

            currentIndex1 = index1;
            currentIndex2 = index2;
            numbers = (int[])array.Clone();

            VisualizeArray();
            UpdateStats();
            UpdateStepInfo(index1, index2);

            Application.DoEvents();
        }

        private void UpdateProgress(int progress)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => UpdateProgress(progress)));
                return;
            }

            progressBar.Value = Math.Min(100, Math.Max(0, progress));
        }

        private void VisualizeArray()
        {
            if (bitmap == null || graphics == null) return;

            graphics.Clear(Color.White);

            int width = pictureBox1.Width;
            int height = pictureBox1.Height;
            int count = numbers.Length;

            if (count <= 1000)
            {
                DrawBars(width, height, count);
            }
            else if (count <= 5000)
            {
                DrawLines(width, height, count);
            }
            else
            {
                DrawDensity(width, height, count);
            }

            pictureBox1.Invalidate();
        }

        private void DrawBars(int width, int height, int count)
        {
            float barWidth = (float)width / count;
            int maxValue = GetMaxValue();

            for (int i = 0; i < count; i++)
            {
                float barHeight = (numbers[i] / (float)maxValue) * height * 0.9f;
                float x = i * barWidth;
                float y = height - barHeight;

                Color color = GetElementColor(i, maxValue);

                using (Brush brush = new SolidBrush(color))
                {
                    graphics.FillRectangle(brush, x, y, barWidth - 1, barHeight);
                }

                if (count <= 50 && barWidth > 20)
                {
                    DrawValue(i, x, y, barWidth);
                }
            }
        }

        private void DrawLines(int width, int height, int count)
        {
            float stepX = (float)width / count;
            int maxValue = GetMaxValue();

            using (Pen pen = new Pen(Color.Blue, 1))
            {
                for (int i = 0; i < count - 1; i++)
                {
                    float x1 = i * stepX;
                    float y1 = height - (numbers[i] / (float)maxValue) * height * 0.9f;
                    float x2 = (i + 1) * stepX;
                    float y2 = height - (numbers[i + 1] / (float)maxValue) * height * 0.9f;

                    graphics.DrawLine(pen, x1, y1, x2, y2);
                }
            }

            if (currentIndex1 >= 0)
            {
                DrawHighlightPoint(currentIndex1, width, height, count, maxValue, Color.Red);
            }
            if (currentIndex2 >= 0)
            {
                DrawHighlightPoint(currentIndex2, width, height, count, maxValue, Color.Green);
            }
        }

        private void DrawDensity(int width, int height, int count)
        {
            int[] density = new int[width];
            int maxValue = GetMaxValue();

            for (int i = 0; i < count; i++)
            {
                int x = (int)((float)i / count * width);
                int value = numbers[i];
                density[x] += value;
            }

            int maxDensity = 1;
            foreach (int d in density)
                if (d > maxDensity) maxDensity = d;

            using (Brush brush = new SolidBrush(Color.FromArgb(100, 0, 0, 255)))
            {
                for (int x = 0; x < width; x++)
                {
                    if (density[x] > 0)
                    {
                        float barHeight = (density[x] / (float)maxDensity) * height * 0.9f;
                        graphics.FillRectangle(brush, x, height - barHeight, 1, barHeight);
                    }
                }
            }
        }

        private Color GetElementColor(int index, int maxValue)
        {
            if (isSorted)
            {
                int colorValue = (int)((numbers[index] / (float)maxValue) * 155) + 100;
                return Color.FromArgb(100, colorValue, 100);
            }
            else if (index == currentIndex1)
            {
                return Color.Red;
            }
            else if (index == currentIndex2)
            {
                return Color.Green;
            }
            else
            {
                int colorValue = (int)((numbers[index] / (float)maxValue) * 155) + 100;
                return Color.FromArgb(100, 100, colorValue);
            }
        }

        private void DrawHighlightPoint(int index, int width, int height, int count, int maxValue, Color color)
        {
            float x = (float)index / count * width;
            float y = height - (numbers[index] / (float)maxValue) * height * 0.9f;

            using (Brush brush = new SolidBrush(color))
            {
                graphics.FillEllipse(brush, x - 3, y - 3, 6, 6);
            }
        }

        private void DrawValue(int index, float x, float y, float barWidth)
        {
            using (Font font = new Font("Arial", 8))
            using (Brush brush = new SolidBrush(Color.Black))
            {
                string text = numbers[index].ToString();
                SizeF size = graphics.MeasureString(text, font);
                graphics.DrawString(text, font, brush,
                    x + (barWidth - size.Width) / 2,
                    y - size.Height - 2);
            }
        }

        private int GetMaxValue()
        {
            int max = 1;
            foreach (int num in numbers)
                if (num > max) max = num;
            return max;
        }

        private void UpdateStats()
        {
            if (lblSteps != null)
                lblSteps.Text = $"Шагов: {totalSteps:N0}";
            if (lblComparisons != null)
                lblComparisons.Text = $"Сравнений: {comparisons:N0}";
            if (lblSwaps != null)
                lblSwaps.Text = $"Обменов: {swaps:N0}";

            if (totalSteps > 0 && lblOpsPerSec != null)
            {
                double elapsedSeconds = (DateTime.Now - sortStartTime).TotalSeconds;
                if (elapsedSeconds > 0)
                {
                    lblOpsPerSec.Text = $"Оп/сек: {(int)(totalSteps / elapsedSeconds):N0}";
                }
            }
        }

        private void UpdateStepInfo(int index1, int index2)
        {
            if (lblStepInfo != null)
            {
                if (numbers.Length > 1000)
                {
                    lblStepInfo.Text = $"Обработано: {totalSteps:N0} шагов";
                }
                else if (index1 >= 0 && index2 >= 0)
                {
                    lblStepInfo.Text = $"Сравнение [{index1}]={numbers[index1]} и [{index2}]={numbers[index2]}";
                }
                else if (index1 >= 0)
                {
                    lblStepInfo.Text = $"Элемент [{index1}]={numbers[index1]}";
                }
                else
                {
                    lblStepInfo.Text = "Готово";
                }
            }
        }

        private void UpdateStatus(string message)
        {
            if (lblStatus != null)
                lblStatus.Text = message;
        }

        private void UpdateUIState()
        {
            bool isSorting = btnStopSort.Enabled;

            btnSort.Enabled = !isSorting && !isSorted;
            btnShuffle.Enabled = !isSorting;
            cmbAlgorithm.Enabled = !isSorting;
            numArraySize.Enabled = !isSorting;
            numDelay.Enabled = !isSorting;

            if (lblStatus != null)
            {
                lblStatus.ForeColor = isSorting ? Color.Blue : (isSorted ? Color.Green : Color.Black);
            }
        }

        // Обработчики событий
        private void MainForm_Load(object sender, EventArgs e)
        {
            // Уже инициализировано в конструкторе, но можно добавить дополнительную настройку
            VisualizeArray();
            UpdateUIState();
        }

        private void btnSort_Click(object sender, EventArgs e) => StartSorting();

        private void btnShuffle_Click(object sender, EventArgs e) => ShuffleArray();

        private void btnStopSort_Click(object sender, EventArgs e) => StopSorting();

        private void StopSorting()
        {
            cancellationTokenSource?.Cancel();
        }

        private void cmbAlgorithm_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbAlgorithm.SelectedItem != null)
            {
                string selected = cmbAlgorithm.SelectedItem.ToString();
                selectedAlgorithm = sortingAlgorithms[selected];
                if (lblAlgorithmInfo != null)
                    lblAlgorithmInfo.Text = $"Алгоритм:";
            }
        }

        private void numArraySize_ValueChanged(object sender, EventArgs e)
        {
            if (!btnStopSort.Enabled)
            {
                InitializeArray();
                UpdateUIState();
            }
        }

        private void numDelay_ValueChanged(object sender, EventArgs e)
        {
            if (lblDelay != null)
                lblDelay.Text = $"Задержка: {numDelay.Value} мс";

            if (numbers != null && numbers.Length > 1000 && numDelay.Value > 10)
            {
                UpdateStatus("Для больших массивов рекомендуется задержка 0-10 мс");
            }
        }

        private void pictureBox1_SizeChanged(object sender, EventArgs e)
        {
            if (Width > 0 && Height > 0)
            {
                SetupDrawing();
                VisualizeArray();
            }
        }

        private void btnResetStats_Click(object sender, EventArgs e)
        {
            ResetVisualization();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            StopSorting();
            base.OnFormClosing(e);
        }
    }
}