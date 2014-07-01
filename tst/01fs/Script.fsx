// Learn more about F# at http://fsharp.net. See the 'F# Tutorial' project
// for more guidance on F# programming.

//#r "C:\dev\PX2\PRJ\PRX\PRJ\PRX\bin\x86\Debug\PRX.dll"
#r "C:\dev\PX2\PRJ\SYS\PRJ\IO\PRJ\k\PRJ\q\obj\Debug\PRX.SYS.IO.q.dll"
#r "C:\dev\PX2\PRJ\SYS\PRJ\IO\PRJ\k\PRJ\q.fs\obj\Debug\PRX.SYS.IO.kx.fs.dll"
//#r "C:\dev\RTK\LIB\nuget\xunit.1.9.1\lib\net20\xunit.dll"
//#r "C:\dev\RTK\LIB\nuget\FsUnit.xUnit.1.2.1.2\Lib\Net40\FsUnit.Xunit.dll"
// not valid assemblies
//#load "Tests.fs"
open System
//open _01fs
open PRX.SYS.IO.kx
//open PRX.SYS.IO.kx


type C=PRX.SYS.IO.kx.c

let c=new c("localhost",5001)



//c.k("1")
//
//// Define your library scripting code here
//
//c.ks("{test::x}",45)
//c.k("test")
//c.ks("{test::x}",45)
//
//c.ks("{test::x}",45)
//
//c.ks("a:{x}",45)
//c.k("{1+x}",4)
//
//
//c.k("(0Wz;-0Wz;0Nz)")
//
//// DateTime(2292,4,1)
//
//let A=c.k("{:Az::(x;y;z)}", DateTime(9223372036854775807L), DateTime(-9223372036854775808L), DateTime(-9223372036854775807L))
//let A2=c.k("{:Az::(x;y;z)}", DateTime.MaxValue, DateTime.MinValue, DateTime.MinValue.AddTicks(1L))
//let A3=c.k("{:Az::(x;y;z)}", DateTime(2291,12,12,23,59,59), DateTime.MinValue, DateTime.MinValue.AddTicks(1L))
//
//(A :?> DateTime[]) |> Array.map (fun x -> x.Ticks) 
//
//[|Int64.MaxValue;(Int64.MinValue+1L);Int64.MinValue|]
//(A :?> DateTime[]) |> Array.map (fun x -> x.Ticks) 
//A
//let Vz = c.k("(0Wz;-0Wz;0Nz)") 
//c.ks("{Vz::x}",Vz)
//let Vz_ret = c.k("Vz")
//
//c.k("(0Wv;-0Wv;0Nv)")
//
//c.k("(0Wj;-0Wj;0Nj)")
//let Vp = c.k("`long$(0Wp;-0Wp;0Np)") 
//let Vp2 = c.k("Vp:(0Wp;-0Wp;0Np);Vp") 
//
//Int64.MinValue = -9223372036854775808L
//Int64.MaxValue = 9223372036854775807L
////svn test
////let Vp = c.k("`long$(0Wp;-0Wp;0Np)") 
////let V = c.k("(0Wp;-0Wp;0Np)") 
////(Vp :?> DateTime[]) |> Array.map (fun x -> x.Ticks) 
//c.ks("{Vp::x}",Vp)
//let Vp_ret = c.k("`long$Vp")
//let Vp_ret2 = c.k("Vp")
//
//let r2=(Vp :?> DateTime[]) |> Array.map (fun x -> x.Ticks) 
//let r3=(Vp_ret :?> DateTime[]) |> Array.map (fun x -> x.Ticks) 
//
//// use native C# types 
//
//
//let Ai = [|c.NULL('i');C.getw('i');C.geta('i')|] |> Array.map (fun x -> x :?> Int32)
//let Vi = c.k("0W -0W 0N")
//let Vi2 = c.k("(0Wi;-0Wi;0Ni)")
//let Vz2 = c.k("(0Wz;-0Wz;0Nz)") 
////let Vz = c.k("(0Wz;-0Wz;0Nz)") :?> DateTime[]
////let Vz = Array.map (fun x -> (x :?> DateTime)) (c.k("(0Wz;-0Wz;0Nz)") :?> obj[])
//let Az = [|c.NULL('i');C.getw('i');C.geta('i')|] |> Array.map (fun x -> x :?> DateTime)
////let Rz = Array.forall2 (fun x y -> x=y) Vz Az 
//let Vp3 = c.k("(0Wp;-0Wp;0Np)") :?> DateTime[]
//let Vg = c.k("(enlist 0Ng)") :?> Guid[]
//let Vx = c.k("(enlist 0Nx)") :?> Byte[]
//let Vh = c.k("(0Wh;-0Wh;0Nh)") :?> Int16[]
//let Vj = c.k("(0Wi;-0Wi;0Ni)") :?> Int32[]
//let Vj2 = c.k("(0Wj;-0Wj;0Nj)") :?> Int64[]
//let Ve = c.k("(0We;-0We;0Ne)") :?> Single[]
//let Vf = c.k("(0Wf;-0Wf;0Nf)") :?> Double[]
////let Vc = c.k("(0Wc;-0Wc;0Nc)")
////let Vs = c.k("(\"\")")
//let Vm = c.k("(0Wm;-0Wm;0Nm)") :?> Month[]
//let Vd = c.k("(0Wd;-0Wd;0Nd)") :?> Date[]
//let Vu = c.k("(0Wu;-0Wu;0Nu)") :?> Minute[]
//let Vu2 = c.k("(24u;-60u;99u)") :?> Minute[]
//let Vv = c.k("(0Wv;-0Wv;0Nv)") :?> Second[]
//let Vt = c.k("(enlist 0Nt)") :?> TimeSpan[]
//
//Vt.[0].isnu()
//Vt.[0].Ticks
//Int64.MinValue
//(c.NULL('t') :?> TimeSpan).Ticks = Vt.[0].Ticks
//
//Vt.[0].isnu()
//
////Vt.[0].
//
//Array.map (fun x -> x.GetType().Name) Vt 
//
////Vt.[0].is
//
//Vm.[0].Equals(Vm.[1])
//
//Vm.[0] > Vm.[1]
//Vm.[0].isnu
//
//// is there some kind of way to do quotations nicely?
////open  Microsoft.FSharp.Quotations
////let expr : Expr<string> = <@ select from abc @>
//
////(12.2).isnu
////
////let xx =12.2
////
////xx.isnu()
//
//Vm.[0].isnu
//
//Vm.[0].isa
//
//let testT = TimeSpan(Int64.MinValue)
//testT.isnu()
//
//
//c.ks("a:44")
//c.ks("{a::x}", 46)  
//c.ks("{.some.name.space.a::x}", 47)
//c.ks("a:",45)                   // fails
//c.ks(".some.name.space.a:", 48) // fails

