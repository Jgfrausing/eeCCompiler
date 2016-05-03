//NUMLIST

typedef struct numlist_element numlist_element;
typedef struct numlist_handle numlist_handle;

numlist_handle * numlist_newHandle();
numlist_element * numlist_newElement(double element);
double numlist_get(int index, numlist_handle * head);
void numlist_add(double element, numlist_handle * head);
void numlist_remove(int index, numlist_handle * head);
void numlist_insert(int index, numlist_handle * head, double element);
void numlist_clear(numlist_handle * head);
void numlist_reverse(numlist_handle * head);
void numlist_swap(numlist_handle * head, int first, int second);
void numlist_set(int index, double value, numlist_handle * head);
void numlist_sort(numlist_handle * head);
numlist_handle numlist_copy(numlist_handle * source);

//BOOLLIST
typedef struct boollist_element boollist_element;
typedef struct boollist_handle boollist_handle;

boollist_handle * boollist_newHandle();
boollist_element * boollist_newElement(int element);
int boollist_get(int index, boollist_handle * head);
void boollist_add(int element, boollist_handle * head);
void boollist_remove(int index, boollist_handle * head);
void boollist_insert(int index, boollist_handle * head, int element);
void boollist_clear(boollist_handle * head);
void boollist_reverse(boollist_handle * head);
void boollist_swap(boollist_handle * head, int first, int second);
void boollist_set(int index, int value, boollist_handle * head);
boollist_handle boollist_copy(boollist_handle * source);

//STRING
typedef struct string_element string_element;
typedef struct string_handle string_handle;

string_handle * string_newHandle();
string_element * string_newElement(char element);
char string_get(int index, string_handle * head);
void string_add(char element, string_handle * head);
void string_remove(int index, string_handle * head);
void string_insert(int index, string_handle * head, char element);
void string_clear(string_handle * head);
void string_reverse(string_handle * head);
void string_swap(string_handle * head, int first, int second);
void string_set(int index, char value, string_handle * head);
void string_sort(string_handle * head);
string_handle string_copy(string_handle * source);
int string_equals(string_handle * first, string_handle * second);

//STANDARD FUNCTIONS
int program_convertNumToString(double input, string_handle * output);
int program_convertBoolToString(int input, string_handle * output);
int program_convertStringToNum(string_handle * input, double *output);
int program_convertStringToBool(string_handle * input, int *output);
void standard_printNum(double input);
void standard_printBool(int input);
void standard_printString(string_handle * input);
void standard_printChars(char input[]);
string_handle standard_createString(char input[]);
void standard_appendChars(string_handle * head, char input[]);
void standard_appendString(string_handle * head, string_handle * input);
void standard_appendNum(string_handle * head, double input);
void standard_appendBool(string_handle * head, int input);
