using System.Collections.Generic;
using eeCCompiler.Interfaces;

namespace eeCCompiler.Nodes
{
    class IExpressionList : AbstractSyntaxTree
    {
        public IExpressionList()
        {
            IExpressions = new List<IExpression>();
        }

        public IExpressionList(List<IExpression> expressions  )
        {
            IExpressions = expressions;
        }
        public List<IExpression> IExpressions { get; set; }
    }
}