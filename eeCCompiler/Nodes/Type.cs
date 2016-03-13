namespace eeCCompiler.Nodes
{
    public class Type : AbstractSyntaxTree
    {
        public Type(string type)
        {
            ValueType = type;
        }

        public string ValueType { get; set; }
    }
}