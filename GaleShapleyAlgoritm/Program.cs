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
        private static ISet<PollingPlace> _places;
        private static ISet<Selector> _selectors;

        static void Main(string[] args)
        {
            _places = new HashSet<PollingPlace>() { _firstPollingPlace, _secondPollingPlace };
            _selectors = new HashSet<Selector>() { _firstSelector, _secondSelector, _thirdSelector };

            // Каждый из избирателей имеет свой вектор предпочтений относительно мест
            // Вектор предпочтений - стандартные. Лучше считается место по ключу больше/меньше.
            // В общем случае можно описать любую кастомную функцию выбора

            // Первый избиратель выберет место с наибольшим ключем
            _firstSelector.SetPlaces(_places, new MaxKeyPollingPlaceComparer());

            // Остальные, наоборот, считают лучшим местом, место с меньшим ключем
            _secondSelector.SetPlaces(_places, new MinKeyPollingPlaceComparer());
            _thirdSelector.SetPlaces(_places, new MinKeyPollingPlaceComparer());


            // Аналогично, есть векторы предпочтений для мест, а также квоты
            // Первое место имеет функцию предпочтения МАХ и квоту на 1 место, второе  функцию - MIN и 2 места по квоте

            _firstPollingPlace.SetSelectors(_selectors, new MaxKeySelectorComparer());
            _secondPollingPlace.SetSelectors(_selectors, new MinKeySelectorComparer());

            AlgorithmExecutor executor = new AlgorithmExecutor(_places, _selectors);
            (_places, _selectors) = executor.RunAlgorithm();

            Print();
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
