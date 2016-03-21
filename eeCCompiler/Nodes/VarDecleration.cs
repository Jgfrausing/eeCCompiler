using eeCCompiler.Interfaces;

namespace eeCCompiler.Nodes
{
    public class VarDecleration : AbstractSyntaxTree, IBodypart, IStructPart
    {
        public VarDecleration(Identifier identifier, AssignmentOperator assignmentOperator, IExpression expression)
        {
            Identifier = identifier;
            AssignmentOperator = assignmentOperator;
            Expression = expression;
        }

        public Identifier Identifier { get; set; }
        public AssignmentOperator AssignmentOperator { get; set; }
        public IExpression Expression { get; set; }

        public override void Accept(IEecVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}