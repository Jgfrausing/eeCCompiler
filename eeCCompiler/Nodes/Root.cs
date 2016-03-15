using System;
using System.Collections.Generic;
using eeCCompiler.Interfaces;

namespace eeCCompiler.Nodes
{
    public class Root : AbstractSyntaxTree
    {
        public Root(ConstantDefinitions constantDefinitions, StructDefinitions structDefinitions, Body body,
            FunctionDeclarations functionDeclarations)
        {
            ConstantDefinitions = constantDefinitions;
            StructDefinitions = structDefinitions;
            Program = body;
            FunctionDeclarations = functionDeclarations;
        }

        public ConstantDefinitions ConstantDefinitions { get; set; }
        public StructDefinitions StructDefinitions { get; set; }
        public Body Program { get; set; }
        public FunctionDeclarations FunctionDeclarations { get; set; }

        public override void Accept(IEecVisitor visitor)
        {
            ConstantDefinitions.Accept(visitor);
            StructDefinitions.Accept(visitor);
            Program.Accept(visitor);
            FunctionDeclarations.Accept(visitor);
            visitor.Visit(this);
        }
    }
}