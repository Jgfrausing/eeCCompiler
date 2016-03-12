using System.Collections.Generic;
using eeCCompiler.Interfaces;

namespace eeCCompiler.Nodes
{
    class ExpressionList : AbstractSyntaxTree
    {
        public ExpressionList()
        {
            IExpressions = new List<IExpression>();
        }

        public ExpressionList(List<IExpression> expressions  )
        {
            IExpressions = expressions;
        }
        public List<IExpression> IExpressions { get; set; }
    }
}