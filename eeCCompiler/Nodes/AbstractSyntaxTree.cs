using System.Collections.Generic;
using eeCCompiler.Interfaces;

namespace eeCCompiler.Nodes
{
    public abstract class AbstractSyntaxTree
    {
        //public string Tag { get; set; }
    }

    class Constants : AbstractSyntaxTree
    {
        public Constants()
        {
            ConstantList = new List<Constant>();
        }

        public Constants(List<Constant> constants )
        {
            ConstantList = constants;
        }
        public List<Constant> ConstantList { get; set; }
    }

    class Constant : AbstractSyntaxTree
    {
        public Constant(Identifier identifier, IConstantPart constantPart)
        {
            Identifier = identifier;
            ConstantPart = constantPart;
        }
        public Identifier Identifier { get; set; }
        public IConstantPart ConstantPart { get; set; }
    }
}