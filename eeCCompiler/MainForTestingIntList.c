void main(){
    boollist_Handle *myboollist = boollist_new();
    boollist_add(1, myboollist);
    boollist_add(2, myboollist);
    boollist_add(3, myboollist);
    boollist_add(4, myboollist);
    printf("Printing List.\n");
    for (int i = 0; i < myboollist->size; ++i)
    {
        printf("%d\n", boollist_get(i, myboollist));
    }
    printf("Removing 3rd element.\n");
    boollist_remove(2, myboollist);
    printf("Printing List.\n");
    for (int i = 0; i < myboollist->size; ++i)
    {
        printf("%d\n", boollist_get(i, myboollist));
    }
    printf("Inserting 3 again.\n");
    boollist_insert(2, myboollist, 3);
        printf("Printing List.\n");
    for (int i = 0; i < myboollist->size; ++i)
    {
        printf("%d\n", boollist_get(i, myboollist));
    }
    printf("Reverting list.\n");
    boollist_reverse(myboollist);
    printf("Printing List.\n");
    for (int i = 0; i < myboollist->size; ++i)
    {
        printf("%d\n", boollist_get(i, myboollist));
    }
    printf("Sorting list.\n");
    boollist_sort(myboollist);
    printf("Printing List.\n");
    for (int i = 0; i < myboollist->size; ++i)
    {
        printf("%d\n", boollist_get(i, myboollist));
    }
    printf("Clearing list.\n");
    boollist_clear(myboollist);
    printf("Printing List.\n");
    printf("Size of list = %d\n", myboollist->size);
    for (int i = 0; i < myboollist->size; ++i)
    {
        printf("%d\n", boollist_get(i, myboollist));
    }


}