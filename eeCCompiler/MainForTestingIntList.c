void main(){
    intlist_Handle *myIntList = intlist_new();
    intlist_add(1, myIntList);
    intlist_add(2, myIntList);
    intlist_add(3, myIntList);
    intlist_add(4, myIntList);
    printf("Printing List.\n");
    for (int i = 0; i < myIntList->size; ++i)
    {
        printf("%d\n", intlist_get(i, myIntList));
    }
    printf("Removing 3rd element.\n");
    intlist_remove(2, myIntList);
    printf("Printing List.\n");
    for (int i = 0; i < myIntList->size; ++i)
    {
        printf("%d\n", intlist_get(i, myIntList));
    }
    printf("Inserting 3 again.\n");
    intlist_insert(2, myIntList, 3);
        printf("Printing List.\n");
    for (int i = 0; i < myIntList->size; ++i)
    {
        printf("%d\n", intlist_get(i, myIntList));
    }
    printf("Reverting list.\n");
    intlist_reverse(myIntList);
    printf("Printing List.\n");
    for (int i = 0; i < myIntList->size; ++i)
    {
        printf("%d\n", intlist_get(i, myIntList));
    }
    printf("Sorting list.\n");
    intlist_sort(myIntList);
    printf("Printing List.\n");
    for (int i = 0; i < myIntList->size; ++i)
    {
        printf("%d\n", intlist_get(i, myIntList));
    }
    printf("Clearing list.\n");
    intlist_clear(myIntList);
    printf("Printing List.\n");
    printf("Size of list = %d\n", myIntList->size);
    for (int i = 0; i < myIntList->size; ++i)
    {
        printf("%d\n", intlist_get(i, myIntList));
    }


}