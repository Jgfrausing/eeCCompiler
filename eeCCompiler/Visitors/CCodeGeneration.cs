using System.Linq;
using System.Security.Policy;
using eeCCompiler.Interfaces;
using eeCCompiler.Nodes;

namespace eeCCompiler.Visitors
{
    public class CCodeGeneration : IEecVisitor
    {
        private string _header { get; set; }
        private string _code { get; set; }

        public string CCode => _header + _code;

        private StructDefinitions _structDefinitions;

        public void Visit(Root root)
        {
            root.Includes.Accept(this);
            root.ConstantDefinitions.Accept(this);
            root.StructDefinitions.Accept(this);
            root.FunctionDeclarations.Accept(this);
            _code += "void main()";
            root.Program.Accept(this);
        }

        public void Visit(Body body)
        {
            _code += "{\n";
            foreach (var bodyPart in body.Bodyparts)
            {
                bodyPart.Accept(this);
                _code += "\n";
            }
            _code += "}\n";
        }

        public void Visit(Constant constant)
        {
            _header += $"#define {constant.Identifier.ToString().ToUpper()} {constant.ConstantPart}\n";
        }

        public void Visit(ConstantDefinitions constantDefinitions)
        {
            constantDefinitions.ConstantList.ForEach(x => x.Accept(this));
            _header += "\n";
        }

        public void Visit(ElseStatement elseStatement)
        {
            _code += "else";
            elseStatement.Body.Accept(this);
        }

        public void Visit(IfStatement ifStatement)
        {
            _code += "if(";
            ifStatement.Expression.Accept(this);
            _code += ")";
            ifStatement.Body.Accept(this);
            if (ifStatement.ElseStatement is IfStatement)
                _code += "else ";
        }

        public void Visit(ExpressionNegate expressionNegate)
        {
            _code += "!";
            expressionNegate.Expression.Accept(this);
        }

        public void Visit(ExpressionVal expressionVal)
        {
            expressionVal.Value.Accept(this);
        }

        public void Visit(Direction direction)
        {
            _code += direction.Incrementing ? "++" : "--";
        }

        public void Visit(ExpressionParenOpExpr expressionParenOpExpr)
        {
            expressionParenOpExpr.ExpressionParen.Accept(this);
            expressionParenOpExpr.Operator.Accept(this);
            expressionParenOpExpr.Expression.Accept(this);
        }

        public void Visit(ExpressionValOpExpr expressionValOpExpr)
        {
            expressionValOpExpr.Value.Accept(this); //måske til _code
            expressionValOpExpr.Operator.Accept(this);
            expressionValOpExpr.Expression.Accept(this);
        }

        public void Visit(ExpressionParen expressionParen)
        {
            _code += "(";
            expressionParen.Expression.Accept(this);
            _code += ")";
        }

        public void Visit(ExpressionMinus expressionMinus)
        {
            _code += "-";
            expressionMinus.Expression.Accept(this);
        }

        public void Visit(ExpressionList expressionList)
        {
            // hvad med ref?
            for (int i = 0; i < expressionList.Expressions.Count; i++)
            {
                if (i > 0)
                    _code += ",";
                expressionList.Expressions[i].Accept(this);
            }
        }

        public void Visit(StructDecleration structDecleration)
        {
            var myStruct = _structDefinitions.Definitions.FirstOrDefault(x => x.Identifier == structDecleration.StructIdentifier);
            foreach (var varDecl in myStruct.StructParts.StructPartList)
            {
                if(varDecl is VarDecleration && !structDecleration.VarDeclerations.VarDeclerationList.Contains(varDecl))
                    structDecleration.VarDeclerations.VarDeclerationList.Add(varDecl as VarDecleration);
            }
            _code += $"struct {structDecleration.StructIdentifier} {structDecleration.Identifier};\n";
            foreach (var varDecl in structDecleration.VarDeclerations.VarDeclerationList)
            {
                _code += $"{structDecleration.Identifier}.{varDecl.Identifier} ";
                varDecl.AssignmentOperator.Accept(this);
                varDecl.Expression.Accept(this);
                _code += ";\n";
            }
        }

        public void Visit(RepeatExpr repeatExpr)
        {
            _code += "while (";
            repeatExpr.Expression.Accept(this);
            _code += ")";
            repeatExpr.Body.Accept(this);
        }

        public void Visit(RepeatFor repeatFor)
        {
            _code += "for (" + repeatFor.VarDecleration.Identifier + " = ";
            repeatFor.VarDecleration.Expression.Accept(this);
            _code += ";";
            repeatFor.Expression.Accept(this);
            _code += ";" + repeatFor.VarDecleration.Identifier;
            repeatFor.Direction.Accept(this);
            _code += ")";
            repeatFor.Body.Accept(this);
        }

        public void Visit(Type type)
        {
            string cType;
            switch (type.ValueType)
            {
                case "num":
                case "bool":
                    cType = "int";
                    break;
                case "string":
                    cType = "char *";
                    break;
                default:
                    cType = "struct " + type.ValueType;
                    break;
            }
            _code += $" {cType} ";
        }

        public void Visit(StringValue stringValue)
        {
            _code += $" {stringValue} ";
        }

        public void Visit(Operator operate)
        {
            string opr = "";
            switch (operate.Symbol)
            {
                case Indexes.Indexes.SymbolIndex.Eq:
                    opr = "=";
                    break;
            }
            _code += $" {opr} ";
        }

        public void Visit(Identifier identifier)
        {
            _code += $" {identifier} ";
        }

        public void Visit(BoolValue boolValue)
        {
            var boolVal = boolValue.Value ? "1" : "0";
            _code += $" {boolVal} ";
        }

        public void Visit(NumValue numValue)
        {
            _code += $" {numValue} ";
        }

        public void Visit(FuncCall funcCall)
        {
            _code += $"{funcCall.Identifier}(";
            for (int i = 0; i < funcCall.Expressions.Count; i++)
            {
                if (i > 0)
                    _code += ",";
                funcCall.Expressions[i].Accept(this);
            }
            _code += ")";
        }

        public void Visit(FunctionDeclarations functionDeclarations)
        {
            functionDeclarations.FunctionDeclaration.ForEach(funcDecl => funcDecl.Accept(this));
        }

        public void Visit(FunctionDeclaration functionDeclaration)
        {
            functionDeclaration.TypeId.Accept(this);
            _code += "(";
            functionDeclaration.Parameters.Accept(this);
            _code += ")";
            functionDeclaration.Body.Accept(this);
            _code += "\n";
        }

        public void Visit(Return returnNode)
        {
            _code += "return ";
            returnNode.Expression.Accept(this);
            _code += ";";
        }

        public void Visit(Refrence referece)
        {
            referece.Identifier.Accept(this);
            referece.StructRefrence.Accept(this);
        }

        public void Visit(VarDeclerations varDecls)
        {
            varDecls.VarDeclerationList.ForEach(varDecl => varDecls.Accept(this));
        }

        public void Visit(VarDecleration varDecl)
        {
            if (varDecl.IsFirstUse)
                _code += varDecl.Type + " ";
            _code += $"{varDecl.Identifier} ";
            varDecl.AssignmentOperator.Accept(this);
            _code += " ";
            varDecl.Expression.Accept(this);
            _code += ";";
        }

        public void Visit(StructDefinition structDef)
        {
            _code += $"struct {structDef.Identifier} " + "{\n";
            structDef.StructParts.Accept(this);
            _code += "\n};\n";
        }

        public void Visit(StructParts structParts)
        {
            structParts.StructPartList.ForEach(structPart => structPart.Accept(this));
        }

        public void Visit(StructDefinitions structDefinitions)
        {
            structDefinitions.Definitions.ForEach(structDef => structDef.Accept(this));
            _code += "\n";
        }

        public void Visit(AssignmentOperator assignOpr)
        {
            _code += "ASSIGNMENTOPERATOR!!!";
        }

        public void Visit(Include include)
        {
            _code += "INCLUDE!!!";
        }

        public void Visit(Includes includes)
        {
            includes.IncludeList.ForEach(include => include.Accept(this));
        }

        public void Visit(ListIndex listIndex)
        {
            foreach (var index in listIndex.Indexes)
            {
                _code += "[";
                index.Accept(this);
                _code += "]";
            }
        }

        public void Visit(Ref refNode)
        {
            _code += "REFNODE!!!";
        }

        public void Visit(ListType listType)
        {
            _code += "LISTTYPE!!!";
        }

        public void Visit(RefId refId)
        {
            _code += $"ref {refId.Identifier}";
        }

        public void Visit(RefTypeId refTypeId)
        {
            _code += $"ref ";
            refTypeId.TypeId.Accept(this);
        }

        public void Visit(ListDimentions listDimentions)
        {
            for (int i = 0; i < listDimentions.Dimentions; i++)
            {
                _code += "[]";
            }
        }

        public void Visit(TypeId typeId)
        {
            typeId.ValueType.Accept(this);
            _code += " " + typeId.Identifier;
        }

        public void Visit(TypeIdList typeIdList)
        {
            typeIdList.TypeIds.ForEach(x => x.Accept(this));
        }

        public void Visit(IdIndex idIndex)
        {
            idIndex.Identifier.Accept(this);
            idIndex.ListIndex.Accept(this);
        }
    }
}