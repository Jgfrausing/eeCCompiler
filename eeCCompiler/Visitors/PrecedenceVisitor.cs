using System.Collections.Generic;
using eeCCompiler.Interfaces;
using eeCCompiler.Nodes;

namespace eeCCompiler.Visitors
{
    internal class PrecedenceVisitor : Visitor
    {
        public ExpressionChecker _expressionChecker;
        public Dictionary<IValue, IExpression> IvalIExprDict;

        public PrecedenceVisitor()
        {
            _expressionChecker = new ExpressionChecker(new Typechecker(new List<string>()));
            level = 0;
            IvalIExprDict = new Dictionary<IValue, IExpression>();
        }

        public IExpression Org { get; set; }
        public int level { get; set; }

        public override void Visit(VarDecleration vardecl)
        {
            var cancer = vardecl.Expression;
            ReplaceValues(ref cancer);
            vardecl.Expression = cancer;
            Org = vardecl.Expression;
            var temp = vardecl.Expression;
            while (level < 6)
            {
                PrecedenceFix2(ref temp, 0);
                level++;
            }
            level = 0;

            foreach (var item in IvalIExprDict)
            {
                ReplaceValue(temp, item.Key, temp, item.Value);
            }
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
                return 5;
            if (symbol == Indexes.Indexes.SymbolIndex.Plus || symbol == Indexes.Indexes.SymbolIndex.Minus)
                return 4;
            if (symbol == Indexes.Indexes.SymbolIndex.Lt || symbol == Indexes.Indexes.SymbolIndex.Gt
                || symbol == Indexes.Indexes.SymbolIndex.Gteq || symbol == Indexes.Indexes.SymbolIndex.Lteq)
                return 3;
            if (symbol == Indexes.Indexes.SymbolIndex.Eqeq || symbol == Indexes.Indexes.SymbolIndex.Exclameq)
                return 2;
            if (symbol == Indexes.Indexes.SymbolIndex.And)
                return 1;
            return 0;
        }


        public void PrecedenceFix2(ref IExpression expression, int ExprNumber)
        {
            if (expression is ExpressionValOpExpr)
            {
                if (expression is ExpressionValOpExpr)
                {
                    var expressionValOpExpr = expression as ExpressionValOpExpr;

                    while (expressionValOpExpr.Expression is ExpressionValOpExpr)
                    {
                        if (LevelFinder((expressionValOpExpr.Expression as ExpressionValOpExpr).Operator.Symbol) ==
                            level)
                        {
                            var Symbol = (expressionValOpExpr.Expression as ExpressionValOpExpr).Operator.Symbol;
                            var rightside = (expressionValOpExpr.Expression as ExpressionValOpExpr).Expression;
                            var leftside = ExprWalker(ExprNumber);
                            leftside = Org;
                            expression = new ExpressionExprOpExpr(rightside, new Operator(Symbol), leftside);
                            Org = expression;
                            ExprNumber++;
                        }
                        else
                        {
                            ExprNumber++;
                            expressionValOpExpr = expressionValOpExpr.Expression as ExpressionValOpExpr;
                        }
                    }
                    //var temp = expressionValOpExpr.Expression;
                    //PrecedenceFix2(ref temp, ++ExprNumber);
                    //expressionValOpExpr.Expression = temp;
                }
            }
            else if (expression is ExpressionParen)
            {
                var temp = (expression as ExpressionParen).Expression;
                PrecedenceFix2(ref temp, ++ExprNumber);
                (expression as ExpressionParen).Expression = temp;
            }
            else if (expression is ExpressionNegate)
            {
                var temp = (expression as ExpressionNegate).Expression;
                PrecedenceFix2(ref temp, ++ExprNumber);
                (expression as ExpressionNegate).Expression = temp;
            }
            else if (expression is ExpressionMinus)
            {
                var temp = (expression as ExpressionMinus).Expression;
                PrecedenceFix2(ref temp, ++ExprNumber);
                (expression as ExpressionMinus).Expression = temp;
            }
            else if (expression is ExpressionExprOpExpr)
            {
                var ExpressionChecker = new PrecedenceVisitor();
                ExpressionChecker.Org = (expression as ExpressionExprOpExpr).ExpressionParen;
                var temp1 = (expression as ExpressionExprOpExpr).ExpressionParen;
                while (ExpressionChecker.level < 6)
                {
                    ExpressionChecker.PrecedenceFix2(ref temp1, 0);
                    ExpressionChecker.level++;
                }
                (expression as ExpressionExprOpExpr).ExpressionParen = ExpressionChecker.Org;

                var ExpressionParenChecker = new PrecedenceVisitor();
                ExpressionParenChecker.Org = (expression as ExpressionExprOpExpr).Expression;
                var temp2 = (expression as ExpressionExprOpExpr).Expression;
                while (ExpressionParenChecker.level < 6)
                {
                    ExpressionParenChecker.PrecedenceFix2(ref temp2, 0);
                    ExpressionParenChecker.level++;
                }
                (expression as ExpressionExprOpExpr).Expression = ExpressionParenChecker.Org;
            }
            //else if (expression is ExpressionParenOpExpr)
            //{
            //    //Her Skal vi checke om OP mellem Paren og Expr bliver overgået af operator i Expr
            //    var expressionParenOpExpr = expression as ExpressionParenOpExpr;

            //    //Operator vi skal være lavere end
            //    var LevelOfOp = LevelFinder(expressionParenOpExpr.Operator.Symbol);

            //    if (LevelOfOp == level)
            //    {
            //        //Så skal den beholde strukturen
            //    }
            //    else
            //    {
            //        //Check om expression har en opr på level
            //        var val = _expressionChecker.CheckExpression(expressionParenOpExpr.ExpressionParen);
            //        IExpression NewExpr = new ExpressionValOpExpr(expressionParenOpExpr.Expression, expressionParenOpExpr.Operator,val);


            //        var ExpressionChecker = new PrecedenceVisitor();
            //        ExpressionChecker.Org = NewExpr;
            //        IExpression temp1 = NewExpr;
            //        while (ExpressionChecker.level < 6)
            //        {
            //            ExpressionChecker.PrecedenceFix2(ref temp1, 0);
            //            ExpressionChecker.level++;
            //        }
            //        NewExpr = ExpressionChecker.Org;

            //        ReplaceValue(NewExpr, val,NewExpr, expressionParenOpExpr.ExpressionParen);

            //        Org = NewExpr;
            //        PrecedenceFix2(ref NewExpr, ExprNumber);
            //        level = 6;
            //        //if (NewExpr is ExpressionValOpExpr)
            //        //    expression = new ExpressionParenOpExpr((NewExpr as ExpressionValOpExpr).Expression, expressionParenOpExpr.Operator, expressionParenOpExpr.ExpressionParen);
            //        //else if (NewExpr is ExpressionExprOpExpr) { 
            //        //    expression = new ExpressionParenOpExpr(NewExpr, expressionParenOpExpr.Operator, expressionParenOpExpr.ExpressionParen);
            //        //}
            //    }
            //}
        }


        public void ReplaceValues(ref IExpression expression)
        {
            if (expression is ExpressionParenOpExpr)
            {
                var expressionParenOpExpr = expression as ExpressionParenOpExpr;
                var val = _expressionChecker.CheckExpression(expressionParenOpExpr.ExpressionParen);
                IvalIExprDict.Add(val, expressionParenOpExpr.ExpressionParen);
                expression = new ExpressionValOpExpr(expressionParenOpExpr.Expression, expressionParenOpExpr.Operator,
                    val);

                var cancer = (expression as ExpressionValOpExpr).Expression;
                ReplaceValues(ref cancer);
                (expression as ExpressionValOpExpr).Expression = cancer;
            }
                #region ExpressionParen

            else if (expression is ExpressionParen)
            {
                var cancer = (expression as ExpressionParen).Expression;
                ReplaceValues(ref cancer);
                (expression as ExpressionParen).Expression = cancer;
            }
                #endregion

                #region ExpressionNegate

            else if (expression is ExpressionNegate)
            {
                var cancer = (expression as ExpressionNegate).Expression;
                ReplaceValues(ref cancer);
                (expression as ExpressionNegate).Expression = cancer;
            }
                #endregion

                #region ExpressionMinus

            else if (expression is ExpressionMinus)
            {
                var cancer = (expression as ExpressionMinus).Expression;
                ReplaceValues(ref cancer);
                (expression as ExpressionMinus).Expression = cancer;
            }
                #endregion

                #region ExpressionValOpExpr

            else if (expression is ExpressionValOpExpr)
            {
                var cancer = (expression as ExpressionValOpExpr).Expression;
                ReplaceValues(ref cancer);
                (expression as ExpressionValOpExpr).Expression = cancer;
            }

            #endregion
        }

        public void ReplaceValue(IExpression expr, IValue val, IExpression Parent, IExpression exprParen)
        {
            if (expr is ExpressionValOpExpr)
            {
                if ((expr as ExpressionValOpExpr).Value == val)
                {
                    if (Parent is ExpressionValOpExpr)
                    {
                        Parent = new ExpressionParenOpExpr((expr as ExpressionValOpExpr).Expression,
                            (Parent as ExpressionValOpExpr).Operator, exprParen);
                    }
                    else if (Parent is ExpressionExprOpExpr)
                    {
                        var Opr = ((Parent as ExpressionExprOpExpr).ExpressionParen as ExpressionValOpExpr).Operator;
                        (Parent as ExpressionExprOpExpr).ExpressionParen =
                            new ExpressionParenOpExpr((expr as ExpressionValOpExpr).Expression, Opr, exprParen);
                    }
                }
            }
            else if (expr is ExpressionExprOpExpr)
            {
                var derpface = expr as ExpressionExprOpExpr;
                ReplaceValue(derpface.Expression, val, expr, exprParen);
                ReplaceValue(derpface.ExpressionParen, val, expr, exprParen);
            }
            else if (expr is ExpressionVal)
            {
                if ((expr as ExpressionVal).Value == val)
                {
                    if (Parent is ExpressionValOpExpr)
                    {
                        Parent = new ExpressionParenOpExpr((Parent as ExpressionValOpExpr).Expression,
                            (Parent as ExpressionValOpExpr).Operator, exprParen);
                    }
                    else if (Parent is ExpressionExprOpExpr)
                    {
                        (Parent as ExpressionExprOpExpr).Expression = exprParen;
                    }
                    else if (Parent is ExpressionParenOpExpr)
                    {
                        (Parent as ExpressionParenOpExpr).Expression = exprParen;
                    }
                }
            }
            else if (expr is ExpressionParenOpExpr)
            {
                var derpface = expr as ExpressionParenOpExpr;
                ReplaceValue(derpface.Expression, val, expr, exprParen);
                ReplaceValue(derpface.ExpressionParen, val, expr, exprParen);
            }
        }

        public IExpression ExprWalker(int ExprNumber)
        {
            var Walk = Org;
            for (var i = 0; i < ExprNumber; i++)
            {
                if (Walk is ExpressionValOpExpr)
                {
                    Walk = (Walk as ExpressionValOpExpr).Expression;
                }
                else if (Walk is ExpressionParenOpExpr)
                {
                    Walk = (Walk as ExpressionParenOpExpr).Expression;
                }
            }
            if (Walk is ExpressionValOpExpr)
            {
                if ((Walk as ExpressionValOpExpr).Expression is ExpressionValOpExpr)
                    (Walk as ExpressionValOpExpr).Expression =
                        new ExpressionVal(((Walk as ExpressionValOpExpr).Expression as ExpressionValOpExpr).Value);
            }
            return Walk;
        }

        //                expression = new ExpressionParenOpExpr(expExpression.Expression, expExpression.Operator, exp2);
        //                var exp2 = new ExpressionValOpExpr(new ExpressionVal(expExpression.Value), exp.Operator, exp.Value);
        //                var expExpression = exp.Expression as ExpressionValOpExpr;
        //            {
        //            if (LevelFinder(exp.Operator.Symbol) == level)
        //        {
        //        if (exp.Expression is ExpressionValOpExpr)
        //        var exp = expression as ExpressionValOpExpr;
        //    {
        //    if (expression is ExpressionValOpExpr)
        //{


        //public void PrecedenceFixer(ref IExpression expression, int level)
        //                var temp = (expression as ExpressionParenOpExpr).Expression;
        //                PrecedenceFixer(ref temp, level);
        //                (expression as ExpressionParenOpExpr).Expression = temp;
        //            }
        //        }
        //        else
        //        {
        //            //
        //        }
        //    }
        //    else if (expression is ExpressionParen)
        //    {
        //        IExpression temp = (expression as ExpressionParen).Expression;
        //        PrecedenceFixer(ref temp, level);
        //        (expression as ExpressionParen).Expression = temp;
        //    }
        //    else if (expression is ExpressionNegate)
        //    {
        //        IExpression temp = (expression as ExpressionNegate).Expression;
        //        PrecedenceFixer(ref temp, level);
        //        (expression as ExpressionNegate).Expression = temp;
        //    }
        //    else if (expression is ExpressionMinus)
        //    {
        //        IExpression temp = (expression as ExpressionMinus).Expression;
        //        PrecedenceFixer(ref temp, level);
        //        (expression as ExpressionMinus).Expression = temp;
        //    }
        //    else if (expression is ExpressionParenOpExpr)
        //    {
        //        var OuterExpression = expression as ExpressionParenOpExpr;

        //        //if (OuterExpression.Expression is ExpressionParenOpExpr)
        //        //{ 
        //        //    var InnerExpression = OuterExpression.Expression as ExpressionParenOpExpr;
        //        //    if (LevelFinder(InnerExpression.Operator.Symbol) != level && LevelFinder(OuterExpression.Operator.Symbol)  == level)
        //        //    {
        //        //        var tempExpr1 = OuterExpression.ExpressionParen;
        //        //        var tempExprParen1 = OuterExpression.Expression;
        //        //        var tempExpr2 = InnerExpression.ExpressionParen;
        //        //        var tempExprParen2 = InnerExpression.Expression;
        //        //        var tempOuterSymbol = OuterExpression.Operator.Symbol;

        //        //        OuterExpression.Expression = tempExprParen2;
        //        //        InnerExpression.Expression = tempExpr1;
        //        //        OuterExpression.Operator = InnerExpression.Operator;
        //        //        InnerExpression.Operator.Symbol = tempOuterSymbol;
        //        //        OuterExpression.ExpressionParen = InnerExpression;
        //        //    }
        //        //}
        //        //else if (OuterExpression.Expression is ExpressionValOpExpr) 
        //        //{
        //        //    var InnerExpression = OuterExpression.Expression as ExpressionValOpExpr;
        //        //    if (LevelFinder(InnerExpression.Operator.Symbol) != level && LevelFinder(OuterExpression.Operator.Symbol) == level)
        //        //    {
        //        //       // var temp = InnerExpression.Value;
        //        //       // OuterExpression.Expression = new ExpressionParenOpExpr(InnerExpression.Expression, InnerExpression.Operator,new ExpressionVal(temp));

        //        //        var tempExpr1 = OuterExpression.ExpressionParen;
        //        //        var tempExprParen1 = OuterExpression.Expression;
        //        //        var tempExprParen2 = InnerExpression.Expression;
        //        //        var tempOuterSymbol = OuterExpression.Operator.Symbol;

        //        //        OuterExpression.Expression = tempExprParen2;
        //        //        InnerExpression.Expression = tempExpr1;
        //        //        OuterExpression.Operator = InnerExpression.Operator;
        //        //        InnerExpression.Operator.Symbol = tempOuterSymbol;
        //        //        OuterExpression.ExpressionParen = InnerExpression;
        //        //    }
        //        //}

        //        expression = OuterExpression;

        //        int OldLevel = level;

        //        level = 0;
        //        IExpression temp1 = (expression as ExpressionParenOpExpr).Expression;
        //        PrecedenceFixer(ref temp1, level);
        //        (expression as ExpressionParenOpExpr).Expression = temp1;

        //        level = 0;
        //        IExpression temp2 = (expression as ExpressionParenOpExpr).ExpressionParen;
        //        PrecedenceFixer(ref temp2, level);
        //        (expression as ExpressionParenOpExpr).ExpressionParen = temp2;
        //    }
        //  }
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