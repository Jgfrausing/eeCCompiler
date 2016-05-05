using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using System.Text.RegularExpressions;
using System.Threading;
using System.Timers;
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
            if (result)
            {
                try
                {
                    var derp = new CCodeGeneration();
                    derp.SortStructDefinitions(parser.Root.StructDefinitions);
                }
                catch (ArgumentException e)
                {
                    Console.WriteLine(e.Message);
                    Console.ReadKey();
                    Environment.Exit(1);
                }
                parser.Root.Accept(new Precedence());
                parser.Root.Accept(new Typechecker(errors));
                errors.ForEach(x => Console.WriteLine(x));
                //  if (errors.Count == 0)
                {
                    var cCodeVisitor = new CCodeGeneration();
                    cCodeVisitor.Visit(parser.Root);

                    var sr = new StreamWriter("code.c");
                    sr.Write(cCodeVisitor.CCode);
                    sr.Close();
                }
            }
            Console.WriteLine("Compile c code? (y/n)");
            int answer = 0;
            while (true)
            {
                var input = Console.ReadKey();
                if (input.Key == ConsoleKey.Y)
                {
                    answer = 1;
                    break;
                }
                else if (input.Key == ConsoleKey.N)
                    break;
                else
                {
                    Console.Write("\b \b");
                }
            }
            if (answer == 1)
            {
                string codeName = "code";
                string codePath = @"..\..\bin\Debug\";
                string compilerPath = @"..\..\C_code\C compiler\bin\gcc";
                string compileArguments = $" {codePath}{codeName}.c -o {codePath}{codeName}.exe"; // Example of arguments
                string runArguments = $"/C start cmd /k {codePath}{codeName}.exe";
                Process p = new Process() {StartInfo = new ProcessStartInfo(compilerPath, compileArguments)};
                p.Start();
                while (!p.HasExited)
                {
                }
                Process.Start("CMD.exe", runArguments);
            }
        }
    }
}