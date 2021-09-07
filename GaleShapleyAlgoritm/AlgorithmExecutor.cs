using System.Collections.Generic;
using System.Linq;
using GaleShapleyAlgoritm.PollingPlaces;
using GaleShapleyAlgoritm.Selectors;

namespace GaleShapleyAlgoritm
{
    public class AlgorithmExecutor
    { 
        // Множество вариантов мест и избирателей
        private ISet<PollingPlace> _places;
        private ISet<Selector> _selectors;

        public AlgorithmExecutor(ISet<PollingPlace> places, ISet<Selector> selectors)
        {
            _places = places;
            _selectors = selectors;
        }

        /// <summary>
        /// Запуск алгоритма Гейла - Шепли
        /// </summary>
        public (ISet<PollingPlace> _places, ISet<Selector> _selectors) RunAlgorithm()
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

            return (_places, _selectors);
        }
    }
}