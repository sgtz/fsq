#r @"bin\Debug\CLX.SYS.IO.q.dll"
#r @"bin\Debug\CLX.SYS.IO.q.fs.dll"
#r @"bin\Debug\CLX.SYS.IO.q.fmt.dll"

open CLX.SYS.IO
open CLX.SYS.IO.Q
#load "QFsiConsole.fs"
open UtilM

let c = new c("", 5001)

let Q x = Q c x

Q "til 10"

let q t = c.q(t)      // k: send and return (synchronous)
let qs t = c.qs(t)    // k: send  (asynchronous)
let qr t = c.qr(t)    // k: synchronous.  No return 

q("{til x}",5)
q("{x+til y}",5,6)

Q("{x+til y}",5,6)

// Q "{x+til y}" 5 6  // it would be nice to enable a flexible style along these lines, but there's no overloading in F#


// TODO: create an F# form that is similar to q s t where q is the verb , s is the string, and t is a tuple of ([x[,y[,z]]]])
// ie.
// q "{til 5}" ()
// q "{til x}" (5)
// q "{x+til y}" (5,6)
// q "{x*y+til z}" (5,6,7)


// otherwise we will need to use the more verbose (which works now).  Please vote on which scheme you prefer

let q1 t x = c.q(t,x)      // k: send and return (synchronous)
let qs1 t x = c.qs(t,x)    // k: send  (asynchronous)
let qr1 t x = c.qr(t,x)    // k: synchronous.  No return 

let q2 t x y= c.q(t,x,y)      // k: send and return (synchronous)
let qs2 t x y= c.qs(t,x,y)    // k: send  (asynchronous)
let qr2 t x y= c.qr(t,x,y)    // k: synchronous.  No return 

let q3 t x y z= c.q(t,x,y,z)      // k: send and return (synchronous)
let qs3 t x y z= c.qs(t,x,y,z)    // k: send  (asynchronous)
let qr3 t x y z= c.qr(t,x,y,z)    // k: synchronous.  No return 


// TODO: need to attach the pretty print function to FSI
