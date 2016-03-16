using eeCCompiler.Interfaces;
using eeCCompiler.Nodes;

namespace eeCCompiler.Visitors
{
    public abstract class Abstractvisitor : IEecVisitor
    {
        public void Visit(Root root)
        {
            throw new System.NotImplementedException();
        }

        public void Visit(Body body)
        {
            throw new System.NotImplementedException();
        }

        public void Visit(Constant constant)
        {
            throw new System.NotImplementedException();
        }

        public void Visit(ConstantDefinitions constantDefinitions)
        {
            throw new System.NotImplementedException();
        }

        public void Visit(ElseStatement elseStatement)
        {
            throw new System.NotImplementedException();
        }

        public void Visit(ExpressionNegate expressionNegate)
        {
            throw new System.NotImplementedException();
        }

        public void Visit(ExpressionVal expressionVal)
        {
            throw new System.NotImplementedException();
        }

        public void Visit(Direction direction)
        {
            throw new System.NotImplementedException();
        }

        public void Visit(ExpressionParenOpExpr expressionParenOpExpr)
        {
            throw new System.NotImplementedException();
        }

        public void Visit(ExpressionValOpExpr rooteValOpExpr)
        {
            throw new System.NotImplementedException();
        }

        public void Visit(ExpressionParen expressionParen)
        {
            throw new System.NotImplementedException();
        }

        public void Visit(ExpressionMinus expressionMinus)
        {
            throw new System.NotImplementedException();
        }

        public void Visit(ExpressionList expressionList)
        {
            throw new System.NotImplementedException();
        }

        public void Visit(StructDecleration structDecleration)
        {
            throw new System.NotImplementedException();
        }

        public void Visit(RepeatExpr repeatExpr)
        {
            throw new System.NotImplementedException();
        }

        public void Visit(Type type)
        {
            throw new System.NotImplementedException();
        }

        public void Visit(StringValue stringValue)
        {
            throw new System.NotImplementedException();
        }

        public void Visit(Operator operate)
        {
            throw new System.NotImplementedException();
        }

        public void Visit(Identifier identifier)
        {
            throw new System.NotImplementedException();
        }

        public void Visit(BoolValue boolValue)
        {
            throw new System.NotImplementedException();
        }

        public void Visit(NumValue numValue)
        {
            throw new System.NotImplementedException();
        }

        public void Visit(FuncCall funcCall)
        {
            throw new System.NotImplementedException();
        }

        public void Visit(FunctionDeclarations functionDeclarations)
        {
            throw new System.NotImplementedException();
        }

        public void Visit(FunctionDeclaration functionDeclaration)
        {
            throw new System.NotImplementedException();
        }

        public void Visit(Return expressionParenOpExpr)
        {
            throw new System.NotImplementedException();
        }
    }
}