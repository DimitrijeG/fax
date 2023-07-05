#pragma once

#include <stack>

using namespace std;


template <typename T>
class Stack {
    stack<T> s;
public:
    Stack() = default;

    void push(T x) { s.push(x); }
    const T& top() const { return s.top(); }
    T pop();

    bool empty() const { return s.empty(); }
    int size() const { return s.size(); }
};


template<typename T>
T Stack<T>::pop() {
    T x = top();
    s.pop();
    return x;
}