namespace eeCCompiler.Nodes
{
    public abstract class AST
    {
        public string Tag { get; set; }
        public abstract string PrettyPrint();
    }
}