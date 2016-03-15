using System;

namespace eeCCompiler.Nodes
{
    public class Type : AbstractSyntaxTree
    {
        public Type(string type)
        {
            ValueType = type;
        }

        public string ValueType { get; set; }

        public override void Accept(IEecVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}