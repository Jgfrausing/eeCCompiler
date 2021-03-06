﻿using eeCCompiler.Interfaces;
using eeCCompiler.Visitors;

namespace eeCCompiler.Nodes
{
    public class Identifier : AbstractSyntaxTree, IValue, IStructRefrence, IType, IExpression, IIdentifier
    {
        public Identifier(string id)
        {
            Id = id;
            Type = new Type("not set in typechecker");
        }

        public Type Type { get; set; }

        public string Id { get; set; }

        public override void Accept(IEecVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override string ToString()
        {
            return Id;
        }

        public override bool Equals(object obj)
        {
            return obj is Identifier && (obj as Identifier).Id == Id;
        }
    }
}