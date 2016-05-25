struct {name}_handle {
    {name}_element *first;
    {name}_element *last;
    int size;
};

struct {name}_element{
    {type} element;
    {name}_element *next;
};

{name}_handle {name}_new(){
    {name}_handle head;
    head.first = NULL;
    head.last = NULL;
    head.size = 0;

    return head;
}

{name}_element *{name}_newElement({type} *inputElement){
    {name}_element * element = malloc(sizeof({name}_element));
    element->element = *inputElement;
    element->next = NULL;

    return element;
}

{type} {name}_get(int index, {name}_handle * head){
    {name}_element * current = head->first;
    int i;
    for (i=0; i<index && current != NULL; i++) {
        current = current->next;
    }
    if (current == NULL){
        raise(SIGSEGV);     //Segmentation fault
    }
    return current->element;
}

void {name}_add({type} *inputElement, {name}_handle * head){
    {name}_element * element = {name}_newElement(inputElement);
    if (head->first == NULL)
    {
        head->first = element;
    }
    else{
        {name}_element * current = head->first;
        while(current->next != NULL){
            current = current->next;
        }
        current->next = element;
    }
    head->last = element;
    head->size ++;
}

void {name}_remove(int index, {name}_handle * head){
    if (index >= head->size){
        raise(SIGSEGV);     //Segmentation fault
    }
    {name}_element * current = head->first;
    {name}_element * previous = NULL;

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

void {name}_insert(int index, {name}_handle * head, {type} *inputElement){
    if (index > head->size){
        raise(SIGSEGV);     //Segmentation fault
    }
    {name}_element * element = {name}_newElement(inputElement);
    {name}_element * current = head->first;
    {name}_element * previous = NULL;

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

void {name}_clear({name}_handle * head){
    while(head->size != 0){
        {name}_remove(0, head);
    }
}

void {name}_reverse({name}_handle * head){
    {name}_handle temporayHandle = {name}_new();
    for (int i = 0; i < head->size; ++i)
    {
        {type} temp = {name}_get(i, head);
        {name}_insert(0, &temporayHandle, &temp);
    }

    for (int i = 0; i < temporayHandle.size; ++i)
    {
        {type} temp = {name}_get(i, &temporayHandle);
        {name}_set(i, &temp, head);
    }
}


void {name}_swap({name}_handle * head, int first, int second){
    if (first >= head->size || second >= head->size){
        raise(SIGSEGV);     //Segmentation fault
    }
    {type} tempFirst = {name}_get(first, head);
    {type} tempSecond = {name}_get(second, head);
    {name}_set(first, &tempSecond, head);
    {name}_set(second, &tempFirst, head);
}

void {name}_set(int index, {type} *value, {name}_handle * head){
    if (index >= head->size){
        raise(SIGSEGV);     //Segmentation fault
    }
    {name}_element * current = head->first;

    for (int i=0; i<index; i++) {
        current = current->next;
    }
    current->element = *value;
}

{name}_handle {name}_copy({name}_handle * source){
    {name}_handle destination = {name}_new();
    {name}_element * current = source->first;
    for (int i = 0; i < source->size; ++i)
    {
        {name}_element * element = {name}_newElement(&current->element);
        current = current->next;
        {name}_add(&element->element, &destination);
        free(element);
    }
    return destination;
}