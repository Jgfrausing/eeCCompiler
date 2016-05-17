using System;
using System.Linq;

namespace eeCCompiler
{
    public class FileChecker
    {
        public void GetArguments(string[] args, ref string path, out string filename)
        {
            string argument;
            if (args.Length != 0)
            {
                argument = args[0];
            }
            else
            {
                Console.WriteLine("Path to .eeC file");
                argument = Console.ReadLine();
            }
            filename = argument.Split('\\').Last().Split('.').First();
            var length = argument.Split('\\').Length;
            for (var i = 0; i < length - 1; i++)
            {
                path += path.Split('\\')[i];
            }
        }
    }
}
