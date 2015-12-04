using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace ncode
{
    internal abstract class Parser
    {
        protected IOutput _info;

        protected Parser(IOutput info)
        {
            this._info = info;
        }

        protected HtmlDocument GetDocument(string uri, int timeout = 3000, int retry = 5)
        {
            return GetDocument(new Uri(uri), timeout, retry);
        }

        protected HtmlDocument GetDocument(Uri uri, int timeout = 3000, int retry = 5)
        {
            _info.WriteLine("fetching: " + uri);
            var web = new HtmlWeb();
            web.PreRequest = delegate (HttpWebRequest webRequest)
            {
                webRequest.Timeout = timeout;
                return true;
            };
            HtmlDocument doc = null;
            for (int i = 0; i <= retry && doc == null; ++i)
            {
                if (i != 0) _info.WriteLine("Retry " + i + " : " + uri);
                try { doc = web.Load(uri.AbsoluteUri); } catch { }
            }
            return doc;
        }

        protected HtmlNode GetCore(HtmlDocument doc, int style = 0)
        {
            switch (style)
            {
                case 0:
                    return doc.DocumentNode.SelectSingleNode(@"/html/body//div[@class=""informList""]");

                case 1:
                    return doc.DocumentNode.SelectSingleNode(@"//div[@class=""readcon""]");

                default:
                    return null;
            }
        }
    }

    internal class IndexParse : Parser
    {
        public IndexParse(IOutput _info) : base(_info)
        {
        }

        public string GetAllContent(string uri)
        {
            var u = new Uri(uri);
            var doc = GetDocument(u);
            var content = GetCore(doc);

            var sb = new StringBuilder();
            sb.Append(@"<p>" + doc.DocumentNode.SelectSingleNode(@"//div[@class=""bookContainTop""]//span").InnerText._Clean());
            sb.Append(@" (" + doc.DocumentNode.SelectSingleNode(@"//div[@class=""bookContainTop""]//a[2]").InnerText._Clean());
            sb.AppendLine(@" / " + doc.DocumentNode.SelectSingleNode(@"//div[@class=""bookContainTop""]//a[3]").InnerText._Clean() + @")</p>");

            var index = new List<KeyValuePair<string, List<KeyValuePair<string, string>>>>();
            var volumes = content.SelectNodes(@"./div[@class=""mainList""]");
            foreach (var v in volumes)
            {
                var vidx = new List<KeyValuePair<string,string>>();
                var vtitle = v.SelectSingleNode(@"./div[@class=""clearfix mainListTop""]").InnerText._Clean();
                sb.AppendLine(@"<p>" + vtitle + @"</p>");
                var chapters = v.SelectNodes(@"./div[@class=""mainList_In""]//a");
                foreach (var c in chapters)
                {
                    var src = u.Scheme + "://" + u.Host + c.Attributes["href"].Value;
                    var ctitle = c.InnerText._Clean();
                    sb.AppendLine(@"<p>" + ctitle + @"</p>");
                    vidx.Add(ctitle, src);
                }
                index.Add(vtitle, vidx);
            }

            foreach (var v in index)
            {
                foreach (var c in v.Value)
                {
                    sb.AppendLine(new PageParse(_info).GetContent(c.Value, v.Key));
                }
            }

            return sb.ToString();
        }
    }

    internal class PageParse : Parser
    {
        public PageParse(IOutput _info) : base(_info)
        {
        }

        public string GetContent(string uri, string volume)
        {
            var content = GetCore(GetDocument(uri), 1);
            ;
            var sb = new StringBuilder();

            sb.AppendLine(@"<p id=""0"">");
            sb.Append(volume + " ");
            sb.AppendLine(content.SelectSingleNode(@"./h2").InnerText._Clean());
            sb.AppendLine(@"</p>");

            sb.AppendLine(content.SelectSingleNode(@"./div[@class=""myContent""]").InnerHtml);

            return sb.ToString();
        }
    }
}