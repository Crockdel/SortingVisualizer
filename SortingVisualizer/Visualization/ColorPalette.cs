using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace SortingVisualizer.Visualization
{
    // Цветовая схема для визуализации
    public static class ColorPalette
    {
        // Основные цвета
        public static Color DefaultBar = Color.FromArgb(70, 130, 180); // Стальной синий
        public static Color SortedBar = Color.FromArgb(60, 179, 113);  // Морской зеленый
        public static Color ActiveBar = Color.FromArgb(255, 215, 0);   // Золотой
        public static Color CompareBar = Color.FromArgb(255, 99, 71);  // Томатный
        public static Color Background = Color.White;
        public static Color GridLine = Color.FromArgb(240, 240, 240);

        // Получить цвет для элемента
        public static Color GetBarColor(int value, int maxValue, int index,
                                        int activeIndex1, int activeIndex2, bool isSorted)
        {
            if (isSorted)
                return SortedBar;

            if (index == activeIndex1 && index == activeIndex2)
                return ActiveBar;

            if (index == activeIndex1 || index == activeIndex2)
                return CompareBar;

            // Градиент на основе значения
            int intensity = (int)((value / (float)maxValue) * 155) + 100;
            return Color.FromArgb(100, 100, intensity);
        }
    }
}