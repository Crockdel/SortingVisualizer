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
                { "Пузырьковая сортировка", new BubbleSort() },
                { "Сортировка выбором", new SelectionSort() },
                { "Сортировка вставками", new InsertionSort() },
                { "Быстрая сортировка", new QuickSort() },
                {"Пирамидальная сортировка", new HeapSort() },
                {"Сортировка слиянием", new MergeSort() },
            };
        }
    }
}