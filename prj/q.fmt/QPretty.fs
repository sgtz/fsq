namespace CLX.SYS.IO.Q
open System
open System.Reflection
open CLX.SYS.IO.q
open QPrettyM

type QPretty() =     
    
    static member CharArrayToString(v : obj) =
        charArrayToString(v)

    // @TODO: use modulus to handle placing multiple dimensions   
    static member PrintFlip(v : Flip) = 
        printFlip v

    static member PrintDict(v : Dict) = 
        printDict v

        // @TODO: find out why dict x,y are defined as an object

    static member Print(v : obj) = 
        print v

    // PROBLEM: @TODO we assume dictionaries are keyed by string or symbol.  This may not be the case

    // @TODO: use "q)types each value a" to get the list of types for prtty printing (no guessing).  `C is a string `c a char `s`S are lists of syms


