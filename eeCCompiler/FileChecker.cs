using System;
using System.IO;
using System.Linq;

namespace eeCCompiler
{
    public class FileChecker
    {
        public void GetArguments(string[] args, ref string path, out string filename)
        {
            //FileStream f = new;
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

            path = Path.GetDirectoryName(argument) + "\\";
            filename = Path.GetFileName(argument).Split('.')[0];
        }
    }
}