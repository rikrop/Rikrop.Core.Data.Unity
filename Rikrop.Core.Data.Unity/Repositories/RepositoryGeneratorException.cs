using System;
using System.Runtime.Serialization;

namespace Rikrop.Core.Data.Unity.Repositories
{
    [Serializable]
    public class RepositoryGeneratorException : Exception
    {
        public RepositoryGeneratorException()
        {
        }

        public RepositoryGeneratorException(string message)
            : base(message)
        {
        }

        public RepositoryGeneratorException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected RepositoryGeneratorException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
