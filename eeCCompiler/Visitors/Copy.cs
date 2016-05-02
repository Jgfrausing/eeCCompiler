using eeCCompiler.Nodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eeCCompiler.Visitors
{
    class Copy
    {
        public void MakeCopyFunc(StructDefinition structDef)
        {
            var code = "";
            code += structDef.Identifier + "_copy(" + structDef.Identifier + "* input){\n";
            code += structDef.Identifier + " * returnValue = malloc(sizeof(" + structDef.Identifier + ");\n";
            foreach (var item in structDef.StructParts.StructPartList)
            {
                if (item is VarDecleration)
                {
                    if ((item as VarDecleration).Identifier.Type.IsBasicType)
                        code += "returnValue->" + (item as VarDecleration).Identifier + " = input->" + (item as VarDecleration).Identifier + ";\n";
                    else if ((item as VarDecleration).Identifier.Type.ValueType.Substring((item as VarDecleration).Identifier.Type.ValueType.Length - 2) == "[]")
                        code += "returnvalue->" + (item as VarDecleration).Identifier + " = " + (item as VarDecleration).Identifier + "list_copy(input->" + (item as VarDecleration).Identifier + ");\n";
                    else
                        code += "returnvalue->" + (item as VarDecleration).Identifier + " = " + (item as VarDecleration).Identifier + "_copy(input->" + (item as VarDecleration).Identifier + ");\n";
                }
            }
        }
    }
}
