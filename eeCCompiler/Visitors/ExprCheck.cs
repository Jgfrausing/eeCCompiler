using eeCCompiler.Interfaces;
using eeCCompiler.Nodes;

namespace eeCCompiler.Visitors
{
    internal class ExprCheck : Visitor
    {
        public IValue Value { get; set; }

        public override void Visit(ExpressionParen exprParen)
        {
            var newChecker = new ExprCheck();
            exprParen.Expression.Accept(newChecker);
            Value = newChecker.Value;
        }

        public override void Visit(ExpressionVal expressionVal)
        {
        }

        public override void Visit(ExpressionValOpExpr expressionValOpExpr)
        {
        }

        public override void Visit(ExpressionParenOpExpr expressionParenOpExpr)
        {
        }

        public override void Visit(ExpressionNegate expressionNegate)
        {
        }

        public override void Visit(ExpressionMinus expressionVal)
        {
        }

        public override void Visit(FuncCall expressionVal)
        {
        }

        public override void Visit(ListType listType)
        {
        }
    }
}