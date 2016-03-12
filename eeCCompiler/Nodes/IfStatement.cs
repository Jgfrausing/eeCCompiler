using System.Collections.Generic;
using eeCCompiler.Interfaces;

namespace eeCCompiler.Nodes
{
    class IfStatement : AbstractSyntaxTree, IBodypart
    {
        public IfStatement(IExpression expression, Body body, ElseStatement elseStatement)
        {
            IExpression = expression;
            Body = body;
            ElseStatement = elseStatement;
        }
        public IExpression IExpression { get; set; }
        public Body Body { get; set; }
        public ElseStatement ElseStatement { get; set; }
    }
}