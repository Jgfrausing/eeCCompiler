
typedef struct {type}list_Element {type}list_Element;
typedef struct {type}list_Handle {type}list_Handle;

{type}list_Handle * {type}list_newHandle();
{type}list_Element * {type}list_newElement({type} element);
{type} {type}list_get(int index, {type}list_Handle * head);
void {type}list_add({type} element, {type}list_Handle * head);
void {type}list_remove(int index, {type}list_Handle * head);
void {type}list_insert(int index, {type}list_Handle * head, {type} element);
void {type}list_clear({type}list_Handle * head);
void {type}list_reverse({type}list_Handle * head);
void {type}list_swap({type}list_Handle * head, int first, int second);
void {type}list_set(int index, {type} value, {type}list_Handle * head);
void {type}list_sort({type}list_Handle * head);