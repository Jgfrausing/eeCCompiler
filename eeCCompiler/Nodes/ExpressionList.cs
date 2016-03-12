using System.Collections.Generic;

namespace eeCCompiler.Nodes
{
    class ExpressionList : AST
    {
        public List<Expression> Expressions { get; set; }

        public override string PrettyPrint()
        {
            throw new System.NotImplementedException();
        }
    }
}