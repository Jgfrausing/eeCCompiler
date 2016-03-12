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
    class ExpressionNegate : AbstractSyntaxTree, IExpression
    {
        public ExpressionNegate(IExpression iExpression)
        {
            IExpression = iExpression;
        }
        public IExpression IExpression { get; set; }
    }
    class ExpressionMinus : AbstractSyntaxTree, IExpression
    {
        public ExpressionMinus(IExpression iExpression)
        {
            IExpression = iExpression;
        }
        public IExpression IExpression { get; set; }
    }
    class ExpressionParen : AbstractSyntaxTree, IExpression
    {
        public ExpressionParen(IExpression iExpression)
        {
            IExpression = iExpression;
        }
        public IExpression IExpression { get; set; }
    }
    class ExpressionParenOpExpr : AbstractSyntaxTree, IExpression
    {
        public ExpressionParenOpExpr(IExpression expressionParen, Operator _operator, IExpression expression)
        {
            ExpressionParen = expressionParen;
            Operator = _operator;
            IExpression = expression;
        }
        public IExpression ExpressionParen { get; set; }
        public Operator Operator { get; set; }
        public IExpression IExpression { get; set; }
    }
}