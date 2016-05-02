using eeCCompiler.Nodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eeCCompiler.Visitors
{
    class StringFinderVisitor : Visitor
    {
        public Queue<StringValue> StringQueue { get; set; }

        public StringFinderVisitor()
        {
            StringQueue = new Queue<StringValue>();
        }
        public override void Visit(StringValue stringValue)
        {
            StringQueue.Enqueue(stringValue);
        }
    }
}
