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
        public List<StringValue> StringList { get; set; }

        public StringFinderVisitor()
        {
            StringList = new List<StringValue>();
        }
        public override void Visit(StringValue stringValue)
        {
            StringList.Add(stringValue);
        }
    }
}
