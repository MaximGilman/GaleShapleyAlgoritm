using System;
using System.Collections.Generic;
using System.Linq;
using GaleShapleyAlgoritm.PollingPlaces;
using GaleShapleyAlgoritm.Selectors;

namespace GaleShapleyAlgoritm
{
    class Program
    {
        // Дано 3 избирателя
        private static Selector _firstSelector;
        private static Selector _secondSelector;
        private static Selector _thirdSelector;

        // Дано 2 мест
        private static PollingPlace _firstPollingPlace;
        private static PollingPlace _secondPollingPlace;

        // Множество вариантов мест и избирателей
        private static readonly HashSet<PollingPlace> _places = new HashSet<PollingPlace>() { _firstPollingPlace, _secondPollingPlace };
        private static readonly HashSet<Selector> _selectors = new HashSet<Selector>() { _firstSelector, _secondSelector, _thirdSelector };

        static void Main(string[] args)
        {
            // Каждый из избирателей имеет свой вектор предпочтений относительно мест
            // Вектор предпочтений - стандартные. Лучше считается место по ключу больше/меньше.
            // В общем случае можно описать любую кастомную функцию выбора

            // Первый избиратель выберет место с наибольшим ключем
            _firstSelector = new Selector(key: 1, _places, new KeyPollingPlaceComparer());

            // Остальные, наоборот, считают лучшим местом, место с меньшим ключем
            _secondSelector = new Selector(key: 2, _places, new ReverseKeyPollingPlaceComparer());
            _thirdSelector = new Selector(key: 3, _places, new ReverseKeyPollingPlaceComparer());


            // Аналогично, есть векторы предпочтений для мест, а также квоты
            // Первое место имеет функцию предпочтения МАХ и квоту на 1 место, второе  функцию - MIN и 2 места по квоте

            _firstPollingPlace = new PollingPlace(1, 1, _selectors, new KeySelectorComparer());
            _secondPollingPlace = new PollingPlace(2, 2, _selectors, new ReverseKeySelectorComparer());

            RunAlgorithm();
        }

        /// <summary>
        /// Запуск алгоритма Гейла - Шепли
        /// </summary>
        private static void RunAlgorithm()
        {
            // В текущей реализации квота == количество желающих, поэтому
            // Алгоритм завершится, когда все найдут места
            while (_selectors.All(x => x.PlaceKey != null))
            {
                // 1. Все избиратели, которые еще не определились - идут на самое предпочтительное место
                foreach (var selector in _selectors.Where(x => x.PlaceKey == null))
                {
                    selector.PlaceKey = selector.OrderedPreferences.Items.First().Key;
                }

                // 2. Все места, которые еще не заполнены, выбирают себе по своим предпочтениям избирателей, учитывая квоту
                foreach (var pollingPlace in _places.Where(x => x.ApprovedSelectors?.Count() == x.Capacity))
                {
                    var candidates = pollingPlace.OrderedPreferences.Items.Take(pollingPlace.Capacity);
                    pollingPlace.ApprovedSelectors = candidates;
                    pollingPlace.Capacity -= candidates.Count();
                }

                // 3. Все избиратели, кто не вошел, идут в следующее по предпочтении место
                {
                    // Взять всех не вошедших избирателей
                    // Убрать у них первое место
                    // Обнулить PlaceKey
                }
            }
        }
    }
}
