using System.Collections.Generic;

namespace eeCCompiler.Nodes
{
    class Program : AbstractSyntaxTree
    {
        public Program(List<Constant> constantList, Body body, FunctionDeclarationList functionDeclarationList)
        {
            ConstantList = constantList;
            Body = body;
            FunctionDeclarationList = functionDeclarationList;
        }

        public List<Constant> ConstantList { get; set; }
        public Body Body { get; set; }
        public FunctionDeclarationList FunctionDeclarationList { get; set; }
    }
}

