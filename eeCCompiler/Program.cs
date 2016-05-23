using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using eeCCompiler.Nodes;
using eeCCompiler.Visitors;

namespace eeCCompiler
{
    internal class Program
    {

        private static void Main(string[] args)
        {
            string path = "", filename;
            var errors = new List<string>();

            // Checking argument
            var fileChecker = new FileChecker();
            fileChecker.GetArguments(args, ref path, out filename);

            // Parsing input
            var parser = new Parser(errors);
            var parsed = parser.Parse(path + filename + ".eec");

            if (parsed)
            {
                // Sorting structs declarations
                var structSorter = new StructSorter();
                structSorter.SortStructs(parser, errors);

                // Fixing precedence
                parser.Root.Accept(new Precedence());

                // Type checking
                parser.Root.Accept(new Typechecker(errors));

                if (errors.Any())
                {
                    // Printing errors
                    errors.ForEach(Console.WriteLine);
                    Console.ReadKey();
                }
                else
                {
                    if (args.Length > 1 && args[1] == "-mips")
                    {
                        // Compiling to MIPS
                        var mips = new MipsPrinter();
                        parser.Root.Accept(mips);
                        File.WriteAllText(filename + ".S", mips.File);
                    }
                    else
                    {
                        // Compiling to C
                        var cCompiler = new CCompiler();
                        cCompiler.CompileToC(parser, filename);
                        //cCompiler.CompileC(path + filename);
                        //cCompiler.Run(path + filename);
                    }
                }
            }
            else
            {
                // Parse unsuccesfully - Printing errors
                errors.ForEach(Console.WriteLine);
                Console.ReadKey();
            }
        }
    }
}