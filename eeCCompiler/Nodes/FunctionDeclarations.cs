﻿using System;
using System.Collections.Generic;
using eeCCompiler.Interfaces;

namespace eeCCompiler.Nodes
{
    public class FunctionDeclarations : AbstractSyntaxTree, IStructPart
    {
        public FunctionDeclarations()
        {
            FunctionDeclaration = new List<FunctionDeclaration>();
        }

        public FunctionDeclarations(List<FunctionDeclaration> functionDeclaration)
        {
            FunctionDeclaration = functionDeclaration;
        }

        public List<FunctionDeclaration> FunctionDeclaration { get; set; }

        public override void Accept(IEecVisitor visitor)
        {
            foreach (var funcdecl in FunctionDeclaration)
            {
                funcdecl.Accept(visitor);
            }
            visitor.Visit(this);
        }
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

        public override void Accept(IEecVisitor visitor)
        {
            TypeId.Accept(visitor);
            Parameters.Accept(visitor);
            Body.Accept(visitor);
            visitor.Visit(this);
        }
    }
}