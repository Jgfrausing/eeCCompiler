using System.Collections.Generic;

namespace eeCCompiler.Nodes
{
    public class FunctionDeclarationList : AbstractSyntaxTree
    {
        public FunctionDeclarationList()
        {
            FunctionDeclaration = new List<FunctionDeclaration>();
        }
        public FunctionDeclarationList(List<FunctionDeclaration> functionDeclaration)
        {
            FunctionDeclaration = functionDeclaration;
        }
        public List<FunctionDeclaration> FunctionDeclaration { get; set; }
    }

    public class FunctionDeclaration : AbstractSyntaxTree
    {
        public FunctionDeclaration(TypeId typeId, TypeIdList parameters, Body body)
        {
            TypeId = typeId;
            Parameters = parameters;
            Body = body;
        }
        public TypeId TypeId { get; set; }
        public TypeIdList Parameters { get; set; }
        public Body Body { get; set; }
    }
}