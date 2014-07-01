namespace CLX.SYS.IO.Q
open System
open System.Reflection
open CLX.SYS.IO.q

module QPrettyM = 

    let charArrayToString(v : obj) = 
        let sb = new System.Text.StringBuilder()
        let buffer(value : string) = sb.Append(value)
        let bufferch(value : char) = sb.Append(value)

        let vt = v.GetType()
        if vt.IsArray && vt.GetElementType() = typedefof<char> then
            let A = v :?> Array
            let c=A.Length
            for i = 0 to c - 1 do
                bufferch <| (A.GetValue(i) :?> char) |> ignore
        else 
            raise (Exception("` a char array"))
        sb.ToString()

    let rec printAtom(v : obj, printTypeChar : bool) = 

        let sb = new System.Text.StringBuilder()
        let buffer(value : string) = sb.Append(value)
        
        let vt = v.GetType()

        let typechar = if c.canHaveTypeChar(v) then c.ct(v) else ' '

        if c.canHaveSpecial(v) && c.qn(v) && ("cbs".IndexOf(typechar) = -1) then
            buffer "0N" |> ignore
        elif c.canHaveSpecial(v) && c.qa(v) && ("cbs".IndexOf(typechar) = -1) then
            buffer "-0W" |> ignore
        elif c.canHaveSpecial(v) && c.qw(v) && ("cbs".IndexOf(typechar) = -1) then
            buffer "0W" |> ignore
        elif printTypeChar then
            buffer <| 
            (match vt with 
            | _ when vt.IsArray && vt.GetElementType() = typedefof<char> ->
                // @TODO: use q meta to determine ` or `C
                // @TODO: use notion of vector of same type determine need for space or semicolon (prefer space)
                // @TODO: use type to have a trailing f or h on spaced numbers.
                // @TODO: use same formatter everywhere
                System.String.Concat(Array.ofList(["\""; (charArrayToString v); "\""]))
            | _ when vt=typedefof<bool> -> sprintf "%b" (v :?> bool) 
            | _ when vt=typedefof<char> -> sprintf "%c" (v :?> char) 
            | _ when vt=typedefof<Int16> ->sprintf "%i" (v :?> Int16) 
            | _ when vt=typedefof<int> -> sprintf "%i" (v :?> int) 
            | _ when vt=typedefof<Int64> -> sprintf "%i" (v :?> Int64) 
            | _ when vt=typedefof<float> -> sprintf "%g" (v :?> float) 
            | _ when vt=typedefof<Single> -> sprintf "%g" (v :?> Single)
            //| _ when vt=typedefof<Guid> -> sprintf "%g" (v :?> Guid) 
            | _ when vt=typedefof<Single> -> sprintf "%g" (v :?> Single)
            | _ when vt=typedefof<string> -> sprintf "`%s" (v :?> string)                           // NB. syms come back as string, strings as an array of char
            | _ when vt=typedefof<DateTime> -> sprintf "%s" <| String.Format("{0:o}",v)|>fun x -> x.Substring(0,x.Length- 4)
            | _ -> sprintf "%s" (print(v))
            //| _ -> raise(Exception(sprintf "Unsupported type %A" vt.Name))
            //| _ when vt=typedefof<Byte> -> buffer <| sprintf "%x" v|> ignore
            //| _ when vt=typedefof<KNanoDattime> -> buffer <| printVector(col :?> KNanoDattime[]) |> ignore
            //| _ when vt=typedefof<Minute> -> buffer <| printVector(col :?> Minute[]) |> ignore
            //| _ when vt=typedefof<Month> -> buffer <| printVector(col :?> Month[]) |> ignore
            //| _ when vt=typedefof<Date> -> buffer <| printVector(col :?> Date[]) |> ignore
            //| _ when vt=typedefof<Second> -> buffer <| printVector(col :?> Second[]) |> ignore
            //buffer <| (sprintf "%A" v) |> ignore
            ) |> ignore
        sb.ToString()   

    // an array of something
    and printVector(V : 'T[]) = 
        let sb = new System.Text.StringBuilder()
        let buffer(value : string) = sb.Append(value)

        let t = V.GetType()
        let isgenerallist = (t.IsArray && (typedefof<obj> = t.GetElementType()))

        let cstr=c.ctt(t.GetElementType()).ToString()
        let mutable haveParen = 
            match cstr with
            | "c" -> false
            | _ -> true
        //if haveParen = false && c.qhasspecial(V) then
            //haveParen <- true

        let hasSpecial = c.qhasspecial(V)

        if not isgenerallist && V.Length = 0 then
            match cstr with
            |"f"|"i"|"s" -> () // assume float if there's a decimal point and no type character.  If no decimal point and no type character then assume it is an int
            | _ -> buffer <| "`" + String(c.Cct(t.GetElementType())) + "$" |> ignore

        if haveParen  || V.Length = 0 then
            buffer <| "(" |> ignore

        let mutable first = true
        if V.Length = 1 then
            buffer <| "enlist " |> ignore
        for value in V do
            let valuet = value.GetType()
            if first = false then
                if isgenerallist then
                    buffer <| ";" |> ignore
                elif cstr <> "s" then 
                    buffer <| " " |> ignore
            first <- false
            buffer <| printAtom(value, not hasSpecial) |> ignore
        if not isgenerallist && V.Length <> 0 then
            match cstr with
            |"f"|"i"|"s" -> () // assume float if there's a decimal point and no type character.  If no decimal point and no type character then assume it is an int
            | _ -> buffer <| c.ctt(t.GetElementType()).ToString() |> ignore
        if haveParen || V.Length = 0 then
            buffer <| ")" |> ignore

        sb.ToString()

    and printArrayObj(values : obj) = 
        let vt = values.GetType()
        let ret=
            match vt.GetElementType() with
            //| _ when vt.IsArray && vt.GetElementType() = typedefof<char[]> -> printVector T
            | _ when vt=typedefof<obj[]>    -> printVector (values :?> obj[])
            | _ when vt=typedefof<bool[]>   -> printVector (values :?> bool[])
            //| _ when vt=typedefof<char[]> as T -> printVector T
            | _ when vt=typedefof<Int16[]>  -> printVector (values :?> Int16[])
            | _ when vt=typedefof<int[]>    -> printVector (values :?> int[])
            | _ when vt=typedefof<Int64[]>  -> printVector (values :?> Int64[])
            | _ when vt=typedefof<float[]>  -> printVector (values :?> float[])
            | _ when vt=typedefof<Single[]> -> printVector (values :?> Single[])
            //| _ when vt=typedefof<Guid> -> 
            //| _ when vt=typedefof<Single[]>  -> printVector (values :?> Single[])
            | _ when vt=typedefof<string[]> -> printVector (values :?> string[])
            | _ when vt=typedefof<DateTime[]> -> printVector (values :?> DateTime[])
            | _ -> raise(Exception(sprintf "Unsupported vector type %A" vt.Name))
        ret
    
    //and printObjWithScalar(value : obj) = 
      
    //and printObj(value : obj) =      

    and printFlip(v : Flip) = 
        let sb = new System.Text.StringBuilder()
        let buffer(value : string) = sb.Append(value)
        buffer <| "(" |> ignore
        if v.x.Length = 1 then
            buffer <| "enlist " |> ignore
        for nme in v.x do 
            buffer <| "`" |> ignore
            buffer <| sprintf "%s" nme |> ignore
        buffer <| ")" |> ignore
        buffer "!" |> ignore
        let mutable firstCol = true
        buffer <| "(" |> ignore
        for values in v.y do
            if firstCol = false then
                buffer <| ";" |> ignore                
            firstCol <- false
            buffer <| printArrayObj values |> ignore
        buffer <| ")" |> ignore
        sb.ToString()     

    and printDict(v : Dict) = 
        let sb = new System.Text.StringBuilder()
        let buffer(value : string) = sb.Append(value)

        // we assume `C returned for a dict key is a symbol (it could also be a string).  @TODO: think of a work-around for this.
        
        buffer <| "(" |> ignore
        let A = v.x :?> Array;
        if A.Length > 0 then
            for nme in (v.x :?> string[]) do             
                // @TODO: use q meta to determine ` or `C
                buffer <| "`" |> ignore
                buffer <| sprintf "%s" nme |> ignore
        buffer <| ")" |> ignore
        buffer "!" |> ignore
        buffer <| "(" |> ignore
        let mutable first = true
        for col in v.Y do
            if first = false then 
                buffer <| ";" |> ignore
            first <- false
            let t = col.GetType()
            let isvector = t.IsArray && t.GetArrayRank() = 1
            if t.IsArray && not isvector then
                raise(Exception("tables and higher dimention arrays not supported"))
            if isvector then
                //bg xhijefcspmdznuvt
                let t=t.GetElementType()
                buffer <| 
                (match t with 
                | _ when t=typedefof<bool>  -> printVector(col :?> bool[])
                | _ when t=typedefof<char>  -> printAtom(col :?> char[],false)
                | _ when t=typedefof<Int16> -> printVector(col :?> Int16[])
                | _ when t=typedefof<int>   -> printVector(col :?> int[])
                | _ when t=typedefof<Int64> -> printVector(col :?> Int64[])
                | _ when t=typedefof<float> -> printVector(col :?> float[])
                | _ when t=typedefof<Single>-> printVector(col :?> Single[])
                | _ when t=typedefof<Guid>  -> printVector(col :?> Guid[])
                | _ when t=typedefof<Single>-> printVector(col :?> Single[])
                | _ when t=typedefof<Byte>  -> printVector(col :?> Byte[])
                | _ when t=typedefof<String>-> printVector(col :?> String[])
                | _ -> raise(Exception(sprintf "Unsupported vector type %A" t.Name))
                ) |> ignore
//                | _ when t=typedefof<KNanoDattime> -> buffer <| printVector(col :?> KNanoDattime[]) |> ignore
//                | _ when t=typedefof<Minute> -> buffer <| printVector(col :?> Minute[]) |> ignore
//                | _ when t=typedefof<Month> -> buffer <| printVector(col :?> Month[]) |> ignore
//                | _ when t=typedefof<Date> -> buffer <| printVector(col :?> Date[]) |> ignore
//                | _ when t=typedefof<Dattime> -> buffer <| printVector(col :?> Dattime[]) |> ignore
//                | _ when t=typedefof<Second> -> buffer <| printVector(col :?> Second[]) |> ignore
//                | _ when t=typedefof<KTimespan> -> buffer <| printVector(col :?> KTimespan[]) |> ignore
            else
                buffer <| printAtom(col, true) |> ignore
        buffer <| ")" |> ignore
        sb.ToString()        

    and alignString (v : string, n, m) = 
        let pad=List.max [0;n-v.Length]

        if n >= 0 then
            (String.replicate pad " ") + v
        else
            (v.[0 .. abs n]) + ".."

    and rightAlign (v : string, n) =  
        alignString(v,n,1)

    and leftAlign (v : string, n) =  
        alignString(v,n,-1)        

    and printFlipInTabularFormatToRowSeq (v : Flip) = 
        let colCount = v.x.Length
        let rowCount = match colCount with
                        | 0 -> 0
                        | _ -> 
                                let tmpA=(v.y.[0] :?> Array)
                                tmpA.Length        
        let A = Seq.toArray 
                    <| seq { for col in 0 .. colCount - 1 do yield v.y.[col] :?> Array }
        // an array of columns
        let As = Seq.toArray 
                    <| seq { for col in 0 .. colCount - 1 do
                                yield Seq.toArray <|
                                    seq { for row in 0 .. rowCount - 1 do
                                            yield printAtom(A.[col].GetValue(row),true) }}
        // get max width of each column
        let Aw = Seq.toArray 
                   <| seq { for col in 0 .. colCount - 1 do
                                yield Seq.max <|
                                    seq { for row in 0 .. rowCount - 1 do
                                            yield As.[col].[row].Length }}
                                            //yield Array.min <| [| 80; As.[col].[row].Length |] }}
        // everything is left aligned at present
        let As2 = Seq.toArray 
                   <| seq { for row in 0 .. rowCount - 1 do
                                yield Seq.toArray <|
                                    seq { for col in 0 .. colCount - 1 do
                                            yield leftAlign(As.[col].[row],Aw.[col])}}        
        let sb = new System.Text.StringBuilder()
        seq {
                for col in 0 .. colCount - 1 do
                    if col > 0 then
                        sb.Append(" ") |> ignore
                    sb.Append v.x.[col] |> ignore
                yield sb.ToString()
                
                for col in 0 .. colCount - 1 do
                    if col > 0 then
                        sb.Append(" ") |> ignore
                    sb.Append(String.replicate Aw.[col] "-") |> ignore
                yield sb.ToString()

                for row in 0 .. rowCount - 1 do
                    for col in 0 .. colCount - 1 do
                        if row > 0 then
                            sb.Append(" ") |> ignore
                        sb.Append As2.[col].[row] |> ignore
                    yield sb.ToString()
            }

    and printFlipInTabularFormat (v : Flip) = 
        let rowSeq = printFlipInTabularFormatToRowSeq(v)
        let sb = new System.Text.StringBuilder()
        Seq.iter (fun x->sb.AppendLine(x)|>ignore) rowSeq
        sb.ToString()

    and printTblInTabularFormat (v : Tbl) =

        let colSeq = printFlipInTabularFormatToRowSeq(v.y)

        let hasNoKeyCol = v.x.x.Length = 0

        // what if keySeq is empty?  How would we know?

        let sb = new System.Text.StringBuilder()
        if hasNoKeyCol then
            Seq.iter (fun (a : string)->             
                            sb.Append(a) |> ignore
                            ) colSeq
        else
            let keySeq = printFlipInTabularFormatToRowSeq(v.x)
            let pairSeq = Seq.zip keySeq colSeq
            Seq.iter (fun (a : string,b : string)->             
                        sb.Append(a) |> ignore
                        sb.Append("|") |> ignore
                        sb.Append(b) |> ignore
                        ) pairSeq

        // convert each column item to string usingCLX.SYS.IO.q.watch.  Store in new Tbl / Flip / Dict
        // convert char arrays to strings.  Wrap strings in quotes.  Pad all strings in a column so that they are as long as the maximum length
        // have an upper limit.  Clamp strings taht are longer than max with .." (unless two characters or smaller)
        // get max char length for each column
        // get number of spacers in section A or section B
        // clamp data row output to 18 rows. Begin the 19th data row with .. (only if data is longer than 18 rows).

        // decimal point clamp

        // make for

        //let mutable tbl = new Tbl(
            
        // add an array to an array

        //let B = Seq.toArray 
        //            <| seq { for col in 0 .. v.yColCount - 1 do yield v.y.y.[col] :?> Array }

        //let keyHeadings = Array.map v.x.x
        
        //Seq.zip
        //Seq.zip3

        //Seq.map (fun x->sb.Append(sprintf "%s" x)) |> ignore
        //sb.AppendLine("") |> ignore
                                                                    
        // right align each string on the way out

//        let hasKeyCol = v.x.x.Length > 0
//
//        let noOfColsInclKeys = v.x.x.Length + v.y.x.Length
//
//        let hdrSpacerLength = noOfColsInclKeys - 1 + Array.sum Aw


//        for i = 0 to v.xRowCount do
//            L <- []
//            let A = v.x.y.[j] :?> Array
//            for j = 0 to v.xColCount do
//                L <- List.append L [printAtom(A.GetValue(i))]

//        let mutable L : string list = []
//        for i = 0 to v.xCount do
//            for j= 0 to v.xCount do
//                L<-List.append L sprintf "%A" v.y.[j]
//            L<-List.append L [" | "]
//            for j = 0 to v.yCount do
//                L<-List.append L 

        //[|"abc   ";"def   ";"hij   "|] |> Array.map (fun x->printf "%s" x)

        //let mutable 

        //printf "%A" r.x.x
        //printf "|"
        //printf "%A" r.y.x
        //printf "------------"
        //printf "%A" r.x.y
        //printf "|"
        //printf "%A" r.y.y

        ""

    and print (v : obj) =
        match v with
        | :? Flip as w -> printFlip w
        | :? Dict as w -> printDict w
        | :? Tbl as w -> printTblInTabularFormat w
        | _ when v.GetType().IsArray -> printArrayObj(v)
        //| _ when v.GetType().IsSeq -> printSeqObj(v)  // `nyi
        //| _ when v.GetType().IsList -> printListObj(v)  // `nyi
        | _ -> printAtom(v, true)

