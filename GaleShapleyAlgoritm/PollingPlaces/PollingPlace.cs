using System.Collections.Generic;
using System.Linq;
using GaleShapleyAlgoritm.Selectors;

namespace GaleShapleyAlgoritm.PollingPlaces
{
    public class PollingPlace
    {
        /// <summary>
        /// Вектор предпочтений для места
        /// </summary>
        public PreferenceVector<Selector> OrderedPreferences;
        
        /// <summary>
        /// Квота на количество избирателей
        /// </summary>
        public int Capacity { get; set; }
        
        /// <summary>
        /// Ключ места
        /// </summary>
        public int Key { get; set; }

        /// <summary>
        /// Избиратели, которые прошли квоту и закрепились за местом
        /// </summary>
        public IEnumerable<Selector> ApprovedSelectors { get; set; }
        
        /// <summary>
        /// Создание места
        /// </summary>
        /// <param name="key">Ключ места</param>
        /// <param name="capacity">Квота принятия</param>
        /// <param name="selectors">Уникальные избирателя для выбора. Сортировка предпочтений берется по умолчанию</param>
        public PollingPlace(int key, int capacity, ISet<Selector> selectors)
        {
            Key = key;
            Capacity = capacity;
            OrderedPreferences = new PreferenceVector<Selector>(selectors, new MinKeySelectorComparer());
        }

        /// <summary>
        /// Создание места
        /// </summary>
        /// <param name="key">Ключ места</param>
        /// <param name="capacity">Квота принятия</param>
        /// <param name="selectors">Уникальные избирателя для выбора</param>
        /// <param name="compareFunction">Функция для сортировки мест для избирателя</param>
        public PollingPlace(int key, int capacity, ISet<Selector> selectors, IComparer<Selector> compareFunction)
        {
            Key = key;
            Capacity = capacity;
            OrderedPreferences = new PreferenceVector<Selector>(selectors, compareFunction);
        }

        public PollingPlace()
        {
        }

        public void SetSelectors(ISet<Selector> selectors, IComparer<Selector> compareFunction)
        {
            OrderedPreferences = new PreferenceVector<Selector>(selectors, compareFunction);
        }

        public override string ToString()
        {
            string orderedSelectorKeys = string.Join(",", ApprovedSelectors.Select(x => x.Key));
            return
                $"Place: {this.Key}, Selectors: {orderedSelectorKeys}, Count: {ApprovedSelectors.Count()}";
        }
    }
}