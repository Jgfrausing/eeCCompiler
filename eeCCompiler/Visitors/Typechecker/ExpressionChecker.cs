using System.Collections.Generic;
using System.Linq;
using eeCCompiler.Interfaces;
using eeCCompiler.Nodes;
using Type = System.Type;

namespace eeCCompiler.Visitors
{
    public class ExpressionChecker
    {
        private readonly Typechecker _typechecker;

        public ExpressionChecker(Typechecker typechecker)
        {
            _typechecker = typechecker;
        }

        public IValue CheckExpression(IExpression expression)
        {
            var expressionType = expression.GetType();

            #region ExpressionVal

            if (expression is ExpressionVal)
            {
                var exp = expression as ExpressionVal;
                IValue value = null;
                if (exp.Value is Refrence)
                {
                    var refrence = exp.Value as Refrence;
                    if (_typechecker.Identifiers.ContainsKey(refrence.StructRefrence.ToString()))
                    {
                        var structRefrence = _typechecker.Identifiers[refrence.StructRefrence.ToString()];
                        if (structRefrence is ListValue && refrence.StructRefrence is IdIndex)
                        {
                            var structType = CheckValueType((structRefrence as ListValue).Value);
                            (refrence.StructRefrence as IdIndex).Identifier.Type.ValueType = structType;
                            value = StructRefrenceChecker(refrence, structType, refrence);

                            exp.Value = refrence;
                        }
                        else if (_typechecker.Identifiers[refrence.StructRefrence.ToString()] is StructValue)
                        {
                            var structType =
                                (_typechecker.Identifiers[refrence.StructRefrence.ToString()] as StructValue).Struct
                                    .Identifier.Id;
                            value = StructRefrenceChecker(refrence, structType, refrence);
                            exp.Value = refrence;
                        }
                        else
                        {
                            if (refrence.Identifier is FuncCall)
                            {
                                refrence.IsFuncCall = true;
                                if ((refrence.Identifier as FuncCall).Identifier.Id == "count")
                                    value = new NumValue(2.0);
                                else
                                    value = new UnInitialisedVariable();
                                //refrence.StructRefrence
                                ListFuncChecker(refrence.Identifier as FuncCall,
                                    _typechecker.Identifiers[refrence.StructRefrence.ToString()] as ListValue);
                            }
                            else if (refrence.Identifier is IdIndex)
                            {
                                var structRef = _typechecker.Identifiers[refrence.StructRefrence.ToString()];
                                IValue val;
                                foreach (var varDecl in (structRef as StructValue).Struct.StructParts.StructPartList)
                                {
                                    if (varDecl is VarDecleration &&
                                        (varDecl as VarDecleration).Identifier.Id ==
                                        (refrence.Identifier as IdIndex).Identifier.Id)
                                        val = CheckExpression((varDecl as VarDecleration).Expression);
                                }
                                var listIden = refrence.Identifier as IdIndex; // identifier på liste
                            }
                            else
                            {
                                _typechecker.Errors.Add($"{_typechecker.LineColumnString(refrence)}Lists do not contain any variables");
                                value = new UnInitialisedVariable();
                            }
                        }
                    }
                    else
                    {
                        _typechecker.Errors.Add($"{_typechecker.LineColumnString(refrence)}\"{refrence.StructRefrence}\" struct was not found");
                        value = new UnInitialisedVariable();
                    }
                }
                else if (exp.Value is Identifier)
                {
                    if (_typechecker.Identifiers.ContainsKey((exp.Value as Identifier).Id))
                        value = _typechecker.Identifiers[(exp.Value as Identifier).Id];
                    else
                    {
                        value = new UnInitialisedVariable();
                        _typechecker.Errors.Add($"{_typechecker.LineColumnString(exp)}\"{(exp.Value as Identifier).Id}\" was not initialised before use");
                    }
                }
                else if (exp.Value is FuncCall) //Func i vardecl
                {
                    (exp.Value as FuncCall).Identifier.Id = "program_" + (exp.Value as FuncCall).Identifier;
                    var id = (exp.Value as FuncCall).Identifier.Id;

                    if (_typechecker.Funcs.ContainsKey(id))
                    {
                        value = _typechecker.Funcs[id].Value;
                        //Check om kald og parametre stemmer overens
                        ParameterChecker(_typechecker.Funcs[id].FuncDecl, exp.Value as FuncCall);
                    }
                    else
                    {
                        _typechecker.Errors.Add($"{_typechecker.LineColumnString(exp)}\"{(exp.Value as FuncCall).Identifier.Id}\" function call was not found");
                        value = new UnInitialisedVariable();
                    }
                }
                else if (exp.Value is IdIndex)
                {
                    var idIndex = exp.Value as IdIndex;
                    if (_typechecker.Identifiers.ContainsKey(idIndex.Identifier.Id))
                    {
                        if (_typechecker.Identifiers[idIndex.Identifier.Id] is ListValue)
                        {
                            value = (_typechecker.Identifiers[idIndex.Identifier.Id] as ListValue).Value;
                        }
                        else
                        {
                            _typechecker.Errors.Add($"{_typechecker.LineColumnString(exp)}Expected a List but identifier \"{idIndex.Identifier.Id}\" is of type " +
                                                    $"\"{_typechecker.Identifiers[idIndex.Identifier.Id].GetType()}\"");
                            value = new UnInitialisedVariable();
                        }
                    }
                    else
                    {
                        value = new UnInitialisedVariable();
                        _typechecker.Errors.Add($"{_typechecker.LineColumnString(exp)}\"{idIndex.Identifier.Id}\" was not found");
                    }
                    return value;
                }
                else if (exp.Value is StringValue)
                {
                    StringConverter(exp.Value as StringValue);
                    value = exp.Value;
                    //Regularexpression på forvidt vi har \{ eller { og check om variablen eksistere i {VariableHer}
                }
                else
                    value = exp.Value;
                return value;
            }
                #endregion

                #region ExpressionParen
            if (expression is ExpressionParen)
            {
                return CheckExpression((expression as ExpressionParen).Expression);
            }
                #endregion

                #region ExpressionNegate
            if (expression is ExpressionNegate)
            {
                var value = CheckExpression((expression as ExpressionNegate).Expression);
                if (!(value is BoolValue) && !(value is UnInitialisedVariable))
                    _typechecker.Errors.Add($"{_typechecker.LineColumnString(expression as ExpressionNegate)}\"{expressionType.Name}\" tried with \"{value.GetType().Name}\"");
                return value;
            }
                #endregion

                #region ExpressionMinus
            if (expression is ExpressionMinus)
            {
                var value = CheckExpression((expression as ExpressionMinus).Expression);
                if (!(value is NumValue) && !(value is UnInitialisedVariable))
                    _typechecker.Errors.Add(expressionType.Name + " with " + value.GetType().Name);
                return value;
            }
                #endregion

                #region ExpressionValOpExpr
            if (expression is ExpressionValOpExpr)
            {
                if ((expression as ExpressionValOpExpr).Value is StringValue)
                    StringConverter((expression as ExpressionValOpExpr).Value as StringValue);
                var expressionValOpExpr = expression as ExpressionValOpExpr;
                IValue value1 = null;
                if (expressionValOpExpr.Value is Refrence)
                {
                    var id = ((expressionValOpExpr.Value as Refrence).StructRefrence as Identifier).Id;
                    if (_typechecker.Identifiers.ContainsKey(id))
                        value1 = _typechecker.Identifiers[id];
                    else
                    {
                        value1 = new UnInitialisedVariable();
                        _typechecker.Errors.Add(id + " Reference was not initialised before use");
                    }
                }
                else if (expressionValOpExpr.Value is Identifier)
                {
                    var id = (expressionValOpExpr.Value as Identifier).Id;
                    if (_typechecker.Identifiers.ContainsKey(id))
                        value1 = _typechecker.Identifiers[id];
                    else
                    {
                        value1 = new UnInitialisedVariable();
                        _typechecker.Errors.Add(id + " identifier was not initialised before use");
                    }
                }
                else
                    value1 = expressionValOpExpr.Value;

                return OprChecker(value1, CheckExpression(expressionValOpExpr.Expression), expressionValOpExpr.Operator,
                    expressionType);
            }
                #endregion

                #region ExpressionParenOpExpr
            if (expression is ExpressionParenOpExpr)
            {
                var expressionParenOpExpr = expression as ExpressionParenOpExpr;

                var value1 = CheckExpression(expressionParenOpExpr.ExpressionParen);
                var value2 = CheckExpression(expressionParenOpExpr.Expression);

                return OprChecker(value1, value2, expressionParenOpExpr.Operator, expressionType);
            }
            if (expression is ExpressionExprOpExpr)
            {
                var expressionParenOpExpr = expression as ExpressionExprOpExpr;

                var value1 = CheckExpression(expressionParenOpExpr.ExpressionParen);
                var value2 = CheckExpression(expressionParenOpExpr.Expression);

                return OprChecker(value1, value2, expressionParenOpExpr.Operator, expressionType);
            }
                #endregion

                #region FuncCall
            if (expression is FuncCall) //Ikke sikker på vi nogensinde får... skal testes!
            {
                //Check om input matcher det som er deklaræret
                var funcCall = expression as FuncCall;
                IValue value = null;
                if (_typechecker.Funcs.ContainsKey(funcCall.Identifier.Id))
                {
                    var valueType = _typechecker.Funcs[funcCall.Identifier.Id].FuncDecl.TypeId.ValueType.ToString();
                    value = TypeChecker(valueType);
                }
                else
                {
                    value = new UnInitialisedVariable();
                    _typechecker.Errors.Add(funcCall.Identifier.Id + " function has not been declared");
                }
                return value; // TODO ikke færdig not sure here.
            }
                #endregion

                #region ListType
            if (expression is ListType)
            {
                return new ListValue(expression as ListType, TypeChecker((expression as ListType).Type.ToString()));
            }

            #endregion

            _typechecker.Errors.Add("FATAL ERROR IN COMPILER EXPRESSION NOT CAUGHT IN TYPECHECKER");
            return new UnInitialisedVariable(); //Burde vi aldrig nå tror jeg
        }

        #region CheckerFunctions

        public void StringConverter(StringValue stringValue)
        {
            var input = stringValue.Value;
            bool escaped = false, firstChar = false;
            var isIdentifier = false;
            var stringParts = new List<IStringPart> {new StringValue("")};
            foreach (var charactor in input.ToCharArray(0, input.Length))
            {
                if (!escaped)
                {
                    if (charactor == '\\')
                    {
                        (stringParts[stringParts.Count - 1] as StringValue).Value += charactor;
                        escaped = true;
                        continue;
                    }
                    if (charactor == '{')
                    {
                        stringParts.Add(new TypeId(new Identifier(""), new Nodes.Type(" ")));
                        isIdentifier = true;
                        firstChar = true;
                    }
                    else if (charactor == '}')
                    {
                        isIdentifier = false;
                        stringParts.Add(new StringValue(""));
                    }
                    else if (firstChar && ValidateFirstChar(charactor))
                    {
                        firstChar = false;
                        (stringParts[stringParts.Count - 1] as TypeId).Identifier.Id += charactor;
                    }
                    else if (isIdentifier && ValidateIdentifier(charactor))
                    {
                        (stringParts[stringParts.Count - 1] as TypeId).Identifier.Id += charactor;
                    }
                    else if (!isIdentifier)
                    {
                        if (charactor != '"')
                            (stringParts[stringParts.Count - 1] as StringValue).Value += charactor;
                    }
                    else
                    {
                        _typechecker.Errors.Add("Identifier in string " + input + ". Has " + charactor +
                                                " which is not allowed for identifiers");
                    }
                }
                else
                {
                    (stringParts[stringParts.Count - 1] as StringValue).Value += charactor;
                }
                escaped = false;
            }
            for (var i = 0; i < stringParts.Count; i++)
            {
                if (stringParts[i] is StringValue)
                {
                    (stringParts[i] as StringValue).Value = ((StringValue) stringParts[i]).Value.Replace(@"\{", "{");
                    (stringParts[i] as StringValue).Value = ((StringValue) stringParts[i]).Value.Replace(@"\}", "}");
                }
                else if (stringParts[i] is TypeId)
                {
                    (stringParts[i] as TypeId).Identifier.Id = ((TypeId) stringParts[i]).Identifier.Id.Replace(@"\{",
                        "{");
                    (stringParts[i] as TypeId).Identifier.Id = ((TypeId) stringParts[i]).Identifier.Id.Replace(@"\{",
                        "{");
                }
            }
            foreach (TypeId typeId in stringParts.Where(x => x is TypeId))
            {
                if (_typechecker.Identifiers.ContainsKey(typeId.Identifier.Id))
                {
                    if (_typechecker.Identifiers[typeId.Identifier.Id] is NumValue)
                        (typeId.ValueType as Nodes.Type).ValueType = "num";
                    else if (_typechecker.Identifiers[typeId.Identifier.Id] is StringValue)
                        (typeId.ValueType as Nodes.Type).ValueType = "string";
                    else if (_typechecker.Identifiers[typeId.Identifier.Id] is BoolValue)
                        (typeId.ValueType as Nodes.Type).ValueType = "bool";
                    else
                        _typechecker.Errors.Add("A " + _typechecker.Identifiers[typeId.Identifier.Id].GetType().Name +
                                                " can no be used in a string");
                }
            }
            stringValue.Elements = stringParts;
        }

        private bool ValidateIdentifier(char charactor)
        {
            return ValidateFirstChar(charactor) || (charactor >= '0' && charactor <= '9');
        }

        private bool ValidateFirstChar(char charactor)
        {
            return (charactor >= 'A' && charactor <= 'Z') || (charactor >= 'a' && charactor <= 'z') ||
                   (charactor == '_');
        }

        public bool NumChecker(IValue value, Operator opr)
        {
            return value is NumValue &&
                   (opr.Symbol == Indexes.Indexes.SymbolIndex.Plus ||
                    opr.Symbol == Indexes.Indexes.SymbolIndex.Minus ||
                    opr.Symbol == Indexes.Indexes.SymbolIndex.Div ||
                    opr.Symbol == Indexes.Indexes.SymbolIndex.Times ||
                    opr.Symbol == Indexes.Indexes.SymbolIndex.Mod);
        }

        public bool BoolNumChecker(IValue value, Operator opr)
        {
            return value is NumValue &&
                   (opr.Symbol == Indexes.Indexes.SymbolIndex.Eqeq ||
                    opr.Symbol == Indexes.Indexes.SymbolIndex.Exclameq ||
                    opr.Symbol == Indexes.Indexes.SymbolIndex.Lt ||
                    opr.Symbol == Indexes.Indexes.SymbolIndex.Lteq ||
                    opr.Symbol == Indexes.Indexes.SymbolIndex.Gt ||
                    opr.Symbol == Indexes.Indexes.SymbolIndex.Gteq);
        }

        public bool BoolChecker(IValue value, Operator opr)
        {
            return value is BoolValue &&
                   !(opr.Symbol == Indexes.Indexes.SymbolIndex.Exclameq ||
                     opr.Symbol == Indexes.Indexes.SymbolIndex.Eqeq ||
                     opr.Symbol == Indexes.Indexes.SymbolIndex.And ||
                     opr.Symbol == Indexes.Indexes.SymbolIndex.Or);
        }

        public string CheckValueType(IValue value)
            //Problemet er, at returværdien her bliver brugt til at sætte en type, men typen bliver ikke brugt. Derfor bliver IsListValue ikke gemt! 
        {
            if (value is NumValue)
                return "num";
            if (value is StringValue)
                return "string";
            if (value is BoolValue)
                return "bool";
            if (value is StructValue)
                return (value as StructValue).Struct.Identifier.Id;
            if (value is ListValue)
                //return (value as ListValue).Type.Type.ToString()+"[]";
            {
                if ((value as ListValue).Type.Type is Nodes.Type)
                    ((value as ListValue).Type.Type as Nodes.Type).IsListValue = true;
                else if ((value as ListValue).Type.Type is Identifier)
                    ((value as ListValue).Type.Type as Identifier).Type.IsListValue = true;
                else
                {
                    var a = (value as ListValue).Type.Type.GetType();
                }
                return (value as ListValue).Type.Type.ToString();
            }
            return "404 type not found";
        }

        public IValue TypeChecker(string Type)
        {
            IValue value;
            switch (Type)
            {
                case "num":
                    value = new NumValue(2.0);
                    break;
                case "void":
                    value = new UnInitialisedVariable();
                    break;
                case "string":
                    value = new StringValue("");
                    break;
                case "bool":
                    value = new BoolValue(true);
                    break;
                default:
                    if (_typechecker.Structs.ContainsKey(Type))
                    {
                        value = new StructValue(_typechecker.Structs[Type]);
                    }
                    else
                    {
                        _typechecker.Errors.Add(Type + " struct identifier was not found");
                        value = new UnInitialisedVariable();
                    }
                    break;
            }
            return value;
        }

        public IValue OprChecker(IValue value1, IValue value2, Operator opr, Type expressionType)
        {
            if (value1 == null || value2 == null || value1.GetType().Name == "UnInitialisedVariable" ||
                value2.GetType().Name == "UnInitialisedVariable")
            {
            } // Ignoreres bare, sker hvis variablen ikke blev instansieret før brug

            else if (value2.GetType().Name != value1.GetType().Name)
                //Hvis begge værdier ikke er ens accepteres det ikke i vores grammatik
                _typechecker.Errors.Add(expressionType.Name + " with " + value1.GetType().Name + " and " +
                                        value2.GetType().Name);

            else if (value1 is BoolValue && BoolChecker(value1, opr))
                //Checker om en lovlig operator bliver brugt i bool opr bool
                _typechecker.Errors.Add(expressionType.Name + " wrong operator tried " + value1.GetType().Name + " " +
                                        opr.Symbol +
                                        " " + value2.GetType().Name);

            else if (value1 is BoolValue && !BoolChecker(value1, opr))
                return value1;

            else if (value1 is StringValue &&
                     (opr.Symbol == Indexes.Indexes.SymbolIndex.Exclameq ||
                      opr.Symbol == Indexes.Indexes.SymbolIndex.Eqeq))
            {
                opr.IsStringOpr = true;
                return new BoolValue(true);
            }
            //Værdi ubetydelig men hvis vi har string opr string så evaluere det til en bool, hvis en af ovenstående operatore.

            else if (value1 is StringValue &&
                     !(opr.Symbol == Indexes.Indexes.SymbolIndex.Exclameq ||
                       opr.Symbol == Indexes.Indexes.SymbolIndex.Eqeq ||
                       opr.Symbol == Indexes.Indexes.SymbolIndex.Plus))
                _typechecker.Errors.Add(expressionType.Name + " wrong operator tried " + value1.GetType().Name + " " +
                                        opr.Symbol +
                                        " " + value2.GetType().Name);

            if (!(NumChecker(value1, opr) || BoolNumChecker(value1, opr)))
                _typechecker.Errors.Add(expressionType.Name + " wrong operator tried " + value1.GetType().Name + " " +
                                        opr.Symbol +
                                        " " + value2.GetType().Name);

            else if (NumChecker(value1, opr)) //Checker om num operator bliver brugt i num opr num
                return value1;

            else if (BoolNumChecker(value1, opr))
                return new BoolValue(true);
            //Værdi ubetydelig men hvis vi har num opr num så evaluere det til en bool, hvis ikke en af ovenstående operatore.


            return value1;
        }

        public IValue StructRefrenceChecker(Refrence refrence, string structType, Refrence exp)
        {
            IValue value = new UnInitialisedVariable();
            var valueFound = false;
            //var id = refrence.StructRefrence.ToString();
            //var structType = (Identifiers[id] as StructValue).Struct.Identifier.Id;

            if (refrence.Identifier is Identifier)
            {
                foreach (var structpart in _typechecker.Structs[structType].StructParts.StructPartList)
                {
                    if (structpart is VarDecleration)
                    {
                        var vardecl = structpart as VarDecleration;
                        if (vardecl.Identifier.Id == (refrence.Identifier as Identifier).Id)
                        {
                            value = CheckExpression(vardecl.Expression);
                            valueFound = true;
                            break;
                        }
                    }
                }
            }
            else if (refrence.Identifier is FuncCall)
            {
                (refrence.Identifier as FuncCall).Identifier.Id = structType + "_" +
                                                                  (refrence.Identifier as FuncCall).Identifier.Id;
                exp.IsFuncCall = true;
                exp.FuncsStruct = structType;
                foreach (var structpart in _typechecker.Structs[structType].StructParts.StructPartList)
                {
                    if (structpart is FunctionDeclaration)
                    {
                        var funcdecl = structpart as FunctionDeclaration;
                        if (funcdecl.TypeId.Identifier.Id == (refrence.Identifier as FuncCall).Identifier.Id)
                        {
                            value = TypeChecker(funcdecl.TypeId.ValueType.ToString());
                            ParameterChecker(funcdecl, refrence.Identifier as FuncCall);
                            valueFound = true;
                            break;
                        }
                    }
                }
            }
            else if (refrence.Identifier is IdIndex)
            {
                foreach (var structpart in _typechecker.Structs[structType].StructParts.StructPartList)
                {
                    if (structpart is VarDecleration)
                    {
                        if ((structpart as VarDecleration).Identifier.Id ==
                            (refrence.Identifier as IdIndex).Identifier.Id)
                        {
                            value = CheckExpression((structpart as VarDecleration).Expression);
                            if (value is ListValue)
                            {
                                value = (value as ListValue).Value;
                            }
                            else
                            {
                                _typechecker.Errors.Add("Expected a list but got a " + value.GetType());
                            }
                            valueFound = true;
                            break;
                        }
                    }
                }
            }
            else
            {
                foreach (var structpart  in _typechecker.Structs[structType].StructParts.StructPartList)
                {
                    if (structpart is StructDecleration)
                    {
                        var structDecleration = structpart as StructDecleration;
                        if (refrence.Identifier is Refrence)
                        {
                            if (structDecleration.Identifier.Id ==
                                (refrence.Identifier as Refrence).StructRefrence.ToString())
                            {
                                value = StructRefrenceChecker(refrence.Identifier as Refrence,
                                    structDecleration.StructIdentifier.Id, exp);
                                valueFound = true;
                                break;
                            }
                        }
                    }
                    else if (structpart is VarDecleration)
                    {
                        var varDecl = structpart as VarDecleration;
                        if (varDecl.Identifier.Id == (refrence.Identifier as Refrence).StructRefrence.ToString())
                        {
                            if (CheckExpression(varDecl.Expression) is ListValue)
                            {
                                var listRefrence = refrence.Identifier as Refrence;
                                if (listRefrence.Identifier is FuncCall)
                                {
                                    var funcCall = listRefrence.Identifier as FuncCall;
                                    funcCall.Identifier.Type.ValueType = varDecl.Identifier.Type.ValueType;
                                    if (funcCall.Identifier.Id == "count")
                                        value = new NumValue(2.0);
                                    else
                                        value = new UnInitialisedVariable();
                                    ListFuncChecker(funcCall, CheckExpression(varDecl.Expression) as ListValue);
                                    valueFound = true;
                                }
                                else
                                {
                                    value = StructRefrenceChecker(listRefrence,
                                        ((CheckExpression(varDecl.Expression) as ListValue).Type.Type as Identifier).Id,
                                        exp);
                                    valueFound = true;
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            if (value is UnInitialisedVariable && !valueFound)
                _typechecker.Errors.Add("A refrence did not exist");
            return value;
        }

        public void NakedFuncCallChecker(Refrence refrence)
        {
            refrence.IsFuncCall = true;
            if (_typechecker.Identifiers.ContainsKey(refrence.StructRefrence.ToString()))
            {
                if (_typechecker.Identifiers[refrence.StructRefrence.ToString()] is StructValue)
                {
                    var refid = refrence.Identifier;
                    while (!(refid is FuncCall))
                    {
                        refid = (refid as Refrence).Identifier;
                    }
                    (refid as FuncCall).IsBodyPart = true;

                    var value = StructRefrenceChecker(refrence,
                        (_typechecker.Identifiers[refrence.StructRefrence.ToString()] as StructValue).Struct.Identifier
                            .Id, refrence);
                }

                else if (_typechecker.Identifiers[refrence.StructRefrence.ToString()] is ListValue)
                {
                    if (refrence.Identifier is FuncCall)
                    {
                        (refrence.Identifier as FuncCall).IsBodyPart = true;
                        (refrence.Identifier as FuncCall).Identifier.Type.ValueType =
                            CheckValueType(_typechecker.Identifiers[refrence.StructRefrence.ToString()]);
                        ListFuncChecker(refrence.Identifier as FuncCall,
                            _typechecker.Identifiers[refrence.StructRefrence.ToString()] as ListValue);
                    }
                    else
                    {
                        _typechecker.Errors.Add("Lists do not have any values associated, please use a function instead");
                    }
                }
            }
            else
                _typechecker.Errors.Add(refrence.StructRefrence + " was not found");
        }

        public void ListFuncChecker(FuncCall funcCall, ListValue listValue)
        {
            if (funcCall.Identifier.Id == "add")
            {
                if (funcCall.Expressions.Count == 1)
                {
                    if (
                        !(CheckExpression(funcCall.Expressions[0] as IExpression).GetType().ToString() ==
                          listValue.Value.GetType().ToString())) //Kan jeg det !!JONATAN DA FUQ!!
                        _typechecker.Errors.Add("List is of type " + listValue.GetType() + " but a " +
                                                (funcCall.Expressions[0] as IExpression).GetType() + " was tried added");
                }
                else if (funcCall.Expressions.Count > 1)
                    _typechecker.Errors.Add("Only one element can be added to a list at a time");
                else
                    _typechecker.Errors.Add("Add takes one parameter");
            }
            else if (funcCall.Identifier.Id == "insert")
            {
                if (funcCall.Expressions.Count == 2)
                {
                    if (!(CheckExpression(funcCall.Expressions[0] as IExpression) is NumValue))
                    {
                        _typechecker.Errors.Add("You can only insert at a num index");
                    } //Kan jeg det !!JONATAN DA FUQ!!
                    else if (
                        !(CheckExpression(funcCall.Expressions[1] as IExpression).GetType().ToString() ==
                          listValue.Value.GetType().ToString()))
                        _typechecker.Errors.Add("List is of type " + listValue.GetType() + " but a " +
                                                (funcCall.Expressions[1] as IExpression).GetType() +
                                                " was tried added");
                }
                else if (funcCall.Expressions.Count > 2)
                    _typechecker.Errors.Add(
                        "Too many parameters for a list insert. Insert takes two parameters: an index and an element");
                else
                    _typechecker.Errors.Add("Insert takes two parameters");
            }
            else if (funcCall.Identifier.Id == "remove")
            {
                if (funcCall.Expressions.Count == 1)
                {
                    if (!(CheckExpression(funcCall.Expressions[0] as IExpression) is NumValue))
                        //Kan jeg det !!JONATAN DA FUQ!!
                        _typechecker.Errors.Add("You can only remove at a num index");
                }
                else
                    _typechecker.Errors.Add("Only one element can be removed from a list at a time");
            }
            else if (funcCall.Identifier.Id == "clear")
            {
                if (funcCall.Expressions.Count > 0)
                    _typechecker.Errors.Add("clear() does not take any parameters");
            }
            else if (funcCall.Identifier.Id == "count")
            {
                if (funcCall.Expressions.Count > 0)
                    _typechecker.Errors.Add("count() does not take any parameters");
            }
            else if (funcCall.Identifier.Id == "reverse")
            {
                if (funcCall.Expressions.Count > 0)
                    _typechecker.Errors.Add("reverse() does not take any parameters");
            }
            else if (funcCall.Identifier.Id == "sort")
            {
                if (funcCall.Expressions.Count > 0)
                    _typechecker.Errors.Add("sort() does not take any parameters");
                else if (funcCall.Identifier.Type.ValueType != "num")
                    _typechecker.Errors.Add("sort() can not be used on " + funcCall.Identifier.Type.ValueType + " list");
            }
            else
                _typechecker.Errors.Add("Unknown list function: " + funcCall.Identifier);
        }

        public bool IfChecker(IValue value, IfStatement ifStatement)
        {
            var returnFound = ReturnChecker(value, ifStatement.Body.Bodyparts);
            var returnFound2 = true;
            if (!(ifStatement.ElseStatement is IfStatement))
                returnFound2 = ReturnChecker(value, ifStatement.ElseStatement.Body.Bodyparts);
            else
                returnFound2 = IfChecker(value, ifStatement.ElseStatement as IfStatement);

            if (returnFound == false || returnFound2 == false)
                return false;

            return true;
        }

        public bool ReturnChecker(IValue value, List<IBodypart> bodyParts)
        {
            var returnFound = false;
            var preBodyIdentifiers = new Dictionary<string, IValue>();
            foreach (var val in _typechecker.Identifiers)
            {
                preBodyIdentifiers.Add(val.Key, val.Value);
            }
            foreach (var bp in bodyParts)
            {
                if (bp is IfStatement)
                {
                    returnFound = IfChecker(value, bp as IfStatement);
                }
                else if (bp is Return)
                {
                    var type1 = value.GetType().Name;
                    var type2 = CheckExpression((bp as Return).Expression).GetType().Name;
                    returnFound = true;
                    if (type1 != type2)
                        _typechecker.Errors.Add("Return value of " + "!!!FunktionsNavnHer!!!" + " is not valid");
                }
            }
            _typechecker.Identifiers = preBodyIdentifiers;
            return returnFound;
        }

        public void ParameterChecker(FunctionDeclaration funcDecl, FuncCall funcCall)
        {
            if (funcDecl.Parameters.TypeIds.Count == funcCall.Expressions.Count)
            {
                var parametersCorrect = true;
                for (var i = 0; i < funcDecl.Parameters.TypeIds.Count; i++)
                {
                    IValue type1, type2;
                    var refBool = false;

                    if (funcCall.Expressions[i] is RefId)
                    {
                        if (_typechecker.Identifiers.ContainsKey((funcCall.Expressions[i] as RefId).Identifier.Id))
                        {
                            (funcCall.Expressions[i] as RefId).Type.ValueType =
                                CheckValueType(
                                    _typechecker.Identifiers[(funcCall.Expressions[i] as RefId).Identifier.Id]);
                            if (_typechecker.Identifiers[(funcCall.Expressions[i] as RefId).Identifier.Id] is ListValue)
                                (funcCall.Expressions[i] as RefId).Identifier.Type.IsListValue = true;
                            type1 = _typechecker.Identifiers[(funcCall.Expressions[i] as RefId).Identifier.Id];
                        }
                        else
                        {
                            _typechecker.Errors.Add((funcCall.Expressions[i] as RefId).Identifier.Id +
                                                    " does not exist at the call of " + funcCall.Identifier.Id);
                            type1 = new UnInitialisedVariable();
                        }
                        refBool = true;
                    }
                    else
                        type1 = CheckExpression(funcCall.Expressions[i] as IExpression);

                    CheckValueType(funcDecl.Parameters.TypeIds[i].TypeId.Identifier);
                    if (funcDecl.Parameters.TypeIds[i].TypeId.ValueType is ListType)
                    {
                        type2 = new ListValue(funcDecl.Parameters.TypeIds[i].TypeId.ValueType as ListType,
                            TypeChecker(funcDecl.Parameters.TypeIds[i].TypeId.ValueType.ToString()));
                    }
                    else
                        type2 = TypeChecker(funcDecl.Parameters.TypeIds[i].TypeId.ValueType.ToString());
                    if (type1 is ListValue && type2 is ListValue)
                    {
                        parametersCorrect = (type1 as ListValue).Value.GetType().ToString() ==
                                            (type2 as ListValue).Value.GetType().ToString() &&
                                            (type1 as ListValue).Type.Dimentions == (type1 as ListValue).Type.Dimentions;
                    }
                    else if (
                        !(type1.GetType().ToString() == type2.GetType().ToString() &&
                          (funcDecl.Parameters.TypeIds[i].Ref == refBool) || !(type1 is UnInitialisedVariable)))
                        parametersCorrect = false;
                }
                if (!parametersCorrect)
                {
                    _typechecker.Errors.Add("Parameters for " + funcCall.Identifier.Id + " was not correct");
                }
            }
            else
                _typechecker.Errors.Add("Parameters for " + funcCall.Identifier.Id + " was not correct");
        }

        #endregion
    }
}