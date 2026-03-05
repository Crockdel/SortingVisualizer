using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;

namespace SortingVisualizer.Helpers
{
    /// <summary>
    /// Высокоточный таймер для микро-задержек в алгоритмах сортировки
    /// </summary>
    public static class PrecisionTimer
    {
        [DllImport("winmm.dll", EntryPoint = "timeBeginPeriod")]
        private static extern uint TimeBeginPeriod(uint uMilliseconds);

        [DllImport("winmm.dll", EntryPoint = "timeEndPeriod")]
        private static extern uint TimeEndPeriod(uint uMilliseconds);

        private static readonly long _frequency;
        private static readonly bool _isHighResolution;
        private static readonly double _ticksPerMicrosecond;
        private static int _timePeriodSet;

        static PrecisionTimer()
        {
            _isHighResolution = Stopwatch.IsHighResolution;
            _frequency = Stopwatch.Frequency;
            _ticksPerMicrosecond = _frequency / 1000000.0;
        }

        /// <summary>
        /// Точная задержка в миллисекундах (оптимизировано для 0-1 мс)
        /// </summary>
        public static void Delay(double milliseconds)
        {
            if (milliseconds <= 0) return;

            // Устанавливаем высокое разрешение таймера
            if (Interlocked.CompareExchange(ref _timePeriodSet, 1, 0) == 0)
            {
                TimeBeginPeriod(1);
            }

            // Для задержек >= 1 мс используем Thread.Sleep с высокой точностью
            if (milliseconds >= 1.0)
            {
                int ms = (int)milliseconds;
                Thread.Sleep(ms);

                // Остаток обрабатываем микро-задержкой
                double remainder = milliseconds - ms;
                if (remainder > 0.001)
                {
                    SpinDelay((long)(remainder * 1000));
                }
            }
            else
            {
                // Для задержек < 1 мс используем только SpinWait
                SpinDelay((long)(milliseconds * 1000));
            }
        }

        /// <summary>
        /// Задержка в микросекундах с использованием SpinWait (без переключения контекста)
        /// </summary>
        private static void SpinDelay(long microseconds)
        {
            if (microseconds <= 0) return;

            long start = Stopwatch.GetTimestamp();
            long targetTicks = (long)(microseconds * _ticksPerMicrosecond);
            long target = start + targetTicks;

            // Оптимизированное активное ожидание
            int spinCount = 0;
            while (Stopwatch.GetTimestamp() < target)
            {
                spinCount++;

                // Для очень маленьких задержек используем интенсивный SpinWait
                if (microseconds < 10)
                {
                    Thread.SpinWait(10);
                }
                // Для средних задержек используем прогрессивный SpinWait
                else if (microseconds < 100)
                {
                    Thread.SpinWait(Math.Min(100, spinCount));
                }
                // Для задержек близких к 1 мс иногда отдаем управление
                else if (target - Stopwatch.GetTimestamp() > _frequency / 10000) // > 0.1 мс
                {
                    Thread.Sleep(0); // Yield
                }
            }
        }

        /// <summary>
        /// Быстрая задержка для использования в циклах сортировки
        /// </summary>
        public static void FastDelay(double milliseconds, ref long lastTimestamp)
        {
            if (milliseconds <= 0) return;

            long now = Stopwatch.GetTimestamp();
            long targetTicks = (long)(milliseconds * _ticksPerMicrosecond * 1000); // в тиках
            long target = lastTimestamp + targetTicks;

            if (target > now)
            {
                // Активное ожидание
                while (Stopwatch.GetTimestamp() < target)
                {
                    Thread.SpinWait(10);
                }
            }

            lastTimestamp = Stopwatch.GetTimestamp();
        }

        /// <summary>
        /// Сброс точности таймера (вызывать при завершении)
        /// </summary>
        public static void Cleanup()
        {
            if (Interlocked.Exchange(ref _timePeriodSet, 0) == 1)
            {
                TimeEndPeriod(1);
            }
        }
    }
}