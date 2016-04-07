using eeCCompiler.Interfaces;

namespace eeCCompiler.Nodes
{
    public class Identifier : AbstractSyntaxTree, IValue, IStructRefrence, IType, IExpression, IIdentifier
    {
        public Identifier(string id)
        {
            Id = id;
        }

        public string Id { get; set; }

        public override void Accept(IEecVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override string ToString()
        {
            return Id;
        }
    }
}