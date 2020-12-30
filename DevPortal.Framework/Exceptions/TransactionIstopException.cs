using System;

namespace DevPortal.Framework
{
    [Serializable]
    public class TransactionIstopException : Exception
    {
        public TransactionIstopException()
        {
        }

        public TransactionIstopException(string message) : base(message)
        {
        }

        public TransactionIstopException(string message, Exception inner) : base(message, inner)
        {
        }

        protected TransactionIstopException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}