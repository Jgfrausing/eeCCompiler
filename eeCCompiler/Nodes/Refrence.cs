using System.Collections.Generic;
using eeCCompiler.Interfaces;

namespace eeCCompiler.Nodes
{
    public class Refrence : AbstractSyntaxTree, IValue
    {
        public Refrence(IStructRefrence structRefrence)
        {
            StructRefrence = structRefrence;
            Identifiers = new List<IIdentifier>();
        }

        public Refrence(Refrence refrence, IIdentifier identifier)
        {
            Identifiers = refrence.Identifiers;
            Identifiers.Insert(0, identifier);
            StructRefrence = refrence.StructRefrence;
        }
        public Refrence(Refrence refrence, ListIndex index, Identifier identifier)
        {
            Identifiers.Insert(0,identifier);
            Identifiers.Insert(0, index);
            StructRefrence = refrence.StructRefrence;
        }

        public List<IIdentifier> Identifiers { get; set; }
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