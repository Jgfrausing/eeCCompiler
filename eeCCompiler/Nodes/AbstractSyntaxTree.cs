using eeCCompiler.Interfaces;
using eeCCompiler.Visitors;

namespace eeCCompiler.Nodes
{
    public abstract class AbstractSyntaxTree : INodeElement
    {
        public int Line { get; set; }
        public int Column { get; set; }
        //public string Tag { get; set; }
        public abstract void Accept(IEecVisitor visitor);
    }
}