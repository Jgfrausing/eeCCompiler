using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eeCCompiler.Interfaces;
using eeCCompiler.Nodes;

namespace eeCCompiler.Visitors
{
    class Precedence : Visitor
    {
        public IExpression GlobalExpr { get; set; }
        public ExpressionChecker _expressionChecker;
        public int level { get; set; }
        public Dictionary<IExpression, IValue> ExprParens;


        public Precedence()
        {
            _expressionChecker = new ExpressionChecker(new Typechecker(new List<string>()));
            level = 0;
            ExprParens = new Dictionary<IExpression, IValue>();
        }

        public override void Visit(VarDecleration vardecl)
        {
            var cancer = vardecl.Expression;
            ReplaceValues(ref cancer);
            vardecl.Expression = cancer;
            GlobalExpr = vardecl.Expression;
            IExpression temp = vardecl.Expression;
            while (level < 6)
            {
                PrecedenceFixer(ref temp, 0);
                level++;
            }
            level = 0;

            foreach (var ValueExprParen in ExprParens)
            {
                ReplaceValue(temp, ValueExprParen.Value, ref temp, ValueExprParen.Key);
            }
            vardecl.Expression = temp;
        }

        public void PrecedenceFixer(ref IExpression expression, int ExprNumber)
        {

            if (expression is ExpressionValOpExpr)
            {
                if (expression is ExpressionValOpExpr)
                {
                    ExpressionValOpExpr expressionValOpExpr = expression as ExpressionValOpExpr;

                    while (expressionValOpExpr.Expression is ExpressionValOpExpr)
                    {
                        if (LevelFinder((expressionValOpExpr.Expression as ExpressionValOpExpr).Operator.Symbol) == level)
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
            }
            else if (expression is ExpressionParen)
            {
                IExpression temp = (expression as ExpressionParen).Expression;
                PrecedenceFixer(ref temp, ++ExprNumber);
                (expression as ExpressionParen).Expression = temp;
            }
            else if (expression is ExpressionNegate)
            {
                IExpression temp = (expression as ExpressionNegate).Expression;
                PrecedenceFixer(ref temp, ++ExprNumber);
                (expression as ExpressionNegate).Expression = temp;
            }
            else if (expression is ExpressionMinus)
            {
                IExpression temp = (expression as ExpressionMinus).Expression;
                PrecedenceFixer(ref temp, ++ExprNumber);
                (expression as ExpressionMinus).Expression = temp;
            }
            else if (expression is ExpressionExprOpExpr)
            {
                var ExpressionChecker = new PrecedenceVisitor();
                ExpressionChecker.Org = (expression as ExpressionExprOpExpr).ExpressionParen;
                IExpression temp1 = (expression as ExpressionExprOpExpr).ExpressionParen;
                while (ExpressionChecker.level < 6)
                {
                    ExpressionChecker.PrecedenceFix2(ref temp1, 0);
                    ExpressionChecker.level++;
                }
                (expression as ExpressionExprOpExpr).ExpressionParen = ExpressionChecker.Org;

                var ExpressionParenChecker = new PrecedenceVisitor();
                ExpressionParenChecker.Org = (expression as ExpressionExprOpExpr).Expression;
                IExpression temp2 = (expression as ExpressionExprOpExpr).Expression;
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
            else if (symbol == Indexes.Indexes.SymbolIndex.Plus || symbol == Indexes.Indexes.SymbolIndex.Minus)
                return 4;
            else if (symbol == Indexes.Indexes.SymbolIndex.Lt || symbol == Indexes.Indexes.SymbolIndex.Gt
                || symbol == Indexes.Indexes.SymbolIndex.Gteq || symbol == Indexes.Indexes.SymbolIndex.Lteq)
                return 3;
            else if (symbol == Indexes.Indexes.SymbolIndex.Eqeq || symbol == Indexes.Indexes.SymbolIndex.Exclameq)
                return 2;
            else if (symbol == Indexes.Indexes.SymbolIndex.And)
                return 1;
            else //Kun Or tilbage
                return 0;
        }
        public void ReplaceValues(ref IExpression expression)
        {
            if (expression is ExpressionParenOpExpr)
            {
                var expressionParenOpExpr = expression as ExpressionParenOpExpr;
                var val = _expressionChecker.CheckExpression(expressionParenOpExpr.ExpressionParen);
                ExprParens.Add(expressionParenOpExpr.ExpressionParen, val);
                expression = new ExpressionValOpExpr(expressionParenOpExpr.Expression, expressionParenOpExpr.Operator, val);

                var cancer = (expression as ExpressionValOpExpr).Expression;
                ReplaceValues(ref cancer);
                (expression as ExpressionValOpExpr).Expression = cancer;
            }
            #region ExpressionParen
            else if (expression is ExpressionParen)
            {

                var expressionParen = (expression as ExpressionParen);

                

                var cancer = (expression as ExpressionParen).Expression;
                ReplaceValues(ref cancer);


                GlobalExpr = cancer;
                while (level < 6)
                {
                    PrecedenceFixer(ref cancer, 0);
                    level++;
                }
                level = 0;

                foreach (var ValueExprParen in ExprParens)
                {
                    ReplaceValue(cancer, ValueExprParen.Value, ref cancer, ValueExprParen.Key);
                }

                (expression as ExpressionParen).Expression = cancer;
                var val = _expressionChecker.CheckExpression(expressionParen.Expression);
                ExprParens.Add(expressionParen, val);

                expression = new ExpressionVal(val);
                //(expression as ExpressionParen).Expression = expressionParen;
            }
            #endregion

            #region ExpressionNegate
            else if (expression is ExpressionNegate)
            {
                var expressionNegate = (expression as ExpressionNegate);



                var cancer = (expression as ExpressionNegate).Expression;
                ReplaceValues(ref cancer);


                GlobalExpr = cancer;
                while (level < 6)
                {
                    PrecedenceFixer(ref cancer, 0);
                    level++;
                }
                level = 0;

                foreach (var ValueExprParen in ExprParens)
                {
                    ReplaceValue(cancer, ValueExprParen.Value, ref cancer, ValueExprParen.Key);
                }

                (expression as ExpressionNegate).Expression = cancer;
                var val = _expressionChecker.CheckExpression(expressionNegate.Expression);
                ExprParens.Add(expressionNegate, val);

                expression = new ExpressionVal(val);
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
        public void ReplaceValue(IExpression expr, IValue val, ref IExpression Parent, IExpression exprParen)
        {
            if (expr is ExpressionValOpExpr)
            {
                if ((expr as ExpressionValOpExpr).Value == val)
                {
                    if (Parent is ExpressionValOpExpr)
                    {
                        Parent = new ExpressionParenOpExpr((expr as ExpressionValOpExpr).Expression, (Parent as ExpressionValOpExpr).Operator, exprParen);
                    }
                    else if (Parent is ExpressionExprOpExpr)
                    {
                        var Opr = ((Parent as ExpressionExprOpExpr).ExpressionParen as ExpressionValOpExpr).Operator;
                        (Parent as ExpressionExprOpExpr).ExpressionParen = new ExpressionParenOpExpr((expr as ExpressionValOpExpr).Expression, Opr, exprParen);
                    }
                }

            }
            else if (expr is ExpressionExprOpExpr)
            {
                var derpface = expr as ExpressionExprOpExpr;
                ReplaceValue(derpface.Expression, val, ref expr, exprParen);
                ReplaceValue(derpface.ExpressionParen, val, ref expr, exprParen);
            }
            else if (expr is ExpressionVal)
            {
                if ((expr as ExpressionVal).Value == val)
                {
                    if (Parent is ExpressionValOpExpr)
                    {
                        Parent = new ExpressionParenOpExpr((Parent as ExpressionValOpExpr).Expression, (Parent as ExpressionValOpExpr).Operator, exprParen);
                    }
                    else if (Parent is ExpressionExprOpExpr)
                    {
                        (Parent as ExpressionExprOpExpr).Expression = (exprParen);
                    }
                    else if (Parent is ExpressionParenOpExpr)
                    {
                        (Parent as ExpressionParenOpExpr).Expression = (exprParen);
                    }
                    else if (Parent is ExpressionVal)
                    {
                        Parent = exprParen;
                    }
                }
            }
            else if (expr is ExpressionParenOpExpr)
            {
                var derpface = expr as ExpressionParenOpExpr;
                ReplaceValue(derpface.Expression, val, ref expr, exprParen);
                ReplaceValue(derpface.ExpressionParen, val, ref expr, exprParen);
            }
        }
        public IExpression ExprWalker(int ExprNumber)
        {
            var Walk = GlobalExpr;
            for (int i = 0; i < ExprNumber; i++)
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
                    (Walk as ExpressionValOpExpr).Expression = new ExpressionVal(((Walk as ExpressionValOpExpr).Expression as ExpressionValOpExpr).Value);
            }
            return Walk;
        }
    }
}
