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
            int totalOperations = 2 * n * (int)Math.Max(1, Math.Log(n + 1));
            int operationsDone = 0;

            // Построение кучи (heapify)
            for (int i = n / 2 - 1; i >= 0; i--)
            {
                if (cancellationToken.IsCancellationRequested)
                    return;

                Heapify(array, n, i, onStep, delayMs, cancellationToken);

                operationsDone++;

                // Безопасное обновление прогресса (первая половина - 50%)
                if (onProgress != null && (operationsDone % 10 == 0 || n <= 50))
                {
                    int progress = (int)((float)operationsDone / Math.Max(1, totalOperations) * 50);
                    progress = Math.Min(50, Math.Max(0, progress)); // Ограничиваем 0-50
                    onProgress(progress);
                }
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

                operationsDone++;

                // Безопасное обновление прогресса (вторая половина - от 50% до 100%)
                if (onProgress != null && (operationsDone % 10 == 0 || n <= 50))
                {
                    int progress = 50 + (int)((float)(operationsDone - n / 2) / Math.Max(1, totalOperations - n / 2) * 50);
                    progress = Math.Min(100, Math.Max(50, progress)); // Ограничиваем 50-100
                    onProgress(progress);
                }
            }

            // Финальный прогресс 100%
            onProgress?.Invoke(100);
        }

        private void Heapify(int[] array, int n, int i,
                            Action<int[], int, int> onStep, double delayMs,
                            CancellationToken cancellationToken)
        {
            int largest = i;
            int left = 2 * i + 1;
            int right = 2 * i + 2;

            // Проверяем левого потомка
            if (left < n)
            {
                if (cancellationToken.IsCancellationRequested)
                    return;

                onStep?.Invoke(array, largest, left);

                if (array[left] > array[largest])
                    largest = left;
            }

            // Проверяем правого потомка
            if (right < n)
            {
                if (cancellationToken.IsCancellationRequested)
                    return;

                onStep?.Invoke(array, largest, right);

                if (array[right] > array[largest])
                    largest = right;
            }

            // Если нужно обменять
            if (largest != i)
            {
                onStep?.Invoke(array, i, largest);

                // Обмен
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

                // Рекурсивно heapify для затронутого поддерева
                Heapify(array, n, largest, onStep, delayMs, cancellationToken);
            }
        }
    }
}