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
            @Typeid_list = 14,                         // <typeid_list> ::= <ref> <typeid> <extra_typeid>
            @Typeid_list2 = 15,                        // <typeid_list> ::= 
            @Extra_typeid_Comma = 16,                  // <extra_typeid> ::= ',' <ref> <typeid> <extra_typeid>
            @Extra_typeid = 17,                        // <extra_typeid> ::= 
            @Ref_Ref = 18,                             // <ref> ::= ref
            @Ref = 19,                                 // <ref> ::= 
            @Typeid_Id = 20,                           // <typeid> ::= <type> Id
            @Var_decls_Semi = 21,                      // <var_decls> ::= <var_decl> ';' <var_decls>
            @Var_decls = 22,                           // <var_decls> ::= 
            @Var_decl_Id = 23,                         // <var_decl> ::= Id <assign_opr> <expr>
            @Struct_decl_Id_Id_Lbrace_Rbrace = 24,     // <struct_decl> ::= Id <assign_opr> Id '{' <var_decls> '}'
            @Struct_defs = 25,                         // <struct_defs> ::= <struct_def> <struct_defs>
            @Struct_defs2 = 26,                        // <struct_defs> ::= 
            @Struct_def_Struct_Id_Lbrace_Rbrace = 27,  // <struct_def> ::= struct Id '{' <struct_parts> '}'
            @Struct_parts_Semi = 28,                   // <struct_parts> ::= <var_decl> ';' <struct_parts>
            @Struct_parts = 29,                        // <struct_parts> ::= <func_decl> <struct_parts>
            @Struct_parts2 = 30,                       // <struct_parts> ::= 
            @Operator_Lt = 31,                         // <operator> ::= '<'
            @Operator_Gt = 32,                         // <operator> ::= '>'
            @Operator_Lteq = 33,                       // <operator> ::= '<='
            @Operator_Gteq = 34,                       // <operator> ::= '>='
            @Operator_And = 35,                        // <operator> ::= and
            @Operator_Or = 36,                         // <operator> ::= or
            @Operator_Eqeq = 37,                       // <operator> ::= '=='
            @Operator_Exclameq = 38,                   // <operator> ::= '!='
            @Operator_Times = 39,                      // <operator> ::= '*'
            @Operator_Div = 40,                        // <operator> ::= '/'
            @Operator_Mod = 41,                        // <operator> ::= mod
            @Operator_Plus = 42,                       // <operator> ::= '+'
            @Operator_Minus = 43,                      // <operator> ::= '-'
            @Assign_opr_Eq = 44,                       // <assign_opr> ::= '='
            @Assign_opr_Pluseq = 45,                   // <assign_opr> ::= '+='
            @Assign_opr_Minuseq = 46,                  // <assign_opr> ::= '-='
            @Type_Void = 47,                           // <type> ::= void
            @Type_String = 48,                         // <type> ::= string
            @Type_Num = 49,                            // <type> ::= num
            @Type_Bool = 50,                           // <type> ::= bool
            @Type_Id = 51,                             // <type> ::= Id
            @Value_Floatliteral = 52,                  // <value> ::= FloatLiteral
            @Value_Stringliteral = 53,                 // <value> ::= StringLiteral
            @Value_Booleanliteral = 54,                // <value> ::= BooleanLiteral
            @Value = 55,                               // <value> ::= <refrence>
            @Refrence = 56,                            // <refrence> ::= <func_call>
            @Refrence_Id = 57,                         // <refrence> ::= Id
            @Refrence_Id_Dot = 58,                     // <refrence> ::= Id '.' <refrence>
            @Refrence_Id_Dot2 = 59,                    // <refrence> ::= Id <index> '.' <refrence>
            @Refrence_Id2 = 60,                        // <refrence> ::= Id <index>
            @Index_Lbracket_Rbracket = 61,             // <index> ::= '[' <value> ']' <index>
            @Index_Lbracket_Rbracket2 = 62,            // <index> ::= '[' <value> ']'
            @Body = 63,                                // <body> ::= <bodypart> <body>
            @Body2 = 64,                               // <body> ::= 
            @Bodypart_Semi = 65,                       // <bodypart> ::= <var_decl> ';'
            @Bodypart_Semi2 = 66,                      // <bodypart> ::= <struct_decl> ';'
            @Bodypart_Semi3 = 67,                      // <bodypart> ::= <func_call> ';'
            @Bodypart = 68,                            // <bodypart> ::= <ctrl_stmt>
            @Bodypart_Return_Semi = 69,                // <bodypart> ::= return <expr> ';'
            @Expr = 70,                                // <expr> ::= <value> <operator> <expr>
            @Expr2 = 71,                               // <expr> ::= <value>
            @Expr_Lparen_Rparen = 72,                  // <expr> ::= '(' <expr> ')'
            @Expr_Lparen_Rparen2 = 73,                 // <expr> ::= '(' <expr> ')' <operator> <expr>
            @Expr_Exclam = 74,                         // <expr> ::= '!' <expr>
            @Expr_Minus = 75,                          // <expr> ::= '-' <expr>
            @Ctrl_stmt_If_Lbrace_Rbrace = 76,          // <ctrl_stmt> ::= if <expr> '{' <body> '}' <if_exp>
            @Ctrl_stmt_Repeat_Lbrace_Rbrace = 77,      // <ctrl_stmt> ::= repeat <var_decl> <direction> <expr> '{' <body> '}'
            @Ctrl_stmt_Repeat_Lparen_Rparen_Lbrace_Rbrace = 78,  // <ctrl_stmt> ::= repeat '(' <var_decl> <direction> <expr> ')' '{' <body> '}'
            @Ctrl_stmt_Repeat_Lbrace_Rbrace2 = 79,     // <ctrl_stmt> ::= repeat <expr> '{' <body> '}'
            @Direction_Downto = 80,                    // <direction> ::= downto
            @Direction_To = 81,                        // <direction> ::= to
            @If_exp_Else_If_Lbrace_Rbrace = 82,        // <if_exp> ::= else if <expr> '{' <body> '}' <if_exp>
            @If_exp_Else_Lbrace_Rbrace = 83,           // <if_exp> ::= else '{' <body> '}'
            @If_exp = 84,                              // <if_exp> ::= 
            @Func_call_Id_Lparen_Rparen = 85,          // <func_call> ::= Id '(' <expr_list> ')'
            @Expr_list = 86,                           // <expr_list> ::= <ref> <expr> <opt_exprs>
            @Expr_list2 = 87,                          // <expr_list> ::= 
            @Opt_exprs_Comma = 88,                     // <opt_exprs> ::= ',' <ref> <expr> <opt_exprs>
            @Opt_exprs = 89                            // <opt_exprs> ::=
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
            @Rbracket = 19,                            // ']'
            @Lbrace = 20,                              // '{'
            @Rbrace = 21,                              // '}'
            @Plus = 22,                                // '+'
            @Pluseq = 23,                              // '+='
            @Lt = 24,                                  // '<'
            @Lteq = 25,                                // '<='
            @Eq = 26,                                  // '='
            @Minuseq = 27,                             // '-='
            @Eqeq = 28,                                // '=='
            @Gt = 29,                                  // '>'
            @Gteq = 30,                                // '>='
            @And = 31,                                 // and
            @Bool = 32,                                // bool
            @Booleanliteral = 33,                      // BooleanLiteral
            @Const = 34,                               // const
            @Downto = 35,                              // downto
            @Else = 36,                                // else
            @Floatliteral = 37,                        // FloatLiteral
            @Id = 38,                                  // Id
            @If = 39,                                  // if
            @Include = 40,                             // include
            @Mod = 41,                                 // mod
            @Num = 42,                                 // num
            @Or = 43,                                  // or
            @Program = 44,                             // program
            @Ref = 45,                                 // ref
            @Repeat = 46,                              // repeat
            @Return = 47,                              // return
            @String = 48,                              // string
            @Stringliteral = 49,                       // StringLiteral
            @Struct = 50,                              // struct
            @To = 51,                                  // to
            @Void = 52,                                // void
            @Assign_opr = 53,                          // <assign_opr>
            @Body = 54,                                // <body>
            @Bodypart = 55,                            // <bodypart>
            @Const2 = 56,                              // <const>
            @Const_part = 57,                          // <const_part>
            @Consts = 58,                              // <consts>
            @Ctrl_stmt = 59,                           // <ctrl_stmt>
            @Direction = 60,                           // <direction>
            @Expr = 61,                                // <expr>
            @Expr_list = 62,                           // <expr_list>
            @Extra_typeid = 63,                        // <extra_typeid>
            @Func_call = 64,                           // <func_call>
            @Func_decl = 65,                           // <func_decl>
            @Func_decls = 66,                          // <Func_decls>
            @If_exp = 67,                              // <if_exp>
            @Include2 = 68,                            // <include>
            @Includes = 69,                            // <includes>
            @Index = 70,                               // <index>
            @Operator = 71,                            // <operator>
            @Opt_exprs = 72,                           // <opt_exprs>
            @Program2 = 73,                            // <Program>
            @Ref2 = 74,                                // <ref>
            @Refrence = 75,                            // <refrence>
            @Struct_decl = 76,                         // <struct_decl>
            @Struct_def = 77,                          // <struct_def>
            @Struct_defs = 78,                         // <struct_defs>
            @Struct_parts = 79,                        // <struct_parts>
            @Type = 80,                                // <type>
            @Typeid = 81,                              // <typeid>
            @Typeid_list = 82,                         // <typeid_list>
            @Value = 83,                               // <value>
            @Var_decl = 84,                            // <var_decl>
            @Var_decls = 85                            // <var_decls>
        }
    }
}