using System.Collections.Generic;
using eeCCompiler.Interfaces;

namespace eeCCompiler.Nodes
{
    public class Includes : AbstractSyntaxTree
    {
        public Includes()
        {
            IncludeList = new List<Include>();
        }

        public Includes(List<Include> includeList)
        {
            IncludeList = includeList;
        }

        public List<Include> IncludeList { get; set; }

        public override void Accept(IEecVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}