namespace eeCCompiler.Indexes
{
    public class Indexes
    {
        public enum ProductionIndex
        {
            @Program_Program_Lbrace_Rbrace = 0,        // <Program> ::= <consts> <struct_defs> program '{' <body> '}' <Func_decls>
            @Consts = 1,                               // <consts> ::= <const> <consts>
            @Consts2 = 2,                              // <consts> ::= 
            @Const_Const_Id_Semi = 3,                  // <const> ::= const Id <const_part> ';'
            @Const_part_Floatliteral = 4,              // <const_part> ::= FloatLiteral
            @Const_part_Stringliteral = 5,             // <const_part> ::= StringLiteral
            @Const_part_Booleanliteral = 6,            // <const_part> ::= BooleanLiteral
            @Func_decls = 7,                           // <Func_decls> ::= <func_decl> <Func_decls>
            @Func_decls2 = 8,                          // <Func_decls> ::= 
            @Func_decl_Lparen_Rparen_Lbrace_Rbrace = 9,  // <func_decl> ::= <typeid> '(' <typeid_list> ')' '{' <body> '}'
            @Typeid_list = 10,                         // <typeid_list> ::= <typeid> <extra_typeid>
            @Typeid_list2 = 11,                        // <typeid_list> ::= 
            @Extra_typeid_Comma = 12,                  // <extra_typeid> ::= ',' <typeid> <extra_typeid>
            @Extra_typeid = 13,                        // <extra_typeid> ::= 
            @Typeid_Id = 14,                           // <typeid> ::= <type> Id
            @Var_decls_Semi = 15,                      // <var_decls> ::= <var_decl> ';' <var_decls>
            @Var_decls = 16,                           // <var_decls> ::= 
            @Var_decl_Id_Eq = 17,                      // <var_decl> ::= Id '=' <expr>
            @Struct_decl_Id_Eq_Id_Lbrace_Rbrace = 18,  // <struct_decl> ::= Id '=' Id '{' <var_decls> '}'
            @Struct_defs = 19,                         // <struct_defs> ::= <struct_def> <struct_defs>
            @Struct_defs2 = 20,                        // <struct_defs> ::= 
            @Struct_def_Struct_Id_Lbrace_Rbrace = 21,  // <struct_def> ::= struct Id '{' <struct_parts> '}'
            @Struct_parts_Semi = 22,                   // <struct_parts> ::= <var_decl> ';' <struct_parts>
            @Struct_parts = 23,                        // <struct_parts> ::= <func_decl> <struct_parts>
            @Struct_parts2 = 24,                       // <struct_parts> ::= 
            @Operator_Lt = 25,                         // <operator> ::= '<'
            @Operator_Gt = 26,                         // <operator> ::= '>'
            @Operator_Lteq = 27,                       // <operator> ::= '<='
            @Operator_Gteq = 28,                       // <operator> ::= '>='
            @Operator_And = 29,                        // <operator> ::= and
            @Operator_Or = 30,                         // <operator> ::= or
            @Operator_Eqeq = 31,                       // <operator> ::= '=='
            @Operator_Exclameq = 32,                   // <operator> ::= '!='
            @Operator_Times = 33,                      // <operator> ::= '*'
            @Operator_Div = 34,                        // <operator> ::= '/'
            @Operator_Mod = 35,                        // <operator> ::= mod
            @Operator_Plus = 36,                       // <operator> ::= '+'
            @Operator_Minus = 37,                      // <operator> ::= '-'
            @Operator_Pluseq = 38,                     // <operator> ::= '+='
            @Operator_Minuseq = 39,                    // <operator> ::= '-='
            @Type_Void = 40,                           // <type> ::= void
            @Type_String = 41,                         // <type> ::= string
            @Type_Num = 42,                            // <type> ::= num
            @Type_Bool = 43,                           // <type> ::= bool
            @Value_Floatliteral = 44,                  // <value> ::= FloatLiteral
            @Value_Stringliteral = 45,                 // <value> ::= StringLiteral
            @Value_Booleanliteral = 46,                // <value> ::= BooleanLiteral
            @Value = 47,                               // <value> ::= <refrence>
            @Refrence = 48,                            // <refrence> ::= <func_call>
            @Refrence_Id = 49,                         // <refrence> ::= Id
            @Refrence_Id_Dot = 50,                     // <refrence> ::= Id '.' <refrence>
            @Body = 51,                                // <body> ::= <bodypart> <body>
            @Body2 = 52,                               // <body> ::= 
            @Bodypart_Semi = 53,                       // <bodypart> ::= <var_decl> ';'
            @Bodypart_Semi2 = 54,                      // <bodypart> ::= <struct_decl> ';'
            @Bodypart_Semi3 = 55,                      // <bodypart> ::= <func_call> ';'
            @Bodypart = 56,                            // <bodypart> ::= <ctrl_stmt>
            @Bodypart_Return_Semi = 57,                // <bodypart> ::= return <expr> ';'
            @Expr = 58,                                // <expr> ::= <value> <operator> <expr>
            @Expr2 = 59,                               // <expr> ::= <value>
            @Expr_Lparen_Rparen = 60,                  // <expr> ::= '(' <expr> ')'
            @Expr_Lparen_Rparen2 = 61,                 // <expr> ::= '(' <expr> ')' <operator> <expr>
            @Expr_Exclam = 62,                         // <expr> ::= '!' <expr>
            @Expr_Minus = 63,                          // <expr> ::= '-' <value>
            @Ctrl_stmt_If_Lbrace_Rbrace = 64,          // <ctrl_stmt> ::= if <expr> '{' <body> '}' <if_exp>
            @Ctrl_stmt_Repeat_Lbrace_Rbrace = 65,      // <ctrl_stmt> ::= repeat <var_decl> <direction> <expr> '{' <body> '}'
            @Ctrl_stmt_Repeat_Lparen_Rparen_Lbrace_Rbrace = 66,  // <ctrl_stmt> ::= repeat '(' <var_decl> <direction> <expr> ')' '{' <body> '}'
            @Ctrl_stmt_Repeat_Lbrace_Rbrace2 = 67,     // <ctrl_stmt> ::= repeat <expr> '{' <body> '}'
            @Direction_Downto = 68,                    // <direction> ::= downto
            @Direction_To = 69,                        // <direction> ::= to
            @If_exp_Else_If_Lbrace_Rbrace = 70,        // <if_exp> ::= else if <expr> '{' <body> '}' <if_exp>
            @If_exp_Else_Lbrace_Rbrace = 71,           // <if_exp> ::= else '{' <body> '}'
            @If_exp = 72,                              // <if_exp> ::= 
            @Func_call_Id_Lparen_Rparen = 73,          // <func_call> ::= Id '(' <expr_list> ')'
            @Expr_list = 74,                           // <expr_list> ::= <expr> <opt_exprs>
            @Expr_list2 = 75,                          // <expr_list> ::= 
            @Opt_exprs_Comma = 76,                     // <opt_exprs> ::= ',' <expr> <opt_exprs>
            @Opt_exprs = 77                            // <opt_exprs> ::= 
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
            @Lbrace = 18,                              // '{'
            @Rbrace = 19,                              // '}'
            @Plus = 20,                                // '+'
            @Pluseq = 21,                              // '+='
            @Lt = 22,                                  // '<'
            @Lteq = 23,                                // '<='
            @Eq = 24,                                  // '='
            @Minuseq = 25,                             // '-='
            @Eqeq = 26,                                // '=='
            @Gt = 27,                                  // '>'
            @Gteq = 28,                                // '>='
            @And = 29,                                 // and
            @Bool = 30,                                // bool
            @Booleanliteral = 31,                      // BooleanLiteral
            @Const = 32,                               // const
            @Downto = 33,                              // downto
            @Else = 34,                                // else
            @Floatliteral = 35,                        // FloatLiteral
            @Id = 36,                                  // Id
            @If = 37,                                  // if
            @Mod = 38,                                 // mod
            @Num = 39,                                 // num
            @Or = 40,                                  // or
            @Program = 41,                             // program
            @Repeat = 42,                              // repeat
            @Return = 43,                              // return
            @String = 44,                              // string
            @Stringliteral = 45,                       // StringLiteral
            @Struct = 46,                              // struct
            @To = 47,                                  // to
            @Void = 48,                                // void
            @Body = 49,                                // <body>
            @Bodypart = 50,                            // <bodypart>
            @Const2 = 51,                              // <const>
            @Const_part = 52,                          // <const_part>
            @Consts = 53,                              // <consts>
            @Ctrl_stmt = 54,                           // <ctrl_stmt>
            @Direction = 55,                           // <direction>
            @Expr = 56,                                // <expr>
            @Expr_list = 57,                           // <expr_list>
            @Extra_typeid = 58,                        // <extra_typeid>
            @Func_call = 59,                           // <func_call>
            @Func_decl = 60,                           // <func_decl>
            @Func_decls = 61,                          // <Func_decls>
            @If_exp = 62,                              // <if_exp>
            @Operator = 63,                            // <operator>
            @Opt_exprs = 64,                           // <opt_exprs>
            @Program2 = 65,                            // <Program>
            @Refrence = 66,                            // <refrence>
            @Struct_decl = 67,                         // <struct_decl>
            @Struct_def = 68,                          // <struct_def>
            @Struct_defs = 69,                         // <struct_defs>
            @Struct_parts = 70,                        // <struct_parts>
            @Type = 71,                                // <type>
            @Typeid = 72,                              // <typeid>
            @Typeid_list = 73,                         // <typeid_list>
            @Value = 74,                               // <value>
            @Var_decl = 75,                            // <var_decl>
            @Var_decls = 76                            // <var_decls>
        }
    }
}