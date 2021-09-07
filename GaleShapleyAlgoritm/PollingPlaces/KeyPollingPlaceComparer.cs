using System.Collections.Generic;

namespace GaleShapleyAlgoritm.PollingPlaces
{
    /// <summary>
    /// Функция для выбора лучшего места для избирателя
    /// Базовая версия. Считает наилучшим, элемент с наибольшем ключем. В общем случае может быть отдельный компарер для каждого избирателя
    /// </summary>
    public class MinKeyPollingPlaceComparer :IComparer<PollingPlace>
    {
        public int Compare(PollingPlace x, PollingPlace y)
        {
            if (ReferenceEquals(x, y)) return 0;
            if (ReferenceEquals(null, y)) return 1;
            if (ReferenceEquals(null, x)) return -1;
            var keyComparison = x.Key.CompareTo(y.Key);
            return keyComparison != 0 ?
                keyComparison :
                x.Key.CompareTo(y.Key);
        }
    }
}