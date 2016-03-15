using System;
using System.Collections.Generic;

namespace eeCCompiler.Nodes
{
    public class TypeId : AbstractSyntaxTree
    {
        public TypeId(Type valueType, Identifier identifier)
        {
            ValueType = valueType;
            Identifier = identifier;
        }

        public Type ValueType { get; set; }
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
            foreach (var Typeids in TypeIds)
            {
                Typeids.Accept(visitor);
            }
        }
    }
}