using eeCCompiler.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eeCCompiler.Nodes;

namespace eeCCompiler.Visitors
{
    public abstract class Visitor : IEecVisitor
    {
        public void Visit(Constant constant)
        {
            constant.Identifier.Accept(this);
            constant.ConstantPart.Accept(this);
        }

        public void Visit(ElseStatement elseStatement)
        {
            elseStatement.Body.Accept(this);
        }

        public void Visit(ExpressionVal expressionVal)
        {
            expressionVal.Value.Accept(this);
        }

        public void Visit(ExpressionParenOpExpr expressionParenOpExpr)
        {
            expressionParenOpExpr.ExpressionParen.Accept(this);
            expressionParenOpExpr.Operator.Accept(this);
            expressionParenOpExpr.Expression.Accept(this);
        }

        public void Visit(ExpressionParen expressionParen)
        {
            expressionParen.Expression.Accept(this);
        }

        public void Visit(ExpressionList expressionList)
        {
            foreach (var expression in expressionList.Expressions)
            {
                expression.Accept(this);
            }
        }

        public void Visit(RepeatExpr repeatExpr)
        {
            repeatExpr.Expression.Accept(this);
            repeatExpr.Body.Accept(this);
        }

        public void Visit(StringValue stringValue)
        {
        }

        public void Visit(Identifier identifier)
        {
        }

        public void Visit(NumValue numValue)
        {
        }

        public void Visit(FunctionDeclarations functionDeclarations)
        {
            foreach (var funcdecl in functionDeclarations.FunctionDeclaration)
            {
                funcdecl.Accept(this);
            }
        }

        public void Visit(Return expressionParenOpExpr)
        {
            expressionParenOpExpr.Expression.Accept(this);
        }

        public void Visit(FunctionDeclaration functionDeclaration)
        {
            functionDeclaration.TypeId.Accept(this);
            functionDeclaration.Parameters.Accept(this);
            functionDeclaration.Body.Accept(this);
        }

        public void Visit(FuncCall funcCall)
        {
            funcCall.Identifier.Accept(this);
            foreach (var expression in funcCall.Expressions)
            {
                expression.Accept(this);
            }
        }

        public void Visit(BoolValue boolValue)
        {
        }

        public void Visit(Operator operate)
        {
        }

        public void Visit(Nodes.Type type)
        {
        }

        public void Visit(StructDecleration structDecleration)
        {
            structDecleration.Identifier.Accept(this);
            structDecleration.StructIdentifier.Accept(this);
            structDecleration.VarDeclerations.Accept(this);
        }

        public void Visit(ExpressionMinus expressionMinus)
        {
            expressionMinus.Expression.Accept(this);
        }

        public void Visit(ExpressionValOpExpr rooteValOpExpr)
        {
            rooteValOpExpr.Value.Accept(this);
            rooteValOpExpr.Operator.Accept(this);
            rooteValOpExpr.Expression.Accept(this);
        }

        public void Visit(Direction direction)
        {
        }

        public void Visit(ExpressionNegate expressionNegate)
        {
            expressionNegate.Expression.Accept(this);
        }

        public void Visit(ConstantDefinitions constantDefinitions)
        {
            foreach (var cont in constantDefinitions.ConstantList)
            {
                cont.Accept(this);
            }
        }

        public void Visit(Body body)
        {
            foreach (var bodypart in body.Bodyparts)
            {
                bodypart.Accept(this);
            }
        }

        public void Visit(Root root)
        {
            root.ConstantDefinitions.Accept(this);
            root.StructDefinitions.Accept(this);
            root.Program.Accept(this);
            root.FunctionDeclarations.Accept(this);
        }
    }
}
