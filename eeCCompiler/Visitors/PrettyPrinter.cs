using System;
using eeCCompiler.Interfaces;
using eeCCompiler.Nodes;
using Type = eeCCompiler.Nodes.Type;

namespace eeCCompiler.Visitors
{
    public class PrettyPrinter : Visitor
    {
        private string _sourceCode = "";

        public override void Visit(Root root)
        {
            root.ConstantDefinitions.Accept(this);
            root.StructDefinitions.Accept(this);
            _sourceCode += "program";
            root.Program.Accept(this);
            root.FunctionDeclarations.Accept(this);
        }

        public override void Visit(Body body)
        {
            _sourceCode += "{\n";
            base.Visit(body);
            _sourceCode += "}\n";
        }

        public override void Visit(Constant constant)
        {
            _sourceCode += "const ";
            Visit(constant);
            _sourceCode += "\n";
        }

        public override void Visit(ConstantDefinitions constantDefinitions)
        {
            
        }

        public override void Visit(ElseStatement elseStatement)
        {
            _sourceCode += "else \n";
            Visit(elseStatement);
        }

        public override void Visit(ExpressionNegate expressionNegate)
        {
            _sourceCode += "1";
            Visit(expressionNegate);
        }

        public override void Visit(ExpressionVal expressionVal)
        {
            Visit(expressionVal);
        }

        public override void Visit(Direction direction)
        {
            _sourceCode += direction.Incrementing ? " to " : " downto ";
        }

        public override void Visit(ExpressionParenOpExpr expressionParenOpExpr)
        {
            Visit(expressionParenOpExpr.ExpressionParen as ExpressionParen);
            Visit(expressionParenOpExpr.Operator);
            Visit(expressionParenOpExpr.Expression);
        }

        private void Visit(IExpression expression)
        {
            _sourceCode += expression.ToString();
        }

        public override void Visit(ExpressionValOpExpr rooteValOpExpr)
        {
            Console.WriteLine("NaN");
        }

        public override void Visit(ExpressionParen expressionParen)
        {
            _sourceCode += "(";
            Visit(expressionParen.Expression);
            _sourceCode += ")";
        }

        public override void Visit(ExpressionMinus expressionMinus)
        {
            Console.WriteLine("NaN");
        }

        public override void Visit(ExpressionList expressionList)
        {
            Console.WriteLine("NaN");
        }

        public override void Visit(StructDecleration structDecleration)
        {
            Console.WriteLine("NaN");
        }

        public override void Visit(RepeatExpr repeatExpr)
        {
            Console.WriteLine("NaN");
        }

        public override void Visit(Type type)
        {
            Console.WriteLine("NaN");
        }

        public override void Visit(StringValue stringValue)
        {
            Console.WriteLine("NaN");
        }

        public override void Visit(Operator operate)
        {
            Console.WriteLine("NaN");
        }

        public override void Visit(Identifier identifier)
        {
            Console.WriteLine("NaN");
        }

        public override void Visit(BoolValue boolValue)
        {
            Console.WriteLine("NaN");
        }

        public override void Visit(NumValue numValue)
        {
            Console.WriteLine("NaN");
        }

        public override void Visit(FuncCall funcCall)
        {
            Console.WriteLine("NaN");
        }

        public override void Visit(FunctionDeclarations functionDeclarations)
        {
            Console.WriteLine("NaN");
        }

        public override void Visit(FunctionDeclaration functionDeclaration)
        {
            Console.WriteLine("NaN");
        }

        public override void Visit(Return expression)
        {
            Console.WriteLine("!lol");
        }
    }
}