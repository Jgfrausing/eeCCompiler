using eeCCompiler.Interfaces;

namespace eeCCompiler.Nodes
{
    public class IfStatement : ElseStatement, IBodypart
    {
        public IfStatement(ElseStatement elseStatement, Body body, IExpression expression) : base(body)
        {
            Expression = expression;
            ElseStatement = elseStatement;
        }

        public IExpression Expression { get; set; }
        public ElseStatement ElseStatement { get; set; }

        public override void Accept(IEecVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}