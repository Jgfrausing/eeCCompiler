using System;
using System.IO;

namespace eeCCompiler
{
    class Project
    {
        static void Main(string[] args)
        {
            var parser = new MyParser();
            parser.Parse(new StreamReader("HelloWorld.eec"));

            Console.ReadKey();
        }
    }
}