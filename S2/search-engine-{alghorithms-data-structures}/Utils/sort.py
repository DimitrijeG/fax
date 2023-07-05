
"""
Modifikovan Heap Sort koji sortira listu tuplova po prvoj vrednosti u opadajucem poretku.
"""
def heap_sort(array: list) -> list:
    for start in range((len(array)-2)//2, -1, -1):
        _siftdown(array, start, len(array)-1)
 
    for end in range(len(array)-1, 0, -1):
        array[end], array[0] = array[0], array[end]
        _siftdown(array, 0, end - 1)
    return array


def _siftdown(array, start, end):
    root = start
    while True:
        child = root * 2 + 1
        if child > end: break
        if child + 1 <= end and array[child][0] > array[child + 1][0]:
            child += 1
        if array[root][0] > array[child][0]:
            array[root], array[child] = array[child], array[root]
            root = child
        else:
            break
