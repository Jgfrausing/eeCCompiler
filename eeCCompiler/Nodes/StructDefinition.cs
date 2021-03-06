using eeCCompiler.Visitors;

namespace eeCCompiler.Nodes
{
    public class StructDefinition : AbstractSyntaxTree
    {
        public StructDefinition(StructParts structParts, Identifier identifier)
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

        public override bool Equals(object obj)
        {
            return obj is StructDefinition && (obj as StructDefinition).Identifier.Id == Identifier.Id;
        }
    }
}