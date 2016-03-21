using System;
using System.Linq;
using eeCCompiler.Nodes;
using Type = eeCCompiler.Nodes.Type;

namespace eeCCompiler.Visitors
{
    internal class Treeprint : Visitor
    {
        private int _count;
        private const string Indenter = "  ";

        public override void Visit(Root root)
        {
            Console.WriteLine(string.Concat(Enumerable.Repeat(Indenter, _count)) + "Root");
            _count++;
            base.Visit(root);
        }

        public override void Visit(ConstantDefinitions constantDefinitions)
        {
            Console.WriteLine(string.Concat(Enumerable.Repeat(Indenter, _count)) + "Constantdefinitions");
            _count++;
            base.Visit(constantDefinitions);
            _count--;
        }

        public override void Visit(StructDefinitions structDefinitions)
        {
            Console.WriteLine(string.Concat(Enumerable.Repeat(Indenter, _count)) + "StructDefinitions");
            _count++;
            base.Visit(structDefinitions);
            _count--;
        }

        public override void Visit(StructDefinition structDefinition)
        {
            Console.WriteLine(string.Concat(Enumerable.Repeat(Indenter, _count)) + "StructDefinition");
            _count++;
            base.Visit(structDefinition);
            _count--;
        }

        //public override void Visit(Program program)
        //{
        //  Console.WriteLine(String.Concat(Enumerable.Repeat(indenter, count)) + "program");
        //  count++;
        //  base.Visit(program);
        //  count--;
        //}

        public override void Visit(FunctionDeclarations functionDeclarations)
        {
            Console.WriteLine(string.Concat(Enumerable.Repeat(Indenter, _count)) + "FunctionDeclarations");
            _count++;
            base.Visit(functionDeclarations);
            _count--;
        }

        public override void Visit(Body body)
        {
            Console.WriteLine(string.Concat(Enumerable.Repeat(Indenter, _count)) + "Body");
            _count++;
            base.Visit(body);
            _count--;
        }

        public override void Visit(ExpressionNegate expressionNegate)
        {
            Console.WriteLine(string.Concat(Enumerable.Repeat(Indenter, _count)) + "ExpressionNegate");
            _count++;
            base.Visit(expressionNegate);
            _count--;
        }

        public override void Visit(Direction direction)
        {
            Console.WriteLine(string.Concat(Enumerable.Repeat(Indenter, _count)) + "Direction");
            _count++;
            base.Visit(direction);
            _count--;
        }

        public override void Visit(ExpressionValOpExpr rooteValOpExpr)
        {
            Console.WriteLine(string.Concat(Enumerable.Repeat(Indenter, _count)) + "RooteValOpExpr");
            _count++;
            base.Visit(rooteValOpExpr);
            _count--;
        }

        public override void Visit(ExpressionMinus expressionMinus)
        {
            Console.WriteLine(string.Concat(Enumerable.Repeat(Indenter, _count)) + "ExpressionMinus");
            _count++;
            base.Visit(expressionMinus);
            _count--;
        }

        public override void Visit(StructDecleration structDecleration)
        {
            Console.WriteLine(string.Concat(Enumerable.Repeat(Indenter, _count)) + "StructDecleration");
            _count++;
            base.Visit(structDecleration);
            _count--;
        }

        public override void Visit(Type type)
        {
            Console.WriteLine(string.Concat(Enumerable.Repeat(Indenter, _count)) + "Type");
            _count++;
            base.Visit(type);
            _count--;
        }

        public override void Visit(Operator operate)
        {
            Console.WriteLine(string.Concat(Enumerable.Repeat(Indenter, _count)) + "Operate");
            _count++;
            base.Visit(operate);
            _count--;
        }

        public override void Visit(BoolValue boolValue)
        {
            Console.WriteLine(string.Concat(Enumerable.Repeat(Indenter, _count)) + "BoolValue");
            _count++;
            base.Visit(boolValue);
            _count--;
        }

        public override void Visit(FuncCall funcCall)
        {
            Console.WriteLine(string.Concat(Enumerable.Repeat(Indenter, _count)) + "FuncCall");
            _count++;
            base.Visit(funcCall);
            _count--;
        }

        public override void Visit(FunctionDeclaration functionDeclaration)
        {
            Console.WriteLine(string.Concat(Enumerable.Repeat(Indenter, _count)) + "FunctionDeclaration");
            _count++;
            base.Visit(functionDeclaration);
            _count--;
        }

        public override void Visit(VarDecleration varDecleration)
        {
            Console.WriteLine(string.Concat(Enumerable.Repeat(Indenter, _count)) + "VarDecleration");
            _count++;
            base.Visit(varDecleration);
            _count--;
        }

        public override void Visit(VarDeclerations varDecls)
        {
            Console.WriteLine(string.Concat(Enumerable.Repeat(Indenter, _count)) + "VarDecls");
            _count++;
            base.Visit(varDecls);
            _count--;
        }

        public override void Visit(Refrence reference)
        {
            Console.WriteLine(string.Concat(Enumerable.Repeat(Indenter, _count)) + "Reference");
            _count++;
            base.Visit(reference);
            _count--;
        }

        public override void Visit(Return expressionParenOpExpr)
        {
            Console.WriteLine(string.Concat(Enumerable.Repeat(Indenter, _count)) + "ExpressionParenOpExpr");
            _count++;
            base.Visit(expressionParenOpExpr);
            _count--;
        }

        //public override void Visit(FunctionDeclarations functionDeclarations)
        //{
        //    Console.WriteLine(String.Concat(Enumerable.Repeat(indenter, count)) + "functionDeclarations");
        //    count++;
        //    base.Visit(functionDeclarations);
        //count--;
        //}
        public override void Visit(NumValue numValue)
        {
            Console.WriteLine(string.Concat(Enumerable.Repeat(Indenter, _count)) + "NumValue");
            _count++;
            base.Visit(numValue);
            _count--;
        }

        public override void Visit(Identifier identifier)
        {
            Console.WriteLine(string.Concat(Enumerable.Repeat(Indenter, _count)) + "Identifier");
            _count++;
            base.Visit(identifier);
            _count--;
        }

        public override void Visit(StringValue stringValue)
        {
            Console.WriteLine(string.Concat(Enumerable.Repeat(Indenter, _count)) + "StringValue");
            _count++;
            base.Visit(stringValue);
            _count--;
        }

        public override void Visit(RepeatExpr repeatExpr)
        {
            Console.WriteLine(string.Concat(Enumerable.Repeat(Indenter, _count)) + "RepeatExpr");
            _count++;
            base.Visit(repeatExpr);
            _count--;
        }

        public override void Visit(ExpressionList expressionList)
        {
            Console.WriteLine(string.Concat(Enumerable.Repeat(Indenter, _count)) + "ExpressionList");
            _count++;
            base.Visit(expressionList);
            _count--;
        }

        public override void Visit(ExpressionParen expressionParen)
        {
            Console.WriteLine(string.Concat(Enumerable.Repeat(Indenter, _count)) + "ExpressionParen");
            _count++;
            base.Visit(expressionParen);
            _count--;
        }

        public override void Visit(ExpressionParenOpExpr expressionParenOpExpr)
        {
            Console.WriteLine(string.Concat(Enumerable.Repeat(Indenter, _count)) + "ExpressionParenOpExpr");
            _count++;
            base.Visit(expressionParenOpExpr);
            _count--;
        }

        public override void Visit(ExpressionVal expressionVal)
        {
            Console.WriteLine(string.Concat(Enumerable.Repeat(Indenter, _count)) + "ExpressionVal");
            _count++;
            base.Visit(expressionVal);
            _count--;
        }

        public override void Visit(IfStatement ifStatement)
        {
            Console.WriteLine(string.Concat(Enumerable.Repeat(Indenter, _count)) + "IfStatement");
            _count++;
            base.Visit(ifStatement);
            _count--;
        }

        public override void Visit(ElseStatement elseStatement)
        {
            Console.WriteLine(string.Concat(Enumerable.Repeat(Indenter, _count)) + "ElseStatement");
            _count++;
            base.Visit(elseStatement);
            _count--;
        }

        public override void Visit(Constant constant)
        {
            Console.WriteLine(string.Concat(Enumerable.Repeat(Indenter, _count)) + "Constant");
            _count++;
            base.Visit(constant);
            _count--;
        }
    }
}