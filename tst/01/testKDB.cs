using System;
using CLX.SYS.IO.q;

namespace PRX.SYS.IO.KDB.Test
{
    public class testKDB
    {
        c c;

        public testKDB()
        {
        }

        static void Main()
        {
            var t = new testKDB();
            t.k_basic();
            t.k_dict();
            t.k_enum();
            t.k_params();
        }

        public void k_basic()
        {
            /*
             * start up a server from the command prompt
             * cd c:\q\
             * q -p 5001
             */

            //test code for null in string/byte array function "ns"
            //string s = "aaa";
            //int i = 1;
            //i = s.IndexOf('\0');
            //Console.WriteLine((int)s.Length);

            var test = String.Format("{0:00}", 44);
            test = String.Format("{0:00}", 1);

            c = new c("localhost", 5001);                   // create object

            c.ks("a:44");

            object o = c.k("a");                            // return data

            c.ks("A:(1 2 3; 4 5 6; 7 8 9);A");              // create array

            o = c.k("A");                                   // return array

            object B = c.k("B:(1 2 3; 4 5 6; 7 8 9);B");    // create array

            //object 

            //c.k("a");
            //c.k(".u.sub[`trade;`MSFT.O`IBM.N]");
            //while(true)
            //{
            //    object r=c.k();
            //    O(n(at(r,2)));
            //} 

            // connect

            // send command

            // retrieve data

            // send to console
        }

        public void k_dict()
        {
            //string test = String.Format("{0:00}", 44);
            //test = String.Format("{0:00}", 1);

            c = new c("localhost", 5001);                   // create object

            object o;
            //c.Dict dict = new c.Dict(;
            c.ks("a:1 2 3;b:11 12 13;c:21 22 23;");
            c.ks("A:`a`b`c!(enlist a;enlist b;enlist c)");              // create dictionary
            c.ks("B:(enlist `d)!enlist (10 20 30)");                 // create dictionary
            c.ks("C:A,B");                                  // join dictionaries
            o = c.k("C");                                   // return dictionary
            //new object o2 = 
            //dict = c.k("C");
        }

        public void k_enum()
        {
            object o;
            c = new c("localhost", 5001);
            c.ks("u:`c`b`a");                        // a list of enums
            c.ks("v:`c`b`a`c`c`b`a`b`a`a`a`c");      // a list of enums
            c.ks("ev:`u$v");                         // cast enum v to enum u
            o = c.k("ev");

            c.ks("sde:`lng`sht");
            o = c.k("sde");
        }

        public void k_params()
        {
            c = new c("localhost", 5001);
            c.ks("testvarfunc:{testvar::x}");
            c.ks("testvarfunc", 4);
            
            object testvar = c.k("testvar");

            c.ks("{testvar2::x}", 4);
            object testvar2 = c.k("testvar2");

            // @TODO: how do we get some exceptions to bubble up by default?

            // c.ks("testvar3:", 4);  this will fail.

            if(((int)testvar) != 4)
                throw new Exception("couldn't assign a variable");
        }
    }
}
