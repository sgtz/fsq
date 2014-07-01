namespace CLX.SYS.IO.Q
open System
open System.Reflection
open Microsoft.FSharp.Reflection
open CLX.SYS.IO.Q
open qutil

module UtilM = 

  let Q0_ (cx:c) x hideCmd =
    let R = cx.Q0 x hideCmd
    let mutable i = 0
    for line in R do
      //printf "%s" (if i=0 then c.SameLineify(line) else line)   // FSI must throw the first \n in, so do not try to strip it out again.
      printf "%s" line
      i <- i+1
    printf "%s" <| String.replicate 200 " "

  let Q1_ (_c:c) x=
    Q0_ _c x false

  /// My Q functions.  Used to distinguish my annotation and exercise extensions
  let MQ x=Q1_ x
  let QP (cx:c) x=
    printf "%s" <| cx.WriteToFSI x
    printf "%s" <| String.replicate 160 " "
  /// Do not execute, and do not print to FSI the console.  
  let QC x=
    printf "%s" <| String.replicate 160 " "
  /// execute, but do not print to the FSI console
  let QX (cx:c) x=
    cx.Q x |> ignore

  let Q (cx:c) t = 
    let A = tupleToList t
    let cnt = A.Length - 1
    let s = (A.[0] :?> string)

    let P = [(if cnt>=1 then Some(convertFsType A.[1]) else None);(if cnt>=2 then Some(convertFsType A.[2]) else None);(if cnt>=3 then Some(convertFsType A.[3]) else None)]

    match P with
    | [None;None;None] -> Q1_ cx (s)
    | [Some x;None;None] -> Q1_ cx (s,x)
    | [Some x;Some y;None] -> Q1_ cx (s,x,y)
    | [Some x;Some y;Some z] -> Q1_ cx (s,x,y,z)
    | _ -> failwith "`nyi"
    // TODO: !!! MOVE THIS FUNCTION TO misc.fs.  Let member private this.doQ_output branch to this function when it needs to.

