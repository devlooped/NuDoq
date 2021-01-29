using System.Collections.Generic;

namespace NuDoq
{
    static class DictionaryExtensions
    {
        /// <summary>
        /// If the <paramref name="value"/> is <see langword="null"/>, the 
        /// entry is removed, otherwise, its value is assigned in the dictionary.
        /// </summary>
        public static void SetOrRemove(this IDictionary<string, string> dictionary, string name, string? value)
        {
            if (value == null)
                dictionary.Remove("href");
            else
                dictionary["href"] = value;
        }
    }
}
