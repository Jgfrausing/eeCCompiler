using System.Collections.Generic;
using eeCCompiler.Interfaces;

namespace eeCCompiler.Nodes
{
    public class VarDeclerations : AbstractSyntaxTree, IBodypart
    {
        public VarDeclerations()
        {
            VarDeclerationList = new List<VarDecleration>();
        }

        public VarDeclerations(List<VarDecleration> varDeclerationList)
        {
            VarDeclerationList = varDeclerationList;
        }

        public List<VarDecleration> VarDeclerationList { get; set; }

        public override void Accept(IEecVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}