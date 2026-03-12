using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SortingVisualizer.Models
{
    // Сбор и хранение статистики сортировки
    public class Statistics
    {
        public int Steps { get; private set; }
        public int Comparisons { get; private set; }
        public int Swaps { get; private set; }
        public DateTime StartTime { get; private set; }
        public TimeSpan ElapsedTime => DateTime.Now - StartTime;
        public int OperationsPerSecond => ElapsedTime.TotalSeconds > 0
            ? (int)(Steps / ElapsedTime.TotalSeconds)
            : 0;

        public Statistics()
        {
            Reset();
        }

        public void Reset()
        {
            Steps = 0;
            Comparisons = 0;
            Swaps = 0;
            StartTime = DateTime.Now;
        }

        public void IncrementSteps() => Steps=Comparisons+Swaps;
        public void IncrementComparisons() => Comparisons++;
        public void IncrementSwaps() => Swaps++;

        public override string ToString()
        {
            return $"Шагов: {Steps:N0} | Сравнений: {Comparisons:N0} | Обменов: {Swaps:N0} | Оп/сек: {OperationsPerSecond:N0}";
        }
    }
}