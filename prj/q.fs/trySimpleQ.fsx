#r @"bin\Debug\CLX.SYS.IO.q.dll"
#r @"bin\Debug\CLX.SYS.IO.q.fs.dll"
#r @"bin\Debug\CLX.SYS.IO.q.fmt.dll"

open CLX.SYS.IO
open CLX.SYS.IO.q
open CLX.SYS.IO.Q

let c = new c("", 5001)
let k t = c.k(t)      // k: send and return (synchronous)
let ks t = c.ks(t)    // k: send  (asynchronous)
let kr t = c.kr(t)    // k: synchronous.  No return 

// create temporary in-memory table
kr "tmp:([x:1+til 10] v1:20+til 10; v2:200+til 10)"   
let o=k "select from tmp" 

let d=k "select from tmp" :?> q.Dict |> (fun x->q.Tbl(x))

d.x.x                 // key / compount key column names
d.x.y.[0]             // the key values
d.y.x                 // non key column names
d.x.y.[0]             // the non key column values

QPretty.Print <| k "`a`b`c!1 2 3"

QPretty.Print d       // ... currently breaking   TODO: fix

// TODO: need to attach the pretty print function to FSI




