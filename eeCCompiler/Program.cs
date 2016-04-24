using System;
using System.Collections.Generic;
using System.IO;
using eeCCompiler.Interfaces;
using eeCCompiler.Visitors;
using eeCCompiler.Visitors.CCode;

namespace eeCCompiler
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            //var testingList = new DefaultCCode();
            //var s = testingList.GetIncludes();
            //s += testingList.GenerateListTypeHeader("int");
            ////s += testingList.GenerateListTypeHeader("myTest");
            //s += testingList.GenerateListTypeCode("int");
            ////s += testingList.GenerateListTypeCode("myTest");
            //s += testingList.IncludeMain(@"..\..\C_code\MainForTestingIntList.c");
            //var test = new StreamWriter("testinglistcode.c");
            //test.Write(s);
            //test.Close();
            //Environment.Exit(1);
            var parser = new MyParser(); //Derp
            var result = parser.Parse(new StreamReader("HelloWorld.eec"));
            var syntax = result ? "The syntax is correct!" : "There are errors in the syntax";
            Console.WriteLine(syntax);
            var errors = new List<string>();
            var identifiers = new Dictionary<string, IValue>();
            //parser.Root.Accept(new PrettyPrinter());
            //Console.WriteLine("::::::::::::::::::");
            //parser.Root.Accept(new Treeprint());
            //Console.WriteLine("::::::::::::::::::");
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