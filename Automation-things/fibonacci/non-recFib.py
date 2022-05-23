import sys;

a = 0;
b = 1;

def fib():
    global a;
    global b;
    _a  = a;
    a = b;
    b = a + b;
    return a;

n = int(sys.argv[1]);
for _ in range(n):
    x = fib();
    #print(x, end= " ")
print(x);
print()
