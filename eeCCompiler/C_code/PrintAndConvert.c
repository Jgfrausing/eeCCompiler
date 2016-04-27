#include <stdio.h>
#include <stdlib.h>
#include <signal.h>
#include <string.h>
typedef struct string_element string_element;
typedef struct string_handle string_handle;
struct string_handle {
    string_element *first;
    string_element *last;
    int size;
};

struct string_element{
    char element;
    string_element *next;
};

string_handle * string_new(){
    string_handle * head = malloc(sizeof(string_handle));
    head->first = NULL;
    head->last = NULL;
    head->size = 0;

    return head;
}

string_element *string_newElement(char inputElement){			// ALLWAYS HAVE ADDCHARACTORTOLIST() CALL THIS!
    string_element * element = malloc(sizeof(string_element));
    element->element = inputElement;
    element->next = NULL;

    return element;
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

void print(char input[])
{
	printf("%s", input);
}

string_handle * read(){
	char str[100];
   	scanf("%s", str);

   	string_handle * head = string_new();
   	for (int i = 0; str[i] == '\0' && i<100; ++i)
   	{
   		string_add(str[i], head);
   	}
}

int convertNumToString(double input, string_handle * output){
    char str[50];
    sprintf(str,"%lf",input);

    for (int i = 0; str[i] != '\0' && i<50; ++i)
    {
        string_add(str[i], output);
    }
    return 1;
}
int convertBoolToString(int input, string_handle * output){
	char str[6], t[] = "true", f[] = "false";
	//strcpy(char *dest, const char *src)

	if(input){
		strcpy(str, t);
	}
	else 
		strcpy(str, f);
   	for (int i = 0; str[i] != '\0' && i<6; ++i)
   		string_add(str[i], output);

   	return 1;
}
int convertStringToNum(string_handle * input, double *output){
	char *str = malloc(sizeof(char)*input->size);
	for (int i = 0; i < input->size; ++i)
	{
		str[i] = string_get(i, input);
	}
	int result = sscanf(str, "%lf", &output);	
	free(str);
	return result;
}
int convertStringToBool(string_handle * input, int *output){
	char *str = malloc(sizeof(char)*input->size);
	for (int i = 0; i < input->size; ++i)
	{
		str[i] = string_get(i, input);
	}

	int result = sscanf(str, "%d", &output);	
	free(str);
	return result;
}

void printNum(double input){
	printf("%lf", input);
}

void printBool(int input){
	string_handle * myString = string_new();
	convertBoolToString(input, myString);
	for (int i = 0; i < myString->size; ++i)
	{
		printf("%c", string_get(i, myString));
	}
	free(myString);
}

void printString(string_handle * input){
	for (int i = 0; i < input->size; ++i)
	{
		printf("%c", string_get(i, input));
	}
}


int main(int argc, char const *argv[])
{
    printBool(1);
    printf("\n");
    printBool(0);
    printf("\n");
    printNum(20.43);
    printf("\n");
    string_handle * myString = string_new();
    for (int i = 0; i < 24; ++i)
    {
    	string_add(i+65, myString);
    }
    printString(myString);

	return 0;
}