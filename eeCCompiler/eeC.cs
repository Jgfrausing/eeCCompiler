﻿//Generated by the GOLD Parser Builder

using System.Collections.Generic;
using System.IO;
using eeCCompiler.Indexes;
using eeCCompiler.Interfaces;
using eeCCompiler.Nodes;
using GOLD;

internal class MyParser
{
    private readonly Parser _parser = new Parser();
    public readonly Stack<Identifier> Identifiers = new Stack<Identifier>();

    public readonly List<AbstractSyntaxTree> List = new List<AbstractSyntaxTree>();
    public readonly Stack<AbstractSyntaxTree> Stack = new Stack<AbstractSyntaxTree>();

    public Program program; //You might derive a specific object

    public MyParser()
    {
        //This procedure can be called to load the parse tables. The class can
        //read tables using a BinaryReader.

        _parser.LoadTables("eec.egt");
    }

    public bool Parse(TextReader reader)
    {
        //This procedure starts the GOLD Parser Engine and handles each of the
        //messages it returns. Each time a reduction is made, you can create new
        //custom object and reassign the .CurrentReduction property. Otherwise, 
        //the system will use the Reduction object that was returned.
        //
        //The resulting tree will be a pure representation of the language 
        //and will be ready to implement.

        var accepted = false; //Was the parse successful?

        _parser.Open(reader);
        _parser.TrimReductions = false; //Please read about this feature before enabling  

        AbstractSyntaxTree currentReduction = null;
        var done = false;
        while (!done)
        {
            var response = _parser.Parse();

            switch (response)
            {
                case ParseMessage.Reduction:
                    //Create a customized object to store the reduction

                    currentReduction = CreateNewObject(_parser.CurrentReduction as Reduction);
                    if (currentReduction != null)
                        Stack.Push(currentReduction);
                    List.Add(currentReduction);
                    break;

                case ParseMessage.Accept:
                    //Accepted!
                    program = Stack.Pop() as Program;
                    done = true;
                    accepted = true;
                    break;

                case ParseMessage.TokenRead:
                    //Console.WriteLine("______________________________________________________");
                    //Console.WriteLine("Data: " + _parser.CurrentToken().Data.ToString());
                    //Console.WriteLine("Parent: " + _parser.CurrentToken().Parent.ToString());
                    //Console.WriteLine("Position: " + _parser.CurrentToken().Position().ToString());
                    //Console.WriteLine("Type: " + _parser.CurrentToken().Type().ToString());
                    if (_parser.CurrentToken().Parent.ToString() == "Id")
                        Identifiers.Push(new Identifier(_parser.CurrentToken().Data.ToString()));
                    //You don't have to do anything here.
                    break;

                    #region Errors

                case ParseMessage.LexicalError:
                    //Cannot recognize token
                    done = true;
                    break;

                case ParseMessage.SyntaxError:
                    //Expecting a different token
                    done = true;
                    break;

                case ParseMessage.InternalError:
                    //INTERNAL ERROR! Something is horribly wrong.
                    done = true;
                    break;

                case ParseMessage.NotLoadedError:
                    //This error occurs if the CGT was not loaded.                   
                    done = true;
                    break;

                case ParseMessage.GroupError:
                    //GROUP ERROR! Unexpected end of file
                    done = true;
                    break;

                    #endregion
            }
        } //while

        return accepted;
    }

    private AbstractSyntaxTree CreateNewObject(Reduction r)
    {
        AbstractSyntaxTree result = null;
        Body body;
        Operator opr;
        IExpression expr;
        IExpressionList exprs;
        Identifier id;
        var bodyparts = new List<IBodypart>();
        var expressionList = new List<IExpression>();
        Constant constant;
        Constants constantList;
        var constants = new List<Constant>();
        switch ((Indexes.ProductionIndex) r.Parent.TableIndex())
        {
            #region Program NOT DONE

            case Indexes.ProductionIndex.Program_Program_Lbrace_Rbrace:
                // <Program> ::= <consts> <struct_defs> program '{' <body> '}' <Func_decls>
                body = Stack.Pop() as Body;
                constantList = Stack.Pop() as Constants;
                result = new Program(constantList.ConstantList, body);
                break;

            #endregion

            #region Const NOT DONE

            case Indexes.ProductionIndex.Consts:
                // <consts> ::= <const> <consts>
                constantList = Stack.Pop() as Constants;
                constant = Stack.Pop() as Constant;
                constants.Add(constant);
                constants.AddRange(constantList.ConstantList);
                result = new Constants(constants);
                break;

            case Indexes.ProductionIndex.Consts2:
                // <consts> ::= 
                result = new Constants();
                break;

            case Indexes.ProductionIndex.Const_Const_Id_Semi:
                // <const> ::= const Id <const_part> ';'
                result = new Constant(Identifiers.Pop(), Stack.Pop() as IConstantPart);
                break;

            case Indexes.ProductionIndex.Const_part_Floatliteral:
                // <const_part> ::= FloatLiteral
                result = new NumValue(2.2);
                break;

            case Indexes.ProductionIndex.Const_part_Stringliteral:
                // <const_part> ::= StringLiteral
                result = new StringValue("HELP_CONST");
                break;

            case Indexes.ProductionIndex.Const_part_Booleanliteral:
                // <const_part> ::= BooleanLiteral'
                result = new BoolValue(false);
                break;

            #endregion

            #region Func_decl NOT DONE

            case Indexes.ProductionIndex.Func_decls:
                // <Func_decls> ::= <func_decl> <Func_decls>
                break;

            case Indexes.ProductionIndex.Func_decls2:
                // <Func_decls> ::= 
                break;

            case Indexes.ProductionIndex.Func_decl_Lparen_Rparen_Lbrace_Rbrace:
                // <func_decl> ::= <typeid> '(' <typeid_list> ')' '{' <body> '}'
                break;

            #endregion

            #region TypeId NOT DONE

            case Indexes.ProductionIndex.Typeid_list:
                // <typeid_list> ::= <typeid> <extra_typeid>
                break;

            case Indexes.ProductionIndex.Typeid_list2:
                // <typeid_list> ::= 
                break;

            case Indexes.ProductionIndex.Extra_typeid_Comma:
                // <extra_typeid> ::= ',' <typeid> <extra_typeid>
                break;

            case Indexes.ProductionIndex.Extra_typeid:
                // <extra_typeid> ::= 
                break;

            case Indexes.ProductionIndex.Typeid_Id:
                // <typeid> ::= <type> Id
                break;

            #endregion

            #region Var_decl NOT DONE

            case Indexes.ProductionIndex.Var_decls:
                // <var_decls> ::= <var_decl> <var_decls>
                break;

            case Indexes.ProductionIndex.Var_decls2:
                // <var_decls> ::= 
                result = Stack.Pop();
                break;

            case Indexes.ProductionIndex.Var_decl_Id_Eq:
                // <var_decl> ::= Id '=' <expr>
                expr = Stack.Pop() as IExpression;
                id = Identifiers.Pop();
                result = new VarDecleration(id, expr);
                break;

            #endregion

            #region Struct NOT DONE

            case Indexes.ProductionIndex.Struct_decl_Id_Eq_Id_Lbrace_Rbrace_Semi:
                // <struct_decl> ::= Id '=' Id '{' <var_decls> '}' ';'
                break;

            case Indexes.ProductionIndex.Struct_defs:
                // <struct_defs> ::= <struct_def> <struct_defs>
                break;

            case Indexes.ProductionIndex.Struct_defs2:
                // <struct_defs> ::= 
                break;

            case Indexes.ProductionIndex.Struct_def_Struct_Id_Lbrace_Rbrace:
                // <struct_def> ::= struct Id '{' <struct_parts> '}'
                break;

            case Indexes.ProductionIndex.Struct_parts:
                // <struct_parts> ::= <var_decl> <struct_parts>
                break;

            case Indexes.ProductionIndex.Struct_parts2:
                // <struct_parts> ::= <func_decl> <struct_parts>
                break;

            case Indexes.ProductionIndex.Struct_parts3:
                // <struct_parts> ::= 
                break;

            #endregion

            #region Operators

            case Indexes.ProductionIndex.Operator_Lt:
                // <operator> ::= '<'
                result = new Operator(Indexes.SymbolIndex.Lt);
                break;

            case Indexes.ProductionIndex.Operator_Gt:
                // <operator> ::= '>'
                result = new Operator(Indexes.SymbolIndex.Gt);
                break;

            case Indexes.ProductionIndex.Operator_Lteq:
                // <operator> ::= '<='
                result = new Operator(Indexes.SymbolIndex.Lteq);
                break;

            case Indexes.ProductionIndex.Operator_Gteq:
                // <operator> ::= '>='
                result = new Operator(Indexes.SymbolIndex.Gteq);
                break;

            case Indexes.ProductionIndex.Operator_And:
                // <operator> ::= and
                result = new Operator(Indexes.SymbolIndex.And);
                break;

            case Indexes.ProductionIndex.Operator_Or:
                // <operator> ::= or
                result = new Operator(Indexes.SymbolIndex.Or);
                break;

            case Indexes.ProductionIndex.Operator_Eqeq:
                // <operator> ::= '=='
                result = new Operator(Indexes.SymbolIndex.Eqeq);
                break;

            case Indexes.ProductionIndex.Operator_Exclameq:
                // <operator> ::= '!='
                result = new Operator(Indexes.SymbolIndex.Exclameq);
                break;

            case Indexes.ProductionIndex.Operator_Times:
                // <operator> ::= '*'
                result = new Operator(Indexes.SymbolIndex.Times);
                break;

            case Indexes.ProductionIndex.Operator_Div:
                // <operator> ::= '/'
                result = new Operator(Indexes.SymbolIndex.Div);
                break;

            case Indexes.ProductionIndex.Operator_Mod:
                // <operator> ::= mod
                result = new Operator(Indexes.SymbolIndex.Mod);
                break;

            case Indexes.ProductionIndex.Operator_Plus:
                // <operator> ::= '+'
                result = new Operator(Indexes.SymbolIndex.Plus);
                break;

            case Indexes.ProductionIndex.Operator_Minus:
                // <operator> ::= '-'
                result = new Operator(Indexes.SymbolIndex.Minus);
                break;

            #endregion

            #region Types NOT DONE

            case Indexes.ProductionIndex.Type_Void:
                // <type> ::= void
                break;

            case Indexes.ProductionIndex.Type_String:
                // <type> ::= string
                break;

            case Indexes.ProductionIndex.Type_Num:
                // <type> ::= num
                break;

            case Indexes.ProductionIndex.Type_Bool:
                // <type> ::= bool
                break;

            #endregion

            #region Values NOT DONE

            case Indexes.ProductionIndex.Value_Floatliteral:
                // <value> ::= FloatLiteral
                result = new NumValue(1.1);
                break;

            case Indexes.ProductionIndex.Value_Stringliteral:
                // <value> ::= StringLiteral
                result = new StringValue("HELP");
                break;

            case Indexes.ProductionIndex.Value_Booleanliteral:
                // <value> ::= BooleanLiteral
                result = new BoolValue(false);
                break;

            case Indexes.ProductionIndex.Value:
                // <value> ::= <func_call>
                result = Stack.Pop();
                break;

            case Indexes.ProductionIndex.Value_Id:
                // <value> ::= Id
                result = Identifiers.Pop();
                break;

            #endregion

            #region Body&Bodypart NOT DONE

            case Indexes.ProductionIndex.Body:
                // <body> ::= <bodypart> <body>

                var b = Stack.Pop() as Body;
                var bp = Stack.Pop() as IBodypart;
                bodyparts.Add(bp);
                bodyparts.AddRange(b.Bodyparts);

                result = new Body(bodyparts);
                break;

            case Indexes.ProductionIndex.Body2:
                // <body> ::= 
                result = new Body();
                break;

            case Indexes.ProductionIndex.Bodypart_Semi:
                // <bodypart> ::= <var_decl> ';'

                result = Stack.Pop();
                break;

            case Indexes.ProductionIndex.Bodypart_Semi2:
                // <bodypart> ::= <struct_decl> ';'
                break;

            case Indexes.ProductionIndex.Bodypart_Semi3:
                // <bodypart> ::= <func_call> ';'
                exprs = Stack.Pop() as IExpressionList;
                id = Stack.Pop() as Identifier;
                result = new FuncCall(id, exprs.IExpressions);
                break;

            case Indexes.ProductionIndex.Bodypart:
                // <bodypart> ::= <ctrl_stmt>
                result = Stack.Pop();
                break;

            #endregion

            #region Expressions NOT DONE

            case Indexes.ProductionIndex.Expr:
                // <expr> ::= <value> <operator> <expr>
                expr = Stack.Pop() as IExpression;
                opr = Stack.Pop() as Operator;
                result = new ExpressionValOpExpr(Stack.Pop() as IValue, opr, expr);
                break;

            case Indexes.ProductionIndex.Expr2:
                // <expr> ::= <value>
                result = new ExpressionVal(Stack.Pop() as IValue);
                break;

            case Indexes.ProductionIndex.Expr_Lparen_Rparen:
                // <expr> ::= '(' <expr> ')'
                result = new ExpressionParen(Stack.Pop() as IExpression);
                break;

            case Indexes.ProductionIndex.Expr_Minus:
                // <expr> ::= '-' <value>
                result = new ExpressionMinus(Stack.Pop() as IExpression);
                break;

            case Indexes.ProductionIndex.Expr_Lparen_Rparen2:
                // <expr> ::= '(' <expr> ')' <operator> <expr>
                expr = Stack.Pop() as IExpression;
                opr = Stack.Pop() as Operator;
                result = new ExpressionParenOpExpr(Stack.Pop() as IExpression, opr, expr);
                break;

            case Indexes.ProductionIndex.Expr_Exclam:
                // <expr> ::= '!' <expr>
                result = new ExpressionNegate(Stack.Pop() as IExpression);
                break;

            case Indexes.ProductionIndex.Expr_list:
                // <expr_list> ::= <expr> <opt_exprs>
                exprs = Stack.Pop() as IExpressionList;
                expr = Stack.Pop() as IExpression;
                expressionList.Add(expr);
                expressionList.AddRange(exprs.IExpressions);
                result = new IExpressionList();
                break;

            case Indexes.ProductionIndex.Expr_list2:
                // <expr_list> ::= 
                result = new IExpressionList();
                break;

            case Indexes.ProductionIndex.Opt_exprs_Comma:
                // <opt_exprs> ::= ',' <expr> <opt_exprs>
                break;

            case Indexes.ProductionIndex.Opt_exprs:
                // <opt_exprs> ::= 
                break;

            #endregion

            #region ControlStatements NOT DONE

            case Indexes.ProductionIndex.Ctrl_stmt_If_Lbrace_Rbrace:
                // <ctrl_stmt> ::= if <expr> '{' <body> '}' <if_exp>
                body = Stack.Pop() as Body;
                expr = Stack.Pop() as IExpression;
                result = new IfStatement(expr, body);
                break;

            case Indexes.ProductionIndex.Ctrl_stmt_Repeat_Lbrace_Rbrace:
                // <ctrl_stmt> ::= repeat <var_decl> <direction> <expr> '{' <body> '}'
                break;

            case Indexes.ProductionIndex.Ctrl_stmt_Repeat_Lparen_Rparen_Lbrace_Rbrace:
                // <ctrl_stmt> ::= repeat '(' <var_decl> <direction> <expr> ')' '{' <body> '}'
                break;

            case Indexes.ProductionIndex.Ctrl_stmt_Repeat:
                // <ctrl_stmt> ::= repeat <expr>
                break;

            case Indexes.ProductionIndex.If_exp_Else_If_Lbrace_Rbrace:
                // <if_exp> ::= else if <expr> '{' <body> '}' <if_exp>
                break;

            case Indexes.ProductionIndex.If_exp_Else_Lbrace_Rbrace:
                // <if_exp> ::= else '{' <body> '}'
                break;

            case Indexes.ProductionIndex.If_exp:
                // <if_exp> ::= 
                break;

                #endregion

            #region Direction NOT DONE

            case Indexes.ProductionIndex.Direction_Downto:
                // <direction> ::= downto
                break;

            case Indexes.ProductionIndex.Direction_To:
                // <direction> ::= to
                break;

            #endregion

            #region FunctionCall

            case Indexes.ProductionIndex.Func_call_Id_Lparen_Rparen:
                // <func_call> ::= Id '(' <expr_list> ')'
                exprs = Stack.Pop() as IExpressionList;
                id = Identifiers.Pop();
                result = new FuncCall(id, exprs.IExpressions);
                break;

            #endregion

        } //switch

        return result;
    }
} //MyParser