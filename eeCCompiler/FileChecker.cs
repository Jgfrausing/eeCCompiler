using System;
using System.IO;

namespace eeCCompiler
{
    public class FileChecker
    {
        public void GetArguments(string[] args, out string path, out string filename, out string extension)
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
            path = path.Trim(' ') == "\\" ? "" : path;
            filename = Path.GetFileName(argument).Split('.')[0];
            extension = Path.GetExtension(argument);
        }
    }
}