namespace eeCCompiler.Nodes
{
    public abstract class Expression : AST
    {

    }
    class ExpressionVal : Expression
    {
        public ExpressionVal(Value value)
        {
            Value = value;
        }
        public Value Value { get; set; }
        public override string PrettyPrint()
        {
            throw new System.NotImplementedException();
        }
    }
    class ExpressionValOpExpr : Expression
    {
        public ExpressionValOpExpr(Value value, Operator _operator, Expression expression)
        {
            Value = value;
            Operator = _operator;
            Expression = expression;
        }
        public Value Value { get; set; }
        public Operator Operator { get; set; }
        public Expression Expression { get; set; }

        public override string PrettyPrint()
        {
            throw new System.NotImplementedException();
        }
    }
}