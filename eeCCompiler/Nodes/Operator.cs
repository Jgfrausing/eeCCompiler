using eeCCompiler.Visitors;

namespace eeCCompiler.Nodes
{
    public class Operator : AbstractSyntaxTree
    {
        public Operator(Indexes.Indexes.SymbolIndex symbol)
        {
            Symbol = symbol;
        }

        public bool IsStringOpr { get; set; }

        public Indexes.Indexes.SymbolIndex Symbol { get; set; }

        public override void Accept(IEecVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override string ToString()
        {
            return Symbol.ToString();
        }
    }
}