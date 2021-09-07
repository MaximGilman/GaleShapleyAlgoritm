using System.Collections.Generic;
using System.Linq;
using GaleShapleyAlgoritm;
using GaleShapleyAlgoritm.PollingPlaces;
using GaleShapleyAlgoritm.Selectors;
using Xbehave;
using Xunit;

namespace GaleShapleyTests
{
    public class LogicGSTests : TestsBase
    {
        [Scenario]
        public void CheckOneToOnePairWasCreated(Selector firstSelector, PollingPlace firstPlace,
           ISet<Selector> allSelectors,
           ISet<PollingPlace> allPlaces, AlgorithmExecutor executor)
        {
            "Дан избиратель".x(() => firstSelector = new Selector() { Key = 1 });
            "Дано место".x(() => firstPlace = new PollingPlace() { Key = 1, Capacity = 1 });

            "Все избиратели формируют множество".x(() => allSelectors = new HashSet<Selector>() { firstSelector });
            "Все места формируют множество".x(() => allPlaces = new HashSet<PollingPlace>() { firstPlace });

            "Избиратель хочет попасть в место".x(() =>
                firstSelector.SetPlaces(allPlaces, new MaxKeyPollingPlaceComparer()));
            "Место тоже заинтересовано в избирателе".x(() =>
                firstPlace.SetSelectors(allSelectors, new MaxKeySelectorComparer()));

            "Когда алгоритм запускается".x(() =>
            {
                executor = new AlgorithmExecutor(allPlaces, allSelectors);
                (allPlaces, allSelectors) = executor.RunAlgorithm();
            });

            "Все избиратели нашли место".x(() =>
            {
                foreach (var selector in allSelectors)
                {
                    Assert.NotNull(selector.PlaceKey);
                }
            });

            "Все места нашли избирателей".x(() =>
            {
                foreach (var place in allPlaces)
                {
                    Assert.NotEmpty(place.ApprovedSelectors);
                }
            });

            "Первый избиратель выбрал первое место".x(() => Assert.Equal(firstSelector.PlaceKey, firstPlace.Key));

            "Первые место посетил только первый избиратель".x(() =>
            {
                var placeSelectors = firstPlace.ApprovedSelectors.Count();
                Assert.Equal(1, placeSelectors);

                Assert.Equal(firstPlace.ApprovedSelectors.First().Key, firstSelector.Key);
            });

        }
    }
}