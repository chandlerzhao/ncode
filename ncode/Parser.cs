using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace ncode
{
    internal class Parser
    {
        public delegate void InfoOutput(string info); // 定义信息输出, 可用于 Console 和 TextBox 等

        private InfoOutput print;
        private ISiteDefine SiteDefine;

        public Parser(ISiteDefine define, InfoOutput info = null)
        {
            if (info == null) { this.print = (x => { }); }
            else { this.print = info; }
            this.SiteDefine = define;
        }

        private HtmlDocument GetDocument(Uri uri, int timeout = 3000, int retry = 5)
        {
            print("fetching: " + uri);
            var web = new HtmlWeb();
            web.PreRequest = delegate (HttpWebRequest webRequest)
            {
                webRequest.Timeout = timeout;
                return true;
            };
            HtmlDocument doc = null;
            for (int i = 0; i <= retry && doc == null; ++i)
            {
                if (i != 0) print("Retry " + i + " : " + uri);
                try { doc = web.Load(uri.AbsoluteUri); } catch { }
            }
            return doc;
        }

        public string FetchAndGenerate(string uri)
        {
            var headUri = new Uri(uri);
            var defines = SiteDefine.Define[headUri.Host];
            var listUri = new Uri(Regex.Replace(headUri.AbsoluteUri, defines.HeadPage.Redirect.Pattern, defines.HeadPage.Redirect.Replacement));

            var sb = new StringBuilder();
            HtmlNode doc = null;

            switch (defines.HeadPage.SynopLoc)
            {
                case SiteInfo._HeadPage.S_Loc.Cover:
                    doc = GetDocument(headUri).DocumentNode;
                    break;

                case SiteInfo._HeadPage.S_Loc.Catalog:
                    doc = GetDocument(listUri).DocumentNode;
                    break;

                default:
                    break;
            }

            try { sb.Append(@"<p>" + doc.SelectSingleNode(defines.HeadPage.Title).OuterHtml.MultiTrim()); } catch { }
            try { sb.Append(@" (" + doc.SelectSingleNode(defines.HeadPage.Author).OuterHtml.MultiTrim() + @")"); } catch { }
            try { sb.Append(@" (" + doc.SelectSingleNode(defines.HeadPage.Genre).OuterHtml.MultiTrim()); } catch { }
            try { sb.AppendLine(@" / " + doc.SelectSingleNode(defines.HeadPage.SubGenre).OuterHtml.MultiTrim() + @")</p>"); } catch { }
            try { sb.AppendLine(@"<p>" + doc.SelectSingleNode(defines.HeadPage.Synopsis).OuterHtml.MultiTrim() + @"</p>"); } catch { }

            if (defines.HeadPage.SynopLoc == SiteInfo._HeadPage.S_Loc.Cover)
            { doc = GetDocument(listUri).DocumentNode; } // Now doc points `list`

            var index = new List<KeyValuePair<string, string>>();

            switch (defines.Volume.Type)
            {
                case SiteInfo._Volume._Type.Fold:
                    {
                        var volumes = doc.SelectNodes(defines.Volume.Handle);
                        foreach (var v in volumes)
                        {
                            var vtitle = v.SelectSingleNode(defines.Volume.Name).InnerText.MultiTrim();
                            var chapters = v.SelectNodes(defines.Chapter.Handle);
                            foreach (var c in chapters)
                            {
                                var ctitle = c.SelectSingleNode(defines.Chapter.Name).InnerText.MultiTrim();
                                var clink = c.SelectSingleNode(defines.Chapter.Link).Attributes["href"].Value;
                                var name = vtitle + " " + ctitle;
                                sb.AppendLine(@"<p>" + name + @"</p>");
                                index.Add(name, clink);
                            }
                        }
                        break;
                    }
                case SiteInfo._Volume._Type.Plat:
                    {
                        var chapters = doc.SelectNodes(defines.Volume.Handle + " | " + defines.Chapter.Handle);
                        string vtitle = "";
                        foreach (var c in chapters)
                        {
                            try { vtitle = c.SelectSingleNode(defines.Volume.Name).InnerText.MultiTrim(); } catch { }
                            var ctitle = c.SelectSingleNode(defines.Chapter.Name).InnerText.MultiTrim();
                            var clink = c.SelectSingleNode(defines.Chapter.Link).Attributes["href"].Value;
                            var name = vtitle + " " + ctitle;
                            sb.AppendLine(@"<p>" + name + @"</p>");
                            index.Add((vtitle + " " + ctitle).Trim(), clink);
                        }
                        break;
                    }
                default:
                    break;
            }

            //////////////////////////////////////////////////////////
            foreach (var idx in index)
            {
                var link = idx.Value;
                if (string.IsNullOrWhiteSpace(link)) continue;
                if (Regex.IsMatch(link, @"^/.*$")) { link = listUri.RootPath() + link; }
                else if (Regex.IsMatch(link, @".*://.*")) { }
                else { link = listUri.ParentPath() + link; }

                var textbox = GetDocument(new Uri(link)).DocumentNode.SelectSingleNode(defines.TextPage.Handle);

                sb.AppendLine(textbox.SelectSingleNode(defines.TextPage.Name).OuterHtml.MultiTrim());
                foreach (var p in textbox.SelectNodes(defines.TextPage.Body))
                { sb.AppendLine(p.OuterHtml.MultiTrim()); }
            }

            return sb.ToString();
        }
    }
}