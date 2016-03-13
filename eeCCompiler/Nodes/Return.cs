using eeCCompiler.Interfaces;

namespace eeCCompiler.Nodes
{
    public class Return : AbstractSyntaxTree
    {
        public Return(IExpression expression)
        {
            Expression = expression;
        }

        public IExpression Expression { get; set; }
    }
}