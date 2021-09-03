using System.Collections.Generic;
using GaleShapleyAlgoritm.PollingPlaces;

namespace GaleShapleyAlgoritm.Selectors
{
    public class Selector
    {
        /// <summary>
        /// Вектор предпочтений избирателя
        /// </summary>
        public PreferenceVector<PollingPlace> OrderedPreferences;
        
        /// <summary>
        /// Ключ избиретеля
        /// </summary>
        public int Key { get; set; }
        
        /// <summary>
        /// Ключ выбранного места
        /// </summary>
        public int? PlaceKey { get; set; }
        
        /// <summary>
        /// Создание избирателя
        /// </summary>
        /// <param name="key">Ключ избирателя</param>
        /// <param name="places">Уникальные места для выбора. Сортировка предпочтений берется по умолчанию</param>
        public Selector(int key, ISet<PollingPlace> places)
        {
            Key = key;
            OrderedPreferences = new PreferenceVector<PollingPlace>(places, new KeyPollingPlaceComparer());
        }

        /// <summary>
        /// Создание избирателя
        /// </summary>
        /// <param name="key">Ключ избирателя</param>
        /// <param name="places">Уникальные места для выбора</param>
        /// <param name="compareFunction">Функция для сортировки мест для избирателя</param>
        public Selector(int key, ISet<PollingPlace> places, IComparer<PollingPlace> compareFunction)
        {
            Key = key;
            OrderedPreferences = new PreferenceVector<PollingPlace>(places, compareFunction);
        }
    }
}