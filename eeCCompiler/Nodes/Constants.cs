using System.Collections.Generic;

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
            foreach (var cont in ConstantList)
            {
                cont.Accept(visitor);
            }
            visitor.Visit(this);
        }
    }
}