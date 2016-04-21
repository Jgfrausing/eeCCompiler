using System.Collections.Generic;
using eeCCompiler.Interfaces;
using eeCCompiler.Visitors;

namespace eeCCompiler.Nodes
{
    public class FunctionDeclarations : AbstractSyntaxTree
    {
        public FunctionDeclarations()
        {
            FunctionDeclarationList = new List<FunctionDeclaration>();
        }

        public FunctionDeclarations(List<FunctionDeclaration> functionDeclaration)
        {
            FunctionDeclarationList = functionDeclaration;
        }

        public List<FunctionDeclaration> FunctionDeclarationList { get; set; }

        public override void Accept(IEecVisitor visitor)
        {
            visitor.Visit(this);
        }
    }

    public class FunctionDeclaration : AbstractSyntaxTree, IStructPart
    {
        public FunctionDeclaration(Body body, TypeIdList parameters, TypeId typeId)
        {
            TypeId = typeId;
            Parameters = parameters;
            Body = body;
        }

        public TypeId TypeId { get; set; }
        public TypeIdList Parameters { get; set; }
        public Body Body { get; set; }

        public override void Accept(IEecVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}