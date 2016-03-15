using System.Collections.Generic;

namespace eeCCompiler.Nodes
{
    public class Root : AbstractSyntaxTree
    {
        public Root(List<Constant> constantDefinitions, List<StructDefinition> structDefinitions, Body body,
            FunctionDeclarationList functionDeclarationList)
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