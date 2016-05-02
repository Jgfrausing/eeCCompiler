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
            StringReplacer(expressionVal.Value);
        }
        public override void Visit(ExpressionValOpExpr expressionValOpExpr)
        {
            StringReplacer(expressionValOpExpr.Value);
            expressionValOpExpr.Expression.Accept(this);
        }

        public void StringReplacer(IValue value)
        {
            if (value is StringValue)
            {
                var stringValue = value as StringValue;
                value = new Identifier("_" + VariableName.ToString());
                StringDict.Add(value as Identifier, stringValue);
                VariableName++;
            }
        }
    }
}
