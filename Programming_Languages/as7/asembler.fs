open CSC7B;
open System;

let baseTransoperand = transoperand
transoperand <- fun n -> 
    match n with
        | "[bx]" -> Mem(Reg("bx"))
        | _ -> baseTransoperand n

let baseExec = execute
execute <- fun m ->
    match m with
        | MOV(Imm(a), Reg(b)) ->
            REGS.[b] <- a
        | MOV(Reg(a),Reg(b)) ->
            REGS.[b] <- REGS.[a]
        | MOV(Reg(a), Mem(b)) ->
            RAM.[REGS.["bx"]] <- REGS.[a]
        | MOV(Mem(a), Reg(b)) ->
            REGS.[b] <- RAM.[REGS.["bx"]]
        | MOV(Mem(a), Mem(b)) ->
            raise(Exception("Go back to CSC110 you don't know basic memory."))
        | JNZ(x) -> if REGS.["cx"] <> 0 then pc <- x - 1
        | JZ(x) -> if REGS.["cx"] = 0 then pc <- x - 1
        | JN(x) -> if REGS.["cx"] < 0 then pc <- x - 1
        | ALU(op,Reg(a),Reg(b)) when op="idiv" ->  // can't handle idiv yet
            let f = aluop(op)  // get function 
            let g = aluop("rem")
            REGS.["dx"] <- g(REGS.[a],REGS.[b])
            REGS.[b] <- f(REGS.[a],REGS.[b])
        | ALU(op,Imm(a),Reg(b)) ->
            let f = aluop(op)
            let g = aluop(op)
            REGS.["dx"]<- g(a,REGS.[b])
            REGS.[b] <- f(a,REGS.[b])
            REGS.["cx"] <- REGS.[b]
        | CALL(x) when sp<RAM.Length ->
            RAM.[sp] <- pc
            sp <- sp+1
            pc <- x-1
        | RET ->
            pc <- RAM.[sp]
            sp <- sp - 1
        | _ -> baseExec m

let base_aluop = aluop
aluop <- fun m ->
    match m with
        | "rem" -> fun(x,y) -> y%x
        | _-> base_aluop m

let base_trans = translate
translate <- fun ary ->
    match ary with
        | [|"mov"; x; y |] -> MOV(transoperand x, transoperand y)
        | [|"jnz"; x |] -> JNZ(int(x))
        | [|"jz"; x |] -> JZ(int(x))
        | [|"jn"; x |] -> JN(int(x))
        | [|"call"; x |] -> CALL(int(x))
        | [|"ret"|] -> RET
        | _-> base_trans ary

let memdump_advice(k,target) =
    let storefun = executeProg // save execute function on stack
    executeProg <- fun inst ->
        try 
            Console.WriteLine("executing instruction: "+string(inst))
            storefun inst  // run original execute
            let mutable run = 0
            while run < k do 
                printfn "RAM Address Value: %d" RAM.[run]
                run <- run + 1
        with
            | exc -> (Console.WriteLine(exc); exit(1)) // catch exception
    target();  // execute target code under local dynamic scope
    executeProg <- storefun;;   // exit dynamic scope, restore original function

type opinion = Trace of opinion | Mem of int*opinion | Run of unit

let rec weave = function
    | Trace(n) -> trace_advice(fun() -> weave(n))
    | Mem(x,n) -> memdump_advice(x, (fun() -> weave(n)))
    | Run() -> run()

//trace_advice(run);;
weave(Trace(Run()));;
