using eeCCompiler.Interfaces;
using Expression = System.Linq.Expressions.Expression;

namespace eeCCompiler.Nodes
{
    class IfStatement : AbstractSyntaxTree, IBodypart
    {
        public IfStatement(Expression expression, Body body)
        {
            Expression = expression;
            Body = body;
        }
        public Expression Expression { get; set; }
        public Body Body { get; set; }
    }
}