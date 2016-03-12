namespace eeCCompiler.Nodes
{
    class Operator : AbstractSyntaxTree
    {
        public Operator(Indexes.Indexes.SymbolIndex symbol)
        {
            Symbol = symbol;
        }
        public Indexes.Indexes.SymbolIndex Symbol { get; set; }
    }
}