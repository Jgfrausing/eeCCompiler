using System.Collections.Generic;
using eeCCompiler.Interfaces;
using eeCCompiler.Nodes;

namespace eeCCompiler.Visitors
{
    internal class MipsPrinter : Visitor
    {
        private readonly bool[] Regs = new bool[8];
        private readonly List<string> UsedVariables = new List<string>();
        public string File => Data + Text;

        public MipsPrinter()
        {
            LabelCount = 0;
            Data = ".data\n";
            Text = ".text\n.globl main\nmain:\n";
        }

        private int LabelCount { get; set; }
        public string Data { get; set; }
        public string Text { get; set; }


        public override void Visit(Root root)
        {
            base.Visit(root);
            Text += "li $v0, 10\nsyscall\n";
        }

        public override void Visit(VarDecleration varDecleration) //Skal lige fixes lidt med stacken
        {
            varDecleration.Identifier.Accept(this);
            varDecleration.Expression.Accept(this);
            if (varDecleration.AssignmentOperator.Symbol == Indexes.Indexes.SymbolIndex.Eq)
            {
                var reg1 = NextReg();
                Text += $"lw  $t{reg1}, 0($sp)\n"; //Pop i reg1
                Text += "addi $sp, $sp, 4\n";
                Text += $"sw $t{reg1}, {varDecleration.Identifier.Id}\n";
                FreeReg(reg1);
            }
            else if (varDecleration.AssignmentOperator.Symbol == Indexes.Indexes.SymbolIndex.Pluseq)
                //Ikke stack implementeret.
            {
                var reg1 = GetReg();
                var reg2 = GetReg();
                Text += $"lw $t{reg2}, {varDecleration.Identifier.Id}\n";
                Text += $"add $t{reg2}, $t{reg1}, $t{reg2}\n";
                Text += $"sw $t{reg2}, {varDecleration.Identifier.Id}\n";
                FreeReg(reg2);
                FreeReg(reg1);
            }
            else if (varDecleration.AssignmentOperator.Symbol == Indexes.Indexes.SymbolIndex.Minuseq)
                //Ikke stack implementeret.
            {
                var reg1 = GetReg();
                var reg2 = GetReg();
                Text += $"lw $t{reg2}, {varDecleration.Identifier.Id}\n";
                Text += $"sub $t{reg2}, $t{reg2}, $t{reg1}\n";
                Text += $"sw $t{reg2}, {varDecleration.Identifier.Id}\n";
                FreeReg(reg2);
                FreeReg(reg1);
            }
        }

        public override void Visit(Identifier identifier)
        {
            if (!UsedVariables.Contains(identifier.Id))
            {
                Data += $"{identifier.Id}: .word 0\n";
                UsedVariables.Add(identifier.Id);
            }
        }

        public override void Visit(RepeatFor repeatFor)
        {
            repeatFor.VarDecleration.Accept(this);
            var Label = LabelCount++; //Sikre sig at vi får nye labels hvis vi nester repeats
            int reg1 = GetReg(), reg2 = GetReg();
            Text += $"lw $t{reg1}, {repeatFor.VarDecleration.Identifier.Id}\n";
                //Temp løsning kun fordi vi ikke kan få værdi ud af expression endnu forventer altid ExpressionVal
            Text += $"li $t{reg2}, {(repeatFor.Expression as ExpressionVal).Value}\n";
            Text += $"loop{Label}:\n";
            if (repeatFor.Direction.Incrementing)
            {
                Text += $"bge $t{reg1}, $t{reg2}, end{Label}\n"; //Branch
                repeatFor.Body.Accept(this);
                Text += $"addi $t{reg1}, $t{reg1}, 1\n";
                Text += $"sw $t{reg1}, {repeatFor.VarDecleration.Identifier}\n";
            }
            else
            {
                Text += $"ble $t{reg1}, $t{reg2}, end{Label}\n";
                repeatFor.Body.Accept(this);
                Text += $"addi $t{reg1}, $t{reg1}, -1\n"; //subi findes ikke derfor bruges addi med negativt tal
            }
            Text += $"j loop{Label}\n";
            Text += $"end{Label}:\n";
            FreeReg(reg1);
            FreeReg(reg2);
        }

        public override void Visit(ExpressionValOpExpr expressionValOpExpr) //Check med short circuit på et tidspunkt.
        {
            if (expressionValOpExpr.Operator.Symbol == Indexes.Indexes.SymbolIndex.And)
            {
                AndOprShortCircuit(expressionValOpExpr.Value, expressionValOpExpr.Expression);
            }
            else
            {
                int reg1 = GetReg(), reg2;

                Text += $"addi $sp, $sp, -4\n";

                if (expressionValOpExpr.Value.ToString() == "true")
                    Text += $"li $t{reg1}, 1\n";
                else if (expressionValOpExpr.Value.ToString() == "false")
                    Text += $"li $t{reg1}, 0\n";
                else
                    Text += $"li $t{reg1}, {expressionValOpExpr.Value}\n";

                Text += $"sw   $t{reg1}, 0($sp)\n";
                FreeReg(reg1);
                expressionValOpExpr.Expression.Accept(this);
                reg1 = GetReg();
                reg2 = GetReg();

                Pop(reg1);
                Pop(reg2);

                ExpressionOperatorPrint(expressionValOpExpr.Operator.Symbol, reg1, reg2);

                /* Push reg 1 på stack*/
                Push(reg1);
            }
        }

        public override void Visit(ExpressionVal expressionVal)
        {
            var reg1 = GetReg();
            Text += $"addi $sp, $sp, -4\n";
            if (expressionVal.Value is Identifier)
                Text += $"lw $t{reg1}, {expressionVal.Value}\n";
            else if (expressionVal.Value.ToString() == "true")
                Text += $"li $t{reg1}, 1\n";
            else if (expressionVal.Value.ToString() == "false")
                Text += $"li $t{reg1}, 0\n";
            else
                Text += $"li $t{reg1}, {expressionVal.Value}\n";
            Text += $"sw   $t{reg1}, 0($sp)\n";
            FreeReg(reg1);
        }

        public override void Visit(ExpressionParenOpExpr expressionParenOpExpr)
        {
            if (expressionParenOpExpr.Operator.Symbol == Indexes.Indexes.SymbolIndex.And) // Short circuit ved and
            {
                AndOprShortCircuit(expressionParenOpExpr.ExpressionParen, expressionParenOpExpr.Expression);
            }
            else
            {
                expressionParenOpExpr.ExpressionParen.Accept(this);
                expressionParenOpExpr.Expression.Accept(this);
                var reg1 = GetReg();
                var reg2 = GetReg();

                Pop(reg1);
                Pop(reg2);

                ExpressionOperatorPrint(expressionParenOpExpr.Operator.Symbol, reg1, reg2);

                /* Push reg 1 på stack*/
                Push(reg1);
            }
        }

        public override void Visit(ExpressionExprOpExpr expressionExprOpExpr)
        {
            int reg1, reg2;
            if (expressionExprOpExpr.Operator.Symbol == Indexes.Indexes.SymbolIndex.And) // Short circuit ved and
            {
                AndOprShortCircuit(expressionExprOpExpr.ExpressionParen, expressionExprOpExpr.Expression);
            }
            else
            {
                expressionExprOpExpr.ExpressionParen.Accept(this);
                expressionExprOpExpr.Expression.Accept(this);
                reg1 = GetReg();
                reg2 = GetReg();

                Pop(reg1);
                Pop(reg2);

                ExpressionOperatorPrint(expressionExprOpExpr.Operator.Symbol, reg1, reg2);

                /* Push reg 1 på stack*/
                Push(reg1);
            }
        }

        public override void Visit(RepeatExpr repeatExpr)
        {
            var Label = LabelCount++;
            var reg1 = NextReg();
            Text += $"loop{Label}:\n";
            repeatExpr.Expression.Accept(this);
            Text += $"beq $t{reg1}, $zero, end{Label}\n";
            repeatExpr.Body.Accept(this);
            Text += $"j loop{Label}\n";
            Text += $"end{Label}:\n";
        }

        public override void Visit(IfStatement ifStatement)
        {
            var label1 = LabelCount++;
            var label2 = LabelCount++;
            ifStatement.Expression.Accept(this);
            var reg1 = GetReg();
            Pop(reg1);
            Text += $"beq $t{reg1}, $zero, end{label1}\n";
            FreeReg(reg1);
            ifStatement.Body.Accept(this);
            Text += $"j end{label2}\n";
            Text += $"end{label1}:\n";
            ifStatement.ElseStatement.Accept(this);
            Text += $"end{label2}:\n";
        }

        public override void Visit(ElseStatement elseStatement)
        {
            elseStatement.Body.Accept(this);
        }

        public override void Visit(FuncCall funcCall)
        {
            if (funcCall.Identifier.Id == "program_print" && funcCall.IsBodyPart)
            {
                foreach (var element in ((funcCall.Expressions[0] as ExpressionVal).Value as StringValue).Elements)
                {
                    if (element is TypeId)
                    {
                        Text += $"lw $a0, {(element as TypeId).Identifier.Id}\n";
                        Text += $"li $v0, 1\n";
                        Text += $"syscall\n";
                        Text += "addi $a0, $0, 0xA\n";
                        Text += "addi $v0, $0, 0xB\n";
                        Text += "syscall\n";
                    }
                }
            }
        }


        private void AndOprShortCircuit(IValue Val, IExpression Expr)
        {
            var reg1 = NextReg();

            Text += $"addi $sp, $sp, -4\n";

            if (Val.ToString() == "true")
                Text += $"li $t{reg1}, 1\n";
            else if (Val.ToString() == "false")
                Text += $"li $t{reg1}, 0\n";
            else
                Text += $"li $t{reg1}, {Val}\n";

            Text += $"sw   $t{reg1}, 0($sp)\n";

            var label = LabelCount++;
            reg1 = GetReg();
            Pop(reg1); //Får resultat fra ExpressionParen
            Text += $"beq $t{reg1}, $zero, end{label}\n"; //Checker om værdi er false hvis den er jump til endlabel
            Expr.Accept(this); //Besøger expression
            var reg2 = GetReg(); //Tager et extra reg til Expression værdi
            Pop(reg2); //Popper expressions værdi ind i reg2
            ExpressionOperatorPrint(Indexes.Indexes.SymbolIndex.And, reg1, reg2); //Checker operator (and)
            Push(reg1);
            Text += $"end{label}:\n"; //End Label
        }

        private void AndOprShortCircuit(IExpression Expr1, IExpression Expr2)
        {
            var reg1 = NextReg();
            Expr1.Accept(this);
            var label = LabelCount++;
            reg1 = GetReg();
            Pop(reg1); //Får resultat fra ExpressionParen
            Text += $"beq $t{reg1}, $zero, end{label}\n"; //Checker om værdi er false hvis den er jump til endlabel
            Expr2.Accept(this); //Besøger expression
            var reg2 = GetReg(); //Tager et extra reg til Expression værdi
            Pop(reg2); //Popper expressions værdi ind i reg2
            ExpressionOperatorPrint(Indexes.Indexes.SymbolIndex.And, reg1, reg2); //Checker operator (and)
            Push(reg1);
            Text += $"end{label}:\n"; //End Label
        }

        private void ExpressionOperatorPrint(Indexes.Indexes.SymbolIndex Opr, int reg1, int reg2)
        {
            if (Opr == Indexes.Indexes.SymbolIndex.Plus)
                Text += $"add $t{reg1}, $t{reg2}, $t{reg1}\n";
            else if (Opr == Indexes.Indexes.SymbolIndex.Minus)
                Text += $"sub $t{reg1}, $t{reg2}, $t{reg1}\n";
            else if (Opr == Indexes.Indexes.SymbolIndex.Times)
                Text += $"mul $t{reg1}, $t{reg2}, $t{reg1}\n";
            else if (Opr == Indexes.Indexes.SymbolIndex.Div)
                Text += $"div $t{reg1}, $t{reg2}, $t{reg1}\n";
            else if (Opr == Indexes.Indexes.SymbolIndex.Mod)
                Text += $"rem $t{reg1}, $t{reg2}, $t{reg1}\n";
            else if (Opr == Indexes.Indexes.SymbolIndex.Eqeq)
                Text += $"seq $t{reg1}, $t{reg2}, $t{reg1}\n";
            //Text += $"subu $t{reg1}, $t{reg1}, $t{reg2}\n"; //reg1 will be 0 if reg1 and reg2 are equal, and non-zero otherwise
            //Text += $"sltu $t{reg1}, $zero, $t{reg1}\n"; // Set reg1 to 1 if it's non-zero
            //Text += $"xori $t{reg1}, $t{reg1}, 1\n"; //Flip the lsb so that 0 becomes 1, and 1 becomes 0
            else if (Opr == Indexes.Indexes.SymbolIndex.Exclameq)
                Text += $"sne $t{reg1}, $t{reg2}, $t{reg1}\n";
            //Text += $"subu $t{reg1}, $t{reg1}, $t{reg2}\n"; //reg1 will be 0 if reg1 and reg2 are equal, and non-zero otherwise
            //Text += $"sltu $t{reg1}, $zero, $t{reg1}\n"; // Set reg1 to 1 if it's non-zero
            else if (Opr == Indexes.Indexes.SymbolIndex.Lt)
                Text += $"slt $t{reg1}, $t{reg2}, $t{reg1}\n";
            else if (Opr == Indexes.Indexes.SymbolIndex.Lteq)
                Text += $"sle $t{reg1}, $t{reg2}, $t{reg1}\n";
            else if (Opr == Indexes.Indexes.SymbolIndex.Gt)
                Text += $"sgt  $t{reg1}, $t{reg2}, $t{reg1}\n";
            else if (Opr == Indexes.Indexes.SymbolIndex.Gteq)
                Text += $"sge  $t{reg1}, $t{reg2}, $t{reg1}\n";
            else if (Opr == Indexes.Indexes.SymbolIndex.Gteq)
                Text += $"sge  $t{reg1}, $t{reg2}, $t{reg1}\n";
            else if (Opr == Indexes.Indexes.SymbolIndex.And)
                Text += $"and $t{reg1}, $t{reg2}, $t{reg1}\n";
            else if (Opr == Indexes.Indexes.SymbolIndex.Or)
                Text += $"or $t{reg1}, $t{reg2}, $t{reg1}\n";

            FreeReg(reg1);
            FreeReg(reg2);
        }

        private void Push(int reg)
        {
            Text += $"addi $sp, $sp, -4\n";
            Text += $"sw   $t{reg}, 0($sp)\n";
        }

        private void Pop(int reg)
        {
            Text += $"lw  $t{reg}, 0($sp)\n"; //Pop i reg1
            Text += "addi $sp, $sp, 4\n";
        }

        private int GetReg() //tager et register til funktionen
        {
            for (var i = 0; i < Regs.Length; i++)
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
            for (var i = 0; i < Regs.Length; i++)
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