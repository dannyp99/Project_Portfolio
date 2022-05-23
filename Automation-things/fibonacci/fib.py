import sys;

def fib(a=0, b=1):
    yield a;
    yield from fib(b, a+b);

def main():
    if len(sys.argv) < 2:
        user_input = input("Enter a number: ");
        if (user_input.isdigit()):
            n = int(user_input);
            x = fib();
            for i in range(n):
                print(next(x), end=" ");
            print();
        else:
            print("That is not a number");
            return;
    elif sys.argv[1].isdigit():
        n = int(sys.argv[1]);
        x = fib();
        for i in range(n):
            print(next(x), end=" ");
        print();
    else:
        print("Please pass a number as an argument");
main();
