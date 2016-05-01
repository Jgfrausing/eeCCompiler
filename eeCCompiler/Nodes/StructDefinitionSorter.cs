using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace eeCCompiler.Nodes
{
    public class StructDefinitionSorter
    {
        public void Sort(List<StructDefinition> structDefinitions)
        {
            int n = structDefinitions.Count;

            for (int i = 0; i < n; i++)
            {
                for (int j = i+1; j < n; j++)
                {
                    if (Compare(structDefinitions[i], structDefinitions[j]) == 1)
                    {
                        Swap(structDefinitions, i, j);
                    }
                }
                    
            }
        }

        private void Swap(List<StructDefinition> structDefinitions, int i, int j)
        {
            var varI = structDefinitions[j];
            var varJ = structDefinitions[i];
            structDefinitions.RemoveAt(j);
            structDefinitions.RemoveAt(i);
            structDefinitions.Insert(i, varI);
            structDefinitions.Insert(j, varJ);
        }

        private int Compare(StructDefinition first, StructDefinition second)
        {
            var yHasX = false;
            var xHasY = false;

            if(second.StructParts.StructPartList.Exists(structDcl => structDcl is StructDecleration && (structDcl as StructDecleration).StructIdentifier.Equals(first.Identifier)))
                yHasX = true;
            if (first.StructParts.StructPartList.Exists(structDcl => structDcl is StructDecleration && (structDcl as StructDecleration).StructIdentifier.Equals(second.Identifier)))
                xHasY = true;

            #region preCleanup
            //foreach (var source in y.StructParts.StructPartList.Where(structdecl => structdecl is StructDecleration))
            //{
            //    if ((source as StructDecleration).StructIdentifier.Equals(x.Identifier))
            //    {
            //        yHasX = true;
            //        break;
            //    }
            //}
            //foreach (var source in x.StructParts.StructPartList.Where(structdecl => structdecl is StructDecleration))
            //{
            //    if ((source as StructDecleration).StructIdentifier.Equals(y.Identifier))
            //    {
            //        xHasY = true;
            //        break;
            //    }
            //}
            #endregion

            int returnValue;

            if (yHasX && !xHasY)
                returnValue = -1;
            else if (!yHasX && xHasY)
                returnValue = 1;
            else if (!yHasX && !xHasY)
                returnValue = 0;
            else
                throw new NotSupportedException();

            return returnValue;
        }
    }
}