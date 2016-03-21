using eeCCompiler.Interfaces;

namespace eeCCompiler.Nodes
{
    public class ElseStatement : AbstractSyntaxTree, IBodypart
    {
        public ElseStatement(Body body)
        {
            Body = body;
        }

        public Body Body { get; set; }

        public override void Accept(IEecVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}