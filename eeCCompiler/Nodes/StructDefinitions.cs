using System.Collections.Generic;
using eeCCompiler.Interfaces;

namespace eeCCompiler.Nodes
{
    public class StructDefinitions : AbstractSyntaxTree
    {
        public StructDefinitions()
        {
            Definitions = new List<StructDefinition>();
        }

        public StructDefinitions(List<StructDefinition> structDefinitions)
        {
            Definitions = structDefinitions;
        }

        public List<StructDefinition> Definitions { get; set; }
    }

    public class StructDefinition : AbstractSyntaxTree
    {
        public StructDefinition(Identifier identifier, StructParts structParts)
        {
            Identifier = identifier;
            StructParts = structParts;
        }

        public Identifier Identifier { get; set; }
        public StructParts StructParts { get; set; }
    }

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
    }
}