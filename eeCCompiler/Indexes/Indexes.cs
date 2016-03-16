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
            @Typeid_list = 14,                         // <typeid_list> ::= <typeid> <extra_typeid>
            @Typeid_list2 = 15,                        // <typeid_list> ::= 
            @Extra_typeid_Comma = 16,                  // <extra_typeid> ::= ',' <typeid> <extra_typeid>
            @Extra_typeid = 17,                        // <extra_typeid> ::= 
            @Typeid_Id = 18,                           // <typeid> ::= <type> Id
            @Var_decls_Semi = 19,                      // <var_decls> ::= <var_decl> ';' <var_decls>
            @Var_decls = 20,                           // <var_decls> ::= 
            @Var_decl_Id = 21,                         // <var_decl> ::= Id <assign_opr> <expr>
            @Struct_decl_Id_Id_Lbrace_Rbrace = 22,     // <struct_decl> ::= Id <assign_opr> Id '{' <var_decls> '}'
            @Struct_defs = 23,                         // <struct_defs> ::= <struct_def> <struct_defs>
            @Struct_defs2 = 24,                        // <struct_defs> ::= 
            @Struct_def_Struct_Id_Lbrace_Rbrace = 25,  // <struct_def> ::= struct Id '{' <struct_parts> '}'
            @Struct_parts_Semi = 26,                   // <struct_parts> ::= <var_decl> ';' <struct_parts>
            @Struct_parts = 27,                        // <struct_parts> ::= <func_decl> <struct_parts>
            @Struct_parts2 = 28,                       // <struct_parts> ::= 
            @Operator_Lt = 29,                         // <operator> ::= '<'
            @Operator_Gt = 30,                         // <operator> ::= '>'
            @Operator_Lteq = 31,                       // <operator> ::= '<='
            @Operator_Gteq = 32,                       // <operator> ::= '>='
            @Operator_And = 33,                        // <operator> ::= and
            @Operator_Or = 34,                         // <operator> ::= or
            @Operator_Eqeq = 35,                       // <operator> ::= '=='
            @Operator_Exclameq = 36,                   // <operator> ::= '!='
            @Operator_Times = 37,                      // <operator> ::= '*'
            @Operator_Div = 38,                        // <operator> ::= '/'
            @Operator_Mod = 39,                        // <operator> ::= mod
            @Operator_Plus = 40,                       // <operator> ::= '+'
            @Operator_Minus = 41,                      // <operator> ::= '-'
            @Assign_opr_Eq = 42,                       // <assign_opr> ::= '='
            @Assign_opr_Pluseq = 43,                   // <assign_opr> ::= '+='
            @Assign_opr_Minuseq = 44,                  // <assign_opr> ::= '-='
            @Type_Void = 45,                           // <type> ::= void
            @Type_String = 46,                         // <type> ::= string
            @Type_Num = 47,                            // <type> ::= num
            @Type_Bool = 48,                           // <type> ::= bool
            @Value_Floatliteral = 49,                  // <value> ::= FloatLiteral
            @Value_Stringliteral = 50,                 // <value> ::= StringLiteral
            @Value_Booleanliteral = 51,                // <value> ::= BooleanLiteral
            @Value = 52,                               // <value> ::= <refrence>
            @Refrence = 53,                            // <refrence> ::= <func_call>
            @Refrence_Id = 54,                         // <refrence> ::= Id
            @Refrence_Id_Dot = 55,                     // <refrence> ::= Id '.' <refrence>
            @Body = 56,                                // <body> ::= <bodypart> <body>
            @Body2 = 57,                               // <body> ::= 
            @Bodypart_Semi = 58,                       // <bodypart> ::= <var_decl> ';'
            @Bodypart_Semi2 = 59,                      // <bodypart> ::= <struct_decl> ';'
            @Bodypart_Semi3 = 60,                      // <bodypart> ::= <func_call> ';'
            @Bodypart = 61,                            // <bodypart> ::= <ctrl_stmt>
            @Bodypart_Return_Semi = 62,                // <bodypart> ::= return <expr> ';'
            @Expr = 63,                                // <expr> ::= <value> <operator> <expr>
            @Expr2 = 64,                               // <expr> ::= <value>
            @Expr_Lparen_Rparen = 65,                  // <expr> ::= '(' <expr> ')'
            @Expr_Lparen_Rparen2 = 66,                 // <expr> ::= '(' <expr> ')' <operator> <expr>
            @Expr_Exclam = 67,                         // <expr> ::= '!' <expr>
            @Expr_Minus = 68,                          // <expr> ::= '-' <expr>
            @Ctrl_stmt_If_Lbrace_Rbrace = 69,          // <ctrl_stmt> ::= if <expr> '{' <body> '}' <if_exp>
            @Ctrl_stmt_Repeat_Lbrace_Rbrace = 70,      // <ctrl_stmt> ::= repeat <var_decl> <direction> <expr> '{' <body> '}'
            @Ctrl_stmt_Repeat_Lparen_Rparen_Lbrace_Rbrace = 71,  // <ctrl_stmt> ::= repeat '(' <var_decl> <direction> <expr> ')' '{' <body> '}'
            @Ctrl_stmt_Repeat_Lbrace_Rbrace2 = 72,     // <ctrl_stmt> ::= repeat <expr> '{' <body> '}'
            @Direction_Downto = 73,                    // <direction> ::= downto
            @Direction_To = 74,                        // <direction> ::= to
            @If_exp_Else_If_Lbrace_Rbrace = 75,        // <if_exp> ::= else if <expr> '{' <body> '}' <if_exp>
            @If_exp_Else_Lbrace_Rbrace = 76,           // <if_exp> ::= else '{' <body> '}'
            @If_exp = 77,                              // <if_exp> ::= 
            @Func_call_Id_Lparen_Rparen = 78,          // <func_call> ::= Id '(' <expr_list> ')'
            @Expr_list = 79,                           // <expr_list> ::= <expr> <opt_exprs>
            @Expr_list2 = 80,                          // <expr_list> ::= 
            @Opt_exprs_Comma = 81,                     // <opt_exprs> ::= ',' <expr> <opt_exprs>
            @Opt_exprs = 82                            // <opt_exprs> ::= 
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
            @Include = 38,                             // include
            @Mod = 39,                                 // mod
            @Num = 40,                                 // num
            @Or = 41,                                  // or
            @Program = 42,                             // program
            @Repeat = 43,                              // repeat
            @Return = 44,                              // return
            @String = 45,                              // string
            @Stringliteral = 46,                       // StringLiteral
            @Struct = 47,                              // struct
            @To = 48,                                  // to
            @Void = 49,                                // void
            @Assign_opr = 50,                          // <assign_opr>
            @Body = 51,                                // <body>
            @Bodypart = 52,                            // <bodypart>
            @Const2 = 53,                              // <const>
            @Const_part = 54,                          // <const_part>
            @Consts = 55,                              // <consts>
            @Ctrl_stmt = 56,                           // <ctrl_stmt>
            @Direction = 57,                           // <direction>
            @Expr = 58,                                // <expr>
            @Expr_list = 59,                           // <expr_list>
            @Extra_typeid = 60,                        // <extra_typeid>
            @Func_call = 61,                           // <func_call>
            @Func_decl = 62,                           // <func_decl>
            @Func_decls = 63,                          // <Func_decls>
            @If_exp = 64,                              // <if_exp>
            @Include2 = 65,                            // <include>
            @Includes = 66,                            // <includes>
            @Operator = 67,                            // <operator>
            @Opt_exprs = 68,                           // <opt_exprs>
            @Program2 = 69,                            // <Program>
            @Refrence = 70,                            // <refrence>
            @Struct_decl = 71,                         // <struct_decl>
            @Struct_def = 72,                          // <struct_def>
            @Struct_defs = 73,                         // <struct_defs>
            @Struct_parts = 74,                        // <struct_parts>
            @Type = 75,                                // <type>
            @Typeid = 76,                              // <typeid>
            @Typeid_list = 77,                         // <typeid_list>
            @Value = 78,                               // <value>
            @Var_decl = 79,                            // <var_decl>
            @Var_decls = 80                            // <var_decls>
        }
    }
}