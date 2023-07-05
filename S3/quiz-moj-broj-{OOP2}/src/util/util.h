#pragma once

#include <string>

using namespace std;
// pomoćne funkcije


template <typename T> void swapObjects(T& a, T& b) {
    T tmp = a;
    a = b;
    b = tmp;
}
