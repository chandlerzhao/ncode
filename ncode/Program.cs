using System;
using System.IO;
using System.Text;

namespace ncode
{
    internal class Program
    {
        //private static void Main(string[] args)
        //{
        //    if (args.Length != 2)
        //    {
        //        Console.WriteLine("ncode <url> <outhtml>");
        //        return;
        //    }

        //    var ysd = new YamlSiteDefine();
        //    ysd.Load("ncode.yaml");

        //    var ip = new Parser(ysd, x => Console.WriteLine(x));
        //    var ac = ip.FetchAndGenerate(args[0]);

        //    ac = "<html><body>" + ac + "</body></html>";

        //    var sw = new StreamWriter(args[1], false, Encoding.UTF8);
        //    sw.Write(ac);
        //    sw.Close();
        //}

        static void Main(string[] args)
        {
            var sd = new YamlSiteDefine();
            var si = new SiteInfo();

            var sif = new SiteInfo._HeadPage();
            sif.Title = @"//div[@class=""bookContainTop""]//span";
            sif.Genre = @"//div[@class=""bookContainTop""]//a[2]";
            sif.SubGenre = @"//div[@class=""bookContainTop""]//a[3]";
            sif.Author = @"//div"" "" l";
            sif.Synopsis = @"####";
            sif.SynopLoc = SiteInfo._HeadPage.S_Loc.Cover;
            sif.Redirect = new System.Collections.Generic.KeyValuePair<string, string>("book", "list");
            si.HeadPage = sif;

            var siv = new SiteInfo._Volume();
            siv.Handle = @"//div[@class=""mainList""]";
            siv.Name = @".//div[@class=""clearfix mainListTop""]//span";
            siv.Type = SiteInfo._Volume._Type.Fold;
            si.Volume = siv;

            var sic = new SiteInfo._Chapter();
            sic.Handle = @".//div[@class=""mainList_In""]//li";
            sic.Name = @".//div";
            sic.Link = @".//a";            
            si.Chapter = sic;

            var sit = new SiteInfo._TextPage();
            sit.Handle = @"//div";
            sit.Name = @"//h2";
            sit.Body = @"//div[@class=""myContent""]";            
            si.TextPage = sit;

            sd.Define.Add("8kana.com", si);

            var ss = new YamlDotNet.Serialization.Serializer();
            using (var sw = new StreamWriter(args[0], false, Encoding.UTF8))
            {
                ss.Serialize(sw, sd.Define);
                //Console.Write(sw.ToString());
            }
            ;
        }
    }
}