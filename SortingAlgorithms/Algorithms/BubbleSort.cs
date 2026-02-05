using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SortingAlgorithms
{
    public class BubbleSort : ISortingAlgorithm
    {
        public string Name => "Bubble Sort";
        private int delay = 50;

        public void Sort(int[] array, Action<int[]> updateVisualization, Action<int, int> highlightElements)
        {
            int n = array.Length;
            for (int i = 0; i < n - 1; i++)
            {
                for (int j = 0; j < n - i - 1; j++)
                {
                    // Подсвечиваем сравниваемые элементы
                    highlightElements?.Invoke(j, j + 1);

                    if (array[j] > array[j + 1])
                    {
                        Swap(array, j, j + 1);

                        // Обновляем визуализацию после каждого обмена
                        updateVisualization?.Invoke(array);
                        Thread.Sleep(delay);
                    }
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