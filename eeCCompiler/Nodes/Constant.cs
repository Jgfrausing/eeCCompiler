using eeCCompiler.Interfaces;

namespace eeCCompiler.Nodes
{
    public class Constant : AbstractSyntaxTree
    {
        public Constant(Identifier identifier, IConstantPart constantPart)
        {
            Identifier = identifier;
            ConstantPart = constantPart;
        }

        public Identifier Identifier { get; set; }
        public IConstantPart ConstantPart { get; set; }

        public override void Accept(IEecVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}