
int program_convertNumToString(double input, string_handle * output){
    char str[50];
    sprintf(str,"%lf",input);

    for (int i = 0; str[i] != '\0' && i<50; ++i)
    {
        string_add(str[i], output);
    }
    return 1;
}
int program_convertBoolToString(int input, string_handle * output){
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
int program_convertStringToNum(string_handle * input, double *output){
	char *str = malloc(sizeof(char)*input->size);
	for (int i = 0; i < input->size; ++i)
	{
		str[i] = string_get(i, input);
	}
	int result = sscanf(str, "%lf", &output);	
	free(str);
	return result;
}
int program_convertStringToBool(string_handle * input, int *output){
	char *str = malloc(sizeof(char)*input->size);
	for (int i = 0; i < input->size; ++i)
	{
		str[i] = string_get(i, input);
	}

	int result = sscanf(str, "%d", &output);	
	free(str);
	return result;
}

void program_printNum(double input){
	printf("%lf", input);
}

void program_printBool(int input){
	string_handle * myString = string_new();
	program_convertBoolToString(input, myString);
	for (int i = 0; i < myString->size; ++i)
	{
		printf("%c", string_get(i, myString));
	}
	free(myString);
}

void program_printString(string_handle * input){
	for (int i = 0; i < input->size; ++i)
	{
		printf("%c", string_get(i, input));
	}
}

string_handle * program_createString(char input[]){
    string_handle *myString = string_new();
    for (int i = 0; input[i] != '\0'; ++i)
    {
        string_add(input[i], myString);
    }

    return myString;
}