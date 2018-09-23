using System;

namespace StaffDirectory
{
    public class MultipleCEOsException : Exception
    {
        public MultipleCEOsException(string message) : base(message)
        {
        }
    }
}