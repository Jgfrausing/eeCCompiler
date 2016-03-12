using eeCCompiler.Interfaces;

namespace eeCCompiler.Nodes
{
    class ExpressionVal : AbstractSyntaxTree, IExpression
    {
        public ExpressionVal(IValue value)
        {
            Value = value;
        }
        public IValue Value { get; set; }
    }
    class ExpressionValOpExpr : AbstractSyntaxTree, IExpression
    {
        public ExpressionValOpExpr(IValue value, Operator _operator, IExpression expression)
        {
            Value = value;
            Operator = _operator;
            IExpression = expression;
        }
        public IValue Value { get; set; }
        public Operator Operator { get; set; }
        public IExpression IExpression { get; set; }
    }
}