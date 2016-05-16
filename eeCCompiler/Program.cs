using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using eeCCompiler.Visitors;

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
            parser.Root.Accept(new Precedence());
            var errors = new List<string>();
            parser.Root.Accept(new Typechecker(errors));
            errors.ForEach(x => Console.WriteLine(x));
            var Mipper = new MipsPrinter();
            parser.Root.Accept(Mipper);
            System.IO.File.WriteAllText(@"C:\Users\mathi\Desktop\Mipser.S", Mipper.Data+Mipper.Text);


            //var errors = new List<string>();
            //if (result)
            //{
            //    try
            //    {
            //        var derp = new CCodeGeneration();
            //        derp.SortStructDefinitions(parser.Root.StructDefinitions);
            //    }
            //    catch (ArgumentException e)
            //    {
            //        Console.WriteLine(e.Message);
            //        Console.ReadKey();
            //        Environment.Exit(1);
            //    }
            //    parser.Root.Accept(new Precedence());
            //    parser.Root.Accept(new Typechecker(errors));
            //    errors.ForEach(x => Console.WriteLine(x));
            //    //  if (errors.Count == 0)
            //    {
            //        var cCodeVisitor = new CCodeGeneration();
            //        cCodeVisitor.Visit(parser.Root);

            //        var sr = new StreamWriter("code.c");
            //        sr.Write(cCodeVisitor.CCode);
            //        sr.Close();
            //    }
            //}
            //Console.WriteLine("Compile c code? (y/n)");
            //var answer = 0;
            //while (true)
            //{
            //    var input = Console.ReadKey();
            //    if (input.Key == ConsoleKey.Y)
            //    {
            //        answer = 1;
            //        break;
            //    }
            //    if (input.Key == ConsoleKey.N)
            //        break;
            //    Console.Write("\b \b");
            //}
            //if (answer == 1)
            //{
            //    var codeName = "code";
            //    var codePath = @"..\..\bin\Debug\";
            //    var compilerPath = @"..\..\C_code\C compiler\bin\gcc";
            //    string compileArguments = $" {codePath}{codeName}.c -o {codePath}{codeName}.exe";
            //        // Example of arguments
            //    string runArguments = $"/C start cmd /k {codePath}{codeName}.exe";
            //    var p = new Process {StartInfo = new ProcessStartInfo(compilerPath, compileArguments)};
            //    p.Start();
            //    while (!p.HasExited)
            //    {
            //    }
            //    Process.Start("CMD.exe", runArguments);
            //}
        }
    }
}