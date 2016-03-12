using eeCCompiler.Interfaces;

namespace eeCCompiler.Nodes
{
    class VarDecleration : AbstractSyntaxTree, IBodypart
    {
        public VarDecleration(Identifier identifier, IExpression expression)
        {
            Identifier = identifier;
            IExpression = expression;
        }
        public Identifier Identifier { get; set; }
        public IExpression IExpression { get; set; }
    }
}