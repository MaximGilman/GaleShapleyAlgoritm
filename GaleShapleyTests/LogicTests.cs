using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using GaleShapleyAlgoritm;
using GaleShapleyAlgoritm.PollingPlaces;
using GaleShapleyAlgoritm.Selectors;
using Xunit;
using Xbehave;

namespace GaleShapleyTests
{
    public class LogicTests : TestsBase
    {
        [Scenario]
        public void CheckOneToOnePairWasCreated(Selector firstSelector, PollingPlace firstPlace,
            ISet<Selector> allSelectors,
            ISet<PollingPlace> allPlaces, AlgorithmExecutor executor)
        {
            "��� ����������".x(() => firstSelector = new Selector() {Key = 1});
            "���� �����".x(() => firstPlace = new PollingPlace() {Key = 1, Capacity = 1});

            "��� ���������� ��������� ���������".x(() => allSelectors = new HashSet<Selector>() {firstSelector});
            "��� ����� ��������� ���������".x(() => allPlaces = new HashSet<PollingPlace>() {firstPlace});

            "���������� ����� ������� � �����".x(() =>
                firstSelector.SetPlaces(allPlaces, new MaxKeyPollingPlaceComparer()));
            "����� ���� �������������� � ����������".x(() =>
                firstPlace.SetSelectors(allSelectors, new MaxKeySelectorComparer()));

            "����� �������� �����������".x(() =>
            {
                executor = new AlgorithmExecutor(allPlaces, allSelectors);
                (allPlaces, allSelectors) = executor.RunAlgorithm();
            });

            "��� ���������� ����� �����".x(() =>
            {
                foreach (var selector in allSelectors)
                {
                    Assert.NotNull(selector.PlaceKey);
                }
            });

            "��� ����� ����� �����������".x(() =>
            {
                foreach (var place in allPlaces)
                {
                    Assert.NotEmpty(place.ApprovedSelectors);
                }
            });

            "������ ���������� ������ ������ �����".x(() => Assert.Equal(firstSelector.PlaceKey, firstPlace.Key));

            "������ ����� ������� ������ ������ ����������".x(() =>
            {
                var placeSelectors = firstPlace.ApprovedSelectors.Count();
                Assert.Equal(1, placeSelectors);

                Assert.Equal(firstPlace.ApprovedSelectors.First().Key, firstSelector.Key);
            });

        }
    }
}
