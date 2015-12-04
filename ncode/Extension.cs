using System.Collections.Generic;
using HtmlAgilityPack;
using System.Linq;

namespace ncode
{
    internal static class Extension
    {
        public static HtmlNode _GetFirstNode(this HtmlNode root, string AttrName, string AttrValue)
        { return root.ChildNodes.Where(x => x.Attributes.Count != 0 && x.Attributes.Select(y => y.Name).ToList().IndexOf(AttrName) != -1 && x.Attributes[AttrName].Value == AttrValue).FirstOrDefault(); }

        public static HtmlNode _GetFirstNode(this HtmlNode root, string Name, string AttrName, string AttrValue)
        { return root.ChildNodes.Where(x => x.Name == Name).Where(x => x.Attributes.Count != 0 && x.Attributes.Select(y => y.Name).ToList().IndexOf(AttrName) != -1 && x.Attributes[AttrName].Value == AttrValue).FirstOrDefault(); }

        public static HtmlNode[] _GetNodes(this HtmlNode root, string Name)
        { return root.ChildNodes.Where(x => x.Name == Name).ToArray(); }

        public static HtmlNode[] _GetNodes(this HtmlNode root, string AttrName, string AttrValue)
        { return root.ChildNodes.Where(x => x.Attributes.Count != 0 && x.Attributes.Select(y => y.Name).ToList().IndexOf(AttrName) != -1 && x.Attributes[AttrName].Value == AttrValue).ToArray(); }

        public static HtmlNode[] _GetNodes(this HtmlNode root, string Name, string AttrName, string AttrValue)
        { return root.ChildNodes.Where(x => x.Name == Name).Where(x => x.Attributes.Count != 0 && x.Attributes.Select(y => y.Name).ToList().IndexOf(AttrName) != -1 && x.Attributes[AttrName].Value == AttrValue).ToArray(); }

        public static HtmlNode _RemoveNodes(this HtmlNode root, string Name)
        { foreach (var r in root._GetNodes(Name)) { r.Remove(); } return root; }

        public static string _Clean(this string source)
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
    }
}