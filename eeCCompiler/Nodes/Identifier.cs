namespace eeCCompiler.Nodes
{
    class Identifier : AST
    {
        public Identifier(string id)
        {
            Id = id;
        }
        public string Id { get; set; }
        public override string PrettyPrint()
        {
            throw new System.NotImplementedException();
        }
    }
}