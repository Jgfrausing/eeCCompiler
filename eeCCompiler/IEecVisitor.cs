using eeCCompiler.Nodes;

namespace eeCCompiler
{
    public interface IEecVisitor
    {
        void Visit(Root root);
        // all
    }
}