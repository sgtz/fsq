#Example code 

##closures to drop into any consumer

let c = new c("", 5001)

let k t = c.tkt(t)               //k send and return

let ks t = c.kst(t)              //k send (f# Tuple based)

let kd t = c.kdt(t)              //k do.  Send, but do not return.  (f# Tuple based)
   
##OLD INTERFACING FROM F# EXAMPLES

let c = new c("", 5001);
let k o = c.kt

let mutable kr = obj()

let kr0 s = kr <- c.K s
let kr1 s x = kr <- c.K1 s x
let kr2 s x y = kr <- c.K2 s x y
let kr3 s x y z = kr <- c.K3 s x y z

