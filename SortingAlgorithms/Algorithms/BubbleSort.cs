using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SortingAlgorithms
{
    public class BubbleSort : ISortingAlgorithm
    {
        public string Name => "Пузырьковая сортировка";

        public void Sort(int[] array, Action<int[], int, int> onStep = null,
                        Action<int> onProgress = null, int delay = 1,
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

                    // Обновляем прогресс каждые 100 сравнений или для малых массивов
                    if (array.Length > 1000 && comparisonsDone % 100 == 0)
                        onProgress?.Invoke((int)((float)comparisonsDone / totalComparisons * 100));

                    // Для больших массивов показываем только каждое 10-е сравнение
                    if (array.Length <= 1000 || j % 10 == 0)
                    {
                        onStep?.Invoke(array, j, j + 1);
                        if (delay > 0)
                            Thread.Sleep(delay);
                    }

                    if (array[j] > array[j + 1])
                    {
                        Swap(array, j, j + 1);
                        swapped = true;

                        if (array.Length <= 1000 || j % 10 == 0)
                        {
                            onStep?.Invoke(array, j, j + 1);
                            if (delay > 0)
                                Thread.Sleep(delay);
                        }
                    }
                }

                // Обновляем прогресс после каждого прохода
                if (array.Length > 1000)
                    onProgress?.Invoke((int)((float)comparisonsDone / totalComparisons * 100));

                if (!swapped)
                    break;
            }

            onProgress?.Invoke(100);
        }

        private void Swap(int[] array, int i, int j)
        {
            int temp = array[i];
            array[i] = array[j];
            array[j] = temp;
        }
    }
}