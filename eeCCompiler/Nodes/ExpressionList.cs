using System.Collections.Generic;
using eeCCompiler.Interfaces;
using eeCCompiler.Visitors;

namespace eeCCompiler.Nodes
{
    public class ExpressionList : AbstractSyntaxTree
    {
        public ExpressionList()
        {
            Expressions = new List<IExprListElement>();
        }

        public ExpressionList(List<IExprListElement> expressions)
        {
            Expressions = expressions;
        }

        public List<IExprListElement> Expressions { get; set; }

        public override void Accept(IEecVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}