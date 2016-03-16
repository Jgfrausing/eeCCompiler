using eeCCompiler.Interfaces;

namespace eeCCompiler.Nodes
{
    public class AssignmentOperator : Operator
    {
        public AssignmentOperator(Indexes.Indexes.SymbolIndex symbol) : base(symbol)
        {
        }

        public string Operator { get; set; }
    }
}