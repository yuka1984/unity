using System;
using System.Runtime.Serialization;

namespace Microsoft.Practices.Unity.TestSupport
{
    [DataContract]
    public class AssertFailedException : Exception
    {
        public AssertFailedException()
        {
        }

        public AssertFailedException(string message) : base(message)
        {
        }

        public AssertFailedException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}