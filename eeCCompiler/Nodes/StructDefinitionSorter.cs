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
            for (int i = n-1; i >= 0; i--)
            {
                for (int j = i; j >= 0; j--)
                {
                    if (structDefinitions[j].StructParts.StructPartList.Exists(x => x is StructDecleration && (x as StructDecleration).StructIdentifier.Equals(structDefinitions[i].Identifier)))
                    {
                        if (i == j)
                        {
                            throw new ArgumentException($"The definition of structs is not possible, since struct of type {structDefinitions[i].Identifier} has an instance of itself.");
                        }
                        else
                        {
                            throw new ArgumentException($"The creation of structs is not possible, since multiple structs creates an endless circular construction.");
                        }
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
                throw new ArgumentException($"Struct of type {first.Identifier} and struct of type {second.Identifier} constructs an endless circular construction.");

            return returnValue;
        }
    }
}