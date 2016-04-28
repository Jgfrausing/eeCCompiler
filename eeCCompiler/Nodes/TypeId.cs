using System.Collections.Generic;
using eeCCompiler.Interfaces;
using eeCCompiler.Visitors;

namespace eeCCompiler.Nodes
{
    public class TypeId : AbstractSyntaxTree, ITypeId, IStringPart
    {
        public TypeId(Identifier identifier, IType valueType)
        {
            ValueType = valueType;
            Identifier = identifier;
        }

        public IType ValueType { get; set; }
        public Identifier Identifier { get; set; }

        public override void Accept(IEecVisitor visitor)
        {
            visitor.Visit(this);
        }
    }

    public class TypeIdList : AbstractSyntaxTree
    {
        public TypeIdList()
        {
            TypeIds = new List<RefTypeId>();
        }

        public TypeIdList(List<RefTypeId> typeIds)
        {
            TypeIds = typeIds;
        }

        public List<RefTypeId> TypeIds { get; set; }

        public override void Accept(IEecVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}