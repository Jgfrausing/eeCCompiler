int program_convertNumToString(double input, string_handle * output);
int program_convertBoolToString(int input, string_handle * output);
int program_convertStringToNum(string_handle * input, double *output);
int program_convertStringToBool(string_handle * input, int *output);
void program_printNum(double input);
void program_printBool(int input);
void program_printString(string_handle * input);
string_handle * program_createString(char input[]);