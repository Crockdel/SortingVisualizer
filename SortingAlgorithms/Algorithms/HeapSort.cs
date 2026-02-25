using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using SortingVisualizer.Helpers;

namespace SortingAlgorithms
{
    public class HeapSort : ISortingAlgorithm
    {
        public string Name => "Пирамидальная сортировка";

        public void Sort(int[] array, Action<int[], int, int> onStep = null,
                        Action<int> onProgress = null, double delayMs = 1.0,
                        CancellationToken cancellationToken = default)
        {
            int n = array.Length;
            int totalOperations = 2 * n * (int)Math.Log(n + 1);
            int operationsDone = 0;

            // Построение кучи (heapify)
            for (int i = n / 2 - 1; i >= 0; i--)
            {
                Heapify(array, n, i, onStep, delayMs, cancellationToken);

                operationsDone += n / 2 - i;
                if (operationsDone % 100 == 0 || n <= 100)
                    onProgress?.Invoke((int)((float)operationsDone / totalOperations * 50));
            }

            // Извлечение элементов из кучи
            for (int i = n - 1; i > 0; i--)
            {
                if (cancellationToken.IsCancellationRequested)
                    return;

                onStep?.Invoke(array, 0, i);

                // Перемещаем текущий корень в конец
                int temp = array[0];
                array[0] = array[i];
                array[i] = temp;

                onStep?.Invoke(array, 0, i);

                // Задержка
                if (delayMs > 0)
                {
                    if (delayMs < 1.0)
                        PrecisionTimer.Delay(delayMs);
                    else
                        Thread.Sleep((int)delayMs);
                }

                // Вызываем heapify на уменьшенной куче
                Heapify(array, i, 0, onStep, delayMs, cancellationToken);

                operationsDone += n - i;
                if (operationsDone % 100 == 0 || n <= 100)
                    onProgress?.Invoke(50 + (int)((float)operationsDone / totalOperations * 50));
            }
        }

        private void Heapify(int[] array, int n, int i,
                            Action<int[], int, int> onStep, double delayMs,
                            CancellationToken cancellationToken)
        {
            int largest = i;
            int left = 2 * i + 1;
            int right = 2 * i + 2;

            if (left < n)
            {
                if (cancellationToken.IsCancellationRequested)
                    return;

                onStep?.Invoke(array, largest, left);

                if (array[left] > array[largest])
                    largest = left;
            }

            if (right < n)
            {
                if (cancellationToken.IsCancellationRequested)
                    return;

                onStep?.Invoke(array, largest, right);

                if (array[right] > array[largest])
                    largest = right;
            }

            if (largest != i)
            {
                onStep?.Invoke(array, i, largest);

                int temp = array[i];
                array[i] = array[largest];
                array[largest] = temp;

                onStep?.Invoke(array, i, largest);

                // Задержка
                if (delayMs > 0)
                {
                    if (delayMs < 1.0)
                        PrecisionTimer.Delay(delayMs);
                    else
                        Thread.Sleep((int)delayMs);
                }

                Heapify(array, n, largest, onStep, delayMs, cancellationToken);
            }
        }
    }
}