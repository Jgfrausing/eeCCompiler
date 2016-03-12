using System.Collections.Generic;
using eeCCompiler.Interfaces;

namespace eeCCompiler.Nodes
{
    class ExpressionList : AbstractSyntaxTree
    {
        public ExpressionList()
        {
            Expressions = new List<IExpression>();
        }

        public ExpressionList(List<IExpression> expressions  )
        {
            Expressions = expressions;
        }
        public List<IExpression> Expressions { get; set; }
    }
}