using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using SortingVisualizer.Helpers;

namespace SortingAlgorithms
{
    // Bogo Sort (глупая сортировка) - 
    // Перемешивает массив случайным образом, пока не станет отсортированным
    public class BogoSort : ISortingAlgorithm
    {
        private readonly Random _random = new Random();

        public string Name => "Bogo Sort (Глупая сортировка)";

        public void Sort(int[] array, Action<int[], int, int> onStep = null,
                        Action<int> onProgress = null, double delayMs = 1.0,
                        CancellationToken cancellationToken = default)
        {
            int iterations = 0;
            int arrayLength = array.Length;

            onProgress?.Invoke(0);

            // Продолжаем перемешивать, пока массив не отсортируется
            while (!IsSorted(array))
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    onProgress?.Invoke(100);
                    return;
                }

                iterations++;

                // Перемешиваем массив (Fisher-Yates shuffle)
                ShuffleArray(array, onStep, delayMs, cancellationToken);

                // Визуализируем после перемешивания
                onStep?.Invoke(array, -1, -1);

                // Обновляем "прогресс" (чем больше итераций, тем меньше шансов)
                double chanceOfBeingSorted = 1.0 / Factorial(arrayLength);
                double progress = Math.Min(100, iterations * chanceOfBeingSorted * 100);
                onProgress?.Invoke((int)progress);

                // Задержка для визуализации
                if (delayMs > 0)
                {
                    if (delayMs < 1.0)
                        PrecisionTimer.Delay(delayMs);
                    else
                        Thread.Sleep((int)delayMs);
                }
            }

            // Ура! Массив отсортирован
            onStep?.Invoke(array, -1, -1);
            onProgress?.Invoke(100);
        }

        // Проверка, отсортирован ли массив
        private bool IsSorted(int[] array)
        {
            for (int i = 1; i < array.Length; i++)
            {
                if (array[i] < array[i - 1])
                    return false;
            }
            return true;
        }

        // Перемешивание массива (Fisher-Yates)
        private void ShuffleArray(int[] array, Action<int[], int, int> onStep,
                                 double delayMs, CancellationToken cancellationToken)
        {
            int n = array.Length;
            for (int i = n - 1; i > 0; i--)
            {
                if (cancellationToken.IsCancellationRequested)
                    return;

                int j = _random.Next(i + 1);

                // Визуализируем обмен
                onStep?.Invoke(array, i, j);

                // Обмен
                int temp = array[i];
                array[i] = array[j];
                array[j] = temp;

                // Визуализируем результат обмена
                onStep?.Invoke(array, i, j);

                // Микро-задержка для визуализации
                if (delayMs > 0 && delayMs < 0.1)
                    PrecisionTimer.Delay(delayMs);
            }
        }

        // Вычисление факториала (для оценки вероятности)
        private long Factorial(int n)
        {
            if (n <= 1) return 1;

            long result = 1;
            for (int i = 2; i <= n; i++)
            {
                result *= i;
                // Предотвращаем переполнение для больших n
                if (result > long.MaxValue / i)
                    return long.MaxValue;
            }
            return result;
        }
    }
}