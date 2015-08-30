    /// <summary>
    /// Extension methods for Enumerables
    /// </summary>
    public static class EnumerableExtensionMethods
    {
        /// <summary>
        /// Distincts elements the by the key provided
        /// </summary>
        public static IEnumerable<TSource> DistinctBy<TSource, TKey>
            (this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            var knownKeys = new HashSet<TKey>();
            return source.Where(element => knownKeys.Add(keySelector(element)));
        }


        public static bool None<TSource>(this IEnumerable<TSource> source)
        {
            return !source.Any();
        }


        /// <summary>
        /// Returns an empty list if the value passed in is null.
        /// </summary>
        public static IList<TSource> ToEmptyListIfNull<TSource>(this IEnumerable<TSource> source)
        {
            if (source == null)
            {
                return new List<TSource>();
            }

            return source.ToList();
        }
    }