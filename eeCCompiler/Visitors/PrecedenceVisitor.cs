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
        public IExpression Org { get; set; }
        public int level { get; set; }
        public PrecedenceVisitor()
        {
            level = 0;
        }
        public override void Visit(VarDecleration vardecl)
        {
            Org = vardecl.Expression;
            IExpression temp = vardecl.Expression;
            while (level < 6)
            {
                PrecedenceFixer(ref temp, level);
                level++;
            }
            level = 0;
            vardecl.Expression = temp;
        }
        public override void Visit(ExpressionValOpExpr expressionValOpExpr)
        {
            if (LevelFinder(expressionValOpExpr.Operator.Symbol) == level)
            {

            }
        }

        public int LevelFinder(Indexes.Indexes.SymbolIndex symbol)
        {
            if (symbol == Indexes.Indexes.SymbolIndex.Times || symbol == Indexes.Indexes.SymbolIndex.Div ||
                symbol == Indexes.Indexes.SymbolIndex.Mod)
                return 0;
            else if (symbol == Indexes.Indexes.SymbolIndex.Plus || symbol == Indexes.Indexes.SymbolIndex.Minus)
                return 1;
            else if (symbol == Indexes.Indexes.SymbolIndex.Lt || symbol == Indexes.Indexes.SymbolIndex.Gt
                || symbol == Indexes.Indexes.SymbolIndex.Gteq || symbol == Indexes.Indexes.SymbolIndex.Lteq)
                return 2;
            else if (symbol == Indexes.Indexes.SymbolIndex.Eqeq || symbol == Indexes.Indexes.SymbolIndex.Exclameq)
                return 3;
            else if (symbol == Indexes.Indexes.SymbolIndex.And)
                return 4;
            else //Kun Or tilbage
                return 5;
        }



        public void PrecedenceFix2(ref IExpression expression, int ExprNumber)
        {
            
            if (expression is ExpressionValOpExpr)
            {
                var expressionValOpExpr = expression as ExpressionValOpExpr;
                if (expressionValOpExpr.Expression is ExpressionValOpExpr)
                {
                    if (LevelFinder(expressionValOpExpr.Operator.Symbol) == level)
                    {
                        var rightside = expressionValOpExpr.Expression;
                        var leftside = ExprWalker(ExprNumber);
                        expression = new ExpressionParenOpExpr(rightside,expressionValOpExpr.Operator,leftside);
                        Org = expression;
                    }
                    else
                    {
                        var temp = expressionValOpExpr.Expression;
                        PrecedenceFix2(ref temp, ++ExprNumber);
                        (expression as ExpressionValOpExpr).Expression = temp;
                    }
                }
            }
        }
        public IExpression ExprWalker(int ExprNumber)
        {
            IExpression CurrentExpr = Org;
            if (ExprNumber > 0)
            {
                if (Org is ExpressionValOpExpr)
                    CurrentExpr = (Org as ExpressionValOpExpr).Expression;
                ExprNumber--;
                return ExprWalker(ExprNumber);
            }
            else
            {
                if (Org is ExpressionValOpExpr)
                    (CurrentExpr as ExpressionValOpExpr).Expression = new ExpressionVal(((CurrentExpr as ExpressionValOpExpr).Expression as ExpressionValOpExpr).Value);
                return CurrentExpr;
            }
        }




        public void PrecedenceFixer(ref IExpression expression, int level)
        {
            if (expression is ExpressionValOpExpr)
            {
                var exp = expression as ExpressionValOpExpr;
                if (exp.Expression is ExpressionValOpExpr)
                {
                    if (LevelFinder(exp.Operator.Symbol) == level)
                    {
                        var expExpression = exp.Expression as ExpressionValOpExpr;
                        var exp2 = new ExpressionValOpExpr(new ExpressionVal(expExpression.Value), exp.Operator, exp.Value);
                        expression = new ExpressionParenOpExpr(expExpression.Expression, expExpression.Operator, exp2);
                        var temp = (expression as ExpressionParenOpExpr).Expression;
                        PrecedenceFixer(ref temp, level);
                        (expression as ExpressionParenOpExpr).Expression = temp;
                    }
                }
                else
                {
                    //
                }
            }
            else if (expression is ExpressionParen)
            {
                IExpression temp = (expression as ExpressionParen).Expression;
                PrecedenceFixer(ref temp, level);
                (expression as ExpressionParen).Expression = temp;
            }
            else if (expression is ExpressionNegate)
            {
                IExpression temp = (expression as ExpressionNegate).Expression;
                PrecedenceFixer(ref temp, level);
                (expression as ExpressionNegate).Expression = temp;
            }
            else if (expression is ExpressionMinus)
            {
                IExpression temp = (expression as ExpressionMinus).Expression;
                PrecedenceFixer(ref temp, level);
                (expression as ExpressionMinus).Expression = temp;
            }
            else if (expression is ExpressionParenOpExpr)
            {
                var OuterExpression = expression as ExpressionParenOpExpr;

                //if (OuterExpression.Expression is ExpressionParenOpExpr)
                //{ 
                //    var InnerExpression = OuterExpression.Expression as ExpressionParenOpExpr;
                //    if (LevelFinder(InnerExpression.Operator.Symbol) != level && LevelFinder(OuterExpression.Operator.Symbol)  == level)
                //    {
                //        var tempExpr1 = OuterExpression.ExpressionParen;
                //        var tempExprParen1 = OuterExpression.Expression;
                //        var tempExpr2 = InnerExpression.ExpressionParen;
                //        var tempExprParen2 = InnerExpression.Expression;
                //        var tempOuterSymbol = OuterExpression.Operator.Symbol;

                //        OuterExpression.Expression = tempExprParen2;
                //        InnerExpression.Expression = tempExpr1;
                //        OuterExpression.Operator = InnerExpression.Operator;
                //        InnerExpression.Operator.Symbol = tempOuterSymbol;
                //        OuterExpression.ExpressionParen = InnerExpression;
                //    }
                //}
                //else if (OuterExpression.Expression is ExpressionValOpExpr) 
                //{
                //    var InnerExpression = OuterExpression.Expression as ExpressionValOpExpr;
                //    if (LevelFinder(InnerExpression.Operator.Symbol) != level && LevelFinder(OuterExpression.Operator.Symbol) == level)
                //    {
                //       // var temp = InnerExpression.Value;
                //       // OuterExpression.Expression = new ExpressionParenOpExpr(InnerExpression.Expression, InnerExpression.Operator,new ExpressionVal(temp));

                //        var tempExpr1 = OuterExpression.ExpressionParen;
                //        var tempExprParen1 = OuterExpression.Expression;
                //        var tempExprParen2 = InnerExpression.Expression;
                //        var tempOuterSymbol = OuterExpression.Operator.Symbol;

                //        OuterExpression.Expression = tempExprParen2;
                //        InnerExpression.Expression = tempExpr1;
                //        OuterExpression.Operator = InnerExpression.Operator;
                //        InnerExpression.Operator.Symbol = tempOuterSymbol;
                //        OuterExpression.ExpressionParen = InnerExpression;
                //    }
                //}

                expression = OuterExpression;

                IExpression temp1 = (expression as ExpressionParenOpExpr).Expression;
                PrecedenceFixer(ref temp1, level);
                (expression as ExpressionParenOpExpr).Expression = temp1;

                IExpression temp2 = (expression as ExpressionParenOpExpr).ExpressionParen;
                PrecedenceFixer(ref temp2, level);
                (expression as ExpressionParenOpExpr).ExpressionParen = temp2;
            }
          }
        //public void PrecedenceFixer(ref IExpression expression, int level)
        //{
        //    if (expression is ExpressionValOpExpr) 
        //    {
        //        var exp = expression as ExpressionValOpExpr;
        //        if (exp.Expression is ExpressionValOpExpr)
        //        {
        //            if (exp.Operator.Symbol == Indexes.Indexes.SymbolIndex.Times && level == 0)
        //            {
        //                expression = new ExpressionParenOpExpr((exp.Expression as ExpressionValOpExpr).Expression, (exp.Expression as ExpressionValOpExpr).Operator, new ExpressionValOpExpr(new ExpressionVal((exp.Expression as ExpressionValOpExpr).Value), exp.Operator, exp.Value));
        //                var temp = (expression as ExpressionParenOpExpr).Expression;
        //                PrecedenceFixer(ref temp, level);
        //                (expression as ExpressionParenOpExpr).Expression = temp;
        //            }
        //            else if (exp.Operator.Symbol == Indexes.Indexes.SymbolIndex.Lt && level == 1)
        //            {
        //                expression = new ExpressionParenOpExpr((exp.Expression as ExpressionValOpExpr).Expression, (exp.Expression as ExpressionValOpExpr).Operator, new ExpressionValOpExpr(new ExpressionVal((exp.Expression as ExpressionValOpExpr).Value), exp.Operator, exp.Value));
        //                var temp = (expression as ExpressionParenOpExpr).Expression;
        //                PrecedenceFixer(ref temp, level); 
        //                (expression as ExpressionParenOpExpr).Expression = temp;
        //            }
        //            else if (exp.Operator.Symbol == Indexes.Indexes.SymbolIndex.And && level == 2)
        //            {
        //                expression = new ExpressionParenOpExpr((exp.Expression as ExpressionValOpExpr).Expression, (exp.Expression as ExpressionValOpExpr).Operator, new ExpressionValOpExpr(new ExpressionVal((exp.Expression as ExpressionValOpExpr).Value), exp.Operator, exp.Value));
        //                var temp = (expression as ExpressionParenOpExpr).Expression;
        //                PrecedenceFixer(ref temp, level);
        //                (expression as ExpressionParenOpExpr).Expression = temp;
        //            }
        //            else if (exp.Operator.Symbol == Indexes.Indexes.SymbolIndex.Or && level == 3)
        //            {
        //                expression = new ExpressionParenOpExpr((exp.Expression as ExpressionValOpExpr).Expression, (exp.Expression as ExpressionValOpExpr).Operator, new ExpressionValOpExpr(new ExpressionVal((exp.Expression as ExpressionValOpExpr).Value), exp.Operator, exp.Value));
        //                var temp = (expression as ExpressionParenOpExpr).Expression;
        //                PrecedenceFixer(ref temp, level);
        //                (expression as ExpressionParenOpExpr).Expression = temp;
        //            }
        //            if (level < 4 && expression is ExpressionValOpExpr)
        //            {
        //                var temp = (expression as ExpressionValOpExpr).Expression;
        //                PrecedenceFixer(ref temp, level);
        //                (expression as ExpressionValOpExpr).Expression = temp;
        //            }
        //        }
        //    }
        //    if (level < 4)
        //        PrecedenceFixer(ref expression, level+1);
        //}
    }
}
