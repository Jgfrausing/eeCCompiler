using System.Collections.Generic;
using eeCCompiler.Interfaces;

namespace eeCCompiler.Nodes
{
    public class FuncCall : AbstractSyntaxTree, IExpression, IBodypart, IStructRefrence
    {
        public FuncCall(List<IExpression> expressions, Identifier identifier)
        {
            Identifier = identifier;
            Expressions = expressions;
        }

        public Identifier Identifier { get; set; }
        public List<IExpression> Expressions { get; set; }

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