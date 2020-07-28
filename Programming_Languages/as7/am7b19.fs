open CSC7B
(*    CSC 7B/FC Sample Program and Assignment (version 7E3)

   Abstract Machine AM7B is a simple computer with 4K (4096) memory that 
   can only be used as a stack.  The machine's CPU contains four general
   purpose registers, ax, bx, cx and dx, special purpose register sp
   (top of stack), an program counter register pc.  Word size is 32
   bits (so there are 1024 memory locations). The ALU can perform
   operations add, sub, imul and idiv.  The semantics of an
   instruction such as  (sub ax bx)  is bx -= ax: the second operand is
   also the destination operand.  There is an exception with the integer
   division instruction: (idiv ax cx) will leave the quotient in cx and
   remainder in dx.  The only other instruction currently implemented is
   'nop', which does nothing (but it plays a role).

   The only other operations (currently) are push and pop. Operands
   can be register or immediate, and only the push operation can take
   an immediate (constant) operand.

   For example, the following program calculates 5+4*3:

   push 3
   push 4
   pop ax
   pop bx
   imul bx ax
   push 5
   pop bx
   add bx ax
   push ax    // leave result on top of stack

AM7B programs should always leave the final result on top of the stack.

This program implements A FRAGMENT OF AM7B in F#, including assembler.
Your assignment would be to complete the implementation.
*)

open System;
open System.Collections.Generic;;
open Microsoft.FSharp.Math;
open System.Text.RegularExpressions;;

// operands are either immediate or register  (Mem is for future expansion)
type operand = Imm of int | Reg of string | Mem of operand;;

type operation = ALU of (string*operand*operand) | PUSH of operand | POP of operand | MOV of operand*operand | JNZ of int | JZ of int | JN of int | CALL of int | RET | NOP;;

// an operation such as 'add ax bx' is represented internally as 
// ALU("add",Reg("ax"),Reg("bx"))
// A program represented by a list of such instructions, 
// which can be pattern matched.

// Currently the program only implements PUSH, POP and ALU operations, but
// the others are listed for later expansion (I will address how they can
// be added modularly later, which would require 'active patterns').


////////////////////// Machine Simulation //////////////////////

let RAM:int[] = Array.zeroCreate 1024;  // 4K memory, only used for stack
let mutable sp = 0;;                 // top of stack register (addresses RAM)
// sp points to next available slot on stack, so 0 means empty stack.
let mutable pc = 0;;                 // program counter register
let mutable fault = false;;          // machine fault flag
let REGS = Dictionary<string,int>();;  // simulate general registers
REGS.["ax"] <- 0   // initially all 0's
REGS.["bx"] <- 0
REGS.["cx"] <- 0
REGS.["dx"] <- 0;;
//REGS.["pc"] <- 0     // these registers should be outside of software control
//REGS.["sp"] <- 0     // so don't add to this table

// but this means exception thrown might not be what you want...
let getreg name =
   try REGS.[name]
   with
     | :? KeyNotFoundException as ke ->
        raise (Exception("no such register: "+name));
let setreg(name,v) =
   try (REGS.[name] <- v)
   with
     | :? KeyNotFoundException as ke ->
        raise (Exception("no such register: "+name));;

// ALU operations dispatch to lambda terms:
let mutable aluop = fun n ->
  match n with
    | "add" -> fun (x,y) -> y+x
    | "sub" -> fun (x,y) -> y-x  
    | "imul" -> fun (x,y) -> y*x
    | "idiv" -> fun (x,y) -> y/x
    | x   -> raise(Exception("illegal ALU operation: "+x));;

(*  The above function maps alu operations "add", "sub", etc to
    lambda terms that perform the corresponding calculuations.  There are
    two things to note here:
     1.  We could have also used a Dictionary<string,int*int->int>, but
         it would again be harder to control exceptions.
     2.  Unlike previous functions, this function is written as a mutable
         lambda term, so it can be changed.  That is, we can assign
	 'aluop' to another lambda term.  Why would we want to do that?
	 Because we may want to modify it later if we want to add additional
	 instructions, like this:

  let base_aluop = aluop  // save copy of original before changing it
  aluop <- fun n ->   // assign aluop to new lambda term
    match n with
      | "cmp" -> fun(x,y) -> if (x=y) then 1 else 0 // compare operation
      | z -> base_aluop z;;  // differ to original function otherwise

Now aluop recognizes one more operation, plus the original ones.  But we
did this without having to edit the original code. Since aluop is a global
definition, all functions that call aluop are also changed.  This is
basically simulating the dynamic scoping of aluop (see more extensive
example at the end).
*)

// core dump prints current snapshot of registers plus top portion of stack.
let coredump() =
  printfn "MACHINE FAULT, CORE DUMPED, HA HA HA!"
  printfn "ax=%d, bx=%d, cx=%d, dx=%d" (REGS.["ax"]) (REGS.["bx"]) (REGS.["cx"]) (REGS.["dx"])
  printfn "sp=%d,  top portion of stack:\n" sp
  let mutable i = sp-1
  while (i>=0 && i>sp-8) do
     printfn "%d" (RAM.[i])
     i <- i-1;;
     

///////// *** KEY FUNCTION *** ...
// The following implements some of the operations of AM7B, but not all...

// Execute a single operation - note execute is also a mutable lambda function
let mutable execute = fun instruction ->
 match instruction with
  | ALU(op,Reg(a),Reg(b)) when op<>"idiv" ->  // can't handle idiv yet
      let f = aluop(op)  // get function 
      REGS.[b] <- f(REGS.[a],REGS.[b])
  | PUSH(Imm(x)) when sp<RAM.Length ->  // push immediate, as in push 3
      RAM.[sp] <- x
      sp <- sp+1
  | PUSH(Reg(r)) when sp<RAM.Length ->  // push register r contents
      RAM.[sp] <- REGS.[r]
      sp <- sp+1
  | POP(Reg(r)) when sp>0 ->            // pop into register r
      sp <- sp - 1
      REGS.[r] <- RAM.[sp]
  | NOP -> ()    // () is of type unit, and does nothing
  | x -> (fault<-true; Console.WriteLine("fault on "+string(x)); coredump());;


//Execute a program as a list of operations. Note how pc is incremented
//so any jump instruction must be written carefully.
let mutable executeProg = fun (program:operation list) ->
  pc <- 0
  fault <- false
  while not(fault) && pc<program.Length do
    let instruction = program.[pc];
    execute instruction
    pc <- pc+1
  // end of while by indentation
  if not(fault) then printfn "top of stack holds value %d" (RAM.[sp-1]);

///////////////////////////// ASSEMBLER/SIMULATION /////////////////////////

//let ins = Console.ReadLine(); // read instruction into string format
//let ins = "add ax bx"
//let tokens = Regex.Split(ins,"\s"); // split string by white spaces
// tokens is of type string[], a .Net array of Strings

// translate a string into an operand:  (note mutable lambda)
let mutable transoperand = fun x -> 
  try Imm(int(x))    // try-with block  exception handling
  with
  | exce -> Reg(x);;   // operand is immediate or memory, but so far
                       // memory operand [bx] not recognized.

// translate a string token array into an AM7B instruction
let mutable translate = fun ary ->   // another mutable lambda
  match ary with
    | [| "push"; x |] -> PUSH(transoperand x)
    | [| "pop"; x |] -> POP(transoperand x)
    | [| op; x; y |] -> ALU(op, transoperand x, transoperand y)
    | [| "nop" |] -> NOP     
    | i -> raise(Exception("Unrecognized instruction: "+string(i)));

let assemble i = translate (Regex.Split(i,"\s"));;

// test that it works
let i1 = assemble "add ax bx";
let i2 = assemble "push 3";
let i3 = assemble "pop cx";
//Console.WriteLine("---");

/// read am7b program from stdin, assemble and execute:
let rec readprog ax (ins:string) =
  match ins with
    | null -> ax
    | n when n.Length=0 || n.[0] = '#'  -> // skip blank or comment line
       readprog ax (Console.ReadLine())
    | ist ->
       let inst = ist.Trim()
       readprog (assemble(inst)::ax) (Console.ReadLine());;

let rec reverse ax = function
  | [] -> ax
  | (a::b) -> reverse (a::ax) b;;

// read file from stdin, assemble and execute:
let run() =   
  let firstline = Console.ReadLine();
  let instructions = reverse [] (readprog [] firstline);
  executeProg(instructions) // better if run under advice:


// When we run a program we want to have the option of tracing.
// we can do this by temporarily modifying the execute function to
// print the instruction being executed as well as a snapshot of the
// registers.  We can do this in the form of an "advice", which is
// something that *injects* extra code into existing code, a concept from
// AOP (aspect oriented programming).  We can write code that temporarily
// changes the execute function by simulating *dynamic scoping* of the function.

// AOP style code enhancement with simulated dynamic scoping of functions
let trace_advice (target:unit->unit) =
   let storefun = execute // save execute function on stack
   execute <- fun inst ->
     try 
       Console.WriteLine("executing instruction: "+string(inst))
       storefun inst  // run original execute
       printfn "ax=%d bx=%d cx=%d dx=%d sp=%d pc=%d" (REGS.["ax"]) (REGS.["bx"]) (REGS.["cx"]) (REGS.["dx"]) sp (pc+1)
     with
       | :? KeyNotFoundException as ke ->
          Console.Write("no such register: ")
          Console.WriteLine(ke)
       | exc -> (Console.WriteLine(exc); exit(1)) // catch exception
   target();  // execute target code under local dynamic scope
   execute <- storefun;;   // exit dynamic scope, restore original function

// reset machine counters before running program
pc <- 0;
sp <- 0;

// This line should be commented out in order to produce the .dll:
trace_advice( run );;   // execute target function 'run' under advice

// Run with (mono) machine.exe < test1.7b

