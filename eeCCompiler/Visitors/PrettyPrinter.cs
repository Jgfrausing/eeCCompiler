using System;
using eeCCompiler.Nodes;
using Type = eeCCompiler.Nodes.Type;

namespace eeCCompiler.Visitors
{
    public class PrettyPrinter : IEecVisitor
    {
        public void Visit(Root root)
        {
            Console.WriteLine(" Batman");
        }

        public void Visit(Body body)
        {
            Console.WriteLine("NaN");
        }

        public void Visit(Constant constant)
        {
            Console.WriteLine("NaN");
        }

        public void Visit(ConstantDefinitions constantDefinitions)
        {
            Console.WriteLine("NaN");
        }

        public void Visit(ElseStatement elseStatement)
        {
            Console.WriteLine("NaN");
        }

        public void Visit(ExpressionNegate expressionNegate)
        {
            Console.WriteLine("NaN");
        }

        public void Visit(ExpressionVal expressionVal)
        {
            Console.WriteLine("NaN");
        }

        public void Visit(Direction direction)
        {
            Console.WriteLine("NaN");
        }

        public void Visit(ExpressionParenOpExpr expressionParenOpExpr)
        {
            Console.WriteLine("NaN");
        }

        public void Visit(ExpressionValOpExpr rooteValOpExpr)
        {
            Console.WriteLine("NaN");
        }

        public void Visit(ExpressionParen expressionParen)
        {
            Console.WriteLine("NaN");
        }

        public void Visit(ExpressionMinus expressionMinus)
        {
            Console.WriteLine("NaN");
        }

        public void Visit(ExpressionList expressionList)
        {
            Console.WriteLine("NaN");
        }

        public void Visit(StructDecleration structDecleration)
        {
            Console.WriteLine("NaN");
        }

        public void Visit(RepeatExpr repeatExpr)
        {
            Console.WriteLine("NaN");
        }

        public void Visit(Type type)
        {
            Console.WriteLine("NaN");
        }

        public void Visit(StringValue stringValue)
        {
            Console.WriteLine("NaN");
        }

        public void Visit(Operator operate)
        {
            Console.WriteLine("NaN");
        }

        public void Visit(Identifier identifier)
        {
            Console.WriteLine("NaN");
        }

        public void Visit(BoolValue boolValue)
        {
            Console.WriteLine("NaN");
        }

        public void Visit(NumValue numValue)
        {
            Console.WriteLine("NaN");
        }

        public void Visit(FuncCall funcCall)
        {
            Console.WriteLine("NaN");
        }

        public void Visit(FunctionDeclarations functionDeclarations)
        {
            Console.WriteLine("NaN");
        }

        public void Visit(FunctionDeclaration functionDeclaration)
        {
            Console.WriteLine("NaN");
        }

        public void Visit(Return expression)
        {
            Console.WriteLine("lol");
        }
    }
}