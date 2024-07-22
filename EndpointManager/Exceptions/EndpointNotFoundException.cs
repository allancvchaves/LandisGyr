using System;

namespace EndpointManager.Exceptions
{
    public class EndpointNotFoundException : Exception
    {
        public EndpointNotFoundException(string message) : base(message) { }
    }
}
