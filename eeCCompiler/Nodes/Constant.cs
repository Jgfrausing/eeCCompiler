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
            Identifier.Accept(visitor);
            ConstantPart.Accept(visitor);
            visitor.Visit(this);
        }
    }
}