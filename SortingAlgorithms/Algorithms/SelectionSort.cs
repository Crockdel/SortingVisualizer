using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SortingAlgorithms;
using System.Threading;

namespace SortingAlgorithms
{

    /// Реализация алгоритма сортировки выбором с визуализацией

    public class SelectionSort : ISortingAlgorithm
    {
        public string Name => "Selection Sort";

        public void Sort(int[] array, Action<int[], int, int> onStep = null,Action<int> onProgress = null, int delay = 1, CancellationToken cancellationToken = default)
        {
            int n = array.Length;

            for (int i = 0; i < n - 1; i++)
            {
                int minIndex = i;

                // Поиск минимального элемента
                for (int j = i + 1; j < n; j++)
                {
                    // Проверяем отмену
                    if (cancellationToken.IsCancellationRequested)
                        return;

                    // Визуализируем сравнение
                    onStep?.Invoke(array, minIndex, j);

                    if (delay > 0)
                        Thread.Sleep(delay);

                    if (array[j] < array[minIndex])
                    {
                        minIndex = j;
                        // Визуализируем новый минимальный элемент
                        onStep?.Invoke(array, i, minIndex);
                    }
                }

                // Обмен минимального элемента с текущим
                if (minIndex != i)
                {
                    // Визуализируем перед обменом
                    onStep?.Invoke(array, i, minIndex);

                    if (delay > 0)
                        Thread.Sleep(delay);

                    Swap(array, i, minIndex);

                    // Визуализируем после обмена
                    onStep?.Invoke(array, i, minIndex);

                    if (delay > 0)
                        Thread.Sleep(delay);
                }
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