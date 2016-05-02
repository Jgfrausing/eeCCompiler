using eeCCompiler.Interfaces;
using eeCCompiler.Visitors;

namespace eeCCompiler.Nodes
{
    public class ListType : AbstractSyntaxTree, IType, IExpression
    {
        public IType Type { get; set; }
        public int Dimentions { get; set; }

        public ListType(ListDimentions dimentions, IType type)
        {
            Type = type;
            Dimentions = dimentions.Dimentions;
        }
        public override void Accept(IEecVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override string ToString()
        {
            return Type.ToString();
        }

        Type IExpression.Type { get; set; }
    }
}