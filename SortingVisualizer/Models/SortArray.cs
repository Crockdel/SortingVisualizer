using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SortingVisualizer.Models
{
    // Модель массива для сортировки
    public class SortArray
    {
        private int[] _values;
        private readonly Random _random = new Random();

        public int[] Values => _values;
        public int Length => _values.Length;
        public int MaxValue { get; private set; }

        public SortArray(int size)
        {
            CreateNewArray(size);
        }

     
        // Создание нового массива
 
        public void CreateNewArray(int size)
        {
            _values = new int[size];
            for (int i = 0; i < size; i++)
            {
                _values[i] = i + 1;
            }
            MaxValue = size;
        }


        // Перемешивание массива (алгоритм Фишера-Йетса)
        public void Shuffle()
        {
            for (int i = _values.Length - 1; i > 0; i--)
            {
                int j = _random.Next(i + 1);
                Swap(i, j);
            }
        }

        // Обмен двух элементов
        public void Swap(int i, int j)
        {
            int temp = _values[i];
            _values[i] = _values[j];
            _values[j] = temp;
        }

        // Проверка, отсортирован ли массив
        public bool IsSorted()
        {
            for (int i = 1; i < _values.Length; i++)
            {
                if (_values[i] < _values[i - 1])
                    return false;
            }
            return true;
        }

        // Копирование массива
        public SortArray Clone()
        {
            SortArray clone = new SortArray(Length);
            Array.Copy(_values, clone._values, Length);
            return clone;
        }

        // Обновление значений из другого массива
        public void UpdateFrom(int[] source)
        {
            if (source.Length == _values.Length)
            {
                Array.Copy(source, _values, _values.Length);
            }
        }
    }
}