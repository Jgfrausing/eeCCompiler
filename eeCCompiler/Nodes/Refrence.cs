using System.Collections.Generic;
using eeCCompiler.Interfaces;
using eeCCompiler.Visitors;

namespace eeCCompiler.Nodes
{
    public class Refrence : AbstractSyntaxTree, IValue, IBodypart, IStructRefrence
    {
        public Refrence(IStructRefrence structRefrence)
        {
            StructRefrence = structRefrence;
            Identifier = null;
        }

        public Refrence(IStructRefrence identifier, IStructRefrence refrence)
        {
            Identifier = identifier;
            StructRefrence = refrence;
        }

        public IStructRefrence Identifier { get; set; }
        public IStructRefrence StructRefrence { get; set; }

        public override void Accept(IEecVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override string ToString()
        {
            var s = Identifier.ToString() + ".";
            return s + StructRefrence;
        }
    }
}