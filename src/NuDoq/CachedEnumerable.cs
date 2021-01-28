using System.Collections;
using System.Collections.Generic;

namespace NuDoq
{
    /// <summary>
    /// Internal class used to cache the results of the enumeration of the 
    /// container elements.
    /// </summary>
    static class CachedEnumerable
    {
        public static IEnumerable<T> Cached<T>(this IEnumerable<T> enumerable)
        {
            return new CachedEnumerableImpl<T>(enumerable);
        }

        class CachedEnumerableImpl<T> : IEnumerable<T>
        {
            IEnumerator<T> enumerator;
            IEnumerable<T> enumerable;
            List<T> cache = new List<T>();

            public CachedEnumerableImpl(IEnumerable<T> enumerable)
            {
                this.enumerable = enumerable;
            }

            public IEnumerator<T> GetEnumerator()
            {
                // First time around, there will be nothing in 
                // this cache.
                foreach (var item in cache)
                {
                    yield return item;
                }

                // First time we'll get the enumerator, only 
                // once. Next time, it will already have a value
                // and so we won't enumerate twice ever.
                if (enumerator == null)
                    enumerator = enumerable.GetEnumerator();

                // First time around, we'll loop until we're done. 
                // Next time it's enumerated, this enumerator will 
                // return false from MoveNext right-away.
                while (enumerator.MoveNext())
                {
                    cache.Add(enumerator.Current);
                    yield return enumerator.Current;
                }
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }
    }
}