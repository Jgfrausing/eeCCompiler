using System;
using eeCCompiler.Interfaces;

namespace eeCCompiler.Nodes
{
    public class StructDecleration : AbstractSyntaxTree, IBodypart
    {
        public StructDecleration(Identifier identifier, AssignmentOperator assignmentOperator, Identifier structIdentifier, VarDeclerations varDeclerations)
        {
            Identifier = identifier;
            AssignmentOperator = assignmentOperator;
            StructIdentifier = structIdentifier;
            VarDeclerations = varDeclerations;
        }

        public Identifier Identifier { get; set; }
        public AssignmentOperator AssignmentOperator { get; set; }
        public Identifier StructIdentifier { get; set; }
        public VarDeclerations VarDeclerations { get; set; }

        public override void Accept(IEecVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}