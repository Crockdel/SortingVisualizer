using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using SortingVisualizer.Helpers;

namespace SortingAlgorithms
{
    public class InsertionSort : ISortingAlgorithm
    {
        public string Name => "Сортировка вставками";

        public void Sort(int[] array, Action<int[], int, int> onStep = null,
                        Action<int> onProgress = null, double delayMs = 1.0,
                        CancellationToken cancellationToken = default)
        {
            int n = array.Length;
            int totalComparisons = n * (n - 1) / 2;
            int comparisonsDone = 0;

            for (int i = 1; i < n; i++)
            {
                int key = array[i];
                int j = i - 1;

                onStep?.Invoke(array, i, j);

                if (delayMs > 0)
                {
                    if (delayMs < 1.0)
                        PrecisionTimer.Delay(delayMs);
                    else
                        Thread.Sleep((int)delayMs);
                }

                // Сдвиг элементов
                while (j >= 0 && array[j] > key)
                {
                    if (cancellationToken.IsCancellationRequested)
                        return;

                    comparisonsDone++;

                    onStep?.Invoke(array, j, j + 1);

                    array[j + 1] = array[j];
                    j--;

                    onStep?.Invoke(array, j + 1, j + 2);

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

                array[j + 1] = key;
                onStep?.Invoke(array, j + 1, -1);
            }

            onProgress?.Invoke(100);
        }
    }
}