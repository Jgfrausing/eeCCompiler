using System.Collections.Generic;
using eeCCompiler.Nodes;

namespace eeCCompiler.Visitors
{
    internal class StructFunctionIdentifiers : Visitor
    {
        public StructFunctionIdentifiers(List<Identifier> identifiers)
        {
            Identifiers = identifiers;
        }

        public List<Identifier> Identifiers { get; set; }

        public override void Visit(Identifier identifier)
        {
            if (Identifiers.Contains(identifier))
            {
                identifier.Id = "this->" + identifier.Id;
            }
        }
    }

    internal class RefrenceIdentifiers : Visitor
    {
        public RefrenceIdentifiers(List<Identifier> identifiers)
        {
            Identifiers = identifiers;
        }

        public List<Identifier> Identifiers { get; set; }

        public override void Visit(Identifier identifier)
        {
            if (Identifiers.Contains(identifier))
            {
                identifier.Id = "*" + identifier.Id;
            }
        }
    }

    internal class RenamePassByValueStructIdentifiers : Visitor
    {
        public RenamePassByValueStructIdentifiers(List<Identifier> identifiers)
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