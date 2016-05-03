using eeCCompiler.Nodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eeCCompiler.Interfaces;

namespace eeCCompiler.Visitors
{
    class StringFinderVisitor : Visitor
    {
        public Dictionary<Identifier, StringValue> StringDict { get; set; }
        public int VariableName { get; set; }
        public StringFinderVisitor(int variableName)
        {
            StringDict = new Dictionary<Identifier, StringValue>();
            VariableName = variableName;
        }
        public override void Visit(ExpressionVal expressionVal)
        {
            expressionVal.Value = StringReplacer(expressionVal.Value);
        }
        public override void Visit(ExpressionValOpExpr expressionValOpExpr)
        {
            expressionValOpExpr.Value = StringReplacer(expressionValOpExpr.Value);
            expressionValOpExpr.Expression.Accept(this);
        }

        public override void Visit(IfStatement ifStatement)
        {
            ifStatement.Expression.Accept(this);
            ifStatement.ElseStatement.Accept(this);
        }

        public override void Visit(ElseStatement elseStatement)
        {

        }

        public override void Visit(VarDecleration varDecl)
        {
            varDecl.Expression.Accept(this);
        }

        public override void Visit(Return returnNode)
        {
            returnNode.Expression.Accept(this);
        }

        public override void Visit(FuncCall funcCall)
        {
            foreach (var parameter in funcCall.Expressions)
            {
                parameter.Accept(this);
            }
        }

        public override void Visit(RepeatFor repeatFor)
        {
            repeatFor.Expression.Accept(this);
        }

        public override void Visit(RepeatExpr repeatExpr)
        {
            repeatExpr.Expression.Accept(this);
        }

        public IValue StringReplacer(IValue value)
        {
            if (value is StringValue)
            {
                var stringValue = value as StringValue;
                value = new Identifier("_" + VariableName.ToString());
                StringDict.Add(value as Identifier, stringValue);
                VariableName++;
            }
            return value;
        }

        
    }
}
