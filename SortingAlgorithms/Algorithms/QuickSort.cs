using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace SortingAlgorithms
{
    /// <summary>
    /// Реализация алгоритма быстрой сортировки с визуализацией
    /// </summary>
    public class QuickSort : ISortingAlgorithm
    {
        public string Name => "Quick Sort";

        public void Sort(int[] array, Action<int[], int, int> onStep = null, int delay = 50, CancellationToken cancellationToken = default)
        {
            QuickSortRecursive(array, 0, array.Length - 1, onStep, delay, cancellationToken);
        }

        private void QuickSortRecursive(int[] array, int low, int high, Action<int[], int, int> onStep, int delay, CancellationToken cancellationToken)
        {
            if (low < high)
            {
                // Проверяем отмену
                if (cancellationToken.IsCancellationRequested)
                    return;

                int pi = Partition(array, low, high, onStep, delay, cancellationToken);

                QuickSortRecursive(array, low, pi - 1, onStep, delay, cancellationToken);
                QuickSortRecursive(array, pi + 1, high, onStep, delay, cancellationToken);
            }
        }

        private int Partition(int[] array, int low, int high, Action<int[], int, int> onStep, int delay, CancellationToken cancellationToken)
        {
            int pivot = array[high];
            int i = low - 1;

            for (int j = low; j < high; j++)
            {
                // Проверяем отмену
                if (cancellationToken.IsCancellationRequested)
                    return i + 1;

                // Визуализируем сравнение с опорным элементом
                onStep?.Invoke(array, j, high);

                if (delay > 0)
                    Thread.Sleep(delay);

                if (array[j] < pivot)
                {
                    i++;

                    // Визуализируем перед обменом
                    onStep?.Invoke(array, i, j);

                    if (delay > 0)
                        Thread.Sleep(delay);

                    if (i != j)
                    {
                        Swap(array, i, j);

                        // Визуализируем после обмена
                        onStep?.Invoke(array, i, j);

                        if (delay > 0)
                            Thread.Sleep(delay);
                    }
                }
            }

            // Визуализируем перед размещением опорного элемента
            onStep?.Invoke(array, i + 1, high);

            if (delay > 0)
                Thread.Sleep(delay);

            if (i + 1 != high)
            {
                Swap(array, i + 1, high);

                // Визуализируем после размещения опорного элемента
                onStep?.Invoke(array, i + 1, high);

                if (delay > 0)
                    Thread.Sleep(delay);
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