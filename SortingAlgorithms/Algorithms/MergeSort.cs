using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using SortingVisualizer.Helpers;

namespace SortingAlgorithms
{
    public class MergeSort : ISortingAlgorithm
    {
        public string Name => "Сортировка слиянием";
        private int[] _temp;
        private int _totalOperations;
        private int _operationsDone;

        public void Sort(int[] array, Action<int[], int, int> onStep = null,
                        Action<int> onProgress = null, double delayMs = 1.0,
                        CancellationToken cancellationToken = default)
        {
            _temp = new int[array.Length];
            _totalOperations = array.Length * (int)Math.Log(array.Length + 1) * 2;
            _operationsDone = 0;

            MergeSortRecursive(array, 0, array.Length - 1, onStep, onProgress, delayMs, cancellationToken);

            onProgress?.Invoke(100);
        }

        private void MergeSortRecursive(int[] array, int left, int right,
                                       Action<int[], int, int> onStep,
                                       Action<int> onProgress, double delayMs,
                                       CancellationToken cancellationToken)
        {
            if (left < right)
            {
                if (cancellationToken.IsCancellationRequested)
                    return;

                int middle = left + (right - left) / 2;

                // Визуализируем разделение
                onStep?.Invoke(array, middle, -1);

                MergeSortRecursive(array, left, middle, onStep, onProgress, delayMs, cancellationToken);
                MergeSortRecursive(array, middle + 1, right, onStep, onProgress, delayMs, cancellationToken);

                Merge(array, left, middle, right, onStep, delayMs, cancellationToken);

                _operationsDone += (right - left);
                if (_operationsDone % 100 == 0 || array.Length <= 100)
                    onProgress?.Invoke((int)((float)_operationsDone / _totalOperations * 100));
            }
        }

        private void Merge(int[] array, int left, int middle, int right,
                          Action<int[], int, int> onStep, double delayMs,
                          CancellationToken cancellationToken)
        {
            int i = left;
            int j = middle + 1;
            int k = left;

            // Копируем во временный массив
            Array.Copy(array, left, _temp, left, right - left + 1);

            while (i <= middle && j <= right)
            {
                if (cancellationToken.IsCancellationRequested)
                    return;

                onStep?.Invoke(array, i, j);

                if (_temp[i] <= _temp[j])
                {
                    array[k] = _temp[i];
                    i++;
                }
                else
                {
                    array[k] = _temp[j];
                    j++;
                }
                k++;

                onStep?.Invoke(array, k - 1, -1);

                // Задержка
                if (delayMs > 0)
                {
                    if (delayMs < 1.0)
                        PrecisionTimer.Delay(delayMs);
                    else
                        Thread.Sleep((int)delayMs);
                }
            }

            // Копируем остатки
            while (i <= middle)
            {
                array[k] = _temp[i];
                onStep?.Invoke(array, k, -1);
                i++;
                k++;
            }

            while (j <= right)
            {
                array[k] = _temp[j];
                onStep?.Invoke(array, k, -1);
                j++;
                k++;
            }
        }
    }
}