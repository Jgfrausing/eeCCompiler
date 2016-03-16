using System.Collections.Generic;
using eeCCompiler.Interfaces;

namespace eeCCompiler.Nodes
{
    public abstract class AbstractSyntaxTree : INodeElement
    {
        //public string Tag { get; set; }
        public abstract void Accept(IEecVisitor visitor);
    }

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
    }

    public interface IStructRefrence
    {
    }
}