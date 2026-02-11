using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SortingAlgorithms
{
    /// <summary>
    /// Интерфейс для алгоритмов сортировки с поддержкой визуализации
    /// </summary>
    public interface ISortingAlgorithm
    {
        string Name { get; }

        /// <summary>
        /// Сортировка с возможностью визуализации каждого шага
        /// </summary>
        /// <param name="array">Массив для сортировки</param>
        /// <param name="onStep">Колбэк для визуализации шага</param>
        /// <param name="delay">Задержка между шагами в миллисекундах</param>
        /// <param name="cancellationToken">Токен для отмены сортировки</param>
        void Sort(int[] array, Action<int[], int, int> onStep = null, int delay = 50, System.Threading.CancellationToken cancellationToken = default);
    }
}