using System;
using eeCCompiler.Interfaces;

namespace eeCCompiler.Nodes
{
    class BoolValue : AbstractSyntaxTree, IValue, IConstantPart
    {
        public BoolValue(Boolean value)
        {
            Value = value;
        }
        public Boolean Value { get; set; }
    }

    class StringValue : AbstractSyntaxTree, IValue, IConstantPart
    {
        public StringValue(String value)
        {
            Value = value;
        }
        public String Value { get; set; }
    }

    class NumValue : AbstractSyntaxTree, IValue, IConstantPart
    {
        public NumValue(Double value)
        {
            Value = value;
        }
        public Double Value { get; set; }
    }
}