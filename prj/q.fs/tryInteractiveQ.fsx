#r @"bin\Debug\CLX.SYS.IO.q.dll"
#r @"bin\Debug\CLX.SYS.IO.q.fs.dll"
#r @"bin\Debug\CLX.SYS.IO.q.fmt.dll"

open CLX.SYS.IO
open CLX.SYS.IO.Q
#load "interactiveQ.fs"
open UtilM

let c = new c("", 5001)

let Q x = Q c x
Q "til 10"

// TODO: need to attach the pretty print function to FSI

