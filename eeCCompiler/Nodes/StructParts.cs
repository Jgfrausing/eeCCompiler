using System.Collections.Generic;
using eeCCompiler.Interfaces;
using eeCCompiler.Visitors;

namespace eeCCompiler.Nodes
{
    public class StructParts : AbstractSyntaxTree
    {
        public StructParts()
        {
            StructPartList = new List<IStructPart>();
        }

        public StructParts(List<IStructPart> structPartList)
        {
            StructPartList = structPartList;
        }

        public List<IStructPart> StructPartList { get; set; }

        public override void Accept(IEecVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}