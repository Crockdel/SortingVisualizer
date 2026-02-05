using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SortingAlgorithms
{
    public interface ISortingAlgorithm
    {
        string Name { get; }
        void Sort(int[] array, Action<int[]> updateVisualization, Action<int, int> highlightElements);
    }
}