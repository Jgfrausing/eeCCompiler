//NUMLIST
struct numlist_handle {
    numlist_element *first;
    numlist_element *last;
    int size;
};

struct numlist_element{
    double element;
    numlist_element *next;
};

numlist_handle numlist_new(){
    numlist_handle head;
    head.first = NULL;
    head.last = NULL;
    head.size = 0;

    return head;
}

numlist_element *numlist_newElement(double inputElement){
    numlist_element * element = malloc(sizeof(numlist_element));
    element->element = inputElement;
    element->next = NULL;

    return element;
}

double numlist_get(int index, numlist_handle * head){
    numlist_element * current = head->first;
    int i;
    for (i=0; i<index && current != NULL; i++) {
        current = current->next;
    }
    if (current == NULL){
        raise(SIGSEGV);     //Segmentation fault
    }
    return current->element;
}

void numlist_add(double inputElement, numlist_handle * head){
    numlist_element * element = numlist_newElement(inputElement);
    if (head->first == NULL)
    {
        head->first = element;
    }
    else{
        numlist_element * current = head->first;
        while(current->next != NULL){
            current = current->next;
        }
        current->next = element;
    }
    head->last = element;
    head->size ++;
}

void numlist_remove(int index, numlist_handle * head){
    if (index >= head->size){
        raise(SIGSEGV);     //Segmentation fault
    }
    numlist_element * current = head->first;
    numlist_element * previous = NULL;

    if(index == 0 && head->first != NULL){
        head->first = head->first->next;
    }
    else{
        for (int i=0; i<index; i++) {
            previous = current;
            current = current->next;
        }
        previous->next = current->next;
    }

    

    if(current->next == NULL){
        head->last = previous;
    }
    free(current);		 //releases memory
    head->size--;
}

void numlist_insert(int index, numlist_handle * head, double inputElement){
    if (index > head->size){
        raise(SIGSEGV);     //Segmentation fault
    }
    numlist_element * element = numlist_newElement(inputElement);
    numlist_element * current = head->first;
    numlist_element * previous = NULL;

    if(index == 0){
        element->next = current;
        head->first = element;
    }
    else{
        for (int i=0; i<index; i++) {
            previous = current;
            current = current->next;
        }
        element->next = current;
        previous->next = element;
    }

    

    if(element->next == NULL){
        head->last = element;
    }
    head->size++;
}

void numlist_clear(numlist_handle * head){
    while(head->size != 0){
        numlist_remove(0, head);
    }
}

void numlist_reverse(numlist_handle * head){
    //PROBLEMER MED LISTE AF LISTER
    numlist_handle temporayHandle = numlist_new();
    while(head->size != 0){
        double element = numlist_get(0, head);
        numlist_remove(0, head);
        numlist_insert(0, &temporayHandle, element);
    }    
    //ET ALTERNATIV TIL HEAD = TEMPORAYHANDLE?
    while(temporayHandle.size != 0){
        double element = numlist_get(0, &temporayHandle);
        numlist_remove(0, &temporayHandle);
        numlist_add(element, head);
    }
}

void numlist_swap(numlist_handle * head, int first, int second){
    if (first >= head->size || second >= head->size){
        raise(SIGSEGV);     //Segmentation fault
    }
    double temp = numlist_get(first, head);
    numlist_set(first, numlist_get(second, head), head);
    numlist_set(second, temp, head);
}

void numlist_set(int index, double value, numlist_handle * head){
    if (index >= head->size){
        raise(SIGSEGV);     //Segmentation fault
    }
    numlist_element * current = head->first;

    for (int i=0; i<index; i++) {
        current = current->next;
    }
    current->element = value;
}

int max (numlist_handle * head, int n, int i, int j, int k) {
    int m = i;
    if (j < n && numlist_get(j, head) > numlist_get(m, head)) {
        m = j;
    }
    if (k < n && numlist_get(k, head) > numlist_get(m, head)) {
        m = k;
    }
    return m;
}
 
void downheap (numlist_handle * head, int n, int i) {
    while (1) {
        int j = max(head, n, i, 2 * i + 1, 2 * i + 2);
        if (j == i) {
            break;
        }
        numlist_swap(head, j, i);
        i = j;
    }
}
 
void numlist_sort(numlist_handle * head){
    //https://rosettacode.org/wiki/Sorting_algorithms/Heapsort#C
    int n = head->size;
    for (int i = (n - 2) / 2; i >= 0; i--) {
        downheap(head, n, i);
    }
    for (int i = 0; i < n; i++) {
        numlist_swap(head, n - i - 1, 0);
        downheap(head, n - i - 1, 0);
    }
}

numlist_handle numlist_copy(numlist_handle * source){
    numlist_handle destination = numlist_new();
    numlist_element * current = source->first;
    for (int i = 0; i < source->size; ++i)
    {
        numlist_element * element = numlist_newElement(current->element);
        current = current->next;
        numlist_add(element->element, &destination);
    }
    return destination;
}

//BOOLLIST
struct boollist_handle {
    boollist_element *first;
    boollist_element *last;
    int size;
};

struct boollist_element{
    int element;
    boollist_element *next;
};

boollist_handle boollist_new(){
    boollist_handle head;
    head.first = NULL;
    head.last = NULL;
    head.size = 0;

    return head;
}

boollist_element *boollist_newElement(int inputElement){			// ALLWAYS HAVE ADDCHARACTORTOLIST() CALL THIS!
    boollist_element * element = malloc(sizeof(boollist_element));
    element->element = inputElement;
    element->next = NULL;

    return element;
}

int boollist_get(int index, boollist_handle * head){
    boollist_element * current = head->first;
    int i;
    for (i=0; i<index && current != NULL; i++) {
        current = current->next;
    }
    if (current == NULL){
        raise(SIGSEGV);     //Segmentation fault
    }
    return current->element;
}

void boollist_add(int inputElement, boollist_handle * head){
    boollist_element * element = boollist_newElement(inputElement);
    if (head->first == NULL)
    {
        head->first = element;
    }
    else{
        boollist_element * current = head->first;
        while(current->next != NULL){
            current = current->next;
        }
        current->next = element;
    }
    head->last = element;
    head->size ++;
}

void boollist_remove(int index, boollist_handle * head){
    if (index >= head->size){
        raise(SIGSEGV);     //Segmentation fault
    }
    boollist_element * current = head->first;
    boollist_element * previous = NULL;

    if(index == 0 && head->first != NULL){
        head->first = head->first->next;
    }
    else{
        for (int i=0; i<index; i++) {
            previous = current;
            current = current->next;
        }
        previous->next = current->next;
    }

    

    if(current->next == NULL){
        head->last = previous;
    }
    free(current);		 //releases memory
    head->size--;
}

void boollist_insert(int index, boollist_handle * head, int inputElement){
    if (index > head->size){
        raise(SIGSEGV);     //Segmentation fault
    }
    boollist_element * element = boollist_newElement(inputElement);
    boollist_element * current = head->first;
    boollist_element * previous = NULL;

    if(index == 0){
        element->next = current;
        head->first = element;
    }
    else{
        for (int i=0; i<index; i++) {
            previous = current;
            current = current->next;
        }
        element->next = current;
        previous->next = element;
    }

    

    if(element->next == NULL){
        head->last = element;
    }
    head->size++;
}

void boollist_clear(boollist_handle * head){
    while(head->size != 0){
        boollist_remove(0, head);
    }
}

void boollist_reverse(boollist_handle * head){
    //PROBLEMER MED LISTE AF LISTER
    boollist_handle temporayHandle = boollist_new();
    while(head->size != 0){
        int element = boollist_get(0, head);
        boollist_remove(0, head);
        boollist_insert(0, &temporayHandle, element);
    }    
    //ET ALTERNATIV TIL HEAD = TEMPORAYHANDLE?
    while(temporayHandle.size != 0){
        int element = boollist_get(0, &temporayHandle);
        boollist_remove(0, &temporayHandle);
        boollist_add(element, head);
    }
}

void boollist_swap(boollist_handle * head, int first, int second){
    if (first >= head->size || second >= head->size){
        raise(SIGSEGV);     //Segmentation fault
    }
    int temp = boollist_get(first, head);
    boollist_set(first, boollist_get(second, head), head);
    boollist_set(second, temp, head);
}

void boollist_set(int index, int value, boollist_handle * head){
    if (index >= head->size){
        raise(SIGSEGV);     //Segmentation fault
    }
    boollist_element * current = head->first;

    for (int i=0; i<index; i++) {
        current = current->next;
    }
    current->element = value;
}

boollist_handle boollist_copy(boollist_handle * source){
    boollist_handle destination = boollist_new();
    boollist_element * current = source->first;
    for (int i = 0; i < source->size; ++i)
    {
        boollist_element * element = boollist_newElement(current->element);
        current = current->next;
        boollist_add(element->element, &destination);
    }
    return destination;
}

//STRING
struct string_handle {
    string_element *first;
    string_element *last;
    int size;
};

struct string_element{
    char element;
    string_element *next;
};

string_handle string_new(){
    string_handle head;
    head.first = NULL;
    head.last = NULL;
    head.size = 0;

    return head;
}

string_element *string_newElement(char inputElement){			// ALLWAYS HAVE ADDCHARACTORTOLIST() CALL THIS!
    string_element * element = malloc(sizeof(string_element));
    element->element = inputElement;
    element->next = NULL;

    return element;
}

char string_get(int index, string_handle * head){
    string_element * current = head->first;
    int i;
    for (i=0; i<index && current != NULL; i++) {
        current = current->next;
    }
    if (current == NULL){
        raise(SIGSEGV);     //Segmentation fault
    }
    return current->element;
}

void string_add(char inputElement, string_handle * head){
    string_element * element = string_newElement(inputElement);
    if (head->first == NULL)
    {
        head->first = element;
    }
    else{
        string_element * current = head->first;
        while(current->next != NULL){
            current = current->next;
        }
        current->next = element;
    }
    head->last = element;
    head->size ++;
}

void string_remove(int index, string_handle * head){
    if (index >= head->size){
        raise(SIGSEGV);     //Segmentation fault
    }
    string_element * current = head->first;
    string_element * previous = NULL;

    if(index == 0 && head->first != NULL){
        head->first = head->first->next;
    }
    else{
        for (int i=0; i<index; i++) {
            previous = current;
            current = current->next;
        }
        previous->next = current->next;
    }

    

    if(current->next == NULL){
        head->last = previous;
    }
    free(current);		 //releases memory
    head->size--;
}

void string_insert(int index, string_handle * head, char inputElement){
    if (index > head->size){
        raise(SIGSEGV);     //Segmentation fault
    }
    string_element * element = string_newElement(inputElement);
    string_element * current = head->first;
    string_element * previous = NULL;

    if(index == 0){
        element->next = current;
        head->first = element;
    }
    else{
        for (int i=0; i<index; i++) {
            previous = current;
            current = current->next;
        }
        element->next = current;
        previous->next = element;
    }

    

    if(element->next == NULL){
        head->last = element;
    }
    head->size++;
}

void string_clear(string_handle * head){
    while(head->size != 0){
        string_remove(0, head);
    }
}

void string_reverse(string_handle * head){
    //PROBLEMER MED LISTE AF LISTER
    string_handle temporayHandle = string_new();
    while(head->size != 0){
        char element = string_get(0, head);
        string_remove(0, head);
        string_insert(0, &temporayHandle, element);
    }    
    //ET ALTERNATIV TIL HEAD = TEMPORAYHANDLE?
    while(temporayHandle.size != 0){
        char element = string_get(0, &temporayHandle);
        string_remove(0, &temporayHandle);
        string_add(element, head);
    }
}

void string_swap(string_handle * head, int first, int second){
    if (first >= head->size || second >= head->size){
        raise(SIGSEGV);     //Segmentation fault
    }
    char temp = string_get(first, head);
    string_set(first, string_get(second, head), head);
    string_set(second, temp, head);
}

void string_set(int index, char value, string_handle * head){
    if (index >= head->size){
        raise(SIGSEGV);     //Segmentation fault
    }
    string_element * current = head->first;

    for (int i=0; i<index; i++) {
        current = current->next;
    }
    current->element = value;
}

string_handle string_copy(string_handle * source){
    string_handle destination = string_new();
    string_element * current = source->first;
    for (int i = 0; i < source->size; ++i)
    {
        string_element * element = string_newElement(current->element);
        current = current->next;
        string_add(element->element, &destination);
    }
    return destination;
}

int string_equals(string_handle * first, string_handle * second){
    if(first->size == second->size){
        for (int i = 0; i < first->size; ++i)
        {
            if(string_get(i, first) != string_get(i, second))
                return 0;
        }
        return 1;
    }
    else
        return 0;
}


//LIST OF STRINGS

struct stringlist_handle {
    stringlist_element *first;
    stringlist_element *last;
    int size;
};

struct stringlist_element{
    string_handle element;
    stringlist_element *next;
};

stringlist_handle stringlist_new(){
    stringlist_handle head;
    head.first = NULL;
    head.last = NULL;
    head.size = 0;

    return head;
}

stringlist_element *stringlist_newElement(string_handle *inputElement){
    stringlist_element * element = malloc(sizeof(stringlist_element));
    element->element = *inputElement;
    element->next = NULL;

    return element;
}

string_handle stringlist_get(int index, stringlist_handle * head){
    stringlist_element * current = head->first;
    int i;
    for (i=0; i<index && current != NULL; i++) {
        current = current->next;
    }
    if (current == NULL){
        raise(SIGSEGV);     //Segmentation fault
    }
    return current->element;
}

void stringlist_add(string_handle *inputElement, stringlist_handle * head){
    stringlist_element * element = stringlist_newElement(inputElement);
    if (head->first == NULL)
    {
        head->first = element;
    }
    else{
        stringlist_element * current = head->first;
        while(current->next != NULL){
            current = current->next;
        }
        current->next = element;
    }
    head->last = element;
    head->size ++;
}

void stringlist_remove(int index, stringlist_handle * head){
    if (index >= head->size){
        raise(SIGSEGV);     //Segmentation fault
    }
    stringlist_element * current = head->first;
    stringlist_element * previous = NULL;

    if(index == 0 && head->first != NULL){
        head->first = head->first->next;
    }
    else{
        for (int i=0; i<index; i++) {
            previous = current;
            current = current->next;
        }
        previous->next = current->next;
    }

    

    if(current->next == NULL){
        head->last = previous;
    }
    free(current);		 //releases memory
    head->size--;
}

void stringlist_insert(int index, stringlist_handle * head, string_handle *inputElement){
    if (index > head->size){
        raise(SIGSEGV);     //Segmentation fault
    }
    stringlist_element * element = stringlist_newElement(inputElement);
    stringlist_element * current = head->first;
    stringlist_element * previous = NULL;

    if(index == 0){
        element->next = current;
        head->first = element;
    }
    else{
        for (int i=0; i<index; i++) {
            previous = current;
            current = current->next;
        }
        element->next = current;
        previous->next = element;
    }

    if(element->next == NULL){
        head->last = element;
    }
    head->size++;
}

void stringlist_clear(stringlist_handle * head){
    while(head->size != 0){
        stringlist_remove(0, head);
    }
}

void stringlist_reverse(stringlist_handle * head){
    stringlist_handle temporayHandle = stringlist_new();
    for (int i = 0; i < head->size; ++i)
    {
        string_handle temp = stringlist_get(i, head);
        stringlist_insert(0, &temporayHandle, &temp);
    }

    for (int i = 0; i < temporayHandle.size; ++i)
    {
        string_handle temp = stringlist_get(i, &temporayHandle);
        stringlist_set(i, &temp, head);
    }
}


void stringlist_swap(stringlist_handle * head, int first, int second){
    if (first >= head->size || second >= head->size){
        raise(SIGSEGV);     //Segmentation fault
    }
    string_handle tempFirst = stringlist_get(first, head);
    string_handle tempSecond = stringlist_get(second, head);
    stringlist_set(first, &tempSecond, head);
    stringlist_set(second, &tempFirst, head);
}

void stringlist_set(int index, string_handle *value, stringlist_handle * head){
    if (index >= head->size){
        raise(SIGSEGV);     //Segmentation fault
    }
    stringlist_element * current = head->first;

    for (int i=0; i<index; i++) {
        current = current->next;
    }
    current->element = *value;
}

stringlist_handle stringlist_copy(stringlist_handle * source){
    stringlist_handle destination = stringlist_new();
    stringlist_element * current = source->first;
    for (int i = 0; i < source->size; ++i)
    {
        stringlist_element * element = stringlist_newElement(&current->element);
        current = current->next;
        stringlist_add(&element->element, &destination);
    }
    return destination;
}

//STANDARD FUNCTIONS
int program_convertNumToString(double input, string_handle * output){
    char str[50];
    sprintf(str,"%lf",input);
    string_clear(output);
    for (int i = 0; str[i] != '\0' && i<50; ++i)
    {
        string_add(str[i], output);
    }
    return 1;
}
int program_convertBoolToString(int input, string_handle * output){
	char str[6], t[] = "true", f[] = "false";
	string_clear(output);

	if(input){
		strcpy(str, t);
	}
	else 
		strcpy(str, f);
   	for (int i = 0; str[i] != '\0' && i<6; ++i)
   		string_add(str[i], output);

   	return 1;
}
int program_convertStringToNum(string_handle * input, double *output){
	char *str = malloc(sizeof(char)*input->size);
	for (int i = 0; i < input->size; ++i)
	{
		str[i] = string_get(i, input);
	}
	int result = sscanf(str, "%lf", output);	
	free(str);
	return result;
}
int program_convertStringToBool(string_handle * input, int *output){
	char *str = malloc(sizeof(char)*input->size);
	for (int i = 0; i < input->size; ++i)
	{
		str[i] = string_get(i, input);
	}

	int result = sscanf(str, "%d", output);	
	free(str);
	return result;
}
string_handle standard_read(){
    char str[100];
    scanf ("%[^\n]%*c", str);

    string_handle head = string_new();
    for (int i = 0; str[i] != '\0' && i<100; ++i)
    {
        string_add(str[i], &head);
    }
	fflush(stdin);
    return head;
}

void standard_printNum(double input){
	printf("%lf", input);
}

void standard_printBool(int input){
	string_handle myString = string_new();
	program_convertBoolToString(input, &myString);
	for (int i = 0; i < myString.size; ++i)
	{
		printf("%c", string_get(i, &myString));
	}
}

void standard_printString(string_handle * input){
	for (int i = 0; i < input->size; ++i)
	{
		printf("%c", string_get(i, input));
	}
}

void standard_printChars(char input[]){
	printf("%s", input);
}

string_handle standard_createString(char input[]){
    string_handle myString = string_new();
    for (int i = 0; input[i] != '\0'; ++i)
    {
        string_add(input[i], &myString);
    }

    return myString;
}

void standard_appendChars(string_handle * head, char input[]){
	string_handle myInputString = standard_createString(input);
	standard_appendString(head, &myInputString);
	string_clear(&myInputString);
}

void standard_appendString(string_handle * head, string_handle * input){
	for (int i = 0; i < input->size; ++i)
	{
		string_add(string_get(i, input), head);
	}
/*    if(head->size == 0){
        head->last = input->last;
        head->first = input->first;
    }
    else{
		head->last->next = input->first;
		head->last = input->last;
    }

    head->size += input->size;*/
}

void standard_appendNum(string_handle * head, double input){
	string_handle myString = string_new();
	program_convertNumToString(input, &myString);
	standard_appendString(head, &myString);
}

void standard_appendBool(string_handle * head, int input){
	string_handle myString = string_new();
	program_convertBoolToString(input, &myString);
	standard_appendString(head, &myString);
}

