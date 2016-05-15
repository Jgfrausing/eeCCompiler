﻿using System.Collections.Generic;
using eeCCompiler.Interfaces;
using eeCCompiler.Nodes;

namespace eeCCompiler.Visitors
{
    internal class Precedence : Visitor
    {
        public ExpressionChecker _expressionChecker;
        public Dictionary<IExpression, IValue> ExprParens;


        public Precedence()
        {
            _expressionChecker = new ExpressionChecker(new Typechecker(new List<string>()));
            level = 0;
            ExprParens = new Dictionary<IExpression, IValue>();
        }

        public IExpression GlobalExpr { get; set; }
        public int level { get; set; }

        public override void Visit(VarDecleration vardecl)
        {
            vardecl.Expression = FullPrecedenceFix(vardecl.Expression);
            ExprParens.Clear();
        }

        public override void Visit(IfStatement ifstatement)
        {
            ifstatement.Expression = FullPrecedenceFix(ifstatement.Expression);
            ExprParens.Clear();
        }

        public override void Visit(VarInStructDecleration varInStructDecleration)
        {
            varInStructDecleration.Expression = FullPrecedenceFix(varInStructDecleration.Expression);
            ExprParens.Clear();
        }

        public override void Visit(RepeatExpr repeatExpr)
        {
            repeatExpr.Expression = FullPrecedenceFix(repeatExpr.Expression);
            ExprParens.Clear();
        }

        public override void Visit(RepeatFor repeatFor)
        {
            repeatFor.Expression = FullPrecedenceFix(repeatFor.Expression);
            ExprParens.Clear();

            repeatFor.VarDecleration.Accept(this);
        }

        public override void Visit(Return eecReturn)
        {
            eecReturn.Expression = FullPrecedenceFix(eecReturn.Expression);
            ExprParens.Clear();
        }

        public IExpression FullPrecedenceFix(IExpression expression)
        {
            ReplaceValues(ref expression);
            GlobalExpr = expression;
            while (level < 6)
            {
                PrecedenceFixer(ref expression, 0);
                level++;
            }
            level = 0;

            foreach (var ValueExprParen in ExprParens)
            {
                ReplaceValue(expression, ValueExprParen.Value, ref expression, ValueExprParen.Key);
            }
            return expression;
        }

        public void PrecedenceFixer(ref IExpression expression, int ExprNumber)
        {
            if (expression is ExpressionValOpExpr)
            {
                var expressionValOpExpr = expression as ExpressionValOpExpr;

                if (LevelFinder(expressionValOpExpr.Operator.Symbol) == level)
                {
                    var Symbol = expressionValOpExpr.Operator.Symbol;
                    var leftside = new ExpressionVal(expressionValOpExpr.Value);
                    var rightside = expressionValOpExpr.Expression;
                    expression = new ExpressionExprOpExpr(rightside, new Operator(Symbol), leftside);
                    //GlobalExpr = expression;
                }

                while (expressionValOpExpr.Expression is ExpressionValOpExpr)
                {
                    if (LevelFinder((expressionValOpExpr.Expression as ExpressionValOpExpr).Operator.Symbol) ==
                        level)
                    {
                        var Symbol = (expressionValOpExpr.Expression as ExpressionValOpExpr).Operator.Symbol;
                        var rightside = (expressionValOpExpr.Expression as ExpressionValOpExpr).Expression;
                        var leftside = ExprWalker(ExprNumber);
                        leftside = GlobalExpr;
                        expression = new ExpressionExprOpExpr(rightside, new Operator(Symbol), leftside);
                        GlobalExpr = expression;
                        ExprNumber++;
                    }
                    else
                    {
                        ExprNumber++;
                        expressionValOpExpr = expressionValOpExpr.Expression as ExpressionValOpExpr;
                    }
                }
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

        public void ReplaceValues(ref IExpression expression)
        {
            if (expression is ExpressionParenOpExpr)
            {
                var expressionParenOpExpr = expression as ExpressionParenOpExpr;
                var val = _expressionChecker.CheckExpression(expressionParenOpExpr.ExpressionParen);
                ExprParens.Add(expressionParenOpExpr.ExpressionParen, val);
                expression = new ExpressionValOpExpr(expressionParenOpExpr.Expression, expressionParenOpExpr.Operator,
                    val);

                var exprExpr = (expression as ExpressionValOpExpr).Expression;
                ReplaceValues(ref exprExpr);
                (expression as ExpressionValOpExpr).Expression = exprExpr;
            }
                #region ExpressionParen

            else if (expression is ExpressionParen)
            {
                var expressionParen = expression as ExpressionParen;


                var exprExpr = (expression as ExpressionParen).Expression;

                (expression as ExpressionParen).Expression = FullPrecedenceFix(exprExpr);

                var val = _expressionChecker.CheckExpression(expressionParen.Expression);
                ExprParens.Add(expressionParen, val);

                expression = new ExpressionVal(val);
            }
                #endregion

                #region ExpressionNegate

            else if (expression is ExpressionNegate)
            {
                var expressionNegate = expression as ExpressionNegate;


                var exprExpr = (expression as ExpressionNegate).Expression;
                (expression as ExpressionNegate).Expression = FullPrecedenceFix(exprExpr);

                var val = _expressionChecker.CheckExpression(expressionNegate.Expression);
                ExprParens.Add(expressionNegate, val);

                expression = new ExpressionVal(val);
            }
                #endregion

                #region ExpressionMinus

            else if (expression is ExpressionMinus)
            {
                var exprExpr = (expression as ExpressionMinus).Expression;
                ReplaceValues(ref exprExpr);
                (expression as ExpressionMinus).Expression = exprExpr;
            }
                #endregion

                #region ExpressionValOpExpr

            else if (expression is ExpressionValOpExpr)
            {
                var exprExpr = (expression as ExpressionValOpExpr).Expression;
                ReplaceValues(ref exprExpr);
                (expression as ExpressionValOpExpr).Expression = exprExpr;
            }

            #endregion
        }

        public void ReplaceValue(IExpression expr, IValue val, ref IExpression Parent, IExpression exprParen)
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
                else
                {
                    var exprtemp = (expr as ExpressionValOpExpr).Expression;
                    ReplaceValue((expr as ExpressionValOpExpr).Expression, val, ref exprtemp, exprParen);
                    (expr as ExpressionValOpExpr).Expression = exprtemp;
                }
            }
            else if (expr is ExpressionExprOpExpr)
            {
                var expressionExprOpExpr = expr as ExpressionExprOpExpr;
                ReplaceValue(expressionExprOpExpr.Expression, val, ref expr, exprParen);
                ReplaceValue(expressionExprOpExpr.ExpressionParen, val, ref expr, exprParen);
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
                    else if (Parent is ExpressionVal)
                    {
                        Parent = exprParen;
                    }
                }
            }
            else if (expr is ExpressionParenOpExpr)
            {
                var expressionParenOpExpr = expr as ExpressionParenOpExpr;
                ReplaceValue(expressionParenOpExpr.Expression, val, ref expr, exprParen);
                ReplaceValue(expressionParenOpExpr.ExpressionParen, val, ref expr, exprParen);
            }
        }

        public IExpression ExprWalker(int ExprNumber)
        {
            var Walk = GlobalExpr;
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
    }
}