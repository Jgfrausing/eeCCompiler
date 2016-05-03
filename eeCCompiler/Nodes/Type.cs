using eeCCompiler.Interfaces;
using eeCCompiler.Visitors;

namespace eeCCompiler.Nodes
{
    public class Type : AbstractSyntaxTree, IType
    {
        public Type(string type)
        {
            ValueType = type;
        }

        public bool IsBasicType => ValueType == "num" || ValueType == "bool";
        public bool IsListValue { get; set; }
        public string ValueType { get; set; }
        

        public override void Accept(IEecVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override string ToString()
        {
            return ValueType;
        }
    }
}