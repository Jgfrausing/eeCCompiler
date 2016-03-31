using System;
using System.Collections.Generic;
using System.IO;
using eeCCompiler.Interfaces;
using eeCCompiler.Visitors;

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
            var errors = new List<string>();
            var Identifiers = new Dictionary<string, IValue>();
            parser.Root.Accept(new PrettyPrinter());
            Console.WriteLine("::::::::::::::::::");
            parser.Root.Accept(new Treeprint());
            Console.WriteLine("::::::::::::::::::");
            parser.Root.Accept(new Typechecker(errors, Identifiers));
            errors.ForEach(x => Console.WriteLine(x));
            Console.ReadKey();
        }
    }
}