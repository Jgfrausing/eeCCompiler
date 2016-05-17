using System;
using System.Diagnostics;
using System.IO;
using eeCCompiler.Visitors;

namespace eeCCompiler
{
    public class CCompiler
    {
        public void CompileToC(Parser parser, string filename)
        {
            var cCodeVisitor = new CCodeGeneration();
            cCodeVisitor.Visit(parser.Root);

            var sr = new StreamWriter(filename + ".c");
            sr.Write(cCodeVisitor.CCode);
            sr.Close();
        }

        public void CompileC(string pathFile)
        {
            var compilerPath = @"..\..\C_code\C compiler\bin\gcc";
            string compileArguments = $" {pathFile}.c -o {pathFile}.exe";
            // Example of arguments
            var p = new Process {StartInfo = new ProcessStartInfo(compilerPath, compileArguments)};
            p.Start();
        }

        public void Run(string pathFile)
        {
            Console.WriteLine("Run compiled code? y/n");
            var answer = 0;
            while (true)
            {
                var input = Console.ReadKey();
                if (input.Key == ConsoleKey.Y)
                {
                    answer = 1;
                    break;
                }
                if (input.Key == ConsoleKey.N)
                    break;
                Console.Write("\b \b");
            }

            if (answer == 1)
            {
                string runArguments = $"/C start cmd /k {pathFile}.exe";
                Process.Start("CMD.exe", runArguments);
            }
        }
    }
}