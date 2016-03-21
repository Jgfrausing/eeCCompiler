﻿using eeCCompiler.Interfaces;

namespace eeCCompiler.Nodes
{
    public class Type : AbstractSyntaxTree
    {
        public Type(string type)
        {
            ValueType = type;
        }

        public string ValueType { get; set; }

        public override void Accept(IEecVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override string ToString()
        {
            return ValueType;
        }
    }
}