using System;
using System.IO;
using System.Runtime.InteropServices;

namespace eeCCompiler
{
    class Project
    {
        static void Main(string[] args)
        {
            var parser = new MyParser();
            var result = parser.Parse(new StreamReader("HelloWorld.eec"));
            string syntax = result ?  "The syntax is correct!" : "There are errors in the syntax";
            Console.WriteLine(syntax);
            Console.ReadKey();
        }
    }
}