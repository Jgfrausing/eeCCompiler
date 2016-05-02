namespace eeCCompiler.Interfaces
{
    public interface IExpression : IExprListElement
    {
        eeCCompiler.Nodes.Type Type { get; set; }
    }
}