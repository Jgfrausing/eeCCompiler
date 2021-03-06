﻿using eeCCompiler.Interfaces;
using eeCCompiler.Visitors;

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
            visitor.Visit(this);
        }
    }
}