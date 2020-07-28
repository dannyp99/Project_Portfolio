// Base program for F# Assignment 3, version 7e3

(*
    The base program contains the following major components:

    1. The definition of the abstract syntax representation of
       expressions such as 3+2*5, and (let x=2:x+x), etc.  These are
       variants of the expression trees that we have looked at, but
       now we're going to use them.  The type 'expr' is the type of
       expressions, and many cases have been included, but you won't
       need all of these cases.  The expression 'Closure' is needed if
       you choose to implement lambda expressions (see below).

    2. The evaluation of abstract syntax expr's, corresponding to an
       interpreter.  The mutable function 'eval' evals all exprs to
       some int.  However, since variables are possible expressions,
       their evaluation requires an "environment".  The type 'environ'
       uses a F# list to represent bindings between variables and
       their values: type 'environ' is just an alias for type
       '(string*(expr ref)) list'.  That is, if this list is
       (x,xv)::cdr, then x is a string and xv is a reference to an
       expression. Unless you implement lambdas, however, the
       expression should only be of the form Val(n). xv is a reference
       so its contents can be changed should you implement the
       'assign' operation (not require).  The functions
       lookup/lookupval and change/changeval have been defined so you
       can use this data structure easily.  

    3. A Lexical analyser that takes an input string such as
      "7+3*2" and convert to a list of symbols (also type expr) like
      [|Val(7.0);Sym("+");Val(3.0);Sym("*");Val(2.0);EOF|];  This
      token array is stored in the global variable TS and indexed by TI.
      This stage of compiling/interpreting is the least interesting and
      I've written all the code you should ever need regardless of how
      far you want to go for this assignment (except maybe for level 7).

      Note: the lexical analyzer is not capable of distinguishing
      between "=" and "==" as separate tokens.  I didn't spend too 
      much time on this part of the program. Use keyword "eq" or some
      other token for boolean equality, use "leq" for <=, etc.

    4. An operator-precedence, shift-reduce parser that takes a list of
       tokens and returns an expr expression tree.  You will need to
       modify this function slightly in order to recognize the
       if-else expression.  However, I've already written the other cases
       you will need.  Do look at this code so you can see what is the
       abstract syntax representations of let bindings and lambda terms.

    5. A compiler that takes an expr tree and compiles code for it
       into am7b+ assembly.  The 'compile7b' function takes three
       arguments: (st,exp,ln) where st is an environ, but which is
       here used as a "symbol table".  This table associates where in
       memory each variable is stored at as well as the starting
       instruction number of each lambda function.  exp is the expr
       tree to be compiled.  ln is the instruction number for the
       first instruction to be generated.  This function should return
       a value of the form (s,lx), where s is a string (with \n to
       break lines) representing the instructions generated and lx is
       the next instruction number.  You must keep track of the
       instruction numbers carefully when generating jump (jz, jnz,
       jn, call) instructions.  The ln and st arguments are not used
       in the base compiler, but will be needed when you compile more
       advanced expressions.

    6. Various AOP-style "advice" intended to locally add features to
       existing code.


                   =======  THE ASSIGNMENT =======

    **************Levels 1-3 are due Tuesday 11/26.****************

This assignment will ask you to extend, modularly as always, this base
program to have more capabilities.  This assignment will be graded by
your demonstrations.  Your work will be graded on the following scale:

1 . To obtain the minimally acceptable grade of GEEK: you must be able to
    interpret (evalutate) boolean expressions and if-else expressions.
    Since the "language" here only has integer as type, we will use 0 for
    false, and anything non-zero for true.  You need to be able to evaluate
    expressions such as (3 eq 1+2), and (2 lt 4-1).  Here, eq and lt are
    "equals" and "less than" respectively.  Once you've implemented these
    booleans, you need to interpret if-else expressions such as
    if (3 lt 2) 3 else 4, which should evaluate to 4, and
    if (0 eq 1-1) 10 else 20, which should evaluate to 10.  Your if
    expression must be implemented to any boolean expression generically,
    not just these two examples (in the future, we may need to add &, |, etc).

    To do this, you must first modify the parser to recognize a parse stack
    of the form   e3::Sym("else")::e2::e1::Sym("if")::cdr.  The parser can
    already recognize boolean expressions and will return exprs such as
    Binop("eq",e1,e2) for them.  You need to change the parser so that
    it creates exprs of the form Ternop("if",e1,e2,e3).

    The only functions you will have to change are parse and eval.

2.  To obtain the better grade of MEGAGEEK, you must meet the
    requirements for GEEK and extend the compiler to compile code for
    boolean and if-else expressions.  Your compiled code should run on
    am7b.  You will have to change the definition of compile7b

    To compile a expr tree, execute a post-order traversal: this 
    will implement call-by-value.  At every node of the tree, code 
    should be generated to push the value of that subtree onto the stack.
    Do not deviate from the stack protocol. The result of compiling any expr 
    must be pushing the result onto the stack.  It becomes immensely 
    more complicated if you don't follow this protocol.  
    You will end up generating a lot of code that looks like:
        push 3
        pop ax
    Do NOT be tempted to generate mov 3 ax instead, because that will
    break the protocol and make your life miserable.  You can optimize
    code like this after the entire program's been generated.

3.  To obtain the moderately impressive grade of GIGAGEEK, you must
    complete the requirements for MEGAGEEK and be able to interpret let
    expressions, which means you will need to evaluate and keep track
    of the scope of variables (Var(x)) in abstract syntax.  You need
    to use the type environ and the function lookupval/pushed (extend
    environment with new binding).  The parser already recognizes let
    expressons and returns abstract syntax in the form  Letexp(x,e1,e2).

    (let x=2:(let x=3:x+x)+x) should evaluate to 8, not 9.

    Note that the lexical analyzer returns exprs like Var("x") for variables
    and Sym("if") for keywords

4.  To obtain the solidly impressive grade of TERAGEEK, you must
    become GIGAGEEK first and then be able to compile let
    expressions. You need to use the "symbol table" argument to
    compile7b: this table maps each variable to a statically allocated
    location in RAM: I used a hp (heap pointer), which is intially set
    to RAM.Length-1, and decrements towars 0 (so the heap and stack
    grow from opposite ends of memory). You should partition memory
    into two halfs: stack and heap. Or, you can modify the execute
    instruction of am7b so that it checks that sp<hp before executing
    each instruction.  Or ask prof for help.

5.  To obtain the exalted grade of PETAGEEK, you must first become
    TERAGEEK, then be able to interpret lambda expressions in let bindings
    and the applications of lambda terms to arguments.
    
       let x=1:(let f=lambda y.x+y:(let x=2:(f 10)))

    should evaluate to 11 because of static scoping.  To implement static
    scoping in an interpreter, the lambda term need to be captured in
    a CLOSURE, which includes its static environment.

    The parser already recognizes and returns expr's of the forms
    Lambda(x,e) and App(f,e)  

    Examples to try:

    let f=lambda x.x+x:(let g=lambda x.x*x:(f (f (g 3))))  // evals to 36
    let log=lambda n.(if (n lt 2) 1 else 1+(log (n/2))):(log 256) // evals to 8


6.  To obtain the ultimate grade of EXAGEEK (extraterrestrial geek),
    you must also compile lambda bindings and applications.  Here, you
    must change the implementations of the CALL and RET
    instructions because the anonymous designer of AM7B forgot to include a
    stack base register (%ebp in x86).  Consult lecture notes and the prof
    will gladly give help.  You must use a function calling protocol
    like the following.  When a function's code starts to execute, the
    stack should look like this:

       saved %ebp  (%ebp will get the value of %esp after save)
       saved pc
       arg passed to function  (always one int, lambdas are typed int->int)

    The value of sp (%esp in x86) is saved in %ebp after the original
    %ebp has been pushed onto the stack: the stack base register records
    where sp was pointing at the very beginning of the function.
    
    When the function returns, the values of %ebp and pc will be
    restored and the return value computed by the function should
    replace the argument passed to the function on the stack.  This
    value should now be top of the stack.  You need to add a new
    variable 'bp' to simulate the %ebp register.  However, since this
    register is not directly accessible from software, you must change
    the implementations of CALL and RET in AM7B to implement the
    entire calling/returning protocol.  This is not typical of
    RISC-style instructions, so it's a compromise.  Alternatively, you
    can change am7b (in a backwards compatible way) to allow sp and bp
    to be accessed directly (if you choose to do this you may change
    my source code for am7b, but ONLY if you choose to do this).

    Inside the body of the lambda term, to access it's argument, you
    will have to access it at RAM.[bp-3] (not sp-3, because sp changes
    while the function runs).  How can you implement this, given that
    we don't have register-index addressing in am7b?  Well, you
    simulate the addressing mode by storing a negative value in bx, so
    then interpret mov [bx] ax with a negative value in bx as loading
    from RAM.[bp+bx]. Then in your updated symbol table you can just
    associate the (sole) lambda argument with -3. Got it? See more
    detailed hints below, but only if you're a true exageek.

    This, top level of the assignment will require you to write about
    500 lines of code in F#, which would be roughly equivalent to 2000 
    lines of java.

7.  There is another level beyond EXAGEEK, but which only exists in
    legend.  To become ZETAGEEK, you must invent your own programming
    language with a fuller set of features, including sequences of
    expressions separated by ";", assignment statements to change
    bindings (this is why environ contains references), and while
    loops.  Also add strings and print statements - in at least the
    interpreter. You can also compile the code into real x86 or x86_64
    assembly.  Look at how gcc -S compiles simple C programs and
    follow the examples.  You can also call printf and other C library
    functions if you generate code that's compatible with gcc.
    Another thing you will need to do is provide better error messages
    with your compiler or interpreter.

    ********
    The following is the base program, which can interpret and compile
    basic arithmetic expressions such as 3+2*4, and correspond to the
    basic am7b (before it was extended to have more instructions).
    To compile:
    fsc/fsharpc compilatorbase.fs -r am7b19.dll (replace with your own .dll)
    To run:
    (mono) compilatorbase.exe interpret
    (mono) compilatorbase.exe compile
*)

module CSC7B
open System;
open Microsoft.FSharp.Math;
open System.Text.RegularExpressions;
open System.Collections.Generic;
open CSC7B;;

//// COMPONENT 1:
////////////////////  Abstract Syntax (not all cases used in this program) ///

// environ and expr must be defined together because a Closure contains
// an environ

type environ = (string*expr ref) list
     and 
     expr = Val of int | Binop of (string*expr*expr) | Uniop of (string*expr) | Var of string | Ternop of string*expr*expr*expr | Seq of (expr list) | Letexp of string*expr*expr | Closure of (environ*expr) | App of string*expr | Lambda of string*expr | Assign of string*expr | Print of expr | Sym of string | EOF ;;

// Basic operations on environments
let rec lookup x (env:environ) =
   match env with
    | [] -> raise(Exception(x+" not found in environment/table"))
    | ((y,rv)::cdr) when x=y -> !rv
    | ((y,rv)::cdr) -> lookup x cdr
let rec change x n (env:environ) =   
   match env with
    | [] -> raise(Exception(x+" not declared in this scope"))
    | ((y,rv)::cdr) when x=y -> rv := n
    | ((y,rv)::cdr) -> change x n cdr;;
let pushed x e env = (x,ref e)::env; // to add binding to environment
let lookupval x env =    // returns int
   let v = lookup x env
   match v with
     | Val(n) -> n
     | _ -> raise(Exception(string(v)+" is not an int"));;
let changeval x n env = change x (Val(n)) env;;   // n is int

//// Note: because it's difficult to extend a discriminated union modularly,
// we are using strings to represent different kinds of expressions, so there
// is a cost to be paid in terms of static type safety.  Although pure F# is
// statically "type safe", there is not much type information available when
// use strings to represent data.  


//// COMPONENT 2: evaluation/interpretation
// use environments of the form [("x",ref Val(0));("y",ref Val(0))] ...

// Because of variable expressions, env represents bindings for variables.
// eval is mutable so we can inject behaviors later...
// mutable funs can't be recursive calls, unless we declare bind them
// to a "dummy" function of the right type:
let mutable eval = fun (env:environ) (exp:expr) -> 0;;
eval <- fun (env:environ) exp ->
  match exp with
    | Val(v) -> v
    | Var(x) -> lookupval x env
    | Binop("+",a,b) -> (eval env a) + (eval env b)  // not Plus(a,b)
    | Binop("*",a,b) -> (eval env a) * (eval env b)  // lose some static safety
    | Binop("-",a,b) -> (eval env a) - (eval env b)
    | Binop("/",a,b) -> (eval env a) / (eval env b)
    | Uniop("-",a) -> -1 * (eval env a)
    | x -> raise (Exception("not supported eval case: "+string(x)));;
////////////////////////////////////////////////////


//// COMPONENT 3:
//////////////////       LEXICAL ANALYSER (LEXER)  // (ignore most here)

///////////////// Lexical Symbol Specification

// use regular expression to represent possible operator symbols:
let mutable operators = "([()\+\-\*/:;%^,.]|\s|\[|\]|=)";;
let mutable keywords = ["if";"then";"else";"while";"let";"eq";"leq";"lt";"print";"begin";"end";"lambda";"def"];

// Reads expressions like  "7+3*2" and convert to list of symbols like
// [|Val(7.0);Sym("+");Val(3.0);Sym("*");Val(2.0);EOF|];
//// This is the first stage of parsing, called lexical analysis or token
// analysis

// assume string is in input_string

// now build list of tokens
let maketoken x =  try Val(int(x))   // exception handling in F#
                   with
                   | excp ->
                      match x with
                       | y when (List.contains y keywords) -> Sym(y)
                       | y when int(y.[0])>96 && int(y.[0])<123 ->  Var(y)
                       | y -> Sym(y);;

let tokenize (s2:string[]) = 
  let rec itokenize ax = function   // inner tail-recursive function
    | i when i>=0 -> 
       let t = s2.[i].Trim()
       if (t<>"") then 
         itokenize (maketoken(s2.[i])::ax) (i-1)
       else
         itokenize ax (i-1)
    | _ -> ax;
  itokenize [EOF] (s2.Length-1);;

let mutable TS =[];; // global list of tokens
let mutable TI = 0;; // global index for TS stream;;

let mutable input_string = "";; // default input string, GLOBAL!

///// The following function takes an input string and sets global
// variable TS, which is a stream of tokens (see commented example above
// for (7+3*2)).  It also sets TI, which is a global index into TS.
let mutable lexer = fun (inp:string) ->  // main lexical analysis function
  let s2 = Regex.Split(inp,operators)
  TS <- tokenize s2
  TI <- 0  // reset if needed
//  printfn "token stream: %A" TS;;


///////////////////


///// COMPONENT 4
////////////////////////// SHIFT-REDUCE PARSER ////////////////////////
(*  Ambiguous context-free grammer:

    E := var of string  |  
         val of float   |
         E + E          |
         E * E          |
         E - E          |
         E / E          |
         (E)            |
         - E;           // unary minus - reduce-reduce precedence
                        //( will have highest precedence - reduce, don't shift
	 if E E else E  |
	 let string=lambda string.E:E |
	 let string = E:E  |
*)


let mutable binops = ["+";"*";"/";"-";"%";"^";"eq";"lt";"leq";"while";"assign"];
let mutable unaryops = ["-"; "!"; "~"];
let mutable ternaryops = ["if"];;

// use hash table (Dictionary) to associate each operator with precedence
let prectable = Dictionary<string,int>();;
prectable.["+"] <- 200;
prectable.["-"] <- 300;
prectable.["*"] <- 400;
prectable.["/"] <- 500;
prectable.["("] <- 990;
prectable.[")"] <- 20;
prectable.[":"] <- 20;
prectable.["="] <- 30;
prectable.["."] <- 20;
prectable.["_"] <- 600;
prectable.["eq"] <- 35;
prectable.["leq"] <- 35;
prectable.["lt"] <- 35;
prectable.["if"] <- 20;
prectable.["else"] <- 18;
prectable.[";"] <- 20;

// function to add new operator (as regex string) with precedence (int)
let newoperator (s:string) prec =
  let n = operators.Length
  let prefix = operators.Substring(0,n-1)
  operators <- prefix + "|" + s + ")"
  if s.[0]='\\' then prectable.[s.Substring(1,s.Length-1)] <- prec
  else prectable.[s] <- prec;;

//sample usage of newoperator function:
//newoperator @"&&" 650;;  // use @ before string or use "\^" (explict escape)
//Console.WriteLine(string(prectable.["&&"]));;  // check if success
//newoperator "^" 600;;   // @"^" didn't work - don't know why
// need newoperator "\?" precedence level
// need newoperator "%" precedence level
// Also change binops, unaryops, if neccessary:
unaryops <- "print"::unaryops;;  // add print(expr) as a unary operation

/// Lexical Token stream and abstract syntax combined into expr, the
//  following will try to distinguish them.
// Proper expression check (shallow): separates proper expression from tokens
// This is the price to pay for using strings: no compile-time verification
let mutable proper = fun f ->
  match f with
    | Binop(s,_,_) when (List.exists (fun x->x=s) binops)  -> true
    | Uniop(s,_) when (List.exists (fun x->x=s) unaryops) -> true
    | Ternop("let",Var(x),_,_) -> true
    | Ternop(s,_,_,_) when (List.exists (fun x->x=s) ternaryops) -> true
    | Binop(_,_,_) -> false;
    | Uniop(_,_) -> false;
    | Sym(_) -> false
    | EOF -> false
    | _ -> true;;


// function defines precedence of symbol, which includes more than just Syms
let mutable precedence = fun s ->
  match s with
   | Val(_) -> 100
   | Var(_) -> 100
   | Sym(s) when prectable.ContainsKey(s) -> prectable.[s]
   | EOF    -> 10
   | _ -> 11;;

// Function defines associativity: true if left associative, false if right...
// Not all operators are left-associative: the assignment operator is
// right associative:  a = b = c; means first assign c to b, then b to a,
// as is the F# type operator ->: a->b->c means a->(b->c).

let mutable leftassoc = fun e ->
  match e with
   | _ -> true;  // most operators are left associative.


// check for precedence, associativity, and proper expressions to determine
// if a reduce rule is applicable.
let mutable checkreducible = fun (a,b,e1,e2) ->
  let (pa,pb) = (precedence(a),precedence(b))
  ((a=b && leftassoc(a)) || pa>=pb) && proper(e1) && proper(e2);;

// parse takes parse stack and lookahead; default is shift

////////////////// HERE IS THE HEART OF THE SHIFT-REDUCE PARSER ////////
let mutable parse = fun (x:expr list,expr) -> EOF // dummy for recursion
parse <- fun (stack,lookahead) ->
  match (stack,lookahead) with
   | ([e],EOF) when proper(e) -> e   // base case, returns an expression
   | (Sym(")")::e1::Var(f)::Sym("(")::t,la) when int(f.[0])>96 && int(f.[0])<123 && checkreducible(Sym("_"),la,e1,e1) ->
     let e = App(f,e1)
     parse(e::t,la)
   | (Sym(")")::e1::Sym("(")::t, la) when checkreducible(Sym("("),la,e1,e1) ->
        parse (e1::t,la)
   | (e2::Sym(op)::e1::cdr,la)   // generic case for binary operators
     when (List.exists (fun x->x=op) binops) && checkreducible(Sym(op),la,e1,e2) ->
        let e = Binop(op,e1,e2)
        parse(e::cdr,la)
   | (e1::Sym("-")::t, la) when checkreducible(Sym("-"),la,e1,e1) ->  // "rrc"
        let e = Uniop("-",e1)
        parse (e::t,la)
   | (e2::Sym(":")::e1::Sym("=")::Var(x)::Sym("let")::t,la) when checkreducible(Sym(":"),la,e1,e2) -> 
        let e = Letexp(x,e1,e2) //let expressions
        parse(e::t,la)
   | (e1::Sym(".")::Var(x)::Sym("lambda")::t,la) when checkreducible(Sym("."),la,e1,e1) ->
        let e = Lambda(x,e1)
        parse(e::t,la)
   | (st,la) when (TI < TS.Length-1) ->  // shift case
        TI <- TI+1;         
        let newla = TS.[TI]
        parse (la::st,newla)
   | (st,la) ->
        raise (Exception("parsing error: "+string(la::st)));;
/////////////////////////////////


//// COMPONENT 5
///////////////////////////// BASE COMPILER /////////////////////
//// limited compiler:
let transinst = function
  | "+" -> "add"
  | "-" -> "sub"
  | "*" -> "imul"
  | "/" -> "idiv"
  | x -> x

//let mutable hp = RAM.Length-1;   // heap pointer register
//let mutable sbp = sp;            // stack base pointer register (ebp in x86)
// keep invariant hp>sp

//let mutable HEAPMAX = 1024-32;

// compile7b must compile expression and take next line number,
// returns next line number, initial line number should be 0
let mutable compile7b = fun (st:environ,exp:expr,line:int) -> ("",0) // dummy
// must return large string
let addi(s:string,inst) = s+inst+"\n"; // for readability

//compiler take as arguments: symbol table (environ), expression, next line num.
compile7b <- fun (st:environ,exp,ln) ->
  match exp with
    | Val(x) -> (sprintf "push %d\n" x),ln+1 //=("push "+string(x)+"\n",ln+1
    | Binop(op,a,b) when (transinst op)<>op ->
        let opr = transinst op
        let (aa,ln2) = compile7b(st,a,ln)
        let (bs,ln3) = compile7b(st,b,ln2)
        let mutable ops = aa+bs+ "pop bx\n" 
        ops <- ops + "pop ax\n"
        ops <- ops + opr+" bx ax\n"
        ops <- ops+"push ax\n"
        (ops,ln3+4)
    | Uniop("-",e) ->
        let (es,lne) = compile7b(st,e,ln)
        let mutable s = addi(es,"mov -1 bx")
        s <- addi(s,"pop ax")
        s <- addi(s,"imul bx ax")
        s <- addi(s,"push ax")
        (s,lne+4)
    | x -> (sprintf "# compilation of %A not supported at this time\n" x),ln;;
//compile7b returns (instructions-string,next-line-number)


(*   ************** Hints for EXAGEEKS: *************
                 earthlings need not look.

// using sbp instead of bp...
//let fcexecute = execute;  // already in amfc19
execute <- fun inst ->
 match inst with
  | MOV(Mem(Reg d),(Reg s)) when REGS.["bx"]<0 && d="bx" ->
      REGS.[s] <- RAM.[sbp+REGS.[d]]
  | CALL(target) when sp+1<hp ->
      RAM.[sp] <- pc
      RAM.[sp+1] <- sbp  // saves previous stack pointer
      sp <- sp+2
      sbp <- sp 
      pc <- target-1
  | RET when sp-4 >= 0 ->
      RAM.[sbp-3] <- RAM.[sp-1]  // copy ret value to where actual arg was
      sp <- sbp   // now tos holds old sbp
      sbp <- RAM.[sp-1]  // pop %ebp
      pc <- RAM.[sp-2]
      sp <- sp-2   // tos now holds return value of function
  | x when sp+1<hp -> fcexecute x
  | _ -> raise(Exception("Out of RAM"));;
  // These are not RISC-like call and ret instruction, so that's a compromise

   Call/Ret should be simpler: in x86, a procedure begins with the
   instructions

     pushl %ebp   // saves current value of ebp
     movl  %esp, %ebp  // ebp holds value of sp at start of procedure

   and the procedure ends with

     popl %ebp    // restore previous value of ebp
     ret

   In am7b (extended), the sbp "register" corresponds to %ebp but
   unlike x86, here it's not under software control.  Thus, we must
   fold these operations into the simulations of the call/ret instructions.

   From the standpoint of security, it would appear to be better if
   certain parts of the hardware is not directly accessible from
   software, but easier said than done ...

   Therefore, our CALL/RET instructions do not have the characteristics
   of RISC-like instructions in that they clearly cannot be completed
   in one or two clock cycles.

Here's a sample program I was able to compile with my program: it
computes 3!! = 6! = 720:

# procedure f:
mov 0 cx
jz 36
mov -3 bx
mov [bx] ax
push ax
push 2
pop bx
pop cx
sub bx cx
jn 13
push 0
mov 0 cx
jz 14
push 1
pop cx
jz 19
push 1
mov 0 cx
jz 35
mov -3 bx
mov [bx] ax
push ax
mov -3 bx
mov [bx] ax
push ax
push 1
pop bx
pop ax
sub bx ax
push ax
call 2
pop bx
pop ax
imul bx ax
push ax
ret
push 3
call 2
call 2
nop
# compiled for: let f=lambda n.if (n lt 2) 1 else n*(f (n-1)):(f (f 3))
*)

//// COMPONENT 6: AOP Advice
////////////////////////

////// Advice to handle exceptions gracefully
let mutable advice_errhandle = fun (target:unit->unit) ->
  let proceed_parse = parse
  let proceed_eval = eval
  parse <- fun(st,la) ->
     try proceed_parse(st,la) with
       | exc ->
           Console.WriteLine("parse failed with exception "+string(exc))
           exit(1)
  eval <- fun env e ->
     try (proceed_eval env e) with
       | exc ->
           Console.Write("eval failed with exception "+string(exc))
           Console.WriteLine(", returning 0 as default...")
           0
  target()  // execute target
  eval <- proceed_eval      
  parse <- proceed_parse;; // restore originals before exit
// advice_errhandle

/// Note that these advice functions group together code that "crosscut" 
// the conventional function-oriented design to be oriented instead towards
// certain "aspects" of the program (tracing, error handling).

///// Advice to read from stdin with prompt:
let mutable advice_io = fun (prompt, target:unit->unit) ->
    if prompt then Console.Write("Enter expression: ");
    input_string <- Console.ReadLine()
    lexer(input_string)
    target();;
//advice_io doesn't need to modify any functions, just inject before target

// advice to trace before/after parse, eval and compile,
let mutable traceopt = fun (before,after,target:unit->unit) ->
  let proceed_compile = compile7b   // simulate dynamic scoping
  let proceed_eval = eval
  let proceed_parse = parse
  eval <- fun env e ->
     if before then printfn "evaluating %A under env %A" e env
     let v = proceed_eval env e
     if after then printfn "  eval returned %d" v
     v  // return
  compile7b <- fun (st,e,ln) ->
     if before then printfn "compiling %A under symtab %A at line %d" e st ln
     let (s,lnn) = proceed_compile(st,e,ln) 
     if after then printfn "  compiler returned %A" (s,lnn)
     (s,lnn)
  parse <- fun(st,la) ->
     if before then printfn "parsing %A with lookahead %A" st la
     let e = proceed_parse(st,la)
     if after then printfn "  parse returned expression %A" e
     e //return
  target()  // execute target operation
  eval <- proceed_eval      
  compile7b <- proceed_compile;; // restore originals before exit  


//// main execution function
let mutable run = fun () ->
  let ee = parse([],TS.[0])
  //  let mutable Bindings = Base(Dictionary<string,int>())
  let mutable Bindings:environ = []
  let v = eval Bindings ee
  printf "\nValue of %s = %d\n" input_string v;;

let mutable interpret = run;  //alias for run

let mutable compile = fun () ->
   let exp = parse([],TS.[0])
   let prog,ln = compile7b([],exp,0)
   Console.WriteLine(prog+"nop");  
   Console.WriteLine("# compiled for: "+input_string);;

//////
let runit() = 
  let argv = Environment.GetCommandLineArgs();
  if argv.Length>1 && argv.[1] = "compile" then
     advice_io(false,compile); // advice to compile  (send output to stdout)
  else 
     advice_io(true,interpret);;  // advice to interpret

// run with before-trace option (trace call but not return):
//traceopt(true,true,runit);;
runit();; // run without advice

//// .dll was compiled with above line commented out.
// please put both module CSC7B and open CSC7B at the top of your program
// like here.  If you want all definitions to be visible, then compile with
// multiple .dlls:  fsharpc program.fs -r first.dll -r second.dll ...
////////// run (mono) compilatorbase.exe compile/interpret

