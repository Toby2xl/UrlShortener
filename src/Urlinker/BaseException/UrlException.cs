using System;
using System.Runtime.Serialization;

namespace Urlinker.BaseException
{
    public class UrlException : Exception
    {
        public UrlException()
        {
        }

        public UrlException(string message) : base(message)
        {
        }

        public UrlException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected UrlException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}