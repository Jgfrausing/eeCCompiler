using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eeCCompiler.Interfaces;
using eeCCompiler.Nodes;

namespace eeCCompiler.Visitors
{
    class PrecedenceVisitor : Visitor
    {
        public override void Visit(VarDecleration vardecl)
        {
            IExpression test = vardecl.Expression;
            PrecedenceFixer(ref test,0);
            vardecl.Expression = test;
            base.Visit(vardecl);
        }
        public void PrecedenceFixer(ref IExpression expression, int level)
        {
            if (expression is ExpressionValOpExpr) 
            {
                var exp = expression as ExpressionValOpExpr;
                if (exp.Operator.Symbol == Indexes.Indexes.SymbolIndex.Lt && level == 0)
                {
                    expression = new ExpressionParenOpExpr(new ExpressionValOpExpr(new ExpressionVal((exp.Expression as ExpressionValOpExpr).Value), exp.Operator, exp.Value), (exp.Expression as ExpressionValOpExpr).Operator, (exp.Expression as ExpressionValOpExpr).Expression);
                }
                else if (exp.Operator.Symbol == Indexes.Indexes.SymbolIndex.And && level == 1)
                {
                    expression = new ExpressionParenOpExpr(new ExpressionValOpExpr(new ExpressionVal((exp.Expression as ExpressionValOpExpr).Value), exp.Operator, exp.Value), exp.Operator, (exp.Expression as ExpressionValOpExpr).Expression);
                }
            }
            if (level < 2)
                PrecedenceFixer(ref expression, level + 1);
        }
    }
}
