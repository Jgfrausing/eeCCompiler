using System;
using System.Collections.Generic;
using eeCCompiler.Interfaces;

namespace eeCCompiler.Nodes
{
    public class StructDefinitions : AbstractSyntaxTree
    {
        public StructDefinitions()
        {
            Definitions = new List<StructDefinition>();
        }

        public StructDefinitions(List<StructDefinition> structDefinitions)
        {
            Definitions = structDefinitions;
        }

        public List<StructDefinition> Definitions { get; set; }

        public override void Accept(IEecVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}