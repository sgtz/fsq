using PRX.SYS.IO.kx;

namespace PRX.SYS.IO.KDB.Test
{
    public class SpecialValues
    {
        c c;
        public SpecialValues()
        {
            c = new c("localhost", 5001);
        }

        public void Test()
        {
            var Vz = c.k("0Wz -0Wz 0Nz");
            var Vp = c.k("0Wp -0Wp 0Np");
            //var Vp = c.k("0Wp -0Wp 0Np");
        }
    }
}