using System.Collections.Generic;
using eeCCompiler.Interfaces;

namespace eeCCompiler.Nodes
{
    class FuncCall : AbstractSyntaxTree, IValue
    {
        public FuncCall(Identifier identifier, List<IExpression> expressions )
        {
            Identifier = identifier;
            IExpressions = expressions;
        }
        public Identifier Identifier { get; set; }
        public List<IExpression> IExpressions { get; set; }
    }
}