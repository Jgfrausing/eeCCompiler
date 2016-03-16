using eeCCompiler.Interfaces;

namespace eeCCompiler.Nodes
{
    public abstract class AbstractSyntaxTree : INodeElement
    {
        //public string Tag { get; set; }
        public abstract void Accept(IEecVisitor visitor);
    }
}