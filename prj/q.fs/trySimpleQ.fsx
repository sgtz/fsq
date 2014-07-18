#r @"bin\Debug\CLX.SYS.IO.q.dll"
#r @"bin\Debug\CLX.SYS.IO.q.fs.dll"
#r @"bin\Debug\CLX.SYS.IO.q.fmt.dll"

open CLX.SYS.IO
open CLX.SYS.IO.q
open CLX.SYS.IO.Q

let c = new c("", 5001)
let q t = c.q(t)      // k: send and return (synchronous)
let qs t = c.qs(t)    // k: send  (asynchronous)
let qr t = c.qr(t)    // k: synchronous.  No return 

// create temporary in-memory table
qr "tmp:([x:1+til 10] v1:20+til 10; v2:200+til 10)"   
q "tmp:([x:1+til 10] v1:20+til 10; v2:200+til 10)"   
let o=q "select from tmp" 
o


let d=q "select from tmp" :?> q.Dict |> (fun x->CLX.SYS.IO.q.Tbl(x))

d.x.x                 // key / compount key column names
d.x.y.[0]             // the key values
d.y.x                 // non key column names
d.x.y.[0]             // the non key column values

QPretty.Print <| q "`a`b`c!1 2 3"

QPretty.Print d       // ... currently breaking   TODO: fix

// TODO: need to attach the pretty print function to FSI




