﻿//Generated by the GOLD Parser Builder

using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using eeCCompiler.Indexes;
using eeCCompiler.Interfaces;
using eeCCompiler.Nodes;
using GOLD;

internal class MyParser
{
    private readonly Parser _parser = new Parser();
    public readonly Stack<Identifier> Identifiers = new Stack<Identifier>(); //Is public for debugging only

    public readonly List<AbstractSyntaxTree> List = new List<AbstractSyntaxTree>(); //Is public for debugging only
    public readonly Stack<AbstractSyntaxTree> Stack = new Stack<AbstractSyntaxTree>(); //Is public for debugging only
    public readonly Stack<string> Values = new Stack<string>(); //Is public for debugging only

    public Root Root;

    public MyParser()
    {
        //Loads the tables created by GOLD parser
        _parser.LoadTables("eec.egt");
    }

    public bool Parse(TextReader reader)
    {
        //This procedure starts the GOLD Parser Engine and handles each of the messages it returns. 
        //The resulting tree is a pure representation of the language and will be ready to implement.

        var accepted = false; //Was the parse successful?

        _parser.Open(reader);
        _parser.TrimReductions = false; //Please read about this feature before enabling  

        var done = false;
        while (!done)
        {
            var response = _parser.Parse();

            switch (response)
            {
                case ParseMessage.Reduction:
                    //Create a customized object to store the reduction
                    var currentReduction = CreateNewObject(_parser.CurrentReduction as Reduction);
                    if (currentReduction != null)
                        Stack.Push(currentReduction);
                    List.Add(currentReduction); //For debuging
                    break;

                case ParseMessage.Accept:
                    //Accepted!
                    Root = Stack.Pop() as Root;
                    done = true;
                    accepted = true;
                    break;

                case ParseMessage.TokenRead:
                    //Checkes token for Literals
                    var regex = new Regex(@"\w*Literal");
                    var match = regex.Match(_parser.CurrentToken().Parent.ToString());
                    if (match.Success)
                        Values.Push(_parser.CurrentToken().Data.ToString());
                    //Checkes token for Identifiers
                    if (_parser.CurrentToken().Parent.ToString() == "Id")
                        Identifiers.Push(new Identifier(_parser.CurrentToken().Data.ToString()));
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
        if (List.Contains(null) || Stack.Count != 0)
            accepted = false; //A reduction returned null or was handled wrong (Compiler error - NOT input error)
        return accepted;
    }

    private AbstractSyntaxTree CreateNewObject(Reduction r)
    {
        /*
        Giving that this is a buttom up parser nodes/reductions are pushed onto a stack when created.
        If a reduction needs other nodes, they are retrived from the stact in reverced order. 
        This means that a program gets created with:
            <Program> ::= <consts> <struct_defs> program '{' <body> '}' <Func_decls>
        <Func_decls> poped from the stack, THEN <body>, THEN <struct_defs>, and THEN <consts>

        Non terminals that reduces to nothing are made as a new instance of its corresponding class
        
        Non terminals that reduced to another single reduction is handled by adding a interface to the new reduction:
            <value> ::= <func_call>
        Here FunctionCall implements the IValue interface. Since all new reductions are pushed onto the stack we
        pops the <func_call> thats on the stack and then pushes it back. When a reduction later needs a <value>
        it expects a object implementing IValue.
        
        Non terminals that works as a list are implemented by retriving objects. (See CreateFunctionDeclarationList() method)
            <Func_decls> ::= <func_decl> <Func_decls>
        Here <Func_decls> (LHS) is popped from the stack. Then <func_decl> gets inserted at first element of <Func_decls>'s
        list. <Func_decls> is then pushed back onto the stack.

        There are multiple "possible" null references. These are handled by the "syntax error case" in Parse();. They are therefore
        not checked here.
        */

        AbstractSyntaxTree result = null;
        //Returnvalue   OBS! Is sometimes used as placeholder, when multiple objects needs popped from the stack.
        switch ((Indexes.ProductionIndex) r.Parent.TableIndex())
        {
                #region Program

            case Indexes.ProductionIndex.Program_Program_Lbrace_Rbrace:
                // <Program> ::= <consts> <struct_defs> program '{' <body> '}' <Func_decls>
                result = CreateProgram(result);
                break;

                #endregion

                #region Const

            case Indexes.ProductionIndex.Consts:
                // <consts> ::= <const> <consts>
                result = CreateConstants(result);
                break;

            case Indexes.ProductionIndex.Consts2:
                // <consts> ::= 
                result = new Constants();
                break;

            case Indexes.ProductionIndex.Const_Const_Id_Semi:
                // <const> ::= const Id <const_part> ';'
                result = new Constant(Identifiers.Pop(), Stack.Pop() as IConstantPart);
                break;

                #endregion

                #region Func_decl

            case Indexes.ProductionIndex.Func_decls:
                // <Func_decls> ::= <func_decl> <Func_decls>
                result = CreateFunctionDeclarationList(result);
                break;

            case Indexes.ProductionIndex.Func_decls2:
                // <Func_decls> ::= 
                result = new FunctionDeclarations();
                break;

            case Indexes.ProductionIndex.Func_decl_Lparen_Rparen_Lbrace_Rbrace:
                // <func_decl> ::= <typeid> '(' <typeid_list> ')' '{' <body> '}'
                result = CreateFunctionDeclaration(result);
                break;

                #endregion

                #region TypeId

            case Indexes.ProductionIndex.Typeid_list:
            // <typeid_list> ::= <typeid> <extra_typeid>
            //FALLTHROUGH

            case Indexes.ProductionIndex.Extra_typeid_Comma:
                result = CreateTypeIdList(result);
                break;

            case Indexes.ProductionIndex.Typeid_list2:
            // <typeid_list> ::= 
            //FALLTHROUGH

            case Indexes.ProductionIndex.Extra_typeid:
                // <extra_typeid> ::= 
                result = new TypeIdList();
                break;

            case Indexes.ProductionIndex.Typeid_Id:
                // <typeid> ::= <type> Id
                result = new TypeId(Stack.Pop() as Type, Identifiers.Pop());
                break;

                #endregion

                #region Var_decl

            case Indexes.ProductionIndex.Var_decls_Semi:
                // <var_decls> ::= <var_decl> <var_decls>
                result = CreateVarDeclerations(result);
                break;

            case Indexes.ProductionIndex.Var_decls:
                // <var_decls> ::= 
                result = new VarDeclerations();
                break;

            case Indexes.ProductionIndex.Var_decl_Id_Eq:
                // <var_decl> ::= Id '=' <expr> ';'
                result = Stack.Pop();
                result = new VarDecleration(Identifiers.Pop(), result as IExpression);
                break;

                #endregion

                #region Struct

            case Indexes.ProductionIndex.Struct_decl_Id_Eq_Id_Lbrace_Rbrace_Semi:
                // <struct_decl> ::= Id '=' Id '{' <var_decls> '}' ';'
                result = CreateStructDeclerations(result);
                break;

            case Indexes.ProductionIndex.Struct_defs:
                // <struct_defs> ::= <struct_def> <struct_defs>
                result = CreateStructDefinitions(result);
                break;

            case Indexes.ProductionIndex.Struct_defs2:
                // <struct_defs> ::= 
                result = new StructDefinitions();
                break;

            case Indexes.ProductionIndex.Struct_def_Struct_Id_Lbrace_Rbrace:
                // <struct_def> ::= struct Id '{' <struct_parts> '}'
                result = new StructDefinition(Identifiers.Pop(), Stack.Pop() as StructParts);
                break;

            case Indexes.ProductionIndex.Struct_parts_Semi:
            // <struct_parts> ::= <var_decl> <struct_parts>
            //FALLTHROUGH

            case Indexes.ProductionIndex.Struct_parts:
                // <struct_parts> ::= <func_decl> <struct_parts>
                result = CreateStructParts(result);
                break;

            case Indexes.ProductionIndex.Struct_parts2:
                // <struct_parts> ::= 
                result = new StructParts();
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

                #region Types

            case Indexes.ProductionIndex.Type_Void:
                // <type> ::= void
                result = new Type("void");
                break;

            case Indexes.ProductionIndex.Type_String:
                // <type> ::= string
                result = new Type("string");
                break;

            case Indexes.ProductionIndex.Type_Num:
                // <type> ::= num
                result = new Type("num");
                break;

            case Indexes.ProductionIndex.Type_Bool:
                // <type> ::= bool
                result = new Type("bool");
                break;

                #endregion

                #region Values

            case Indexes.ProductionIndex.Value_Floatliteral:
            // <value> ::= FloatLiteral
            //FALLTHROUGH

            case Indexes.ProductionIndex.Const_part_Floatliteral:
                // <const_part> ::= FloatLiteral
                result = new NumValue(double.Parse(Values.Pop()));
                break;

            case Indexes.ProductionIndex.Value_Stringliteral:
            // <value> ::= StringLiteral
            //FALLTHROUGH

            case Indexes.ProductionIndex.Const_part_Stringliteral:
                // <const_part> ::= StringLiteral
                result = new StringValue(Values.Pop());
                break;

            case Indexes.ProductionIndex.Value_Booleanliteral:
            // <value> ::= BooleanLiteral
            //FALLTHROUGH
            case Indexes.ProductionIndex.Const_part_Booleanliteral:
                // <const_part> ::= BooleanLiteral'
                result = new BoolValue(bool.Parse(Values.Pop()));
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

                #region Body&Bodypart

            case Indexes.ProductionIndex.Body:
                // <body> ::= <bodypart> <body>

                result = CreateBody(result);
                break;

            case Indexes.ProductionIndex.Body2:
                // <body> ::= 
                result = new Body();
                break;

            case Indexes.ProductionIndex.Bodypart_Semi:
                // <bodypart> ::= <var_decl> ';'

                result = Stack.Pop();
                break;

            case Indexes.ProductionIndex.Bodypart:
                // <bodypart> ::= <struct_decl> ';'
                result = Stack.Pop();
                break;

            case Indexes.ProductionIndex.Bodypart_Semi2:
                // <bodypart> ::= <func_call> ';'
                result = Stack.Pop();
                break;

            case Indexes.ProductionIndex.Bodypart2:
                // <bodypart> ::= <ctrl_stmt>
                result = Stack.Pop();
                break;

            case Indexes.ProductionIndex.Bodypart_Return_Semi:
                // <bodypart> ::= return <expr> ';'
                result = new Return(Stack.Pop() as IExpression);
                break;

                #endregion

                #region Expressions

            case Indexes.ProductionIndex.Expr:
                // <expr> ::= <value> <operator> <expr>
                result = CreateExpressionValOpExpr(result);
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
                result = CreateExprParenOpExpr(result);
                break;

            case Indexes.ProductionIndex.Expr_Exclam:
                // <expr> ::= '!' <expr>
                result = new ExpressionNegate(Stack.Pop() as IExpression);
                break;

            case Indexes.ProductionIndex.Expr_list:
            // <expr_list> ::= <expr> <opt_exprs>
            //Fallthrough

            case Indexes.ProductionIndex.Opt_exprs_Comma:
                // <opt_exprs> ::= ',' <expr> <opt_exprs>
                result = CreateExpressionList(result);
                break;

            case Indexes.ProductionIndex.Expr_list2:
            // <expr_list> ::= 
            //FallThrough

            case Indexes.ProductionIndex.Opt_exprs:
                // <opt_exprs> ::= 
                result = new ExpressionList();
                break;

                #endregion

                #region ControlStatements

            case Indexes.ProductionIndex.Ctrl_stmt_If_Lbrace_Rbrace:
            // <ctrl_stmt> ::= if <expr> '{' <body> '}' <if_exp>
            //FALLTHROUGH

            case Indexes.ProductionIndex.If_exp_Else_If_Lbrace_Rbrace:
                // <if_exp> ::= else if <expr> '{' <body> '}' <if_exp>
                result = CreateIfStatement(result);
                break;

            case Indexes.ProductionIndex.Ctrl_stmt_Repeat_Lbrace_Rbrace:
            // <ctrl_stmt> ::= repeat <var_decl> <direction> <expr> '{' <body> '}'
            //FALLTHROUGH

            case Indexes.ProductionIndex.Ctrl_stmt_Repeat_Lparen_Rparen_Lbrace_Rbrace:
                // <ctrl_stmt> ::= repeat '(' <var_decl> <direction> <expr> ')' '{' <body> '}'
                result = CreateRepeatFor(result);
                break;

            case Indexes.ProductionIndex.@Ctrl_stmt_Repeat_Lbrace_Rbrace2:
                // <ctrl_stmt> ::= repeat <expr> '{' Body '}'
                result = Stack.Pop();
                result = new RepeatExpr(Stack.Pop() as IExpression, result as Body);
                break;

            case Indexes.ProductionIndex.If_exp_Else_Lbrace_Rbrace:
                // <if_exp> ::= else '{' <body> '}'
                result = new ElseStatement(Stack.Pop() as Body);
                break;

            case Indexes.ProductionIndex.If_exp:
                // <if_exp> ::= 
                result = new ElseStatement(new Body());
                break;

                #endregion

                #region Direction

            case Indexes.ProductionIndex.Direction_Downto:
                // <direction> ::= downto
                result = new Direction(false);
                break;

            case Indexes.ProductionIndex.Direction_To:
                // <direction> ::= to
                result = new Direction(true);
                break;

                #endregion

                #region FunctionCall

            case Indexes.ProductionIndex.Func_call_Id_Lparen_Rparen:
                // <func_call> ::= Id '(' <expr_list> ')'
                result = new FuncCall(Identifiers.Pop(), (Stack.Pop() as ExpressionList).Expressions);
                break;

                #endregion
        } //switch

        return result;
    }

    private AbstractSyntaxTree CreateFunctionDeclarationList(AbstractSyntaxTree result)
    {
        var funcDecls = Stack.Pop() as FunctionDeclarations;
        funcDecls.FunctionDeclaration.Insert(0, Stack.Pop() as FunctionDeclaration);
        result = funcDecls;
        return result;
    }

    private AbstractSyntaxTree CreateFunctionDeclaration(AbstractSyntaxTree result)
    {
        var body = Stack.Pop() as Body;
        var typeIds = Stack.Pop() as TypeIdList;
        result = new FunctionDeclaration(Stack.Pop() as TypeId, typeIds, body);
        return result;
    }

    private AbstractSyntaxTree CreateTypeIdList(AbstractSyntaxTree result)
    {
        var typeIds = Stack.Pop() as TypeIdList;
        typeIds.TypeIds.Insert(0, Stack.Pop() as TypeId);
        result = typeIds;
        return result;
    }

    private AbstractSyntaxTree CreateVarDeclerations(AbstractSyntaxTree result)
    {
        var varDecls = Stack.Pop() as VarDeclerations;
        varDecls.VarDeclerationList.Insert(0, Stack.Pop() as VarDecleration);
        return varDecls;
    }

    private AbstractSyntaxTree CreateStructDeclerations(AbstractSyntaxTree result)
    {
        var varDecls = Stack.Pop() as VarDeclerations;
        var id = Identifiers.Pop();
        result = new StructDecleration(Identifiers.Pop(), id, varDecls);
        return result;
    }

    private AbstractSyntaxTree CreateStructDefinitions(AbstractSyntaxTree result)
    {
        var structDefs = Stack.Pop() as StructDefinitions;
        structDefs.Definitions.Insert(0, Stack.Pop() as StructDefinition);
        result = structDefs;
        return result;
    }

    private AbstractSyntaxTree CreateStructParts(AbstractSyntaxTree result)
    {
        var structParts = Stack.Pop() as StructParts;
        structParts.StructPartList.Insert(0, Stack.Pop() as VarDecleration);
        return structParts;
    }

    private AbstractSyntaxTree CreateBody(AbstractSyntaxTree result)
    {
        var body = Stack.Pop() as Body;
        body.Bodyparts.Insert(0, Stack.Pop() as IBodypart);

        result = body;
        return result;
    }

    private AbstractSyntaxTree CreateExprParenOpExpr(AbstractSyntaxTree result)
    {
        var expr = Stack.Pop() as IExpression;
        var opr = Stack.Pop() as Operator;
        result = new ExpressionParenOpExpr(Stack.Pop() as IExpression, opr, expr);
        return result;
    }

    private AbstractSyntaxTree CreateProgram(AbstractSyntaxTree result)
    {
        var funcDecls = Stack.Pop() as FunctionDeclarations;
        var body = Stack.Pop() as Body;
        var structDef = Stack.Pop() as StructDefinitions;
        var constantList = Stack.Pop() as Constants;

        return new Root(constantList.ConstantList, structDef.Definitions, body, funcDecls);
    }

    private AbstractSyntaxTree CreateConstants(AbstractSyntaxTree result)
    {
        var constants = Stack.Pop() as Constants;
        constants.ConstantList.Insert(0, Stack.Pop() as Constant);
        return constants;
    }

    private AbstractSyntaxTree CreateExpressionValOpExpr(AbstractSyntaxTree result)
    {
        var expr = Stack.Pop() as IExpression;
        var opr = Stack.Pop() as Operator;
        result = new ExpressionValOpExpr(Stack.Pop() as IValue, opr, expr);
        return result;
    }

    private AbstractSyntaxTree CreateExpressionList(AbstractSyntaxTree result)
    {
        var exprs = Stack.Pop() as ExpressionList;
        exprs.Expressions.Insert(0, Stack.Pop() as IExpression);
        result = exprs;
        return result;
    }

    private AbstractSyntaxTree CreateIfStatement(AbstractSyntaxTree result)
    {
        var elseStatement = Stack.Pop() as ElseStatement;
        var body = Stack.Pop() as Body;
        result = new IfStatement(Stack.Pop() as IExpression, body, elseStatement);
        return result;
    }

    private AbstractSyntaxTree CreateRepeatFor(AbstractSyntaxTree result)
    {
        var body = Stack.Pop() as Body;
        var expr = Stack.Pop() as IExpression;
        var direction = Stack.Pop() as Direction;
        result = new RepeatFor(Stack.Pop() as VarDecleration, direction, expr, body);
        return result;
    }
} //MyParser