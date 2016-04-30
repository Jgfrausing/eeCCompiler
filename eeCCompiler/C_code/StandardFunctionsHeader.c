int program_convertNumToString(double input, string_handle * output);
int program_convertBoolToString(int input, string_handle * output);
int program_convertStringToNum(string_handle * input, double *output);
int program_convertStringToBool(string_handle * input, int *output);
void standard_printNum(double input);
void standard_printBool(int input);
void standard_printString(string_handle * input);
void standard_printChars(char input[]);
string_handle * standard_createString(char input[]);
void standard_appendChars(string_handle * head, char input[]);
void standard_appendString(string_handle * head, string_handle * input);
void standard_appendNum(string_handle * head, double input);
void standard_appendBool(string_handle * head, int input);


