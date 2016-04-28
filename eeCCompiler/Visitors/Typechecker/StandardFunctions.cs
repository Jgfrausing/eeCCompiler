using eeCCompiler.Interfaces;
using eeCCompiler.Nodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eeCCompiler.Visitors
{
    class StandardFunctions
    {
        public static Dictionary<string, Function> FunctionDict()
        {
            var FuncDict = new Dictionary<string, Function>();
            var type = new eeCCompiler.Nodes.Type("void");

            List<RefTypeId> parameters = new List<RefTypeId>()
            { new RefTypeId(new TypeId(new Identifier("input"), new eeCCompiler.Nodes.Type("string")),new Ref(false))};

            FuncDict.Add("program_print",CreateFunction("program_print",parameters,new TypeId(new Identifier("program_print"), type),
                type, new UnInitialisedVariable()).Value);

            parameters.Clear();
            type.ValueType = "string";

            FuncDict.Add("program_read", CreateFunction("program_read", parameters, new TypeId(new Identifier("program_read"), type),
                type, new StringValue("")).Value);

            parameters = new List<RefTypeId>()
            {   new RefTypeId(new TypeId(new Identifier("input1"), new eeCCompiler.Nodes.Type("num")),new Ref(false)),
                new RefTypeId(new TypeId(new Identifier("input2"), new eeCCompiler.Nodes.Type("string")),new Ref(true)) };
            type.ValueType = "bool";

            FuncDict.Add("program_convertNumToString", CreateFunction("program_convertNumToString", parameters, new TypeId(new Identifier("program_convertNumToString"), type),
                type, new BoolValue(true)).Value);

            (parameters[0].TypeId.ValueType as eeCCompiler.Nodes.Type).ValueType = "bool";

            FuncDict.Add("program_convertBoolToString", CreateFunction("program_convertBoolToString", parameters, new TypeId(new Identifier("program_convertBoolToString"), type),
                type, new BoolValue(true)).Value);

            (parameters[0].TypeId.ValueType as eeCCompiler.Nodes.Type).ValueType = "string";
            (parameters[1].TypeId.ValueType as eeCCompiler.Nodes.Type).ValueType = "num";

            FuncDict.Add("program_convertStringToNum", CreateFunction("program_convertStringToNum", parameters, new TypeId(new Identifier("program_convertStringToNum"), type),
                type, new BoolValue(true)).Value);

            (parameters[1].TypeId.ValueType as eeCCompiler.Nodes.Type).ValueType = "bool";

            FuncDict.Add("program_convertStringToBool", CreateFunction("program_convertStringToBool", parameters, new TypeId(new Identifier("program_convertStringToBool"), type),
                type, new BoolValue(true)).Value);

            return FuncDict;
        }
        private static KeyValuePair<string, Function> CreateFunction(string functionName, List<RefTypeId> parameters, TypeId typeId,
            eeCCompiler.Nodes.Type type, IValue value )
        {
            FunctionDeclaration funcDecl = new FunctionDeclaration(new Body(), new TypeIdList(parameters),
               new TypeId(new Identifier(functionName), type));

            Function func = new Function(funcDecl,value);
            
            return new KeyValuePair<string, Function> (func.FuncDecl.TypeId.Identifier.Id, func);
        }
    }
}

