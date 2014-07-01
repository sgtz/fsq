using System;

namespace CLX.SYS.IO.q
{
    public class KDBException : Exception
    {
        private const string _msg = "KDB Exception";

        public KDBException() : base() { }
        public KDBException(string message) : base(message) { }
        public KDBException(string message, Exception innerException) : base(_msg, innerException) { }
        public KDBException(Exception innerException) : base(_msg, innerException) { }
    }
}