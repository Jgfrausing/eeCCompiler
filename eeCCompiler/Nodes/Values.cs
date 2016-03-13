using eeCCompiler.Interfaces;

namespace eeCCompiler.Nodes
{
    internal class BoolValue : AbstractSyntaxTree, IValue, IConstantPart
    {
        public BoolValue(bool value)
        {
            Value = value;
        }

        public bool Value { get; set; }
    }

    internal class StringValue : AbstractSyntaxTree, IValue, IConstantPart
    {
        public StringValue(string value)
        {
            Value = value;
        }

        public string Value { get; set; }
    }

    internal class NumValue : AbstractSyntaxTree, IValue, IConstantPart
    {
        public NumValue(double value)
        {
            Value = value;
        }

        public double Value { get; set; }
    }
}