using System;
using System.IO;
using System.Reflection;
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
            var curExe = Assembly.GetExecutingAssembly().Location;
            var curDir = Path.GetDirectoryName(curExe);
            var curFn = Path.GetFileNameWithoutExtension(curExe);

            ysd.Load(Path.Combine(curDir, curFn) + ".yaml");

            var ip = new Parser(ysd, x => Console.WriteLine(x));
            var ac = ip.FetchAndGenerate(args[0]);

            ac = "<html><body>" + ac + "</body></html>";

            var sw = new StreamWriter(args[1], false, Encoding.UTF8);
            sw.Write(ac);
            sw.Close();
        }
    }
}