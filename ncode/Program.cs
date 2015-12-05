using System;
using System.IO;
using System.Text;

namespace ncode
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            if (args.Length != 2)
            {
                Console.WriteLine("ncode <url> <outhtml>");
                return;
            }

            var ip = new IndexParse(x => Console.WriteLine(x));
            var ac = ip.GetAllContent(args[0]);

            ac = "<html><body>" + ac + "</body></html>";

            var sw = new StreamWriter(args[1], false, Encoding.UTF8);
            sw.Write(ac);
            sw.Close();
        }

        //static void Main(string[] args)
        //{
        //    var sd = new YamlSiteDefine();
        //    sd.Load(args[0]);
        //    //sd.Save(args[0]);
        //}
    }
}