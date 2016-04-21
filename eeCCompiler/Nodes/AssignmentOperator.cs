using eeCCompiler.Visitors;

namespace eeCCompiler.Nodes
{
    public class AssignmentOperator : Operator
    {
        public AssignmentOperator(Indexes.Indexes.SymbolIndex symbol) : base(symbol)
        {
        }
        public override void Accept(IEecVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}