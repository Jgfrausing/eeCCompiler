using System.Collections.Generic;
using eeCCompiler.Interfaces;
using eeCCompiler.Visitors;

namespace eeCCompiler.Nodes
{
    public class Body : AbstractSyntaxTree
    {
        public Body()
        {
            Bodyparts = new List<IBodypart>();
        }

        public Body(List<IBodypart> bodyparts)
        {
            Bodyparts = bodyparts;
        }

        public List<IBodypart> Bodyparts { get; set; }

        public override void Accept(IEecVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}