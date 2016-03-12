namespace eeCCompiler.Nodes
{
    class VarDecleration : Bodypart
    {
        public VarDecleration(Identifier identifier, Expression expression)
        {
            Identifier = identifier;
            Expression = expression;
        }
        public Identifier Identifier { get; set; }
        public Expression Expression { get; set; }
        public override string PrettyPrint()
        {
            throw new System.NotImplementedException();
        }
    }
}