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
            Identifiers = new List<IStructRefrence>();
        }

        public Refrence(IStructRefrence identifier, IStructRefrence refrence)
        {
            Identifiers = new List<IStructRefrence>() { identifier};
            StructRefrence = refrence;
        }
        public Refrence(IStructRefrence refrence, ListIndex index, Identifier identifier)
        {
            var v = new ListIndex(identifier);
            Identifiers.Insert(0,identifier);
            Identifiers.Insert(0, index);
            StructRefrence = refrence;
        }

        public List<IStructRefrence> Identifiers { get; set; }
        public IStructRefrence StructRefrence { get; set; }

        public override void Accept(IEecVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override string ToString()
        {
            var s = "";
            foreach (var id in Identifiers)
            {
                s += id.ToString();
                s += ".";
            }
            return s + StructRefrence;
        }
    }
}