using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SortingAlgorithms
{
    /// <summary>
    /// Фабрика для получения списка алгоритмов сортировки
    /// </summary>
    public static class SortingAlgorithmsList
    {
        /// <summary>
        /// Возвращает словарь доступных алгоритмов сортировки
        /// </summary>
        public static Dictionary<string, ISortingAlgorithm> GetAlgorithms()
        {
            return new Dictionary<string, ISortingAlgorithm>
            {
                { "Пузырьковая сортировка", new BubbleSort() },
                { "Сортировка выбором", new SelectionSort() },
                { "Сортировка вставками", new InsertionSort() },
                { "Быстрая сортировка", new QuickSort() }
            };
        }
    }
}