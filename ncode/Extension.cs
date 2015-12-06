using System;
using System.Collections.Generic;
using System.Linq;

namespace ncode
{
    internal static class Extension
    {
        public static string MultiTrim(this string source)
        {
            var single = source
                .Split(new char[] { '\r', '\n' })
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Select(x => x.Trim())
                .ToArray();
            return string.Concat(single).Trim();
        }

        public static void Add<TKey, TValue>(this List<KeyValuePair<TKey, TValue>> list, TKey key, TValue value)
        { list.Add(new KeyValuePair<TKey, TValue>(key, value)); }

        public static string ParentPath(this Uri uri)
        { return uri.AbsolutePath.Remove(uri.AbsolutePath.Length - uri.Segments.Last().Length); }

        public static string RootPath(this Uri uri)
        { return uri.Scheme + @"://" + uri.Host; }
    }
}