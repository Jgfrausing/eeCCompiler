﻿//Generated by the GOLD Parser Builder

using System.Collections.Generic;
using System.IO;
using eeCCompiler.Interfaces;
using eeCCompiler.Nodes;
using GOLD;

namespace eeCCompiler
{
    internal class MyParser
    {
        private readonly Parser _parser = new Parser();

        private readonly Stack<AbstractSyntaxTree> _reductionStack = new Stack<AbstractSyntaxTree>();
            //Is public for debugging only

        public Root Root;

        public MyParser()
        {
            //Loads the tables created by GOLD parser
            _parser.LoadTables(@"..\..\eec.egt");
            Errors = new List<string>();
        }

        public List<string> Errors { get; set; }

        private int Line { get; set; }
        private int Column { get; set; }

        public bool Parse(TextReader reader)
        {
            //This procedure starts the GOLD Parser Engine and handles each of the messages it returns. 
            //The resulting tree is a pure representation of the language and will be ready to implement.

            var accepted = false; //Was the parse successful?
            _parser.Open(reader);
            _parser.TrimReductions = false; //Ommits reduntant reductions

            var done = false;
            var errorOccur = false;
            while (!done)
            {
                var response = _parser.Parse();
                var a = _parser.CurrentToken();
                switch (response)
                {
                    case ParseMessage.Reduction:
                        //Create a customized object to store the reduction
                        var currentReduction = CreateNewObject(_parser.CurrentReduction as Reduction);
                        _reductionStack.Push(currentReduction);
                        break;

                    case ParseMessage.Accept:
                        //Accepted!
                        Root = _reductionStack.Pop() as Root;
                        done = true;
                        accepted = true;
                        break;

                    case ParseMessage.TokenRead:
                        var token = _parser.CurrentToken();
                        //Reads tokens
                        Column = token.Position().Column;
                        Line = token.Position().Line;
                        break;

                        #region Errors

                    case ParseMessage.LexicalError:
                        //Cannot recognize token           
                        Errors.Add("Lexical error, token not recognized:\n" +
                                   "Position: " + _parser.CurrentPosition().Line + ", " +
                                   _parser.CurrentPosition().Column + "\n" +
                                   "Token Read: " + _parser.CurrentToken().Data + "\n" +
                                   "Expecting: " + _parser.ExpectedSymbols().Text());

                        done = true;
                        break;

                    case ParseMessage.SyntaxError:
                        //Expecting a different token
                        Errors.Add("Syntax Error:\n" +
                                   "Position: " + _parser.CurrentPosition().Line + ", " +
                                   _parser.CurrentPosition().Column + "\n" +
                                   "Token Read: " + _parser.CurrentToken().Data + "\n" +
                                   "Expecting: " + _parser.ExpectedSymbols().Text());
                        done = true;
                        break;

                    case ParseMessage.InternalError:
                        //INTERNAL ERROR! Something is horribly wrong.
                        Errors.Add("Internal error - something went wrong.");
                        done = true;
                        break;

                    case ParseMessage.NotLoadedError:
                        //This error occurs if the CGT was not loaded.   
                        Errors.Add("Error: CGT not loaded.");
                        done = true;
                        break;

                    case ParseMessage.GroupError:
                        //GROUP ERROR! Unexpected end of file
                        Errors.Add("Error: Unexpected end of file");
                        done = true;
                        break;

                        #endregion
                }
            } //while
            if (_reductionStack.Count != 0)
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
            switch ((Indexes.Indexes.ProductionIndex) r.Parent.TableIndex())
            {
                    #region Program

                case Indexes.Indexes.ProductionIndex.Program_Program_Lbrace_Rbrace:
                    // <Program> ::= <consts> <struct_defs> program '{' <body> '}' <Func_decls>
                    result = new Root(_reductionStack.Pop() as FunctionDeclarations, _reductionStack.Pop() as Body,
                        _reductionStack.Pop() as StructDefinitions, _reductionStack.Pop() as ConstantDefinitions,
                        _reductionStack.Pop() as Includes);
                    break;

                    #endregion

                    #region Include

                case Indexes.Indexes.ProductionIndex.Includes_Include:
                    // <includes> ::= include <include> <includes>
                    var includes = _reductionStack.Pop() as Includes;
                    includes.IncludeList.Insert(0, _reductionStack.Pop() as Include);
                    result = includes;
                    break;

                case Indexes.Indexes.ProductionIndex.Includes:
                    // <includes> ::= 
                    result = new Includes();
                    break;

                case Indexes.Indexes.ProductionIndex.Include_Id_Dot:
                    // <include> ::= Id '.' <include>
                    var include = _reductionStack.Pop() as Include;
                    include.Identifiers.Insert(0, new Identifier(r.get_Data(0).ToString()));
                    result = include;
                    break;

                case Indexes.Indexes.ProductionIndex.Include_Id:
                    // <include> ::= Id 
                    result = new Include(new List<Identifier> {new Identifier(r.get_Data(0).ToString())});
                    break;

                    #endregion

                    #region Const

                case Indexes.Indexes.ProductionIndex.Consts:
                    // <consts> ::= <const> <consts>
                    result = CreateConstants();
                    break;

                case Indexes.Indexes.ProductionIndex.Consts2:
                    // <consts> ::= 
                    result = new ConstantDefinitions();
                    break;

                case Indexes.Indexes.ProductionIndex.Const_Const_Id_Semi:
                    // <const> ::= const Id <const_part> ';'
                    result = new Constant(_reductionStack.Pop() as IConstantPart,
                        new Identifier(r.get_Data(1).ToString()));
                    break;

                    #endregion

                    #region Func_decl

                case Indexes.Indexes.ProductionIndex.Func_decls:
                    // <Func_decls> ::= <func_decl> <Func_decls>
                    result = CreateFunctionDeclarationList();
                    break;

                case Indexes.Indexes.ProductionIndex.Func_decls2:
                    // <Func_decls> ::= 
                    result = new FunctionDeclarations();
                    break;

                case Indexes.Indexes.ProductionIndex.Func_decl_Lparen_Rparen_Lbrace_Rbrace:
                    // <func_decl> ::= <typeid> '(' <typeid_list> ')' '{' <body> '}'
                    result = new FunctionDeclaration(_reductionStack.Pop() as Body, _reductionStack.Pop() as TypeIdList,
                        _reductionStack.Pop() as TypeId);
                    break;

                    #endregion

                    #region TypeId

                case Indexes.Indexes.ProductionIndex.Typeid_list:
                // <typeid_list> ::= <ref_typeid> <extra_typeid>
                //FALLTHROUGH

                case Indexes.Indexes.ProductionIndex.Extra_typeid_Comma:
                    // <extra_typeid> ::= ',' <ref_typeid> <extra_typeid>
                    result = CreateTypeIdList();
                    break;

                case Indexes.Indexes.ProductionIndex.Typeid_list2:
                // <typeid_list> ::= 
                //FALLTHROUGH

                case Indexes.Indexes.ProductionIndex.Extra_typeid:
                    // <extra_typeid> ::= 
                    result = new TypeIdList();
                    break;

                case Indexes.Indexes.ProductionIndex.Ref_typeid:
                    // <ref_typeid> ::= <ref> <typeid>
                    result = new RefTypeId(_reductionStack.Pop() as TypeId, _reductionStack.Pop() as Ref);
                    break;

                case Indexes.Indexes.ProductionIndex.Typeid_Id:
                    // <typeid> ::= <type> Id
                    result = new TypeId(new Identifier(r.get_Data(1).ToString()), _reductionStack.Pop() as IType);
                    break;

                case Indexes.Indexes.ProductionIndex.Typeid_Id2:
                    // <typeid> ::= <list> Id
                    result = new TypeId(new Identifier(r.get_Data(1).ToString()), _reductionStack.Pop() as IType);
                    break;

                case Indexes.Indexes.ProductionIndex.Brackets_Lbracketrbracket:
                    // <brackets> ::= '[]' <brackets>
                    result = new ListDimentions(_reductionStack.Pop() as ListDimentions);
                    break;

                case Indexes.Indexes.ProductionIndex.Brackets_Lbracketrbracket2:
                    // <brackets> ::= '[]'
                    result = new ListDimentions();
                    break;

                    #endregion

                    #region ref

                case Indexes.Indexes.ProductionIndex.Ref_Ref:
                    // <ref> ::= ref
                    result = new Ref(true);
                    break;

                case Indexes.Indexes.ProductionIndex.Ref:
                    // <ref> ::=
                    result = new Ref(false);
                    break;

                case Indexes.Indexes.ProductionIndex.Ref_id_Ref_Id:
                    // <ref_id> ::= ref Id
                    result = new RefId(new Identifier(r.get_Data(1).ToString()));
                    break;

                    #endregion

                    #region Var_decl

                case Indexes.Indexes.ProductionIndex.Var_decls_Semi:
                    // <var_decls> ::= <var_decl> ';' <var_decls>
                    result = CreateVarDeclerations();
                    break;

                case Indexes.Indexes.ProductionIndex.Var_decls:
                    // <var_decls> ::= 
                    result = new VarDeclerations();
                    break;

                case Indexes.Indexes.ProductionIndex.Var_decl_Id:
                    // <var_decl> ::= Id <assign_opr> <expr>
                    result = new VarDecleration(_reductionStack.Pop() as IExpression,
                        _reductionStack.Pop() as AssignmentOperator, new Identifier(r.get_Data(0).ToString()));
                    break;

                case Indexes.Indexes.ProductionIndex.Var_decl_Id2:
                    // <var_decl> ::= Id <assign_opr> <list>
                    result = new VarDecleration(_reductionStack.Pop() as IExpression,
                        _reductionStack.Pop() as AssignmentOperator, new Identifier(r.get_Data(0).ToString()));
                    break;

                case Indexes.Indexes.ProductionIndex.Var_decl:
                    // <var_decl> ::= <refrence> <assign_opr> <expr>
                    result = new VarInStructDecleration(_reductionStack.Pop() as IExpression,
                        _reductionStack.Pop() as AssignmentOperator, _reductionStack.Pop() as Refrence);
                    break;
                case Indexes.Indexes.ProductionIndex.List:
                    // <list> ::= <type> <brackets>
                    result = new ListType(_reductionStack.Pop() as ListDimentions, _reductionStack.Pop() as IType);
                    break;

                    #endregion

                    #region Struct

                case Indexes.Indexes.ProductionIndex.Struct_decl_Id_Id_Lbrace_Rbrace:
                    // <struct_decl> ::= Id <assign_opr> Id '{' <var_decls> '}'
                    result = new StructDecleration(_reductionStack.Pop() as VarDeclerations,
                        new Identifier(r.get_Data(2).ToString()), _reductionStack.Pop() as AssignmentOperator,
                        new Identifier(r.get_Data(0).ToString()));
                    break;

                case Indexes.Indexes.ProductionIndex.Struct_defs:
                    // <struct_defs> ::= <struct_def> <struct_defs>
                    result = CreateStructDefinitions();
                    break;

                case Indexes.Indexes.ProductionIndex.Struct_defs2:
                    // <struct_defs> ::= 
                    result = new StructDefinitions();
                    break;

                case Indexes.Indexes.ProductionIndex.Struct_def_Struct_Id_Lbrace_Rbrace:
                    // <struct_def> ::= struct Id '{' <struct_parts> '}'
                    result = new StructDefinition(_reductionStack.Pop() as StructParts,
                        new Identifier(r.get_Data(1).ToString()));
                    break;

                case Indexes.Indexes.ProductionIndex.Struct_parts_Semi:
                // <struct_parts> ::= <var_decl> ';' <struct_parts>
                //FALLTHROUGH
                case Indexes.Indexes.ProductionIndex.Struct_parts_Semi2:
                // <struct_parts> ::= <struct_decl> ';' <struct_parts>
                //FALLTHROUGH

                case Indexes.Indexes.ProductionIndex.Struct_parts:
                    // <struct_parts> ::= <func_decl> ';' <struct_parts>
                    result = CreateStructParts();
                    break;

                case Indexes.Indexes.ProductionIndex.Struct_parts2:
                    // <struct_parts> ::= 
                    result = new StructParts();
                    break;

                    #endregion

                    #region Operators

                case Indexes.Indexes.ProductionIndex.Operator_Lt:
                    // <operator> ::= '<'
                    result = new Operator(Indexes.Indexes.SymbolIndex.Lt);
                    break;

                case Indexes.Indexes.ProductionIndex.Operator_Gt:
                    // <operator> ::= '>'
                    result = new Operator(Indexes.Indexes.SymbolIndex.Gt);
                    break;

                case Indexes.Indexes.ProductionIndex.Operator_Lteq:
                    // <operator> ::= '<='
                    result = new Operator(Indexes.Indexes.SymbolIndex.Lteq);
                    break;

                case Indexes.Indexes.ProductionIndex.Operator_Gteq:
                    // <operator> ::= '>='
                    result = new Operator(Indexes.Indexes.SymbolIndex.Gteq);
                    break;

                case Indexes.Indexes.ProductionIndex.Operator_And:
                    // <operator> ::= and
                    result = new Operator(Indexes.Indexes.SymbolIndex.And);
                    break;

                case Indexes.Indexes.ProductionIndex.Operator_Or:
                    // <operator> ::= or
                    result = new Operator(Indexes.Indexes.SymbolIndex.Or);
                    break;

                case Indexes.Indexes.ProductionIndex.Operator_Eqeq:
                    // <operator> ::= '=='
                    result = new Operator(Indexes.Indexes.SymbolIndex.Eqeq);
                    break;

                case Indexes.Indexes.ProductionIndex.Operator_Exclameq:
                    // <operator> ::= '!='
                    result = new Operator(Indexes.Indexes.SymbolIndex.Exclameq);
                    break;

                case Indexes.Indexes.ProductionIndex.Operator_Times:
                    // <operator> ::= '*'
                    result = new Operator(Indexes.Indexes.SymbolIndex.Times);
                    break;

                case Indexes.Indexes.ProductionIndex.Operator_Div:
                    // <operator> ::= '/'
                    result = new Operator(Indexes.Indexes.SymbolIndex.Div);
                    break;

                case Indexes.Indexes.ProductionIndex.Operator_Mod:
                    // <operator> ::= mod
                    result = new Operator(Indexes.Indexes.SymbolIndex.Mod);
                    break;

                case Indexes.Indexes.ProductionIndex.Operator_Plus:
                    // <operator> ::= '+'
                    result = new Operator(Indexes.Indexes.SymbolIndex.Plus);
                    break;

                case Indexes.Indexes.ProductionIndex.Operator_Minus:
                    // <operator> ::= '-'
                    result = new Operator(Indexes.Indexes.SymbolIndex.Minus);
                    break;

                case Indexes.Indexes.ProductionIndex.Assign_opr_Eq:
                    // <assign_opr> ::= '='
                    result = new AssignmentOperator(Indexes.Indexes.SymbolIndex.Eq);
                    break;

                case Indexes.Indexes.ProductionIndex.Assign_opr_Pluseq:
                    // <assign_opr> ::= '+='
                    result = new AssignmentOperator(Indexes.Indexes.SymbolIndex.Pluseq);
                    break;

                case Indexes.Indexes.ProductionIndex.Assign_opr_Minuseq:
                    // <assign_opr> ::= '-='
                    result = new AssignmentOperator(Indexes.Indexes.SymbolIndex.Minuseq);
                    break;

                    #endregion

                    #region Types

                case Indexes.Indexes.ProductionIndex.Type_Void:
                    // <type> ::= void
                    result = new Type("void");
                    break;

                case Indexes.Indexes.ProductionIndex.Type_String:
                    // <type> ::= string
                    result = new Type("string");
                    break;

                case Indexes.Indexes.ProductionIndex.Type_Num:
                    // <type> ::= num
                    result = new Type("num");
                    break;

                case Indexes.Indexes.ProductionIndex.Type_Bool:
                    // <type> ::= bool
                    result = new Type("bool");
                    break;

                case Indexes.Indexes.ProductionIndex.Type_Id:
                    // <type> ::= Id
                    result = new Identifier(r.get_Data(0).ToString());
                    break;

                    #endregion

                    #region Values

                case Indexes.Indexes.ProductionIndex.Value_Floatliteral:
                // <value> ::= FloatLiteral
                //FALLTHROUGH

                case Indexes.Indexes.ProductionIndex.Const_part_Floatliteral:
                    // <const_part> ::= FloatLiteral
                    result = new NumValue(double.Parse(r.get_Data(0).ToString()));
                    break;

                case Indexes.Indexes.ProductionIndex.Value_Stringliteral:
                // <value> ::= StringLiteral
                //FALLTHROUGH

                case Indexes.Indexes.ProductionIndex.Const_part_Stringliteral:
                    // <const_part> ::= StringLiteral
                    result = new StringValue(r.get_Data(0).ToString());
                    break;

                case Indexes.Indexes.ProductionIndex.Value_Booleanliteral:
                // <value> ::= BooleanLiteral
                //FALLTHROUGH
                case Indexes.Indexes.ProductionIndex.Const_part_Booleanliteral:
                    // <const_part> ::= BooleanLiteral'
                    result = new BoolValue(bool.Parse(r.get_Data(0).ToString()));
                    break;

                case Indexes.Indexes.ProductionIndex.Value:
                    // <value> ::= <refrence>
                    result = _reductionStack.Pop();
                    break;


                case Indexes.Indexes.ProductionIndex.Var_refrence_w_id_Id:
                    // <var_refrence_w_id> ::= Id
                    result = new Identifier(r.get_Data(0).ToString());
                    break;


                case Indexes.Indexes.ProductionIndex.Var_refrence_w_id:
                // <var_refrence_w_id> ::= <id_index>
                case Indexes.Indexes.ProductionIndex.Func_refrence:
                // <func_refrence> ::= <func_call>
                case Indexes.Indexes.ProductionIndex.Refrence:
                    // <refrence> ::= <func_call>
                    result = _reductionStack.Pop();
                    break;

                case Indexes.Indexes.ProductionIndex.Refrence_Id:
                    // <refrence> ::= Id
                    result = new Identifier(r.get_Data(0).ToString());
                    break;

                case Indexes.Indexes.ProductionIndex.Func_refrence_Id_Dot:
                // <func_refrence> ::= Id '.' <func_refrence>

                case Indexes.Indexes.ProductionIndex.Var_refrence_Id_Dot:
                // <var_refrence> ::= Id '.' <var_refrence_w_id>


                case Indexes.Indexes.ProductionIndex.Var_refrence_w_id_Id_Dot:
                // <var_refrence_w_id> ::= Id '.' <var_refrence_w_id>

                case Indexes.Indexes.ProductionIndex.Refrence_Id_Dot:
                    // <refrence> ::= Id '.' <refrence>
                    result = new Refrence(_reductionStack.Pop() as IStructRefrence,
                        new Identifier(r.get_Data(0).ToString()));
                    break;

                case Indexes.Indexes.ProductionIndex.Func_refrence_Dot:
                // <func_refrence> ::= <id_index> '.' <func_refrence>

                case Indexes.Indexes.ProductionIndex.Var_refrence_Dot:
                // <var_refrence> ::= <id_index> '.' <var_refrence_w_id>

                case Indexes.Indexes.ProductionIndex.Var_refrence_w_id_Dot:
                // <var_refrence_w_id> ::= <id_index> '.' <var_refrence_w_id>
                case Indexes.Indexes.ProductionIndex.Refrence_Dot:
                    // <refrence> ::= <id_index> '.' <refrence>
                    result = new Refrence(_reductionStack.Pop() as IStructRefrence,
                        _reductionStack.Pop() as IStructRefrence);
                    break;

                case Indexes.Indexes.ProductionIndex.Var_refrence:
                // <var_refrence> ::= <id_index>
                case Indexes.Indexes.ProductionIndex.Refrence2:
                    // <refrence> ::= <id_index>
                    result = _reductionStack.Pop() as IdIndex;
                    break;

                case Indexes.Indexes.ProductionIndex.Id_index_Id:
                    // <id_index> ::= Id <index>
                    result = new IdIndex(_reductionStack.Pop() as ListIndex, new Identifier(r.get_Data(0).ToString()));
                    break;

                case Indexes.Indexes.ProductionIndex.Index_Lbracket_Rbracket:
                    // <index> ::= '[' <value> ']' <index>
                    result = new ListIndex(_reductionStack.Pop() as ListIndex, _reductionStack.Pop() as IExpression);
                    break;
                case Indexes.Indexes.ProductionIndex.Index_Lbracket_Rbracket2:
                    // <index> ::= '[' <value> ']'
                    //var f = _reductionStack.Pop().GetType();
                    result = new ListIndex(_reductionStack.Pop() as IExpression);
                    break;

                    #endregion

                    #region Body&Bodypart

                case Indexes.Indexes.ProductionIndex.Body:
                    // <body> ::= <bodypart> <body>

                    result = CreateBody();
                    break;

                case Indexes.Indexes.ProductionIndex.Body2:
                    // <body> ::= 
                    result = new Body();
                    break;

                case Indexes.Indexes.ProductionIndex.Bodypart_Semi:
                    // <bodypart> ::= <var_decl> ';'

                    result = _reductionStack.Pop();
                    break;

                case Indexes.Indexes.ProductionIndex.Bodypart_Semi2:
                    // <bodypart> ::= <struct_decl> ';'
                    result = _reductionStack.Pop();
                    break;

                case Indexes.Indexes.ProductionIndex.Bodypart_Semi3:
                    // <bodypart> ::= <func_refrence> ';'
                    result = _reductionStack.Pop();
                    break;

                case Indexes.Indexes.ProductionIndex.Bodypart:
                    // <bodypart> ::= <ctrl_stmt>
                    result = _reductionStack.Pop();
                    break;

                case Indexes.Indexes.ProductionIndex.Bodypart_Return_Semi:
                    // <bodypart> ::= return <expr> ';'
                    result = new Return(_reductionStack.Pop() as IExpression);
                    break;

                    #endregion

                    #region Expressions

                case Indexes.Indexes.ProductionIndex.Expr:
                    // <expr> ::= <value> <operator> <expr>
                    result = new ExpressionValOpExpr(_reductionStack.Pop() as IExpression,
                        _reductionStack.Pop() as Operator, _reductionStack.Pop() as IValue);
                    break;

                case Indexes.Indexes.ProductionIndex.Expr2:
                    // <expr> ::= <value>
                    result = new ExpressionVal(_reductionStack.Pop() as IValue);
                    break;

                case Indexes.Indexes.ProductionIndex.Expr_Lparen_Rparen:
                    // <expr> ::= '(' <expr> ')'
                    result = new ExpressionParen(_reductionStack.Pop() as IExpression);
                    break;

                case Indexes.Indexes.ProductionIndex.Expr_Minus:
                    // <expr> ::= '-' <expr>
                    result = new ExpressionMinus(_reductionStack.Pop() as IExpression);
                    break;

                case Indexes.Indexes.ProductionIndex.Expr_Lparen_Rparen2:
                    // <expr> ::= '(' <expr> ')' <operator> <expr>
                    result = new ExpressionParenOpExpr(_reductionStack.Pop() as IExpression,
                        _reductionStack.Pop() as Operator, _reductionStack.Pop() as IExpression);
                    break;

                case Indexes.Indexes.ProductionIndex.Expr_Exclam:
                    // <expr> ::= '!' <expr>
                    result = new ExpressionNegate(_reductionStack.Pop() as IExpression);
                    break;

                case Indexes.Indexes.ProductionIndex.Expr_list:
                // <expr_list> ::= <expr> <opt_exprs
                //Fallthrough

                case Indexes.Indexes.ProductionIndex.Opt_exprs_Comma:
                // <opt_exprs> ::= ',' <expr> <opt_exprs>
                // FAllthrough

                case Indexes.Indexes.ProductionIndex.Expr_list2:
                // <expr_list> ::= <ref_id> <opt_exprs>
                //Fallthrough

                case Indexes.Indexes.ProductionIndex.Opt_exprs_Comma2:
                    // <opt_exprs> ::= ',' <ref_id> <opt_exprs>
                    result = CreateExpressionList();
                    break;

                case Indexes.Indexes.ProductionIndex.Expr_list3:
                // <expr_list> ::= 
                //FallThrough

                case Indexes.Indexes.ProductionIndex.Opt_exprs:
                    // <opt_exprs> ::= 
                    result = new ExpressionList();
                    break;

                    #endregion

                    #region ControlStatements

                case Indexes.Indexes.ProductionIndex.Ctrl_stmt_If_Lbrace_Rbrace:
                // <ctrl_stmt> ::= if <expr> '{' <body> '}' <if_exp>
                //FALLTHROUGH

                case Indexes.Indexes.ProductionIndex.If_exp_Else_If_Lbrace_Rbrace:
                    // <if_exp> ::= else if <expr> '{' <body> '}' <if_exp>
                    result = new IfStatement(_reductionStack.Pop() as ElseStatement, _reductionStack.Pop() as Body,
                        _reductionStack.Pop() as IExpression);
                    break;

                case Indexes.Indexes.ProductionIndex.Ctrl_stmt_Repeat_Lbrace_Rbrace:
                // <ctrl_stmt> ::= repeat <var_decl> <direction> <expr> '{' <body> '}'
                //FALLTHROUGH

                case Indexes.Indexes.ProductionIndex.Ctrl_stmt_Repeat_Lparen_Rparen_Lbrace_Rbrace:
                    // <ctrl_stmt> ::= repeat '(' <var_decl> <direction> <expr> ')' '{' <body> '}'
                    result = new RepeatFor(_reductionStack.Pop() as Body, _reductionStack.Pop() as IExpression,
                        _reductionStack.Pop() as Direction, _reductionStack.Pop() as VarDecleration);
                    break;

                case Indexes.Indexes.ProductionIndex.Ctrl_stmt_Repeat_Lbrace_Rbrace2:
                    // <ctrl_stmt> ::= repeat <expr> '{' Body '}'
                    result = new RepeatExpr(_reductionStack.Pop() as Body, _reductionStack.Pop() as IExpression);
                    break;

                case Indexes.Indexes.ProductionIndex.If_exp_Else_Lbrace_Rbrace:
                    // <if_exp> ::= else '{' <body> '}'
                    result = new ElseStatement(_reductionStack.Pop() as Body);
                    break;

                case Indexes.Indexes.ProductionIndex.If_exp:
                    // <if_exp> ::= 
                    result = new ElseStatement(new Body());
                    break;

                    #endregion

                    #region Direction

                case Indexes.Indexes.ProductionIndex.Direction_Downto:
                    // <direction> ::= downto
                    result = new Direction(false);
                    break;

                case Indexes.Indexes.ProductionIndex.Direction_To:
                    // <direction> ::= to
                    result = new Direction(true);
                    break;

                    #endregion

                    #region FunctionCall

                case Indexes.Indexes.ProductionIndex.Func_call_Id_Lparen_Rparen:
                    // <func_call> ::= Id '(' <expr_list> ')'
                    result = new FuncCall((_reductionStack.Pop() as ExpressionList).Expressions,
                        new Identifier(r.get_Data(0).ToString()));
                    break;

                    #endregion
            } //switch
            result.Line = Line;
            result.Column = Column;
            return result;
        }

        private AbstractSyntaxTree CreateFunctionDeclarationList()
        {
            var funcDecls = _reductionStack.Pop() as FunctionDeclarations;
            funcDecls.FunctionDeclarationList.Insert(0, _reductionStack.Pop() as FunctionDeclaration);

            return funcDecls;
        }

        private AbstractSyntaxTree CreateTypeIdList()
        {
            var typeIds = _reductionStack.Pop() as TypeIdList;
            typeIds.TypeIds.Insert(0, _reductionStack.Pop() as RefTypeId);

            return typeIds;
        }

        private AbstractSyntaxTree CreateVarDeclerations()
        {
            var varDecls = _reductionStack.Pop() as VarDeclerations;
            varDecls.VarDeclerationList.Insert(0, _reductionStack.Pop() as VarDecleration);

            return varDecls;
        }

        private AbstractSyntaxTree CreateStructDefinitions()
        {
            var structDefs = _reductionStack.Pop() as StructDefinitions;
            structDefs.Definitions.Insert(0, _reductionStack.Pop() as StructDefinition);

            return structDefs;
        }

        private AbstractSyntaxTree CreateStructParts()
        {
            var structParts = _reductionStack.Pop() as StructParts;
            structParts.StructPartList.Insert(0, _reductionStack.Pop() as IStructPart);

            return structParts;
        }

        private AbstractSyntaxTree CreateBody()
        {
            var body = _reductionStack.Pop() as Body;
            body.Bodyparts.Insert(0, _reductionStack.Pop() as IBodypart);

            return body;
        }

        private AbstractSyntaxTree CreateConstants()
        {
            var constants = _reductionStack.Pop() as ConstantDefinitions;
            constants.ConstantList.Insert(0, _reductionStack.Pop() as Constant);
            return constants;
        }

        private AbstractSyntaxTree CreateExpressionList()
        {
            var exprs = _reductionStack.Pop() as ExpressionList;
            exprs.Expressions.Insert(0, _reductionStack.Pop() as IExprListElement);

            return exprs;
        }
    }
} //MyParser