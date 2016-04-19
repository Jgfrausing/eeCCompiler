using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eeCCompiler.Interfaces;
using eeCCompiler.Visitors;

namespace eeCCompiler.Nodes
{
    public class ListIndex : AbstractSyntaxTree, IStructRefrence, IIdentifier
    {
        public ListIndex(IValue index)
        {
            Indexes = new List<IValue>() {index};
        }

        public ListIndex(ListIndex indexes, IValue value )
        {
            indexes.Indexes.Insert(0,value);
            Indexes = indexes.Indexes;
        }

        public List<IValue> Indexes { get; set; }
        public override void Accept(IEecVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
