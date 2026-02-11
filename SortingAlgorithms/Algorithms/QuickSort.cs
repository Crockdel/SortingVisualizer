using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace SortingAlgorithms
{
    public class QuickSort : ISortingAlgorithm
    {
        private int totalOperations = 0;
        private int operationsDone = 0;

        public string Name => "Быстрая сортировка";

        public void Sort(int[] array, Action<int[], int, int> onStep = null,
                        Action<int> onProgress = null, int delay = 1,
                        CancellationToken cancellationToken = default)
        {
            totalOperations = EstimateOperations(array.Length);
            operationsDone = 0;

            QuickSortRecursive(array, 0, array.Length - 1, onStep, onProgress, delay, cancellationToken);
            onProgress?.Invoke(100);
        }

        private int EstimateOperations(int n)
        {
            // Примерная оценка операций для быстрой сортировки: O(n log n)
            return (int)(n * Math.Log(n + 1) * 2);
        }

        private void QuickSortRecursive(int[] array, int low, int high,
                                       Action<int[], int, int> onStep,
                                       Action<int> onProgress, int delay,
                                       CancellationToken cancellationToken)
        {
            if (low < high)
            {
                if (cancellationToken.IsCancellationRequested)
                    return;

                int pi = Partition(array, low, high, onStep, onProgress, delay, cancellationToken);

                QuickSortRecursive(array, low, pi - 1, onStep, onProgress, delay, cancellationToken);
                QuickSortRecursive(array, pi + 1, high, onStep, onProgress, delay, cancellationToken);
            }
        }

        private int Partition(int[] array, int low, int high,
                             Action<int[], int, int> onStep,
                             Action<int> onProgress, int delay,
                             CancellationToken cancellationToken)
        {
            int pivot = array[high];
            int i = low - 1;

            for (int j = low; j < high; j++)
            {
                if (cancellationToken.IsCancellationRequested)
                    return i + 1;

                operationsDone++;

                // Обновляем прогресс для больших массивов
                if (array.Length > 1000 && operationsDone % 100 == 0)
                    onProgress?.Invoke((int)((float)operationsDone / totalOperations * 100));

                // Для больших массивов показываем только часть шагов
                if (array.Length <= 1000 || j % 5 == 0)
                {
                    onStep?.Invoke(array, j, high);
                    if (delay > 0)
                        Thread.Sleep(delay);
                }

                if (array[j] < pivot)
                {
                    i++;

                    if (array.Length <= 1000 || j % 5 == 0)
                    {
                        onStep?.Invoke(array, i, j);
                        if (delay > 0)
                            Thread.Sleep(delay);
                    }

                    if (i != j)
                    {
                        Swap(array, i, j);

                        if (array.Length <= 1000 || j % 5 == 0)
                        {
                            onStep?.Invoke(array, i, j);
                            if (delay > 0)
                                Thread.Sleep(delay);
                        }
                    }
                }
            }

            if (array.Length <= 1000)
            {
                onStep?.Invoke(array, i + 1, high);
                if (delay > 0)
                    Thread.Sleep(delay);
            }

            if (i + 1 != high)
            {
                Swap(array, i + 1, high);

                if (array.Length <= 1000)
                {
                    onStep?.Invoke(array, i + 1, high);
                    if (delay > 0)
                        Thread.Sleep(delay);
                }
            }

            return i + 1;
        }

        private void Swap(int[] array, int i, int j)
        {
            int temp = array[i];
            array[i] = array[j];
            array[j] = temp;
        }
    }
}