using System.Collections.Generic;
using eeCCompiler.Interfaces;
using eeCCompiler.Visitors;

namespace eeCCompiler.Nodes
{
    public class FuncCall : AbstractSyntaxTree, IExpression, IBodypart, IStructRefrence, IValue
    {
        public FuncCall(List<IExprListElement> expressions, Identifier identifier)
        {
            Identifier = identifier;
            Expressions = expressions;
        }

        public Identifier Identifier { get; set; }
        public List<IExprListElement> Expressions { get; set; }

        public override void Accept(IEecVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override string ToString()
        {
            var s = "";
            foreach (var expression in Expressions)
            {
                s += expression.ToString();
            }
            return $"{Identifier}({s})";
        }
    }
}