using System;
using System.Text;

namespace ncode
{
    internal class Output : IOutput
    {
        public void Write(string s)
        { Console.Write(s); }

        public void WriteLine(string s)
        { Console.WriteLine(s); }
    }

    internal class Program
    {
        private static void Main(string[] args)
        {
            if (args.Length != 2)
            {
                new Output().WriteLine("ncode <url> <outhtml>");
                return;
            }

            var ip = new IndexParse(new Output());
            var ac = ip.GetAllContent(args[0]);

            ac = "<html><body>" + ac + "</body></html>";

            var sw = new System.IO.StreamWriter(args[1], false, Encoding.UTF8);
            sw.Write(ac);
            sw.Close();
        }
    }
}