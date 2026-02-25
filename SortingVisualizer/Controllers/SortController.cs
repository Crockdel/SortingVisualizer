using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using SortingVisualizer.Models;
using SortingAlgorithms;
using System.Diagnostics;

namespace SortingVisualizer.Controllers
{
    public class SortController
    {
        private readonly Statistics _statistics;
        private CancellationTokenSource _cancellationTokenSource;
        private bool _isSorting;
        private Stopwatch _stepTimer;
        private int _stepsSinceLastUpdate;
        private const int MAX_UPDATES_PER_SECOND = 60; // Максимум 60 кадров в секунду

        public event Action<SortArray, int, int> StepVisualized;
        public event Action<int> ProgressUpdated;
        public event Action SortingCompleted;
        public event Action<string> StatusChanged;
        public event Action<bool> SortingStateChanged; // Новое событие для состояния

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
        }

        public async Task StartSorting(SortArray array, ISortingAlgorithm algorithm, int delayMs)
        {
            if (IsSorting) return;

            IsSorting = true;
            _statistics.Reset();
            _cancellationTokenSource = new CancellationTokenSource();
            _stepsSinceLastUpdate = 0;
            _stepTimer.Restart();

            StatusChanged?.Invoke("Сортировка...");

            try
            {
                var arrayToSort = array.Clone();
                var cancellationToken = _cancellationTokenSource.Token;

                // Оптимизация: если задержка 0, используем максимальную скорость
                double effectiveDelay = delayMs; // теперь double

                await Task.Run(() =>
                {
                    algorithm.Sort(arrayToSort.Values,
                        (arr, i1, i2) => OnStep(arr, i1, i2, array, effectiveDelay == 0),
                        (progress) => ProgressUpdated?.Invoke(progress),
                        effectiveDelay, // передаем double
                        cancellationToken);
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
            }
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

            // В быстром режиме обновляем визуализацию не чаще 60 FPS
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