using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eeCCompiler.Nodes;

namespace eeCCompiler.Visitors
{
    class MipsPrinter : Visitor
    {
        public MipsPrinter()
        {
            LabelCount = 0;
            Code = "";
        }
        private int LabelCount { get; set; }
        public string Code { get; set; }
        private bool[] Regs = new bool[8];
        

        public override void Visit(RepeatFor repeatFor)
        {

            int Label = LabelCount++; //Sikre sig at vi får nye labels hvis vi nester repeats
            int reg1 = GetReg(), reg2 = GetReg();
            Code += $"li $t{reg1}, {(repeatFor.VarDecleration.Expression as ExpressionVal).Value.ToString()}";  //Temp løsning kun fordi vi ikke kan få værdi ud af expression endnu forventer altid ExpressionVal
            Code += $"li $t{reg2}, {(repeatFor.Expression as ExpressionVal).Value.ToString()}";
            Code += $"loop{Label}:";
            if (repeatFor.Direction.Incrementing)
            {
                Code += $"beq $t{reg2}, $t{reg1}, end{Label}"; //Branch
                Code += $"addi $t{reg1}, $t{reg1}, 1";                
            }
            else
            {
                Code += $"beq $t{reg2}, $t{reg1}, end{Label}";
                Code += $"addi $t{reg1}, $t{reg1}, -1"; //subi findes ikke derfor bruges addi med negativt tal
            }
            repeatFor.Body.Accept(this);
            Code += $"j loop{Label}";
            Code += $"end{Label}:";
            FreeReg(reg1); FreeReg(reg2);
        }
        private int GetReg()
        {
            for (int i = 0; i < Regs.Length; i++)
            {
                if (!Regs[i])
                {
                    Regs[i] = true;
                    return i;
                }

            }
            return 9;
        }
        private void FreeReg(int reg)
        {
            Regs[reg] = false;
        }
    }
}
