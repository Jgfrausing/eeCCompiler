using System;
using System.IO;
using eeCCompiler.Visitors;
using System.Collections.Generic;
using eeCCompiler.Interfaces;

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
            Dictionary<string, IValue> Identifiers = new Dictionary<string,IValue>();
            parser.Root.Accept(new Typechecker(errors, Identifiers));
            errors.ForEach(x => Console.WriteLine(x));
            Console.ReadKey();
            
        }
    }
}