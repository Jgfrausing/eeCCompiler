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
            Console.WriteLine(_sourceCode);
        }

        public override void Visit(Body body)
        {
            _sourceCode += "{\n";
            foreach (var bodypart in body.Bodyparts)
            {
                bodypart.Accept(this);
                
                if (bodypart is VarDecleration || bodypart is FuncCall || bodypart is Return)
                    _sourceCode += ";";
                _sourceCode += "\n";
            }
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
        public override void Visit(IfStatement ifStatement)
        {
            _sourceCode += "if ";
            base.Visit(ifStatement);
        }

        public override void Visit(ElseStatement elseStatement)
        {
            _sourceCode += "else\n";
            base.Visit(elseStatement);
        }

        public override void Visit(ExpressionNegate expressionNegate)
        {
            _sourceCode += "1";
            base.Visit(expressionNegate);
        }

        public override void Visit(ExpressionVal expressionVal)
        {
            base.Visit(expressionVal);
        }

        public override void Visit(Direction direction)
        {
            _sourceCode += direction.Incrementing ? " to " : " downto ";
        }

        public override void Visit(ExpressionParenOpExpr expressionParenOpExpr)
        {
            _sourceCode += expressionParenOpExpr.ToString();
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
            Console.WriteLine("rooteValOpExpr");
        }

        public override void Visit(ExpressionParen expressionParen)
        {
            _sourceCode += "(";
            Visit(expressionParen.Expression);
            _sourceCode += ")";
        }

        public override void Visit(ExpressionMinus expressionMinus)
        {
            Console.WriteLine("expressionMinus");
        }

        public override void Visit(ExpressionList expressionList)
        {
            Console.WriteLine("expressionList");
        }

        public override void Visit(StructDecleration structDecleration)
        {
            Console.WriteLine("structDecleration");
            
        }

        public override void Visit(RepeatExpr repeatExpr)
        {
            Console.WriteLine("repeatExpr");
        }

        public override void Visit(Type type)
        {
            Console.WriteLine("type");
        }

        public override void Visit(StringValue stringValue)
        {
            Console.WriteLine("stringValue");
        }

        public override void Visit(Operator operate)
        {
            _sourceCode += operate.ToString();
        }

        public override void Visit(Identifier identifier)
        {
            _sourceCode += identifier.ToString();
        }

        public override void Visit(BoolValue boolValue)
        {
            Console.WriteLine("boolValue");
        }

        public override void Visit(NumValue numValue)
        {
            Console.WriteLine("numValue");
        }

        public override void Visit(FuncCall funcCall)
        {
            Console.WriteLine("funcCall");
        }

        public override void Visit(FunctionDeclarations functionDeclarations)
        {
            Console.WriteLine("functionDeclarations");
        }

        public override void Visit(FunctionDeclaration functionDeclaration)
        {
            Console.WriteLine("functionDeclaration");
        }

        public override void Visit(Return _return)
        {
            Console.WriteLine("return");
        }

        public override void Visit(VarDecleration varDecleration)
        {
            Visit(varDecleration.Identifier);
            _sourceCode += "=";
            Visit(varDecleration.Expression);
        }
        public override void Visit(VarDeclerations varDeclerations)
        {
            foreach (var vardecl in varDeclerations.VarDeclerationList)
            {
                Visit(vardecl);
                _sourceCode += ";\n";
            }
        }
    }
}