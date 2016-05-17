using System;
using System.Collections.Generic;
using eeCCompiler.Nodes;

namespace eeCCompiler
{
    internal class StructSorter
    {
        public void SortStructs(Parser parser, List<string> errors)
        {
            try
            {
                var sorter = new StructDefinitionSorter();
                sorter.Sort(parser.Root.StructDefinitions.Definitions);
            }
            catch (ArgumentException e)
            {
                errors.Add(e.Message);
            }
        }
    }
}