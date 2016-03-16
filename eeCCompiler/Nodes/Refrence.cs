using System.Collections.Generic;
using eeCCompiler.Interfaces;

namespace eeCCompiler.Nodes
{
    public class Refrence : AbstractSyntaxTree, IValue
    {
        public Refrence(IStructRefrence structRefrence)
        {
            StructRefrence = structRefrence;
            Identifiers = new List<Identifier>();
        }
        public Refrence(Identifier identifier, Refrence refrence )
        {
            Identifiers = refrence.Identifiers;
            Identifiers.Insert(0, identifier);
            StructRefrence = refrence.StructRefrence;
        }
        public List<Identifier> Identifiers { get; set; }
        public IStructRefrence StructRefrence { get; set; }
        public override void Accept(IEecVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override string ToString()
        {
            string s = "";
            foreach (var id in Identifiers)
            {
                s += id.ToString();
                s += ".";
            }
            return s + StructRefrence;
        }
    }
}