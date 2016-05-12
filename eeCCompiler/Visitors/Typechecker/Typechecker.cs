using System.Collections.Generic;
using eeCCompiler.Interfaces;
using eeCCompiler.Nodes;

namespace eeCCompiler.Visitors
{
    public class Typechecker : Visitor
    {
        private readonly ExpressionChecker _expressionChecker;

        public Typechecker(List<string> errors)
        {
            Errors = errors;
            Identifiers = new Dictionary<string, IValue>();
            Funcs = StandardFunctions.FunctionDict();
            Structs = new Dictionary<string, StructDefinition>();
            _expressionChecker = new ExpressionChecker(this);
        }

        public List<string> Errors { get; set; }
        public Dictionary<string, IValue> Identifiers { get; set; }
        public Dictionary<string, Function> Funcs { get; set; }
        public Dictionary<string, StructDefinition> Structs { get; set; }

        public Dictionary<string, IValue> saveScope()
        {
            var preBodyIdentifiers = new Dictionary<string, IValue>();
            foreach (var val in Identifiers)
            {
                preBodyIdentifiers.Add(val.Key, val.Value);
            }
            return preBodyIdentifiers;
        }

        #region Visits

        public override void Visit(Root root)
        {
            root.Includes.Accept(this);
            root.ConstantDefinitions.Accept(this);
            root.StructDefinitions.Accept(this);
            root.FunctionDeclarations.Accept(this); //Flyttet over program for at checke kald giver menning
            root.Program.Accept(this);
        }

        public override void Visit(Constant constant)
        {
            constant.Identifier.Accept(this);
            if (constant.ConstantPart is NumValue)
                Identifiers[constant.Identifier.Id] = constant.ConstantPart as NumValue;
            else if (constant.ConstantPart is BoolValue)
                Identifiers[constant.Identifier.Id] = constant.ConstantPart as BoolValue;
            else if (constant.ConstantPart is StringValue)
                Identifiers[constant.Identifier.Id] = constant.ConstantPart as StringValue;
        }

        public override void Visit(StructDefinition structDefinition)
        {
            if (!Structs.ContainsKey(structDefinition.Identifier.Id))
                Structs.Add(structDefinition.Identifier.Id, structDefinition);
            else
                Errors.Add($"{LineColumnString(structDefinition)}\"{structDefinition.Identifier.Id}\" was declared twice" );

            //RYD OP!!

            var preBodyIdentifiers = saveScope();
            var preStructDefFunctions = new Dictionary<string, Function>();
            foreach (var val in Funcs)
            {
                preStructDefFunctions.Add(val.Key, val.Value);
            }

            foreach (var stpart in structDefinition.StructParts.StructPartList)
            {
                if (stpart is FunctionDeclaration)
                    (stpart as FunctionDeclaration).TypeId.Identifier.Id = structDefinition.Identifier.Id + "_" +
                                                                           (stpart as FunctionDeclaration).TypeId
                                                                               .Identifier.Id;
            }
            structDefinition.StructParts.Accept(this);
            Identifiers = preBodyIdentifiers;
            Funcs = preStructDefFunctions;
        }

        public override void Visit(StructDecleration structDecleration)
        {
            if (!(structDecleration.AssignmentOperator.Symbol == Indexes.Indexes.SymbolIndex.Eq))
                Errors.Add($"{LineColumnString(structDecleration)}The \"{structDecleration.AssignmentOperator.Symbol}\" operator can not be used on a struct");
            if (!Identifiers.ContainsKey(structDecleration.Identifier.Id))
            {
                structDecleration.Identifier.Accept(this);
                if (Structs.ContainsKey(structDecleration.StructIdentifier.Id))
                {
                    Identifiers[structDecleration.Identifier.Id] =
                        new StructValue(Structs[structDecleration.StructIdentifier.Id]);
                    var structVariables = new Dictionary<string, IValue>();
                    foreach (var structPart in Structs[structDecleration.StructIdentifier.Id].StructParts.StructPartList
                        )
                    {
                        if (structPart is VarDecleration)
                        {
                            structVariables.Add((structPart as VarDecleration).Identifier.Id,
                                _expressionChecker.CheckExpression((structPart as VarDecleration).Expression));
                        }
                        else if (structPart is StructDecleration)
                        {
                            if (Structs.ContainsKey((structPart as StructDecleration).StructIdentifier.Id))
                                structVariables.Add((structPart as StructDecleration).Identifier.Id,
                                    new StructValue(Structs[(structPart as StructDecleration).StructIdentifier.Id]));
                            else
                                Errors.Add($"{LineColumnString(structPart as StructDecleration)}Struct \"{(structPart as StructDecleration).StructIdentifier.Id}\" was not declared but was used in program");
                        }
                    }

                    foreach (var varDecl in structDecleration.VarDeclerations.VarDeclerationList)
                    {
                        if (structVariables.ContainsKey(varDecl.Identifier.Id))
                        {
                            var expressionVariable = _expressionChecker.CheckExpression(varDecl.Expression);
                            if (expressionVariable is StructValue)
                            {
                                var exprStructIden = (expressionVariable as StructValue).Struct.Identifier.Id;
                                var declStructIden =
                                    (structVariables[varDecl.Identifier.Id] as StructValue).Struct.Identifier.Id;
                                if (exprStructIden != declStructIden)
                                    Errors.Add($"{LineColumnString(varDecl)}Value of struct variable \"{varDecl.Identifier.Id}\" is not of same type as the expression");
                            }
                            else if (
                                !(structVariables[varDecl.Identifier.Id].GetType().ToString() ==
                                  _expressionChecker.CheckExpression(varDecl.Expression).GetType().ToString()))
                                Errors.Add($"{LineColumnString(varDecl)}Value of struct variable \"{varDecl.Identifier.Id}\" is not of same type as the expression");
                        }
                        else
                            Errors.Add($"{LineColumnString(varDecl)}Struct \"{structDecleration.StructIdentifier.Id}\" does not contain a variable called \"{varDecl.Identifier.Id}\"");
                    }
                }
                else
                    Errors.Add($"{LineColumnString(structDecleration)}Struct \"{structDecleration.StructIdentifier.Id}\" was not declared but was used in program");
            }
            else if (!(Identifiers[structDecleration.Identifier.Id] is StructValue))
                Errors.Add($"{LineColumnString(structDecleration)}\"{structDecleration.Identifier.Id}\" is not of type \"{structDecleration.StructIdentifier.Id}\"");
            else if (
                !((Identifiers[structDecleration.Identifier.Id] as StructValue).Struct.Identifier.Id ==
                  Structs[structDecleration.StructIdentifier.Id].Identifier.Id))
                Errors.Add($"{structDecleration.Identifier.Id}\" is not of type \"{structDecleration.StructIdentifier.Id}\"");
        }

        public override void Visit(ExpressionNegate expressionNegate)
        {
            _expressionChecker.CheckExpression(expressionNegate);
        }

        public override void Visit(ExpressionValOpExpr expressionValOpExpr)
        {
            if (expressionValOpExpr.Value is StringValue)
                _expressionChecker.StringConverter(expressionValOpExpr.Value as StringValue);
            _expressionChecker.CheckExpression(expressionValOpExpr);
        }

        public override void Visit(ExpressionMinus expressionMinus)
        {
            _expressionChecker.CheckExpression(expressionMinus);
        }

        public override void Visit(ExpressionParen expressionParen)
        {
            _expressionChecker.CheckExpression(expressionParen);
        }

        public override void Visit(ExpressionParenOpExpr expressionParenOpExpr)
        {
            _expressionChecker.CheckExpression(expressionParenOpExpr);
        }

        public override void Visit(ExpressionVal expressionVal)
        {
            _expressionChecker.CheckExpression(expressionVal);
        }

        public override void Visit(Identifier identifier)
        {
            if (!Identifiers.ContainsKey(identifier.Id))
                Identifiers.Add(identifier.Id, identifier);
            if (Structs.ContainsKey(identifier.Id))
                Errors.Add($"{LineColumnString(identifier)}Variables can not be of the same name as one of the structs \"{identifier.Id}\"");
        }

        public override void Visit(VarInStructDecleration varInStructDecleration)
        {
            IValue value1 = null;
            if (Identifiers.ContainsKey((varInStructDecleration.Refrence.StructRefrence as Identifier).Id))
            {
                if (Identifiers[(varInStructDecleration.Refrence.StructRefrence as Identifier).Id] is StructValue)
                {
                    value1 = _expressionChecker.StructRefrenceChecker(varInStructDecleration.Refrence,
                        (Identifiers[(varInStructDecleration.Refrence.StructRefrence as Identifier).Id] as StructValue)
                            .Struct.Identifier.Id,
                        varInStructDecleration.Refrence);
                    if (value1.GetType().ToString() !=
                        _expressionChecker.CheckExpression(varInStructDecleration.Expression).GetType().ToString())
                    {
                        Errors.Add($"{LineColumnString(varInStructDecleration)}Struct variable is of type \"{value1.GetType()}\" and expression is of type" +
                       $"\"{_expressionChecker.CheckExpression(varInStructDecleration.Expression).GetType()}\"");
                    }
                    //Den skal ikke gøre noget hvis det er lykkes da vi er ligeglade med værdien.
                }
                else
                    Errors.Add($"{LineColumnString(varInStructDecleration)}\"{(varInStructDecleration.Refrence.StructRefrence as Identifier).Id}\" is not a struct");
            }
            else
            {
                Errors.Add($"{LineColumnString(varInStructDecleration)}\"{(varInStructDecleration.Refrence.StructRefrence as Identifier).Id}\" does not exist");//Måske dum fejlbesked
            }
        }

        public override void Visit(VarDecleration varDecleration)
        {
            if (!Identifiers.ContainsKey(varDecleration.Identifier.Id))
                varDecleration.IsFirstUse = true;

            if (!(varDecleration.AssignmentOperator.Symbol == Indexes.Indexes.SymbolIndex.Eq) &&
                varDecleration.IsFirstUse)
                Errors.Add($"{LineColumnString(varDecleration)}The \"{varDecleration.AssignmentOperator.Symbol}\" operator can not be used on an uninitialised variable");
            varDecleration.Identifier.Accept(this);

            varDecleration.AssignmentOperator.Accept(this);
            var value = _expressionChecker.CheckExpression(varDecleration.Expression);

            if (Identifiers[varDecleration.Identifier.Id] is Identifier)
                (Identifiers[varDecleration.Identifier.Id] as Identifier).Type.ValueType =
                    _expressionChecker.CheckValueType(value);
            else
            {
                varDecleration.Identifier.Type.ValueType = _expressionChecker.CheckValueType(value);
            }
            if (value is UnInitialisedVariable)
            {
                Errors.Add($"{LineColumnString(varDecleration)}Identifier \"{varDecleration.Identifier.Id}\" was not assigned a value");
            }
            else if (Identifiers[varDecleration.Identifier.Id] is Identifier)
            {
                if (value is StructValue)
                    (Identifiers[varDecleration.Identifier.Id] as Identifier).Type.ValueType =
                        (value as StructValue).Struct.Identifier.Id;
                else if (value is ListValue)
                {
                    (Identifiers[varDecleration.Identifier.Id] as Identifier).Type.ValueType =
                        _expressionChecker.CheckValueType(value);
                    (Identifiers[varDecleration.Identifier.Id] as Identifier).Type.IsListValue = true;
                }
                Identifiers[varDecleration.Identifier.Id] = value;
            }
            else if (Identifiers[varDecleration.Identifier.Id].GetType().Name == value.GetType().Name)
            {
                if (Identifiers[varDecleration.Identifier.Id] is BoolValue &&
                    !(varDecleration.AssignmentOperator.Symbol == Indexes.Indexes.SymbolIndex.Eq))
                    Errors.Add($"{LineColumnString(varDecleration)}\"{varDecleration.AssignmentOperator.Symbol}\" operator can not be used with two boolean values");
                else if (Identifiers[varDecleration.Identifier.Id] is StringValue &&
                         varDecleration.AssignmentOperator.Symbol == Indexes.Indexes.SymbolIndex.Minuseq)
                    Errors.Add($"{LineColumnString(varDecleration)}\"{varDecleration.AssignmentOperator.Symbol}\" operator can not be used with two string values");
                Identifiers[varDecleration.Identifier.Id] = value;
            }
            else
                Errors.Add($"{LineColumnString(varDecleration)}Identifier \"{varDecleration.Identifier.Id}\" is of type" +
                $"\" {Identifiers[varDecleration.Identifier.Id].GetType().Name}\" but a \"{value.GetType().Name}\" was tried to be assigned to this identifier");
        }

        public override void Visit(Body body)
        {
            var preBodyIdentifiers = saveScope();
            base.Visit(body);
            Identifiers = preBodyIdentifiers;
        }

        public override void Visit(FunctionDeclarations functionsDeclarations)
        {
            foreach (var decl in functionsDeclarations.FunctionDeclarationList)
            {
                decl.TypeId.Identifier.Id = "program_" + decl.TypeId.Identifier.Id;
                decl.Accept(this);
            }
        }

        public override void Visit(FunctionDeclaration functionDeclaration)
        {
            if (!Funcs.ContainsKey(functionDeclaration.TypeId.Identifier.Id))
            {
                var preBodyIdentifiers = saveScope();
                foreach (var parameter in functionDeclaration.Parameters.TypeIds)
                {
                    if (parameter.TypeId.ValueType.ToString() == "void")
                        Errors.Add($"{LineColumnString(functionDeclaration)}Parameter \"{parameter.TypeId.Identifier.Id}\"" +
                                    $"to function \"{functionDeclaration.TypeId.Identifier.Id}\" can not be of type void");
                    Identifiers.Add(parameter.TypeId.Identifier.Id,
                        _expressionChecker.TypeChecker(parameter.TypeId.ValueType.ToString()));
                }
                bool returnFound;

                foreach (var bodyPart in functionDeclaration.Body.Bodyparts)
                {
                    if (bodyPart is VarDecleration)
                    {
                        (bodyPart as VarDecleration).Accept(this);
                    }
                }
                var Value = _expressionChecker.TypeChecker(functionDeclaration.TypeId.ValueType.ToString());
                if (functionDeclaration.TypeId.ValueType.ToString() == "void")
                    returnFound = true;
                else
                    returnFound = _expressionChecker.ReturnChecker(Value, functionDeclaration.Body.Bodyparts);
                if (returnFound == false)
                    Errors.Add($"{LineColumnString(functionDeclaration)}Not all paths in \"{functionDeclaration.TypeId.Identifier.Id}\" return a value");
                Funcs.Add(functionDeclaration.TypeId.Identifier.Id,
                    new Function(functionDeclaration,
                        _expressionChecker.TypeChecker(functionDeclaration.TypeId.ValueType.ToString())));
                Identifiers = preBodyIdentifiers;
            }
            else
                Errors.Add($"{LineColumnString(functionDeclaration)}\"{functionDeclaration.TypeId.Identifier.Id}\" was declared twice");
        }

        public override void Visit(IfStatement ifStatement)
        {
            if (!(_expressionChecker.CheckExpression(ifStatement.Expression) is BoolValue))
                Errors.Add($"{LineColumnString(ifStatement)}if statement expects boolean but got \"{_expressionChecker.CheckExpression(ifStatement.Expression).GetType().Name}\"");
            Visit(ifStatement.Body);
            Visit(ifStatement.ElseStatement);
        }

        public override void Visit(RepeatExpr repeatExpr)
        {
            if (!(_expressionChecker.CheckExpression(repeatExpr.Expression) is BoolValue))
                Errors.Add($"{LineColumnString(repeatExpr)}repeat expects a boolean but got \"{_expressionChecker.CheckExpression(repeatExpr.Expression).GetType().Name}\"");
            Visit(repeatExpr.Body);
        }

        public override void Visit(RepeatFor repeatFor)
        {
            var preRepeatIdentifiers = saveScope();
            Visit(repeatFor.VarDecleration);

            if (!(Identifiers[repeatFor.VarDecleration.Identifier.Id] is NumValue))
                Errors.Add($"{LineColumnString(repeatFor)}\"{Identifiers[repeatFor.VarDecleration.Identifier.Id]}\" was expected to be a numeric value, but was instead " +
                           $"\"{Identifiers[repeatFor.VarDecleration.Identifier.Id].GetType().Name}\"");
            if (!(_expressionChecker.CheckExpression(repeatFor.Expression) is NumValue))
                Errors.Add($"{LineColumnString(repeatFor)}Expected a numeric value in repeat expression but got " +
                           $"\"{_expressionChecker.CheckExpression(repeatFor.Expression).GetType().Name}\"");

            Visit(repeatFor.Direction);
            Visit(repeatFor.Body);
            Identifiers = preRepeatIdentifiers;
        }

        public override void Visit(Refrence refrence)
            //Bliver kun besøgt hvis refrence er en bodypart aka når vi har et funktionskald uden for en vardecl.
        {
            _expressionChecker.NakedFuncCallChecker(refrence);
        }

        public override void Visit(FuncCall funcCall)
        {
            funcCall.IsBodyPart = true;
            funcCall.Identifier.Id = "program_" + funcCall.Identifier.Id;
            if (!Funcs.ContainsKey(funcCall.Identifier.Id))
            {
                Errors.Add($"{LineColumnString(funcCall)}\"{funcCall.Identifier.Id}\" was not declared but was used in the code");
            }
            else
                _expressionChecker.ParameterChecker(Funcs[funcCall.Identifier.Id].FuncDecl, funcCall);
        }
        public string LineColumnString(AbstractSyntaxTree nodeElement)
        {
            return $"L:{nodeElement.Line}, C:{nodeElement.Column}:: ";
        } 

        #endregion
    }

    public class UnInitialisedVariable : IValue
    {
        public void Accept(IEecVisitor visitor)
        {
            //Skal aldrig accepteres så bliver aldrig kaldt (y)
        }
    }

    public class StructValue : IValue
    {
        public StructValue(StructDefinition identifier)
        {
            Struct = identifier;
        }

        public StructDefinition Struct { get; set; }

        public void Accept(IEecVisitor visitor)
        {
            //Skal aldrig accepteres så bliver aldrig kaldt (y)
        }
    }

    public class Function : IValue
    {
        public Function(FunctionDeclaration func, IValue value)
        {
            FuncDecl = func;
            Value = value;
        }

        public IValue Value { get; set; }
        public FunctionDeclaration FuncDecl { get; set; }

        public void Accept(IEecVisitor visitor)
        {
            //bliver aldrig kaldt
        }
    }

    public class ListValue : IValue
    {
        public ListValue(ListType listType, IValue value)
        {
            Type = listType;
            Value = value;
        }

        public ListType Type { get; set; }
        public IValue Value { get; set; }

        public void Accept(IEecVisitor visitor)
        {
            //bliver aldrig kaldt
        }
    }
}