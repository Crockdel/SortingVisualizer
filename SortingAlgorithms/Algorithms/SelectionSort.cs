using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SortingAlgorithms;
using System.Threading;

namespace SortingAlgorithms
{
    public class SelectionSort : ISortingAlgorithm
    {
        public string Name => "Selection Sort";
        private int delay = 50;

        public void Sort(int[] array, Action<int[]> updateVisualization, Action<int, int> highlightElements)
        {
            int n = array.Length;

            for (int i = 0; i < n - 1; i++)
            {
                int minIndex = i;

                // Подсвечиваем текущий минимальный элемент
                highlightElements?.Invoke(minIndex, -1);

                for (int j = i + 1; j < n; j++)
                {
                    // Подсвечиваем сравниваемые элементы
                    highlightElements?.Invoke(minIndex, j);
                    Thread.Sleep(delay / 2);

                    if (array[j] < array[minIndex])
                    {
                        minIndex = j;
                        highlightElements?.Invoke(minIndex, -1);
                    }
                }

                if (minIndex != i)
                {
                    Swap(array, i, minIndex);
                    updateVisualization?.Invoke(array);
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

        public void SetDelay(int ms)
        {
            delay = ms;
        }
    }
}