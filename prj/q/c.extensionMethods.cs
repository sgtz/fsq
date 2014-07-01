namespace CLX.SYS.IO
{
    using System;

    //csc c.cs  given >q trade.q -p 5001
    namespace q
    {
        public static class cExtensionMethods
        {
            public static bool isw(this int x) { return x == c.wi; }
            public static bool isa(this int x) { return x == c.ai; }
            public static bool isnu(this int x) { return x == c.ni; }

            public static bool isw(this short x) { return x == c.wh; }
            public static bool isa(this short x) { return x == c.ah; }
            public static bool isnu(this short x) { return x == c.nh; }

            public static bool isw(this long x) { return x == c.wj; }
            public static bool isa(this long x) { return x == c.aj; }
            public static bool isnu(this long x) { return x == c.nj; }

            public static bool isw(this double x) { return Double.IsPositiveInfinity(x); }
            public static bool isa(this double x) { return Double.IsNegativeInfinity(x); }
            public static bool isnu(this double x) { return Double.IsNaN(x); }

            public static bool isw(this Single x) { return Single.IsPositiveInfinity(x); }
            public static bool isa(this Single x) { return Single.IsNegativeInfinity(x); }
            public static bool isnu(this Single x) { return Single.IsNaN(x); }

            public static bool isw(this DateTime x) { return x == c.wz; }
            public static bool isa(this DateTime x) { return x == c.az; }
            public static bool isnu(this DateTime x) { return x == c.nz; }

            public static bool isw(this TimeSpan x) { return x == c.wt; }
            public static bool isa(this TimeSpan x) { return x == c._at; }
            public static bool isnu(this TimeSpan x) { return x == c.nut; }

            public static bool isw(this KTimespan x) { return x.Equals(c.wkt); }
            public static bool isa(this KTimespan x) { return x.Equals(c.akt); }
            public static bool isnu(this KTimespan x) { return x.Equals(c.nukt); }

            public static bool isw(this Month x) { return x.i == c.wi; }
            public static bool isa(this Month x) { return x.i == c.ai; }
            public static bool isnu(this Month x) { return x.i == c.ni; }

            public static bool isw(this Minute x) { return x.i == c.wi; }
            public static bool isa(this Minute x) { return x.i == c.ai; }
            public static bool isnu(this Minute x) { return x.i == c.ni; }

            public static bool isw(this Second x) { return x.i == c.wi; }
            public static bool isa(this Second x) { return x.i == c.ai; }
            public static bool isnu(this Second x) { return x.i == c.ni; }
            
            public static bool isw(this Date x) { return x.i == c.wi; }
            public static bool isa(this Date x) { return x.i == c.ai; }
            public static bool isnu(this Date x) { return x.i == c.ni; }

            // TODO: should 0Nc and 0Ng be further integrated into c.cs
            public static bool isnu(this char x) { return x == ' '; }
            public static bool isnu(this Guid x) { return x == Guid.Empty; }
        }
    }
}