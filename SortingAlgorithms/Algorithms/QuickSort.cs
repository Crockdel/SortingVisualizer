using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using SortingVisualizer.Helpers;

namespace SortingAlgorithms
{
    public class QuickSort : ISortingAlgorithm
    {
        public string Name => "Быстрая сортировка";
        private int _totalOperations;
        private int _operationsDone;

        public void Sort(int[] array, Action<int[], int, int> onStep = null,
                        Action<int> onProgress = null, double delayMs = 1.0,
                        CancellationToken cancellationToken = default)
        {
            _totalOperations = EstimateOperations(array.Length);
            _operationsDone = 0;

            // Используем итеративную версию для избежания переполнения стека
            IterativeQuickSort(array, 0, array.Length - 1, onStep, onProgress, delayMs, cancellationToken);

            onProgress?.Invoke(100);
        }

        private void IterativeQuickSort(int[] array, int low, int high,
                                       Action<int[], int, int> onStep,
                                       Action<int> onProgress, double delayMs,
                                       CancellationToken cancellationToken)
        {
            // Стек для хранения границ
            Stack<Tuple<int, int>> stack = new Stack<Tuple<int, int>>();
            stack.Push(new Tuple<int, int>(low, high));

            while (stack.Count > 0)
            {
                if (cancellationToken.IsCancellationRequested)
                    return;

                var bounds = stack.Pop();
                int l = bounds.Item1;
                int h = bounds.Item2;

                if (l < h)
                {
                    int pi = Partition(array, l, h, onStep, delayMs, cancellationToken);

                    // Сначала кладем меньшую часть для оптимизации стека
                    if (pi - l < h - pi)
                    {
                        stack.Push(new Tuple<int, int>(pi + 1, h));
                        stack.Push(new Tuple<int, int>(l, pi - 1));
                    }
                    else
                    {
                        stack.Push(new Tuple<int, int>(l, pi - 1));
                        stack.Push(new Tuple<int, int>(pi + 1, h));
                    }

                    // Прогресс
                    _operationsDone += (h - l);
                    if (_operationsDone % 100 == 0 || array.Length <= 100)
                        onProgress?.Invoke((int)((float)_operationsDone / _totalOperations * 100));
                }
            }
        }

        private int Partition(int[] array, int low, int high,
                             Action<int[], int, int> onStep, double delayMs,
                             CancellationToken cancellationToken)
        {
            // Оптимизация: выбираем медиану из трех
            int mid = low + (high - low) / 2;
            int pivotIndex = MedianOfThree(array, low, mid, high);

            // Меняем опорный элемент с последним
            if (pivotIndex != high)
            {
                Swap(array, pivotIndex, high);
                onStep?.Invoke(array, pivotIndex, high);
                if (delayMs > 0) PrecisionTimer.Delay(delayMs);
            }

            int pivot = array[high];
            int i = low - 1;

            for (int j = low; j < high; j++)
            {
                if (cancellationToken.IsCancellationRequested)
                    return i + 1;

                onStep?.Invoke(array, j, high);

                if (array[j] < pivot)
                {
                    i++;
                    Swap(array, i, j);
                    onStep?.Invoke(array, i, j);
                }

                if (delayMs > 0 && delayMs < 1.0)
                    PrecisionTimer.Delay(delayMs);
                else if (delayMs >= 1.0)
                    Thread.Sleep((int)delayMs);
            }

            Swap(array, i + 1, high);
            onStep?.Invoke(array, i + 1, high);

            return i + 1;
        }

        private int MedianOfThree(int[] array, int a, int b, int c)
        {
            if (array[a] > array[b])
                SwapRef(ref a, ref b);
            if (array[b] > array[c])
                SwapRef(ref b, ref c);
            if (array[a] > array[b])
                SwapRef(ref a, ref b);
            return b;
        }

        private void Swap(int[] array, int i, int j)
        {
            int temp = array[i];
            array[i] = array[j];
            array[j] = temp;
        }

        private void SwapRef(ref int a, ref int b)
        {
            int temp = a;
            a = b;
            b = temp;
        }

        private int EstimateOperations(int n)
        {
            return (int)(n * Math.Log(n + 1) * 2);
        }
    }
}