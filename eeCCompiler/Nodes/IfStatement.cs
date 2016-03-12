using System.Collections.Generic;
using eeCCompiler.Interfaces;

namespace eeCCompiler.Nodes
{
    class IfStatement : ElseStatement, IBodypart
    {
        public IfStatement(IExpression expression, Body body, ElseStatement elseStatement) : base(body)
        {
            IExpression = expression;
            ElseStatement = elseStatement;
        }
        public IExpression IExpression { get; set; }
        public ElseStatement ElseStatement { get; set; }
    }
}