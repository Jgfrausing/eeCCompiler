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
    }
}