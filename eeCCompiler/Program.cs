using System;
using System.IO;
using eeCCompiler.Visitors;
using System.Collections.Generic;

namespace eeCCompiler
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var parser = new MyParser(); //Derp
            var result = parser.Parse(new StreamReader("HelloWorld.eec"));
            var syntax = result ? "The syntax is correct!" : "There are errors in the syntax";
            Console.WriteLine(syntax);
            List<string> errors = new List<string>();
            parser.Root.Accept(new Typechecker(errors));
            errors.ForEach(x => Console.WriteLine(x));
            Console.ReadKey();
            
        }
    }
}