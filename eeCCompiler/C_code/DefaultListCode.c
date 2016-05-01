
struct {name}_handle {
    {name}_element *first;
    {name}_element *last;
    int size;
};

struct {name}_element{
    {type} element;
    {name}_element *next;
};

{name}_handle * {name}_new(){
    {name}_handle * head = malloc(sizeof({name}_handle));
    head->first = NULL;
    head->last = NULL;
    head->size = 0;

    return head;
}

{name}_element *{name}_newElement({type} inputElement){			// ALLWAYS HAVE ADDCHARACTORTOLIST() CALL THIS!
    {name}_element * element = malloc(sizeof({name}_element));
    element->element = inputElement;
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

void {name}_add({type} inputElement, {name}_handle * head){
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

void {name}_insert(int index, {name}_handle * head, {type} inputElement){
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
    //PROBLEMER MED LISTE AF LISTER
    {name}_handle *temporayHandle = {name}_new();
    while(head->size != 0){
        {type} element = {name}_get(0, head);
        {name}_remove(0, head);
        {name}_insert(0, temporayHandle, element);
    }    
    //ET ALTERNATIV TIL HEAD = TEMPORAYHANDLE?
    while(temporayHandle->size != 0){
        {type} element = {name}_get(0, temporayHandle);
        {name}_remove(0, temporayHandle);
        {name}_add(element, head);
    }
    free(temporayHandle);
}


void {name}_swap({name}_handle * head, int first, int second){
    if (first >= head->size || second >= head->size){
        raise(SIGSEGV);     //Segmentation fault
    }
    {type} temp = {name}_get(first, head);
    {name}_set(first, {name}_get(second, head), head);
    {name}_set(second, temp, head);
}

void {name}_set(int index, {type} value, {name}_handle * head){
    if (index >= head->size){
        raise(SIGSEGV);     //Segmentation fault
    }
    {name}_element * current = head->first;

    for (int i=0; i<index; i++) {
        current = current->next;
    }
    current->element = value;
}

void {name}_sort({name}_handle * head){
    int m = 0, t = 0, q = 1;

    for (int heapsize = 0; heapsize<head->size; heapsize++){
        int n = heapsize;
        while (n > 0){
            int p = (n+1)/2;
            if({name}_get(n, head) > {name}_get(p, head)){
                {name}_swap(head, n, p); //ChildWithParent
                n = p;
            }
            else{
                break;
            }
        }
    }

    for(int heapsize = head->size; heapsize>0;){
        {name}_swap(head, 0, --heapsize); //root with last heap element. decreasing heapsize
        int n = 0;
        while(1){
            int left = (n*2)+1;
            if (left >= heapsize)
                break;
            int right = left+1;
            if (right >= heapsize){
                if ({name}_get(left, head) > {name}_get(n, head))
                    {name}_swap(head, left, n);
                break;
            }
            if ({name}_get(left, head) > {name}_get(n, head)){
                if ({name}_get(left, head) > {name}_get(right, head)){
                    {name}_swap(head, left, n);
                    n = left;
                    continue;
                }
                else{
                    {name}_swap(head, right, n);
                    n = right;
                    continue;
                }
            }
            else {
                if ({name}_get(right, head) > {name}_get(n, head))
                {
                    {name}_swap(head, right,n);
                    n = right;
                    continue;
                }
                else
                    break;
            }
        }
    }
}

{name}_handle * {name}_copy({name}_handle * source){
    {name}_handle *destination = {name}_new();
    {name}_element * current = source->first;
    for (int i = 0; i < source->size; ++i)
    {
        {name}_element * element = {name}_newElement(current->element);
        current = current->next;
        {name}_add(element->element, destination);
    }
    return destination;
}