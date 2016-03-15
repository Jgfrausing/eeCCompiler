using System;
using eeCCompiler.Interfaces;

namespace eeCCompiler.Nodes
{
    public class Return : AbstractSyntaxTree, IBodypart
    {
        public Return(IExpression expression)
        {
            Expression = expression;
        }

        public IExpression Expression { get; set; }

        public override void Accept(IEecVisitor visitor)
        {
            Expression.Accept(visitor);
            visitor.Visit(this);
        }
    }
}