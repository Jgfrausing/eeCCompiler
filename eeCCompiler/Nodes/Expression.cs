﻿using eeCCompiler.Interfaces;

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
            Value.Accept(visitor);
            visitor.Visit(this);
        }
    }

    public class ExpressionValOpExpr : AbstractSyntaxTree, IExpression
    {
        public ExpressionValOpExpr(IValue value, Operator _operator, IExpression expression)
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
            Value.Accept(visitor);
            Operator.Accept(visitor);
            Expression.Accept(visitor);
            visitor.Visit(this);
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
            Expression.Accept(visitor);
            visitor.Visit(this);
        }
    }

    public class ExpressionMinus : AbstractSyntaxTree, IExpression
    {
        public ExpressionMinus(IExpression iExpression)
        {
            Expression = iExpression;
        }

        public IExpression Expression { get; set; }
        public override void Accept(IEecVisitor visitor)
        {
            Expression.Accept(visitor);
            visitor.Visit(this);
        }
    }

    public class ExpressionParen : AbstractSyntaxTree, IExpression
    {
        public ExpressionParen(IExpression iExpression)
        {
            Expression = iExpression;
        }

        public IExpression Expression { get; set; }
        public override void Accept(IEecVisitor visitor)
        {
            Expression.Accept(visitor);
            visitor.Visit(this);
        }
    }

    public class ExpressionParenOpExpr : AbstractSyntaxTree, IExpression
    {
        public ExpressionParenOpExpr(IExpression expressionParen, Operator _operator, IExpression expression)
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
            ExpressionParen.Accept(visitor);
            Operator.Accept(visitor);
            Expression.Accept(visitor);
            visitor.Visit(this);
        }
    }

    internal class OptExpression : AbstractSyntaxTree, IExpression
    {
        public override void Accept(IEecVisitor visitor)
        {
            throw new System.NotImplementedException();
        }
    }
}