namespace PRX.SYS.IO.kx.TST
open Xunit
open FsUnit.Xunit
open System
open CLX.SYS.IO.q
open CLX.SYS.IO.Q

module c_cs_for_fs = 
    
    let c = new c("", 5001)
    let k t = c.k(t)      // k: send and return (synchronous)
    let ks t = c.ks(t)    // k: send  (asynchronous)
    let kr t = c.kr(t)    // k: synchronous.  No return 

    // k: send and return // k: send  // k: do.  Send, but do not return

    let [<Fact>] ``c->k(C)``() = c.k("1+1") |> should equal (box 2L)

    let [<Fact>] ``c->k(C,x)``() = c.k("1+1+", 3) |> should equal (box 5L)
    
    let [<Fact>] ``c->k(C,x,y)``() = c.k("{x+1+1+y}", 4, 5) |> should equal (box 11L)

    //let [<Fact>] ``k (C,x,y)``() = k("{x+1+1+y}", 4, 5) |> should equal (box 11L)

    //let [<Fact>] ``k (C,x)``() = k("{x+1+1}", 4) |> should equal (box 6L)

    //let [<Fact>] ``k (C)``() = k("1+1") |> should equal (box 2L)

    let [<Fact>] ``c->k1``() = c.k1 "1+1+" 4 |> should equal (box 6L)

    let [<Fact>] ``c->k2i``() = c.k2i 4 "{x+1+y}" 5 |> should equal (box 10L)

    let [<Fact>] ``c->k3``() = c.k3 "{[x;y;z] x+y+z}" 1 2 5 |> should equal (box 8)



    

