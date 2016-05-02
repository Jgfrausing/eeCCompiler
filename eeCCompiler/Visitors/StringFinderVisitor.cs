using eeCCompiler.Nodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eeCCompiler.Visitors
{
    class StringFinderVisitor : Visitor
    {
        public Queue<StringValue> StringQueue { get; set; }
        public int VariableName { get; set; }
        public StringFinderVisitor(int variableName)
        {
            StringQueue = new Queue<StringValue>();
            VariableName = variableName;
        }
        public override void Visit(ExpressionVal expressionVal)
        {
            base.Visit(expressionVal);
            if (expressionVal.Value is StringValue)
            { 
                expressionVal.Value = new Identifier("_" + VariableName.ToString());
                VariableName++;
            }
        }
        public override void Visit(ExpressionValOpExpr expressionValOpExpr)
        {
            base.Visit(expressionValOpExpr);
            if (expressionValOpExpr.Value is StringValue)
            {
                expressionValOpExpr.Value = new Identifier("_" + VariableName.ToString());
                VariableName++;
            }
        }
        public override void Visit(StringValue stringValue)
        {
            StringQueue.Enqueue(stringValue);
        }
    }
}
