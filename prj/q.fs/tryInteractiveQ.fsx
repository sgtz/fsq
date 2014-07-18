// currying

//   q t
// or
//   q (s,[x[,y[,z]]]]), where q is the verb, s is a string, and t is a tuple

#r @"bin\Debug\CLX.SYS.IO.q.dll"
#r @"bin\Debug\CLX.SYS.IO.q.fs.dll"
#r @"bin\Debug\CLX.SYS.IO.q.fmt.dll"

open CLX.SYS.IO
open CLX.SYS.IO.Q
#load "QFsiConsole.fs"
open UtilM

let c = new c("", 5001)
let Q x = Q c x
let q t = c.q(t)      // k: send and return (synchronous)
let qs t = c.qs(t)    // k: send  (asynchronous)
let qr t = c.qr(t)    // k: synchronous.  No return 

Q "til 10"


q("{til x}",5)
q("{x+til y}",5,6)

Q("{x+til y}",5,6)

