using eeCCompiler.Visitors;

namespace eeCCompiler.Nodes
{
    public class ListDimentions : AbstractSyntaxTree
    {
        public ListDimentions()
        {
            Dimentions = 1;
        }

        public ListDimentions(ListDimentions listDimentions)
        {
            Dimentions = listDimentions.Dimentions + 1;
        }

        public int Dimentions { get; set; }

        public override void Accept(IEecVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}