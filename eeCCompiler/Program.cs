using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.ExceptionServices;
using System.Text.RegularExpressions;
using System.Xml.Schema;
using eeCCompiler.Interfaces;
using eeCCompiler.Nodes;
using eeCCompiler.Visitors;
using eeCCompiler.Visitors.CCode;

namespace eeCCompiler
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var parser = new MyParser();
            var result = parser.Parse(new StreamReader("HelloWorld.eec"));
            var syntax = result ? "The syntax is correct!" : "There are errors in the syntax";
            Console.WriteLine(syntax);
            var errors = new List<string>();
            //var identifiers = new Dictionary<string, IValue>();
            ////parser.Root.Accept(new PrettyPrinter());
            ////Console.WriteLine("::::::::::::::::::");
            ////parser.Root.Accept(new Treeprint());
            ////Console.WriteLine("::::::::::::::::::");



            parser.Root.Accept(new PrecedenceVisitor());
            parser.Root.Accept(new Typechecker(errors));
            errors.ForEach(x => Console.WriteLine(x));
            var cCodeVisitor = new CCodeGeneration();
            cCodeVisitor.Visit(parser.Root);

            var sr = new StreamWriter("code.c");
            sr.Write(cCodeVisitor.CCode);
            sr.Close();
            //Console.Write(cCodeVisitor.CCode);
            Console.ReadKey();
        }
    }
}