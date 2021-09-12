using System.Collections.Generic;
using System.Linq;
using GaleShapleyAlgoritm;
using GaleShapleyAlgoritm.PollingPlaces;
using GaleShapleyAlgoritm.Selectors;
using Xbehave;
using Xunit;

namespace GaleShapleyTests
{
    public class SuccessLogicGSTests
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

        [Scenario]
        public void CheckReversePreferencesWillCorrect(Selector firstSelector, Selector secondSelector, PollingPlace firstPlace, PollingPlace secondPlace,
          ISet<Selector> allSelectors,
          ISet<PollingPlace> allPlaces, AlgorithmExecutor executor)
        {
            "Даны избиратели".x(() =>
            {
                firstSelector = new Selector() { Key = 1 };
                secondSelector = new Selector() { Key = 2 };
            });

            "Даны места".x(() =>
            {
                firstPlace = new PollingPlace() { Key = 1, Capacity = 1 };
                secondPlace = new PollingPlace() { Key = 2, Capacity = 1 };

            });

            "Все избиратели формируют множество".x(() => allSelectors = new HashSet<Selector>() { firstSelector, secondSelector });
            "Все места формируют множество".x(() => allPlaces = new HashSet<PollingPlace>() { firstPlace, secondPlace });

            "Избиратели хотят попасть в места с диаметральными функциями".x(() =>
            {
                // Первый хочет во второе место, а второй в первое
                firstSelector.SetPlaces(allPlaces, new MaxKeyPollingPlaceComparer());
                secondSelector.SetPlaces(allPlaces, new MinKeyPollingPlaceComparer());

            });
            "Места имеют одинаковый вектор предпочтений".x(() =>
            {
                // Оба места предпочтут максимального избирателя
                firstPlace.SetSelectors(allSelectors, new MaxKeySelectorComparer());
                secondPlace.SetSelectors(allSelectors, new MaxKeySelectorComparer());
            });

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

            "Первый избиратель выбрал второе место".x(() =>
             Assert.Equal(firstSelector.PlaceKey, secondPlace.Key));
            
            "Второй избиратель выбрал первое место".x(() =>
                Assert.Equal(secondSelector.PlaceKey, firstPlace.Key));
            
            "Первое место выбрало только второго избирателя".x(() =>
            {
                var placeSelectors = firstPlace.ApprovedSelectors.Count();
                Assert.Equal(1, placeSelectors);

                Assert.Equal(firstPlace.ApprovedSelectors.First().Key, secondSelector.Key);
            });

            "Второе место выбрало только первого избирателя".x(() =>
            {
                var placeSelectors = secondPlace.ApprovedSelectors.Count();
                Assert.Equal(1, placeSelectors);

                Assert.Equal(secondPlace.ApprovedSelectors.First().Key, firstSelector.Key);
            });

        }

        [Scenario]
        public void CheckPairwiseReverseLeadsReverseResult(Selector firstSelector, Selector secondSelector, PollingPlace firstPlace, PollingPlace secondPlace,
          ISet<Selector> allSelectors,
          ISet<PollingPlace> allPlaces, AlgorithmExecutor executor)
        {
            "Даны избиратели".x(() =>
            {
                firstSelector = new Selector() { Key = 1 };
                secondSelector = new Selector() { Key = 2 };
            });

            "Даны места".x(() =>
            {
                firstPlace = new PollingPlace() { Key = 1, Capacity = 1 };
                secondPlace = new PollingPlace() { Key = 2, Capacity = 1 };

            });

            "Все избиратели формируют множество".x(() => allSelectors = new HashSet<Selector>() { firstSelector, secondSelector });
            "Все места формируют множество".x(() => allPlaces = new HashSet<PollingPlace>() { firstPlace, secondPlace });

            "Избиратели хотят попасть в места с диаметральными функциями".x(() =>
            {
                // Первый хочет во второе место, а второй в первое
                firstSelector.SetPlaces(allPlaces, new MaxKeyPollingPlaceComparer());
                secondSelector.SetPlaces(allPlaces, new MinKeyPollingPlaceComparer());

            });
            "Места имеют диаметрально разный вектор предпочтений".x(() =>
            {
                // Первое место хочет первого избирателя, второе - второго
                firstPlace.SetSelectors(allSelectors, new MinKeySelectorComparer());
                secondPlace.SetSelectors(allSelectors, new MaxKeySelectorComparer());
            });

            "Таким образом, каждый будет желать перейти в другое место и алгоритм не остановится".x(() => { });

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

            "Первый избиратель выбрал второе место".x(() =>
             Assert.Equal(firstSelector.PlaceKey, secondPlace.Key));

            "Второй избиратель выбрал первое место".x(() =>
                Assert.Equal(secondSelector.PlaceKey, firstPlace.Key));

            "Первое место выбрало только второго избирателя".x(() =>
            {
                var placeSelectors = firstPlace.ApprovedSelectors.Count();
                Assert.Equal(1, placeSelectors);

                Assert.Equal(firstPlace.ApprovedSelectors.First().Key, secondSelector.Key);
            });

            "Второе место выбрало только первого избирателя".x(() =>
            {
                var placeSelectors = secondPlace.ApprovedSelectors.Count();
                Assert.Equal(1, placeSelectors);

                Assert.Equal(secondPlace.ApprovedSelectors.First().Key, firstSelector.Key);
            });

        }
    }
}