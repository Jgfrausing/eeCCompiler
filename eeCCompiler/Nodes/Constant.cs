namespace eeCCompiler.Nodes
{
    internal class Constant : AbstractSyntaxTree
    {
        public Constant(Identifier identifier, IConstantPart constantPart)
        {
            Identifier = identifier;
            ConstantPart = constantPart;
        }

        public Identifier Identifier { get; set; }
        public IConstantPart ConstantPart { get; set; }
    }
}