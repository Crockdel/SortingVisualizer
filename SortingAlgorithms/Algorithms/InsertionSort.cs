using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace SortingAlgorithms
{
    /// <summary>
    /// Реализация алгоритма сортировки вставками с визуализацией
    /// </summary>
    public class InsertionSort : ISortingAlgorithm
    {
        public string Name => "Insertion Sort";

        public void Sort(int[] array, Action<int[], int, int> onStep = null, int delay = 50, CancellationToken cancellationToken = default)
        {
            for (int i = 1; i < array.Length; i++)
            {
                int key = array[i];
                int j = i - 1;

                // Визуализируем текущий элемент для вставки
                onStep?.Invoke(array, i, j);

                if (delay > 0)
                    Thread.Sleep(delay);

                // Перемещаем элементы, которые больше key
                while (j >= 0 && array[j] > key)
                {
                    // Проверяем отмену
                    if (cancellationToken.IsCancellationRequested)
                        return;

                    // Визуализируем сдвиг
                    onStep?.Invoke(array, j, j + 1);

                    if (delay > 0)
                        Thread.Sleep(delay);

                    array[j + 1] = array[j];
                    j--;

                    // Визуализируем после сдвига
                    onStep?.Invoke(array, j + 1, j + 2);

                    if (delay > 0)
                        Thread.Sleep(delay);
                }

                array[j + 1] = key;

                // Визуализируем размещение элемента
                onStep?.Invoke(array, j + 1, -1);

                if (delay > 0)
                    Thread.Sleep(delay);
            }
        }
    }
}