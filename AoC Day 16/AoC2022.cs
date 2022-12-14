using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC2022
{
    /// <summary>
    /// This class contains code that usually are helpful for multiple days, or aren't written by me.
    /// If any are by someone else, they are marked as such.
    /// </summary>
    static class HelpF
    {
        // Split a string into separate strings, as specified by the delimiter.
        public static string[] SplitToStringArray(this string str, string split, bool removeEmpty)
        {
            return str.Split(new string[] { split }, removeEmpty ? StringSplitOptions.RemoveEmptyEntries : StringSplitOptions.None);
        }

        // Split a string into separate strings, as specified by the delimiter.
        public static string[] SplitToStringArray(this string str, char[] split, bool removeEmpty)
        {
            return str.Split(split, removeEmpty ? StringSplitOptions.RemoveEmptyEntries : StringSplitOptions.None);
        }

        // Split a string into an int array.
        public static int[] SplitToIntArray(this string str, string split)
        {
            return Array.ConvertAll(str.SplitToStringArray(split, true), s => int.Parse(s));
        }

        // Split a string into an int array.
        public static int[] SplitToIntArray(this string str, params char[] split)
        {
            return Array.ConvertAll(str.SplitToStringArray(split, true), s => int.Parse(s));
        }

        // Split a string into a long array.
        public static long[] SplitToLongArray(this string str, string split)
        {
            return Array.ConvertAll(str.SplitToStringArray(split, true), s => long.Parse(s));
        }

        // Split a string into a long array.
        public static long[] SplitToLongArray(this string str, params char[] split)
        {
            return Array.ConvertAll(str.SplitToStringArray(split, true), s => long.Parse(s));
        }

        public static V Read<K, V>(this Dictionary<K, V> dict, K key)
        {
            if (dict.ContainsKey(key)) return dict[key];
            return default(V);
        }

        public static V Read<K, V>(this Dictionary<K, V> dict, K key, V def)
        {
            if (dict.ContainsKey(key)) return dict[key];
            return def;
        }
    }
}