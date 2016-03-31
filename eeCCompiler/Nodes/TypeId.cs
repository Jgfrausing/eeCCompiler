using System.Collections.Generic;
using eeCCompiler.Interfaces;

namespace eeCCompiler.Nodes
{
    public class TypeId : AbstractSyntaxTree
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
            ValueType.Accept(visitor);
            Identifier.Accept(visitor);
        }
    }

    public class TypeIdList : AbstractSyntaxTree
    {
        public TypeIdList()
        {
            TypeIds = new List<TypeId>();
        }

        public TypeIdList(List<TypeId> typeIds)
        {
            TypeIds = typeIds;
        }

        public List<TypeId> TypeIds { get; set; }

        public override void Accept(IEecVisitor visitor)
        {
            foreach (var typeids in TypeIds)
            {
                typeids.Accept(visitor);
            }
        }
    }
}