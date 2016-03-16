using System;
using System.Linq;
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
            root.Includes.Accept(this);
            root.ConstantDefinitions.Accept(this);
            root.StructDefinitions.Accept(this);
            _sourceCode += "program";
            root.Program.Accept(this);
            root.FunctionDeclarations.Accept(this);
            Console.WriteLine(_sourceCode);
        }
        public override void Visit(StructDefinition structDefinition)
        {
            _sourceCode += "struct ";
            structDefinition.Identifier.Accept(this);
            _sourceCode += "{\n";
            structDefinition.StructParts.Accept(this);
            _sourceCode += "\n}\n";
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
            constant.Identifier.Accept(this);
            _sourceCode += " ";
            constant.ConstantPart.Accept(this);
            _sourceCode += ";\n";
        }

        public override void Visit(ConstantDefinitions constantDefinitions)
        {
            base.Visit(constantDefinitions);
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
            _sourceCode += direction.ToString();
        }

        public override void Visit(ExpressionParenOpExpr expressionParenOpExpr)
        {
            _sourceCode += expressionParenOpExpr.ToString();
            
            expressionParenOpExpr.ExpressionParen.Accept(this);
            expressionParenOpExpr.Operator.Accept(this);
            expressionParenOpExpr.Expression.Accept(this);
        }

        private void Visit(IExpression expression)
        {
            _sourceCode += expression.ToString();
        }

        public override void Visit(ExpressionValOpExpr expressionValOpExpr)
        {
            base.Visit(expressionValOpExpr);
        }

        public override void Visit(ExpressionParen expressionParen)
        {
            _sourceCode += "(";
            expressionParen.Expression.Accept(this);
            _sourceCode += ")";
        }

        public override void Visit(ExpressionMinus expressionMinus)
        {
            base.Visit(expressionMinus);
        }
        public override void Visit(Include include)
        {
            _sourceCode += "include ";
            if (include.Identifiers.Any())
                include.Identifiers[0].Accept(this);
            for (int i = 1; i < include.Identifiers.Count; i++)
            {
                _sourceCode += ".";
                include.Identifiers[i].Accept(this);
            }
            _sourceCode += "\n";
        }

        public override void Visit(Includes includes)
        {
            base.Visit(includes);
        }

        public override void Visit(ExpressionList expressionList)
        {
            foreach (var expression in expressionList.Expressions)
            {
                expression.Accept(this);
                _sourceCode += ", ";
            }
        }

        public override void Visit(StructDecleration structDecleration)
        {
            structDecleration.Identifier.Accept(this);
            structDecleration.AssignmentOperator.Accept(this);
            structDecleration.StructIdentifier.Accept(this); 
            _sourceCode += "{";
            structDecleration.VarDeclerations.Accept(this);
            _sourceCode += "};";
        }

        public override void Visit(RepeatExpr repeatExpr)
        {
            _sourceCode += "repeat ";
            base.Visit(repeatExpr);
        }
        public override void Visit(RepeatFor repeatFor)
        {
            
            repeatFor.VarDecleration.Accept(this);
            repeatFor.Direction.Accept(this);
            repeatFor.Expression.Accept(this);
            repeatFor.Body.Accept(this);
            
        }

        public override void Visit(Type type)
        {
            _sourceCode += type + " ";
        }

        public override void Visit(StringValue stringValue)
        {
            _sourceCode += stringValue.ToString();
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
            _sourceCode += boolValue.ToString();
        }

        public override void Visit(NumValue numValue)
        {
            _sourceCode += numValue.ToString();
        }

        public override void Visit(FuncCall funcCall)
        {
            funcCall.Identifier.Accept(this);
            _sourceCode += "(";

            if(funcCall.Expressions.Count>0)
                funcCall.Expressions[0].Accept(this);

            for (int i = 1; i < funcCall.Expressions.Count; i++)
            {
                _sourceCode += ", ";
                funcCall.Expressions[i].Accept(this);
            }
            _sourceCode += ")";
        }

        public override void Visit(FunctionDeclarations functionDeclarations)
        {
            base.Visit(functionDeclarations);
        }

        public override void Visit(FunctionDeclaration functionDeclaration)
        {
            base.Visit(functionDeclaration);
        }

        public override void Visit(Return _return)
        {
            _sourceCode += "return ";
            base.Visit(_return);
        }
        public override void Visit(Refrence reference)
        {
            base.Visit(reference);
        }
        public override void Visit(VarDecleration varDecleration)
        {
            base.Visit(varDecleration);
        }
        public override void Visit(VarDeclerations varDeclerations)
        {
            _sourceCode += " ";
            foreach (var vardecl in varDeclerations.VarDeclerationList)
            {
                Visit(vardecl);
                _sourceCode += "; ";
            }
        }

        public override void Visit(AssignmentOperator assignmentOperator)
        {
            Visit(assignmentOperator);
        }
    }
}