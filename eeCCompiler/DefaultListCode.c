
struct {type}list_Handle {
    {type}list_Element *first;
    {type}list_Element *last;
    int size;
};

struct {type}list_Element{
    {type} element;
    {type}list_Element *next;
};

{type}list_Handle * {type}list_new(){
    {type}list_Handle * head = malloc(sizeof({type}list_Handle));
    head->first = NULL;
    head->last = NULL;
    head->size = 0;

    return head;
}

{type}list_Element *{type}list_newElement({type} inputElement){			// ALLWAYS HAVE ADDCHARACTORTOLIST() CALL THIS!
    {type}list_Element * element = malloc(sizeof({type}list_Element));
    element->element = inputElement;
    element->next = NULL;

    return element;
}

{type} {type}list_get(int index, {type}list_Handle * head){
    {type}list_Element * current = head->first;
    int i;
    for (i=0; i<index && current != NULL; i++) {
        current = current->next;
    }
    if (current == NULL){
        raise(SIGSEGV);     //Segmentation fault
    }
    return current->element;
}

void {type}list_add({type} inputElement, {type}list_Handle * head){
    {type}list_Element * element = {type}list_newElement(inputElement);
    if (head->first == NULL)
    {
        head->first = element;
    }
    else{
        {type}list_Element * current = head->first;
        while(current->next != NULL){
            current = current->next;
        }
        current->next = element;
    }
    head->last = element;
    head->size ++;
}

void {type}list_remove(int index, {type}list_Handle * head){
    if (index >= head->size){
        raise(SIGSEGV);     //Segmentation fault
    }
    {type}list_Element * current = head->first;
    {type}list_Element * previous = NULL;

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

void {type}list_insert(int index, {type}list_Handle * head, {type} inputElement){
    if (index > head->size){
        raise(SIGSEGV);     //Segmentation fault
    }
    {type}list_Element * element = {type}list_newElement(inputElement);
    {type}list_Element * current = head->first;
    {type}list_Element * previous = NULL;

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

void {type}list_clear({type}list_Handle * head){
    while(head->size != 0){
        {type}list_remove(0, head);
    }
}

void {type}list_reverse({type}list_Handle * head){
    //PROBLEMER MED LISTE AF LISTER
    {type}list_Handle *temporayHandle = {type}list_new();
    while(head->size != 0){
        {type} element = {type}list_get(0, head);
        {type}list_remove(0, head);
        {type}list_insert(0, temporayHandle, element);
    }    
    //ET ALTERNATIV TIL HEAD = TEMPORAYHANDLE?
    while(temporayHandle->size != 0){
        {type} element = {type}list_get(0, temporayHandle);
        {type}list_remove(0, temporayHandle);
        {type}list_add(element, head);
    }
    free(temporayHandle);
}


void {type}list_swap({type}list_Handle * head, int first, int second){
    if (first >= head->size || second >= head->size){
        raise(SIGSEGV);     //Segmentation fault
    }
    {type} temp = {type}list_get(first, head);
    {type}list_set(first, {type}list_get(second, head), head);
    {type}list_set(second, temp, head);
}

void {type}list_set(int index, {type} value, {type}list_Handle * head){
    if (index >= head->size){
        raise(SIGSEGV);     //Segmentation fault
    }
    {type}list_Element * current = head->first;

    for (int i=0; i<index; i++) {
        current = current->next;
    }
    current->element = value;
}

void {type}list_sort({type}list_Handle * head){
    int m = 0, t = 0, q = 1;

    for (int heapsize = 0; heapsize<head->size; heapsize++){
        int n = heapsize;
        while (n > 0){
            int p = (n+1)/2;
            if({type}list_get(n, head) > {type}list_get(p, head)){
                {type}list_swap(head, n, p); //ChildWithParent
                n = p;
            }
            else{
                break;
            }
        }
    }

    for(int heapsize = head->size; heapsize>0;){
        {type}list_swap(head, 0, --heapsize); //root with last heap element. decreasing heapsize
        int n = 0;
        while(1){
            int left = (n*2)+1;
            if (left >= heapsize)
                break;
            int right = left+1;
            if (right >= heapsize){
                if ({type}list_get(left, head) > {type}list_get(n, head))
                    {type}list_swap(head, left, n);
                break;
            }
            if ({type}list_get(left, head) > {type}list_get(n, head)){
                if ({type}list_get(left, head) > {type}list_get(right, head)){
                    {type}list_swap(head, left, n);
                    n = left;
                    continue;
                }
                else{
                    {type}list_swap(head, right, n);
                    n = right;
                    continue;
                }
            }
            else {
                if ({type}list_get(right, head) > {type}list_get(n, head))
                {
                    {type}list_swap(head, right,n);
                    n = right;
                    continue;
                }
                else
                    break;
            }
        }
    }
        
}