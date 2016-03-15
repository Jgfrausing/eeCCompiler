using System;
using eeCCompiler.Interfaces;

namespace eeCCompiler.Nodes
{
    public class Operator : AbstractSyntaxTree
    {
        public Operator(Indexes.Indexes.SymbolIndex symbol)
        {
            Symbol = symbol;
        }

        public Indexes.Indexes.SymbolIndex Symbol { get; set; }

        public override void Accept(IEecVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}