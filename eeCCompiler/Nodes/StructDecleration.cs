using eeCCompiler.Interfaces;

namespace eeCCompiler.Nodes
{
    public class StructDecleration : AbstractSyntaxTree, IBodypart
    {
        public StructDecleration(Identifier identifier, Identifier structIdentifier, VarDeclerations varDeclerations)
        {
            Identifier = identifier;
            StructIdentifier = structIdentifier;
            VarDeclerations = varDeclerations;
        }
        public Identifier Identifier { get; set; }
        public Identifier StructIdentifier { get; set; }
        public VarDeclerations VarDeclerations { get; set; }
    }
}