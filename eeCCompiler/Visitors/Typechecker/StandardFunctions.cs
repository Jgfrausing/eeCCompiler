using System.Collections.Generic;
using eeCCompiler.Interfaces;
using eeCCompiler.Nodes;

namespace eeCCompiler.Visitors
{
    internal class StandardFunctions
    {
        public static Dictionary<string, Function> FunctionDict()
        {
            var FuncDict = new Dictionary<string, Function>();
            var type = new Type("void");

            var parameters = new List<RefTypeId>
            {
                new RefTypeId(new TypeId(new Identifier("input"), new Type("string")), new Ref(false))
            };

            FuncDict.Add("program_print",
                CreateFunction("program_print", parameters, new TypeId(new Identifier("program_print"), type),
                    type, new UnInitialisedVariable()).Value);

            parameters = new List<RefTypeId>();
            type.ValueType = "string";

            FuncDict.Add("program_read",
                CreateFunction("program_read", parameters, new TypeId(new Identifier("program_read"), type),
                    type, new StringValue("")).Value);

            parameters = new List<RefTypeId>
            {
                new RefTypeId(new TypeId(new Identifier("input1"), new Type("num")), new Ref(false)),
                new RefTypeId(new TypeId(new Identifier("input2"), new Type("string")), new Ref(true))
            };
            type.ValueType = "bool";

            FuncDict.Add("program_convertNumToString",
                CreateFunction("program_convertNumToString", parameters,
                    new TypeId(new Identifier("program_convertNumToString"), type),
                    type, new BoolValue(true)).Value);

            parameters = new List<RefTypeId>
            {
                new RefTypeId(new TypeId(new Identifier("input1"), new Type("bool")), new Ref(false)),
                new RefTypeId(new TypeId(new Identifier("input2"), new Type("string")), new Ref(true))
            };


            FuncDict.Add("program_convertBoolToString",
                CreateFunction("program_convertBoolToString", parameters,
                    new TypeId(new Identifier("program_convertBoolToString"), type),
                    type, new BoolValue(true)).Value);

            parameters = new List<RefTypeId>
            {
                new RefTypeId(new TypeId(new Identifier("input1"), new Type("string")), new Ref(false)),
                new RefTypeId(new TypeId(new Identifier("input2"), new Type("num")), new Ref(true))
            };

            FuncDict.Add("program_convertStringToNum",
                CreateFunction("program_convertStringToNum", parameters,
                    new TypeId(new Identifier("program_convertStringToNum"), type),
                    type, new BoolValue(true)).Value);

            parameters = new List<RefTypeId>
            {
                new RefTypeId(new TypeId(new Identifier("input1"), new Type("string")), new Ref(false)),
                new RefTypeId(new TypeId(new Identifier("input2"), new Type("bool")), new Ref(true))
            };

            FuncDict.Add("program_convertStringToBool",
                CreateFunction("program_convertStringToBool", parameters,
                    new TypeId(new Identifier("program_convertStringToBool"), type),
                    type, new BoolValue(true)).Value);

            return FuncDict;
        }

        private static KeyValuePair<string, Function> CreateFunction(string functionName, List<RefTypeId> parameters,
            TypeId typeId,
            Type type, IValue value)
        {
            var funcDecl = new FunctionDeclaration(new Body(), new TypeIdList(parameters),
                new TypeId(new Identifier(functionName), type));

            var func = new Function(funcDecl, value);

            return new KeyValuePair<string, Function>(func.FuncDecl.TypeId.Identifier.Id, func);
        }
    }
}