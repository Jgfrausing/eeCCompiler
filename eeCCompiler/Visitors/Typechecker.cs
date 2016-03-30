using System;
using System.Collections.Generic;
using eeCCompiler.Interfaces;
using eeCCompiler.Nodes;
using Type = System.Type;

namespace eeCCompiler.Visitors
{
    public class Typechecker : Visitor
    {
        public Typechecker(List<string> errors, Dictionary<string, IValue> identifiers)
        {
            Errors = errors;
            Identifiers = identifiers;
            Funcs = new Dictionary<string, FunctionDeclaration>();
            Structs = new Dictionary<string, StructDefinition>();
        }

        public List<string> Errors { get; set; }
        public Dictionary<string, IValue> Identifiers { get; set; }
        public Dictionary<string, FunctionDeclaration> Funcs { get; set; }
        public Dictionary<string, StructDefinition> Structs { get; set; }

        public IValue CheckExpression(IExpression expression)
        {
            var expressionType = expression.GetType();

            if (expression is ExpressionVal)
            {
                var exp = expression as ExpressionVal;
                IValue value = null;
                if (exp.Value is Refrence)
                {
                    var refrence = (exp.Value as Refrence);
                    var id = (refrence.Identifiers[0].Id); //Har fat i structen
                    if (Structs.ContainsKey(id))
                    {
                        value = StructRefrenceChecker(refrence);
                    }
                    else
                    {
                        value = new UnInitialisedVariable();
                        Errors.Add(id + " Reference was not initialised before use");
                    }
                }
                else
                    value = exp.Value;
                return value;
            }
            if (expression is ExpressionParen)
            {
                return CheckExpression((expression as ExpressionParen).Expression);
            }
            if (expression is ExpressionNegate)
            {
                var value = CheckExpression((expression as ExpressionNegate).Expression);
                if (!(value is BoolValue) && !(value is UnInitialisedVariable))
                    Errors.Add(expressionType.Name + " tried with " + value.GetType().Name);
                return value;
            }
            if (expression is ExpressionMinus)
            {
                var value = CheckExpression((expression as ExpressionMinus).Expression);
                if (!(value is NumValue) && !(value is UnInitialisedVariable))
                    Errors.Add(expressionType.Name + " with " + value.GetType().Name);
                return value;
            }
            if (expression is ExpressionValOpExpr)
            {
                var expressionValOpExpr = expression as ExpressionValOpExpr;
                IValue value1 = null;
                if (expressionValOpExpr.Value is Refrence)
                {
                    var id = ((expressionValOpExpr.Value as Refrence).StructRefrence as Identifier).Id;
                    if (Identifiers.ContainsKey(id))
                        value1 = Identifiers[id];
                    else
                    {
                        value1 = new UnInitialisedVariable(); 
                        Errors.Add(id + " Reference was not initialised before use");
                    }
                }
                else
                    value1 = expressionValOpExpr.Value;

                return OprChecker(value1, CheckExpression(expressionValOpExpr.Expression), expressionValOpExpr.Operator,
                    expressionType);
            }
            if (expression is ExpressionParenOpExpr)
            {
                var expressionParenOpExpr = expression as ExpressionParenOpExpr;

                var value1 = CheckExpression(expressionParenOpExpr.ExpressionParen);
                var value2 = CheckExpression(expressionParenOpExpr.Expression);

                return OprChecker(value1, value2, expressionParenOpExpr.Operator, expressionType);
            }
            if (expression is FuncCall)
            {
                //Check om input matcher det som er deklaræret
                var funcCall = expression as FuncCall;
                IValue value = null;
                if (Funcs.ContainsKey(funcCall.Identifier.Id))
                {
                    string valueType = Funcs[funcCall.Identifier.Id].TypeId.ValueType.ValueType;
                    value = TypeChecker(valueType);
                }
                else
                {
                    value = new UnInitialisedVariable();
                    Errors.Add(funcCall.Identifier.Id + " function has not been declared");
                }
                return value; // TODO ikke færdig not sure here.
            }
            return new NumValue(2.0); //Burde vi aldrig nå tror jeg
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

        public override void Visit(StructDefinition structDefinition)
        {
            if (!Structs.ContainsKey(structDefinition.Identifier.Id))
                Structs.Add(structDefinition.Identifier.Id, structDefinition);
            else
                Errors.Add(structDefinition.Identifier.Id + " was declared twice");
        }

        public override void Visit(ExpressionNegate expressionNegate)
        {
            CheckExpression(expressionNegate);
        }

        public override void Visit(ExpressionValOpExpr expressionValOpExpr)
        {
            CheckExpression(expressionValOpExpr);


            /*if(expression.Name == "ExpressionVal")
            {
                IValue expressionValue = (expressionValOpExpr.Expression as ExpressionVal).Value;
                if (expressionValue.GetType().Name == value1.Name)
                {
                    if (value1.Name == "NumValue" &&
                        expressionValOpExpr.Operator.Symbol == Indexes.Indexes.SymbolIndex.Plus ||
                        expressionValOpExpr.Operator.Symbol == Indexes.Indexes.SymbolIndex.Minus ||
                        expressionValOpExpr.Operator.Symbol == Indexes.Indexes.SymbolIndex.Div ||
                        expressionValOpExpr.Operator.Symbol == Indexes.Indexes.SymbolIndex.Times ||
                        expressionValOpExpr.Operator.Symbol == Indexes.Indexes.SymbolIndex.Mod)
                    {
                        Console.WriteLine("YOU DID A NUM RIGHT");
                    }
                    else if (value1.Name == "StringValue" &&
                            expressionValOpExpr.Operator.Symbol == Indexes.Indexes.SymbolIndex.Plus)
                    {
                        Console.WriteLine("YOU DID A STRING RIGHT");
                    }
                    else if (value1.Name == "BoolValue" &&
                        expressionValOpExpr.Operator.Symbol == Indexes.Indexes.SymbolIndex.And ||
                        expressionValOpExpr.Operator.Symbol == Indexes.Indexes.SymbolIndex.Eqeq ||
                        expressionValOpExpr.Operator.Symbol == Indexes.Indexes.SymbolIndex.Lt ||
                        expressionValOpExpr.Operator.Symbol == Indexes.Indexes.SymbolIndex.Lteq ||
                        expressionValOpExpr.Operator.Symbol == Indexes.Indexes.SymbolIndex.Gt ||
                        expressionValOpExpr.Operator.Symbol == Indexes.Indexes.SymbolIndex.Gteq ||
                        expressionValOpExpr.Operator.Symbol == Indexes.Indexes.SymbolIndex.Or ||
                        expressionValOpExpr.Operator.Symbol == Indexes.Indexes.SymbolIndex.Exclameq)
                    {
                        Console.WriteLine("YOU DID A BOOL RIGHT");
                    }*/
        }

        public override void Visit(ExpressionMinus expressionMinus)
        {
            CheckExpression(expressionMinus);
        }

        public override void Visit(ExpressionParen expressionParen)
        {
            CheckExpression(expressionParen);
        }

        public override void Visit(ExpressionParenOpExpr expressionParenOpExpr)
        {
            CheckExpression(expressionParenOpExpr);
        }

        public override void Visit(ExpressionVal expressionVal)
        {
            CheckExpression(expressionVal);
        }

        public override void Visit(Identifier identifier)
        {
            if (!Identifiers.ContainsKey(identifier.Id))
                Identifiers.Add(identifier.Id, identifier);
        }

        public override void Visit(VarDecleration varDecleration)
        {
            varDecleration.Identifier.Accept(this);
            varDecleration.AssignmentOperator.Accept(this);
            var value = CheckExpression(varDecleration.Expression);
            if (value.GetType().Name == "UnInitialisedVariable")
            {
                Errors.Add("Identifiier " + varDecleration.Identifier.Id + " was not assigned a value");
            }
            else if (Identifiers[varDecleration.Identifier.Id] is Identifier)
                Identifiers[varDecleration.Identifier.Id] = value;
            else if (Identifiers[varDecleration.Identifier.Id].GetType().Name == value.GetType().Name)
                Identifiers[varDecleration.Identifier.Id] = value;
            else
                Errors.Add("Identifier " + Identifiers[varDecleration.Identifier.Id] + " is of type " +
                           Identifiers[varDecleration.Identifier.Id].GetType().Name + " but a "
                           + value.GetType().Name + " was tried to be assigned to this identifier");
        }

        public override void Visit(Body body)
        {
            var preBodyIdentifiers = new Dictionary<string, IValue>();
            foreach (var val in Identifiers)
            {
                preBodyIdentifiers.Add(val.Key, val.Value);
            }
            base.Visit(body);
            Identifiers = preBodyIdentifiers;
        }

        #endregion

        #region Private checker metoder

        private bool NumChecker(IValue value, Operator opr)
        {
            return value is NumValue &&
                   !(opr.Symbol == Indexes.Indexes.SymbolIndex.Plus ||
                     opr.Symbol == Indexes.Indexes.SymbolIndex.Minus ||
                     opr.Symbol == Indexes.Indexes.SymbolIndex.Div ||
                     opr.Symbol == Indexes.Indexes.SymbolIndex.Times ||
                     opr.Symbol == Indexes.Indexes.SymbolIndex.Mod);
        }

        private bool BoolChecker(IValue value, Operator opr)
        {
            return value is BoolValue &&
                   !(opr.Symbol == Indexes.Indexes.SymbolIndex.Exclameq ||
                     opr.Symbol == Indexes.Indexes.SymbolIndex.Eqeq ||
                     opr.Symbol == Indexes.Indexes.SymbolIndex.And ||
                     opr.Symbol == Indexes.Indexes.SymbolIndex.Or);
        }
        private IValue TypeChecker(string Type)
        {
            IValue value;
            switch (Type)
            {
                case "Num":
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
                    if (Structs.ContainsKey(Type))
                    {
                        value = new StructValue(Structs[Type]);
                    }
                    else
                    {
                        Errors.Add(Type + " struct identifier was not found");
                        value = new UnInitialisedVariable();
                    }
                    break;
            }
            return value;
        }

        private IValue OprChecker(IValue value1, IValue value2, Operator opr, Type expressionType)
        {
            if (value1 == null || value2 == null || value1.GetType().Name == "UnInitialisedVariable" || value2.GetType().Name == "UnInitialisedVariable")
            {
            } // Ignoreres bare, sker hvis variablen ikke blev instansieret før brug

            else if ((value2.GetType().Name != value1.GetType().Name))
                //Hvis begge værdier ikke er ens accepteres det ikke i vores grammatik
                Errors.Add(expressionType.Name + " with " + value1.GetType().Name + " and " + value2.GetType().Name);

            else if (BoolChecker(value1, opr)) //Checker om en lovlig operator bliver brugt i bool opr bool
                Errors.Add(expressionType.Name + " wrong operator tried " + value1.GetType().Name + " " + opr.Symbol +
                           " " + value2.GetType().Name);

            else if (NumChecker(value1, opr)) //Checker om bool operator bliver brugt i num opr num
                return new BoolValue(true);
                    //Værdi ubetydelig men hvis vi har num opr num så evaluere det til en bool, hvis ikke en af ovenstående operatore.

            else if (value1 is StringValue &&
                     (opr.Symbol == Indexes.Indexes.SymbolIndex.Exclameq ||
                      opr.Symbol == Indexes.Indexes.SymbolIndex.Eqeq))
                return new BoolValue(true);
                    //Værdi ubetydelig men hvis vi har string opr string så evaluere det til en bool, hvis en af ovenstående operatore.

            else if (value1 is StringValue &&
                     !(opr.Symbol == Indexes.Indexes.SymbolIndex.Exclameq ||
                       opr.Symbol == Indexes.Indexes.SymbolIndex.Eqeq ||
                       opr.Symbol == Indexes.Indexes.SymbolIndex.Plus))
                Errors.Add(expressionType.Name + " wrong operator tried " + value1.GetType().Name + " " + opr.Symbol +
                           " " + value2.GetType().Name);
            return value1;
        }
        private IValue StructRefrenceChecker(Refrence refrence)
        {
            IValue value = new UnInitialisedVariable();
            var id = (refrence.Identifiers[0].Id);
            if (refrence.StructRefrence is Identifier)
            {
                foreach (var structpart in Structs[id].StructParts.StructPartList)
                {
                    if (structpart is VarDecleration)
                    {
                        VarDecleration vardecl = (structpart as VarDecleration);
                        if (vardecl.Identifier.Id == (refrence.StructRefrence as Identifier).Id)
                        {
                            value = CheckExpression(vardecl.Expression);
                            break;
                        }

                    }
                }
            }
            else
            {
                foreach (var structpart in Structs[id].StructParts.StructPartList)
                {
                    if (structpart is FunctionDeclaration)
                    {
                        FunctionDeclaration funcdecl = (structpart as FunctionDeclaration);
                        if (funcdecl.TypeId.Identifier.Id == (refrence.StructRefrence as FuncCall).Identifier.Id)
                        {
                            value = TypeChecker(funcdecl.TypeId.ValueType.ValueType);
                            break;
                        }

                    }
                }
            }
            if (value is UnInitialisedVariable)
            {
                Errors.Add(id + "." + refrence.StructRefrence.ToString() + " was not found");
            }



            return value;
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
            this.Identifier = identifier;
        }
        public StructDefinition Identifier { get; set; }
        public void Accept(IEecVisitor visitor)
        {
            //Skal aldrig accepteres så bliver aldrig kaldt (y)
        }
    }
}