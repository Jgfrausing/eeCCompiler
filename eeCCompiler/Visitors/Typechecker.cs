using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eeCCompiler.Nodes;
using eeCCompiler.Interfaces;

namespace eeCCompiler.Visitors
{
    public class Typechecker : Visitor
    {
        public override void Visit(ExpressionValOpExpr expressionValOpExpr)
        {
            checkExpression(expressionValOpExpr);


            /*if(expression.Name == "ExpressionVal")
            {
                IValue expressionValue = (expressionValOpExpr.Expression as ExpressionVal).Value;
                if (expressionValue.GetType().Name == value1.Name)
                {
                    if (value1.Name == "NumValue" &&
                        expressionValOpExpr.Operator.Symbol == Indexes.Indexes.SymbolIndex.Plus ||
                        expressionValOpExpr.Operator.Symbol == Indexes.Indexes.SymbolIndex.Minus ||
                        expressionValOpExpr.Operator.Symbol == Indexes.Indexes.SymbolIndex.Div ||
                        expressionValOpExpr.Operator.Symbol == Indexes.Indexes.SymbolIndex.Times ||
                        expressionValOpExpr.Operator.Symbol == Indexes.Indexes.SymbolIndex.Mod)
                    {
                        Console.WriteLine("YOU DID A NUM RIGHT");
                    }
                    else if (value1.Name == "StringValue" &&
                            expressionValOpExpr.Operator.Symbol == Indexes.Indexes.SymbolIndex.Plus)
                    {
                        Console.WriteLine("YOU DID A STRING RIGHT");
                    }
                    else if (value1.Name == "BoolValue" &&
                        expressionValOpExpr.Operator.Symbol == Indexes.Indexes.SymbolIndex.And ||
                        expressionValOpExpr.Operator.Symbol == Indexes.Indexes.SymbolIndex.Eqeq ||
                        expressionValOpExpr.Operator.Symbol == Indexes.Indexes.SymbolIndex.Lt ||
                        expressionValOpExpr.Operator.Symbol == Indexes.Indexes.SymbolIndex.Lteq ||
                        expressionValOpExpr.Operator.Symbol == Indexes.Indexes.SymbolIndex.Gt ||
                        expressionValOpExpr.Operator.Symbol == Indexes.Indexes.SymbolIndex.Gteq ||
                        expressionValOpExpr.Operator.Symbol == Indexes.Indexes.SymbolIndex.Or ||
                        expressionValOpExpr.Operator.Symbol == Indexes.Indexes.SymbolIndex.Exclameq)
                    {
                        Console.WriteLine("YOU DID A BOOL RIGHT");
                    }*/
        }
        public IValue checkExpression(IExpression expression)
        {
            System.Type ExpressionType = expression.GetType();

            if (ExpressionType.Name == "ExpressionVal")
            {
                return ((expression as ExpressionVal).Value);
            }
            else if (ExpressionType.Name == "ExpressionParen")
            {
                return (checkExpression((expression as ExpressionParen).Expression));
            }
            else if (ExpressionType.Name == "ExpressionNegate")
            {
                return (checkExpression((expression as ExpressionNegate).Expression));
            }
            else if (ExpressionType.Name == "ExpressionMinus")
            {
                return (checkExpression((expression as ExpressionMinus).Expression));
            }
            else if (ExpressionType.Name == "ExpressionValOpExpr")
            {
                ExpressionValOpExpr expressionValOpExpr = (expression as ExpressionValOpExpr);
                System.Type value1 = expressionValOpExpr.Value.GetType();
                System.Type expression2 = expressionValOpExpr.Expression.GetType();
                System.Type value2 = checkExpression(expressionValOpExpr.Expression).GetType();
                if (value2.Name == value1.Name)
                    Console.WriteLine("FUCK YOU MATHIAS");
                else
                    Console.WriteLine("FUCK YOU DERPFACE");
            }
                
            return new NumValue(2.0);
        }
    }
}