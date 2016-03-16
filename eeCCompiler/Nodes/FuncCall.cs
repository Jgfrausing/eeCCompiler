using System.Collections.Generic;
using eeCCompiler.Interfaces;

namespace eeCCompiler.Nodes
{
    public class FuncCall : AbstractSyntaxTree, IExpression, IBodypart, IStructRefrence
    {
        public FuncCall(Identifier identifier, List<IExpression> expressions)
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
            string s = "";
            foreach (var expression in Expressions)
            {
                s += expression.ToString();
            }
            return $"{Identifier}({s})";
        }
    }
}