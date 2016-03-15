using eeCCompiler.Nodes;

namespace eeCCompiler.Interfaces
{
    public interface IEecVisitor
    {
        void Visit(Root root);
        void Visit(Body body);
        void Visit(Constant constant);
        void Visit(ConstantDefinitions constantDefinitions);
        void Visit(ElseStatement elseStatement);
        void Visit(ExpressionNegate expressionNegate);
        void Visit(ExpressionVal expressionVal);
        void Visit(Direction direction);
        void Visit(ExpressionParenOpExpr expressionParenOpExpr);
        void Visit(ExpressionValOpExpr rooteValOpExpr);
        void Visit(ExpressionParen expressionParen);
        void Visit(ExpressionMinus expressionMinus);
        void Visit(ExpressionList expressionList);
        void Visit(StructDecleration structDecleration);
        void Visit(RepeatExpr repeatExpr);
        void Visit(Type type);
        void Visit(StringValue stringValue);
        void Visit(Operator operate);
        void Visit(Identifier identifier);
        void Visit(BoolValue boolValue);
        void Visit(NumValue numValue);
        void Visit(FuncCall funcCall);
        void Visit(FunctionDeclarations functionDeclarations);
        void Visit(FunctionDeclaration functionDeclaration);
        void Visit(Return expressionParenOpExpr);
    }
}