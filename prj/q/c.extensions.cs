using System;
using System.Linq;
using System.Net.Sockets; //csc c.cs  given >q trade.q -p 5001

namespace CLX.SYS.IO.q
{
    //TODO: for (c.Date,Minute,Month,Second,KTimestamp) ensure that +/- infinity, nulls are handled correctly.

    public partial class c : TcpClient
    {
        // kd : do, throw, but do not return
        // ks : send to k
        // k  : send to k and return
        /// <summary>
        /// Throw error.  A synchronous statement inbetween k and ks.  
        /// ie. C# ignores returns, whereas F# needs |> ignore.  
        /// </summary>
        public void kr(String s) { k(s); }
        /// <summary>
        /// Throw error.  A synchronous statement inbetween k and ks
        /// ie. C# ignores returns, whereas F# needs |> ignore.  
        /// </summary>
        public void kr(String s, object x) { k(s, x); }
        /// <summary>
        /// Throw error.  A synchronous statement inbetween k and ks
        /// ie. C# ignores returns, whereas F# needs |> ignore.  
        /// </summary>
        public void kr(String s, object x, object y) { k(s, x, y); }
        /// <summary>
        /// Throw error.  A synchronous statement inbetween k and ks
        /// ie. C# ignores returns, whereas F# needs |> ignore.  
        /// </summary>
        public void kr(String s, object x, object y, object z) { k(s, x, y, z); }

        public static Int16 ah = Int16.MinValue + 1; public static Int16 wh = Int16.MaxValue; public static Int32 ai = Int32.MinValue + 1; public static Int32 wi = Int32.MaxValue;
        public static Int64 aj = Int64.MinValue + 1; public static Int64 wj = Int64.MaxValue;

        public static Int16 nh = Int16.MinValue;

        long /*nj=long.MinValue,*/ jw = long.MaxValue, ja = long.MinValue + 1;
        public static DateTime nz = new DateTime(DateTime.MinValue.Ticks);
        public static DateTime np = new DateTime(DateTime.MinValue.Ticks);

        public bool NullableMode { get; set; }

        // real
        public static Single we = Single.PositiveInfinity;
        public static Single ae = Single.NegativeInfinity;

        // float
        public static Double wf = Double.PositiveInfinity;
        public static Double af = Double.NegativeInfinity;

        //char


        // temporal

        // date
        public static DateTime wd = DateTime.MaxValue;
        public static DateTime ad = DateTime.MinValue.AddTicks(1L);

        // datetime
        public static DateTime wz = DateTime.MaxValue;
        public static DateTime az = DateTime.MinValue.AddTicks(1L);

        // Timestamp
        public static DateTime wp = DateTime.MaxValue;
        public static DateTime ap = DateTime.MinValue.AddTicks(1L);

        // time

        public static KTimespan wkt = new KTimespan(wj);
        public static KTimespan akt = new KTimespan(aj);
        public static KTimespan nukt = new KTimespan(nj);

        public static TimeSpan wt = new TimeSpan(wj);
        public static TimeSpan _at = new TimeSpan(aj);
        public static TimeSpan nut = new TimeSpan(nj);

        private const long TicksPerMillisecond = 10000;
        private const long TicksPerSecond = TicksPerMillisecond * 1000;
        private const long TicksPerMinute = TicksPerSecond * 60;
        private const long TicksPerHour = TicksPerMinute * 60;
        private const long TicksPerDay = TicksPerHour * 24;

        // Number of milliseconds per time unit 
        private const int MillisPerSecond = 1000;
        private const int MillisPerMinute = MillisPerSecond * 60;
        private const int MillisPerHour = MillisPerMinute * 60;
        private const int MillisPerDay = MillisPerHour * 24;

        // Number of days in a non-leap year 
        private const int DaysPerYear = 365;
        // Number of days in 4 years
        private const int DaysPer4Years = DaysPerYear * 4 + 1;       // 1461 
        // Number of days in 100 years
        private const int DaysPer100Years = DaysPer4Years * 25 - 1;  // 36524
        // Number of days in 400 years
        private const int DaysPer400Years = DaysPer100Years * 4 + 1; // 146097 

        // Number of days from 1/1/0001 to 12/31/1600 
        private const int DaysTo1601 = DaysPer400Years * 4;          // 584388 
        // Number of days from 1/1/0001 to 12/30/1899
        private const int DaysTo1899 = DaysPer400Years * 4 + DaysPer100Years * 3 - 367;
        // Number of days from 1/1/0001 to 12/31/9999
        private const int DaysTo10000 = DaysPer400Years * 25 - 366;  // 3652059

        internal const long MinTicks = 0;
        internal const long MaxTicks = DaysTo10000 * TicksPerDay - 1;

        static readonly char[] typechar = {
                                 ' ',      
                                 'b',          /* b - boolean */
                                 'g',          /* GUID */
                                 ' ',          /* empty */
                                 'x',          /* byte */
                                 'h',          /* short */
                                 'i',          /* int */
                                 'j',          /* long */
                                 'e',          /* real */
                                 'w',          /* float */
                                 'c',          /* char */
                                 's',          /* symbol */
                                 'm',          /* month */
                                 'd',          /* date */
                                 'z',          /* datetime */
                                 'p',          /* timestamp */
                                 'u',          /* minute */
                                 'v',          /* second */
                                 't'           /* time */
                             };

        static readonly string[] typename = {
                                 "",      
                                 "bool",       /* b - boolean */
                                 "guid",       /* GUID */
                                 "",           /* empty */
                                 "byte",       /* byte */
                                 "short",      /* short */
                                 "int",        /* int */
                                 "long",       /* long */
                                 "real",       /* real */
                                 "float",      /* float */
                                 "char",       /* char */
                                 "symbol",     /* symbol */
                                 "month",      /* month */
                                 "date",       /* date */
                                 "datetime",   /* datetime */
                                 "timestamp",  /* timestamp */
                                 "minute",     /* minute */
                                 "second",     /* second */
                                 "time"        /* time */
                             };


        static readonly object[] posW = {
                                 null,      
                                 false,        /* b - boolean */
                                 null,         /* empty */
                                 null,         /* empty */
                                 (byte)0,      /* byte */
                                 wh,           /* short */
                                 wi,           /* int */
                                 wj,           /* long */
                                 we,           /* real */
                                 wf,           /* float */
                                 null,         /* char */
                                 null,         /* symbol */
                                 new Month(Int32.MaxValue),     /* month */
                                 new Date(Int32.MaxValue),      /* date */
                                 new DateTime(MaxTicks),        /* datetime */
                                 new DateTime(MaxTicks),        /* timestamp */
                                 new Minute(Int32.MaxValue),    /* minute */
                                 new Second(Int32.MaxValue),    /* second */
                                 new TimeSpan(MaxTicks)         /* time */
                             };
        static readonly object[] negW = {
                                 null,      
                                 false,        /* b - boolean */
                                 null,         /* empty */
                                 null,         /* empty */
                                 (byte)0,      /* byte */
                                 ah,           /* short */
                                 ai,           /* int */
                                 aj,           /* long */
                                 ae,           /* real */
                                 af,           /* float */
                                 null,         /* char */
                                 null,         /* symbol */
                                 new Month(Int32.MinValue +1),  /* month */
                                 new Date(Int32.MinValue +1),   /* date */
                                 new DateTime(MinTicks+1),      /* datetime */
                                 new DateTime(MinTicks+1),      /* timestamp */
                                 new Minute(Int32.MinValue +1), /* minute */
                                 new Second(Int32.MinValue +1), /* second */
                                 new TimeSpan(MinTicks+1)       /* time */

                             };

        public static object getw(char x)
        {
            return posW[Math.Abs(t(x))];
        }
        public static object getw(Type x)
        {
            return posW[Math.Abs(t(x))];
        }
        public static object geta(char x)
        {
            return negW[Math.Abs(t(x))];
        }
        public static object geta(Type x)
        {
            return negW[Math.Abs(t(x))];
        }

        public static bool qw(object x)
        {
            return x.Equals(posW[Math.Abs(t(x))]);
        }

        public static bool qa(object x)
        {
            return x.Equals(negW[Math.Abs(t(x))]);
        }

        /// <summary>
        /// type from type
        /// </summary>
        static int tt(Type x)
        {
            return x == typeof(bool) ? -1 : x == typeof(Guid) ? -2 : x == typeof(byte) ? -4 : x == typeof(short) ? -5 : x == typeof(int) ? -6 : x == typeof(long) ? -7 : x == typeof(float) ? -8 : x == typeof(double) ? -9 : x == typeof(char) ? -10 :
                x == typeof(string) ? -11 : /*x == typeof(DateTime) ? -12 :*/ x == typeof(Month) ? -13 : x == typeof(Date) ? -14 : x == typeof(DateTime) ? -15 : x == typeof(KTimespan) ? -16 : x == typeof(Minute) ? -17 : x == typeof(Second) ? -18 : x == typeof(TimeSpan) ? -19 :
                x == typeof(bool[]) ? 1 : x == typeof(Guid[]) ? 2 : x == typeof(byte[]) ? 4 : x == typeof(short[]) ? 5 : x == typeof(int[]) ? 6 : x == typeof(long[]) ? 7 : x == typeof(float[]) ? 8 : x == typeof(double[]) ? 9 : x == typeof(char[]) ? 10 :
                /* x == typeof(DateTime[]) ? 12 :*/ x == typeof(DateTime[]) ? 15 : x == typeof(KTimespan[]) ? 16 : x == typeof(TimeSpan[]) ? 19 : x == typeof(Flip) ? 98 : x == typeof(Dict) ? 99 : 0;
        }

        /// <summary>
        /// type character from type int
        /// </summary>
        public static char cti(int ti)
        {
            var c = typechar[Math.Abs(ti)];
            if (Math.Sign(ti) > 0)
                c = Char.ToUpper(c);
            return c;
        }

        /// <summary>
        /// Char array from type int
        /// </summary>
        public static char[] Cti(int ti)
        {
            var str = typename[Math.Abs(ti)];
            if (Math.Sign(ti) > 0)
                str = str.ToUpper();
            return str.ToCharArray();
        }

        /// <summary>
        /// char of type
        /// upper case denotes a vector as in Q
        /// </summary>
        public static char ct(object o)
        {
            int ti = t(o);
            return cti(ti);
        }


        /// <summary>
        /// type characer from .net Type 
        /// </summary>
        public static char ctt(Type x)
        {
            var ti = tt(x);
            return cti(ti);
        }

        /// <summary>
        /// type name from .net type
        /// </summary>
        public static char[] Cct(Type x)
        {
            var ti = tt(x);
            return Cti(ti);
        }

        public static bool canHaveSpecial(object x) { return Math.Abs(c.t(x)) < 20; }
        public static bool canHaveTypeChar(object x) { return Math.Abs(c.t(x)) < 20; }
        
        public static bool qspecial(object x)
        {
            if (canHaveSpecial(x))
            {
                if (qn(x)) return true;
                if (qa(x)) return true;
                if (qw(x)) return true;
                return false;
            }
            else
            {
                return false;
            }
        }

        public static bool qhasspecial<T>(T[] X)
        {
            return X.Any(x => qspecial(x));
        }

        //TODO: what if array has two dimensions?
        //TODO: need to change from T[] to U?[]

        /// <summary>
        /// Alternatively, you could just use NULL to test it
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="U"></typeparam>
        /// <param name="A"></param>
        /// <returns></returns>
        /// /
        /*
        public static U[] convNull<T,U>(T[] A) 
        {
            //int typ = t(A.GetType().GetElementType());
            Object nu = NU[Math.Abs(t(A))];
            U[] r = new U[A.GetLength(0)];
            for (int i = 0; i < A.GetLength(0); i++)
            {
                // BUG in C#?
                // https://connect.microsoft.com/VisualStudio/feedback/details/281842/c-compiler-does-not-allow-implicit-conversion-from-a-type-parameter-to-an-array-type-object-though-the-type-parameter-is-effectively-constrained-with-this-type
                r[i] = (A[i]==null) ? (U)nu : (U)A[i];
            }
            return r;
        }
        */

        // TODO: tidy up conversions

        public static Int32?[] ConvQ(Int32[] A)
        {
            Int32 c = A.Length;
            Int32?[] B = new Int32?[c];
            for (int i = 0; i < c; i++)
                B[i] = ConvQ(A[i]);
            return B;
        }

        public static Int32? ConvQ(Int32 i)
        {
            if (i == (int)NU[Math.Abs(t(i))])
                return null;
            else
                return i == ai ? Int32.MinValue : i;
        }

        public static Int16?[] ConvQ(Int16[] A)
        {
            Int32 c = A.Length;
            Int16?[] B = new Int16?[c];
            for (int i = 0; i < c; i++)
                B[i] = ConvQ(A[i]);
            return B;
        }

        public static Int16? ConvQ(Int16 h)
        {
            if (h == (Int16)NU[Math.Abs(t(h))])
                return null;
            else
                return h == ah ? Int16.MinValue : h;
        }

        public static Int64?[] ConvQ(Int64[] A)
        {
            Int32 c = A.Length;
            Int64?[] B = new Int64?[c];
            for (int i = 0; i < c; i++)
                B[i] = ConvQ(A[i]);
            return B;
        }

        public static Int64? ConvQ(Int64 j)
        {
            if (j == (Int64)NU[Math.Abs(t(j))])
                return null;
            else
                return j == aj ? Int64.MinValue : j;
        }

        static Single?[] ConvQ(Single[] A)
        {
            Int32 c = A.Length;
            Single?[] B = new Single?[c];
            for (int i = 0; i < c; i++)
                B[i] = ConvQ(A[i]);
            return B;
        }

        public static Single? ConvQ(Single e)
        {
            if (Single.IsNaN(e))
                return null;
            else
                return e;
        }

        public static Double?[] ConvQ(Double[] A)
        {
            Int32 c = A.Length;
            Double?[] B = new Double?[c];
            for (int i = 0; i < c; i++)
                B[i] = ConvQ(A[i]);
            return B;
        }

        public static Double? ConvQ(Double f)
        {
            if (Double.IsNaN(f))
                return null;
            else
                return f;
        }

        public static DateTime?[] ConvQ(DateTime[] A)
        {
            Int32 c = A.Length;
            DateTime?[] B = new DateTime?[c];
            for (int i = 0; i < c; i++)
                B[i] = ConvQ(A[i]);
            return B;
        }

        public static DateTime? ConvQ(DateTime z)
        {
            if (z == (DateTime)NU[Math.Abs(t(z))])
                return null;
            else
                return z;
        }

        public static Date ConvQ(Date d) { return d; }
        public static Month ConvQ(Month m) { return m; }
        public static Minute ConvQ(Minute u) { return u; }
        public static Second ConvQ(Second v) { return v; }
        public static KTimespan ConvQ(KTimespan n) { return n; }

        static Boolean?[] ConvQ(Boolean[] A)
        {
            Int32 c = A.Length;
            Boolean?[] B = new Boolean?[c];
            for (int i = 0; i < c; i++)
                B[i] = ConvQ(A[i]);
            return B;
        }

        public static Boolean? ConvQ(Boolean b)
        {
            if (b == (Boolean)NU[Math.Abs(t(b))])
                return null;
            else
                return b;
        }

        static Byte?[] ConvQ(Byte[] A)
        {
            Int32 c = A.Length;
            Byte?[] B = new Byte?[c];
            for (int i = 0; i < c; i++)
                B[i] = ConvQ(A[i]);
            return B;
        }

        public static Byte? ConvQ(Byte x)
        {
            if (x == (Byte)NU[Math.Abs(t(x))])
                return null;
            else
                return x;
        }

        static bool IsNullable(Type theType)
        {
            return (theType.IsGenericType && theType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)));
        }

        static bool IsNullable<T>(T obj)
        {
            if (obj == null) return true; // obvious 
            Type type = typeof(T);
            if (!type.IsValueType) return true; // ref-type 
            if (Nullable.GetUnderlyingType(type) != null) return true; // Nullable<T> 
            return false; // value-type 
        }

        /// <summary>
        /// once a type has been pushed into an object, this is the only way
        /// that I know of to discover if the type was orignally nullable
        /// 
        /// Not the fastest test in the world.  Should we have a nullable mode for c.cs?
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        static bool IsNullable2(object o)
        {
            Type objType = o.GetType();

            if (objType.FullName.StartsWith(typeof(Nullable<>).FullName))
            {
                if (objType.IsArray)
                {
                    return objType.GetElementType().IsValueType;
                }
                else
                {
                    return objType.IsValueType;
                }
            }

            return false;
        }

        //------------------------------------------------------------------------------------------------------------
        static Int32[] ConvToQiA(Object i_A)
        {
            Int32 nu = (int)NU[Math.Abs(t(typeof(int)))];
            Int32?[] A = (Int32?[])i_A;
            Int32 c = A.Length;
            Int32[] B = new Int32[c];
            for (int i = 0; i < c; i++)
                B[i] = ConvToQi(A[i]);
            return B;
        }

        public static Int32 ConvToQi(Object i)
        {
            if (i == null)
                return (Int32)NU[Math.Abs(t(i))];
            else
                return (Int32)i;
        }

        public static Int32 ConvToQi(Int32? i)
        {
            if (i == null)
                return (Int32)NU[Math.Abs(t(i))];
            else
                return (Int32)i;
        }

        static Int16[] ConvToQhA(Object i_A)
        {
            Int16 nu = (Int16)NU[Math.Abs(t(typeof(Int16)))];
            Int16?[] A = (Int16?[])i_A;
            Int32 c = A.Length;
            Int16[] B = new Int16[c];
            for (int i = 0; i < c; i++)
                B[i] = ConvToQh(A[i]);
            return B;
        }

        public static Int16 ConvToQh(Int16? h)
        {
            if (h == null)
                return (Int16)NU[Math.Abs(t(h))];
            else
                return (Int16)h;
        }
        public static Int16 ConvToQh(Object h)
        {
            if (h == null)
                return (Int16)NU[Math.Abs(t(h))];
            else
                return (Int16)h;
        }

        static Int64[] ConvToQjA(Object i_A)
        {
            Int64 nu = (Int64)NU[Math.Abs(t(typeof(Int64)))];
            Int64?[] A = (Int64?[])i_A;
            Int32 c = A.Length;
            Int64[] B = new Int64[c];
            for (int i = 0; i < c; i++)
                B[i] = ConvToQj(A[i]);
            return B;
        }

        public static Int64 ConvToQj(Int64? j)
        {
            if (j == null)
                return (Int64)NU[Math.Abs(t(j))];
            else
                return (Int64)j;
        }
        public static Int64 ConvToQj(Object j)
        {
            if (j == null)
                return (Int64)NU[Math.Abs(t(j))];
            else
                return (Int64)j;
        }

        static Single[] ConvToQeA(Object i_A)
        {
            Single nu = (Single)NU[Math.Abs(t(typeof(Single)))];
            Single?[] A = (Single?[])i_A;
            Int32 c = A.Length;
            Single[] B = new Single[c];
            for (int i = 0; i < c; i++)
                B[i] = ConvToQe(A[i]);
            return B;
        }

        public static Single ConvToQe(Single? e)
        {
            if (e == null)
                return (Single)NU[Math.Abs(t(e))];
            else
                return (Single)e;
        }
        public static Single ConvToQe(Object e)
        {
            if (e == null)
                return (Single)NU[Math.Abs(t(e))];
            else
                return (Single)e;
        }

        static Double[] ConvToQfA(Object i_A)
        {
            Double nu = (Double)NU[Math.Abs(t(typeof(Double)))];
            Single?[] A = (Single?[])i_A;
            Int32 c = A.Length;
            Double[] B = new Double[c];
            for (int i = 0; i < c; i++)
                B[i] = ConvToQf(A[i]);
            return B;
        }

        public static Double ConvToQf(Double? f)
        {
            if (f == null)
                return (Double)NU[Math.Abs(t(f))];
            else
                return (Double)f;
        }
        public static Double ConvToQf(Object f)
        {
            if (f == null)
                return (Double)NU[Math.Abs(t(f))];
            else
                return (Double)f;
        }

        static TimeSpan[] ConvToQtA(Object i_A)
        {
            TimeSpan nu = (TimeSpan)NU[Math.Abs(t(typeof(TimeSpan)))];
            TimeSpan?[] A = (TimeSpan?[])i_A;
            Int32 c = A.Length;
            TimeSpan[] B = new TimeSpan[c];
            for (int i = 0; i < c; i++)
                B[i] = ConvToQt(A[i]);
            return B;
        }

        public static TimeSpan ConvToQt(Object _t)
        {
            if (_t == null)
                return (TimeSpan)NU[Math.Abs(t(_t))];
            else
                return (TimeSpan)_t;
        }

        static DateTime[] ConvToQzA(Object i_A)
        {
            DateTime nu = (DateTime)NU[Math.Abs(t(typeof(DateTime)))];
            DateTime?[] A = (DateTime?[])i_A;
            Int32 c = A.Length;
            DateTime[] B = new DateTime[c];
            for (int i = 0; i < c; i++)
                B[i] = ConvToQz(A[i]);
            return B;
        }

        public static DateTime ConvToQz(DateTime? z)
        {
            if (z == null)
                return (DateTime)NU[Math.Abs(t(z))];
            else
                return (DateTime)z;
        }

        public static DateTime ConvToQz(Object z)
        {
            if (z == null)
                return (DateTime)NU[Math.Abs(t(z))];
            else
                return (DateTime)z;
        }

        static Boolean[] ConvToQbA(Object i_A)
        {
            Boolean nu = (Boolean)NU[Math.Abs(t(typeof(Boolean)))];
            Boolean?[] A = (Boolean?[])i_A;
            Int32 c = A.Length;
            Boolean[] B = new Boolean[c];
            for (int i = 0; i < c; i++)
                B[i] = ConvToQb(A[i]);
            return B;
        }

        public static Boolean ConvToQb(Boolean? b)
        {
            if (b == null)
                return (Boolean)NU[Math.Abs(t(b))];
            else
                return (Boolean)b;
        }
        public static Boolean ConvToQb(Object b)
        {
            if (b == null)
                return (Boolean)NU[Math.Abs(t(b))];
            else
                return (Boolean)b;
        }

        static Byte[] ConvToQxA(Object i_A)
        {
            Byte nu = (Byte)NU[Math.Abs(t(typeof(Byte)))];
            Boolean?[] A = (Boolean?[])i_A;
            Int32 c = A.Length;
            Byte[] B = new Byte[c];
            for (int i = 0; i < c; i++)
                B[i] = ConvToQx(A[i]);
            return B;
        }

        public static Byte ConvToQx(Byte? x)
        {
            if (x == null)
                return (Byte)NU[Math.Abs(t(x))];
            else
                return (Byte)x;
        }
        public static Byte ConvToQx(Object x)
        {
            if (x == null)
                return (Byte)NU[Math.Abs(t(x))];
            else
                return (Byte)x;
        }

        static Char[] ConvToQcA(Object i_A)
        {
            Char nu = (Char)NU[Math.Abs(t(typeof(Char)))];
            Boolean?[] A = (Boolean?[])i_A;
            Int32 c = A.Length;
            Char[] B = new Char[c];
            for (int i = 0; i < c; i++)
                B[i] = ConvToQc(A[i]);
            return B;
        }

        public static Char ConvToQc(Char? x)
        {
            if (x == null)
                return (Char)NU[Math.Abs(t(x))];
            else
                return (Char)x;
        }
        public static Char ConvToQc(Object x)
        {
            if (x == null)
                return (Char)NU[Math.Abs(t(x))];
            else
                return (Char)x;
        }
        //----------------------------------------------------------------------------------------

        /// <summary>
        /// Returns an Object with the specified Type and whose value is equivalent to the specified object.
        /// </summary>
        /// <param name="value">An Object that implements the IConvertible interface.</param>
        /// <param name="conversionType">The Type to which value is to be converted.</param>
        /// <returns>An object whose Type is conversionType (or conversionType's underlying type if conversionType
        /// is Nullable&lt;&gt;) and whose value is equivalent to value. -or- a null reference, if value is a null
        /// reference and conversionType is not a value type.</returns>
        /// <remarks>
        /// This method exists as a workaround to System.Convert.ChangeType(Object, Type) which does not handle
        /// nullables as of version 2.0 (2.0.50727.42) of the .NET Framework. The idea is that this method will
        /// be deleted once Convert.ChangeType is updated in a future version of the .NET Framework to handle
        /// nullable types, so we want this to behave as closely to Convert.ChangeType as possible.
        /// This method was written by Peter Johnson at:
        /// http://aspalliance.com/author.aspx?uId=1026.
        /// </remarks>
        public static object ChangeType(object value, Type conversionType)
        {
            // Note: This if block was taken from Convert.ChangeType as is, and is needed here since we're
            // checking properties on conversionType below.
            if (conversionType == null)
            {
                throw new ArgumentNullException("conversionType");
            } // end if

            // If it's not a nullable type, just pass through the parameters to Convert.ChangeType

            if (conversionType.IsGenericType &&
              conversionType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
            {
                // It's a nullable type, so instead of calling Convert.ChangeType directly which would throw a
                // InvalidCastException (per http://weblogs.asp.net/pjohnson/archive/2006/02/07/437631.aspx),
                // determine what the underlying type is
                // If it's null, it won't convert to the underlying type, but that's fine since nulls don't really
                // have a type--so just return null
                // Note: We only do this check if we're converting to a nullable type, since doing it outside
                // would diverge from Convert.ChangeType's behavior, which throws an InvalidCastException if
                // value is null and conversionType is a value type.
                if (value == null)
                {
                    return null;
                } // end if

                // It's a nullable type, and not null, so that means it can be converted to its underlying type,
                // so overwrite the passed-in conversion type with this underlying type
                var nullableConverter = new System.ComponentModel.NullableConverter(conversionType);
                conversionType = nullableConverter.UnderlyingType;
            } // end if

            // Now that we've guaranteed conversionType is something Convert.ChangeType can handle (i.e. not a
            // nullable type), pass the call on to Convert.ChangeType
            return Convert.ChangeType(value, conversionType);
        }


        // if IsNullable
        //  ConvertToNullable

        //TODO: as we scroll through the types we check the Type.IsNullable

        /// <summary>
        /// takes the encoded Kx number and makes a C# null
        /// 
        /// While C# code that accepts Kx norms is faster, this conversion simply allows old C# code to operate as is.
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        static Object ConvQ(Object o)
        {
            return
                  o is bool ? o
                : o is byte ? ConvQ((Byte)o)
                : o is short ? ConvQ((short)o)
                : o is int ? ConvQ((int)o)
                : o is long ? ConvQ((long)o)
                : o is float ? ConvQ((float)o)
                : o is double ? ConvQ((double)o)
                : o is char ? ConvQ((char)o)
                : o is string ? ConvQ((string)o)
                : o is DateTime ? ConvQ((DateTime)o)
                : o is Month ? ConvQ((Month)o)
                : o is Date ? ConvQ((Date)o)
                : o is DateTime ? ConvQ((DateTime)o)
                : o is KTimespan ? ConvQ((KTimespan)o)
                : o is Minute ? ConvQ((Minute)o)
                : o is Second ? ConvQ((Second)o)
                : o is TimeSpan ? ConvQ((TimeSpan)o)
                : o is bool[] ? ConvQ((bool[])o)
                : o is byte[] ? ConvQ((byte[])o)
                : o is short[] ? ConvQ((short[])o)
                : o is int[] ? ConvQ((int[])o)
                : o is long[] ? ConvQ((long[])o)
                : o is float[] ? ConvQ((float[])o)
                : o is double[] ? ConvQ((double[])o)
                : o is char[] ? ConvQ((char[])o)
                : o is DateTime[] ? ConvQ((DateTime[])o)
                : o is KTimespan[] ? ConvQ((KTimespan[])o)
                : o is TimeSpan[] ? ConvQ((TimeSpan[])o)
                : o is Flip ? ConvQ((Flip)o)
                : o is Dict ? ConvQ((Dict)o)

                : 0
                ;
        }

        void w(DateTime? p) { qn(p); w(p == null ? nj : p == za ? ja : p == zw ? jw : (100 * (((DateTime)p).Ticks - o))); }
        DateTime? rpn() { DateTime? p = rp(); return p == nz ? (DateTime?)null : p; }
    }

    /*
    // TODO: implement to string for minute, second, date, and timespan
    public partial class Dict
    {
        public override string ToString()
        {
            bool usebracket = false;
            string lparen = "";
            string rparen = "";
            if (x is Array)
            {
                var len = ((Array)x).Length;
                usebracket = len == 1;
            }
            if (usebracket)
            {
                lparen = "(";
                rparen = ")";
            }
            return lparen + x.ToString() + rparen + "!" + y.ToString();
        }        
    }
     */

    /// <summary>
    /// maximum resolution in C# is 100 nanoseconds
    /// </summary>
    public class KNanoDateTime
    {
        Int64 j;
        System.DateTime ndt = new DateTime(1970, 1, 1, 0, 0, 0, 0);
        public KNanoDateTime(Int64 _j)
        {
            this.j = _j;

            // mod seconds
            // check that Round doesn't get in the way 
            int s = (int)_j / 1000000;
            int n = (int)_j % 1000000;

            ndt = ndt.AddSeconds(s);
            ndt = ndt.AddTicks(n);
        }
    }

    /// <summary>
    /// Make table structures easier to navigate
    /// </summary>
    public class Tbl
    {
        public Flip x;
        public Flip y;

        public Tbl(Flip i_x, Flip i_y) {
            x = i_x;
            y = i_y;
        }

        public Tbl(Dict d) : this((Flip)d.x,(Flip)d.y)
        {            
        }

        public int xRowCount{get { return ((Array)x.y[0]).Length;}}

        public int xColCount{get{return x.y.Length; }}

        public int yRowCount{get{return ((Array)y.y[0]).Length;}}

        public int yColCount{get{return y.y.Length;}}

        public object at(string s) {
            var i = c.find(x.x, s);
            if(i<x.x.Length) return x.y[i];
            i = c.find(y.x, s);
            if (i < y.x.Length) return y.y[i];
            throw new ArgumentException(String.Format("{0} does not exist in Tbl",s));
        } 
        public object at<T>(string s) { return (T)at(s); }
    }
}


