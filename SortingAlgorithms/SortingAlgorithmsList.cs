using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SortingAlgorithms
{

    /// Фабрика для получения списка алгоритмов сортировки

    public static class SortingAlgorithmsList
    {
    
        /// Возвращает словарь доступных алгоритмов сортировки
    
        public static Dictionary<string, ISortingAlgorithm> GetAlgorithms()
        {
            return new Dictionary<string, ISortingAlgorithm>
            {
                { "Bubble Sort", new BubbleSort() },
                { "Selection Sort", new SelectionSort() },
                { "Insertion Sort", new InsertionSort() },
                { "Quick Sort", new QuickSort() },
                {"Heap Sort", new HeapSort() },
                {"Merge Sort", new MergeSort() },
                { "Bogo Sort", new BogoSort() }
            };
        }
    }
}