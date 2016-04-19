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
        public ListIndex(IExpression index)
        {
            Indexes = new List<IExpression>() {index};
        }

        public ListIndex(ListIndex indexes, IExpression value )
        {
            indexes.Indexes.Insert(0,value);
            Indexes = indexes.Indexes;
        }

        public List<IExpression> Indexes { get; set; }
        public override void Accept(IEecVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}
