void print(char *input);
char* read();
int stringconvertNumToString(double input, char *output);
int convertBoolToString(int input, char *output); 
int convertStringToNum(char* input, double *output);
int convertStringToBool(char* input, int* output);


void print(char *input){
    printf("%s\n", input);
}
char* read(){
    char *input = "";
    scanf("%s", input);
    return input;
}
int stringconvertNumToString(double input, char *output){
    sprintf(output,"%lf", input);
    return true;
}
int convertBoolToString(int input, char *output); 
{
    sprintf(output,"%d", input);
    return true;
}
int convertStringToNum(char* input, double *output);
int convertStringToBool(char* input, int* output);
