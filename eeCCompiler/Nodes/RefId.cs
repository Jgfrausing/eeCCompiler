using eeCCompiler.Interfaces;
using eeCCompiler.Visitors;

namespace eeCCompiler.Nodes
{
    public class RefId : AbstractSyntaxTree, IIdentifier, IExprListElement
    {
        public RefId(Identifier identifier)
        {
            Identifier = identifier;
        }
        public Identifier Identifier { get; set; }
        public override void Accept(IEecVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}