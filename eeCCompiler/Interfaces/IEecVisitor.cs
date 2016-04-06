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
        void Visit(IfStatement ifStatement);
        void Visit(ExpressionNegate expressionNegate);
        void Visit(ExpressionVal expressionVal);
        void Visit(Direction direction);
        void Visit(ExpressionParenOpExpr expressionParenOpExpr);
        void Visit(ExpressionValOpExpr expressionValOpExpr);
        void Visit(ExpressionParen expressionParen);
        void Visit(ExpressionMinus expressionMinus);
        void Visit(ExpressionList expressionList);
        void Visit(StructDecleration structDecleration);
        void Visit(RepeatExpr repeatExpr);
        void Visit(RepeatFor repeatFor);
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
        void Visit(Refrence expressionParenOpExpr);
        void Visit(VarDeclerations expressionParenOpExpr);
        void Visit(VarDecleration expressionParenOpExpr);
        void Visit(StructDefinition expressionParenOpExpr);
        void Visit(StructParts expressionParenOpExpr);
        void Visit(StructDefinitions structDefinitions);
        void Visit(AssignmentOperator expressionParenOpExpr);
        void Visit(Include expressionParenOpExpr);
        void Visit(Includes includes);
        void Visit(ListIndex expressionParenOpExpr);
        void Visit(Ref expressionParenOpExpr);
    }
}