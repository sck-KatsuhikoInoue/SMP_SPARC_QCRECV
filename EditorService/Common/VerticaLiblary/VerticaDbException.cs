using System;
using System.Runtime.Serialization;

namespace EditorService
{
    [Serializable]
    internal class VerticaDbException : Exception
    {
        public VerticaDbException()
        {
        }

        public VerticaDbException(string message) : base(message)
        {
        }

        public VerticaDbException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected VerticaDbException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}