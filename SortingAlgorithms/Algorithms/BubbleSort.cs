using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SortingAlgorithms;
using SortingVisualizer.Helpers;

public class BubbleSort : ISortingAlgorithm
{
    public string Name => "Пузырьковая сортировка";

    public void Sort(int[] array, Action<int[], int, int> onStep = null,
                    Action<int> onProgress = null, double delayMs = 1.0,
                    CancellationToken cancellationToken = default)
    {
        int n = array.Length;
        bool swapped;
        int totalComparisons = n * (n - 1) / 2;
        int comparisonsDone = 0;

        for (int i = 0; i < n - 1; i++)
        {
            swapped = false;

            for (int j = 0; j < n - i - 1; j++)
            {
                if (cancellationToken.IsCancellationRequested)
                    return;

                comparisonsDone++;

                if (comparisonsDone % 100 == 0)
                    onProgress?.Invoke((int)((float)comparisonsDone / totalComparisons * 100));

                onStep?.Invoke(array, j, j + 1);

                if (array[j] > array[j + 1])
                {
                    int temp = array[j];
                    array[j] = array[j + 1];
                    array[j + 1] = temp;
                    swapped = true;

                    onStep?.Invoke(array, j, j + 1);
                }

                // Используем точную задержку
                if (delayMs > 0)
                {
                    if (delayMs < 1.0)
                        PrecisionTimer.Delay(delayMs);
                    else
                        Thread.Sleep((int)delayMs);
                }
            }

            if (!swapped)
                break;
        }

        onProgress?.Invoke(100);
    }
}