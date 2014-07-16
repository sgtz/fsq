namespace PRX.SYS.IO.kx.TST
open Xunit
open FsUnit.Xunit
open System
open CLX.SYS.IO
open CLX.SYS.IO.q
open CLX.SYS.IO.Q

module c_cs_q_io_via_fs = 
          
    let c = new c("localhost", 5001);
    let k t = c.k(t)      // k: send and return (synchronous)
    let ks t = c.ks(t)    // k: send  (asynchronous)
    let kr t = c.kr(t)    // k: synchronous.  No return 

    let [<Fact>] ``isnu``() = 
        let xx =12.2
        xx.isnu() |> should equal false
    
    let [<Fact>] ``can get enlisted t``() =
        let Vt = c.k("(enlist 0Nt)") :?> TimeSpan[]
        Vt.[0].isnu() |> should equal true

    let [<Fact>] ``can get 0Nt``()      = (c.k("0Nt")  :?> TimeSpan).isnu()|> should equal true
    let [<Fact>] ``can not get 0Wt``()  = (c.k("0Wt")  :?> TimeSpan).isw() |> should equal false
    let [<Fact>] ``can not get -0Wt``() = (c.k("-0Wt") :?> TimeSpan).isa() |> should equal false

    // 0Wg and -0Wg do not exist
    let [<Fact>] ``can get 0Ng``()      = (c.k("0Ng")  :?> Guid).isnu()|> should equal true
//
    // 0Wc and -0Wc do not exist
    let [<Fact>] ``can get 0Nc``()      = (c.k("0Nc")  :?> char).isnu()|> should equal true

    let [<Fact>] ``can get 0Nz``()  = (c.k("0Nz")  :?> DateTime).isnu()    |> should equal true
    let [<Fact>] ``can get 0Wz``()  = (c.k("0Wz")  :?> DateTime).isw()     |> should equal true
    let [<Fact>] ``can get -0Wz``() = (c.k("-0Wz") :?> DateTime).isa()     |> should equal true

    let [<Fact>] ``can get 0Nh``()  = (c.k("0Nh")  :?> Int16).isnu()       |> should equal true
    let [<Fact>] ``can get 0Wh``()  = (c.k("0Wh")  :?> Int16).isw()        |> should equal true
    let [<Fact>] ``can get -0Wh``() = (c.k("-0Wh") :?> Int16).isa()        |> should equal true
        
    let [<Fact>] ``can get 0Ni``()  = (c.k("0Ni")  :?> Int32).isnu()       |> should equal true
    let [<Fact>] ``can get 0Wi``()  = (c.k("0Wi")  :?> Int32).isw()        |> should equal true
    let [<Fact>] ``can get -0Wi``() = (c.k("-0Wi") :?> Int32).isa()        |> should equal true

    let [<Fact>] ``can get 0Nj``()  = (c.k("0Nj")  :?> Int64).isnu()       |> should equal true
    let [<Fact>] ``can get 0Wj``()  = (c.k("0Wj")  :?> Int64).isw()        |> should equal true
    let [<Fact>] ``can get -0Wj``() = (c.k("-0Wj") :?> Int64).isa()        |> should equal true
    
    let [<Fact>] ``can get 0Ne``()  = (c.k("0Ne")  :?> Single).isnu()      |> should equal true
    let [<Fact>] ``can get 0We``()  = (c.k("0We")  :?> Single).isw()       |> should equal true
    let [<Fact>] ``can get -0We``() = (c.k("-0We") :?> Single).isa()       |> should equal true  

    let [<Fact>] ``can get 0Nf``()  = (c.k("0Nf")  :?> float).isnu()       |> should equal true
    let [<Fact>] ``can get 0Wf``()  = (c.k("0Wf")  :?> float).isw()        |> should equal true
    let [<Fact>] ``can get -0Wf``() = (c.k("-0Wf") :?> float).isa()        |> should equal true
    
    let [<Fact>] ``can get 0Nd``()  = (c.k("0Nd")  :?> Date).isnu()        |> should equal true
    let [<Fact>] ``can get 0Wd``()  = (c.k("0Wd")  :?> Date).isw()         |> should equal true
    let [<Fact>] ``can get -0Wd``() = (c.k("-0Wd") :?> Date).isa()         |> should equal true

    let [<Fact>] ``can get 0Nm``()  = (c.k("0Nm")  :?> Month).isnu()       |> should equal true
    let [<Fact>] ``can get 0Wm``()  = (c.k("0Wm")  :?> Month).isw()        |> should equal true
    let [<Fact>] ``can get -0Wm``() = (c.k("-0Wm") :?> Month).isa()        |> should equal true

    let [<Fact>] ``can get 0Nu``()  = (c.k("0Nu")  :?> Minute).isnu()      |> should equal true
    let [<Fact>] ``can get 0Wu``()  = (c.k("0Wu")  :?> Minute).isw()       |> should equal true
    let [<Fact>] ``can get -0Wu``() = (c.k("-0Wu") :?> Minute).isa()       |> should equal true

    let [<Fact>] ``can get 0Nv``()  = (c.k("0Nv")  :?> Second).isnu()      |> should equal true
    let [<Fact>] ``can get 0Wv``()  = (c.k("0Wv")  :?> Second).isw()       |> should equal true
    let [<Fact>] ``can get -0Wv``() = (c.k("-0Wv") :?> Second).isa()       |> should equal true

    let [<Fact>] ``can assign``() = 
        c.ks("{abc::x}", 123)
        c.k("abc") :?> Int32 |> should equal 123

    let [<Fact>] ``other Tests``() = 

        // what do the results from q sql look like?
        let sel=c.k("select from E")

        c.ks("oNow:x", DateTime.Now)
        let ooNow = c.k("oNow")
        c.ks("oNull:x", DateTime.MinValue)
        let ooNull = c.k("oNull:x;oNull")
        c.ks("{oMax:x;oMax}", DateTime.MaxValue)

        // what do the c provided 0N 0W and -0W values look like?

        // types via character: bg xhijefcspmdznuvt
        let test = c.NULL('i')
        let Ai = [|c.NULL('i');CLX.SYS.IO.q.c.getw('i');CLX.SYS.IO.q.c.geta('i')|]    // TODO: C# .getw / geta is not visible.  It should be.  Investigate.
        let Az = [|c.NULL('i');CLX.SYS.IO.q.c.getw('i');CLX.SYS.IO.q.c.geta('i')|]    
        let Vs = c.k("()")
        ()