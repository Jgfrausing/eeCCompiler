using eeCCompiler.Interfaces;
using eeCCompiler.Visitors;

namespace eeCCompiler.Nodes
{
    public class IdIndex : AbstractSyntaxTree, IStructRefrence, IIdentifier, IExpression, IValue
    {
        public IdIndex(ListIndex listIndex, Identifier identifier)
        {
            ListIndex = listIndex;
            Identifier = identifier;
        }
        public Identifier Identifier { get; set; }
        public ListIndex ListIndex { get; set; }

        public override void Accept(IEecVisitor visitor)
        {
            visitor.Visit(this);
        }

        public Type Type { get; set; }
    }
}