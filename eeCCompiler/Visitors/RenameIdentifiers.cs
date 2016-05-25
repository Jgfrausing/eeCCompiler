using System.Collections.Generic;
using eeCCompiler.Nodes;

namespace eeCCompiler.Visitors
{
    internal class RenameIdentifiers : Visitor
    {
        public RenameIdentifiers(List<Identifier> identifiers)
        {
            Identifiers = identifiers;
        }

        public List<Identifier> Identifiers { get; set; }

        public override void Visit(Identifier identifier)
        {
            if (Identifiers.Contains(identifier))
            {
                identifier.Id = "_" + identifier.Id;
            }
        }
    }
}