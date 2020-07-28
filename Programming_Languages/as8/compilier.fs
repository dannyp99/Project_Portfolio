module CSC7B
open System
open CSC7B

// let mutable traceopt = fun (before,after,target:unit->unit) ->
//   let proceed_compile = compile7b   // simulate dynamic scoping
//   let proceed_eval = eval
//   let proceed_parse = parse
//   eval <- fun env e ->
//      if before then printfn "evaluating %A under env %A" e env
//      let v = proceed_eval env e
//      if after then printfn "  eval returned %d" v
//      v  // return
//   compile7b <- fun (st,e,ln) ->
//      if before then printfn "compiling %A under symtab %A at line %d" e st ln
//      let (s,lnn) = proceed_compile(st,e,ln) 
//      if after then printfn "  compiler returned %A" (s,lnn)
//      (s,lnn)
//   parse <- fun(st,la) ->
//      if before then printfn "parsing %A with lookahead %A" st la
//      let e = proceed_parse(st,la)
//      if after then printfn "  parse returned expression %A" e
//      e //return
//   target()  // execute target operation
//   eval <- proceed_eval      
//   compile7b <- proceed_compile;; // restore originals before exit  

let base_eval = eval
eval <- fun (env:environ) exp ->
    match exp with
        | Binop("lambda", Closure(clenv, Lambda(x, e1)), App(x2,e2)) -> 
            let mutable newenv = pushed x (Val(eval env e2)) clenv
            newenv <- pushed x2 (Closure(clenv, Lambda(x,e1))) newenv
            eval newenv e1
        | App(x,e1) ->
            let val1 = lookup x env
            eval env (Binop("lambda", val1, App(x, e1)))
        | Letexp (x, Lambda(x1,e1), e2) ->
            let newenv = pushed x (Closure(env, (Lambda(x1, e1)))) env
            eval newenv e2
        | Letexp (name,value,e1) -> 
            let newenv = pushed name (Val(eval env value)) env
            eval  newenv e1
        | Binop ("eq",a,b) -> if (eval env a) = (eval env b) then 1 else 0
        | Binop ("lt",a,b) -> if (eval env a) < (eval env b) then 1 else 0
        | Binop ("leq",a,b) -> if (eval env a) <= (eval env b) then 1 else 0
        | Ternop ("if",a,b,c) -> if (eval env a) = 1 then (eval env b) else (eval env c)
        |_->base_eval env exp

let base_parse = parse
parse <- fun (stack,lookahead) ->
    match (stack,lookahead) with
        | (e3::Sym("else")::e2::e1::Sym("if")::cdr,la) when e3<>Sym("(") ->
            parse(Ternop("if",e1,e2,e3)::cdr,la)
        |_->base_parse (stack,lookahead)

let mutable hp = 1023
let base_compile7b = compile7b
compile7b <- fun (st:environ,exp,ln) ->
    match exp with
        | Var (x) ->
            let px = lookupval x st
            let mutable ops  = sprintf "mov %d bx\n" px
            ops <- addi(ops,"mov [bx] ax")
            ops <- addi(ops, "push ax")
            (ops,ln+3)
        | Binop(op,a,b) when (transinst op)=op ->
            let (aa,ln2) = compile7b(st,a,ln)
            let (bb,ln3) = compile7b(st,b,ln2)
            let mutable ops = aa
            ops <- ops + bb
            ops <- ops + "pop bx\n"
            ops <- ops + "pop cx\n"
            ops <- ops + "sub bx cx\n"
            ops <- ops + "jnz " + string(ln3+6) + "\n"
            ops <- ops + "push 1\n"
            ops <- ops + "jnz " + string(ln3+7) + "\n"
            ops <- ops + "push 0\n"
            (ops,ln3+7)
        | Ternop ("if",c,a,b) ->
            let (cc,ln2) = compile7b(st,c,ln)
            let (aa,ln3) = compile7b(st,a,ln2+2)
            let (bb,ln4) = compile7b(st,b,ln3+2)
            let mutable ops = cc
            ops <- ops + "pop cx\n"
            ops <- ops + "jz " + string(ln3+3) + "\n"
            ops <- ops + aa
            ops <- ops + "mov 0 cx\n"
            ops <- ops + "jz " + string(ln3+2) + "\n"
            ops <- ops + bb
            (ops,ln4)
        | Letexp (name,e1,e2) ->
            let (aa,ln2) = compile7b(st,e1,ln)
            let mutable ops = addi(aa, "pop ax")
            ops <- addi(ops, sprintf "mov %d bx" hp)
            ops <- ops + "mov ax [bx]\n"
            let newenv = pushed name (Val(hp)) st
            if hp < 0 then raise (Exception("Heap Space Error Download more RAM!!!"))
            hp <- hp - 1
            let (bb,ln3) = compile7b(newenv,e2,ln2+3) 
            ops <- ops + bb
            (ops,ln3)
        |_-> base_compile7b (st,exp,ln)

let argv = Environment.GetCommandLineArgs()
if argv.Length > 1 && argv.[1] = "compile" then
    //advice_io(false,compile)
    traceopt(true,false,runit)
    traceopt(false,false,runit)
else
    //advice_io(true,interpret)
    traceopt(true,true,runit)
