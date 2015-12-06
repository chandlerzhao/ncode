using System.Collections.Generic;

namespace ncode
{
    public class SiteInfo
    {
        public struct _HeadPage
        {
            public enum S_Loc { Null, Cover, Catalog, }

            public S_Loc SynopLoc { get; set; }
            public KeyValuePair<string, string> Redirect { get; set; }
            public string Title { get; set; }
            public string Genre { get; set; }
            public string SubGenre { get; set; }
            public string Author { get; set; }
            public string Synopsis { get; set; }
        }

        public _HeadPage HeadPage { get; set; }

        public struct _Volume
        {
            public enum _Type { Null, Fold, Plat, }

            public string Handle { get; set; }
            public string Name { get; set; } // Relative of @Handle
            public _Type Type { get; set; }
        }

        public _Volume Volume { get; set; }

        public struct _Chapter
        {
            public string Handle { get; set; } // Relative of @Volume.@Handle when @Fold, Absolute when @Plat
            public string Name { get; set; } // Relative of @Handle
            public string Link { get; set; } // Relative of @Handle
        }

        public _Chapter Chapter { get; set; }

        public struct _TextPage
        {
            public string Handle { get; set; }
            public string Name { get; set; } // Relative of @Handle
            public string Body { get; set; } // Relative of @Handle
        }

        public _TextPage TextPage { get; set; }
    }

    public interface ISiteDefine
    {
        Dictionary<string, SiteInfo> Define { get; set; }

        void Load(string path);

        void Save(string path);
    }
}