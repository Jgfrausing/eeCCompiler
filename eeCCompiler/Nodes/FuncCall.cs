using System.Collections.Generic;
using eeCCompiler.Interfaces;

namespace eeCCompiler.Nodes
{
    public class FuncCall : AbstractSyntaxTree, IExpression, IBodypart
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