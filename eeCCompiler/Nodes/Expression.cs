using eeCCompiler.Interfaces;
using eeCCompiler.Visitors;

namespace eeCCompiler.Nodes
{
    public class ExpressionVal : AbstractSyntaxTree, IExpression
    {
        public ExpressionVal(IValue value)
        {
            Value = value;
        }

        public IValue Value { get; set; }

        public override void Accept(IEecVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }

    public class ExpressionValOpExpr : AbstractSyntaxTree, IExpression
    {
        public ExpressionValOpExpr(IExpression expression, Operator _operator, IValue value)
        {
            Value = value;
            Operator = _operator;
            Expression = expression;
        }

        public IValue Value { get; set; }
        public Operator Operator { get; set; }
        public IExpression Expression { get; set; }

        public override void Accept(IEecVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override string ToString()
        {
            return $"{Value} {Operator} {Expression}";
        }
    }

    public class ExpressionNegate : AbstractSyntaxTree, IExpression
    {
        public ExpressionNegate(IExpression expression)
        {
            Expression = expression;
        }

        public IExpression Expression { get; set; }

        public override void Accept(IEecVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override string ToString()
        {
            return $"{Expression}";
        }
    }

    public class ExpressionMinus : AbstractSyntaxTree, IExpression
    {
        public ExpressionMinus(IExpression expression)
        {
            Expression = expression;
        }

        public IExpression Expression { get; set; }

        public override void Accept(IEecVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override string ToString()
        {
            return $"{Expression}";
        }
    }

    public class ExpressionParen : AbstractSyntaxTree, IExpression
    {
        public ExpressionParen(IExpression expression)
        {
            Expression = expression;
        }

        public IExpression Expression { get; set; }

        public override void Accept(IEecVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override string ToString()
        {
            return "(" + Expression + ")";
        }
    }

    public class ExpressionParenOpExpr : AbstractSyntaxTree, IExpression
    {
        public ExpressionParenOpExpr(IExpression expression, Operator _operator, IExpression expressionParen)
        {
            ExpressionParen = expressionParen;
            Operator = _operator;
            Expression = expression;
        }

        public IExpression ExpressionParen { get; set; }
        public Operator Operator { get; set; }
        public IExpression Expression { get; set; }

        public override void Accept(IEecVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override string ToString()
        {
            return $"{ExpressionParen} {Operator} {Expression}";
        }
    }
    public class ExpressionExprOpExpr : AbstractSyntaxTree, IExpression
    {
        public ExpressionExprOpExpr(IExpression expression, Operator _operator, IExpression expressionParen)
        {
            ExpressionParen = expressionParen;
            Operator = _operator;
            Expression = expression;
        }

        public IExpression ExpressionParen { get; set; }
        public Operator Operator { get; set; }
        public IExpression Expression { get; set; }

        public override void Accept(IEecVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override string ToString()
        {
            return $"{ExpressionParen} {Operator} {Expression}";
        }
    }
}