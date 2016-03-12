using System.Collections.Generic;

namespace eeCCompiler.Nodes
{
    class Program : AbstractSyntaxTree
    {
        public Program(List<Constant> constantList, List<StructDefinition> structDefinitions, Body body, FunctionDeclarationList functionDeclarationList)
        {
            ConstantList = constantList;
            StructDefinitions = structDefinitions;
            Body = body;
            FunctionDeclarationList = functionDeclarationList;
        }

        public List<Constant> ConstantList { get; set; }
        public List<StructDefinition> StructDefinitions { get; set; }
        public Body Body { get; set; }
        public FunctionDeclarationList FunctionDeclarationList { get; set; }
    }
}

