using System;

namespace EndpointManager.Exceptions
{
    public class DuplicateSerialNumberException : Exception
    {
        public DuplicateSerialNumberException(string message) : base(message) { }
    }
}
