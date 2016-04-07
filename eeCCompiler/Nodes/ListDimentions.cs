using eeCCompiler.Interfaces;

namespace eeCCompiler.Nodes
{
    public class ListDimentions : AbstractSyntaxTree
    {
        public ListDimentions()
        {
            Dimentions = 1;
        }
        public int Dimentions { get; set; }
        public override void Accept(IEecVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}