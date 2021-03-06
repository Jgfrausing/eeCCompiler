﻿using eeCCompiler.Visitors;

namespace eeCCompiler.Nodes
{
    public class Direction : AbstractSyntaxTree
    {
        public Direction(bool incrementing)
        {
            Incrementing = incrementing;
        }

        public bool Incrementing { get; set; }

        public override void Accept(IEecVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override string ToString()
        {
            return Incrementing ? "to" : "downto";
        }
    }
}