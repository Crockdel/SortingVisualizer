using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SortingAlgorithms
{
    public static class SortingAlgorithmsList
    {
        public static Dictionary<string, ISortingAlgorithm> GetAlgorithms()
        {
            return new Dictionary<string, ISortingAlgorithm>
            {
                { "Bubble Sort", new BubbleSort() },
                { "Selection Sort", new SelectionSort() },
                { "Quick Sort", new QuickSort() },
                //{ "Merge Sort", new MergeSort() },
            };
        }
    }
}