namespace eeCCompiler.Nodes
{
    class Program : AST
    {
        public Program(Body body)
        {
            Body = body;
        }
        public Body Body { get; set; }
        public override string PrettyPrint()
        {
            throw new System.NotImplementedException();
        }
    }
}

