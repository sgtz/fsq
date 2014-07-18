// Experimental syntax

// an F# form that is similar to q s t where q is the verb , s is the string, and t is a tuple of ([x[,y[,z]]]])
// eg.
// q "{til 5}" ()
// q "{til x}" (5)
// q "{x+til y}" (5,6)
// q "{x*y+til z}" (5,6,7)

#r @"bin\Debug\CLX.SYS.IO.q.dll"
#r @"bin\Debug\CLX.SYS.IO.q.fs.dll"
#r @"bin\Debug\CLX.SYS.IO.q.fmt.dll"

open CLX.SYS.IO
open CLX.SYS.IO.Q
#load "QFsiConsole.fs"
open UtilM

let c = new c("", 5001)

let Q x = exp_Q c x

let q s t = c.exp_q s t      // k: send and return (synchronous)
let qs s t = c.exp_qs s t    // k: send  (asynchronous)
let qr s t = c.exp_qr s t      // k: synchronous.  No return 

let _0N = c.NULL('i')  // hacky -- could be any value

q "{til x}" (5)
q "{x+til y}" (5,6)

Q "{x+til y}" (5,6)

q "{til 5}" _0N           // no such thing as an empty tuple ie. q "{til 5}" ()
q "{x+til y}" (5,6)
q "{x*y+til z}" (5,6,7)


