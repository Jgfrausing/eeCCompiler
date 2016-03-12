namespace eeCCompiler.Indexes
{
    public class Indexes
    {
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
            @Div = 15,                                 // '/'
            @Semi = 16,                                // ';'
            @Lbrace = 17,                              // '{'
            @Rbrace = 18,                              // '}'
            @Plus = 19,                                // '+'
            @Lt = 20,                                  // '<'
            @Lteq = 21,                                // '<='
            @Eq = 22,                                  // '='
            @Eqeq = 23,                                // '=='
            @Gt = 24,                                  // '>'
            @Gteq = 25,                                // '>='
            @And = 26,                                 // and
            @Bool = 27,                                // bool
            @Booleanliteral = 28,                      // BooleanLiteral
            @Const = 29,                               // const
            @Downto = 30,                              // downto
            @Else = 31,                                // else
            @Floatliteral = 32,                        // FloatLiteral
            @Id = 33,                                  // Id
            @If = 34,                                  // if
            @Mod = 35,                                 // mod
            @Num = 36,                                 // num
            @Or = 37,                                  // or
            @Program = 38,                             // program
            @Repeat = 39,                              // repeat
            @String = 40,                              // string
            @Stringliteral = 41,                       // StringLiteral
            @Struct = 42,                              // struct
            @To = 43,                                  // to
            @Void = 44,                                // void
            @Body = 45,                                // <body>
            @Bodypart = 46,                            // <bodypart>
            @Const2 = 47,                              // <const>
            @Const_part = 48,                          // <const_part>
            @Consts = 49,                              // <consts>
            @Ctrl_stmt = 50,                           // <ctrl_stmt>
            @Direction = 51,                           // <direction>
            @Expr = 52,                                // <expr>
            @Expr_list = 53,                           // <expr_list>
            @Extra_typeid = 54,                        // <extra_typeid>
            @Func_call = 55,                           // <func_call>
            @Func_decl = 56,                           // <func_decl>
            @Func_decls = 57,                          // <Func_decls>
            @If_exp = 58,                              // <if_exp>
            @Operator = 59,                            // <operator>
            @Opt_exprs = 60,                           // <opt_exprs>
            @Program2 = 61,                            // <Program>
            @Struct_decl = 62,                         // <struct_decl>
            @Struct_def = 63,                          // <struct_def>
            @Struct_defs = 64,                         // <struct_defs>
            @Struct_parts = 65,                        // <struct_parts>
            @Type = 66,                                // <type>
            @Typeid = 67,                              // <typeid>
            @Typeid_list = 68,                         // <typeid_list>
            @Value = 69,                               // <value>
            @Var_decl = 70,                            // <var_decl>
            @Var_decls = 71                            // <var_decls>
        }

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
            @Var_decls = 15,                           // <var_decls> ::= <var_decl> <var_decls>
            @Var_decls2 = 16,                          // <var_decls> ::= 
            @Var_decl_Id_Eq_Semi = 17,                 // <var_decl> ::= Id '=' <expr> ';'
            @Struct_decl_Id_Eq_Id_Lbrace_Rbrace_Semi = 18,  // <struct_decl> ::= Id '=' Id '{' <var_decls> '}' ';'
            @Struct_defs = 19,                         // <struct_defs> ::= <struct_def> <struct_defs>
            @Struct_defs2 = 20,                        // <struct_defs> ::= 
            @Struct_def_Struct_Id_Lbrace_Rbrace = 21,  // <struct_def> ::= struct Id '{' <struct_parts> '}'
            @Struct_parts = 22,                        // <struct_parts> ::= <var_decl> <struct_parts>
            @Struct_parts2 = 23,                       // <struct_parts> ::= <func_decl> <struct_parts>
            @Struct_parts3 = 24,                       // <struct_parts> ::= 
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
            @Type_Void = 38,                           // <type> ::= void
            @Type_String = 39,                         // <type> ::= string
            @Type_Num = 40,                            // <type> ::= num
            @Type_Bool = 41,                           // <type> ::= bool
            @Value_Floatliteral = 42,                  // <value> ::= FloatLiteral
            @Value_Stringliteral = 43,                 // <value> ::= StringLiteral
            @Value_Booleanliteral = 44,                // <value> ::= BooleanLiteral
            @Value = 45,                               // <value> ::= <func_call>
            @Value_Id = 46,                            // <value> ::= Id
            @Body = 47,                                // <body> ::= <bodypart> <body>
            @Body2 = 48,                               // <body> ::= 
            @Bodypart = 49,                            // <bodypart> ::= <var_decl>
            @Bodypart2 = 50,                           // <bodypart> ::= <struct_decl>
            @Bodypart_Semi = 51,                       // <bodypart> ::= <func_call> ';'
            @Bodypart3 = 52,                           // <bodypart> ::= <ctrl_stmt>
            @Expr = 53,                                // <expr> ::= <value> <operator> <expr>
            @Expr2 = 54,                               // <expr> ::= <value>
            @Expr_Lparen_Rparen = 55,                  // <expr> ::= '(' <expr> ')'
            @Expr_Minus = 56,                          // <expr> ::= '-' <value>
            @Expr_Lparen_Rparen2 = 57,                 // <expr> ::= '(' <expr> ')' <operator> <expr>
            @Expr_Exclam = 58,                         // <expr> ::= '!' <expr>
            @Ctrl_stmt_If_Lbrace_Rbrace = 59,          // <ctrl_stmt> ::= if <expr> '{' <body> '}' <if_exp>
            @Ctrl_stmt_Repeat_Lbrace_Rbrace = 60,      // <ctrl_stmt> ::= repeat <var_decl> <direction> <expr> '{' <body> '}'
            @Ctrl_stmt_Repeat_Lparen_Rparen_Lbrace_Rbrace = 61,  // <ctrl_stmt> ::= repeat '(' <var_decl> <direction> <expr> ')' '{' <body> '}'
            @Ctrl_stmt_Repeat_Lbrace_Rbrace2 = 62,     // <ctrl_stmt> ::= repeat <expr> '{' <body> '}'
            @Direction_Downto = 63,                    // <direction> ::= downto
            @Direction_To = 64,                        // <direction> ::= to
            @If_exp_Else_If_Lbrace_Rbrace = 65,        // <if_exp> ::= else if <expr> '{' <body> '}' <if_exp>
            @If_exp_Else_Lbrace_Rbrace = 66,           // <if_exp> ::= else '{' <body> '}'
            @If_exp = 67,                              // <if_exp> ::= 
            @Func_call_Id_Lparen_Rparen = 68,          // <func_call> ::= Id '(' <expr_list> ')'
            @Expr_list = 69,                           // <expr_list> ::= <expr> <opt_exprs>
            @Expr_list2 = 70,                          // <expr_list> ::= 
            @Opt_exprs_Comma = 71,                     // <opt_exprs> ::= ',' <expr> <opt_exprs>
            @Opt_exprs = 72                            // <opt_exprs> ::= 
        }
    }
}