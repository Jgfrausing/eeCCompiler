using eeCCompiler.Interfaces;
using eeCCompiler.Nodes;

namespace eeCCompiler.Visitors
{
    public abstract class Visitor : IEecVisitor
    {
        public virtual void Visit(Constant constant)
        {
            constant.Identifier.Accept(this);
            constant.ConstantPart.Accept(this);
        }

        public virtual void Visit(ElseStatement elseStatement)
        {
            elseStatement.Body.Accept(this);
        }

        public virtual void Visit(IfStatement ifStatement)
        {
            ifStatement.Expression.Accept(this);
            ifStatement.Body.Accept(this);
            ifStatement.ElseStatement.Accept(this);
        }

        public virtual void Visit(ExpressionVal expressionVal)
        {
            expressionVal.Value.Accept(this);
        }

        public virtual void Visit(ExpressionParenOpExpr expressionParenOpExpr)
        {
            expressionParenOpExpr.ExpressionParen.Accept(this);
            expressionParenOpExpr.Operator.Accept(this);
            expressionParenOpExpr.Expression.Accept(this);
        }

        public virtual void Visit(ExpressionParen expressionParen)
        {
            expressionParen.Expression.Accept(this);
        }

        public virtual void Visit(ExpressionList expressionList)
        {
            foreach (var expression in expressionList.Expressions)
            {
                expression.Accept(this);
            }
        }

        public virtual void Visit(RepeatExpr repeatExpr)
        {
            repeatExpr.Expression.Accept(this);
            repeatExpr.Body.Accept(this);
        }

        public virtual void Visit(StringValue stringValue)
        {
        }

        public virtual void Visit(Identifier identifier)
        {
        }

        public virtual void Visit(NumValue numValue)
        {
        }

        public virtual void Visit(FunctionDeclarations functionDeclarations)
        {
            foreach (var funcdecl in functionDeclarations.FunctionDeclaration)
            {
                funcdecl.Accept(this);
            }
        }

        public virtual void Visit(Return expressionParenOpExpr)
        {
            expressionParenOpExpr.Expression.Accept(this);
        }

        public virtual void Visit(Refrence reference)
        {
            foreach (var id in reference.Identifiers)
            {
                id.Accept(this);
            }
            reference.StructRefrence.Accept(this);
        }

        public virtual void Visit(VarDeclerations varDecls)
        {
            foreach (var varDecl in varDecls.VarDeclerationList)
            {
                varDecl.Accept(this);
            }
        }

        public virtual void Visit(VarDecleration varDecleration)
        {
            varDecleration.Identifier.Accept(this);
            varDecleration.AssignmentOperator.Accept(this);
            varDecleration.Expression.Accept(this);
        }

        public virtual void Visit(StructDefinition structDefinition)
        {
            structDefinition.Identifier.Accept(this);
            structDefinition.StructParts.Accept(this);
        }

        public virtual void Visit(StructParts structParts)
        {
            foreach (var structpart in structParts.StructPartList)
            {
                structpart.Accept(this);
            }
        }

        public virtual void Visit(StructDefinitions structDefinitions)
        {
            foreach (var structdefi in structDefinitions.Definitions)
            {
                structdefi.Accept(this);
            }
        }

        public virtual void Visit(AssignmentOperator assignmentOperator)
        {
        }

        public virtual void Visit(Include include)
        {
        }

        public virtual void Visit(Includes includes)
        {
            foreach (var include in includes.IncludeList)
            {
                include.Accept(this);
            }
        }

        public virtual void Visit(FunctionDeclaration functionDeclaration)
        {
            functionDeclaration.TypeId.Accept(this);
            functionDeclaration.Parameters.Accept(this);
            functionDeclaration.Body.Accept(this);
        }

        public virtual void Visit(FuncCall funcCall)
        {
            funcCall.Identifier.Accept(this);
            foreach (var expression in funcCall.Expressions)
            {
                expression.Accept(this);
            }
        }

        public virtual void Visit(BoolValue boolValue)
        {
        }

        public virtual void Visit(Operator operate)
        {
        }

        public virtual void Visit(Type type)
        {
        }

        public virtual void Visit(StructDecleration structDecleration)
        {
            structDecleration.Identifier.Accept(this);
            structDecleration.AssignmentOperator.Accept(this);
            structDecleration.StructIdentifier.Accept(this);
            structDecleration.VarDeclerations.Accept(this);
        }

        public virtual void Visit(ExpressionMinus expressionMinus)
        {
            expressionMinus.Expression.Accept(this);
        }

        public virtual void Visit(ExpressionValOpExpr expressionValOpExpr)
        {
            expressionValOpExpr.Value.Accept(this);
            expressionValOpExpr.Operator.Accept(this);
            expressionValOpExpr.Expression.Accept(this);
        }

        public virtual void Visit(Direction direction)
        {
        }

        public virtual void Visit(ExpressionNegate expressionNegate)
        {
            expressionNegate.Expression.Accept(this);
        }

        public virtual void Visit(ConstantDefinitions constantDefinitions)
        {
            foreach (var constDef in constantDefinitions.ConstantList)
            {
                constDef.Accept(this);
            }
        }

        public virtual void Visit(Body body)
        {
            foreach (var bodypart in body.Bodyparts)
            {
                bodypart.Accept(this);
            }
        }

        public virtual void Visit(Root root)
        {
            root.Includes.Accept(this);
            root.ConstantDefinitions.Accept(this);
            root.StructDefinitions.Accept(this);
            root.Program.Accept(this);
            root.FunctionDeclarations.Accept(this);
        }

        public virtual void Visit(RepeatFor repeatFor)
        {
            repeatFor.VarDecleration.Accept(this);
            repeatFor.Direction.Accept(this);
            repeatFor.Expression.Accept(this);
            repeatFor.Body.Accept(this);
        }
    }
}