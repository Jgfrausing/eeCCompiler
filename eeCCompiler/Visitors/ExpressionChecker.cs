using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using eeCCompiler.Interfaces;
using eeCCompiler.Nodes;
using Type = System.Type;

namespace eeCCompiler.Visitors
{
    public class ExpressionChecker
    {
        private Typechecker _typechecker;

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
                    var refrence = (exp.Value as Refrence);
                    if (_typechecker.Identifiers.ContainsKey(refrence.StructRefrence.ToString()))
                    {
                        if (_typechecker.Identifiers[refrence.StructRefrence.ToString()] is StructValue) {
                            var structType = (_typechecker.Identifiers[refrence.StructRefrence.ToString()] as StructValue).Struct.Identifier.Id;
                            value = StructRefrenceChecker(refrence, structType, refrence);
                            exp.Value = refrence;
                        }
                        else
                        {
                            if (refrence.Identifier is FuncCall)
                            {
                                if ((refrence.Identifier as FuncCall).Identifier.Id == "count")
                                    value = new NumValue(2.0);
                                else
                                    value = new UnInitialisedVariable();

                                ListFuncChecker(refrence.Identifier as FuncCall, _typechecker.Identifiers[refrence.StructRefrence.ToString()] as ListValue);
                            }
                            else
                            {
                                _typechecker.Errors.Add("Lists do not contain any fields"); 
                                value = new UnInitialisedVariable();
                            }
                        }

                    }
                    else
                    {
                        _typechecker.Errors.Add(refrence.StructRefrence + " struct refrence was not found");
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
                        _typechecker.Errors.Add((exp.Value as Identifier).Id + " Identifier was not initialised before use");
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
                        _typechecker.Errors.Add((exp.Value as FuncCall).Identifier.Id + " function call was not found");
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
                            _typechecker.Errors.Add("Expected a List but identifier " + idIndex.Identifier.Id + " is of type " +
                                _typechecker.Identifiers[idIndex.Identifier.Id].GetType().ToString());
                            value = new UnInitialisedVariable();
                        }
                    }
                    else
                    {
                        value = new UnInitialisedVariable();
                        _typechecker.Errors.Add(idIndex.Identifier.Id + " identifier was not found");
                    }
                    return value;
                }
                else
                    value = exp.Value;
                return value;
            }
                #endregion
            else if (expression is ExpressionParen)
            {
                return CheckExpression((expression as ExpressionParen).Expression);
            }
            else if (expression is ExpressionNegate)
            {
                var value = CheckExpression((expression as ExpressionNegate).Expression);
                if (!(value is BoolValue) && !(value is UnInitialisedVariable))
                    _typechecker.Errors.Add(expressionType.Name + " tried with " + value.GetType().Name);
                return value;
            }
            else if (expression is ExpressionMinus)
            {
                var value = CheckExpression((expression as ExpressionMinus).Expression);
                if (!(value is NumValue) && !(value is UnInitialisedVariable))
                    _typechecker.Errors.Add(expressionType.Name + " with " + value.GetType().Name);
                return value;
            }
            else if (expression is ExpressionValOpExpr)
            {
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
            else if (expression is ExpressionParenOpExpr)
            {
                var expressionParenOpExpr = expression as ExpressionParenOpExpr;

                var value1 = CheckExpression(expressionParenOpExpr.ExpressionParen);
                var value2 = CheckExpression(expressionParenOpExpr.Expression);

                return OprChecker(value1, value2, expressionParenOpExpr.Operator, expressionType);
            }
            else if (expression is FuncCall) //Ikke sikker p� vi nogensinde f�r... skal testes!
            {
                //Check om input matcher det som er deklar�ret
                var funcCall = expression as FuncCall;
                IValue value = null;
                if (_typechecker.Funcs.ContainsKey(funcCall.Identifier.Id))
                {
                    string valueType = _typechecker.Funcs[funcCall.Identifier.Id].FuncDecl.TypeId.ValueType.ToString();
                    value = TypeChecker(valueType);
                }
                else
                {
                    value = new UnInitialisedVariable();
                    _typechecker.Errors.Add(funcCall.Identifier.Id + " function has not been declared");
                }
                return value; // TODO ikke f�rdig not sure here.
            }
            else if (expression is ListType)
            {
                return new ListValue(expression as ListType, TypeChecker((expression as ListType).Type.ToString()));
            }
            _typechecker.Errors.Add("FATAL ERROR IN COMPILER EXPRESSION NOT CAUGHT IN TYPECHECKER");
            return new UnInitialisedVariable(); //Burde vi aldrig n� tror jeg
        }

        #region CheckerFunctions
        public bool NumChecker(IValue value, Operator opr)
        {
            return value is NumValue &&
                   !(opr.Symbol == Indexes.Indexes.SymbolIndex.Plus ||
                     opr.Symbol == Indexes.Indexes.SymbolIndex.Minus ||
                     opr.Symbol == Indexes.Indexes.SymbolIndex.Div ||
                     opr.Symbol == Indexes.Indexes.SymbolIndex.Times ||
                     opr.Symbol == Indexes.Indexes.SymbolIndex.Mod);
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
        {
            if (value is NumValue)
                return "num";
            else if (value is StringValue)
                return "string";
            else if (value is BoolValue)
                return "bool";
            else return "404 type not found";

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
            if (value1 == null || value2 == null || value1.GetType().Name == "UnInitialisedVariable" || value2.GetType().Name == "UnInitialisedVariable")
            {
            } // Ignoreres bare, sker hvis variablen ikke blev instansieret f�r brug

            else if ((value2.GetType().Name != value1.GetType().Name))
                //Hvis begge v�rdier ikke er ens accepteres det ikke i vores grammatik
                _typechecker.Errors.Add(expressionType.Name + " with " + value1.GetType().Name + " and " + value2.GetType().Name);

            else if (BoolChecker(value1, opr)) //Checker om en lovlig operator bliver brugt i bool opr bool
                _typechecker.Errors.Add(expressionType.Name + " wrong operator tried " + value1.GetType().Name + " " + opr.Symbol +
                                        " " + value2.GetType().Name);

            else if (NumChecker(value1, opr)) //Checker om bool operator bliver brugt i num opr num
                return new BoolValue(true);
            //V�rdi ubetydelig men hvis vi har num opr num s� evaluere det til en bool, hvis ikke en af ovenst�ende operatore.

            else if (value1 is StringValue &&
                     (opr.Symbol == Indexes.Indexes.SymbolIndex.Exclameq ||
                      opr.Symbol == Indexes.Indexes.SymbolIndex.Eqeq))
                return new BoolValue(true);
            //V�rdi ubetydelig men hvis vi har string opr string s� evaluere det til en bool, hvis en af ovenst�ende operatore.

            else if (value1 is StringValue &&
                     !(opr.Symbol == Indexes.Indexes.SymbolIndex.Exclameq ||
                       opr.Symbol == Indexes.Indexes.SymbolIndex.Eqeq ||
                       opr.Symbol == Indexes.Indexes.SymbolIndex.Plus))
                _typechecker.Errors.Add(expressionType.Name + " wrong operator tried " + value1.GetType().Name + " " + opr.Symbol +
                                        " " + value2.GetType().Name);
            return value1;
        }

        public IValue StructRefrenceChecker(Refrence refrence, string structType, Refrence exp)
        {
            IValue value = new UnInitialisedVariable();
            bool valueFound = false;
            //var id = refrence.StructRefrence.ToString();
            //var structType = (Identifiers[id] as StructValue).Struct.Identifier.Id;

            if (refrence.Identifier is Identifier)
            { 
                foreach (var structpart in _typechecker.Structs[structType].StructParts.StructPartList)
                {
                    if (structpart is VarDecleration)
                    {
                        VarDecleration vardecl = (structpart as VarDecleration);
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
                (refrence.Identifier as FuncCall).Identifier.Id = structType + "_" + (refrence.Identifier as FuncCall).Identifier.Id;
                exp.IsFuncCall = true;
                exp.FuncsStruct = structType;
                foreach (var structpart in _typechecker.Structs[structType].StructParts.StructPartList)
                {
                    if (structpart is FunctionDeclaration)
                    {
                        FunctionDeclaration funcdecl = (structpart as FunctionDeclaration);
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
                        if ((structpart as VarDecleration).Identifier.Id == (refrence.Identifier as IdIndex).Identifier.Id)
                        {
                            value = CheckExpression((structpart as VarDecleration).Expression);
                            if (value is ListValue)
                            {
                                value = (value as ListValue).Value;
                            }
                            else
                            {
                                _typechecker.Errors.Add("Expected list but got a " + value.GetType().ToString());
                            }
                            valueFound = true;
                            break;
                        }
                    }
                }
            }
            else
            {
                foreach (var structpart in _typechecker.Structs[structType].StructParts.StructPartList)
                {
                    if (structpart is StructDecleration)
                    {
                        StructDecleration structDecleration = (structpart as StructDecleration);
                        if (refrence.Identifier is Refrence)
                        {
                            if (structDecleration.Identifier.Id == (refrence.Identifier as Refrence).StructRefrence.ToString())
                            {
                                value = StructRefrenceChecker(refrence.Identifier as Refrence, structDecleration.StructIdentifier.Id,exp);
                                valueFound = true;
                                break;
                            }
                        }
                    }
                    else if (structpart is VarDecleration)
                    {
                        var varDecl  = (structpart as VarDecleration);
                        if (varDecl.Identifier.Id == (refrence.Identifier as Refrence).StructRefrence.ToString())
                        {
                            if ((CheckExpression(varDecl.Expression) is ListValue))
                            {
                                var listRefrence = (refrence.Identifier as Refrence);
                                if (listRefrence.Identifier is FuncCall)
                                {
                                    var funcCall = listRefrence.Identifier as FuncCall;
                                    if (funcCall.Identifier.Id == "count")
                                        value = new NumValue(2.0);
                                    else
                                        value = new UnInitialisedVariable();
                                    ListFuncChecker(funcCall, CheckExpression(varDecl.Expression) as ListValue);
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
            if (_typechecker.Identifiers.ContainsKey(refrence.StructRefrence.ToString()))
            {
                if (_typechecker.Identifiers[refrence.StructRefrence.ToString()] is StructValue)
                {
                    var refid = refrence.Identifier;
                    while (!(refid is FuncCall)) {
                        refid = (refid as Refrence).Identifier;
                    }
                    (refid as FuncCall).IsBodyPart = true;
                    var value = StructRefrenceChecker(refrence, (_typechecker.Identifiers[refrence.StructRefrence.ToString()] as StructValue).Struct.Identifier.Id, refrence);
                }

                else if (_typechecker.Identifiers[refrence.StructRefrence.ToString()] is ListValue)
                {
                    if (refrence.Identifier is FuncCall)
                    {
                        (refrence.Identifier as FuncCall).IsBodyPart = true;
                        ListFuncChecker((refrence.Identifier as FuncCall), _typechecker.Identifiers[refrence.StructRefrence.ToString()] as ListValue);
                    }
                    else
                    {
                        _typechecker.Errors.Add("Lists do not have any values associated to them, please use a function instead");
                    }
                }
            }
            else
                _typechecker.Errors.Add(refrence.StructRefrence.ToString() + " was not found");
        }

        public void ListFuncChecker(FuncCall funcCall, ListValue listValue)
        {
            if (funcCall.Identifier.Id == "add")
            {
                if (funcCall.Expressions.Count == 1)
                {
                    if (!(CheckExpression(funcCall.Expressions[0] as IExpression).GetType().ToString() == listValue.Value.GetType().ToString())) //Kan jeg det !!JONATAN DA FUQ!!
                        _typechecker.Errors.Add("List is of type " + listValue.GetType().ToString() + " but a " + (funcCall.Expressions[0] as IExpression).GetType().ToString() + " was tried to be added");
                }
                else
                    _typechecker.Errors.Add("Only one element can be added to a list at a time");
            }
            else if (funcCall.Identifier.Id == "insert")
            {
                if (funcCall.Expressions.Count == 2)
                {
                    if (!(CheckExpression(funcCall.Expressions[0] as IExpression).GetType().ToString() == "num"))
                    {
                        _typechecker.Errors.Add("You can only insert at a num index");
                    }//Kan jeg det !!JONATAN DA FUQ!!
                    else if (!(CheckExpression(funcCall.Expressions[1] as IExpression).GetType().ToString() == listValue.Value.GetType().ToString()))
                        _typechecker.Errors.Add("List is of type " + listValue.GetType().ToString() + " but a " + (funcCall.Expressions[1] as IExpression).GetType().ToString() + " was tried to be added");
                }
                else
                    _typechecker.Errors.Add("Too many parameters for a list insert");
            }
            else if (funcCall.Identifier.Id == "remove")
            {
                if (funcCall.Expressions.Count == 1)
                {
                    if (!(CheckExpression(funcCall.Expressions[0] as IExpression).GetType().ToString() == "num")) //Kan jeg det !!JONATAN DA FUQ!!
                        _typechecker.Errors.Add("You can only remove at a num index");
                }
                else
                    _typechecker.Errors.Add("Only one element can be removed from a list at a time");
            }
            else if (funcCall.Identifier.Id == "clear")
            {
                if (funcCall.Expressions.Count > 0)
                    _typechecker.Errors.Add("clear() does not take in parameters");
            }
            else if (funcCall.Identifier.Id == "count")
            {
                if (funcCall.Expressions.Count > 0)
                    _typechecker.Errors.Add("count() does not take in parameters");
            }
            else if (funcCall.Identifier.Id == "reverse")
            {
                if (funcCall.Expressions.Count > 0)
                    _typechecker.Errors.Add("reverse() does not take in parameters");
            }
            else if (funcCall.Identifier.Id == "sort")
            {
                if (funcCall.Expressions.Count > 0)
                    _typechecker.Errors.Add("sort() does not take in parameters");
            }
            else
                _typechecker.Errors.Add("unknown list function " + funcCall.Identifier.ToString());
        }

        public bool IfChecker(IValue value, IfStatement ifStatement)
        {
            bool returnFound = ReturnChecker(value, ifStatement.Body.Bodyparts);
            bool returnFound2 = true;
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
            bool returnFound = false;
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
                    string type1 = value.GetType().Name;
                    string type2 = CheckExpression((bp as Return).Expression).GetType().Name;
                    returnFound = true;
                    if (type1 != type2)
                        _typechecker.Errors.Add("return value of " + "!!!FunktionsNavnHer!!!" + " is not valid");
                }
            }
            _typechecker.Identifiers = preBodyIdentifiers;
            return returnFound;
        }

        public void ParameterChecker(FunctionDeclaration funcDecl, FuncCall funcCall)
        {
            if (funcDecl.Parameters.TypeIds.Count == funcCall.Expressions.Count)
            {
                bool parametersCorrect = true;
                for (int i = 0; i < funcDecl.Parameters.TypeIds.Count; i++)
                {
                    string type1 = "", type2 = "";
                    bool refBool = false;

                    if (funcCall.Expressions[i] is RefId)
                    {
                        if (_typechecker.Identifiers.ContainsKey((funcCall.Expressions[i] as RefId).Identifier.Id))
                            type1 = _typechecker.Identifiers[(funcCall.Expressions[i] as RefId).Identifier.Id].GetType().ToString();
                        else
                        {
                            _typechecker.Errors.Add((funcCall.Expressions[i] as RefId).Identifier.Id + " does not exist at the call of " + funcCall.Identifier.Id); 
                            type1 = "Uninitialized value";
                        }
                        refBool = true;
                    }
                    else
                        type1 = CheckExpression(funcCall.Expressions[i] as IExpression).GetType().ToString();

                    type2 = TypeChecker(funcDecl.Parameters.TypeIds[i].TypeId.ValueType.ToString()).GetType().ToString();

                    if (!(type1 == type2 && (funcDecl.Parameters.TypeIds[i].Ref == refBool) || type1 == "Uninitialized value"))
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