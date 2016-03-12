using System.Collections.Generic;

namespace eeCCompiler.Nodes
{
    class Body : AST
    {
        public Body(List<Bodypart> bodyparts )
        {
            Bodyparts = bodyparts;
        }
        public List<Bodypart> Bodyparts { get; set; }
        public override string PrettyPrint()
        {
            throw new System.NotImplementedException();
        }
    }
}