using System.Security.AccessControl;
using eeCCompiler.Interfaces;
using eeCCompiler.Visitors;

namespace eeCCompiler.Nodes
{
    public abstract class AbstractSyntaxTree : INodeElement
    {
        //public string Tag { get; set; }
        public abstract void Accept(IEecVisitor visitor);
        public int Line { get; set; }
        public int Column { get; set; }
    }
}