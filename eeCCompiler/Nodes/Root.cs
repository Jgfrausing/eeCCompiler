using System;
using System.Collections.Generic;

namespace eeCCompiler.Nodes
{
    public class Root : AbstractSyntaxTree
    {
        public Root(List<Constant> constantDefinitions, List<StructDefinition> structDefinitions, Body body,
            FunctionDeclarations functionDeclarations)
        {
            ConstantDefinitions = constantDefinitions;
            StructDefinitions = structDefinitions;
            Program = body;
            FunctionDeclarations = functionDeclarations;
        }

        public List<Constant> ConstantDefinitions { get; set; }
        public List<StructDefinition> StructDefinitions { get; set; }
        public Body Program { get; set; }
        public FunctionDeclarations FunctionDeclarations { get; set; }

        public override void Accept(IEecVisitor visitor)
        {
            foreach (var cont in ConstantDefinitions)
            {
                cont.Accept(visitor);
            }
            foreach (var structdef in StructDefinitions)
            {
                structdef.Accept(visitor);
            }
            Program.Accept(visitor);
            FunctionDeclarations.Accept(visitor);

            visitor.Visit(this);
        }
    }
}