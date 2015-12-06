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

            var ysd = new YamlSiteDefine();
            ysd.Load("ncode.yaml");

            var ip = new Parser(ysd, x => Console.WriteLine(x));
            var ac = ip.FetchAndGenerate(args[0]);

            ac = "<html><body>" + ac + "</body></html>";

            var sw = new StreamWriter(args[1], false, Encoding.UTF8);
            sw.Write(ac);
            sw.Close();
        }        
    }
}