namespace eeCCompiler.Nodes
{
    class Operator : AST
    {
        public Operator(Indexes.Indexes.SymbolIndex symbol)
        {
            Symbol = symbol;
        }
        public Indexes.Indexes.SymbolIndex Symbol { get; set; }
        public override string PrettyPrint()
        {
            throw new System.NotImplementedException();
        }
    }
}