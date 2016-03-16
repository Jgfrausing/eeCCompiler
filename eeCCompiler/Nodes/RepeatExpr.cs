using System;
using eeCCompiler.Interfaces;

namespace eeCCompiler.Nodes
{
    public class RepeatExpr : AbstractSyntaxTree, IBodypart
    {
        public RepeatExpr(IExpression expression, Body body)
        {
            Expression = expression;
            Body = body;
        }

        public IExpression Expression { get; set; }
        public Body Body { get; set; }

        public override void Accept(IEecVisitor visitor)
        {
            visitor.Visit(this);
        }
    }

    public class RepeatFor : RepeatExpr
    {
        public RepeatFor(VarDecleration varDecleration, Direction direction, IExpression expression, Body body)
            : base(expression, body)
        {
            VarDecleration = varDecleration;
            Direction = direction;
        }

        public VarDecleration VarDecleration { get; set; }
        public Direction Direction { get; set; }

        public override void Accept(IEecVisitor visitor)
        {
            visitor.Visit(this);
        }

    }
}