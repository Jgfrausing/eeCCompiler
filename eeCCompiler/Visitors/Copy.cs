using eeCCompiler.Nodes;

namespace eeCCompiler.Visitors
{
    public class Copy
    {
        public string MakeCopyFunc(StructDefinition structDef)
        {
            var code = "";
            code += structDef.Identifier + " " + structDef.Identifier + "_copy(" + structDef.Identifier + "* input){\n";
            code += structDef.Identifier + " * returnValue = malloc(sizeof(" + structDef.Identifier + "));\n";
            foreach (var item in structDef.StructParts.StructPartList)
            {
                if (item is VarDecleration)
                {
                    if ((item as VarDecleration).Identifier.Type.IsBasicType)
                        code += "returnValue->" + (item as VarDecleration).Identifier + " = input->" +
                                (item as VarDecleration).Identifier + ";\n";
                    else if ((item as VarDecleration).Identifier.Type.IsListValue)
                        code += "returnValue->" + (item as VarDecleration).Identifier + " = " +
                                (item as VarDecleration).Identifier.Type.ValueType + "list_copy(input->" +
                                (item as VarDecleration).Identifier + ");\n";
                    else
                        code += "returnValue->" + (item as VarDecleration).Identifier + " = " +
                                (item as VarDecleration).Identifier.Type.ValueType + "_copy(&input->" +
                                (item as VarDecleration).Identifier + ");\n";
                }
            }
            code += "return *returnValue;\n}\n";
            return code;
        }
    }
}