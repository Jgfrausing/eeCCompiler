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
            @List = 29,                                // <list> ::= <type> <brackets>
            @Struct_decl_Id_Id_Lbrace_Rbrace = 30,     // <struct_decl> ::= Id <assign_opr> Id '{' <var_decls> '}'
            @Struct_defs = 31,                         // <struct_defs> ::= <struct_def> <struct_defs>
            @Struct_defs2 = 32,                        // <struct_defs> ::= 
            @Struct_def_Struct_Id_Lbrace_Rbrace = 33,  // <struct_def> ::= struct Id '{' <struct_parts> '}'
            @Struct_parts_Semi = 34,                   // <struct_parts> ::= <var_decl> ';' <struct_parts>
            @Struct_parts = 35,                        // <struct_parts> ::= <func_decl> <struct_parts>
            @Struct_parts2 = 36,                       // <struct_parts> ::= 
            @Operator_Lt = 37,                         // <operator> ::= '<'
            @Operator_Gt = 38,                         // <operator> ::= '>'
            @Operator_Lteq = 39,                       // <operator> ::= '<='
            @Operator_Gteq = 40,                       // <operator> ::= '>='
            @Operator_And = 41,                        // <operator> ::= and
            @Operator_Or = 42,                         // <operator> ::= or
            @Operator_Eqeq = 43,                       // <operator> ::= '=='
            @Operator_Exclameq = 44,                   // <operator> ::= '!='
            @Operator_Times = 45,                      // <operator> ::= '*'
            @Operator_Div = 46,                        // <operator> ::= '/'
            @Operator_Mod = 47,                        // <operator> ::= mod
            @Operator_Plus = 48,                       // <operator> ::= '+'
            @Operator_Minus = 49,                      // <operator> ::= '-'
            @Assign_opr_Eq = 50,                       // <assign_opr> ::= '='
            @Assign_opr_Pluseq = 51,                   // <assign_opr> ::= '+='
            @Assign_opr_Minuseq = 52,                  // <assign_opr> ::= '-='
            @Type_Void = 53,                           // <type> ::= void
            @Type_String = 54,                         // <type> ::= string
            @Type_Num = 55,                            // <type> ::= num
            @Type_Bool = 56,                           // <type> ::= bool
            @Type_Id = 57,                             // <type> ::= Id
            @Value_Floatliteral = 58,                  // <value> ::= FloatLiteral
            @Value_Stringliteral = 59,                 // <value> ::= StringLiteral
            @Value_Booleanliteral = 60,                // <value> ::= BooleanLiteral
            @Value = 61,                               // <value> ::= <refrence>
            @Refrence = 62,                            // <refrence> ::= <func_call>
            @Refrence_Id = 63,                         // <refrence> ::= Id
            @Refrence_Id_Dot = 64,                     // <refrence> ::= Id '.' <refrence>
            @Refrence_Id_Dot2 = 65,                    // <refrence> ::= Id <index> '.' <refrence>
            @Refrence_Id2 = 66,                        // <refrence> ::= Id <index>
            @Index_Lbracket_Rbracket = 67,             // <index> ::= '[' <expr> ']' <index>
            @Index_Lbracket_Rbracket2 = 68,            // <index> ::= '[' <expr> ']'
            @Body = 69,                                // <body> ::= <bodypart> <body>
            @Body2 = 70,                               // <body> ::= 
            @Bodypart_Semi = 71,                       // <bodypart> ::= <var_decl> ';'
            @Bodypart_Semi2 = 72,                      // <bodypart> ::= <struct_decl> ';'
            @Bodypart_Semi3 = 73,                      // <bodypart> ::= <func_call> ';'
            @Bodypart = 74,                            // <bodypart> ::= <ctrl_stmt>
            @Bodypart_Return_Semi = 75,                // <bodypart> ::= return <expr> ';'
            @Expr = 76,                                // <expr> ::= <value> <operator> <expr>
            @Expr2 = 77,                               // <expr> ::= <value>
            @Expr_Lparen_Rparen = 78,                  // <expr> ::= '(' <expr> ')'
            @Expr_Lparen_Rparen2 = 79,                 // <expr> ::= '(' <expr> ')' <operator> <expr>
            @Expr_Exclam = 80,                         // <expr> ::= '!' <expr>
            @Expr_Minus = 81,                          // <expr> ::= '-' <expr>
            @Ctrl_stmt_If_Lbrace_Rbrace = 82,          // <ctrl_stmt> ::= if <expr> '{' <body> '}' <if_exp>
            @Ctrl_stmt_Repeat_Lbrace_Rbrace = 83,      // <ctrl_stmt> ::= repeat <var_decl> <direction> <expr> '{' <body> '}'
            @Ctrl_stmt_Repeat_Lparen_Rparen_Lbrace_Rbrace = 84,  // <ctrl_stmt> ::= repeat '(' <var_decl> <direction> <expr> ')' '{' <body> '}'
            @Ctrl_stmt_Repeat_Lbrace_Rbrace2 = 85,     // <ctrl_stmt> ::= repeat <expr> '{' <body> '}'
            @Direction_Downto = 86,                    // <direction> ::= downto
            @Direction_To = 87,                        // <direction> ::= to
            @If_exp_Else_If_Lbrace_Rbrace = 88,        // <if_exp> ::= else if <expr> '{' <body> '}' <if_exp>
            @If_exp_Else_Lbrace_Rbrace = 89,           // <if_exp> ::= else '{' <body> '}'
            @If_exp = 90,                              // <if_exp> ::= 
            @Func_call_Id_Lparen_Rparen = 91,          // <func_call> ::= Id '(' <expr_list> ')'
            @Expr_list = 92,                           // <expr_list> ::= <expr> <opt_exprs>
            @Expr_list2 = 93,                          // <expr_list> ::= <ref_id> <opt_exprs>
            @Expr_list3 = 94,                          // <expr_list> ::= 
            @Opt_exprs_Comma = 95,                     // <opt_exprs> ::= ',' <expr> <opt_exprs>
            @Opt_exprs_Comma2 = 96,                    // <opt_exprs> ::= ',' <ref_id> <opt_exprs>
            @Opt_exprs = 97,                           // <opt_exprs> ::= 
            @Ref_id_Ref_Id = 98                        // <ref_id> ::= ref Id
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
            @If_exp = 69,                              // <if_exp>
            @Include2 = 70,                            // <include>
            @Includes = 71,                            // <includes>
            @Index = 72,                               // <index>
            @List = 73,                                // <list>
            @Operator = 74,                            // <operator>
            @Opt_exprs = 75,                           // <opt_exprs>
            @Program2 = 76,                            // <Program>
            @Ref2 = 77,                                // <ref>
            @Ref_id = 78,                              // <ref_id>
            @Ref_typeid = 79,                          // <ref_typeid>
            @Refrence = 80,                            // <refrence>
            @Struct_decl = 81,                         // <struct_decl>
            @Struct_def = 82,                          // <struct_def>
            @Struct_defs = 83,                         // <struct_defs>
            @Struct_parts = 84,                        // <struct_parts>
            @Type = 85,                                // <type>
            @Typeid = 86,                              // <typeid>
            @Typeid_list = 87,                         // <typeid_list>
            @Value = 88,                               // <value>
            @Var_decl = 89,                            // <var_decl>
            @Var_decls = 90                            // <var_decls>
        }
    }
}