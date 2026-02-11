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
using System.Threading;

namespace SortingVisualizer
{
    /// Главная форма приложения для визуализации алгоритмов сортировки
    public partial class MainForm : Form
    {
        private int[] numbers; // Массив чисел для сортировки
        private Random random = new Random(); // Генератор случайных чисел
        private Dictionary<string, ISortingAlgorithm> sortingAlgorithms; // Список алгоритмов
        private ISortingAlgorithm selectedAlgorithm; // Выбранный алгоритм
        private Graphics graphics; // Объект для рисования
        private Bitmap bitmap; // Изображение для визуализации
        private CancellationTokenSource cancellationTokenSource; // Для отмены сортировки
        private int currentIndex1 = -1; // Первый выделяемый элемент
        private int currentIndex2 = -1; // Второй выделяемый элемент
        private bool isSorted = false; // Флаг отсортированности массива

        public MainForm()
        {
            InitializeComponent();
            InitializeAlgorithms();
            InitializeArray();
            SetupDrawing();
            UpdateUIState();
        }

        /// Инициализация списка алгоритмов
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

        /// Инициализация массива чисел
        private void InitializeArray()
        {
            numbers = new int[(int)numArraySize.Value];
            for (int i = 0; i < numbers.Length; i++)
            {
                numbers[i] = i + 1;
            }
            ShuffleArray();
            isSorted = false;
        }

        /// Настройка графики для отрисовки
        private void SetupDrawing()
        {
            if (bitmap != null)
                bitmap.Dispose();
            if (graphics != null)
                graphics.Dispose();

            bitmap = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            graphics = Graphics.FromImage(bitmap);
            pictureBox1.Image = bitmap;
        }

        /// Перемешивание массива
        private void ShuffleArray()
        {
            if (btnStopSort.Enabled) return; // Не перемешиваем во время сортировки

            for (int i = numbers.Length - 1; i > 0; i--)
            {
                int j = random.Next(i + 1);
                int temp = numbers[i];
                numbers[i] = numbers[j];
                numbers[j] = temp;
            }

            currentIndex1 = -1;
            currentIndex2 = -1;
            isSorted = false;
            VisualizeArray();
        }

        /// Запуск сортировки массива
        private async void StartSorting()
        {
            if (selectedAlgorithm == null || btnStopSort.Enabled) return;

            // Создаем токен для отмены
            cancellationTokenSource = new CancellationTokenSource();
            var cancellationToken = cancellationTokenSource.Token;

            // Блокируем UI элементы
            btnSort.Enabled = false;
            btnShuffle.Enabled = false;
            btnStopSort.Enabled = true;
            cmbAlgorithm.Enabled = false;
            numArraySize.Enabled = false;
            numDelay.Enabled = false;

            // Создаем копию массива для сортировки
            int[] arrayToSort = (int[])numbers.Clone();
            int delay = (int)numDelay.Value;

            try
            {
                // Запускаем сортировку в отдельном потоке
                await Task.Run(() =>
                {
                    selectedAlgorithm.Sort(arrayToSort, VisualizeStep, delay, cancellationToken);
                }, cancellationToken);

                // Если сортировка завершилась успешно
                if (!cancellationToken.IsCancellationRequested)
                {
                    numbers = arrayToSort;
                    isSorted = true;
                    currentIndex1 = -1;
                    currentIndex2 = -1;
                    VisualizeArray();

                    // Воспроизводим звук завершения
                    System.Media.SystemSounds.Beep.Play();
                }
            }
            catch (OperationCanceledException)
            {
                // Сортировка была отменена - это нормально
            }
            finally
            {
                // Разблокируем UI элементы
                btnStopSort.Enabled = false;
                btnSort.Enabled = true;
                btnShuffle.Enabled = true;
                cmbAlgorithm.Enabled = true;
                numArraySize.Enabled = true;
                numDelay.Enabled = true;

                cancellationTokenSource.Dispose();
                cancellationTokenSource = null;
            }
        }

        /// Остановка сортировки
        private void StopSorting()
        {
            if (cancellationTokenSource != null && !cancellationTokenSource.IsCancellationRequested)
            {
                cancellationTokenSource.Cancel();
            }
        }

        /// Визуализация одного шага алгоритма
        private void VisualizeStep(int[] array, int index1, int index2)
        {
            if (InvokeRequired)
            {
                // Вызываем в UI потоке
                Invoke(new Action(() => VisualizeStep(array, index1, index2)));
                return;
            }

            // Обновляем текущие индексы для выделения
            currentIndex1 = index1;
            currentIndex2 = index2;

            // Обновляем массив и визуализацию
            numbers = (int[])array.Clone();
            VisualizeArray();

            // Обновляем информацию о текущем шаге
            UpdateStepInfo(index1, index2);

            // Принудительно обновляем отображение
            Application.DoEvents();
        }

        /// Обновление информации о текущем шаге
        private void UpdateStepInfo(int index1, int index2)
        {
            string info = "Текущий шаг: ";

            if (index1 >= 0 && index2 >= 0)
            {
                info += $"сравнение элементов [{index1}]={numbers[index1]} и [{index2}]={numbers[index2]}";
            }
            else if (index1 >= 0)
            {
                info += $"работа с элементом [{index1}]={numbers[index1]}";
            }
            else
            {
                info += "ожидание";
            }

            lblStepInfo.Text = info;
        }

        /// Визуализация всего массива
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

                // Выбираем цвет в зависимости от состояния элемента
                Color barColor;

                if (isSorted)
                {
                    // Для отсортированного массива - зеленый градиент
                    int colorValue = (int)((numbers[i] / (float)maxValue) * 155) + 100;
                    barColor = Color.FromArgb(100, colorValue, 100);
                }
                else if (i == currentIndex1 || i == currentIndex2)
                {
                    // Для активных элементов - красный
                    barColor = Color.Red;
                }
                else
                {
                    // Для обычных элементов - синий градиент
                    int colorValue = (int)((numbers[i] / (float)maxValue) * 155) + 100;
                    barColor = Color.FromArgb(100, 100, colorValue);
                }

                using (Brush brush = new SolidBrush(barColor))
                {
                    graphics.FillRectangle(brush, x, y, barWidth - 1, barHeight);
                }

                using (Pen pen = new Pen(Color.Black, 1))
                {
                    graphics.DrawRectangle(pen, x, y, barWidth - 1, barHeight);
                }

                // Подписываем элементы, если массив небольшой
                if (numbers.Length <= 30)
                {
                    using (Font font = new Font("Arial", 8))
                    using (Brush textBrush = new SolidBrush(Color.Black))
                    {
                        string text = numbers[i].ToString();
                        SizeF textSize = graphics.MeasureString(text, font);
                        graphics.DrawString(text, font, textBrush,
                            x + (barWidth - textSize.Width) / 2,
                            y - textSize.Height);
                    }
                }
            }

            // Подписываем оси
            using (Font axisFont = new Font("Arial", 10))
            using (Brush axisBrush = new SolidBrush(Color.Black))
            {
                graphics.DrawString("Индекс элемента →", axisFont, axisBrush,
                    pictureBox1.Width / 2 - 50, pictureBox1.Height - 20);
                graphics.DrawString("↑ Значение", axisFont, axisBrush, 10, 10);
            }

            pictureBox1.Invalidate();
        }

        /// Обновление состояния UI элементов
        private void UpdateUIState()
        {
            bool isSorting = btnStopSort.Enabled;

            btnSort.Enabled = !isSorting && !isSorted;
            btnShuffle.Enabled = !isSorting;
            cmbAlgorithm.Enabled = !isSorting;
            numArraySize.Enabled = !isSorting;
            numDelay.Enabled = !isSorting;

            lblStatus.Text = isSorting ? "Сортировка..." : (isSorted ? "Отсортировано" : "Готово");
            lblStatus.ForeColor = isSorting ? Color.Blue : (isSorted ? Color.Green : Color.Black);
        }

        // Обработчики событий
        private void MainForm_Load(object sender, EventArgs e)
        {
            VisualizeArray();
            UpdateUIState();
        }

        private void btnShuffle_Click(object sender, EventArgs e)
        {
            ShuffleArray();
            UpdateUIState();
        }

        private void btnSort_Click(object sender, EventArgs e)
        {
            StartSorting();
            UpdateUIState();
        }

        private void btnStopSort_Click(object sender, EventArgs e)
        {
            StopSorting();
            UpdateUIState();
        }

        private void cmbAlgorithm_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbAlgorithm.SelectedItem != null)
            {
                string selected = cmbAlgorithm.SelectedItem.ToString();
                selectedAlgorithm = sortingAlgorithms[selected];
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

        private void pictureBox1_SizeChanged(object sender, EventArgs e)
        {
            if (Width > 0 && Height > 0)
            {
                SetupDrawing();
                VisualizeArray();
            }
        }

        private void numDelay_ValueChanged(object sender, EventArgs e)
        {
            lblDelay.Text = $"Задержка(мс):";
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            StopSorting();
            base.OnFormClosing(e);
        }
    }
}