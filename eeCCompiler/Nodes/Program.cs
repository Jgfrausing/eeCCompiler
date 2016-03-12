using System.Collections.Generic;

namespace eeCCompiler.Nodes
{
    class Program : AbstractSyntaxTree
    {
        public Program(List<Constant> constantList, Body body)
        {
            ConstantList = constantList;
            Body = body;
        }

        public List<Constant> ConstantList { get; set; }
        public Body Body { get; set; }
    }
}

