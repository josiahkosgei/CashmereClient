
//HierarchyMethods


using System;
using System.Collections.Generic;
using System.Linq;

namespace Cashmere.Library.Standard.Utilities
{
    public static class HierarchyMethods
    {
        public static IEnumerable<T> Ancestors<T>(T item, Func<T, T> parentSelector)
        {
            while (item != null)
            {
                item = parentSelector(item);
                yield return item;
            }
        }

        public static bool Repeats<T>(this IEnumerable<T> sequence, IEqualityComparer<T> comparer = null)
        {
            try
            {
                comparer = comparer ?? EqualityComparer<T>.Default;
                HashSet<T> objSet = new HashSet<T>(comparer);
                foreach (T obj in sequence)
                {
                    if (!objSet.Add(obj))
                        return true;
                }
                return false;
            }
            catch (NullReferenceException ex)
            {
                return false;
            }
        }

        public static bool ContainsCycles<T>(
          IEnumerable<T> sequence,
          Func<T, T> parentSelector,
          IEqualityComparer<T> comparer = null)
        {
            comparer = comparer ?? EqualityComparer<T>.Default;
            return sequence.Any(item => Ancestors(item, parentSelector).Repeats(comparer));
        }
    }
}
