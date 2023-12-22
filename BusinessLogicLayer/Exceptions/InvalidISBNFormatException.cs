using System.Runtime.Serialization;

namespace BusinessLogicLayer.Exceptions
{
    public class InvalidISBNFormatException : Exception
    {
        public InvalidISBNFormatException()
        {
        }

        public InvalidISBNFormatException(string? message) : base(message)
        {
        }

        public InvalidISBNFormatException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected InvalidISBNFormatException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}