using eeCCompiler.Interfaces;

namespace eeCCompiler.Nodes
{
    public class VarDecleration : AbstractSyntaxTree, IBodypart, IStructPart
    {
        public VarDecleration(Identifier identifier, IExpression expression)
        {
            Identifier = identifier;
            Expression = expression;
        }
        public Identifier Identifier { get; set; }
        public IExpression Expression { get; set; }
    }
}