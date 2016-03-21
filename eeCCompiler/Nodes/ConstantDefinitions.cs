using System.Collections.Generic;
using eeCCompiler.Interfaces;

namespace eeCCompiler.Nodes
{
    public class ConstantDefinitions : AbstractSyntaxTree
    {
        public ConstantDefinitions()
        {
            ConstantList = new List<Constant>();
        }

        public ConstantDefinitions(List<Constant> constantDefinitions)
        {
            ConstantList = constantDefinitions;
        }

        public List<Constant> ConstantList { get; set; }

        public override void Accept(IEecVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}