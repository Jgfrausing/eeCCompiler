using System.Collections.Generic;

namespace eeCCompiler.Nodes
{
    class FuncCall : AST
    {
        public FuncCall(Identifier identifier, List<Expression> expressions )
        {
            Identifier = identifier;
            Expressions = expressions;
        }
        public Identifier Identifier { get; set; }
        public List<Expression> Expressions { get; set; }

        public override string PrettyPrint()
        {
            throw new System.NotImplementedException();
        }
    }
}