using eeCCompiler.Interfaces;
using eeCCompiler.Visitors;

namespace eeCCompiler.Nodes
{
    public class ListType : AbstractSyntaxTree, IType, IExpression
    {
        public ListType(ListDimentions dimentions, IType type)
        {
            Type = type;
            Dimentions = dimentions.Dimentions;
        }

        public IType Type { get; set; }
        public int Dimentions { get; set; }

        public override void Accept(IEecVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override string ToString()
        {
            return Type.ToString();
        }
    }
}