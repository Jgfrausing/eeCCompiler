using eeCCompiler.Interfaces;

namespace eeCCompiler.Nodes
{
    public class StructDefinition : AbstractSyntaxTree
    {
        public StructDefinition(Identifier identifier, StructParts structParts)
        {
            Identifier = identifier;
            StructParts = structParts;
        }

        public Identifier Identifier { get; set; }
        public StructParts StructParts { get; set; }

        public override void Accept(IEecVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}