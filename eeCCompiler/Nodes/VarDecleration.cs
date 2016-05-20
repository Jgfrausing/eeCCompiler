using eeCCompiler.Interfaces;
using eeCCompiler.Visitors;

namespace eeCCompiler.Nodes
{
    public class VarDecleration : AbstractSyntaxTree, IBodypart, IStructPart
    {
        public VarDecleration(IExpression expression, AssignmentOperator assignmentOperator, Identifier identifier)
        {
            Identifier = identifier;
            AssignmentOperator = assignmentOperator;
            Expression = expression;
        }

        public bool IsFirstUse { get; set; }
        public Identifier Identifier { get; set; }
        public AssignmentOperator AssignmentOperator { get; set; }
        public IExpression Expression { get; set; }

        public override void Accept(IEecVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override bool Equals(object obj) //Vurderer om de har samme identifier
        {
            return obj is VarDecleration
                ? (obj as VarDecleration).Identifier.ToString() == Identifier.ToString()
                : false;
        }
    }

    public class VarInStructDecleration : AbstractSyntaxTree, IBodypart, IStructPart
    {
        public VarInStructDecleration(IExpression expression, AssignmentOperator assignmentOperator, IStructRefrence refrence)
        {
            Refrence = refrence;
            AssignmentOperator = assignmentOperator;
            Expression = expression;
        }

        public IStructRefrence Refrence { get; set; }
        public AssignmentOperator AssignmentOperator { get; set; }
        public IExpression Expression { get; set; }

        public override void Accept(IEecVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}