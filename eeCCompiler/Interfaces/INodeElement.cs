namespace eeCCompiler.Interfaces
{
    public interface INodeElement
    {
        void Accept(IEecVisitor visitor);
    }
}