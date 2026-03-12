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

        // Для отслеживания обменов
        private int[] _previousArrayState;

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

            // Сохраняем начальное состояние массива для отслеживания обменов
            _previousArrayState = (int[])array.Values.Clone();

            StatusChanged?.Invoke("Сортировка...");

            try
            {
                var arrayToSort = array.Clone();
                var cancellationToken = _cancellationTokenSource.Token;

                bool fastMode = delayMs < 0.1;
                bool microMode = delayMs > 0 && delayMs < 1000.0;

                await Task.Run(() =>
                {
                    if (microMode)
                    {
                        RunSortWithMicroDelay(arrayToSort, algorithm, delayMs, cancellationToken);
                    }
                    else
                    {
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
                _previousArrayState = null;
            }
        }

        private void RunSortWithMicroDelay(SortArray array, ISortingAlgorithm algorithm,
                                          double delayMs, CancellationToken cancellationToken)
        {
            var tempArray = (int[])array.Values.Clone();
            long lastTimestamp = Stopwatch.GetTimestamp();

            algorithm.Sort(tempArray,
                (arr, i1, i2) =>
                {
                    if (cancellationToken.IsCancellationRequested) return;

                    // Обновляем массив
                    array.UpdateFrom(arr);

                    // Определяем, был ли обмен
                    if (i1 >= 0 && i2 >= 0 && i1 < arr.Length && i2 < arr.Length)
                    {
                        _statistics.IncrementComparisons();

                        // Проверяем, отличаются ли значения в текущем массиве от предыдущего состояния
                        if (_previousArrayState != null &&
                            i1 < _previousArrayState.Length &&
                            i2 < _previousArrayState.Length)
                        {
                            // Если значения поменялись местами, значит был обмен
                            if (_previousArrayState[i1] != arr[i1] ||
                                _previousArrayState[i2] != arr[i2])
                            {
                                _statistics.IncrementSwaps();
                            }
                        }

                        // Обновляем предыдущее состояние
                        _previousArrayState = (int[])arr.Clone();
                    }

                    _statistics.IncrementSteps();
                    _stepsSinceLastUpdate++;

                    if (_stepTimer.ElapsedMilliseconds >= 1000 / MAX_UPDATES_PER_SECOND)
                    {
                        StepVisualized?.Invoke(array, i1, i2);
                        _stepTimer.Restart();
                        _stepsSinceLastUpdate = 0;
                    }

                    if (delayMs > 0)
                    {
                        PrecisionTimer.FastDelay(delayMs, ref lastTimestamp);
                    }
                },
                null,
                0,
                cancellationToken);
        }

        private void OnStep(int[] arr, int index1, int index2, SortArray originalArray, bool fastMode)
        {
            _statistics.IncrementSteps();
            _stepsSinceLastUpdate++;

            // Определяем, был ли обмен
            if (index1 >= 0 && index2 >= 0 && index1 < arr.Length && index2 < arr.Length)
            {
                _statistics.IncrementComparisons();

                // Проверяем, отличаются ли значения в текущем массиве от предыдущего состояния
                if (_previousArrayState != null &&
                    index1 < _previousArrayState.Length &&
                    index2 < _previousArrayState.Length)
                {
                    // Если значения поменялись местами, значит был обмен
                    if (_previousArrayState[index1] != arr[index1] ||
                        _previousArrayState[index2] != arr[index2])
                    {
                        _statistics.IncrementSwaps();
                    }
                }

                // Обновляем предыдущее состояние
                _previousArrayState = (int[])arr.Clone();
            }

            originalArray.UpdateFrom(arr);

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
        }
    }
}