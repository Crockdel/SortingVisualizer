using System;
using System.Collections.Generic;
using System.Windows.Forms;
using SortingAlgorithms;
using SortingVisualizer.Models;
using SortingVisualizer.Visualization;
using SortingVisualizer.Controllers;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using ToolTip = System.Windows.Forms.ToolTip;
using SortingVisualizer.Helpers;
namespace SortingVisualizer
{
    public partial class MainForm : Form
    {
        private SortArray _array;
        private SortController _sortController;
        private ArrayRenderer _renderer;
        private Dictionary<string, ISortingAlgorithm> _algorithms;
        private ISortingAlgorithm _selectedAlgorithm;
        private int _activeIndex1 = -1;
        private int _activeIndex2 = -1;
        private bool _isSorted = false;
        private bool _isSorting = false;

        public MainForm()
        {
            InitializeComponent();
            InitializeAlgorithms();
            InitializeArray();
            InitializeRenderer();
            UpdateUIState();

            // Подписываемся на изменение состояния сортировки
            _sortController.SortingStateChanged += OnSortingStateChanged;

            // Устанавливаем начальное значение метки задержки
            UpdateDelayLabel();
        }

        private void InitializeAlgorithms()
        {
            _algorithms = SortingAlgorithmsList.GetAlgorithms();

            cmbAlgorithm.Items.Clear();
            foreach (var alg in _algorithms.Keys)
            {
                cmbAlgorithm.Items.Add(alg);
            }

            if (cmbAlgorithm.Items.Count > 0)
            {
                cmbAlgorithm.SelectedIndex = 0;
                _selectedAlgorithm = _algorithms[cmbAlgorithm.SelectedItem.ToString()];
                lblAlgorithm.Text = $"Алгоритм: {_selectedAlgorithm.Name}";
            }
        }

        private void InitializeArray()
        {
            _array = new SortArray((int)numArraySize.Value);
            _array.Shuffle();
            _isSorted = false;
        }

        private void InitializeRenderer()
        {
            _renderer = new ArrayRenderer(pictureBox1.Width, pictureBox1.Height);
            pictureBox1.Image = _renderer.Bitmap;

            _sortController = new SortController();
            _sortController.StepVisualized += OnStepVisualized;
            _sortController.ProgressUpdated += OnProgressUpdated;
            _sortController.SortingCompleted += OnSortingCompleted;
            _sortController.StatusChanged += OnStatusChanged;
            _sortController.SortingStateChanged += OnSortingStateChanged;
        }

        private void OnSortingStateChanged(bool sorting)
        {
            // Этот метод вызывается в потоке UI благодаря Invoke в контроллере
            if (InvokeRequired)
            {
                Invoke(new Action(() => OnSortingStateChanged(sorting)));
                return;
            }

            _isSorting = sorting;
            UpdateUIState();
        }

        private void OnStepVisualized(SortArray array, int index1, int index2)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => OnStepVisualized(array, index1, index2)));
                return;
            }

            _activeIndex1 = index1;
            _activeIndex2 = index2;
            _renderer.Render(array, _activeIndex1, _activeIndex2, _isSorted);
            pictureBox1.Invalidate();
            UpdateStats();
        }

        private void OnProgressUpdated(int progress)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => OnProgressUpdated(progress)));
                return;
            }
            progressBar.Value = progress;
        }

        private void OnSortingCompleted()
        {
            if (InvokeRequired)
            {
                Invoke(new Action(OnSortingCompleted));
                return;
            }

            _isSorted = true;
            _activeIndex1 = -1;
            _activeIndex2 = -1;
            _renderer.Render(_array, -1, -1, true);
            pictureBox1.Invalidate();

            if (chkSound.Checked)
                System.Media.SystemSounds.Beep.Play();
        }

        private void OnStatusChanged(string status)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => OnStatusChanged(status)));
                return;
            }
            lblStatus.Text = status;
        }

        private void UpdateStats()
        {
            var stats = _sortController.Statistics;

            // Форматируем числа с разделителями разрядов
            string steps = stats.Steps.ToString("N0");
            string comparisons = stats.Comparisons.ToString("N0");
            string swaps = stats.Swaps.ToString("N0");
            string opsPerSec = stats.OperationsPerSecond.ToString("N0");

            lblStats.Text = $"👣 Шаги: {steps} | " +
                           $"⚖️ Сравнений: {comparisons} | " +
                           $"🔄 Обменов: {swaps} | " +
                           $"⚡ Оп/сек: {opsPerSec}";
        }

        private void UpdateUIState()
        {
            // Обновляем состояние кнопок на основе _isSorting
            btnSort.Enabled = !_isSorting;
            btnShuffle.Enabled = !_isSorting;
            btnStop.Enabled = _isSorting;
            cmbAlgorithm.Enabled = !_isSorting;
            numArraySize.Enabled = !_isSorting;
            trackSpeed.Enabled = !_isSorting;
            numExactDelay.Enabled = !_isSorting;
            btnResetStats.Enabled = !_isSorting;

            // Визуальное выделение активной кнопки
            btnStop.BackColor = _isSorting ? System.Drawing.Color.LightCoral : System.Drawing.SystemColors.Control;
            btnSort.BackColor = _isSorting ? System.Drawing.SystemColors.Control : System.Drawing.Color.LightGreen;
        }

        // Обработчики событий
        private async void btnSort_Click(object sender, EventArgs e)
        {
            if (_selectedAlgorithm == null || _isSorting) return;

            // Получаем точное значение задержки из numExactDelay
            double delayMs = (double)numExactDelay.Value;

            UpdateUIState();
            await _sortController.StartSorting(_array, _selectedAlgorithm, delayMs);
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            if (_isSorting)
            {
                _sortController.StopSorting();
            }
        }

        private void btnShuffle_Click(object sender, EventArgs e)
        {
            if (_isSorting) return;

            _array.Shuffle();
            _isSorted = false;
            _renderer.Render(_array, -1, -1, false);
            pictureBox1.Invalidate();
            _sortController.Statistics.Reset();
            UpdateStats();
        }
        private void btnExplainAlgorithm_Click(object sender, EventArgs e)
        {
            if (_selectedAlgorithm == null)
            {
                MessageBox.Show(
                    "Пожалуйста, выберите алгоритм из списка.",
                    "Алгоритм не выбран",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return;
            }

            string explanation = AlgorithmExplainer.GetAlgorithmExplanation(_selectedAlgorithm);

            // Используем большой MessageBox с возможностью копирования
            ShowScrollableMessageBox(explanation, $"Алгоритм: {_selectedAlgorithm.Name}");
        }

        private void btnExplainVisualization_Click(object sender, EventArgs e)
        {
            string explanation = AlgorithmExplainer.GetVisualizationExplanation();
            ShowScrollableMessageBox(explanation, "Визуализация сортировки - научное объяснение");
        }

        /// <summary>
        /// Показывает MessageBox с прокруткой для длинного текста
        /// </summary>
        private void ShowScrollableMessageBox(string text, string caption)
        {
            // Создаем форму с RichTextBox для длинного текста
            Form messageForm = new Form
            {
                Text = caption,
                Size = new System.Drawing.Size(600, 700),
                StartPosition = FormStartPosition.CenterParent,
                MinimizeBox = false,
                MaximizeBox = false,
                ShowIcon = false,
                ShowInTaskbar = false,
                FormBorderStyle = FormBorderStyle.FixedDialog
            };

            RichTextBox textBox = new RichTextBox
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                BackColor = System.Drawing.Color.White,
                Font = new System.Drawing.Font("Consolas", 10),
                WordWrap = false,
                Text = text
            };

            messageForm.Controls.Add(textBox);
            messageForm.ShowDialog();
        }
        private void btnExplainBigO_Click(object sender, EventArgs e)
        {
            string explanation = AlgorithmExplainer.GetBigOExplanation();
            ShowScrollableMessageBox(explanation, "Нотация Big O - научное объяснение");
        }

        private void cmbAlgorithm_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbAlgorithm.SelectedItem != null && !_isSorting)
            {
                _selectedAlgorithm = _algorithms[cmbAlgorithm.SelectedItem.ToString()];
                lblAlgorithm.Text = $"Алгоритм: {_selectedAlgorithm.Name}";
            }
        }

        private void numArraySize_ValueChanged(object sender, EventArgs e)
        {
            if (!_isSorting)
            {
                InitializeArray();
                _renderer.Render(_array, -1, -1, false);
                pictureBox1.Invalidate();
                _sortController.Statistics.Reset();
                UpdateStats();
            }
        }

        private void trackSpeed_Scroll(object sender, EventArgs e)
        {
            // Синхронизируем точное значение с ползунком
            numExactDelay.Value = trackSpeed.Value;
            UpdateDelayLabel();
        }

        private void numExactDelay_ValueChanged(object sender, EventArgs e)
        {
            trackSpeed.Value = (int)Math.Round(numExactDelay.Value);
            UpdateDelayLabel();
        }

        private void UpdateDelayLabel()
        {
            double delay = (double)numExactDelay.Value;
            if (delay == 0)
                lblSpeed.Text = "Задержка: 0 мс (макс. скорость)";
            else
                lblSpeed.Text = $"Задержка: {delay:F2} мс";
        }

        private void pictureBox1_SizeChanged(object sender, EventArgs e)
        {
            try
            {
                if (_renderer == null || pictureBox1 == null) return;
                if (pictureBox1.Width <= 0 || pictureBox1.Height <= 0) return;

                BeginInvoke(new Action(() =>
                {
                    try
                    {
                        _renderer.Resize(pictureBox1.Width, pictureBox1.Height);

                        if (_array != null)
                        {
                            _renderer.Render(_array, _activeIndex1, _activeIndex2, _isSorted);
                        }

                        pictureBox1.Image = _renderer.Bitmap;
                        pictureBox1.Invalidate();
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"Ошибка при изменении размера: {ex.Message}");
                    }
                }));
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Ошибка в SizeChanged: {ex.Message}");
            }
        }

        private void btnResetStats_Click(object sender, EventArgs e)
        {
            if (!_isSorting)
            {
                _sortController.Statistics.Reset();
                UpdateStats();
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            _sortController?.StopSorting();
            _renderer?.Dispose();
            PrecisionTimer.Cleanup();
            base.OnFormClosing(e);
        }
    }
}