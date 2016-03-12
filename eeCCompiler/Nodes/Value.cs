using System;

namespace eeCCompiler.Nodes
{
    public abstract class Value : AST
    {
    }

    class BooleeC : Value
    {
        public BooleeC(Boolean value)
        {
            Value = value;
        }
        public Boolean Value { get; set; }
        public override string PrettyPrint()
        {
            throw new System.NotImplementedException();
        }
    }

    class StringeeC : Value
    {
        public StringeeC(String value)
        {
            Value = value;
        }
        public String Value { get; set; }
        public override string PrettyPrint()
        {
            throw new System.NotImplementedException();
        }
    }

    class NumeeC : Value
    {
        public NumeeC(Double value)
        {
            Value = value;
        }
        public Double Value { get; set; }
        public override string PrettyPrint()
        {
            throw new System.NotImplementedException();
        }
    }
}