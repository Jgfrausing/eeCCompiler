using eeCCompiler.Interfaces;
using eeCCompiler.Visitors;

namespace eeCCompiler.Nodes
{
    public class RefId : AbstractSyntaxTree, IIdentifier, IExprListElement
    {
        public RefId(Identifier identifier)
        {
            Identifier = identifier;
            Type = new Type("Not set in typechecker");
        }

        public Identifier Identifier { get; set; }
        public Type Type { get; set; }

        public override void Accept(IEecVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}