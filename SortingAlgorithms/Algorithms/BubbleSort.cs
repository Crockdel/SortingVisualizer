using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SortingAlgorithms
{
    /// <summary>
    /// Реализация алгоритма пузырьковой сортировки с визуализацией
    /// </summary>
    public class BubbleSort : ISortingAlgorithm
    {
        public string Name => "Bubble Sort";

        public void Sort(int[] array, Action<int[], int, int> onStep = null, int delay = 50, CancellationToken cancellationToken = default)
        {
            int n = array.Length;
            bool swapped;

            for (int i = 0; i < n - 1; i++)
            {
                swapped = false;

                for (int j = 0; j < n - i - 1; j++)
                {
                    // Проверяем отмену
                    if (cancellationToken.IsCancellationRequested)
                        return;

                    // Визуализируем текущие сравниваемые элементы
                    onStep?.Invoke(array, j, j + 1);

                    if (delay > 0)
                        Thread.Sleep(delay);

                    if (array[j] > array[j + 1])
                    {
                        // Меняем элементы местами
                        Swap(array, j, j + 1);
                        swapped = true;

                        // Визуализируем после обмена
                        onStep?.Invoke(array, j, j + 1);

                        if (delay > 0)
                            Thread.Sleep(delay);
                    }
                }

                // Если за проход не было обменов, массив отсортирован
                if (!swapped)
                    break;
            }
        }

        private void Swap(int[] array, int i, int j)
        {
            int temp = array[i];
            array[i] = array[j];
            array[j] = temp;
        }
    }
}