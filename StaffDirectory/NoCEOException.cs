using System;

namespace StaffDirectory
{
    public class NoCEOException : Exception
    {
        public NoCEOException(string message): base(message) 
        {
        }
    }
}