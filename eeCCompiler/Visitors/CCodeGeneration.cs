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
        private string _includeAndTypedef { get; set; }
        private string _header { get; set; }
        private string _code { get; set; }
        private readonly DefaultCCode _defaultCCode = new DefaultCCode();
        private readonly Stack<List<Identifier>> _heapIdentifiers = new Stack<List<Identifier>>(); // clear lister først
        public int tempCVariable { get; set; }

        public string CCode => _includeAndTypedef + _header + _code;

        private Root _root;

        public void Visit(Root root)
        {
            tempCVariable = 0;
            _includeAndTypedef = _defaultCCode.GetIncludes() + "\n";
            _header = _defaultCCode.StandardFunctionsHeader();
            _code += _defaultCCode.StandardFunctionsCode();
            _root = root;
            root.Includes.Accept(this);
            root.ConstantDefinitions.Accept(this);
            _header +=_code;
            _code = "";
            
            try
            {
                SortStructDefinitions(root.StructDefinitions);
                root.StructDefinitions.Accept(this);
            }
            catch (ArgumentException e)
            {
                Console.WriteLine(e.Message);
                Console.ReadKey();
                Environment.Exit(1);
            }
           
            
            //_header += _defaultCCode.GenerateListTypeHeader("string_handle", "string_handle", true);
            //_code += _defaultCCode.GenerateListTypeCode("string_handle", "string_handle", true);

            foreach (var structDefinition in root.StructDefinitions.Definitions)
            {
                _code += _defaultCCode.GenerateListTypeHeader(structDefinition.Identifier.Id + "list", structDefinition.Identifier.Id, true);
                _code += _defaultCCode.GenerateListTypeCode(structDefinition.Identifier.Id + "list", structDefinition.Identifier.Id, true);
            }
            ////////
            var copy = new Copy();
            
            foreach (var structDefinition in root.StructDefinitions.Definitions)
            {
                _code += copy.MakeCopyFunc(structDefinition);
            }
            root.FunctionDeclarations.Accept(this);
            _code += "void main()";
            root.Program.Accept(this);
        }

        private void SortStructDefinitions(StructDefinitions structDefinitions)
        {
            var sorter = new StructDefinitionSorter();
            sorter.Sort(structDefinitions.Definitions);
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
            var stringCreater = new StringFinderVisitor(tempCVariable);
            PreExpressionStringCreater(stringCreater, ifStatement);

            _code += "if(";
            ifStatement.Expression.Accept(this);
            _code += ")";
            ifStatement.Body.Accept(this);
            if (ifStatement.ElseStatement is IfStatement)
                _code += "else ";
            ifStatement.ElseStatement.Accept(this);
            PostExpressionStringCreater(stringCreater);
        }

        private void PreExpressionStringCreater(StringFinderVisitor stringCreater, INodeElement ifStatement)
        {
            ifStatement.Accept(stringCreater);
            foreach (var stringValue in stringCreater.StringDict)
            {
                _code += $"string_handle *{stringValue.Key.Id} = string_new();\n";
                AppendToString(stringValue.Value.Elements, new Identifier(stringValue.Key.Id));
            }
        }

        private void PostExpressionStringCreater(StringFinderVisitor stringCreater)
        {
            foreach (var stringValue in stringCreater.StringDict)
            {
                _code += $"string_clear({stringValue.Key.Id});free({stringValue.Key.Id});";
            }
            tempCVariable = stringCreater.VariableName;
        }

        public void Visit(ExpressionNegate expressionNegate)
        {
            _code += "!";
            expressionNegate.Expression.Accept(this);
        }

        public void Visit(ExpressionVal expressionVal)
        {
            if (expressionVal.Value is Refrence && (expressionVal.Value as Refrence).IsFuncCall)
            {
                var reference = expressionVal.Value as Refrence;
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
                if (reference.IsFuncCall)
                {
                    while (!(reference.Identifier is FuncCall))
                    {
                        if (reference.Identifier is Refrence)
                        {
                            reference = (reference.Identifier as Refrence);
                        }
                    }
                    (reference.Identifier as FuncCall).Accept(this);
                }
                else
                {
                    string strucCallOrder = reference.StructRefrence.ToString();
                    while (!(reference.Identifier is Identifier))
                    {
                        if (reference.Identifier is Refrence)
                        {
                            reference = (reference.Identifier as Refrence);
                            strucCallOrder += "->";
                            reference.StructRefrence.Accept(this);
                        }
                    }

                    _code += strucCallOrder + "->" + reference.Identifier;
                }
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
            if (expressionParenOpExpr.Operator.IsStringOpr)
            {
                if (expressionParenOpExpr.Operator.Symbol == Indexes.Indexes.SymbolIndex.Eqeq)
                    CompareString(expressionParenOpExpr.ExpressionParen, expressionParenOpExpr.Expression, true);
                else
                    CompareString(expressionParenOpExpr.ExpressionParen, expressionParenOpExpr.Expression, false);
            }
            else
            {
                expressionParenOpExpr.ExpressionParen.Accept(this);
                expressionParenOpExpr.Operator.Accept(this);
                expressionParenOpExpr.Expression.Accept(this);
            }
            
        }

        public void Visit(ExpressionValOpExpr expressionValOpExpr)
        {
            if (expressionValOpExpr.Operator.IsStringOpr)
            {
                if (expressionValOpExpr.Operator.Symbol == Indexes.Indexes.SymbolIndex.Eqeq)
                {
                    var expval = new ExpressionVal(expressionValOpExpr.Value);
                    CompareString(expval, expressionValOpExpr.Expression, true);
                }
                else
                {
                    var expval = new ExpressionVal(expressionValOpExpr.Value);
                    CompareString(expval, expressionValOpExpr.Expression, false);
                }
                    
            }
            else
            {
                expressionValOpExpr.Value.Accept(this);
                expressionValOpExpr.Operator.Accept(this);
                expressionValOpExpr.Expression.Accept(this);
            }
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
            RetreveVariabels(structDecleration);
            _code += $"{structDecleration.StructIdentifier} {structDecleration.Identifier};\n";
            foreach (var varDecl in structDecleration.VarDeclerations.VarDeclerationList)
            {
                _code += $"{structDecleration.Identifier}.{varDecl.Identifier.Id} ";
                if (varDecl.Identifier.Type.IsBasicType && !varDecl.Identifier.Type.IsListValue)
                {
                    varDecl.AssignmentOperator.Accept(this);
                    varDecl.Expression.Accept(this);
                }
                else if (varDecl.Identifier.Type.IsListValue)
                {
                    _code += "= " + varDecl.Identifier.Type.ValueType + "list_newHandle();";
                }
                else
                {
                    _code += "= " + varDecl.Expression;
                }
                _code += ";\n";
            }

        }

        private void RetreveVariabels(StructDecleration structDecleration)
        {
            var myStruct = _root.StructDefinitions.Definitions.FirstOrDefault(x => x.Identifier.ToString() == structDecleration.StructIdentifier.ToString());
            foreach (var varDecl in myStruct.StructParts.StructPartList)
            {
                if (varDecl is VarDecleration && !structDecleration.VarDeclerations.VarDeclerationList.Contains(varDecl))
                    structDecleration.VarDeclerations.VarDeclerationList.Add(varDecl as VarDecleration);
                else if (varDecl is StructDecleration)
                {
                    RetreveVariabels(varDecl as StructDecleration);
                }
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

                case Indexes.Indexes.SymbolIndex.Plus:
                    opr = "+";
                    break;
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
            //PreExpressionStringCreater();
            #region Print
            if (funcCall.Identifier.Id == "program_print")
            {
                var parameter = (funcCall.Expressions[0] as ExpressionVal).Value;
                _code += $"standard_printString";
                GetCFuncType("print", new Type("!Not implemented!"));
                _code += "(";
                CreateRefrence(parameter);
                _code += $")";
            }
            #endregion
            #region Convert
            else if (funcCall.Identifier.Id == "program_convertStringToBool" 
                || funcCall.Identifier.Id == "program_convertStringToNum")
            {
                _code += $"{funcCall.Identifier}({funcCall.Expressions[0]}, &{(funcCall.Expressions[1] as RefId).Identifier})";
            }
            else if (funcCall.Identifier.Id == "program_convertBoolToString" 
                || funcCall.Identifier.Id == "program_convertNumToString")
            {
                _code += $"{funcCall.Identifier}({funcCall.Expressions[0]}, {(funcCall.Expressions[1] as RefId).Identifier})";
            }
            #endregion
            #region ListFunctions

            if (funcCall.Identifier.Id == "count")
            {
                _code += $"{funcCall.Expressions[0]}->size";
            }
            else if (funcCall.Identifier.Id == "add")
            {
                _code += $"";
            }
            else if (funcCall.Identifier.Id == "insert")
            {
                _code += $"";
            }
            else if (funcCall.Identifier.Id == "remove")
            {
                _code += $"";
            }
            else if (funcCall.Identifier.Id == "clear")
            {
                _code += $"";
            }
            else if (funcCall.Identifier.Id == "reverse")
            {
                _code += $"";
            }
            else if (funcCall.Identifier.Id == "sort")
            {
                _code += $"";
            }
            #endregion
            #region User functions
            else
            {
                if (funcCall.Identifier.Id == "copy")
                {
                    RenameFunction(funcCall);
                }

                _code += $"{funcCall.Identifier}(";
                for (int i = 0; i < funcCall.Expressions.Count; i++)
                {
                    if (i > 0)
                        _code += ",";
                    funcCall.Expressions[i].Accept(this);
                }
                _code += ")";
            }
            #endregion
            if (funcCall.IsBodyPart)
                _code += ";";
        }

        private void CreateRefrence(IValue parameter)
        {
            if (parameter is Identifier)
            {
                _code += $"{(parameter as Identifier).Id}";
            }
            else if (parameter is Refrence)
            {
                if ((parameter as Refrence).Identifier is Refrence)
                {
                    _code += $"{(((parameter as Refrence).Identifier as Refrence).StructRefrence as Identifier).Id}.";
                    CreateRefrence(((parameter as Refrence).Identifier as Refrence));
                }
                else if ((parameter as Refrence).Identifier is Identifier)
                {
                    _code += $"{((parameter as Refrence).Identifier as Identifier).Id}";
                }
                
            }
        }

        private void RenameFunction(FuncCall funcCall)
        {
            funcCall.Identifier.Id = $"string_{funcCall.Identifier}";
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
            var refIdentifiers = new List<Identifier>();
            var notRefIdentifiers = new List<Identifier>();

            var refTypeIds = new List<RefTypeId>();
            
            foreach (var refrence in functionDeclaration.Parameters.TypeIds)
            {
                refTypeIds.Add(refrence);
                if (refrence.Ref)
                    refIdentifiers.Add(refrence.TypeId.Identifier);
                else
                {
                    notRefIdentifiers.Add(refrence.TypeId.Identifier);
                }
            }
            var refIdentifierVisitor = new RefrenceIdentifiers(refIdentifiers);
            functionDeclaration.Body.Accept(refIdentifierVisitor);

            var renamingIdentifiersVisitor = new RenamePassByValueStructIdentifiers(notRefIdentifiers);
            functionDeclaration.Body.Accept(renamingIdentifiersVisitor);

            foreach (var refTypeId in functionDeclaration.Parameters.TypeIds)
            {
                switch (refTypeId.TypeId.ValueType.ToString())
                {
                    case "num":
                    case "bool":
                        break;
                    default:
                        if (!refTypeId.Ref)
                        {
                            var identifier = new Identifier(refTypeId.TypeId.Identifier.Id);
                            identifier.Type.ValueType = refTypeId.TypeId.Identifier.Type.ValueType;
                            var funcCall =
                                new FuncCall(new List<IExprListElement>()
                                {
                                    identifier
                                }, 
                                new Identifier("copy"));
                            funcCall.ListType = new Type(refTypeId.TypeId.ValueType.ToString());
                            var copy = new VarDecleration(funcCall, new AssignmentOperator(Indexes.Indexes.SymbolIndex.Eq),
                                    new Identifier("_" + refTypeId.TypeId.Identifier.Id)
                                    );
                            copy.IsFirstUse = true;
                            functionDeclaration.Body.Bodyparts.Insert(0, copy);
                        }
                            
                        refTypeId.Ref = true;
                        break;
                }
                //refTypeId.TypeId.Identifier.Id
            }

            CreatePrototype(functionDeclaration);
            functionDeclaration.TypeId.Accept(this);
            _code += "(";
            functionDeclaration.Parameters.Accept(this);
            _code += "){";
            foreach (var refTypeId in functionDeclaration.Parameters.TypeIds.Where(x => !x.Ref))
            {
                _code += $"{refTypeId.TypeId.ValueType}_handle * ={refTypeId.TypeId.ValueType}_copy({refTypeId.TypeId.Identifier});\n";
            }
            foreach (var bodypart in functionDeclaration.Body.Bodyparts)//functionDeclaration.Body.Accept(this);
            {
                bodypart.Accept(this);
            }
            
            _code += "}\n";
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
            referece.IsFuncCall = true;
            if (!referece.IsFuncCall)
            {
                referece.StructRefrence.Accept(this);
                _code += "->";
            }
            if (referece.Identifier is FuncCall)
            {
                var funcCall = referece.Identifier as FuncCall;
                funcCall.ListType = (new eeCCompiler.Nodes.Type("string"));
                funcCall.IsBodyPart = true;
                if (funcCall.IsListFunction)
                {
                    funcCall.Identifier.Id = funcCall.ListType.ValueType + "_" + funcCall.Identifier.Id;
                }
                funcCall.Expressions.Insert(0, new Identifier(referece.StructRefrence.ToString()));
            }
            referece.Identifier.Accept(this);
        }

        public void Visit(VarDeclerations varDecls)
        {
            varDecls.VarDeclerationList.ForEach(varDecl => varDecls.Accept(this));
        }

        public void Visit(VarDecleration varDecl)
        {
            if (varDecl.Identifier.Type.IsListValue)
            {
                _code +=
                    $"{varDecl.Identifier.Type.ValueType}list_handle * {varDecl.Identifier.Id} = {varDecl.Identifier.Type.ValueType}list";
                if (varDecl.Expression is ExpressionVal && (varDecl.Expression as ExpressionVal).Value is Identifier)
                {
                    _code += $"_copy({(varDecl.Expression as ExpressionVal).Value as Identifier});";
                }
                else
                    _code +=$"_new();";
            }
            else if (varDecl.Identifier.Type.ValueType == "string" && !(varDecl.Expression is FuncCall) )
            {
                if (varDecl.AssignmentOperator.Symbol == Indexes.Indexes.SymbolIndex.Eq)
                    CreateNewString(varDecl);

                else if (varDecl.AssignmentOperator.Symbol == Indexes.Indexes.SymbolIndex.Pluseq)
                    AppendToString(((varDecl.Expression as ExpressionVal).Value as StringValue).Elements, varDecl.Identifier);
            }
            else
            {
                if (varDecl.IsFirstUse)
                    _code += GetValueType(varDecl.Identifier.Type) + " ";
                //if (!varDecl.Identifier.Type.IsBasicType)
                //    _code += "*";
                _code += $"{varDecl.Identifier.Id} ";
                varDecl.AssignmentOperator.Accept(this);
                _code += " ";
                varDecl.Expression.Accept(this);
                _code += ";";
            }
        }

        private void AppendToString(List<IStringPart> parameter, Identifier varIdentifier)//VarDecleration varDecl)
        {
            //var parameter = ((varDecl.Expression as ExpressionVal).Value as StringValue).Elements;
            for (int i = 0; i < parameter.Count; i++)
            {
                var element = parameter[i];
                //CreateFunctions(element, "standard_append");
                var stdprt = "standard_append";
                if (element is StringValue)
                {
                    _code += $"{stdprt}Chars";
                }
                else if (element is TypeId)
                {
                    var typeId = (element as TypeId);
                    var type = typeId.ValueType as eeCCompiler.Nodes.Type;
                    GetCFuncType(stdprt, type);
                }
                if (element is StringValue)
                {
                    _code += $"({varIdentifier},\"{(element as StringValue).Value}\")";
                }
                else if (element is TypeId)
                {
                    _code += $"({varIdentifier},{(element as TypeId).Identifier})";
                }
                _code += ";";
            }
        }

        private void GetCFuncType(string stdprt, Type type)
        {
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

        private void CreateNewString(VarDecleration varDecl)
        {
            if (varDecl.IsFirstUse)
            {
                _code += GetValueType(varDecl.Identifier.Type) + $" *{varDecl.Identifier}  =  string_new();\n";
            }
            else
            {
                _code += $"string_clear({varDecl.Identifier.Id});";
            }
            AppendToString(((varDecl.Expression as ExpressionVal).Value as StringValue).Elements, varDecl.Identifier);
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
                if (structPart is VarDecleration && !(structPart as VarDecleration).Identifier.Type.IsListValue)
                    _code += $"{GetValueType((structPart as VarDecleration).Identifier.Type)} {(structPart as VarDecleration).Identifier};";
                else if (structPart is VarDecleration && (structPart as VarDecleration).Identifier.Type.IsListValue)
                {
                    _code += $"{GetValueType((structPart as VarDecleration).Identifier.Type)}list_handle * {(structPart as VarDecleration).Identifier};";
                }
                else if (structPart is StructDecleration)
                    _code += $"{GetValueType((structPart as StructDecleration).StructIdentifier)} {(structPart as StructDecleration).Identifier};";
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
                foreach (var structPart in structDefinition.StructParts.StructPartList)
                {
                    if ((structPart is VarDecleration)) list.Add((structPart as VarDecleration).Identifier);
                    if ((structPart is StructDecleration)) list.Add((structPart as StructDecleration).Identifier);
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
            if(refId.Type.IsBasicType)
                _code += " &";
            _code += $" {refId.Identifier} ";
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

        public void Visit(ExpressionExprOpExpr expressionExprOpExpr)
        {
            if (expressionExprOpExpr.Operator.IsStringOpr)
            {
                if (expressionExprOpExpr.Operator.Symbol == Indexes.Indexes.SymbolIndex.Eqeq)
                {
                    CompareString(expressionExprOpExpr.ExpressionParen, expressionExprOpExpr.Expression, true);
                }
                else
                {
                    CompareString(expressionExprOpExpr.ExpressionParen, expressionExprOpExpr.Expression, false);
                }
                
            }
            else
            {
                expressionExprOpExpr.ExpressionParen.Accept(this);
                expressionExprOpExpr.Operator.Accept(this);
                expressionExprOpExpr.Expression.Accept(this);
            }
            
        }

        private void CompareString(IExpression first, IExpression second, bool isEqual)
        {
            if (!isEqual)
                _code += "!";
            _code += "string_equals(";
            first.Accept(this);
            _code += ", ";
            second.Accept(this);
            _code += ")";
        }
    }
}