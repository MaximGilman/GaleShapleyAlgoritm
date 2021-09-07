using System.Collections.Generic;

namespace GaleShapleyAlgoritm.Selectors
{
    /// <summary>
    ///  Функция для выбора лучшего избирателя с точки зрения места
    /// Базовая версия. Считает наилучшим, элемент с наименьшим ключем. В общем случае может быть отдельный компарер для каждого избирателя
    /// </summary>
    public class MaxKeySelectorComparer : IComparer<Selector>
    {
        public int Compare(Selector x, Selector y)
        {
            if (ReferenceEquals(x, y)) return 0;
            if (ReferenceEquals(null, y)) return 1;
            if (ReferenceEquals(null, x)) return -1;
            var keyComparison = x.Key.CompareTo(y.Key);
            return keyComparison != 0 ?
                keyComparison * -1 :
                x.Key.CompareTo(y.Key);
        }
    }
}