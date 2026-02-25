using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Drawing2D;
using SortingVisualizer.Models;

namespace SortingVisualizer.Visualization
{
    public class ArrayRenderer : IDisposable
    {
        private Bitmap _bitmap;
        private Graphics _graphics;
        private int _width;
        private int _height;
        private bool _disposed;
        private readonly object _lockObject = new object(); // Для потокобезопасности

        public Bitmap Bitmap => _bitmap;

        public ArrayRenderer(int width, int height)
        {
            _width = Math.Max(1, width);
            _height = Math.Max(1, height);
            CreateBitmap();
        }

        private void CreateBitmap()
        {
            lock (_lockObject)
            {
                // Освобождаем старые ресурсы
                _graphics?.Dispose();
                _bitmap?.Dispose();

                // Создаем новые с проверкой размеров
                _width = Math.Max(1, _width);
                _height = Math.Max(1, _height);

                _bitmap = new Bitmap(_width, _height);
                _graphics = Graphics.FromImage(_bitmap);
                _graphics.SmoothingMode = SmoothingMode.AntiAlias;
                _graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
                _graphics.CompositingQuality = CompositingQuality.HighSpeed;
                _graphics.InterpolationMode = InterpolationMode.Low;
            }
        }

        public void Resize(int width, int height)
        {
            if (width <= 0 || height <= 0) return;

            lock (_lockObject)
            {
                if (_width == width && _height == height)
                    return;

                _width = width;
                _height = height;
                CreateBitmap();
            }
        }

        public void Render(SortArray array, int activeIndex1, int activeIndex2, bool isSorted)
        {
            if (_graphics == null || _bitmap == null) return;
            if (array == null || array.Length == 0) return;

            lock (_lockObject)
            {
                try
                {
                    // Очистка
                    _graphics.Clear(ColorPalette.Background);

                    int count = array.Length;
                    if (count == 0) return;

                    float barWidth = (float)_width / count;
                    int maxValue = array.MaxValue;

                    // Предотвращаем слишком маленькую ширину
                    if (barWidth < 1)
                        barWidth = 1;

                    // Рисуем столбцы
                    for (int i = 0; i < count; i++)
                    {
                        // Проверяем индексы
                        if (i >= array.Length) break;

                        float barHeight = (array.Values[i] / (float)maxValue) * _height * 0.9f;
                        float x = i * barWidth;
                        float y = _height - barHeight;

                        // Проверка на выход за границы
                        if (x + barWidth > _width)
                            barWidth = _width - x;

                        Color color = ColorPalette.GetBarColor(
                            array.Values[i], maxValue, i,
                            activeIndex1, activeIndex2, isSorted);

                        using (var brush = new SolidBrush(color))
                        {
                            _graphics.FillRectangle(brush, x, y, Math.Max(1, barWidth - 1), barHeight);
                        }

                        // Для малых массивов показываем значения
                        if (count <= 50 && barWidth > 15)
                        {
                            DrawValue(i, array.Values[i], x, y, barWidth);
                        }
                    }

                    // Рисуем рамку
                    using (var pen = new Pen(Color.FromArgb(200, 200, 200)))
                    {
                        _graphics.DrawRectangle(pen, 0, 0, _width - 1, _height - 1);
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Ошибка рендеринга: {ex.Message}");
                }
            }
        }

        private void DrawValue(int index, int value, float x, float y, float barWidth)
        {
            try
            {
                using (var font = new Font("Arial", 8))
                using (var brush = new SolidBrush(Color.Black))
                {
                    string text = value.ToString();
                    var size = _graphics.MeasureString(text, font);

                    // Проверяем, поместится ли текст
                    if (size.Width < barWidth - 2)
                    {
                        _graphics.DrawString(text, font, brush,
                            x + (barWidth - size.Width) / 2,
                            y - size.Height - 2);
                    }
                }
            }
            catch { /* Игнорируем ошибки отрисовки текста */ }
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                lock (_lockObject)
                {
                    _graphics?.Dispose();
                    _bitmap?.Dispose();
                    _graphics = null;
                    _bitmap = null;
                }
                _disposed = true;
            }
        }
    }
}