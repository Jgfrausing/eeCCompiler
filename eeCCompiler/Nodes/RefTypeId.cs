using eeCCompiler.Interfaces;
using eeCCompiler.Visitors;

namespace eeCCompiler.Nodes
{
    public class RefTypeId : AbstractSyntaxTree, ITypeId
    {
        public RefTypeId(TypeId typeId, Ref _ref)
        {
            TypeId = typeId;
            Ref = _ref.IsRefrence;
        }

        public TypeId TypeId { get; set; }
        public bool Ref { get; set; }

        public override void Accept(IEecVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}