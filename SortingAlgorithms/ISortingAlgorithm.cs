using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SortingAlgorithms
{
    /// Интерфейс для алгоритмов сортировки с поддержкой визуализации
    public interface ISortingAlgorithm
    {
        string Name { get; }

        /// Сортировка с возможностью визуализации каждого шага
        void Sort(int[] array,
                 Action<int[], int, int> onStep = null,
                 Action<int> onProgress = null,
                 int delay = 1,
                 System.Threading.CancellationToken cancellationToken = default);
    }
}