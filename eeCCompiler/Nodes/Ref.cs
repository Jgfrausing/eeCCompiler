using eeCCompiler.Interfaces;
using eeCCompiler.Visitors;

namespace eeCCompiler.Nodes
{
    public class Ref : AbstractSyntaxTree
    {
        public Ref(bool isRefrence)
        {
            IsRefrence = isRefrence;
        }
        public bool IsRefrence { get; set; }
        public override void Accept(IEecVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}