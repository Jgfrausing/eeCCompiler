using System;
using System.Collections.Generic;
using eeCCompiler.Nodes;

namespace eeCCompiler.Visitors
{
    class StructFunctionIdentifiers : Visitor
    {
        public List<Identifier> Identifiers { get; set; }
        public StructFunctionIdentifiers(List<Identifier> identifiers)
        {
            Identifiers = identifiers;
        }

        public override void Visit(Identifier identifier)
        {
            if (Identifiers.Contains(identifier))
            {
                identifier.Id = "this->" + identifier.Id;
            }
        }
    }
}