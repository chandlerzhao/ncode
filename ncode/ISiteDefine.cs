using System.Collections.Generic;

namespace ncode
{
    public class SiteInfo
    {
        // Main info
        public string Title { get; set; }

        public string Genre { get; set; }
        public string Author { get; set; }
        public string Intro { get; set; }

        // Index info
        public string Volume { get; set; }

        public string Chapter { get; set; }

        public enum V_C_Type { Null, Fold, Plat, }

        public V_C_Type type { get; set; }
        public string ChLink { get; set; }

        // Chapter info
        public string SubTitle { get; set; }

        public string Text { get; set; }
    }

    public interface ISiteDefine
    {
        Dictionary<string, SiteInfo> Define { get; set; }

        void Load(string path);

        void Save(string path);
    }
}