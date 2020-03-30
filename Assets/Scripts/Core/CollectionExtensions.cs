using System.Collections.Generic;

namespace Core
{
    public static class CollectionExtensions
    {
        public static TValue SafeGet<TKey, TValue>(this Dictionary<TKey, TValue> dictionary
            , TKey key)
        {
            if (dictionary == null || key == null)
            {
                return default;
            }

            if (!dictionary.ContainsKey(key))
            {
                return default;
            }

            return dictionary[key];
        }
    }
}