using System;
using System.Collections.Generic;
using System.Linq;
using eeCCompiler.Visitors;

namespace eeCCompiler
{
    internal class Program
    {
        private static readonly List<string> Errors = new List<string>();

        private static void Main(string[] args)
        {
            string path = "", filename;

            // Checking argument
            var fileChecker = new FileChecker();
            fileChecker.GetArguments(args, ref path, out filename);

            // Parsing input
            var parser = new Parser(Errors);
            var parsed = parser.Parse(path + filename + ".eec");

            if (parsed)
            {
                // Sorting structs declarations
                var structSorter = new StructSorter();
                structSorter.SortStructs(parser, Errors);

                // Fixing precedence
                parser.Root.Accept(new Precedence());

                // Type checking
                parser.Root.Accept(new Typechecker(Errors));

                if (Errors.Any())
                {
                    // Printing errors
                    Errors.ForEach(Console.WriteLine);
                    Console.ReadKey();
                }
                else
                {
                    // Compiling to C
                    var cCompiler = new CCompiler();
                    cCompiler.CompileToC(parser, filename);
                    cCompiler.CompileC(path + filename);
                    cCompiler.Run(path + filename);
                }
            }
            else
            {
                // Parse unsuccesfully - Printing errors
                Errors.ForEach(Console.WriteLine);
                Console.ReadKey();
            }
            Console.Clear();
        }
    }
}