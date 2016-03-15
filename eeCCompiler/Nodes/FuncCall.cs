using System.Collections.Generic;
using eeCCompiler.Interfaces;

namespace eeCCompiler.Nodes
{
    internal class FuncCall : AbstractSyntaxTree, IValue
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
            Identifier.Accept(visitor);
            foreach (var expression in Expressions)
            {
                expression.Accept(visitor);
            }
            visitor.Visit(this);
        }
    }
}