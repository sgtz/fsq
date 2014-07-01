//#define"ERRORHANDLING"

namespace CLX.SYS.IO.Q

open Microsoft.FSharp.Reflection
open System.Reflection
open System
open CLX.SYS.IO.q
open System.Text.RegularExpressions 
open System.Collections
open CLX.SYS.IO.Q

module Seq =
  let curtail (n:int) (S : seq<'a>) = 
      S |> Seq.toList |> List.rev |> Seq.skip n |> Seq.toList |>  List.rev |> List.toSeq

module qutil =
    
    // TODO: allow the conversion of nested variations

    // NB. tupple could contain a record

    let isRecord r = 
      FSharpType.IsRecord r

    let hasInterface (typ:Type) (interfaceTyp:Type ) =
      match typ.FindInterfaces(TypeFilter(fun (typ : Type) (criteria : obj) -> typ = (criteria :?> Type) ), interfaceTyp) with
      | [||] -> false
      | _ -> true

    let isSequence (typ:Type) =
      hasInterface typ typedefof<IEnumerable>
    let isSequenceT<'a>() =  
      isSequence <| typedefof<'a>

    /// currently not nested / recursively defined
    let tupleToList o = 
        if FSharpType.IsTuple(o.GetType()) then 
            FSharpValue.GetTupleFields o 
            //|> Array.map(fun x->(tupleToList x) :> obj)   // fails because atoms come back as lists
            |> Array.toList
        else 
            [o]

    /// currently not nested / recursively defined
    let tupleToArray o =
        if FSharpType.IsTuple(o.GetType()) then
            FSharpValue.GetTupleFields o 
            // |> Array.map(fun x->(tupleToArray x) :> obj)
        else
            [|o|]

    let tupleToList1 o = 
        if FSharpType.IsTuple(o.GetType()) then 
            Some (Microsoft.FSharp.Reflection.FSharpValue.GetTupleFields o |> Array.toList)
        else 
            None
    let tupleToArray1 o =
        if FSharpType.IsTuple(o.GetType()) then
            Some (FSharpValue.GetTupleFields o |> Array.toList)
        else
            None

    let listToTuple l =
        let l = List.toArray l
        let types = l |> Array.map (fun o -> o.GetType())
        let tupleType = FSharpType.MakeTupleType types
        FSharpValue.MakeTuple (l , tupleType)

    let arrayToTuple a =
        let types = a |> Array.map (fun o -> o.GetType())
        let tupleType = FSharpType.MakeTupleType types
        FSharpValue.MakeTuple (a , tupleType)

    let itemToTuple o : obj = 
        let L = [|o|]
        let types = L |> Array.map (fun o -> o.GetType())
        let tupleType = FSharpType.MakeTupleType types
        FSharpValue.MakeTuple(L, tupleType)

    let listToArray (L:obj) = 
      match L with 
      | :? List<String> as v -> v |> List.toArray :> obj
      | :? List<byte> as v -> v |> List.toArray :> obj
      | :? List<bool> as v -> v |> List.toArray :> obj
      | :? List<Int16> as v -> v |> List.toArray :> obj
      | :? List<Int32> as v -> v |> List.toArray :> obj
      | :? List<Int64> as v -> v |> List.toArray :> obj
      | :? List<Double> as v -> v |> List.toArray :> obj
      | :? List<Single> as v -> v |> List.toArray :> obj
      | :? List<char> as v -> v |> List.toArray :> obj
      | :? List<DateTime> as v -> v |> List.toArray :> obj
      | :? List<Minute> as v -> v |> List.toArray :> obj
      | :? List<Second> as v -> v |> List.toArray :> obj
      | :? List<Date> as v -> v |> List.toArray :> obj
      | :? List<Dict> as v -> v |> List.toArray :> obj
      | :? List<Flip> as v -> v |> List.toArray :> obj
      | :? List<KTimespan> as v -> v |> List.toArray :> obj
      | :? List<obj> as v -> v |> List.toArray :> obj
      | _ -> failwith "unknown list type"

    let seqToArray (S:obj) = 
      match S with 
      | :? seq<String> as v -> v |> Seq.toArray :> obj
      | :? seq<byte> as v -> v |> Seq.toArray :> obj
      | :? seq<bool> as v -> v |> Seq.toArray :> obj
      | :? seq<Int16> as v -> v |> Seq.toArray :> obj
      | :? seq<Int32> as v -> v |> Seq.toArray :> obj
      | :? seq<Int64> as v -> v |> Seq.toArray :> obj
      | :? seq<Double> as v -> v |> Seq.toArray :> obj
      | :? seq<Single> as v -> v |> Seq.toArray :> obj
      | :? seq<char> as v -> v |> Seq.toArray :> obj
      | :? seq<DateTime> as v -> v |> Seq.toArray :> obj
      | :? seq<Minute> as v -> v |> Seq.toArray :> obj
      | :? seq<Second> as v -> v |> Seq.toArray :> obj
      | :? seq<Date> as v -> v |> Seq.toArray :> obj
      | :? seq<Dict> as v -> v |> Seq.toArray :> obj
      | :? seq<Flip> as v -> v |> Seq.toArray :> obj
      | :? seq<KTimespan> as v -> v |> Seq.toArray :> obj
      | :? List<obj> as v -> v |> Seq.toArray :> obj
      | _ -> failwith "unknown sequence type"

    /// a fast q dictionary can double up as a record
    let rec convFsRecord (v:obj) = 
      let typ = v.GetType()
      if FSharpType.IsRecord(typ) then
        let P = FSharpType.GetRecordFields(typ)
        Dict(
          P |> Array.map(fun x->x.Name),
          P |> Array.map(fun x->
                  let item = x.GetValue(v)
                  if FSharpType.IsRecord(item.GetType()) then
                    convFsRecord(item) :> obj
                  else
                    item)
          )
      else
        failwith "not a record"
      // TODO: test if this definition converts nested records into nested dictionaries

    // conversion of all major F# constructs: tuple, list, array, sequence, record
    // to KDB, arrays and lists look the same.  
    let convertFsType (x:obj) =
      let typ = x.GetType()
      if typ.IsArray then
        x // leave as is
      elif isRecord typ then
        convFsRecord x :> obj
      elif FSharpType.IsTuple typ then
        tupleToArray x :> obj
      elif isSequence typ then
        seqToArray x 
      else
        x // leave it for c.cs code to deal with.  

    /// char array to string
    let CtoStr (x:char[]) = String(x)

    /// remove last (\r\n) and put it infront 
    let newLineInFront (x:char[]) = 
      x |> Seq.curtail 2 |> Seq.append ['\n'] |> Seq.toArray 

    let removeTrailingNewLine (x:char[]) = 
      x |> Seq.curtail 2 |> Seq.toArray 

    let shortenErrorLine (s:string) = 
      let i = s.IndexOf(" in ")
      if i >= 0 then
        s.Substring(0, i)  
      else 
        s
    let truncateError (x:string) = 
      let lines = x.Split([|"\n"|],StringSplitOptions.RemoveEmptyEntries) |> Array.map(fun s->shortenErrorLine(s)) 
      let L = 
        if lines.Length >=2 then 
          (lines |> Seq.take 2 |> Seq.toArray)
        else 
          lines
      String.Join(" ", L) + (if lines.Length>2 then "..." else "")

type TDict<'T,'U>(x0 : 'T, y0 : 'U) =
    let mutable X = x0
    let mutable Y = y0

    member __.x with get() = X and set(value) = X <- value
    member __.y with get() = Y and set(value) = Y <- value


type TTbl(x0,y0) = 
    let mutable X : Flip = x0
    let mutable Y : Flip = y0

    member __.x with get() = X and set(value) = X <- value
    member __.y with get() = Y and set(value) = Y <- value

    new(d : Dict) = 
        new TTbl(d.x :?> Flip, d.y :?> Flip)
    new(d : TDict<'Flip,'Flip>) = 
        new TTbl(d.x, d.y)
    new(o : obj)  = 
        new TTbl(o :?> Dict)

type KxType() = 

    static let rec makeDictFun(d : Dict) = 
        let generic = typedefof<TDict<_,_>>

        let arg1 = 
            match d.x with
            | :? Dict as x -> makeDictFun(d.x :?> Dict)
            | _ as x -> x

        let arg2 = 
            match d.y with
            | :? Dict as y -> makeDictFun(d.y :?> Dict)
            | _ as y -> y

        let types = [|arg1.GetType();arg2.GetType()|]
        let constructed = generic.MakeGenericType(types)
        //Microsoft.FSharp.Core.Operators.box( box (Activator.CreateInstance(constructed, [|arg1;arg2|]))  // TODO: problem -- how do we unbox something that is defined as an object?
        //Microsoft.FSharp.Core.g
        //Microsoft.FSharp.Reflection.FSharpValue
        Activator.CreateInstance(constructed)
        // TODO: we need a type provider here!!!!!

    static member isTable(d : Dict) = 
        match(d.x.GetType(), d.y.GetType()) with
        | (xt,yt) when xt = typedefof<Flip> && yt = typedefof<Flip>-> true
        | (_,_) -> false

    static member makeTbl(d : Dict) =        
        if KxType.isTable(d) then
          new TTbl(d)
        else
            raise(new Exception("dict is not a table"))

    static member makeDict(x) = 
        makeDictFun(x)

    //  convert + invoke type provider to provider boxed value
    static member wrap(x : obj) =
        match x with
        | :? Dict as d when KxType.isTable(d) -> KxType.makeTbl(d) :> obj
        | :? Dict as d -> KxType.makeDict(d)
        | _ -> x

    static member unwrap(x) = 
        //unbox x
        raise(new System.NotImplementedException())

open qutil
open System
open System.Text

type LineType =
| AssignLTyp
| FuncLTyp
| VsSentLTyp
| OtherLTyp

/// a connection to KDB+'s q.exe over TCP/IP with:
/// • F# currying
/// • tupple
/// • and FSI console support
type c(host,port,user,maxBufferSize) = 
    inherit CLX.SYS.IO.q.c(host,port,user,maxBufferSize)       

    let notAString = System.String.IsNullOrEmpty
    let isAString  x = not <| notAString x

    let provideTypedData(x) = x

    let firstNonSpace (s:string) =
      let mutable n = -1
      for i in [0 .. s.Length-1] do
        if n < 0 && s.[i] <> ' ' then
          n <- i
      n

    let normalSplit (s:string) = 
      s.Split([|'\n'|],StringSplitOptions.RemoveEmptyEntries)

    let splitIndented (s:string) =
      let S=s.Split([|'\n'|])
      let I=S |> Array.map(fun s->firstNonSpace (s))
      let sb = StringBuilder()
      let min = Array.min <| (Array.filter (fun x->x> -1) I)
      let appendIf (sb:StringBuilder) (s:string) =
        if s.Length > 0 then
          if sb.Length = 0 then
            sb.Append(s) |> ignore
          else
            sb.Append(sprintf "%s\n" s) |> ignore
      seq{
        let mutable c = 0
        for i = 0 to I.Length-1 do
          if I.[i] = -1 && sb.Length > 0 then
            yield sb.ToString()
            sb.Clear() |> ignore
          elif I.[i] = min then
            if sb.Length > 0 then 
              yield sb.ToString()
              sb.Clear() |> ignore
            appendIf sb S.[i] |> ignore
          else
            appendIf sb S.[i] |> ignore
        if sb.Length > 0 then
          yield sb.ToString()
      }

    // colons are still allowed, for example q)select j:(c1,'c2) from t1
    //let isLeftmostAnAssignment = Regex(@"([\s]*^[`a-zA-Z0-9_]*[\s]*[`a-zA-Z0-9_]+[\s]*:)")

    let getMatchList (r:Regex) s = 
      let M = r.Matches(s)
      [for m in M do if m.Success && m.Groups.[1].Success then yield m.Groups.[1].Value]

    let alphanumeric = [ 'a' .. 'z' ] |> List.append [ 'A' .. 'Z' ] |> List.append [ '0' .. '9'] |> List.append [ '_' ]
    let alphanumeric = alphanumeric |> List.append [ '@'; '\"' ] 
    let getLineType (s:string) = 
      let mode = ref 'z'
      let tokenCount = ref 0
      let foundSeq = 
        seq {
          for c in s do
            if !tokenCount < 3 then
              let newmode =
                if List.exists(fun x->x=c) alphanumeric then 'a'
                elif c = ' ' then !mode // ignore spaces
                elif c = ':' then ':'
                elif c = '#' then '#'
                elif c = '@' then '@'
                elif c = '{' then '{' 
                else 'n'
              if newmode <> !mode then 
                mode := newmode
                tokenCount := !tokenCount + 1
                yield !mode
          }
      let foundSeq = Seq.toList foundSeq
      let foundSeq2 = List.append foundSeq ['z';'z';'z';'z']
      let L = (Seq.take 4 foundSeq2) |> Seq.toList
      match L with
      | [ 'a' ; ':' ; '{' ; _ ] -> FuncLTyp
      | [ 'a' ; ':' ; _   ; _ ] -> AssignLTyp
      | [ '#' ; 'a' ; '@' ; '"' ] -> VsSentLTyp
      | _ -> OtherLTyp
    [<DefaultValue>]val mutable tutorialMode : bool
    let mutable commandOnSameLineAsFsPrompt : bool = true
    let mutable indentFirstLineOn : bool = false
    //[<DefaultValue>]val mutable qPromptAlways : bool
    //let mutable indentFirstLine : int = 4
    let mutable qPromptAlways : bool = true
    let mutable indentFirstLine : int = 2
    // TODO: indent command line by n  @indent
    let mutable margin : int = 2
    
    // generic console related stuff
    let leftPad() = if indentFirstLineOn then (String.replicate indentFirstLine " ") else ""
    let addMargin(ignoreFirst:bool)(s:string) = 
      s.Split('\n') 
      |> Array.mapi(fun i x->if ignoreFirst && i = 0 then x else (String.replicate margin " ")+x) 
      |> (fun S->String.Join("\n", S))

    let dropFirst(removeNl:bool,nspace:int,s:string) = 
      let mutable i = 0
      if s.Length > 0 then
        i <- i + (if removeNl && (s.[0] = '\n') then 1 else 0)
      if s.Length > (i+nspace) then
        i <- i + (if (String.replicate nspace " ") = s.Substring(i,nspace) then nspace else 0)
      s.Substring(i)
    //let filterFunc(s:string) = if s.StartsWith("{") then "" else s 
      
    /// make the same line as FSI "> " prompt
    let sameLineify s = 
      if commandOnSameLineAsFsPrompt then
        dropFirst(true,2,s)
      else
        s

    member __.SameLineify s =
      sameLineify s

    /// when enabled, lines must be prefixed by q) for the line to be executed by q.
    /// this is so that informative text can be written to the cosole prior to output being generated.
    member this.TutorialMode
      with get() = this.tutorialMode
      and set(value) = this.tutorialMode<-value
    member this.IndentFirstLineOn
      with get() = indentFirstLineOn
      and set(value) = indentFirstLineOn<-value
    member this.CommandOnSameLineAsFsPrompt
      with get() = commandOnSameLineAsFsPrompt
      and set(value) = commandOnSameLineAsFsPrompt<-value
    member this.Margin
      with get() = margin
      and set(value) = margin<-value
    member this.QPromptAlways
      with get() = qPromptAlways
      and set(value) = qPromptAlways<-value
    member this.IndentFirstLine
      with get() = indentFirstLine
      and set(value) = indentFirstLine<-value

    member __.k0 s = base.k(s)
    member __.k1 s x = base.k(s,x)
    member __.k2 s x y = base.k(s,x,y)
    member __.k2i x s y = base.k(s,x,y)
    member __.k3 s x y z = base.k(s,x,y,z)

    member __.ks0 s x = base.ks(s)
    member __.ks1 s x = base.ks(s,x)
    member __.ks2 s x y = base.ks(s,x,y)
    member __.ks2i x s y = base.ks(s,x,y)
    member __.ks3 s x y z = base.ks(s,x,y,z)
    member __.q o =  
        // TODO: include record conversion, and if in list format, leave as is.
        match tupleToList o with
        | [s] -> __.k(string s)
        | [s;a0] -> __.k(string s, a0)
        | [s;a0;a1] -> __.k(string s, a0, a1)
        | [s;a0;a1;a2] -> __.k(string s, a0, a1, a2)
        | L -> raise(System.ArgumentException(sprintf "only up to (s,x,y,z) can be supplied.  %i arguments were presented" L.Length))       

    member __.qs o =
        match tupleToList o with
        | [s] -> __.ks(string s)
        | [s;a0] -> __.ks(string s, a0)
        | [s;a0;a1] -> __.ks(string s, a0, a1)
        | [s;a0;a1;a2] -> __.ks(string s, a0, a1, a2)
        | L -> raise(System.ArgumentException(sprintf "only up to (s,x,y,z) can be supplied.  %i arguments were presented" L.Length))       

    member __.qr o =
        match tupleToList o with
        | [s] -> __.kr(string s)
        | [s;a0] -> __.kr(string s, a0)
        | [s;a0;a1] -> __.kr(string s, a0, a1)
        | [s;a0;a1;a2] -> __.kr(string s, a0, a1, a2)
        | L -> raise(System.ArgumentException(sprintf "only up to (s,x,y,z) can be supplied.  %i arguments were presented" L.Length))       

    member this.WriteToFSI(line:string) =
      sprintf "%s%s" (leftPad()) line

    member this.LineIsAnAssignment (s : string) =
      getLineType(s) = AssignLTyp

    member private this.doQ_output (line:string) (t:obj list) (isFirstInSeq:bool) (hideCmd:bool) = 
      let hasPrompt = line.StartsWith("q)")
      let line = if hasPrompt then line.Substring(2) else line
      let line = line.TrimStart()
      let makePrompt hasPrompt = 
        if this.tutorialMode then
          if hasPrompt then 
            "q)" 
          else 
            ""
        else
          if hasPrompt || (not(hasPrompt) && qPromptAlways) then 
            "q)" 
          else 
            ""
      let leftPad() = if indentFirstLineOn then (String.replicate this.IndentFirstLine " ") else ""
      // consider subtracting one margin value if we decide to omit the first \n
      let doNewLine() = if not(isFirstInSeq) then "\n" else ""
      
      let makeCommand (line:string,hasPrompt:bool,lineType:LineType) = 
        sprintf "%s%s%s%s" (doNewLine()) (if (lineType <> FuncLTyp) then leftPad() else "") (makePrompt hasPrompt) (line)
      
      let getVsRefRegEx = Regex("""(?:\s*[#])\s*([a-zA-Z0-9_]+)(?:\s*@")""")
      let getVsRef x =
        let L = getMatchList getVsRefRegEx x
        if L.Length > 0 then L.[0] else ""
      let result = 
        if line.StartsWith("/") || (not(hasPrompt) && this.TutorialMode) then
          makeCommand(line,hasPrompt,OtherLTyp)
        elif isAString line then
//#if ERRORHANDLING 
          try 
//#endif
            let cnt = if not(line.TrimEnd().EndsWith("}")) || line.TrimEnd().EndsWith("]") then -1 else t.Length
            let execLine = 
              sprintf ".Q.s %s%s" line (cnt |> (function|0->"[]"|1->"[x]"|2->"[x;y]"|3->"[x;y;z]"| _ ->"")) 
              |> (fun x->if cnt > 0 then sprintf "{%s}" x else x)
            let tu=listToTuple <| [execLine :> obj] @ t
            let r=(this.q tu) :?> char[] |> removeTrailingNewLine |> CtoStr
            let prettyArgs=try QPretty.Print(t |> List.toArray) with | _ -> sprintf "%A" t  // use default F# formatting if pretty print fails for whatever reason
            let cmdPostfix = (cnt |> (function| -1|0->""| _->sprintf " <| %s" prettyArgs))  // in this case the symbol <| means send across the network and execute the function to the left with supplied params
            match getLineType(line) with
            | AssignLTyp -> makeCommand(line, hasPrompt, AssignLTyp)
            | FuncLTyp -> makeCommand(line, hasPrompt, FuncLTyp)
            | VsSentLTyp -> sprintf "VS Send error @ %s.  (partially quoted string?)" (getVsRef line)
            | OtherLTyp -> 
                (if hideCmd then "" else (makeCommand(line,hasPrompt,OtherLTyp) + " " + cmdPostfix + "\n"))
                + (if isAString(r) then r else "")
//#if ERRORHANDLING 
          with
          | x -> sprintf "%s%s\n%s" (doNewLine()) (makeCommand(line,hasPrompt,OtherLTyp)) ((if x.Message.StartsWith("`") then "" else "'") + truncateError(x.Message))
//#endif
        else 
          sprintf "%s%s%s" (doNewLine()) (makePrompt(hasPrompt)) (line)
      let result2 = addMargin false result
      result2

    /// sends string to Q and returns in q console format
    member __.Q0 x hideCmd = 
      let h::t = tupleToList x
      let H = splitIndented (string h) |> Seq.toArray
      match H,t with
      | [||],_ -> seq { yield "" }
      | [|line|],_ ->seq { yield __.doQ_output line t true hideCmd }
      | lines,[] ->   seq { 
                            let first = ref true
                            for line in lines do
                              yield __.doQ_output line t !first hideCmd
                              first := false
                         }
      | A,_ ->    seq { yield sprintf "%s" "parameters are not allowed for multi-line commands" }
      //| _ -> [] |> List.toSeq
      
      // TODO: consider enabling for a rectangular set of parameters might work

    member __.Q x =   
      __.Q0 x false

    // k through the Kx Type Provider -- just stubs so far.  Not implemented.
    member __.tk0 s = provideTypedData <| base.k(s)
    member __.tk1 s x = provideTypedData <| base.k(s,x)
    member __.tk2 s x y = provideTypedData <| base.k(s,x,y)
    member __.tk2i x s y = provideTypedData <| base.k(s,x,y)
    member __.tk3 s x y z = provideTypedData <| base.k(s,x,y,z)

    member __.tkt o = __.q o
    /// qconnection
    new(host,port) = new c(host,port,System.Environment.UserName,c.DefaultMaxBufferSize)
    /// qconnection
    new(host,port,user) = new c(host,port,user,c.DefaultMaxBufferSize)


// create conversion (opt. implicit) methods for each of these:
// month, date, minute, second, KTimespan, KNanoDateTime, Dict, TBL, TFlip
