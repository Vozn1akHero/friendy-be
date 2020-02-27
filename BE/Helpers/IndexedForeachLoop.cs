using System.Collections.Generic;
using System.Linq;

namespace BE.Helpers
{
    public static class IndexedForeachLoop
    {
        public static IEnumerable<(T item, int index)> WithIndex<T>(IEnumerable<T> source)
        {
            return source.Select((item, index) => (item, index));
        }
    }
}