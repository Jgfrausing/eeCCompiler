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
            Data = ".data\n";
            Text = ".text\n.globl main\nmain:\n";
            StackHeight = 0;
        }
        private int LabelCount { get; set; }
        public string Data { get; set; }
        public string Text { get; set; }
        private bool[] Regs = new bool[8];
        private List<string> UsedVariables = new List<string>();
        public int StackHeight { get; set; } //Word size dvs aktuel stacksize er StackHeight/4

        public override void Visit(VarDecleration varDecleration) //Skal lige fixes lidt med stacken
        {
            varDecleration.Identifier.Accept(this);
            int reg1 = NextReg(); 
            varDecleration.Expression.Accept(this);
            if (varDecleration.AssignmentOperator.Symbol == Indexes.Indexes.SymbolIndex.Eq)
            {
                Text += $"lw  $t{reg1}, 0($sp)\n"; //Pop i reg1
                Text += "addi $sp, $sp, 4\n";
                Text += $"sw $t{reg1}, {varDecleration.Identifier.Id}\n";
            }
            else if (varDecleration.AssignmentOperator.Symbol == Indexes.Indexes.SymbolIndex.Pluseq) //Ikke stack implementeret.
            {
                reg1 = GetReg(); //Skal gettes så vi kan få fat på reg2, ingen betydning senere da vi free'er lige efter
                int reg2 = GetReg();
                Text += $"lw $t{reg2}, {varDecleration.Identifier.Id}\n";
                Text += $"add $t{reg2}, $t{reg1}, $t{reg2}\n";
                Text += $"sw $t{reg2}, {varDecleration.Identifier.Id}\n";
                FreeReg(reg2); FreeReg(reg1);
            }
        }
        public override void Visit(Identifier identifier)
        {
            if (!UsedVariables.Contains(identifier.Id)) {
                Data += $"{identifier.Id}: .word 0\n";
                UsedVariables.Add(identifier.Id);
            }
        }
        public override void Visit(RepeatFor repeatFor)
        {
            int Label = LabelCount++; //Sikre sig at vi får nye labels hvis vi nester repeats
            int reg1 = GetReg(), reg2 = GetReg();
            Text += $"li $t{reg1}, {(repeatFor.VarDecleration.Expression as ExpressionVal).Value.ToString()}\n";  //Temp løsning kun fordi vi ikke kan få værdi ud af expression endnu forventer altid ExpressionVal
            Text += $"li $t{reg2}, {(repeatFor.Expression as ExpressionVal).Value.ToString()}\n";
            Text += $"loop{Label}:\n";
            if (repeatFor.Direction.Incrementing)
            {
                Text += $"beq $t{reg2}, $t{reg1}, end{Label}\n"; //Branch
                Text += $"addi $t{reg1}, $t{reg1}, 1\n";                
            }
            else
            {
                Text += $"beq $t{reg2}, $t{reg1}, end{Label}\n";
                Text += $"addi $t{reg1}, $t{reg1}, -1\n"; //subi findes ikke derfor bruges addi med negativt tal
            }
            repeatFor.Body.Accept(this);
            Text += $"j loop{Label}\n";
            Text += $"end{Label}:\n";
            FreeReg(reg1); FreeReg(reg2);
        }
        public override void Visit(ExpressionValOpExpr expressionValOpExpr)
        {
            int reg1 = GetReg(),reg2;
            //Text += $"li $t{reg1}, {expressionVal.Value}\n";
            Text += $"addi $sp, $sp, -4\n";
            Text += $"li $t{reg1}, {expressionValOpExpr.Value}\n";
            Text += $"sw   $t{reg1}, 0($sp)\n";
            FreeReg(reg1);
            expressionValOpExpr.Expression.Accept(this);
            reg1 = GetReg(); reg2 = GetReg();

            Text += $"lw  $t{reg1}, 0($sp)\n"; //Pop i reg1
            Text += "addi $sp, $sp, 4\n";

            Text += $"lw  $t{reg2}, 0($sp)\n"; //Pop i reg2
            Text += "addi $sp, $sp, 4\n";

            if (expressionValOpExpr.Operator.Symbol == Indexes.Indexes.SymbolIndex.Plus)
                Text += $"add $t{reg1}, $t{reg2}, $t{reg1}\n";
            else if (expressionValOpExpr.Operator.Symbol == Indexes.Indexes.SymbolIndex.Minus)
                Text += $"sub $t{reg1}, $t{reg2}, $t{reg1}\n";
            else if (expressionValOpExpr.Operator.Symbol == Indexes.Indexes.SymbolIndex.Times)
                Text += $"mul $t{reg1}, $t{reg2}, $t{reg1}\n";
            else if (expressionValOpExpr.Operator.Symbol == Indexes.Indexes.SymbolIndex.Div)
                Text += $"div $t{reg1}, $t{reg2}, $t{reg1}\n";
            FreeReg(reg1); FreeReg(reg2);

            /* Push reg 1 på stack*/
            Text += $"addi $sp, $sp, -4\n";
            Text += $"sw   $t{reg1}, 0($sp)\n";
        }
        public override void Visit(ExpressionVal expressionVal)
        {
            int reg1 = GetReg();
            Text += $"addi $sp, $sp, -4\n";
            Text += $"li $t{reg1}, {expressionVal.Value}\n";
            Text += $"sw   $t{reg1}, 0($sp)\n";
            FreeReg(reg1);
        }
        public override void Visit(ExpressionParenOpExpr expressionParenOpExpr)
        {
            expressionParenOpExpr.ExpressionParen.Accept(this);
            expressionParenOpExpr.Expression.Accept(this);

            int reg1 = GetReg(); int reg2 = GetReg();

            Text += $"lw  $t{reg1}, 0($sp)\n"; //Pop i reg1
            Text += "addi $sp, $sp, 4\n";

            Text += $"lw  $t{reg2}, 0($sp)\n"; //Pop i reg2
            Text += "addi $sp, $sp, 4\n";

            if (expressionParenOpExpr.Operator.Symbol == Indexes.Indexes.SymbolIndex.Plus)
                Text += $"add $t{reg1}, $t{reg2}, $t{reg1}\n";
            else if (expressionParenOpExpr.Operator.Symbol == Indexes.Indexes.SymbolIndex.Minus)
                Text += $"sub $t{reg1}, $t{reg2}, $t{reg1}\n";
            else if (expressionParenOpExpr.Operator.Symbol == Indexes.Indexes.SymbolIndex.Times)
                Text += $"mul $t{reg1}, $t{reg2}, $t{reg1}\n";
            else if (expressionParenOpExpr.Operator.Symbol == Indexes.Indexes.SymbolIndex.Div)
                Text += $"div $t{reg1}, $t{reg2}, $t{reg1}\n";
            FreeReg(reg1); FreeReg(reg2);

            /* Push reg 1 på stack*/
            Text += $"addi $sp, $sp, -4\n";
            Text += $"sw   $t{reg1}, 0($sp)\n";
        }
        public override void Visit(ExpressionExprOpExpr expressionExprOpExpr)
        {
            expressionExprOpExpr.ExpressionParen.Accept(this);
            expressionExprOpExpr.Expression.Accept(this);

            int reg1 = GetReg(); int reg2 = GetReg();

            Text += $"lw  $t{reg1}, 0($sp)\n"; //Pop i reg1
            Text += "addi $sp, $sp, 4\n";

            Text += $"lw  $t{reg2}, 0($sp)\n"; //Pop i reg2
            Text += "addi $sp, $sp, 4\n";

            if (expressionExprOpExpr.Operator.Symbol == Indexes.Indexes.SymbolIndex.Plus)
                Text += $"add $t{reg1}, $t{reg2}, $t{reg1}\n";
            else if (expressionExprOpExpr.Operator.Symbol == Indexes.Indexes.SymbolIndex.Minus)
                Text += $"sub $t{reg1}, $t{reg2}, $t{reg1}\n";
            else if (expressionExprOpExpr.Operator.Symbol == Indexes.Indexes.SymbolIndex.Times)
                Text += $"mul $t{reg1}, $t{reg2}, $t{reg1}\n";
            else if (expressionExprOpExpr.Operator.Symbol == Indexes.Indexes.SymbolIndex.Div)
                Text += $"div $t{reg1}, $t{reg2}, $t{reg1}\n";
            FreeReg(reg1); FreeReg(reg2);

            /* Push reg 1 på stack*/
            Text += $"addi $sp, $sp, -4\n";
            Text += $"sw   $t{reg1}, 0($sp)\n";
        }
        private int GetReg() //tager et register til funktionen
        {
            for (int i = 0; i < Regs.Length; i++)
            {
                if (!Regs[i])
                {
                    Regs[i] = true;
                    return i;
                }

            }
            return 8;
        }
        private int NextReg() // Finder næste reg men tager det ikke 
        {
            for (int i = 0; i < Regs.Length; i++)
            {
                if (!Regs[i])
                { 
                    return i;
                }

            }
            return 8;
        }
        private void FreeReg(int reg)
        {
            if (reg < Regs.Length)
                Regs[reg] = false;
        }
    }
}
