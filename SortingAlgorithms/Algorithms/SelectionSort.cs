using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SortingAlgorithms;
using System.Threading;
using SortingVisualizer.Helpers;

namespace SortingAlgorithms
{
    public class SelectionSort : ISortingAlgorithm
    {
        public string Name => "Сортировка выбором";

        public void Sort(int[] array, Action<int[], int, int> onStep = null,
                        Action<int> onProgress = null, double delayMs = 1.0,
                        CancellationToken cancellationToken = default)
        {
            int n = array.Length;
            int totalComparisons = n * (n - 1) / 2;
            int comparisonsDone = 0;

            for (int i = 0; i < n - 1; i++)
            {
                int minIndex = i;

                // Поиск минимального элемента
                for (int j = i + 1; j < n; j++)
                {
                    if (cancellationToken.IsCancellationRequested)
                        return;

                    comparisonsDone++;

                    onStep?.Invoke(array, minIndex, j);

                    if (array[j] < array[minIndex])
                    {
                        minIndex = j;
                    }

                    // Прогресс
                    if (comparisonsDone % 100 == 0 || n <= 100)
                        onProgress?.Invoke((int)((float)comparisonsDone / totalComparisons * 100));

                    // Задержка
                    if (delayMs > 0)
                    {
                        if (delayMs < 1.0)
                            PrecisionTimer.Delay(delayMs);
                        else
                            Thread.Sleep((int)delayMs);
                    }
                }

                // Обмен
                if (minIndex != i)
                {
                    onStep?.Invoke(array, i, minIndex);

                    int temp = array[i];
                    array[i] = array[minIndex];
                    array[minIndex] = temp;

                    onStep?.Invoke(array, i, minIndex);
                }
            }

            onProgress?.Invoke(100);
        }
    }
}