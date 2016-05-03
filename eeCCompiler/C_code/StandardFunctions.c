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

void numlist_sort(numlist_handle * head){
    int m = 0, t = 0, q = 1;

    for (int heapsize = 0; heapsize<head->size; heapsize++){
        int n = heapsize;
        while (n > 0){
            int p = (n+1)/2;
            if(numlist_get(n, head) > numlist_get(p, head)){
                numlist_swap(head, n, p); //ChildWithParent
                n = p;
            }
            else{
                break;
            }
        }
    }

    for(int heapsize = head->size; heapsize>0;){
        numlist_swap(head, 0, --heapsize); //root with last heap element. decreasing heapsize
        int n = 0;
        while(1){
            int left = (n*2)+1;
            if (left >= heapsize)
                break;
            int right = left+1;
            if (right >= heapsize){
                if (numlist_get(left, head) > numlist_get(n, head))
                    numlist_swap(head, left, n);
                break;
            }
            if (numlist_get(left, head) > numlist_get(n, head)){
                if (numlist_get(left, head) > numlist_get(right, head)){
                    numlist_swap(head, left, n);
                    n = left;
                    continue;
                }
                else{
                    numlist_swap(head, right, n);
                    n = right;
                    continue;
                }
            }
            else {
                if (numlist_get(right, head) > numlist_get(n, head))
                {
                    numlist_swap(head, right,n);
                    n = right;
                    continue;
                }
                else
                    break;
            }
        }
    }
}

numlist_handle numlist_copy(numlist_handle * source){
    numlist_handle destination;
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
    boollist_handle destination;
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
    string_handle destination;
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
}

void standard_appendString(string_handle * head, string_handle * input){
    if(head->size == 0){
        head->last = input->last;
        head->first = input->first;
    }
    else{
		head->last->next = input->first;
		head->last = input->last;
    }

    head->size += input->size;
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