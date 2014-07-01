#Example code 

##Useful closures to drop into a client

let c = new c("", 5001)

let k t = c.tkt(t)               //k send and return

let ks t = c.kst(t)              //k send (Tuple based)

let kd t = c.kdt(t)              //k do.  Send, but do not return.  (Tuple based)

   
##Old Examples

let c = new c("", 5001);
let k o = c.kt

let mutable kr = obj()

let kr0 s = kr <- c.K s
let kr1 s x = kr <- c.K1 s x
let kr2 s x y = kr <- c.K2 s x y
let kr3 s x y z = kr <- c.K3 s x y z

