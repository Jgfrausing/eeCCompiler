using System.Collections;
using System.Collections.Generic;
using eeCCompiler.Interfaces;
using eeCCompiler.Visitors;

namespace eeCCompiler.Nodes
{
    public class BoolValue : AbstractSyntaxTree, IValue, IConstantPart, IExpression
    {
        public BoolValue(bool value)
        {
            Value = value;
        }

        public bool Value { get; set; }

        public override void Accept(IEecVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override string ToString()
        {
            return Value ? "1" : "0";   
        }
    }

    public class StringValue : AbstractSyntaxTree, IValue, IConstantPart, IExpression, IStringPart
    {
        public StringValue(string value)
        {
            Value = value;
        }

        public string Value { get; set; }
        public List<IStringPart> Elements { get; set; }

        public override void Accept(IEecVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override string ToString()
        {
            return Value;
        }
    }

    public class NumValue : AbstractSyntaxTree, IValue, IConstantPart, IExpression
    {
        public NumValue(double value)
        {
            Value = value;
        }

        public double Value { get; set; }

        public override void Accept(IEecVisitor visitor)
        {
            visitor.Visit(this);
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}