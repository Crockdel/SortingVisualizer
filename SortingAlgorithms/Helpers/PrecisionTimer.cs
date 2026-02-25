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

    /// Высокоточный таймер для микро-задержек

    public static class PrecisionTimer
    {
        // Импортируем WinAPI функции для высокоточных задержек
        [DllImport("winmm.dll", EntryPoint = "timeBeginPeriod")]
        private static extern uint TimeBeginPeriod(uint uMilliseconds);

        [DllImport("winmm.dll", EntryPoint = "timeEndPeriod")]
        private static extern uint TimeEndPeriod(uint uMilliseconds);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool QueryPerformanceFrequency(out long frequency);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool QueryPerformanceCounter(out long count);

        private static readonly long _frequency;
        private static bool _highResSupported;

        static PrecisionTimer()
        {
            _highResSupported = QueryPerformanceFrequency(out _frequency);
        }

    
        /// Точная задержка в миллисекундах (поддерживает дробные значения)
    
        public static void Delay(double milliseconds)
        {
            if (milliseconds <= 0) return;

            // Для задержек меньше 1 мс используем высокоточный таймер
            if (milliseconds < 1.0 && _highResSupported)
            {
                HighResDelay(milliseconds);
            }
            else
            {
                // Для задержек >= 1 мс используем Thread.Sleep
                int ms = (int)Math.Floor(milliseconds);
                if (ms > 0)
                {
                    Thread.Sleep(ms);
                }

                // Остаток микросекунд добиваем высокоточным таймером
                double remainder = milliseconds - ms;
                if (remainder > 0.001) // больше 1 микросекунды
                {
                    HighResDelay(remainder);
                }
            }
        }

        private static void HighResDelay(double milliseconds)
        {
            // Устанавливаем высокое разрешение таймера
            TimeBeginPeriod(1);

            long start, current;
            QueryPerformanceCounter(out start);

            double targetTicks = (_frequency * milliseconds) / 1000.0;
            long targetCount = (long)targetTicks;

            do
            {
                QueryPerformanceCounter(out current);
                // Для очень маленьких задержек используем SpinWait
                if (targetTicks < 1000) // меньше 1 мс
                {
                    Thread.SpinWait(10);
                }
            } while ((current - start) < targetCount);

            TimeEndPeriod(1);
        }

    
        /// Активное ожидание с минимальной задержкой
    
        public static void SpinDelay(int nanoseconds)
        {
            if (nanoseconds <= 0) return;

            var start = DateTime.Now.Ticks;
            var target = start + (nanoseconds / 100); // конвертация в тики (1 тик = 100 нс)

            while (DateTime.Now.Ticks < target)
            {
                Thread.SpinWait(1);
            }
        }
    }
}