
typedef struct {name}_element {name}_element;
typedef struct {name}_handle {name}_handle;

{name}_handle * {name}_newHandle();
{name}_element * {name}_newElement({type} element);
{type} {name}_get(int index, {name}_handle * head);
void {name}_add({type} element, {name}_handle * head);
void {name}_remove(int index, {name}_handle * head);
void {name}_insert(int index, {name}_handle * head, {type} element);
void {name}_clear({name}_handle * head);
void {name}_reverse({name}_handle * head);
void {name}_swap({name}_handle * head, int first, int second);
void {name}_set(int index, {type} value, {name}_handle * head);
void {name}_sort({name}_handle * head);
{name}_handle * {name}_copy({name}_handle * source);