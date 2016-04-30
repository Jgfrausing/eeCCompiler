using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text.RegularExpressions;
using eeCCompiler.Interfaces;
using eeCCompiler.Nodes;
using eeCCompiler.Visitors.CCode;
using Type = eeCCompiler.Nodes.Type;

namespace eeCCompiler.Visitors
{
    public class CCodeGeneration : IEecVisitor
    {
        private string _header { get; set; }
        private string _code { get; set; }
        private readonly DefaultCCode _defaultCCode = new DefaultCCode();
        private readonly Stack<List<Identifier>> _heapIdentifiers = new Stack<List<Identifier>>();

        public string CCode => _header + _code;

        private Root _root;

        public void Visit(Root root)    
        {
            _header += _defaultCCode.GetIncludes();
            _root = root;
            root.Includes.Accept(this);
            root.ConstantDefinitions.Accept(this);
            _header +=_code;
            _code = "";
            root.StructDefinitions.Accept(this);
            

            /////////
            //_header += _defaultCCode.GenerateListTypeHeader("numlist", "double", false);
            //_header += _defaultCCode.GenerateListTypeHeader("boollist", "int", false);
            _header += _defaultCCode.GenerateListTypeHeader("string", "char", false);
            //_header += _defaultCCode.GenerateListTypeHeader("string_handle", "string_handle", true);

            //_code += _defaultCCode.GenerateListTypeCode("numlist", "double", false);
            //_code += _defaultCCode.GenerateListTypeCode("boollist", "int", false);
            _code += _defaultCCode.GenerateListTypeCode("string", "char", false);
            //_code += _defaultCCode.GenerateListTypeCode("string_handle", "string_handle", true);
            _header += _defaultCCode.StandardFunctionsHeader();
            _code += _defaultCCode.StandardFunctionsCode();

            foreach (var structDefinition in root.StructDefinitions.Definitions)
            {
                _header += _defaultCCode.GenerateListTypeHeader(structDefinition.Identifier.Id + "list", structDefinition.Identifier.Id, true);
                _code += _defaultCCode.GenerateListTypeCode(structDefinition.Identifier.Id + "list", structDefinition.Identifier.Id, true);
            }
            ////////
            root.FunctionDeclarations.Accept(this);
            _code += "void main()";
            root.Program.Accept(this);
        }

        public void Visit(Body body)
        {
            _heapIdentifiers.Push(new List<Identifier>());
            _code += "{\n";
            foreach (var bodyPart in body.Bodyparts)
            {
                bodyPart.Accept(this);
                _code += "\n";
            }
            foreach (var heapIdentifier in _heapIdentifiers.Pop())
            {
                _code += $"free({heapIdentifier});\n";
            }
            _code += "}\n";
        }

        public void Visit(Constant constant)
        {
            _code += $"#define {constant.Identifier.ToString().ToUpper()} {constant.ConstantPart}\n";
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
        //MANDAGS ARBEJDE! HAVE FUN!
        public void Visit(ExpressionVal expressionVal)
        {
            //if (expressionVal.Value is Refrence &&
            //    (expressionVal.Value as Refrence).Identifier.ToString().Contains("("))
            //{
            //    (expressionVal.Value as Refrence).IsFuncCall = true; }

            if (expressionVal.Value is Refrence && (expressionVal.Value as Refrence).IsFuncCall)
            {
                var reference = expressionVal.Value as Refrence;
                 //_code += reference.FuncsStruct + "_";
                string strucCallOrder = reference.StructRefrence.ToString();
                while (!(reference.Identifier is FuncCall))
                {
                    reference = reference.Identifier as Refrence;
                    strucCallOrder += "->" + reference.StructRefrence;
                }
                (reference.Identifier as FuncCall).Expressions.Insert(0, new Identifier(strucCallOrder));
                (reference.Identifier as FuncCall).Accept(this);
            }
            else if (expressionVal.Value is Refrence)
            {
                var reference = expressionVal.Value as Refrence;
                string strucCallOrder = reference.StructRefrence.ToString();
                while (!(reference.Identifier is Identifier))
                {
                    reference = (reference.Identifier as Refrence);
                    strucCallOrder += "->";
                    reference.StructRefrence.Accept(this);
                }

                _code += strucCallOrder + "->" + reference.Identifier;
            }
            else
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
            var myStruct = _root.StructDefinitions.Definitions.FirstOrDefault(x => x.Identifier.ToString() == structDecleration.StructIdentifier.ToString());
            foreach (var varDecl in myStruct.StructParts.StructPartList)
            {
                if (varDecl is VarDecleration && !structDecleration.VarDeclerations.VarDeclerationList.Contains(varDecl))
                    structDecleration.VarDeclerations.VarDeclerationList.Add(varDecl as VarDecleration);
            }
            _code += $"{structDecleration.StructIdentifier} *{structDecleration.Identifier} = malloc(sizeof({structDecleration.Identifier}));\n";
            _heapIdentifiers.Peek().Add(structDecleration.Identifier);
            foreach (var varDecl in structDecleration.VarDeclerations.VarDeclerationList)
            {
                _code += $"{structDecleration.Identifier}->{varDecl.Identifier.Id} ";
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
            var cType = GetValueType(type);
            _code += $"{cType}";
        }

        private static string GetValueType(IType type)
        {
            string cType;
            switch (type.ToString())
            {
                case "num":
                    cType = "double";
                    break;
                case "bool":
                    cType = "int";
                    break;
                case "string":
                    cType = "string_handle";
                    break;
                default:
                    cType = type.ToString();
                    break;
            }
            return cType;
        }

        public void Visit(StringValue stringValue)
        {
            _code += $" {stringValue} ";
        }

        public void Visit(Operator operate)
        {
            string opr = "OPERATOR!!!";
            switch (operate.Symbol)
            {
                case Indexes.Indexes.SymbolIndex.Eqeq:
                    opr = "==";
                    break;
                case Indexes.Indexes.SymbolIndex.Minus:
                    opr = "-";
                    break;
                case Indexes.Indexes.SymbolIndex.Exclameq:
                    opr = "!=";
                    break;
                case Indexes.Indexes.SymbolIndex.Times:
                    opr = "*";
                    break;
                case Indexes.Indexes.SymbolIndex.Div:
                    opr = "/";
                    break;
                case Indexes.Indexes.SymbolIndex.Lt:
                    opr = "<";
                    break;
                case Indexes.Indexes.SymbolIndex.Lteq:
                    opr = "<=";
                    break;
                case Indexes.Indexes.SymbolIndex.Gt:
                    opr = ">";
                    break;
                case Indexes.Indexes.SymbolIndex.Gteq:
                    opr = ">=";
                    break;
                case Indexes.Indexes.SymbolIndex.And:
                    opr = "&&";
                    break;
                case Indexes.Indexes.SymbolIndex.Or:
                    opr = "||";
                    break;
                case Indexes.Indexes.SymbolIndex.Mod:
                    opr = "%";
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
            #region Print
            if (funcCall.Identifier.Id == "program_print")
            {
                foreach (var element in ((funcCall.Expressions[0] as ExpressionVal).Value as StringValue).Elements)
                {
                    CreateFunctions(element, "standard_print");
                    if (element is StringValue)
                    {
                         if ((element as StringValue).Value != "")
                            _code += $"(\"{(element as StringValue).Value}\");";
                    }
                    else if (element is TypeId)
                    {
                        _code += $"({(element as TypeId).Identifier});";
                    }
                }
            }
                #endregion
            #region Convert
            else if (funcCall.Identifier.Id == "program_convertStringToBool" 
                || funcCall.Identifier.Id == "program_convertStringToNum")
            {
                _code += $"{funcCall.Identifier}({funcCall.Expressions[0]}, &{funcCall.Expressions})";
            }
            else if (funcCall.Identifier.Id == "program_convertBoolToString" 
                || funcCall.Identifier.Id == "program_convertNumToString")
            {
                _code += $"{funcCall.Identifier}({funcCall.Expressions[0]}, {(funcCall.Expressions[1] as RefId).Identifier})";
            }
            #endregion
            #region User functions
            else
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
            if (funcCall.IsBodyPart)
                _code += ";";
            #endregion
        }

        private void CreateFunctions(IStringPart element, string stdprt)
        {
            if (element is StringValue)
            {
                if ((element as StringValue).Value != "")
                    _code += $"{stdprt}Chars";
            }
            else if (element is TypeId)
            {
                var typeId = (element as TypeId);
                var type = typeId.ValueType as eeCCompiler.Nodes.Type;
                switch (type.ValueType)
                {
                    case "string":
                        _code += $"{stdprt}String";
                        break;
                    case "num":
                        _code += $"{stdprt}Num";
                        break;
                    case "bool":
                        _code += $"{stdprt}Bool";
                        break;
                }
            }
        }

        public void Visit(FunctionDeclarations functionDeclarations)
        {
            foreach (var funcDecl in functionDeclarations.FunctionDeclarationList)
            {
                
                funcDecl.Accept(this);
            }
        }

        public void Visit(FunctionDeclaration functionDeclaration)
        {
            CreatePrototype(functionDeclaration);
            functionDeclaration.TypeId.Accept(this);
            _code += "(";
            functionDeclaration.Parameters.Accept(this);
            _code += ")";
            functionDeclaration.Body.Accept(this);
            _code += "\n";
        }

        private void CreatePrototype(FunctionDeclaration functionDeclaration)
        {
            _header += $"{GetValueType(functionDeclaration.TypeId.ValueType as Type)} {functionDeclaration.TypeId.Identifier}(";
            for (int i = 0; i < functionDeclaration.Parameters.TypeIds.Count; i++)
            {
                if (i > 0)
                    _header += ",";
                var parameter = functionDeclaration.Parameters.TypeIds[i];
                _header += parameter.Ref ? 
                    GetValueType(parameter.TypeId.ValueType) + " *" + parameter.TypeId.Identifier : 
                    GetValueType(parameter.TypeId.ValueType) + " " + parameter.TypeId.Identifier;
            }
            _header += ");\n";
        }

        public void Visit(Return returnNode)
        {
            _code += "return ";
            returnNode.Expression.Accept(this);
            _code += ";";
        }

        public void Visit(Refrence referece)
        {
            if (!referece.IsFuncCall)
            {
                referece.StructRefrence.Accept(this);
                _code += "->";
            }

            if (referece.Identifier is FuncCall)
             (referece.Identifier as FuncCall).Expressions.Insert(0, new Identifier(referece.StructRefrence.ToString()));
            referece.Identifier.Accept(this);
        }

        public void Visit(VarDeclerations varDecls)
        {
            varDecls.VarDeclerationList.ForEach(varDecl => varDecls.Accept(this));
        }

        public void Visit(VarDecleration varDecl)
        {
            if (varDecl.Type.ValueType == "string")
            {
                #region Eq
                if (varDecl.AssignmentOperator.Symbol == Indexes.Indexes.SymbolIndex.Eq)
                {
                    if (varDecl.IsFirstUse)
                    {
                        _code += GetValueType(varDecl.Type) + $" *{varDecl.Identifier}  =  string_new();\n";
                    }
                    else
                    {
                        _code += $"string_clear({varDecl.Identifier.Id})";
                    }
                    var element = ((varDecl.Expression as ExpressionVal).Value as StringValue).Elements[0];
                    CreateFunctions(element, "standard_append");
                    if (element is StringValue)
                    {
                        if ((element as StringValue).Value != "")
                            _code += $"({varDecl.Identifier},\"{(element as StringValue).Value}\");";
                    }
                    else if (element is TypeId)
                    {
                        _code += $"({varDecl.Identifier},{(element as TypeId).Identifier});";
                    }
                }
                #endregion
                #region Pluseq
                else if (varDecl.AssignmentOperator.Symbol == Indexes.Indexes.SymbolIndex.Pluseq)
                {
                    var element = ((varDecl.Expression as ExpressionVal).Value as StringValue).Elements[0];
                    CreateFunctions(element, "standard_append");
                    if (element is StringValue)
                    {
                        if ((element as StringValue).Value != "")
                            _code += $"({varDecl.Identifier},\"{(element as StringValue).Value}\");";
                    }
                    else if (element is TypeId)
                    {
                        _code += $"({varDecl.Identifier},{(element as TypeId).Identifier})";
                    }
                }
                #endregion
            }
            else
            {
                if (varDecl.IsFirstUse)
                    _code += GetValueType(varDecl.Type) + " ";
                _code += $"{varDecl.Identifier.Id} ";
                varDecl.AssignmentOperator.Accept(this);
                _code += " ";
                varDecl.Expression.Accept(this);
                _code += ";";
            }
        }

        private void getString(IExpression expression)
        {
            foreach (var element in (expression as StringValue).Elements)
            {
                if (element is StringValue)
                {
                    var stringValue = element as StringValue;
                }
                else if (element is TypeId)
                {
                    var typeId = element as TypeId;
                }

            }
        }

        public void Visit(StructDefinition structDef)
        {
            _code += $"struct {structDef.Identifier} " + "{\n";
            foreach (var structPart in structDef.StructParts.StructPartList)
            {
                if (structPart is VarDecleration)
                    _code += $"{GetValueType((structPart as VarDecleration).Type)} {(structPart as VarDecleration).Identifier};";
            }
            _code += "\n};\n";
        }

        public void Visit(StructParts structParts)
        {
            // not used - implemented in "public void Visit(StructDefinition structDef)"
            throw new NotImplementedException();
        }

        public void Visit(StructDefinitions structDefinitions)
        {
            CreateTypedef(structDefinitions);
            MoveStructFunctions(structDefinitions);
            foreach (var structDef in structDefinitions.Definitions)
            {
                structDef.Accept(this);
            }
            _code += "\n";
        }

        private void MoveStructFunctions(StructDefinitions structDefinitions)
        {
            foreach (var structDefinition in structDefinitions.Definitions)
            {
                var list = new List<Identifier>();
                foreach (var varDecl in structDefinition.StructParts.StructPartList)
                {
                    if ((varDecl is VarDecleration)) list.Add((varDecl as VarDecleration).Identifier);
                }
                var StructFuncVisitor = new StructFunctionIdentifiers(list);

                foreach (var structPart in structDefinition.StructParts.StructPartList)
                {
                    if (structPart is FunctionDeclaration)
                    {
                        (structPart as FunctionDeclaration).Body.Accept(StructFuncVisitor);
                        (structPart as FunctionDeclaration).Parameters.TypeIds
                            .Insert(0, new RefTypeId(new TypeId(new Identifier("this"), new Type(structDefinition.Identifier.Id) ), new Ref(true)));
                        _root.FunctionDeclarations.FunctionDeclarationList.Add(structPart as FunctionDeclaration);
                    }
                }
            }
        }

        private void CreateTypedef(StructDefinitions structDefinitions)
        {
            foreach (var structDefinition in structDefinitions.Definitions)
            {
                _header += $"typedef struct {structDefinition.Identifier} {structDefinition.Identifier};\n";
            }
        }

        public void Visit(AssignmentOperator assignOpr)
        {
            var opr = "ASSIGNMENTOPERATOR!!!";
            switch (assignOpr.Symbol)
            {
                case Indexes.Indexes.SymbolIndex.Eq:
                    opr = "=";
                    break;
                case Indexes.Indexes.SymbolIndex.Minuseq:
                    opr = "-=";
                    break;
                case Indexes.Indexes.SymbolIndex.Pluseq:
                    opr = "+=";
                    break;
            }
            _code += opr;
        }

        public void Visit(Include include)
        {
            //_header += "#include <";
            //for (int i = 0; i < include.Identifiers.Count; i++)
            //{
            //    if (i > 0)
            //        _header += ".";
            //    _header = include.Identifiers[i].Id;   
            //}
            //_header += ">";
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
            // Ref bliver ikke lagt i det endelige AST - men bliver brugt under parsing.
            throw new NotImplementedException();
        }

        public void Visit(ListType listType)
        {
            _code += "list_" + GetValueType(listType);
        }

        public void Visit(RefId refId)
        {
            _code += $" ref {refId.Identifier} ";
        }

        public void Visit(RefTypeId refTypeId)
        {
            _code += refTypeId.Ref ? 
                GetValueType(refTypeId.TypeId.ValueType) + " *" +  refTypeId.TypeId.Identifier : 
                GetValueType(refTypeId.TypeId.ValueType) + " " + refTypeId.TypeId.Identifier;
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
            for (int i = 0; i < typeIdList.TypeIds.Count; i++)
            {
                if (i > 0)
                    _code += ", ";
                typeIdList.TypeIds[i].Accept(this);
            }
        }

        public void Visit(IdIndex idIndex)
        {
            idIndex.Identifier.Accept(this);
            idIndex.ListIndex.Accept(this);
        }

        public void Visit(VarInStructDecleration varInStructDecleration)
        {
            varInStructDecleration.Refrence.Accept(this);
            _code += " ";
            varInStructDecleration.AssignmentOperator.Accept(this);
            _code += " ";
            varInStructDecleration.Expression.Accept(this);
            _code += ";";
        }
    }
}