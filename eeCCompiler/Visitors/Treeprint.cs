using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eeCCompiler.Nodes;

namespace eeCCompiler.Visitors
{
    class Treeprint:Visitor
    {
        private int count = 0;
        public override void Visit(Root root)
        {
            Console.WriteLine(String.Concat(Enumerable.Repeat("  ", count)) + "root");
            count++;
            base.Visit(root);
        }

        public override void Visit(ConstantDefinitions constantDefinitions)
        {
            Console.WriteLine(String.Concat(Enumerable.Repeat("  ", count)) + "constantdefinitions");
            count++;
            base.Visit(constantDefinitions);
            count--;
        }

        //public override void Visit(StructDefinitions StructDefinitions)
        //{
        //  Console.WriteLine(String.Concat(Enumerable.Repeat("  ", count)) + "StructDefinitions");
        //  count++;
        //  base.Visit(StructDefinitions);
        //  count--;
        //}

        //public override void Visit(Program program)
        //{
        //  Console.WriteLine(String.Concat(Enumerable.Repeat("  ", count)) + "program");
        //  count++;
        //  base.Visit(program);
        //  count--;
        //}

        public override void Visit(FunctionDeclarations functionDeclarations)
        {
            Console.WriteLine(String.Concat(Enumerable.Repeat("  ", count)) + "functionDeclarations");
            count++;
            base.Visit(functionDeclarations);
            count--;
        }

        public override void Visit(Body body)
        {
            
            Console.WriteLine(String.Concat(Enumerable.Repeat("  ", count)) + "Body");
            count++;
            base.Visit(body);
            count--;
        }

        public override void Visit(ExpressionNegate expressionNegate)
        {
            Console.WriteLine(String.Concat(Enumerable.Repeat("  ", count)) + "expressionNegate");
            count++;
            base.Visit(expressionNegate);
            count--;
        }
        public override void Visit(Direction direction)
        {
            Console.WriteLine(String.Concat(Enumerable.Repeat("  ", count)) + "direction");
            count++;
            base.Visit(direction);
            count--;
        }

        public override void Visit(ExpressionValOpExpr rooteValOpExpr)
        {
            Console.WriteLine(String.Concat(Enumerable.Repeat("  ", count)) + "rooteValOpExpr");
            count++;
            base.Visit(rooteValOpExpr);
            count--;
        }

        public override void Visit(ExpressionMinus expressionMinus)
        {
            Console.WriteLine(String.Concat(Enumerable.Repeat("  ", count)) + "expressionMinus");
            count++;
            base.Visit(expressionMinus);
            count--;
        }

        public override void Visit(StructDecleration structDecleration)
        {
            Console.WriteLine(String.Concat(Enumerable.Repeat("  ", count)) + "structDecleration");
            count++;
            base.Visit(structDecleration);
            count--;
        }

        public override void Visit(Nodes.Type type)
        {
            Console.WriteLine(String.Concat(Enumerable.Repeat("  ", count)) + "type");
            count++;
            base.Visit(type);
            count--;
        }

        public override void Visit(Operator operate)
        {
            Console.WriteLine(String.Concat(Enumerable.Repeat("  ", count)) + "operate");
            count++;
            base.Visit(operate);
            count--;
        }
        public override void Visit(BoolValue boolValue)
        {
            Console.WriteLine(String.Concat(Enumerable.Repeat("  ", count)) + "boolValue");
            count++;
            base.Visit(boolValue);
            count--;
        }
        public override void Visit(FuncCall funcCall)
        {
            Console.WriteLine(String.Concat(Enumerable.Repeat("  ", count)) + "funcCall");
            count++;
            base.Visit(funcCall);
            count--;
        }
        public override void Visit(FunctionDeclaration functionDeclaration)
        {
            Console.WriteLine(String.Concat(Enumerable.Repeat("  ", count)) + "functionDeclaration");
            count++;
            base.Visit(functionDeclaration);
            count--;
        }
        public override void Visit(VarDecleration varDecleration)
        {
            Console.WriteLine(String.Concat(Enumerable.Repeat("  ", count)) + "varDecleration");
            count++;
            base.Visit(varDecleration);
            count--;
        }
        public override void Visit(VarDeclerations varDecls)
        {
            Console.WriteLine(String.Concat(Enumerable.Repeat("  ", count)) + "varDecls");
            count++;
            base.Visit(varDecls);
            count--;
        }
        public override void Visit(Refrence reference)
        {
            Console.WriteLine(String.Concat(Enumerable.Repeat("  ", count)) + "reference");
            count++;
            base.Visit(reference);
            count--;
        }
        public override void Visit(Return expressionParenOpExpr)
        {
            Console.WriteLine(String.Concat(Enumerable.Repeat("  ", count)) + "expressionParenOpExpr");
            count++;
            base.Visit(expressionParenOpExpr);
            count--;
        }
        //public override void Visit(FunctionDeclarations functionDeclarations)
        //{
        //    Console.WriteLine(String.Concat(Enumerable.Repeat("  ", count)) + "functionDeclarations");
        //    count++;
        //    base.Visit(functionDeclarations);
        //count--;
        //}
        public override void Visit(NumValue numValue)
        {
            Console.WriteLine(String.Concat(Enumerable.Repeat("  ", count)) + "numValue");
            count++;
            base.Visit(numValue);
            count--;
        }
        public override void Visit(Identifier identifier)
        {
            Console.WriteLine(String.Concat(Enumerable.Repeat("  ", count)) + "identifier");
            count++;
            base.Visit(identifier);
            count--;
        }
        public override void Visit(StringValue stringValue)
        {
            Console.WriteLine(String.Concat(Enumerable.Repeat("  ", count)) + "stringValue");
            count++;
            base.Visit(stringValue);
            count--;
        }
        public override void Visit(RepeatExpr repeatExpr)
        {
            Console.WriteLine(String.Concat(Enumerable.Repeat("  ", count)) + "repeatExpr");
            count++;
            base.Visit(repeatExpr);
            count--;
        }
        public override void Visit(ExpressionList expressionList)
        {
            Console.WriteLine(String.Concat(Enumerable.Repeat("  ", count)) + "expressionList");
            count++;
            base.Visit(expressionList);
            count--;
        }
        public override void Visit(ExpressionParen expressionParen)
        {
            Console.WriteLine(String.Concat(Enumerable.Repeat("  ", count)) + "expressionParen");
            count++;
            base.Visit(expressionParen);
            count--;
        }
        public override void Visit(ExpressionParenOpExpr expressionParenOpExpr)
        {
            Console.WriteLine(String.Concat(Enumerable.Repeat("  ", count)) + "expressionParenOpExpr");
            count++;
            base.Visit(expressionParenOpExpr);
            count--;
        }
        public override void Visit(ExpressionVal expressionVal)
        {
            Console.WriteLine(String.Concat(Enumerable.Repeat("  ", count)) + "expressionVal");
            count++;
            base.Visit(expressionVal);
            count--;
        }
        public override void Visit(IfStatement ifStatement)
        {
            Console.WriteLine(String.Concat(Enumerable.Repeat("  ", count)) + "ifStatement");
            count++;
            base.Visit(ifStatement);
            count--;
        }
        public override void Visit(ElseStatement elseStatement)
        {
            Console.WriteLine(String.Concat(Enumerable.Repeat("  ", count)) + "elseStatement");
            count++;
            base.Visit(elseStatement);
            count--;
        }
        public override void Visit(Constant constant)
        {
            Console.WriteLine(String.Concat(Enumerable.Repeat("  ", count)) + "constant");
            count++;
            base.Visit(constant);
            count--;
        }
    }
}
