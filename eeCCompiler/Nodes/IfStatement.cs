﻿using eeCCompiler.Interfaces;

namespace eeCCompiler.Nodes
{
    internal class IfStatement : ElseStatement, IBodypart
    {
        public IfStatement(IExpression expression, Body body, ElseStatement elseStatement) : base(body)
        {
            Expression = expression;
            ElseStatement = elseStatement;
        }

        public IExpression Expression { get; set; }
        public ElseStatement ElseStatement { get; set; }
    }
}