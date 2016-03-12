using System.Collections.Generic;

namespace eeCCompiler.Nodes
{
    class SourceCode : AbstractSyntaxTree
    {
        public SourceCode(List<Constant> constantDefinitions, List<StructDefinition> structDefinitions, Body body, FunctionDeclarationList functionDeclarationList)
        {
            ConstantDefinitions = constantDefinitions;
            StructDefinitions = structDefinitions;
            Program = body;
            FunctionDeclarations = functionDeclarationList.FunctionDeclaration;
        }

        public List<Constant> ConstantDefinitions { get; set; }
        public List<StructDefinition> StructDefinitions { get; set; }
        public Body Program { get; set; }
        public List<FunctionDeclaration> FunctionDeclarations { get; set; }
    }
}

