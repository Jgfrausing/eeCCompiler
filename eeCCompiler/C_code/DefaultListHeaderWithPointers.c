typedef struct {name}_handle {name}_handle;
typedef struct {name}_element {name}_element;
{name}_handle * {name}_new();
{name}_element *{name}_newElement({type} *inputElement);
{type} *{name}_get(int index, {name}_handle * head);
void {name}_add({type} *inputElement, {name}_handle * head);
void {name}_remove(int index, {name}_handle * head);
void {name}_insert(int index, {name}_handle * head, {type} *inputElement);
void {name}_clear({name}_handle * head);
void {name}_reverse({name}_handle * head);
void {name}_swap({name}_handle * head, int first, int second);
void {name}_set(int index, {type} *value, {name}_handle * head);
{name}_handle * {name}_copy({name}_handle * source);