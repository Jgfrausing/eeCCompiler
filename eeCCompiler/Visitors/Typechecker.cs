using System;
using System.Collections.Generic;
using eeCCompiler.Interfaces;
using eeCCompiler.Nodes;
using Type = System.Type;

namespace eeCCompiler.Visitors
{
    public class Typechecker : Visitor
    {
        private readonly ExpressionChecker _expressionChecker;

        public Typechecker(List<string> errors)
        {
            Errors = errors;
            Identifiers = new Dictionary<string, IValue>();
            Funcs = new Dictionary<string, Function>();
            Structs = new Dictionary<string, StructDefinition>();
            _expressionChecker = new ExpressionChecker(this);
        }

        public List<string> Errors { get; set; }
        public Dictionary<string, IValue> Identifiers { get; set; }
        public Dictionary<string, Function> Funcs { get; set; }
        public Dictionary<string, StructDefinition> Structs { get; set; }

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
                Errors.Add(structDefinition.Identifier.Id + " was declared twice");

            //RYD OP!!
            var preBodyIdentifiers = new Dictionary<string, IValue>();
            foreach (var val in Identifiers)
            {
                preBodyIdentifiers.Add(val.Key, val.Value);
            }
            var preStructDefFunctions = new Dictionary<string, Function>();
            foreach (var val in preStructDefFunctions)
            {
                preStructDefFunctions.Add(val.Key, val.Value);
            }

            structDefinition.StructParts.Accept(this);
            Identifiers = preBodyIdentifiers;
            Funcs = preStructDefFunctions;
        }

        public override void Visit(StructDecleration structDecleration)
        {
            if (!(structDecleration.AssignmentOperator.Symbol == Indexes.Indexes.SymbolIndex.Eq))
                Errors.Add("The " + structDecleration.AssignmentOperator.Symbol + " can not be used on a struct");
            if (!(Identifiers.ContainsKey(structDecleration.Identifier.Id)))
                Identifiers.Add(structDecleration.Identifier.Id, new StructValue(Structs[structDecleration.StructIdentifier.Id]));
            else if (!(Identifiers[structDecleration.Identifier.Id] is StructValue))
                Errors.Add(structDecleration.Identifier.Id + " is not of type " + structDecleration.StructIdentifier.Id);
            else if (!((Identifiers[structDecleration.Identifier.Id] as StructValue).Struct.Identifier.Id == Structs[structDecleration.StructIdentifier.Id].Identifier.Id))
                Errors.Add(structDecleration.Identifier.Id + " is not of type " + structDecleration.StructIdentifier.Id);
           // base.Visit(structDecleration);
        }

        public override void Visit(ExpressionNegate expressionNegate)
        {
            _expressionChecker.CheckExpression(expressionNegate);
        }

        public override void Visit(ExpressionValOpExpr expressionValOpExpr)
        {
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
        }

        public override void Visit(VarDecleration varDecleration)
        {
            if (!Identifiers.ContainsKey(varDecleration.Identifier.Id))
                varDecleration.IsFirstUse = true;
            if (!(varDecleration.AssignmentOperator.Symbol == Indexes.Indexes.SymbolIndex.Eq) && varDecleration.IsFirstUse)
                Errors.Add("The " + varDecleration.AssignmentOperator.Symbol + " can not be used on an uninitialised variable");
            varDecleration.Identifier.Accept(this);
            varDecleration.AssignmentOperator.Accept(this);
            var value = _expressionChecker.CheckExpression(varDecleration.Expression);
            if (value is UnInitialisedVariable)
            {
                Errors.Add("Identifier " + varDecleration.Identifier.Id + " was not assigned a value");
            }
            else if (Identifiers[varDecleration.Identifier.Id] is Identifier)
            {
                if (value is StructValue)
                    varDecleration.Type.ValueType = (value as StructValue).Struct.Identifier.Id;
                else
                    varDecleration.Type.ValueType = _expressionChecker.CheckValueType(value);
                Identifiers[varDecleration.Identifier.Id] = value;
            }
            else if (Identifiers[varDecleration.Identifier.Id].GetType().Name == value.GetType().Name)
            {
                if (Identifiers[varDecleration.Identifier.Id] is BoolValue &&
                   !(varDecleration.AssignmentOperator.Symbol == Indexes.Indexes.SymbolIndex.Eq))
                    Errors.Add(varDecleration.AssignmentOperator.Symbol + " can not be used with two bool values");
                else if (Identifiers[varDecleration.Identifier.Id] is StringValue && varDecleration.AssignmentOperator.Symbol == Indexes.Indexes.SymbolIndex.Minuseq)
                    Errors.Add(varDecleration.AssignmentOperator.Symbol + " can not be used with two string values");
                Identifiers[varDecleration.Identifier.Id] = value;
            }
            else
                Errors.Add("Identifier " + varDecleration.Identifier.Id + " is of type " +
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

        public override void Visit(FunctionDeclaration functionDeclaration)
        {
            if (!(Funcs.ContainsKey(functionDeclaration.TypeId.Identifier.Id)))
            {
                var preBodyIdentifiers = new Dictionary<string, IValue>();
                foreach (var val in Identifiers)
                {
                    preBodyIdentifiers.Add(val.Key, val.Value);
                }
                foreach (var parameter in functionDeclaration.Parameters.TypeIds)
                {
                    if (parameter.TypeId.ValueType.ToString() == "void")
                        Errors.Add("Parameter to function can not be of type void");
                    Identifiers.Add(parameter.TypeId.Identifier.Id, _expressionChecker.TypeChecker(parameter.TypeId.ValueType.ToString()));
                }
                bool returnFound;
                IValue Value = _expressionChecker.TypeChecker(functionDeclaration.TypeId.ValueType.ToString());
                if (functionDeclaration.TypeId.ValueType.ToString() == "void")
                    returnFound = true;
                else
                    returnFound = _expressionChecker.ReturnChecker(Value, functionDeclaration.Body.Bodyparts);
                if (returnFound == false)
                    Errors.Add("not all paths in " + functionDeclaration.TypeId.Identifier.Id + " return a value");
                Funcs.Add(functionDeclaration.TypeId.Identifier.Id, new Function(functionDeclaration, _expressionChecker.TypeChecker(functionDeclaration.TypeId.ValueType.ToString())));
                Identifiers = preBodyIdentifiers;
            }
            else
                Errors.Add(functionDeclaration.TypeId.Identifier.Id + " was declared twice");
        }

        public override void Visit(IfStatement ifStatement)
        {
            if (!(_expressionChecker.CheckExpression(ifStatement.Expression) is BoolValue))
                Errors.Add("if statements expects bool but got " + _expressionChecker.CheckExpression(ifStatement.Expression).GetType().Name);
            Visit(ifStatement.Body);
            Visit(ifStatement.ElseStatement);
        }

        public override void Visit(RepeatExpr repeatExpr)
        {
            if (!(_expressionChecker.CheckExpression(repeatExpr.Expression) is BoolValue))
                Errors.Add("repeats expects a bool but got " + _expressionChecker.CheckExpression(repeatExpr.Expression).GetType().Name);
            Visit(repeatExpr.Body);
        }

        public override void Visit(RepeatFor repeatFor)
        {
            Visit(repeatFor.VarDecleration);

            if (!(Identifiers[repeatFor.VarDecleration.Identifier.Id] is NumValue))
                Errors.Add(Identifiers[repeatFor.VarDecleration.Identifier.Id] + " was expected to be a num value, but was instead " + Identifiers[repeatFor.VarDecleration.Identifier.Id].GetType().Name);
            if (!(_expressionChecker.CheckExpression(repeatFor.Expression) is NumValue))
                Errors.Add("Expected NumValue in repeat expression but got " + _expressionChecker.CheckExpression(repeatFor.Expression).GetType().Name);

            Visit(repeatFor.Direction);
            Visit(repeatFor.Body);
        }

        public override void Visit(Refrence refrence) //Bliver kun besøgt hvis refrence er en bodypart aka når vi har et funktionskald uden for en vardecl.
        {
            _expressionChecker.NakedFuncCallChecker(refrence);
        }

        public override void Visit(FuncCall funcCall)
        {
            if (!(Funcs.ContainsKey(funcCall.Identifier.Id)))
            {
                string expressions = "";
                funcCall.Expressions.ForEach(x => expressions += x.ToString() + ",");
                if (expressions.Length > 0)
                    expressions = expressions.Remove(expressions.Length - 1, 1);
                Errors.Add(funcCall.Identifier.Id + "(" + expressions +  ") was not declared but was used in the code");
            }
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
            this.Struct = identifier;
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
        public ListType Type {get; set;}
        public IValue Value { get; set; }
        public void Accept(IEecVisitor visitor)
        {
            //bliver aldrig kaldt
        }
    }
}