﻿using eeCCompiler.Interfaces;

namespace eeCCompiler.Nodes
{
    public class Identifier : AbstractSyntaxTree, IValue
    {
        public Identifier(string id)
        {
            Id = id;
        }

        public string Id { get; set; }
    }
}