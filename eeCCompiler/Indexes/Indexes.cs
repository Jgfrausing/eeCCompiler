namespace eeCCompiler.Indexes
{
    public class Indexes
    {
        public enum ProductionIndex
        {
            @Program_Program_Lbrace_Rbrace = 0,        // <Program> ::= <includes> <consts> <struct_defs> program '{' <body> '}' <Func_decls>
            @Includes_Include = 1,                     // <includes> ::= include <include> <includes>
            @Includes = 2,                             // <includes> ::= 
            @Include_Id_Dot = 3,                       // <include> ::= Id '.' <include>
            @Include_Id = 4,                           // <include> ::= Id
            @Consts = 5,                               // <consts> ::= <const> <consts>
            @Consts2 = 6,                              // <consts> ::= 
            @Const_Const_Id_Semi = 7,                  // <const> ::= const Id <const_part> ';'
            @Const_part_Floatliteral = 8,              // <const_part> ::= FloatLiteral
            @Const_part_Stringliteral = 9,             // <const_part> ::= StringLiteral
            @Const_part_Booleanliteral = 10,           // <const_part> ::= BooleanLiteral
            @Func_decls = 11,                          // <Func_decls> ::= <func_decl> <Func_decls>
            @Func_decls2 = 12,                         // <Func_decls> ::= 
            @Func_decl_Lparen_Rparen_Lbrace_Rbrace = 13,  // <func_decl> ::= <typeid> '(' <typeid_list> ')' '{' <body> '}'
            @Typeid_list = 14,                         // <typeid_list> ::= <ref_typeid> <extra_typeid>
            @Typeid_list2 = 15,                        // <typeid_list> ::= 
            @Extra_typeid_Comma = 16,                  // <extra_typeid> ::= ',' <ref_typeid> <extra_typeid>
            @Extra_typeid = 17,                        // <extra_typeid> ::= 
            @Ref_typeid = 18,                          // <ref_typeid> ::= <ref> <typeid>
            @Ref_Ref = 19,                             // <ref> ::= ref
            @Ref = 20,                                 // <ref> ::= 
            @Typeid_Id = 21,                           // <typeid> ::= <type> Id
            @Typeid_Id2 = 22,                          // <typeid> ::= <list> Id
            @Brackets_Lbracketrbracket = 23,           // <brackets> ::= '[]' <brackets>
            @Brackets_Lbracketrbracket2 = 24,          // <brackets> ::= '[]'
            @Var_decls_Semi = 25,                      // <var_decls> ::= <var_decl> ';' <var_decls>
            @Var_decls = 26,                           // <var_decls> ::= 
            @Var_decl_Id = 27,                         // <var_decl> ::= Id <assign_opr> <expr>
            @Var_decl_Id2 = 28,                        // <var_decl> ::= Id <assign_opr> <list>
            @Var_decl = 29,                            // <var_decl> ::= <var_refrence> <assign_opr> <expr>
            @List = 30,                                // <list> ::= <type> <brackets>
            @Struct_decl_Id_Id_Lbrace_Rbrace = 31,     // <struct_decl> ::= Id <assign_opr> Id '{' <var_decls> '}'
            @Struct_defs = 32,                         // <struct_defs> ::= <struct_def> <struct_defs>
            @Struct_defs2 = 33,                        // <struct_defs> ::= 
            @Struct_def_Struct_Id_Lbrace_Rbrace = 34,  // <struct_def> ::= struct Id '{' <struct_parts> '}'
            @Struct_parts_Semi = 35,                   // <struct_parts> ::= <var_decl> ';' <struct_parts>
            @Struct_parts_Semi2 = 36,                  // <struct_parts> ::= <struct_decl> ';' <struct_parts>
            @Struct_parts = 37,                        // <struct_parts> ::= <func_decl> <struct_parts>
            @Struct_parts2 = 38,                       // <struct_parts> ::= 
            @Operator_Lt = 39,                         // <operator> ::= '<'
            @Operator_Gt = 40,                         // <operator> ::= '>'
            @Operator_Lteq = 41,                       // <operator> ::= '<='
            @Operator_Gteq = 42,                       // <operator> ::= '>='
            @Operator_And = 43,                        // <operator> ::= and
            @Operator_Or = 44,                         // <operator> ::= or
            @Operator_Eqeq = 45,                       // <operator> ::= '=='
            @Operator_Exclameq = 46,                   // <operator> ::= '!='
            @Operator_Times = 47,                      // <operator> ::= '*'
            @Operator_Div = 48,                        // <operator> ::= '/'
            @Operator_Mod = 49,                        // <operator> ::= mod
            @Operator_Plus = 50,                       // <operator> ::= '+'
            @Operator_Minus = 51,                      // <operator> ::= '-'
            @Assign_opr_Eq = 52,                       // <assign_opr> ::= '='
            @Assign_opr_Pluseq = 53,                   // <assign_opr> ::= '+='
            @Assign_opr_Minuseq = 54,                  // <assign_opr> ::= '-='
            @Type_Void = 55,                           // <type> ::= void
            @Type_String = 56,                         // <type> ::= string
            @Type_Num = 57,                            // <type> ::= num
            @Type_Bool = 58,                           // <type> ::= bool
            @Type_Id = 59,                             // <type> ::= Id
            @Value_Floatliteral = 60,                  // <value> ::= FloatLiteral
            @Value_Stringliteral = 61,                 // <value> ::= StringLiteral
            @Value_Booleanliteral = 62,                // <value> ::= BooleanLiteral
            @Value = 63,                               // <value> ::= <refrence>
            @Refrence = 64,                            // <refrence> ::= <func_call>
            @Refrence_Id = 65,                         // <refrence> ::= Id
            @Refrence_Id_Dot = 66,                     // <refrence> ::= Id '.' <refrence>
            @Refrence_Dot = 67,                        // <refrence> ::= <id_index> '.' <refrence>
            @Refrence2 = 68,                           // <refrence> ::= <id_index>
            @Var_refrence_Id_Dot = 69,                 // <var_refrence> ::= Id '.' <var_refrence_w_id>
            @Var_refrence_Dot = 70,                    // <var_refrence> ::= <id_index> '.' <var_refrence_w_id>
            @Var_refrence = 71,                        // <var_refrence> ::= <id_index>
            @Var_refrence_w_id_Id = 72,                // <var_refrence_w_id> ::= Id
            @Var_refrence_w_id_Id_Dot = 73,            // <var_refrence_w_id> ::= Id '.' <var_refrence_w_id>
            @Var_refrence_w_id_Dot = 74,               // <var_refrence_w_id> ::= <id_index> '.' <var_refrence_w_id>
            @Var_refrence_w_id = 75,                   // <var_refrence_w_id> ::= <id_index>
            @Func_refrence = 76,                       // <func_refrence> ::= <func_call>
            @Func_refrence_Id_Dot = 77,                // <func_refrence> ::= Id '.' <func_refrence>
            @Func_refrence_Dot = 78,                   // <func_refrence> ::= <id_index> '.' <func_refrence>
            @Id_index_Id = 79,                         // <id_index> ::= Id <index>
            @Index_Lbracket_Rbracket = 80,             // <index> ::= '[' <expr> ']' <index>
            @Index_Lbracket_Rbracket2 = 81,            // <index> ::= '[' <expr> ']'
            @Body = 82,                                // <body> ::= <bodypart> <body>
            @Body2 = 83,                               // <body> ::= 
            @Bodypart_Semi = 84,                       // <bodypart> ::= <var_decl> ';'
            @Bodypart_Semi2 = 85,                      // <bodypart> ::= <struct_decl> ';'
            @Bodypart_Semi3 = 86,                      // <bodypart> ::= <func_refrence> ';'
            @Bodypart = 87,                            // <bodypart> ::= <ctrl_stmt>
            @Bodypart_Return_Semi = 88,                // <bodypart> ::= return <expr> ';'
            @Expr = 89,                                // <expr> ::= <value> <operator> <expr>
            @Expr2 = 90,                               // <expr> ::= <value>
            @Expr_Lparen_Rparen = 91,                  // <expr> ::= '(' <expr> ')'
            @Expr_Lparen_Rparen2 = 92,                 // <expr> ::= '(' <expr> ')' <operator> <expr>
            @Expr_Exclam = 93,                         // <expr> ::= '!' <expr>
            @Expr_Minus = 94,                          // <expr> ::= '-' <expr>
            @Ctrl_stmt_If_Lbrace_Rbrace = 95,          // <ctrl_stmt> ::= if <expr> '{' <body> '}' <if_exp>
            @Ctrl_stmt_Repeat_Lbrace_Rbrace = 96,      // <ctrl_stmt> ::= repeat <var_decl> <direction> <expr> '{' <body> '}'
            @Ctrl_stmt_Repeat_Lparen_Rparen_Lbrace_Rbrace = 97,  // <ctrl_stmt> ::= repeat '(' <var_decl> <direction> <expr> ')' '{' <body> '}'
            @Ctrl_stmt_Repeat_Lbrace_Rbrace2 = 98,     // <ctrl_stmt> ::= repeat <expr> '{' <body> '}'
            @Direction_Downto = 99,                    // <direction> ::= downto
            @Direction_To = 100,                       // <direction> ::= to
            @If_exp_Else_If_Lbrace_Rbrace = 101,       // <if_exp> ::= else if <expr> '{' <body> '}' <if_exp>
            @If_exp_Else_Lbrace_Rbrace = 102,          // <if_exp> ::= else '{' <body> '}'
            @If_exp = 103,                             // <if_exp> ::= 
            @Func_call_Id_Lparen_Rparen = 104,         // <func_call> ::= Id '(' <expr_list> ')'
            @Expr_list = 105,                          // <expr_list> ::= <expr> <opt_exprs>
            @Expr_list2 = 106,                         // <expr_list> ::= <ref_id> <opt_exprs>
            @Expr_list3 = 107,                         // <expr_list> ::= 
            @Opt_exprs_Comma = 108,                    // <opt_exprs> ::= ',' <expr> <opt_exprs>
            @Opt_exprs_Comma2 = 109,                   // <opt_exprs> ::= ',' <ref_id> <opt_exprs>
            @Opt_exprs = 110,                          // <opt_exprs> ::= 
            @Ref_id_Ref_Id = 111                       // <ref_id> ::= ref Id
        }

        public enum SymbolIndex
        {
            @Eof = 0,                                  // (EOF)
            @Error = 1,                                // (Error)
            @Comment = 2,                              // Comment
            @Newline = 3,                              // NewLine
            @Whitespace = 4,                           // Whitespace
            @Timesdiv = 5,                             // '*/'
            @Divtimes = 6,                             // '/*'
            @Divdiv = 7,                               // '//'
            @Minus = 8,                                // '-'
            @Exclam = 9,                               // '!'
            @Exclameq = 10,                            // '!='
            @Lparen = 11,                              // '('
            @Rparen = 12,                              // ')'
            @Times = 13,                               // '*'
            @Comma = 14,                               // ','
            @Dot = 15,                                 // '.'
            @Div = 16,                                 // '/'
            @Semi = 17,                                // ';'
            @Lbracket = 18,                            // '['
            @Lbracketrbracket = 19,                    // '[]'
            @Rbracket = 20,                            // ']'
            @Lbrace = 21,                              // '{'
            @Rbrace = 22,                              // '}'
            @Plus = 23,                                // '+'
            @Pluseq = 24,                              // '+='
            @Lt = 25,                                  // '<'
            @Lteq = 26,                                // '<='
            @Eq = 27,                                  // '='
            @Minuseq = 28,                             // '-='
            @Eqeq = 29,                                // '=='
            @Gt = 30,                                  // '>'
            @Gteq = 31,                                // '>='
            @And = 32,                                 // and
            @Bool = 33,                                // bool
            @Booleanliteral = 34,                      // BooleanLiteral
            @Const = 35,                               // const
            @Downto = 36,                              // downto
            @Else = 37,                                // else
            @Floatliteral = 38,                        // FloatLiteral
            @Id = 39,                                  // Id
            @If = 40,                                  // if
            @Include = 41,                             // include
            @Mod = 42,                                 // mod
            @Num = 43,                                 // num
            @Or = 44,                                  // or
            @Program = 45,                             // program
            @Ref = 46,                                 // ref
            @Repeat = 47,                              // repeat
            @Return = 48,                              // return
            @String = 49,                              // string
            @Stringliteral = 50,                       // StringLiteral
            @Struct = 51,                              // struct
            @To = 52,                                  // to
            @Void = 53,                                // void
            @Assign_opr = 54,                          // <assign_opr>
            @Body = 55,                                // <body>
            @Bodypart = 56,                            // <bodypart>
            @Brackets = 57,                            // <brackets>
            @Const2 = 58,                              // <const>
            @Const_part = 59,                          // <const_part>
            @Consts = 60,                              // <consts>
            @Ctrl_stmt = 61,                           // <ctrl_stmt>
            @Direction = 62,                           // <direction>
            @Expr = 63,                                // <expr>
            @Expr_list = 64,                           // <expr_list>
            @Extra_typeid = 65,                        // <extra_typeid>
            @Func_call = 66,                           // <func_call>
            @Func_decl = 67,                           // <func_decl>
            @Func_decls = 68,                          // <Func_decls>
            @Func_refrence = 69,                       // <func_refrence>
            @Id_index = 70,                            // <id_index>
            @If_exp = 71,                              // <if_exp>
            @Include2 = 72,                            // <include>
            @Includes = 73,                            // <includes>
            @Index = 74,                               // <index>
            @List = 75,                                // <list>
            @Operator = 76,                            // <operator>
            @Opt_exprs = 77,                           // <opt_exprs>
            @Program2 = 78,                            // <Program>
            @Ref2 = 79,                                // <ref>
            @Ref_id = 80,                              // <ref_id>
            @Ref_typeid = 81,                          // <ref_typeid>
            @Refrence = 82,                            // <refrence>
            @Struct_decl = 83,                         // <struct_decl>
            @Struct_def = 84,                          // <struct_def>
            @Struct_defs = 85,                         // <struct_defs>
            @Struct_parts = 86,                        // <struct_parts>
            @Type = 87,                                // <type>
            @Typeid = 88,                              // <typeid>
            @Typeid_list = 89,                         // <typeid_list>
            @Value = 90,                               // <value>
            @Var_decl = 91,                            // <var_decl>
            @Var_decls = 92,                           // <var_decls>
            @Var_refrence = 93,                        // <var_refrence>
            @Var_refrence_w_id = 94                    // <var_refrence_w_id>
        }
    }
}