using System.Collections.Generic;

namespace eeCCompiler.Nodes
{
    internal class Constants : AbstractSyntaxTree
    {
        public Constants()
        {
            ConstantList = new List<Constant>();
        }

        public Constants(List<Constant> constants)
        {
            ConstantList = constants;
        }

        public List<Constant> ConstantList { get; set; }
    }
}