using eeCCompiler.Interfaces;

namespace eeCCompiler.Nodes
{
    public class Root : AbstractSyntaxTree
    {
        public Root(Includes includes, ConstantDefinitions constantDefinitions, StructDefinitions structDefinitions,
            Body body,
            FunctionDeclarations functionDeclarations)
        {
            Includes = includes;
            ConstantDefinitions = constantDefinitions;
            StructDefinitions = structDefinitions;
            Program = body;
            FunctionDeclarations = functionDeclarations;
        }

        public Includes Includes { get; set; }
        public ConstantDefinitions ConstantDefinitions { get; set; }
        public StructDefinitions StructDefinitions { get; set; }
        public Body Program { get; set; }
        public FunctionDeclarations FunctionDeclarations { get; set; }

        public override void Accept(IEecVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}