using System.Collections.Generic;
using eeCCompiler.Interfaces;

namespace eeCCompiler.Nodes
{
    public class Include : AbstractSyntaxTree
    {
        public Include(Identifier identifier)
        {
            Identifiers = new List<Identifier> {identifier};
        }

        public Include(List<Identifier> identifiers)
        {
            Identifiers = identifiers;
        }

        public List<Identifier> Identifiers { get; set; }

        public override void Accept(IEecVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}