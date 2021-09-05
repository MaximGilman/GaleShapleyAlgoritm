using System.Collections.Generic;

namespace GaleShapleyAlgoritm
{
    public class PreferenceVector <T>
    {
        public SortedSet<T> Items { get; set; }

        public PreferenceVector(IEnumerable<T> items, IComparer<T> comparer) 
        {
            Items = new SortedSet<T>(items, comparer);
        }
    }
}