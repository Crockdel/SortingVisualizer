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

        void Sort(int[] array,
                 Action<int[], int, int> onStep = null,
                 Action<int> onProgress = null,
                 double delayMs = 1.0,  // Изменили на double
                 System.Threading.CancellationToken cancellationToken = default);
    }
}