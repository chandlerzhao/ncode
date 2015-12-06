using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using YamlDotNet.Serialization;

namespace ncode
{
    internal class YamlSiteDefine : ISiteDefine
    {
        public Dictionary<string, SiteInfo> Define { get; set; }

        public YamlSiteDefine()
        {
            Define = new Dictionary<string, SiteInfo>();
        }

        public void Load(string path)
        {
            var d = new Deserializer();
            var sw = new StreamReader(path, Encoding.UTF8, true);
            Define = d.Deserialize<Dictionary<string, SiteInfo>>(sw);
            sw.Close();
        }

        public void Save(string path)
        {
            throw new NotImplementedException(); // method closed

            var s = new Serializer();
            var sr = new StringWriter();
            s.Serialize(sr, Define);
            var sw = new StreamWriter(path, false, Encoding.UTF8);
            sw.Write(sr.ToString());
            sw.Close();
            sr.Close();
        }
    }
}