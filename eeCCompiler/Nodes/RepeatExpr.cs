using eeCCompiler.Interfaces;
using eeCCompiler.Visitors;

namespace eeCCompiler.Nodes
{
    public class RepeatExpr : AbstractSyntaxTree, IBodypart
    {
        public RepeatExpr(Body body, IExpression expression)
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
        public RepeatFor(Body body, IExpression expression, Direction direction, VarDecleration varDecleration)
            : base(body, expression)
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