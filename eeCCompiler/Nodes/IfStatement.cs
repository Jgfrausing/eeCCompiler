using eeCCompiler.Interfaces;

namespace eeCCompiler.Nodes
{
    class IfStatement : AbstractSyntaxTree, IBodypart
    {
        public IfStatement(IExpression expression, Body body)
        {
            IExpression = expression;
            Body = body;
        }
        public IExpression IExpression { get; set; }
        public Body Body { get; set; }
    }
}