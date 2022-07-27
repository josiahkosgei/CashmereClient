
// Type: Cashmere.Library.Standard.Security.InvalidHashException


using System;

namespace Cashmere.Library.Standard.Security
{
    internal class InvalidHashException : Exception
    {
        public InvalidHashException()
        {
        }

        public InvalidHashException(string message)
          : base(message)
        {
        }

        public InvalidHashException(string message, Exception inner)
          : base(message, inner)
        {
        }
    }
}
