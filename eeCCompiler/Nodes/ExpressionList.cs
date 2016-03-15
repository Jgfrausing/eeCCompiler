using System.Collections.Generic;
using eeCCompiler.Interfaces;

namespace eeCCompiler.Nodes
{
    internal class ExpressionList : AbstractSyntaxTree
    {
        public ExpressionList()
        {
            Expressions = new List<IExpression>();
        }

        public ExpressionList(List<IExpression> expressions)
        {
            Expressions = expressions;
        }

        public List<IExpression> Expressions { get; set; }

        public override void Accept(IEecVisitor visitor)
        {
            foreach (var expression in Expressions)
            {
                expression.Accept(visitor);
            }
            visitor.Visit(this);
        }
    }
}