using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using GaleShapleyAlgoritm.PollingPlaces;
using GaleShapleyAlgoritm.Selectors;

namespace GaleShapleyAlgoritm
{
    class Program
    {
        // Дано 3 избирателя
        private static readonly Selector _firstSelector = new Selector() { Key = 1 };
        private static readonly Selector _secondSelector = new Selector() { Key = 2 };
        private static readonly Selector _thirdSelector = new Selector() { Key = 3 };

        // Дано 2 мест. С квотами на 1 и 2 места, соответственно
        private static PollingPlace _firstPollingPlace = new PollingPlace() { Key = 1, Capacity = 1 };
        private static PollingPlace _secondPollingPlace = new PollingPlace() { Key = 2, Capacity = 2 };

        // Множество вариантов мест и избирателей
        private static HashSet<PollingPlace> _places;
        private static HashSet<Selector> _selectors;

        static void Main(string[] args)
        {
            _places = new HashSet<PollingPlace>() { _firstPollingPlace, _secondPollingPlace };
            _selectors = new HashSet<Selector>() { _firstSelector, _secondSelector, _thirdSelector };


            // Каждый из избирателей имеет свой вектор предпочтений относительно мест
            // Вектор предпочтений - стандартные. Лучше считается место по ключу больше/меньше.
            // В общем случае можно описать любую кастомную функцию выбора

            // Первый избиратель выберет место с наибольшим ключем
            _firstSelector.SetPlaces(_places, new ReverseKeyPollingPlaceComparer());

            // Остальные, наоборот, считают лучшим местом, место с меньшим ключем
            _secondSelector.SetPlaces(_places, new KeyPollingPlaceComparer());
            _thirdSelector.SetPlaces(_places, new KeyPollingPlaceComparer());


            // Аналогично, есть векторы предпочтений для мест, а также квоты
            // Первое место имеет функцию предпочтения МАХ и квоту на 1 место, второе  функцию - MIN и 2 места по квоте

            _firstPollingPlace.SetSelectors(_selectors, new ReverseKeySelectorComparer());
            _secondPollingPlace.SetSelectors(_selectors, new KeySelectorComparer());

            RunAlgorithm();

            Print();
        }

        /// <summary>
        /// Запуск алгоритма Гейла - Шепли
        /// </summary>
        private static void RunAlgorithm()
        {
            // В текущей реализации квота == количество желающих, поэтому
            // Алгоритм завершится, когда все найдут места
            while (_selectors.Any(x => x?.PlaceKey == null))
            {
                // 1. Все избиратели, которые еще не определились - идут на самое предпочтительное место
                foreach (var selector in _selectors.Where(x => x.PlaceKey == null))
                {
                    selector.PlaceKey = selector.OrderedPreferences.Items.First().Key;
                }

                // 2. Все места, которые еще не заполнены, выбирают себе по своим предпочтениям избирателей, учитывая квоту
                foreach (var pollingPlace in _places.Where(x => x.Capacity != 0))
                {
                    // В кандидаты на участие идут все избиратели, которые постучались в место в этом раунде
                    var candidates = pollingPlace.
                        OrderedPreferences.Items.
                        Where(x => x.PlaceKey == pollingPlace.Key).
                        Take(pollingPlace.Capacity);


                    pollingPlace.ApprovedSelectors = pollingPlace?.ApprovedSelectors?.Any() == true 
                        ? pollingPlace.ApprovedSelectors.Union(candidates) : 
                        candidates;

                    pollingPlace.Capacity -= candidates.Count();
                }

                // 3. Все избиратели, кто не вошел, идут в следующее по предпочтении место
                foreach (var selector in _selectors)
                {
                    var preferredPlace = _places.FirstOrDefault(x => x.Key == selector.PlaceKey);

                    if (!preferredPlace.ApprovedSelectors.Contains(selector))
                    {
                        selector.PlaceKey = null;
                        selector.UpdatePlaces(selector.OrderedPreferences.Items.Skip(1).ToHashSet());
                    }
                }
            }
        }

        private static void Print()
        {
            // Выводим значения по столбцам
            // В заголовке пишем место, затем, все его избиратели, в порядке приоритета
            foreach (var pollingPlace in _places)
            {
                Console.WriteLine(pollingPlace.ToString());
            }

        }
    }
}
