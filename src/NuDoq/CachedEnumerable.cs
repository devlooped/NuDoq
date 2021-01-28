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
        public static IEnumerable<T> Cached<T>(this IEnumerable<T> enumerable) => new CachedEnumerableImpl<T>(enumerable);

        class CachedEnumerableImpl<T> : IEnumerable<T>
        {
            readonly IEnumerable<T> enumerable;
            List<T>? cache;

            public CachedEnumerableImpl(IEnumerable<T> enumerable) => this.enumerable = enumerable;

            public IEnumerator<T> GetEnumerator() => (cache ??= new List<T>(enumerable)).GetEnumerator();

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }
    }
}