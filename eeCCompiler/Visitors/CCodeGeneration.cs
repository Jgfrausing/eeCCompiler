using System;
using System.Collections.Generic;
using System.Linq;
using eeCCompiler.Interfaces;
using eeCCompiler.Nodes;
using eeCCompiler.Visitors.CCode;
using Type = eeCCompiler.Nodes.Type;

namespace eeCCompiler.Visitors
{
    public class CCodeGeneration : IEecVisitor
    {
        private readonly DefaultCCode _defaultCCode = new DefaultCCode();

        public Root _root;

        public CCodeGeneration()
        {
            Code = "";
            TempCVariable = 0;
            Header = _defaultCCode.GetIncludes() + "\n";
            StandardFunctions += _defaultCCode.StandardFunctionsHeader();
            StandardFunctions += _defaultCCode.StandardFunctionsCode();
        }

        private string Header { get; set; }
        public string Code { get; set; }
        private string StandardFunctions { get; }
        public int TempCVariable { get; set; }

        public string CCode => Header + Code;

        public void Visit(Root root)
        {
            _root = root;

            root.Includes.Accept(this);
            root.ConstantDefinitions.Accept(this);

            Code += StandardFunctions;
            Header += Code;

            Code = "";
            

            _defaultCCode.CreateListPrototypes(this);
            root.StructDefinitions.Accept(this);
            _defaultCCode.CreateCopyFunctions(this);

            root.FunctionDeclarations.Accept(this);

            Code += "void main()";
            root.Program.Accept(this);
        }

        public void Visit(Body body)
        {
            Code += "{\n";
            foreach (var bodyPart in body.Bodyparts)
            {
                bodyPart.Accept(this);
                Code += "\n";
            }
            Code += "}\n";
        }

        public void Visit(Constant constant)
        {
            Code += $"#define {constant.Identifier.ToString().ToUpper()} {constant.ConstantPart}\n";
        }

        public void Visit(ConstantDefinitions constantDefinitions)
        {
            constantDefinitions.ConstantList.ForEach(x => x.Accept(this));
            Header += "\n";
        }

        public void Visit(ElseStatement elseStatement)
        {
            Code += "else";
            elseStatement.Body.Accept(this);
        }

        public void Visit(IfStatement ifStatement)
        {
            var stringCreater = new StringFinderVisitor(TempCVariable);
            PreExpressionStringCreater(stringCreater, ifStatement);

            Code += "if(";
            ifStatement.Expression.Accept(this);
            Code += ")";
            ifStatement.Body.Accept(this);
            if (ifStatement.ElseStatement is IfStatement)
                Code += "else ";
            ifStatement.ElseStatement.Accept(this);
            PostExpressionStringCreater(stringCreater);
        }

        public void Visit(ExpressionNegate expressionNegate)
        {
            Code += "!";
            expressionNegate.Expression.Accept(this);
        }

        public void Visit(ExpressionVal expressionVal)
        {
            if (expressionVal.Value is Refrence && (expressionVal.Value as Refrence).IsFuncCall)
            {
                var reference = expressionVal.Value as Refrence;
                var strucCallOrder = reference.StructRefrence.ToString();
                while (!(reference.Identifier is FuncCall))
                {
                    reference = reference.Identifier as Refrence;
                    strucCallOrder += "." + reference.StructRefrence;
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
                            reference = reference.Identifier as Refrence;
                        }
                    }
                    (reference.Identifier as FuncCall).Accept(this);
                }
                else
                {
                    if (reference.StructRefrence is IdIndex)
                    {
                        Code += (reference.StructRefrence as IdIndex).Identifier.Type.ValueType + "list_get(";
                        Code += (reference.StructRefrence as IdIndex).ListIndex.Indexes[0];
                        Code += ", &" + (reference.StructRefrence as IdIndex).Identifier.Id + ")";
                    }

                    if (!(reference.StructRefrence is IdIndex))
                        Code += reference.StructRefrence.ToString();
                    var strucCallOrder = "";
                    while (!(reference.Identifier is Identifier))
                    {
                        if (reference.Identifier is Refrence)
                        {
                            reference = reference.Identifier as Refrence;
                            strucCallOrder += ".";
                            reference.StructRefrence.Accept(this);
                        }
                    }

                    Code += strucCallOrder + "." + reference.Identifier;
                }
            }
            else
                expressionVal.Value.Accept(this);
        }

        public void Visit(Direction direction)
        {
            Code += direction.Incrementing ? "++" : "--";
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
                if (expressionParenOpExpr.Operator.Symbol == Indexes.Indexes.SymbolIndex.Mod)
                    Code += "(int)(";
                expressionParenOpExpr.ExpressionParen.Accept(this);
                if (expressionParenOpExpr.Operator.Symbol == Indexes.Indexes.SymbolIndex.Mod)
                    Code += ")";
                expressionParenOpExpr.Operator.Accept(this);
                if (expressionParenOpExpr.Operator.Symbol == Indexes.Indexes.SymbolIndex.Mod)
                    Code += "(int)(";
                expressionParenOpExpr.Expression.Accept(this);
                if (expressionParenOpExpr.Operator.Symbol == Indexes.Indexes.SymbolIndex.Mod)
                    Code += ")";
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
                if (expressionValOpExpr.Operator.Symbol == Indexes.Indexes.SymbolIndex.Mod)
                    Code += "(int)(";
                expressionValOpExpr.Value.Accept(this);
                if (expressionValOpExpr.Operator.Symbol == Indexes.Indexes.SymbolIndex.Mod)
                    Code += ")";
                expressionValOpExpr.Operator.Accept(this);
                if (expressionValOpExpr.Operator.Symbol == Indexes.Indexes.SymbolIndex.Mod)
                    Code += "(int)(";
                expressionValOpExpr.Expression.Accept(this);
                if (expressionValOpExpr.Operator.Symbol == Indexes.Indexes.SymbolIndex.Mod)
                    Code += ")";
            }
        }

        public void Visit(ExpressionParen expressionParen)
        {
            Code += "(";
            expressionParen.Expression.Accept(this);
            Code += ")";
        }

        public void Visit(ExpressionMinus expressionMinus)
        {
            Code += "-";
            expressionMinus.Expression.Accept(this);
        }

        public void Visit(ExpressionList expressionList)
        {
            // hvad med ref?
            for (var i = 0; i < expressionList.Expressions.Count; i++)
            {
                if (i > 0)
                    Code += ",";
                expressionList.Expressions[i].Accept(this);
            }
        }

        public void Visit(StructDecleration structDecleration)
        {
            RetreveVariabels(structDecleration);
            Code += $"{structDecleration.StructIdentifier} {structDecleration.Identifier};\n";
            foreach (var varDecl in structDecleration.VarDeclerations.VarDeclerationList)
            {
                var stringCreater = new StringFinderVisitor(TempCVariable);
                PreExpressionStringCreater(stringCreater, varDecl);


                if (varDecl.Identifier.Type.IsBasicType && !varDecl.Identifier.Type.IsListValue)
                {
                    Code += $"{structDecleration.Identifier}.{varDecl.Identifier.Id} ";
                    varDecl.AssignmentOperator.Accept(this);
                    varDecl.Expression.Accept(this);
                }
                else if (varDecl.Identifier.Type.IsListValue)
                {
                    Code += $"{structDecleration.Identifier}.{varDecl.Identifier.Id} ";
                    Code += "= " + varDecl.Identifier.Type.ValueType + "list_newHandle();";
                }
                else if (varDecl.Identifier.Type.ValueType == "string")
                {
                    Code += $"{structDecleration.Identifier}.{varDecl.Identifier.Id} ";
                    Code += "= string_copy(&";
                    varDecl.Expression.Accept(this);
                    Code += ")";
                }
                else
                {
                    Code += $"{structDecleration.Identifier}.{varDecl.Identifier.Id} ";
                    Code += "= ";
                    varDecl.Expression.Accept(this);
                }
                Code += ";\n";
                PostExpressionStringCreater(stringCreater);
            }
        }

        public void Visit(RepeatExpr repeatExpr)
        {
            var stringCreater = new StringFinderVisitor(TempCVariable);
            PreExpressionStringCreater(stringCreater, repeatExpr);

            Code += "while (";
            repeatExpr.Expression.Accept(this);
            Code += ")";
            repeatExpr.Body.Accept(this);
            PostExpressionStringCreater(stringCreater);
        }

        public void Visit(RepeatFor repeatFor)
        {
            var stringCreater = new StringFinderVisitor(TempCVariable);
            PreExpressionStringCreater(stringCreater, repeatFor);

            Code += "for (";
            repeatFor.VarDecleration.Accept(this);
            Code += repeatFor.VarDecleration.Identifier.Id;
            Code += repeatFor.Direction.Incrementing ? "<" : ">";
            repeatFor.Expression.Accept(this);
            Code += ";" + repeatFor.VarDecleration.Identifier;
            repeatFor.Direction.Accept(this);
            Code += ")";
            repeatFor.Body.Accept(this);
            PostExpressionStringCreater(stringCreater);
        }

        public void Visit(Type type)
        {
            var cType = GetValueType(type);
            Code += $"{cType}";
        }

        public void Visit(StringValue stringValue)
        {
            Code += $" {stringValue} ";
        }

        public void Visit(Operator operate)
        {
            var opr = "OPERATOR!!!";
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
            Code += $" {opr} ";
        }

        public void Visit(Identifier identifier)
        {
            Code += $"{identifier}";
        }

        public void Visit(BoolValue boolValue)
        {
            var boolVal = boolValue.Value ? "1" : "0";
            Code += $" {boolVal} ";
        }

        public void Visit(NumValue numValue)
        {
            Code += $" {numValue} ";
        }

        public void Visit(FuncCall funcCall)
        {
            var stringCreater = new StringFinderVisitor(TempCVariable);
            PreExpressionStringCreater(stringCreater, funcCall);

            #region Print & Read

            if (funcCall.Identifier.Id == "program_print")
            {
                var parameter = (funcCall.Expressions[0] as ExpressionVal).Value;
                Code += $"standard_printString";
                Code += "(&";
                CreateRefrence(parameter);
                Code += $")";
            }
            else if (funcCall.Identifier.Id == "program_read")
            {
                if (funcCall.IsBodyPart)
                {
                    TempCVariable += 1;
                    Code += $"string_handle _{TempCVariable} = standard_read();";
                    Code += $"string_clear(&_{TempCVariable})";
                }
                else
                    Code += $"standard_read()" ;
            }
            #endregion
            #region Convert

            else if (funcCall.Identifier.Id == "program_convertStringToBool"
                     || funcCall.Identifier.Id == "program_convertStringToNum")
            {
                Code +=
                    $"{funcCall.Identifier}(&{funcCall.Expressions[0]}, &{(funcCall.Expressions[1] as RefId).Identifier})";
            }
            else if (funcCall.Identifier.Id == "program_convertBoolToString"
                     || funcCall.Identifier.Id == "program_convertNumToString")
            {
                Code +=
                    $"{funcCall.Identifier}({funcCall.Expressions[0]}, &{(funcCall.Expressions[1] as RefId).Identifier})";
            }
                #endregion
                #region ListFunctions

            else if (funcCall.Identifier.Id == "count")
            {
                Code += $"{funcCall.Expressions[0]}.size";
            }
            else if (funcCall.Identifier.Id == "add")
            {
                Code += $"{funcCall.Identifier.Type.ValueType}list_add(";
                if (!funcCall.Identifier.Type.IsBasicType)
                {
                    Code += "&";
                }
                funcCall.Expressions[1].Accept(this);
                
                Code += $",";
                funcCall.Expressions[0].Accept(this);
                Code += $")";
            }
            else if (funcCall.Identifier.Id == "insert")
            {
                //void clist_insert(int index, clist_handle * head, c *inputElement);
                Code += $"{funcCall.Identifier.Type.ValueType}list_insert(";
                funcCall.Expressions[1].Accept(this);
                var ExprChecker = new ExpressionChecker(new Typechecker(new List<string>()));

                Code += $", &{(funcCall.Expressions[0] as Identifier).Id}, ";
                if (funcCall.Identifier.Type.ValueType == "string" ||
                    (funcCall.Expressions[2] is Identifier && !(funcCall.Expressions[2] as Identifier).Type.IsBasicType))
                {
                    Code += "&";
                }
                funcCall.Expressions[2].Accept(this);
                Code += ")";
            }
            else if (funcCall.Identifier.Id == "remove")
            {
                Code += $"{funcCall.Identifier.Type.ValueType}" +
                        $"list_remove(";
                funcCall.Expressions[1].Accept(this);
                Code += $",";
                funcCall.Expressions[0].Accept(this);
                Code += ")";
            }
            else if (funcCall.Identifier.Id == "clear")
            {
                Code += $"{funcCall.Identifier.Type.ValueType}list_clear(";
                funcCall.Expressions[0].Accept(this);
                Code += ")";
            }
            else if (funcCall.Identifier.Id == "reverse")
            {
                //void numlist_reverse(numlist_handle * head);
                Code +=
                    $"{funcCall.Identifier.Type.ValueType}list_reverse(&{(funcCall.Expressions[0] as Identifier).Id})";
            }
            else if (funcCall.Identifier.Id == "sort")
            {
                //void numlist_sort(numlist_handle * head);
                Code += $"{funcCall.Identifier.Type.ValueType}list_sort(&{(funcCall.Expressions[0] as Identifier).Id})";
            }
                #endregion
                #region User functions + copy

            else
            {
                if (funcCall.Identifier.Id == "copy")
                {
                    RenameFunction(funcCall);
                }

                Code += $"{funcCall.Identifier}(";
                for (var i = 0; i < funcCall.Expressions.Count; i++)
                {
                    if (i > 0)
                        Code += ",";
                    else if (funcCall.IsStructFunction)
                        Code += "&";
                    funcCall.Expressions[i].Accept(this);
                }
                Code += ")";
            }

            #endregion

            if (funcCall.IsBodyPart)
                Code += ";";
            PostExpressionStringCreater(stringCreater);
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
                else if(!refrence.TypeId.Identifier.Type.IsBasicType)
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
                                new FuncCall(new List<IExprListElement>
                                {
                                    identifier
                                },
                                    new Identifier("copy"));
                            funcCall.ListType = new Type(refTypeId.TypeId.ValueType.ToString());
                            var copy = new VarDecleration(funcCall,
                                new AssignmentOperator(Indexes.Indexes.SymbolIndex.Eq),
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
            Code += "(";
            functionDeclaration.Parameters.Accept(this);
            Code += "){";
            foreach (var refTypeId in functionDeclaration.Parameters.TypeIds.Where(x => !x.Ref && !x.TypeId.Identifier.Type.IsBasicType))
            {
                Code +=
                    $"{refTypeId.TypeId.ValueType}_handle ={refTypeId.TypeId.ValueType}_copy(&{refTypeId.TypeId.Identifier});\n";
            }
            foreach (var bodypart in functionDeclaration.Body.Bodyparts) //functionDeclaration.Body.Accept(this);
            {
                bodypart.Accept(this);
            }

            Code += "}\n";
        }

        public void Visit(Return returnNode)
        {
            var stringCreater = new StringFinderVisitor(TempCVariable);
            PreExpressionStringCreater(stringCreater, returnNode);

            Code += "return ";
            returnNode.Expression.Accept(this);
            Code += ";";

            PostExpressionStringCreater(stringCreater);
        }

        public void Visit(Refrence referece)
        {
            if (!referece.IsFuncCall)
            {
                referece.StructRefrence.Accept(this);
                Code += ".";
            }
            if (referece.Identifier is FuncCall)
            {
                var funcCall = referece.Identifier as FuncCall;
                if (funcCall.IsListFunction)
                {
                    funcCall.Identifier.Id = funcCall.ListType.ValueType + "_" + funcCall.Identifier.Id;
                }
                funcCall.Expressions.Insert(0, new RefId(new Identifier(referece.StructRefrence.ToString())));
            }
            referece.Identifier.Accept(this);
        }

        public void Visit(VarDeclerations varDecls)
        {
            varDecls.VarDeclerationList.ForEach(varDecl => varDecls.Accept(this));
        }

        public void Visit(VarDecleration varDecl)
        {
            var stringCreater = new StringFinderVisitor(TempCVariable);
            PreExpressionStringCreater(stringCreater, varDecl);

            if (varDecl.Identifier.Type.IsListValue)
            {
                if (varDecl.IsFirstUse)
                {
                    Code += varDecl.Identifier.Type.ValueType;

                    Code += "list_handle ";
                }
                Code += varDecl.Identifier + " = " + varDecl.Identifier.Type.ValueType + "list";
                if (varDecl.Expression is ExpressionVal && (varDecl.Expression as ExpressionVal).Value is Identifier)
                {
                    Code += $"_copy(&{(varDecl.Expression as ExpressionVal).Value as Identifier});";
                }
                else
                    Code += $"_new();";
            }
 
            //Ret
            else if (varDecl.Identifier.Type.ValueType == "string" && !(varDecl.Expression is ExpressionVal) && (varDecl.Expression as ExpressionVal).Value is FuncCall)
            {
                if (varDecl.AssignmentOperator.Symbol == Indexes.Indexes.SymbolIndex.Eq)
                {
                    if (varDecl.Expression is ExpressionVal && (varDecl.Expression as ExpressionVal).Value is Identifier)
                    {
                        Code += $"{varDecl.Identifier.Type.ValueType}_handle {varDecl.Identifier} = {varDecl.Identifier.Type.ValueType}_copy(&{(varDecl.Expression as ExpressionVal).Value});\n";
                    }
                    else
                    {
                        CreateNewString(varDecl);
                    }
                    
                    
                }

                else if (varDecl.AssignmentOperator.Symbol == Indexes.Indexes.SymbolIndex.Pluseq)
                    AppendToString(((varDecl.Expression as ExpressionVal).Value as StringValue).Elements,
                        varDecl.Identifier);
            }
            else
            {
                if (varDecl.IsFirstUse)
                    Code += GetValueType(varDecl.Identifier.Type) + " ";

                Code += $"{varDecl.Identifier.Id} ";
                varDecl.AssignmentOperator.Accept(this);
                Code += " ";
                if (varDecl.Identifier.Type.ValueType == "string"
                && ((varDecl.Expression as ExpressionVal)?.Value as FuncCall)?.Identifier.Id == "program_read")
                {
                    if (varDecl.AssignmentOperator.Symbol == Indexes.Indexes.SymbolIndex.Eq)
                    { 
                        Code += $" standard_read()";
                    }
                    else
                        throw new NotImplementedException();
                }
                else if (!varDecl.Identifier.Type.IsBasicType)
                {
                    varDecl.Expression.Accept(this);
                    Code += ";";

                    Code += $"{varDecl.Identifier.Id} = {varDecl.Identifier.Type.ValueType}_copy(&{varDecl.Identifier.Id})";
                }
                else
                {
                    varDecl.Expression.Accept(this);
                }
                Code += ";";
            }
            PostExpressionStringCreater(stringCreater);
        }

        public void Visit(StructDefinition structDef)
        {
            Code += $"struct {structDef.Identifier} " + "{\n";
            foreach (var structPart in structDef.StructParts.StructPartList)
            {
                if (structPart is VarDecleration && !(structPart as VarDecleration).Identifier.Type.IsListValue)
                    Code +=
                        $"{GetValueType((structPart as VarDecleration).Identifier.Type)} {(structPart as VarDecleration).Identifier};";
                else if (structPart is VarDecleration && (structPart as VarDecleration).Identifier.Type.IsListValue)
                {
                    Code +=
                        $"{GetValueType((structPart as VarDecleration).Identifier.Type)}list_handle {(structPart as VarDecleration).Identifier};";
                }
                else if (structPart is StructDecleration)
                    Code +=
                        $"{GetValueType((structPart as StructDecleration).StructIdentifier)} {(structPart as StructDecleration).Identifier};";
            }
            Code += "\n};\n";

            Code += _defaultCCode.GenerateListTypeCode(structDef.Identifier.Id + "list",
                structDef.Identifier.Id, true);
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
            Code += "\n";
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
            Code += opr;
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
                Code += "[";
                index.Accept(this);
                Code += "]";
            }
        }

        public void Visit(Ref refNode)
        {
            // Ref bliver ikke lagt i det endelige AST - men bliver brugt under parsing.
            throw new NotImplementedException();
        }

        public void Visit(ListType listType)
        {
            Code += "list_" + GetValueType(listType);
        }

        public void Visit(RefId refId)
        {
            //if (refId.Type.IsBasicType)
                Code += " &";
            Code += $" {refId.Identifier} ";
        }

        public void Visit(RefTypeId refTypeId)
        {
            Code += refTypeId.Ref
                ? GetValueType(refTypeId.TypeId.ValueType) + " *" + refTypeId.TypeId.Identifier
                : GetValueType(refTypeId.TypeId.ValueType) + " " + refTypeId.TypeId.Identifier;
        }

        public void Visit(ListDimentions listDimentions)
        {
            for (var i = 0; i < listDimentions.Dimentions; i++)
            {
                Code += "[]";
            }
        }

        public void Visit(TypeId typeId)
        {
            typeId.ValueType.Accept(this);
            Code += " " + typeId.Identifier;
        }

        public void Visit(TypeIdList typeIdList)
        {
            for (var i = 0; i < typeIdList.TypeIds.Count; i++)
            {
                if (i > 0)
                    Code += ", ";
                typeIdList.TypeIds[i].Accept(this);
            }
        }

        public void Visit(IdIndex idIndex)
        {
            Code += $"{idIndex.Identifier.Type.ValueType}list_get(";
            idIndex.ListIndex.Indexes[0].Accept(this);
            Code += $", &{idIndex.Identifier.Id})";
        }

        public void Visit(VarInStructDecleration varInStructDecleration)
        {
            var stringCreater = new StringFinderVisitor(TempCVariable);
            PreExpressionStringCreater(stringCreater, varInStructDecleration);

            if (varInStructDecleration.Refrence is Refrence)
            {
                if (varInStructDecleration.Expression is ExpressionVal &&
                    (varInStructDecleration.Expression as ExpressionVal).Value is Identifier &&
                    !((varInStructDecleration.Expression as ExpressionVal).Value as Identifier).Type.IsBasicType &&
                    !((varInStructDecleration.Expression as ExpressionVal).Value as Identifier).Type.IsListValue &&
                    !(((varInStructDecleration.Expression as ExpressionVal).Value as Identifier).Type.ValueType == "string"))
                {
                    varInStructDecleration.Refrence.Accept(this);
                    Code += "= " + ((varInStructDecleration.Expression as ExpressionVal).Value as Identifier).Type.ValueType +
                            "_copy(&";
                    Code += ((varInStructDecleration.Expression as ExpressionVal).Value as Identifier).Id + ");";
                }
                else if (varInStructDecleration.Expression is ExpressionVal &&
                         (varInStructDecleration.Expression as ExpressionVal).Value is StringValue)
                {
                    if (varInStructDecleration.AssignmentOperator.Symbol == Indexes.Indexes.SymbolIndex.Eq)
                        CreateNewString(varInStructDecleration);

                    else if (varInStructDecleration.AssignmentOperator.Symbol == Indexes.Indexes.SymbolIndex.Pluseq)
                        AppendToString(
                            ((varInStructDecleration.Expression as ExpressionVal).Value as StringValue).Elements,
                            new Identifier(varInStructDecleration.Refrence.ToString()));
                }
                else
                {
                    varInStructDecleration.Refrence.Accept(this);
                    varInStructDecleration.AssignmentOperator.Accept(this);
                    varInStructDecleration.Expression.Accept(this);
                    Code += ";";
                }
            }
            else if (varInStructDecleration.Refrence is IdIndex)
            {
                var idIndex = varInStructDecleration.Refrence as IdIndex;
                Code += $"{idIndex.Identifier.Type.ValueType}list_set(";
                idIndex.ListIndex.Indexes[0].Accept(this);
                Code += ", ";
                if (idIndex.Identifier.Type.ValueType.ToString() == "string")
                    Code += "&";
                varInStructDecleration.Expression.Accept(this);
                Code += $", &{idIndex.Identifier.Id});";

            }

            PostExpressionStringCreater(stringCreater);
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
                if (expressionExprOpExpr.Operator.Symbol == Indexes.Indexes.SymbolIndex.Mod)
                    Code += "(int)(";
                expressionExprOpExpr.ExpressionParen.Accept(this);
                if (expressionExprOpExpr.Operator.Symbol == Indexes.Indexes.SymbolIndex.Mod)
                    Code += ")";
                expressionExprOpExpr.Operator.Accept(this);
                if (expressionExprOpExpr.Operator.Symbol == Indexes.Indexes.SymbolIndex.Mod)
                    Code += "(int)(";
                expressionExprOpExpr.Expression.Accept(this);
                if (expressionExprOpExpr.Operator.Symbol == Indexes.Indexes.SymbolIndex.Mod)
                    Code += ")";
            }
        }

        private void PreExpressionStringCreater(StringFinderVisitor stringCreater, INodeElement node)
        {
            node.Accept(stringCreater);
            foreach (var stringValue in stringCreater.StringDict)
            {
                Code += $"string_handle {stringValue.Key.Id} = string_new();\n";
                AppendToString(stringValue.Value.Elements, new Identifier(stringValue.Key.Id));
            }
        }

        private void PostExpressionStringCreater(StringFinderVisitor stringCreater)
        {
            foreach (var stringValue in stringCreater.StringDict)
            {
                Code += $"string_clear(&{stringValue.Key.Id});";
            }
            TempCVariable = stringCreater.VariableName;
        }

        private void RetreveVariabels(StructDecleration structDecleration)
        {
            var myStruct =
                _root.StructDefinitions.Definitions.FirstOrDefault(
                    x => x.Identifier.ToString() == structDecleration.StructIdentifier.ToString());

            foreach (var varDecl in myStruct.StructParts.StructPartList)
            {
                if (varDecl is VarDecleration && !structDecleration.VarDeclerations.VarDeclerationList.Contains(varDecl))
                    structDecleration.VarDeclerations.VarDeclerationList.Add(varDecl as VarDecleration);
                else if (varDecl is StructDecleration)
                {
                    var structDecl = varDecl as StructDecleration;
                    var oldIdentifer = structDecl.Identifier.Id;
                    structDecl.Identifier.Id = "_" + TempCVariable;
                    structDecleration.VarDeclerations.VarDeclerationList.Add(
                        new VarDecleration(new ExpressionVal(new Identifier("_" + TempCVariable)),
                            new AssignmentOperator(structDecl.AssignmentOperator.Symbol),
                            new Identifier(oldIdentifer)));

                    TempCVariable++;
                    varDecl.Accept(this);
                    structDecl.Identifier.Id = oldIdentifer;
                }
            }
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

        private void CreateRefrence(IValue parameter)
        {
            if (parameter is Identifier)
            {
                Code += $"{(parameter as Identifier).Id}";
            }
            else if (parameter is Refrence)
            {
                if ((parameter as Refrence).Identifier is Refrence)
                {
                    Code += $"{(((parameter as Refrence).Identifier as Refrence).StructRefrence as Identifier).Id}.";
                    CreateRefrence((parameter as Refrence).Identifier as Refrence);
                }
                else if ((parameter as Refrence).Identifier is Identifier)
                {
                    Code += $"{((parameter as Refrence).Identifier as Identifier).Id}";
                }
            }
        }

        private void RenameFunction(FuncCall funcCall)
        {
            funcCall.Identifier.Id = $"string_{funcCall.Identifier}";
        }

        private void CreatePrototype(FunctionDeclaration functionDeclaration)
        {
            Header +=
                $"{GetValueType(functionDeclaration.TypeId.ValueType as Type)} {functionDeclaration.TypeId.Identifier}(";
            for (var i = 0; i < functionDeclaration.Parameters.TypeIds.Count; i++)
            {
                if (i > 0)
                    Header += ",";
                var parameter = functionDeclaration.Parameters.TypeIds[i];
                Header += parameter.Ref
                    ? GetValueType(parameter.TypeId.ValueType) + " *" + parameter.TypeId.Identifier
                    : GetValueType(parameter.TypeId.ValueType) + " " + parameter.TypeId.Identifier;
            }
            Header += ");\n";
        }

        private void AppendToString(List<IStringPart> parameter, Identifier varIdentifier) //VarDecleration varDecl)
        {
            //var parameter = ((varDecl.Expression as ExpressionVal).Value as StringValue).Elements;
            for (var i = 0; i < parameter.Count; i++)
            {
                var element = parameter[i];
                //CreateFunctions(element, "standard_append");
                var stdprt = "standard_append";
                if (element is StringValue)
                {
                    Code += $"{stdprt}Chars";
                }
                else if (element is TypeId)
                {
                    var typeId = element as TypeId;
                    var type = typeId.ValueType as Type;
                    GetCFuncType(stdprt, type);
                }
                if (element is StringValue)
                {
                    Code += $"(&{varIdentifier},\"{(element as StringValue).Value}\")";
                }
                else if (element is TypeId)
                {
                    if ((element as TypeId).ValueType.ToString() == "string")
                    {
                        Code += $"(&{varIdentifier},&{(element as TypeId).Identifier})";
                    }
                    else
                    {
                        Code += $"(&{varIdentifier},{(element as TypeId).Identifier})";
                    }
                }
                Code += ";";
            }
        }

        private void GetCFuncType(string stdprt, Type type)
        {
            switch (type.ValueType)
            {
                case "string":
                    Code += $"{stdprt}String";
                    break;
                case "num":
                    Code += $"{stdprt}Num";
                    break;
                case "bool":
                    Code += $"{stdprt}Bool";
                    break;
            }
        }

        private void CreateNewString(INodeElement element)
        {
            var isFirstUse = false;
            var identifier = new Identifier("");
            identifier.Type = new Type("string");
            IExpression expression = null;
            if (element is VarDecleration)
            {
                var varDecl = element as VarDecleration;
                isFirstUse = varDecl.IsFirstUse;
                identifier = varDecl.Identifier;
                expression = varDecl.Expression;
            }
            else if (element is Refrence)
            {
                var reference = element as VarDecleration;
                isFirstUse = reference.IsFirstUse;
                identifier.Id = reference.ToString();
                expression = reference.Expression;
            }
            else
            {
                throw new NotImplementedException();
            }
            if (isFirstUse)
            {
                Code += GetValueType(identifier.Type) + $" {identifier}  =  string_new();\n";
            }
            else
            {
                Code += $"string_clear(&{identifier.Id});";
            }
            if ((expression as ExpressionVal).Value is StringValue)
                AppendToString(((expression as ExpressionVal).Value as StringValue).Elements, identifier);
        }

        private void MoveStructFunctions(StructDefinitions structDefinitions)
        {
            foreach (var structDefinition in structDefinitions.Definitions)
            {
                var list = new List<Identifier>();
                foreach (var structPart in structDefinition.StructParts.StructPartList)
                {
                    if (structPart is VarDecleration) list.Add((structPart as VarDecleration).Identifier);
                    if (structPart is StructDecleration) list.Add((structPart as StructDecleration).Identifier);
                }
                var StructFuncVisitor = new StructFunctionIdentifiers(list);
                foreach (var structPart in structDefinition.StructParts.StructPartList)
                {
                    if (structPart is FunctionDeclaration)
                    {
                        (structPart as FunctionDeclaration).Body.Accept(StructFuncVisitor);
                        (structPart as FunctionDeclaration).Parameters.TypeIds
                            .Insert(0,
                                new RefTypeId(
                                    new TypeId(new Identifier("this"), new Type(structDefinition.Identifier.Id)),
                                    new Ref(true)));
                        _root.FunctionDeclarations.FunctionDeclarationList.Add(structPart as FunctionDeclaration);
                    }
                }
            }
        }

        private void CreateTypedef(StructDefinitions structDefinitions)
        {
            foreach (var structDefinition in structDefinitions.Definitions)
            {
                Header += $"typedef struct {structDefinition.Identifier} {structDefinition.Identifier};\n";
            }
        }

        private void CompareString(IExpression first, IExpression second, bool isEqual)
        {
            if (!isEqual)
                Code += "!";
            Code += "string_equals(&";
            first.Accept(this);
            Code += ", &";
            second.Accept(this);
            Code += ")";
        }
    }
}