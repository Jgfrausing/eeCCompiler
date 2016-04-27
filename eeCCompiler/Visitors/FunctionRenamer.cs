using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eeCCompiler.Nodes;
using Type = eeCCompiler.Nodes.Type;

namespace eeCCompiler.Visitors
{
    class FunctionRenamer : Visitor
    {
        public override void Visit(Root root)
        {
            root.StructDefinitions.Accept(this);
            root.FunctionDeclarations.Accept(this);
            root.Program.Accept(this);
        }

        public override void Visit(Body body)
        {
            base.Visit(body);
        }

        public override void Visit(ElseStatement elseStatement)
        {
base.Visit();
        }

        public override void Visit(IfStatement ifStatement)
        {
            throw new NotImplementedException();
        }

        public override void Visit(ExpressionNegate expressionNegate)
        {
            throw new NotImplementedException();
        }

        public override void Visit(ExpressionVal expressionVal)
        {
            throw new NotImplementedException();
        }

        public override void Visit(Direction direction)
        {
            throw new NotImplementedException();
        }

        public override void Visit(ExpressionParenOpExpr expressionParenOpExpr)
        {
            throw new NotImplementedException();
        }

        public override void Visit(ExpressionValOpExpr expressionValOpExpr)
        {
            throw new NotImplementedException();
        }

        public override void Visit(ExpressionParen expressionParen)
        {
            throw new NotImplementedException();
        }

        public override void Visit(ExpressionMinus expressionMinus)
        {
            throw new NotImplementedException();
        }

        public override void Visit(ExpressionList expressionList)
        {
            throw new NotImplementedException();
        }

        public override void Visit(StructDecleration structDecleration)
        {
            throw new NotImplementedException();
        }

        public override void Visit(RepeatExpr repeatExpr)
        {
            throw new NotImplementedException();
        }

        public override void Visit(RepeatFor repeatFor)
        {
            throw new NotImplementedException();
        }

        public override void Visit(Type type)
        {
            throw new NotImplementedException();
        }

        public override void Visit(StringValue stringValue)
        {
            throw new NotImplementedException();
        }

        public override void Visit(Operator operate)
        {
            throw new NotImplementedException();
        }

        public override void Visit(Identifier identifier)
        {
            throw new NotImplementedException();
        }

        public override void Visit(BoolValue boolValue)
        {
            throw new NotImplementedException();
        }

        public override void Visit(NumValue numValue)
        {
            throw new NotImplementedException();
        }

        public override void Visit(FuncCall funcCall)
        {
            throw new NotImplementedException();
        }

        public override void Visit(FunctionDeclarations functionDeclarations)
        {
            throw new NotImplementedException();
        }

        public override void Visit(FunctionDeclaration functionDeclaration)
        {
            throw new NotImplementedException();
        }

        public override void Visit(Return expressionParenOpExpr)
        {
            throw new NotImplementedException();
        }

        public override void Visit(Refrence expressionParenOpExpr)
        {
            throw new NotImplementedException();
        }

        public override void Visit(VarDeclerations expressionParenOpExpr)
        {
            throw new NotImplementedException();
        }

        public override void Visit(VarDecleration expressionParenOpExpr)
        {
            throw new NotImplementedException();
        }

        public override void Visit(StructDefinition expressionParenOpExpr)
        {
            throw new NotImplementedException();
        }

        public override void Visit(StructParts expressionParenOpExpr)
        {
            throw new NotImplementedException();
        }

        public override void Visit(StructDefinitions structDefinitions)
        {
            throw new NotImplementedException();
        }

        public override void Visit(AssignmentOperator expressionParenOpExpr)
        {
            throw new NotImplementedException();
        }

        public override void Visit(Include expressionParenOpExpr)
        {
            throw new NotImplementedException();
        }

        public override void Visit(Includes includes)
        {
            throw new NotImplementedException();
        }

        public override void Visit(ListIndex expressionParenOpExpr)
        {
            throw new NotImplementedException();
        }

        public override void Visit(Ref expressionParenOpExpr)
        {
            throw new NotImplementedException();
        }

        public override void Visit(ListType expressionParenOpExpr)
        {
            throw new NotImplementedException();
        }

        public override void Visit(RefId expressionParenOpExpr)
        {
            throw new NotImplementedException();
        }

        public override void Visit(RefTypeId expressionParenOpExpr)
        {
            throw new NotImplementedException();
        }

        public override void Visit(ListDimentions expressionParenOpExpr)
        {
            throw new NotImplementedException();
        }

        public override void Visit(TypeId expressionParenOpExpr)
        {
            throw new NotImplementedException();
        }

        public override void Visit(TypeIdList expressionParenOpExpr)
        {
            throw new NotImplementedException();
        }

        public override void Visit(IdIndex expressionParenOpExpr)
        {
            throw new NotImplementedException();
        }

        public override void Visit(VarInStructDecleration expressionParenOpExpr)
        {
            throw new NotImplementedException();
        }
    }
}
