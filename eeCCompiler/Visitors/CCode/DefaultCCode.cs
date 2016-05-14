using System.IO;

namespace eeCCompiler.Visitors.CCode
{
    internal class DefaultCCode
    {
        private readonly string _location = @"..\..\C_code\";

        public string GenerateListTypeHeader(string name, string type, bool isRefrenceType)
        {
            var sr = isRefrenceType
                ? new StreamReader(_location + "DefaultListHeaderWithPointers.c")
                : new StreamReader(_location + "DefaultListHeader.c");
            return sr.ReadToEnd().Replace("{type}", type).Replace("{name}", name);
        }

        public string GenerateListTypeCode(string name, string type, bool isRefrenceType)
        {
            var sr = isRefrenceType
                ? new StreamReader(_location + "DefaultListCodeWithPointers.c")
                : new StreamReader(_location + "DefaultListCode.c");
            return sr.ReadToEnd().Replace("{type}", type).Replace("{name}", name);
        }

        public string GetIncludes()
        {
            var sr = new StreamReader(_location + "Includes.c");
            return sr.ReadToEnd();
        }

        public string StandardFunctionsHeader()
        {
            var sr = new StreamReader(_location + "StandardFunctionsHeader.c");
            return sr.ReadToEnd();
        }

        public string StandardFunctionsCode()
        {
            var sr = new StreamReader(_location + "StandardFunctions.c");
            return sr.ReadToEnd();
        }

        public string IncludeMain(string main)
        {
            var sr = new StreamReader(main);
            return sr.ReadToEnd();
        }

        public void CreateListPrototypes(CCodeGeneration cCodeGeneration)
        {
            foreach (var structDefinition in cCodeGeneration._root.StructDefinitions.Definitions)
            {
                cCodeGeneration.Code += GenerateListTypeHeader(structDefinition.Identifier.Id + "list",
                    structDefinition.Identifier.Id, true);
            }
        }

        public void CreateCopyFunctions(CCodeGeneration cCodeGeneration)
        {
            var copy = new Copy();
            foreach (var structDefinition in cCodeGeneration._root.StructDefinitions.Definitions)
            {
                cCodeGeneration.Code += copy.MakeCopyFunc(structDefinition);
            }
        }
    }
}