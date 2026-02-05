using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace SortingAlgorithms
{
    public class QuickSort : ISortingAlgorithm
    {
        public string Name => "Quick Sort";
        private int delay = 50;
        private Action<int[]> updateVisualization;
        private Action<int, int> highlightElements;

        public void Sort(int[] array, Action<int[]> updateVisualization, Action<int, int> highlightElements)
        {
            this.updateVisualization = updateVisualization;
            this.highlightElements = highlightElements;
            QuickSortRecursive(array, 0, array.Length - 1);
        }

        private void QuickSortRecursive(int[] array, int low, int high)
        {
            if (low < high)
            {
                // Подсвечиваем текущий раздел
                highlightElements?.Invoke(low, high);
                Thread.Sleep(delay);

                int pi = Partition(array, low, high);
                QuickSortRecursive(array, low, pi - 1);
                QuickSortRecursive(array, pi + 1, high);
            }
        }

        private int Partition(int[] array, int low, int high)
        {
            int pivot = array[high];
            int i = low - 1;

            for (int j = low; j < high; j++)
            {
                // Подсвечиваем сравниваемые элементы
                highlightElements?.Invoke(j, high);
                Thread.Sleep(delay / 2);

                if (array[j] < pivot)
                {
                    i++;
                    Swap(array, i, j);

                    // Обновляем визуализацию
                    updateVisualization?.Invoke(array);
                    Thread.Sleep(delay);
                }
            }

            Swap(array, i + 1, high);
            updateVisualization?.Invoke(array);
            Thread.Sleep(delay);

            return i + 1;
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