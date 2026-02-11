using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace SortingAlgorithms
{
    /// <summary>
    /// Пирамидальная сортировка (Heap Sort) - эффективна для больших массивов
    /// </summary>
    public class HeapSort : ISortingAlgorithm
    {
        public string Name => "Пирамидальная сортировка";

        public void Sort(int[] array, Action<int[], int, int> onStep = null,
                        Action<int> onProgress = null, int delay = 1,
                        CancellationToken cancellationToken = default)
        {
            int n = array.Length;
            int totalOps = 2 * n * (int)Math.Log(n + 1);
            int opsDone = 0;

            // Построение кучи
            for (int i = n / 2 - 1; i >= 0; i--)
            {
                Heapify(array, n, i, onStep, delay, cancellationToken);

                opsDone += n / 2 - i;
                if (n > 1000 && opsDone % 100 == 0)
                    onProgress?.Invoke((int)((float)opsDone / totalOps * 50));
            }

            // Извлечение элементов из кучи
            for (int i = n - 1; i > 0; i--)
            {
                if (cancellationToken.IsCancellationRequested)
                    return;

                // Перемещаем текущий корень в конец
                if (n <= 1000 || i % 10 == 0)
                {
                    onStep?.Invoke(array, 0, i);
                    if (delay > 0) Thread.Sleep(delay);
                }

                Swap(array, 0, i);

                if (n <= 1000 || i % 10 == 0)
                {
                    onStep?.Invoke(array, 0, i);
                    if (delay > 0) Thread.Sleep(delay);
                }

                // Вызываем heapify на уменьшенной куче
                Heapify(array, i, 0, onStep, delay, cancellationToken);

                opsDone += n - i;
                if (n > 1000 && opsDone % 100 == 0)
                    onProgress?.Invoke(50 + (int)((float)opsDone / totalOps * 50));
            }

            onProgress?.Invoke(100);
        }

        private void Heapify(int[] array, int n, int i,
                            Action<int[], int, int> onStep, int delay,
                            CancellationToken cancellationToken)
        {
            int largest = i;
            int left = 2 * i + 1;
            int right = 2 * i + 2;

            if (left < n)
            {
                if (cancellationToken.IsCancellationRequested)
                    return;

                if (array.Length <= 1000)
                {
                    onStep?.Invoke(array, largest, left);
                    if (delay > 0) Thread.Sleep(delay);
                }

                if (array[left] > array[largest])
                    largest = left;
            }

            if (right < n)
            {
                if (cancellationToken.IsCancellationRequested)
                    return;

                if (array.Length <= 1000)
                {
                    onStep?.Invoke(array, largest, right);
                    if (delay > 0) Thread.Sleep(delay);
                }

                if (array[right] > array[largest])
                    largest = right;
            }

            if (largest != i)
            {
                if (array.Length <= 1000)
                {
                    onStep?.Invoke(array, i, largest);
                    if (delay > 0) Thread.Sleep(delay);
                }

                Swap(array, i, largest);

                if (array.Length <= 1000)
                {
                    onStep?.Invoke(array, i, largest);
                    if (delay > 0) Thread.Sleep(delay);
                }

                Heapify(array, n, largest, onStep, delay, cancellationToken);
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