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
        public Typechecker(List<string> input) : base()
        {
            Errors = input;
        }
        public List<string>  Errors { get; set; }
        public override void Visit(ExpressionNegate expressionNegate)
        {
            checkExpression(expressionNegate);
        }
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
        public override void Visit(ExpressionMinus expressionMinus)
        {
            checkExpression(expressionMinus);
        }
        public override void Visit(ExpressionParen expressionParen)
        {
            checkExpression(expressionParen);
        }
        public override void Visit(ExpressionParenOpExpr expressionParenOpExpr)
        {
            checkExpression(expressionParenOpExpr);
        }
        public override void Visit(ExpressionVal expressionVal)
        {
            checkExpression(expressionVal);
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
                IValue value = checkExpression((expression as ExpressionNegate).Expression);
                if (!(value is BoolValue))
                   Errors.Add(ExpressionType.Name + " tried with " + value.GetType().Name);
                return value;
            }
            else if (ExpressionType.Name == "ExpressionMinus")
            {
                return (checkExpression((expression as ExpressionMinus).Expression));
            }
            else if (ExpressionType.Name == "ExpressionValOpExpr")
            {
                ExpressionValOpExpr expressionValOpExpr = (expression as ExpressionValOpExpr);
                System.Type value1 = expressionValOpExpr.Value.GetType();
                System.Type value2 = checkExpression(expressionValOpExpr.Expression).GetType();
                if (value2.Name != value1.Name)
                    Errors.Add(ExpressionType.Name + " with " + value1.Name + " and " + value2.Name);
                return expressionValOpExpr.Value;
            }
            else if (ExpressionType.Name == "ExpressionParenOpExpr")
            {
                ExpressionParenOpExpr expressionParenOpExpr = (expression as ExpressionParenOpExpr);

                IValue value = checkExpression(expressionParenOpExpr.ExpressionParen);
                System.Type value1 = value.GetType();
                
                System.Type value2 = checkExpression(expressionParenOpExpr.Expression).GetType();
                if (value2.Name != value1.Name)
                    Errors.Add(ExpressionType.Name + " with " + value1.Name + " and " + value2.Name);
                return value;
            }
                
            return new NumValue(2.0); //Skulle aldrig rammes med skal være her ellers klager visual studio
        }
    }
}