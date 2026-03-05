using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using SortingVisualizer.Models;
using SortingAlgorithms;
using System.Diagnostics;
using SortingVisualizer.Helpers;

namespace SortingVisualizer.Controllers
{
    public class SortController
    {
        private readonly Statistics _statistics;
        private CancellationTokenSource _cancellationTokenSource;
        private bool _isSorting;
        private Stopwatch _stepTimer;
        private int _stepsSinceLastUpdate;
        private const int MAX_UPDATES_PER_SECOND = 60;
        private long _lastDelayTimestamp;

        public event Action<SortArray, int, int> StepVisualized;
        public event Action<int> ProgressUpdated;
        public event Action SortingCompleted;
        public event Action<string> StatusChanged;
        public event Action<bool> SortingStateChanged;

        public bool IsSorting
        {
            get => _isSorting;
            private set
            {
                if (_isSorting != value)
                {
                    _isSorting = value;
                    SortingStateChanged?.Invoke(value);
                }
            }
        }

        public Statistics Statistics => _statistics;

        public SortController()
        {
            _statistics = new Statistics();
            _stepTimer = new Stopwatch();
            _lastDelayTimestamp = Stopwatch.GetTimestamp();
        }

        public async Task StartSorting(SortArray array, ISortingAlgorithm algorithm, double delayMs)
        {
            if (IsSorting) return;

            IsSorting = true;
            _statistics.Reset();
            _cancellationTokenSource = new CancellationTokenSource();
            _stepsSinceLastUpdate = 0;
            _stepTimer.Restart();
            _lastDelayTimestamp = Stopwatch.GetTimestamp();

            StatusChanged?.Invoke("Сортировка...");

            try
            {
                var arrayToSort = array.Clone();
                var cancellationToken = _cancellationTokenSource.Token;

                // Определяем режим визуализации
                bool fastMode = delayMs < 0.1; // Режим максимальной скорости
                bool microMode = delayMs > 0 && delayMs < 1000.0; // Режим микро-задержек

                await Task.Run(() =>
                {
                    if (microMode)
                    {
                        // Для микро-задержек используем специальную версию
                        RunSortWithMicroDelay(arrayToSort, algorithm, delayMs, cancellationToken);
                    }
                    else
                    {
                        // Обычный режим
                        algorithm.Sort(arrayToSort.Values,
                            (arr, i1, i2) => OnStep(arr, i1, i2, array, fastMode),
                            (progress) => ProgressUpdated?.Invoke(progress),
                            delayMs,
                            cancellationToken);
                    }
                }, cancellationToken);

                if (!cancellationToken.IsCancellationRequested)
                {
                    array.UpdateFrom(arrayToSort.Values);
                    SortingCompleted?.Invoke();
                    StatusChanged?.Invoke("Готово");
                }
            }
            catch (OperationCanceledException)
            {
                StatusChanged?.Invoke("Отменено");
            }
            finally
            {
                IsSorting = false;
                _cancellationTokenSource?.Dispose();
                _cancellationTokenSource = null;
                _stepTimer.Stop();
                PrecisionTimer.Cleanup(); // Важно! Сбрасываем таймер
            }
        }

        /// <summary>
        /// Специальная версия для микро-задержек (0.01 - 0.99 мс)
        /// </summary>
        private void RunSortWithMicroDelay(SortArray array, ISortingAlgorithm algorithm,
                                          double delayMs, CancellationToken cancellationToken)
        {
            var tempArray = (int[])array.Values.Clone();
            long lastTimestamp = Stopwatch.GetTimestamp();

            // Подписываемся на шаги алгоритма
            algorithm.Sort(tempArray,
                (arr, i1, i2) =>
                {
                    if (cancellationToken.IsCancellationRequested) return;

                    // Обновляем массив
                    array.UpdateFrom(arr);

                    // Ограничиваем частоту обновления
                    _stepsSinceLastUpdate++;
                    if (_stepTimer.ElapsedMilliseconds >= 1000 / MAX_UPDATES_PER_SECOND)
                    {
                        StepVisualized?.Invoke(array, i1, i2);
                        _stepTimer.Restart();
                        _stepsSinceLastUpdate = 0;
                    }

                    // Микро-задержка
                    if (delayMs > 0)
                    {
                        PrecisionTimer.FastDelay(delayMs, ref lastTimestamp);
                    }

                    // Обновляем статистику
                    _statistics.IncrementSteps();
                    if (i1 >= 0 && i2 >= 0)
                    {
                        _statistics.IncrementComparisons();
                    }
                },
                null,
                0, // Передаем 0, чтобы алгоритм не использовал свои задержки
                cancellationToken);
        }

        private void OnStep(int[] arr, int index1, int index2, SortArray originalArray, bool fastMode)
        {
            _statistics.IncrementSteps();
            _stepsSinceLastUpdate++;

            if (index1 >= 0 && index2 >= 0)
            {
                _statistics.IncrementComparisons();

                if (originalArray.Values[index1] != arr[index1] ||
                    originalArray.Values[index2] != arr[index2])
                {
                    _statistics.IncrementSwaps();
                }
            }

            originalArray.UpdateFrom(arr);

            // В быстром режиме обновляем визуализацию реже
            if (fastMode)
            {
                if (_stepTimer.ElapsedMilliseconds >= 1000 / MAX_UPDATES_PER_SECOND)
                {
                    StepVisualized?.Invoke(originalArray, index1, index2);
                    _stepTimer.Restart();
                    _stepsSinceLastUpdate = 0;
                }
            }
            else
            {
                StepVisualized?.Invoke(originalArray, index1, index2);
            }
        }

        public void StopSorting()
        {
            _cancellationTokenSource?.Cancel();
            PrecisionTimer.Cleanup();
        }
    }
}